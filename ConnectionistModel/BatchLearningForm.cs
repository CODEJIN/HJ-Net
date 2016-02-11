using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace ConnectionistModel
{
    public partial class BatchLearningForm : Form
    {
        List<BatchData> batchDataList = new List<BatchData>();

        Simulator simulator = new Simulator(SimulatorAccessor.random);
        List<LearningSetup> learningSetupList = new List<LearningSetup>();

        Thread processThread;
        bool isPause = true;

        int currentBathDataIndex = 0;
        int currentLearningSetupIndex = 0;
        int currentEpoch = 0;
        int totalEpoch = 0;

        public BatchLearningForm()
        {
            InitializeComponent();
            this.FormClosed += BatchLearningForm_FormClosed;

            this.batchDataList = SimulatorAccessor.batchDataList;

            BatchListBoxRefresh();
        }        
        
        private void testDisplayButton_Click(object sender, EventArgs e)
        {
            TestDisplayRefresh();
        }
            
        private void startButton_Click(object sender, EventArgs e)
        {   
            saveLayerActivationCheckBox.Enabled = false;
            saveWeightCheckBox.Enabled = false;

            isPause = false;
            startButton.Enabled = false;         
            pauseButton.Enabled = true;
            exitButton.Enabled = false;

            
            processThread = new Thread(new ThreadStart(ProcessThread));
            processThread.Start();
            
        }
        private void pauseButton_Click(object sender, EventArgs e)
        {
            isPause = true;            
            pauseButton.Enabled = false;            
        }        
        private void exitButton_Click(object sender, EventArgs e)
        {            
            this.Close();            
        }
        
        private void BatchLearningForm_FormClosed(object sender, FormClosedEventArgs e)
        {            
            if(!isPause)processThread.Abort();
            this.Owner.Visible = true;
        }
        
        private void ProcessThread()
        {
            for (int batchIndex = currentBathDataIndex; batchIndex < batchDataList.Count; batchIndex++)
            {
                currentBathDataIndex = batchIndex;

                simulator = batchDataList[batchIndex].Simulator;
                learningSetupList = batchDataList[batchIndex].LearningSetupList;

                simulator.UseActivationInformation = saveLayerActivationCheckBox.Checked;
                simulator.UseWeightInformation = saveWeightCheckBox.Checked;

                this.Invoke(new MethodInvoker(delegate
                {
                    BatchListBoxRefresh();
                    TestDisplayRefresh();
                }));

                for (int learningSetupIndex = currentLearningSetupIndex; learningSetupIndex < learningSetupList.Count; learningSetupIndex++)
                {
                    totalEpoch = 0;
                    for (int i = 0; i < learningSetupIndex; i++) totalEpoch += learningSetupList[i].TrainingEpoch;

                    currentLearningSetupIndex = learningSetupIndex;
                    List<MatchingInformation> currentTrainingMatchingInformationList = learningSetupList[learningSetupIndex].TrainingMatchingInformationList;
                    List<MatchingInformation> currentTestMatchingInformationList = learningSetupList[learningSetupIndex].TestMatchingInformationList;

                    List<TrialInformation> testMatrixTrialInformationList = new List<TrialInformation>();
                    foreach (MatchingInformation testMatchingInformation in currentTestMatchingInformationList)
                    {
                        List<StimuliMatrix> stimuliMatrixList = simulator.StimuliPackDictionary[testMatchingInformation.StimuliPackName].GetMatrixStimuliData(false, false, simulator.StimuliPackDictionary[testMatchingInformation.StimuliPackName].Count);
                        foreach(StimuliMatrix stimuliMatrix in stimuliMatrixList)
                        {
                            TrialInformation newTestTrialInformation = new TrialInformation();
                            newTestTrialInformation.Process = simulator.ProcessDictionary[testMatchingInformation.ProcessName];
                            newTestTrialInformation.PatternSetup = testMatchingInformation.PatternSetup;
                            newTestTrialInformation.StimuliMatrix = stimuliMatrix;

                            testMatrixTrialInformationList.Add(newTestTrialInformation);
                        }                        
                    }

                    if (currentEpoch == 0)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            currentEpochTextBox.Text = "0";
                            pauseButton.Text = "Testing...";
                            pauseButton.Enabled = false;
                            maxYAxisTestDisplayTextBox.Enabled = false;
                            useTimeStampCheckBox.Enabled = false;
                            testDisplayButton.Enabled = false;
                        }));
                        if (simulator.UseWeightInformation) simulator.WeightInformationReader(totalEpoch + 0);
                        for (int testIndex = 0; testIndex < testMatrixTrialInformationList.Count; testIndex++)
                        {
                            simulator.Process(testMatrixTrialInformationList[testIndex], totalEpoch + 0);
                            this.Invoke(new MethodInvoker(delegate
                            {
                                pauseButton.Text = "Testing...";
                            }));

                        }
                        this.Invoke(new MethodInvoker(delegate
                        {
                            pauseButton.Text = "Pause";
                            pauseButton.Enabled = true;
                            maxYAxisTestDisplayTextBox.Enabled = true;
                            useTimeStampCheckBox.Enabled = true;
                            testDisplayButton.Enabled = true;
                        }));
                    }

                    for (int epochIndex = currentEpoch; epochIndex < learningSetupList[learningSetupIndex].TrainingEpoch; epochIndex++)
                    {
                        currentEpoch = epochIndex;
                        this.Invoke(new MethodInvoker(delegate
                        {
                            currentEpochTextBox.Text = (epochIndex + 1).ToString();
                        }));

                        List<TrialInformation> trainingMatrixTrialInformationList = new List<TrialInformation>();
                        foreach (MatchingInformation trainingMatchingInformation in currentTrainingMatchingInformationList)
                        {
                            List<StimuliMatrix> stimuliMatrixList = new List<StimuliMatrix>();
                            switch (learningSetupList[learningSetupIndex].TrainingProcessMode)
                            {
                                case ProcessMode.RandomAll:
                                case ProcessMode.RandomInStimuliPack:
                                    stimuliMatrixList = simulator.StimuliPackDictionary[trainingMatchingInformation.StimuliPackName].GetMatrixStimuliData(true, true, learningSetupList[learningSetupIndex].MatrixCalculationSize);
                                    break;
                                case ProcessMode.SequentialAll:
                                case ProcessMode.SequentialinStimuliPack:
                                    stimuliMatrixList = simulator.StimuliPackDictionary[trainingMatchingInformation.StimuliPackName].GetMatrixStimuliData(true, false, learningSetupList[learningSetupIndex].MatrixCalculationSize);
                                    break;
                            }

                            foreach (StimuliMatrix stimuliMatrix in stimuliMatrixList)
                            {
                                TrialInformation newTrainingTrialInformation = new TrialInformation();
                                newTrainingTrialInformation.Process = simulator.ProcessDictionary[trainingMatchingInformation.ProcessName];
                                newTrainingTrialInformation.PatternSetup = trainingMatchingInformation.PatternSetup;
                                newTrainingTrialInformation.StimuliMatrix = stimuliMatrix;

                                trainingMatrixTrialInformationList.Add(newTrainingTrialInformation);
                            }
                        }

                        switch (learningSetupList[learningSetupIndex].TrainingProcessMode)
                        {
                            case ProcessMode.RandomAll:
                            case ProcessMode.SequentialinStimuliPack:
                                trainingMatrixTrialInformationList = trainingMatrixTrialInformationList.OrderBy(x => (SimulatorAccessor.random.Next())).ToList();
                                break;
                        }

                        for (int trainingIndex = 0; trainingIndex < trainingMatrixTrialInformationList.Count; trainingIndex++)
                        {
                            if (trainingMatrixTrialInformationList[trainingIndex].StimuliMatrix.StimuliCount > 0)
                                simulator.Process(trainingMatrixTrialInformationList[trainingIndex], totalEpoch + epochIndex + 1);
                            this.Invoke(new MethodInvoker(delegate
                            {
                                currentProcessTextBox.Text = trainingMatrixTrialInformationList[trainingIndex].Process.Name;
                                currentStimuliPackTextBox.Text = trainingMatrixTrialInformationList[trainingIndex].StimuliMatrix.PackName;
                                currentStimulusTextBox.Text = trainingIndex.ToString() + "/" + trainingMatrixTrialInformationList.Count.ToString();
                                if (isPause) pauseButton.Text = "Training...";
                            }));
                        }

                        this.Invoke(new MethodInvoker(delegate
                        {
                            pauseButton.Text = "Pause";
                        }));

                        if ((epochIndex + 1) % learningSetupList[learningSetupIndex].TestTiming == 0)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                pauseButton.Text = "Testing...";
                                pauseButton.Enabled = false;
                                maxYAxisTestDisplayTextBox.Enabled = false;
                                useTimeStampCheckBox.Enabled = false;
                                testDisplayButton.Enabled = false;
                            }));
                            if (simulator.UseWeightInformation) simulator.WeightInformationReader(totalEpoch + epochIndex + 1);
                            for (int testIndex = 0; testIndex < testMatrixTrialInformationList.Count; testIndex++)
                            {
                                simulator.Process(testMatrixTrialInformationList[testIndex], totalEpoch + epochIndex + 1);
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    pauseButton.Text = "Testing...";
                                }));

                            }
                            this.Invoke(new MethodInvoker(delegate
                            {
                                pauseButton.Text = "Pause";
                                pauseButton.Enabled = true;
                                maxYAxisTestDisplayTextBox.Enabled = true;
                                useTimeStampCheckBox.Enabled = true;
                                testDisplayButton.Enabled = true;
                                TestDisplayRefresh();
                            }));
                        }

                        if (isPause)
                        {
                            currentEpoch = epochIndex + 1;
                            break;
                        }
                    }

                    if (isPause)
                    {
                        if (currentEpoch >= learningSetupList[learningSetupIndex].TrainingEpoch)
                        {
                            currentEpoch = 0;
                            currentLearningSetupIndex = learningSetupIndex + 1;
                        }
                        this.Invoke(new MethodInvoker(delegate
                        {
                            startButton.Enabled = true;
                            exitButton.Enabled = true;
                        }));

                        break;
                    }
                }
                if (isPause)
                {
                    break;
                }
                else
                {
                    string resultPath = simulator.OutputResult();
                    simulator.WeightSave(resultPath + "Status.WDATA");
                    currentLearningSetupIndex = 0;
                    currentEpoch = 0;
                    totalEpoch = 0;
                }
            }
            if (!isPause) this.Invoke(new MethodInvoker(delegate
            {
                startButton.Enabled = false;
                pauseButton.Enabled = false;
                exitButton.Enabled = true;

                MessageBox.Show("Training End");

                currentBathDataIndex++;
                BatchListBoxRefresh();

            }));
        }

        public void TestDisplayRefresh()
        {
            if (testDisplayModeComboBox.SelectedIndex < 0)
            {

            }
            else if (!RegularExpression.DoubleCheck(minYAxisTestDisplayTextBox.Text))
            {
                MessageBox.Show("Min Y Axis value is worng. Check Please.");

                minYAxisTestDisplayTextBox.Text = "0.0";
                minYAxisTestDisplayTextBox.Focus();
                minYAxisTestDisplayTextBox.SelectAll();
            }
            else if (!RegularExpression.DoubleCheck(maxYAxisTestDisplayTextBox.Text))
            {
                MessageBox.Show("Max Y Axis value is worng. Check Please.");

                maxYAxisTestDisplayTextBox.Text = "1.0";
                maxYAxisTestDisplayTextBox.Focus();
                maxYAxisTestDisplayTextBox.SelectAll();
            }
            else if (double.Parse(minYAxisTestDisplayTextBox.Text) < 0)
            {
                MessageBox.Show("Min Y Axis value must be 0 or bigger. Check Please.");

                minYAxisTestDisplayTextBox.Text = "0.0";
                maxYAxisTestDisplayTextBox.Focus();
                maxYAxisTestDisplayTextBox.SelectAll();
            }
            else if (double.Parse(maxYAxisTestDisplayTextBox.Text) <= 0)
            {
                MessageBox.Show("Max Y Axis value must be bigger than 0. Check Please.");

                maxYAxisTestDisplayTextBox.Text = "1.0";
                maxYAxisTestDisplayTextBox.Focus();
                maxYAxisTestDisplayTextBox.SelectAll();
            }
            else if (double.Parse(maxYAxisTestDisplayTextBox.Text) <= double.Parse(minYAxisTestDisplayTextBox.Text))
            {
                MessageBox.Show("Max Y Axis value must be bigger than min Y Axis value. Check Please.");

                minYAxisTestDisplayTextBox.Text = "0.0";
                maxYAxisTestDisplayTextBox.Text = "1.0";
                maxYAxisTestDisplayTextBox.Focus();
                maxYAxisTestDisplayTextBox.SelectAll();
            }
            else if (!useTimeStampCheckBox.Checked)
            {
                int maxXAxis = 0;
                foreach (LearningSetup learningSetup in learningSetupList) maxXAxis += learningSetup.TrainingEpoch;

                testDisplayChart.ChartAreas[0].Area3DStyle.Enable3D = false;

                testDisplayChart.ChartAreas[0].AxisX.Minimum = 0;
                testDisplayChart.ChartAreas[0].AxisX.Maximum = maxXAxis;
                testDisplayChart.ChartAreas[0].AxisY.Minimum = double.Parse(minYAxisTestDisplayTextBox.Text);
                testDisplayChart.ChartAreas[0].AxisY.Maximum = double.Parse(maxYAxisTestDisplayTextBox.Text);

                List<DisplayData> displayDataList = new List<DisplayData>();

                try
                {
                    switch (testDisplayModeComboBox.SelectedIndex)
                    {
                        case 0:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch)
                                    {
                                        displayData.InsertValue(testData.MeanSquredError);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.InsertValue(testData.MeanSquredError);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 1:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch)
                                    {
                                        displayData.InsertValue(testData.MeanSemanticStress);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.InsertValue(testData.MeanSemanticStress);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 2:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch)
                                    {
                                        displayData.InsertValue(testData.MeanCrossEntropy);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.InsertValue(testData.MeanCrossEntropy);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 3:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch)
                                    {
                                        if (testData.Correctness) displayData.InsertValue(1);
                                        else displayData.InsertValue(0);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    if (testData.Correctness) newDisplayData.InsertValue(1);
                                    else newDisplayData.InsertValue(0);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 4:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch)
                                    {
                                        displayData.InsertValue(testData.MeanActiveUnitActivation);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.InsertValue(testData.MeanActiveUnitActivation);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 5:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch)
                                    {
                                        displayData.InsertValue(testData.MeanInactiveUnitActivation);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.InsertValue(testData.MeanInactiveUnitActivation);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                    }
                    testDisplayChart.Series.Clear();
                    testDisplayChart.Legends.Clear();

                    Series dataSeries = new Series();
                    dataSeries.ChartType = SeriesChartType.Line;
                    testDisplayChart.Series.Add(dataSeries);

                    foreach (DisplayData displayData in displayDataList)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.SetValueXY(displayData.Epoch, displayData.Value);
                        dataSeries.Points.Add(dataPoint);
                    }
                }
                catch
                {

                }                
            }
            else if (useTimeStampCheckBox.Checked)
            {
                int maxXAxis = 0;
                foreach (TestData testData in simulator.TestDataList) if (testData.TimeStamp > maxXAxis) maxXAxis = testData.TimeStamp;

                testDisplayChart.ChartAreas[0].Area3DStyle.Enable3D = true;
                testDisplayChart.ChartAreas[0].Area3DStyle.PointDepth = 50;

                testDisplayChart.ChartAreas[0].AxisX.Minimum = 0;
                testDisplayChart.ChartAreas[0].AxisX.Maximum = maxXAxis;
                testDisplayChart.ChartAreas[0].AxisY.Minimum = double.Parse(minYAxisTestDisplayTextBox.Text);
                testDisplayChart.ChartAreas[0].AxisY.Maximum = double.Parse(maxYAxisTestDisplayTextBox.Text);

                List<DisplayData> displayDataList = new List<DisplayData>();

                try
                {
                    switch (testDisplayModeComboBox.SelectedIndex)
                    {
                        case 0:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch && testData.TimeStamp == displayData.TimeStamp)
                                    {
                                        displayData.InsertValue(testData.MeanSquredError);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.TimeStamp = testData.TimeStamp;
                                    newDisplayData.InsertValue(testData.MeanSquredError);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 1:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch && testData.TimeStamp == displayData.TimeStamp)
                                    {
                                        displayData.InsertValue(testData.MeanSemanticStress);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.TimeStamp = testData.TimeStamp;
                                    newDisplayData.InsertValue(testData.MeanSemanticStress);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 2:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch && testData.TimeStamp == displayData.TimeStamp)
                                    {
                                        displayData.InsertValue(testData.MeanCrossEntropy);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.TimeStamp = testData.TimeStamp;
                                    newDisplayData.InsertValue(testData.MeanCrossEntropy);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 3:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch && testData.TimeStamp == displayData.TimeStamp)
                                    {
                                        if (testData.Correctness) displayData.InsertValue(1);
                                        else displayData.InsertValue(0);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.TimeStamp = testData.TimeStamp;
                                    if (testData.Correctness) newDisplayData.InsertValue(1);
                                    else newDisplayData.InsertValue(0);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 4:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch && testData.TimeStamp == displayData.TimeStamp)
                                    {
                                        displayData.InsertValue(testData.MeanActiveUnitActivation);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.TimeStamp = testData.TimeStamp;
                                    newDisplayData.InsertValue(testData.MeanActiveUnitActivation);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                        case 5:
                            foreach (TestData testData in simulator.TestDataList)
                            {
                                bool isExist = false;
                                foreach (DisplayData displayData in displayDataList)
                                {
                                    if (testData.Epoch == displayData.Epoch && testData.TimeStamp == displayData.TimeStamp)
                                    {
                                        displayData.InsertValue(testData.MeanInactiveUnitActivation);
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    DisplayData newDisplayData = new DisplayData();
                                    newDisplayData.Epoch = testData.Epoch;
                                    newDisplayData.TimeStamp = testData.TimeStamp;
                                    newDisplayData.InsertValue(testData.MeanInactiveUnitActivation);
                                    displayDataList.Add(newDisplayData);
                                }
                            }
                            break;
                    }
                    testDisplayChart.Series.Clear();
                    testDisplayChart.Legends.Clear();
                    testDisplayChart.Legends.Add(new Legend());
                    testDisplayChart.Legends[0].Name = "";

                    Dictionary<int, Series> dataSeriesDictionary = new Dictionary<int, Series>();
                    
                    foreach (DisplayData displayData in displayDataList)
                    {
                        if(!dataSeriesDictionary.ContainsKey(displayData.Epoch))
                        {
                            Series newSeries = new Series();
                            newSeries.ChartType = SeriesChartType.Line;
                            newSeries.Name = "Epoch" + displayData.Epoch.ToString();
                            testDisplayChart.Series.Add(newSeries);
                            dataSeriesDictionary[displayData.Epoch] = newSeries;

                            LegendItem newLegendItem = new LegendItem();                            
                            newLegendItem.ImageStyle = LegendImageStyle.Line;
                            //newLegendItem.Name = "Epoch" + displayData.Epoch.ToString();                            
                            newLegendItem.SeriesName = displayData.Epoch.ToString();
                            testDisplayChart.Legends[0].CustomItems.Add(newLegendItem);
                        }
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.SetValueXY(displayData.TimeStamp, displayData.Value);
                        dataSeriesDictionary[displayData.Epoch].Points.Add(dataPoint);
                    }                    
                }
                catch
                {

                }
            }
        }

        public void BatchListBoxRefresh()
        {
            BatchDataListBox.Items.Clear();

            for(int index=0;index<batchDataList.Count;index++)
            {
                if (currentBathDataIndex > index) BatchDataListBox.Items.Add(batchDataList[index].Name + " -> " + "Finished");
                else if (currentBathDataIndex == index) BatchDataListBox.Items.Add(batchDataList[index].Name + " -> " + "Ongoing");
                else if (currentBathDataIndex < index) BatchDataListBox.Items.Add(batchDataList[index].Name + " -> " + "Standby");
            }
            
        }        
    }
}
