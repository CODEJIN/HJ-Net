using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConnectionistModel
{
    public partial class LearningForm : Form
    {
        Simulator simulator;
        List<LearningSetup> learningSetupList = new List<LearningSetup>();

        List<MatchingInformation> newTrainingMatchingInformationList;
        List<MatchingInformation> newTestMatchingInformationList;
        Dictionary<int, string> newTrainingPatternSetup;
        Dictionary<int, string> newTestPatternSetup;

        bool isMaking = false;

        Thread processThread;
        bool isPause = true;

        int currentLearningSetupIndex = 0;
        int currentEpoch = 0;
        int totalEpoch = 0;

        public LearningForm()
        {
            InitializeComponent();
            this.FormClosed += ProcessForm_FormClosed;

            simulator = SimulatorAccessor.simulator;

            foreach (string key in simulator.StimuliPackDictionary.Keys)
            {
                trainingStimuliPackComboBox.Items.Add(key);
                testStimuliPackComboBox.Items.Add(key);
            }
            foreach (string key in simulator.ProcessDictionary.Keys)
            {
                trainingProcessComboBox.Items.Add(key);
                testProcessComboBox.Items.Add(key);
            }

        }

        private void learningSetupMakingButton_Click(object sender, EventArgs e)
        {
            isMaking = true;

            learningSetupMakingButton.Enabled = false;
            learningSetupInsertButton.Enabled = true;
            trainingEpochTextBox.Enabled = true;
            testTimingTextBox.Enabled = true;
            matrixCalculationSizeTextBox.Enabled = true;
            trainingModeComboBox.Enabled = true;
            learningSetupSaveButton.Enabled = false;
            learningSetupLoadButton.Enabled = false;

            newTrainingMatchingInformationList = new List<MatchingInformation>();
            newTestMatchingInformationList = new List<MatchingInformation>();
            newTrainingPatternSetup = new Dictionary<int, string>();
            newTestPatternSetup = new Dictionary<int, string>();

            trainingEpochTextBox.Text = "";
            testTimingTextBox.Text = "";
            matrixCalculationSizeTextBox.Text = "1";

            TrainingMatchInforamtionRefresh();
            TestMatchInforamtionRefresh();
        }
        private void learningSetupInsertButton_Click(object sender, EventArgs e)
        {            
            if(newTrainingMatchingInformationList.Count < 0) MessageBox.Show("Insert the matching of training process and stimluli pack.");            
            else if (!RegularExpression.UIntCheck(trainingEpochTextBox.Text))
            {
                MessageBox.Show("Inserted training epoch is worng. Check Please.");

                trainingEpochTextBox.Focus();
                trainingEpochTextBox.SelectAll();
            }
            else if (int.Parse(trainingEpochTextBox.Text) <= 0)
            {
                MessageBox.Show("Training epoch must be bigger than 0. Check Please.");

                trainingEpochTextBox.Focus();
                trainingEpochTextBox.SelectAll();
            }
            else if (newTestMatchingInformationList.Count < 0) MessageBox.Show("Insert the matching of test process and stimluli pack.");
            else if (!RegularExpression.UIntCheck(testTimingTextBox.Text))
            {
                MessageBox.Show("Inserted test timing is worng. Check Please.");

                testTimingTextBox.Focus();
                testTimingTextBox.SelectAll();
            }
            else if (int.Parse(testTimingTextBox.Text) <= 0)
            {
                MessageBox.Show("Test timing must be bigger than 0. Check Please.");

                testTimingTextBox.Focus();
                testTimingTextBox.SelectAll();
            }
            else if (!RegularExpression.UIntCheck(matrixCalculationSizeTextBox.Text))
            {
                MessageBox.Show("Inserted matrix calculation size is wrong. Check Please");
            }
            else if (int.Parse(matrixCalculationSizeTextBox.Text) == 0)
            {
                MessageBox.Show("Matrix calculation size must be bigger than 0.");
            }
            else if (trainingModeComboBox.SelectedIndex < 0) MessageBox.Show("Select the training mode.");
            else
            {
                if (int.Parse(matrixCalculationSizeTextBox.Text) > 1)
                {
                    bool isSRN = false;
                    foreach (MatchingInformation matchingInformation in newTrainingMatchingInformationList)
                    {
                        foreach (Order order in simulator.ProcessDictionary[matchingInformation.ProcessName])
                        {
                            if (order.Code == OrderCode.SRNTraining || order.Code == OrderCode.SRNTest) isSRN = true;
                        }
                    }
                    foreach (MatchingInformation matchingInformation in newTestMatchingInformationList)
                    {
                        foreach (Order order in simulator.ProcessDictionary[matchingInformation.ProcessName])
                        {
                            if (order.Code == OrderCode.SRNTraining || order.Code == OrderCode.SRNTest) isSRN = true;
                        }
                    }

                    if (isSRN)
                    {
                        if (MessageBox.Show("It is limited to use matrix calcuation in SRN.\nThe time-stamps of all stimuli will become the first stimulus's.\nDo you use the matrix calculation?", "Caution", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            matrixCalculationSizeTextBox.Text = "1";
                        }
                    }
                }

                LearningSetup newProcessSetup = new LearningSetup();
                newProcessSetup.TrainingMatchingInformationList = newTrainingMatchingInformationList;
                newProcessSetup.TrainingEpoch = int.Parse(trainingEpochTextBox.Text);
                newProcessSetup.MatrixCalculationSize = int.Parse(matrixCalculationSizeTextBox.Text);
                newProcessSetup.TestMatchingInformationList = newTestMatchingInformationList;                
                newProcessSetup.TestTiming = int.Parse(testTimingTextBox.Text);
                switch(trainingModeComboBox.SelectedIndex)
                {
                    case 0:
                        newProcessSetup.TrainingProcessMode = ProcessMode.RandomAll;
                        break;
                    case 1:
                        newProcessSetup.TrainingProcessMode = ProcessMode.RandomInStimuliPack;
                        break;
                    case 2:
                        newProcessSetup.TrainingProcessMode = ProcessMode.SequentialAll;
                        break;
                    case 3:
                        newProcessSetup.TrainingProcessMode = ProcessMode.SequentialinStimuliPack;
                        break;
                }
                
                learningSetupList.Add(newProcessSetup);

                isMaking = false;

                learningSetupMakingButton.Enabled = true;
                learningSetupInsertButton.Enabled = false;
                trainingEpochTextBox.Enabled = false;
                testTimingTextBox.Enabled = false;
                matrixCalculationSizeTextBox.Enabled = false;
                trainingModeComboBox.Enabled = false;
                learningSetupSaveButton.Enabled = true;
                learningSetupLoadButton.Enabled = true;

                ProcessSetupRefresh();
            }            
        }        
        private void learningSetupDeleteButton_Click(object sender, EventArgs e)
        {
            if (learningSetupListBox.SelectedIndex >= 0)
            {
                learningSetupList.RemoveAt(learningSetupListBox.SelectedIndex);

                ProcessSetupRefresh();
            }
        }
        private void learningSetupUpButton_Click(object sender, EventArgs e)
        {
            if (learningSetupListBox.SelectedIndex > 0)
            {
                LearningSetup tempUseProcess = learningSetupList[learningSetupListBox.SelectedIndex];
                learningSetupList[learningSetupListBox.SelectedIndex] = learningSetupList[learningSetupListBox.SelectedIndex - 1];
                learningSetupList[learningSetupListBox.SelectedIndex - 1] = tempUseProcess;

                ProcessSetupRefresh();
            }
        }
        private void learningSetupDownButton_Click(object sender, EventArgs e)
        {
            if (learningSetupListBox.SelectedIndex + 1 < learningSetupListBox.Items.Count)
            {
                LearningSetup tempUseProcess = learningSetupList[learningSetupListBox.SelectedIndex];
                learningSetupList[learningSetupListBox.SelectedIndex] = learningSetupList[learningSetupListBox.SelectedIndex + 1];
                learningSetupList[learningSetupListBox.SelectedIndex + 1] = tempUseProcess;

                ProcessSetupRefresh();
            }
        }

        private void testDisplayButton_Click(object sender, EventArgs e)
        {
            TestDisplayRefresh();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (learningSetupList.Count < 1)
            {
                MessageBox.Show("There is no learning setup.");
            }
            else
            {
                simulator.UseActivationInformation = saveLayerActivationCheckBox.Checked;
                simulator.UseWeightInformation = saveWeightCheckBox.Checked;
                saveLayerActivationCheckBox.Enabled = false;
                saveWeightCheckBox.Enabled = false;

                processSetupGroupBox.Enabled = false;

                isPause = false;
                startButton.Enabled = false;
                resultSaveButton.Enabled = false;
                statusSaveButton.Enabled = false;
                statusLoadButton.Enabled = false;
                pauseButton.Enabled = true;
                exitButton.Enabled = false;

                processThread = new Thread(new ThreadStart(ProcessThread));
                processThread.Start();
            }
        }
        private void pauseButton_Click(object sender, EventArgs e)
        {
            isPause = true;            
            pauseButton.Enabled = false;            
        }
        private void resultSaveButton_Click(object sender, EventArgs e)
        {
            bool startStatus = startButton.Enabled;
            startButton.Enabled = false;
            startButton.Text = "Saving...";
            simulator.OutputResult();
            startButton.Enabled = startStatus;
            startButton.Text = "Start";
        }
        private void statusSaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Wegiht Data File(*.WDATA)|*.WDATA";
            saveFileDialog.InitialDirectory = Application.StartupPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                simulator.WeightSave(saveFileDialog.FileName);
            }
        }
        private void statusLoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Weight Data File(*.WDATA)|*.WDATA";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Current Weight will be deleted.", "Caution", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) simulator.WeightLoad(openFileDialog.FileName);
            }
        }
        private void exitButton_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
        
        private void ProcessForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            if(!isPause)processThread.Abort();
            this.Owner.Visible = true;
        }
                
        private void ProcessThread()
        {
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
                    bool isSRN = false;
                    foreach (Order order in simulator.ProcessDictionary[testMatchingInformation.ProcessName])
                    {
                        if (order.Code == OrderCode.SRNTraining || order.Code == OrderCode.SRNTest) isSRN = true;
                    }

                    List<StimuliMatrix> stimuliMatrixList;
                    if (isSRN && learningSetupList[learningSetupIndex].MatrixCalculationSize == 1) stimuliMatrixList = simulator.StimuliPackDictionary[testMatchingInformation.StimuliPackName].GetMatrixStimuliData(true, true, 1);
                    else stimuliMatrixList = simulator.StimuliPackDictionary[testMatchingInformation.StimuliPackName].GetMatrixStimuliData(false, false, simulator.StimuliPackDictionary[testMatchingInformation.StimuliPackName].Count);
                    foreach (StimuliMatrix stimuliMatrix in stimuliMatrixList)
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
                        currentEpochTextBox.Text = "0" + learningSetupList[learningSetupIndex].TrainingEpoch.ToString();
                        pauseButton.Text = "Testing...";
                        pauseButton.Enabled = false;
                        minYAxisTestDisplayTextBox.Enabled = false;
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
                        minYAxisTestDisplayTextBox.Enabled = true;
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
                        currentEpochTextBox.Text = (epochIndex + 1).ToString() + "/" + learningSetupList[learningSetupIndex].TrainingEpoch.ToString();
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
                            minYAxisTestDisplayTextBox.Enabled = false;
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
                            minYAxisTestDisplayTextBox.Enabled = true;
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
                        resultSaveButton.Enabled = true;
                        statusSaveButton.Enabled = true;
                        exitButton.Enabled = true;
                    }));

                    break;
                }
            }

            if (!isPause) this.Invoke(new MethodInvoker(delegate
            {
                startButton.Enabled = false;
                pauseButton.Enabled = false;
                resultSaveButton.Enabled = true;
                statusSaveButton.Enabled = true;
                exitButton.Enabled = true;

                simulator.OutputResult();

                MessageBox.Show("Training End");

            }));
        }

        public void ProcessSetupRefresh()
        {               
            trainingEpochTextBox.Text = "";
            testTimingTextBox.Text = "";
            matrixCalculationSizeTextBox.Text = "1";
            trainingModeComboBox.SelectedIndex = -1;
            
            learningSetupListBox.Items.Clear();
            foreach (LearningSetup useProcess in learningSetupList)
                learningSetupListBox.Items.Add("Training Info.(" + useProcess.TrainingMatchingInformationList.Count + ")/" + "Test Info.(" + useProcess.TestMatchingInformationList.Count + ")");

            TrainingMatchInforamtionRefresh();
            TestMatchInforamtionRefresh();
        }
        public void TrainingMatchInforamtionRefresh()
        {
            trainingStimuliPackComboBox.Enabled = isMaking;
            trainingProcessComboBox.Enabled = isMaking;
            trainingPatternSetupListBox.Enabled = isMaking;
            trainingPatternDeleteButton.Enabled = isMaking;
            trainingPatternSetupComboBox.Enabled = isMaking;
            trainingOrderSetupComboBox.Enabled = isMaking;
            trainingPatternSetupButton.Enabled = isMaking;
            trainingMatchingUpButton.Enabled = isMaking;
            trainingMatchingDownButton.Enabled = isMaking;
            trainingMatchingDeleteButton.Enabled = isMaking;
            trainingMatchingAddButton.Enabled = isMaking;

            List<MatchingInformation> selectedMatchingInformationList = new List<MatchingInformation>();
            if (isMaking) selectedMatchingInformationList = newTrainingMatchingInformationList;
            else if (learningSetupListBox.SelectedIndex >= 0)
                selectedMatchingInformationList = learningSetupList[learningSetupListBox.SelectedIndex].TrainingMatchingInformationList;
            
            trainingStimuliPackComboBox.SelectedIndex = -1;
            trainingProcessComboBox.SelectedIndex = -1;

            trainingPatternSetupComboBox.SelectedIndex = -1;
            trainingOrderSetupComboBox.SelectedIndex = -1;

            trainingMatchingInformationRichTextBox.Text = "";

            trainingMatchingListBox.Items.Clear();

            foreach (MatchingInformation matchingInformation in selectedMatchingInformationList)
                trainingMatchingListBox.Items.Add(matchingInformation.StimuliPackName + "->" + matchingInformation.ProcessName);
        }
        public void TestMatchInforamtionRefresh()
        {
            testStimuliPackComboBox.Enabled = isMaking;
            testProcessComboBox.Enabled = isMaking;
            testPatternSetupListBox.Enabled = isMaking;
            testPatternDeleteButton.Enabled = isMaking;
            testPatternSetupComboBox.Enabled = isMaking;
            testOrderSetupComboBox.Enabled = isMaking;
            testPatternSetupButton.Enabled = isMaking;            
            testMatchingDeleteButton.Enabled = isMaking;
            testMatchingAddButton.Enabled = isMaking;

            List<MatchingInformation> selectedMatchingInformationList = new List<MatchingInformation>();
            if (isMaking) selectedMatchingInformationList = newTestMatchingInformationList;
            else if (learningSetupListBox.SelectedIndex >= 0)
                selectedMatchingInformationList = learningSetupList[learningSetupListBox.SelectedIndex].TestMatchingInformationList;            

            testStimuliPackComboBox.SelectedIndex = -1;
            testProcessComboBox.SelectedIndex = -1;

            testPatternSetupComboBox.SelectedIndex = -1;
            testOrderSetupComboBox.SelectedIndex = -1;

            testMatchingInformationRichTextBox.Text = "";

            testMatchingListBox.Items.Clear();

            foreach (MatchingInformation matchingInformation in selectedMatchingInformationList)
                testMatchingListBox.Items.Add(matchingInformation.StimuliPackName + "->" + matchingInformation.ProcessName);
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
                foreach (LearningSetup processSetup in learningSetupList) maxXAxis += processSetup.TrainingEpoch;

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
                                        if (testData.MeanActiveUnitActivation == testData.MeanActiveUnitActivation)
                                        {
                                            displayData.InsertValue(testData.MeanActiveUnitActivation);
                                            isExist = true;
                                        }
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    if (testData.MeanActiveUnitActivation == testData.MeanActiveUnitActivation)
                                    {
                                        DisplayData newDisplayData = new DisplayData();
                                        newDisplayData.Epoch = testData.Epoch;
                                        newDisplayData.InsertValue(testData.MeanActiveUnitActivation);
                                        displayDataList.Add(newDisplayData);
                                    }
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
                                        if (testData.MeanInactiveUnitActivation == testData.MeanInactiveUnitActivation)
                                        {
                                            displayData.InsertValue(testData.MeanInactiveUnitActivation);
                                            isExist = true;
                                        }
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    if (testData.MeanInactiveUnitActivation == testData.MeanInactiveUnitActivation)
                                    {
                                        DisplayData newDisplayData = new DisplayData();
                                        newDisplayData.Epoch = testData.Epoch;
                                        newDisplayData.InsertValue(testData.MeanInactiveUnitActivation);
                                        displayDataList.Add(newDisplayData);
                                    }
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
                                        if (testData.MeanInactiveUnitActivation == testData.MeanInactiveUnitActivation)
                                        {
                                            displayData.InsertValue(testData.MeanActiveUnitActivation);
                                            isExist = true;
                                        }
                                        break;
                                    }
                                }
                                if (!isExist)
                                {
                                    if (testData.MeanInactiveUnitActivation == testData.MeanInactiveUnitActivation)
                                    {
                                        DisplayData newDisplayData = new DisplayData();
                                        newDisplayData.Epoch = testData.Epoch;
                                        newDisplayData.TimeStamp = testData.TimeStamp;
                                        newDisplayData.InsertValue(testData.MeanActiveUnitActivation);
                                        displayDataList.Add(newDisplayData);
                                    }
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
                        if (!dataSeriesDictionary.ContainsKey(displayData.Epoch))
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

        private void trainingStimuliPackComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            newTrainingPatternSetup.Clear();            

            TrainingPatternSetupRefresh();
        }
        private void trainingProcessComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            newTrainingPatternSetup.Clear();
            

            TrainingPatternSetupRefresh();
        }
        private void testStimuliPackComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            newTestPatternSetup.Clear();
            

            TestPatternSetupRefresh();
        }
        private void testProcessComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            newTestPatternSetup.Clear();
            
            TestPatternSetupRefresh();
        }

        private void trainingPatternSetupButton_Click(object sender, EventArgs e)
        {
            if (trainingPatternSetupComboBox.SelectedIndex > -1 && trainingOrderSetupComboBox.SelectedIndex > -1)
            {
                int orderIndex = int.Parse(((string)trainingOrderSetupComboBox.SelectedItem).Substring(0, ((string)trainingOrderSetupComboBox.SelectedItem).IndexOf(':')));
                Order selectedOrder = simulator.ProcessDictionary[(string)trainingProcessComboBox.SelectedItem][orderIndex];

                if (simulator.LayerList[selectedOrder.Layer1Name].UnitCount == simulator.StimuliPackDictionary[(string)trainingStimuliPackComboBox.SelectedItem].RepresentationSize((string)trainingPatternSetupComboBox.SelectedItem)) newTrainingPatternSetup[orderIndex] = (string)trainingPatternSetupComboBox.SelectedItem;
                else MessageBox.Show("Pattern and Order's Layer aren't matching.");

                TrainingPatternSetupRefresh();
            }
        }
        private void testPatternSetupButton_Click(object sender, EventArgs e)
        {
            if (testPatternSetupComboBox.SelectedIndex > -1 && testOrderSetupComboBox.SelectedIndex > -1)
            {
                int orderIndex = int.Parse(((string)testOrderSetupComboBox.SelectedItem).Substring(0, ((string)testOrderSetupComboBox.SelectedItem).IndexOf(':')));
                Order selectedOrder = simulator.ProcessDictionary[(string)testProcessComboBox.SelectedItem][orderIndex];

                if (simulator.LayerList[selectedOrder.Layer1Name].UnitCount == simulator.StimuliPackDictionary[(string)testStimuliPackComboBox.SelectedItem].RepresentationSize((string)testPatternSetupComboBox.SelectedItem)) newTestPatternSetup[orderIndex] = (string)testPatternSetupComboBox.SelectedItem;
                else MessageBox.Show("Pattern and Order's Layer aren't matching.");

                TestPatternSetupRefresh();
            }
        }

        private void trainingPatternDeleteButton_Click(object sender, EventArgs e)
        {
            if(trainingPatternSetupListBox.SelectedIndex > -1)
            {
                string listBoxText = ((string)trainingPatternSetupListBox.Items[trainingPatternSetupListBox.SelectedIndex]);
                int key = int.Parse(listBoxText.Substring(listBoxText.IndexOf("->") + 2, listBoxText.IndexOf(":") - (listBoxText.IndexOf("->") + 2)));
                newTrainingPatternSetup.Remove(key);

                TrainingPatternSetupRefresh();
            }
        }
        private void testPatternDeleteButton_Click(object sender, EventArgs e)
        {
            if (testPatternSetupListBox.SelectedIndex > -1)
            {
                string listBoxText = ((string)testPatternSetupListBox.Items[testPatternSetupListBox.SelectedIndex]);
                int key = int.Parse(listBoxText.Substring(listBoxText.IndexOf("->") + 2, listBoxText.IndexOf(":") - (listBoxText.IndexOf("->") + 2)));
                newTestPatternSetup.Remove(key);

                TestPatternSetupRefresh();
            }
        }

        private void TrainingPatternSetupRefresh()
        {
            trainingPatternSetupComboBox.Items.Clear();
            trainingOrderSetupComboBox.Items.Clear();
            trainingPatternSetupListBox.Items.Clear();

            if (trainingStimuliPackComboBox.SelectedIndex != -1)
            {
                StimuliPack selectedStimuliPack = simulator.StimuliPackDictionary[(string)trainingStimuliPackComboBox.SelectedItem];
                foreach (string name in selectedStimuliPack.PatternNameList) trainingPatternSetupComboBox.Items.Add(name);
            }

            if (trainingProcessComboBox.SelectedIndex != -1)
            {
                Process selectedProcess = simulator.ProcessDictionary[(string)trainingProcessComboBox.SelectedItem];

                for (int i = 0; i < selectedProcess.Count; i++)
                {
                    bool assigned = false;
                    foreach (int key in newTrainingPatternSetup.Keys) if (key == i) assigned = true;
                    if (assigned) continue;

                    switch (selectedProcess[i].Code)
                    {
                        case OrderCode.ActivationInput: //Activation Input 
                            trainingOrderSetupComboBox.Items.Add(i.ToString() + ": " + selectedProcess[i].Layer1Name + "(Act. In)");
                            break;
                        case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                            trainingOrderSetupComboBox.Items.Add(i.ToString() + ": " + selectedProcess[i].Layer1Name + "(ER. Cal.)");
                            break;
                        case OrderCode.TestValueStore: //Test Value Store
                            trainingOrderSetupComboBox.Items.Add(i.ToString() + ": " + selectedProcess[i].Layer1Name + "(Test)");
                            break;
                    }
                }
            }
            foreach (int key in newTrainingPatternSetup.Keys)
            {
                Process selectedProcess = simulator.ProcessDictionary[(string)trainingProcessComboBox.SelectedItem];
                
                switch (selectedProcess[key].Code)
                {
                    case OrderCode.ActivationInput: //Activation Input 
                        trainingPatternSetupListBox.Items.Add(newTrainingPatternSetup[key] + "->" + key + ": " + selectedProcess[key].Layer1Name + "(Act. In)");
                        break;
                    case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                        trainingPatternSetupListBox.Items.Add(newTrainingPatternSetup[key] + "->" + key + ": " + selectedProcess[key].Layer1Name + "(ER. Cal.)");
                        break;
                    case OrderCode.TestValueStore: //Test Value Store
                        trainingPatternSetupListBox.Items.Add(newTrainingPatternSetup[key] + "->" + key + ": " + selectedProcess[key].Layer1Name + "(Test)");
                        break;
                }
            }

        }
        private void TestPatternSetupRefresh()
        {
            testPatternSetupComboBox.Items.Clear();
            testOrderSetupComboBox.Items.Clear();
            testPatternSetupListBox.Items.Clear();

            if (testStimuliPackComboBox.SelectedIndex != -1)
            {
                StimuliPack selectedStimuliPack = simulator.StimuliPackDictionary[(string)testStimuliPackComboBox.SelectedItem];
                foreach (string name in selectedStimuliPack.PatternNameList) testPatternSetupComboBox.Items.Add(name);
            }

            if (testProcessComboBox.SelectedIndex != -1)
            {
                Process selectedProcess = simulator.ProcessDictionary[(string)testProcessComboBox.SelectedItem];

                for (int i = 0; i < selectedProcess.Count; i++)
                {
                    bool assigned = false;
                    foreach (int key in newTestPatternSetup.Keys) if (key == i) assigned = true;
                    if (assigned) continue;

                    switch (selectedProcess[i].Code)
                    {
                        case OrderCode.ActivationInput: //Activation Input 
                            testOrderSetupComboBox.Items.Add(i.ToString() + ": " + selectedProcess[i].Layer1Name + "(Act. In)");
                            break;
                        case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                            testOrderSetupComboBox.Items.Add(i.ToString() + ": " + selectedProcess[i].Layer1Name + "(ER. Cal.)");
                            break;
                        case OrderCode.TestValueStore: //Test Value Store
                            testOrderSetupComboBox.Items.Add(i.ToString() + ": " + selectedProcess[i].Layer1Name + "(Test)");
                            break;
                    }
                }
            }
            foreach (int key in newTestPatternSetup.Keys)
            {
                Process selectedProcess = simulator.ProcessDictionary[(string)testProcessComboBox.SelectedItem];

                switch (selectedProcess[key].Code)
                {
                    case OrderCode.ActivationInput: //Activation Input 
                        testPatternSetupListBox.Items.Add(newTestPatternSetup[key] + "->" + key + ": " + selectedProcess[key].Layer1Name + "(Act. In)");
                        break;
                    case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                        testPatternSetupListBox.Items.Add(newTestPatternSetup[key] + "->" + key + ": " + selectedProcess[key].Layer1Name + "(ER. Cal.)");
                        break;
                    case OrderCode.TestValueStore: //Test Value Store
                        testPatternSetupListBox.Items.Add(newTestPatternSetup[key] + "->" + key + ": " + selectedProcess[key].Layer1Name + "(Test)");
                        break;
                }
            }
        }

        private void learningSetupSaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Learning Setup XML File(*.LSXML)|*.LSXML";
            saveFileDialog.InitialDirectory = Application.StartupPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StructureSave(saveFileDialog.FileName);
            }
        }
        private void learningSetupLoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Learning Setup XML File(*.LSXML)|*.LSXML";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Current setups will be deleted.", "Caution", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) StructureLoad(openFileDialog.FileName);
            }
        }

        private void StructureSave(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

            XmlNode rootNode = xmlDocument.CreateElement("", "Root", "");
            xmlDocument.AppendChild(rootNode);

            XmlNode processSetupListNode = xmlDocument.CreateElement("ProcessSetupList");
            
            foreach (LearningSetup processSetup in learningSetupList)
            {   
                XmlElement processSetupElement = xmlDocument.CreateElement("ProcessSetup");

                XmlElement trainingInformationElement = xmlDocument.CreateElement("TrainingInformation");                
                trainingInformationElement.SetAttribute("Epoch", processSetup.TrainingEpoch.ToString());
                trainingInformationElement.SetAttribute("MatrixCalculationSize", processSetup.MatrixCalculationSize.ToString());
                trainingInformationElement.SetAttribute("Mode", processSetup.TrainingProcessMode.ToString());
                foreach (MatchingInformation trainingMatchingInformation in processSetup.TrainingMatchingInformationList)
                {
                    XmlElement matchingInformation = xmlDocument.CreateElement("MatchingInformation");
                    matchingInformation.SetAttribute("StimuliPack", trainingMatchingInformation.StimuliPackName);
                    matchingInformation.SetAttribute("Process", trainingMatchingInformation.ProcessName);                    
                    foreach (int key in trainingMatchingInformation.PatternSetup.Keys)
                    {
                        XmlElement patternSetup = xmlDocument.CreateElement("PatternSetup");
                        patternSetup.SetAttribute("Order", key.ToString());
                        patternSetup.SetAttribute("Pattern", trainingMatchingInformation.PatternSetup[key]);
                        matchingInformation.AppendChild(patternSetup);
                    }
                    trainingInformationElement.AppendChild(matchingInformation);
                }
                processSetupElement.AppendChild(trainingInformationElement);
                
                XmlElement testInformationElement = xmlDocument.CreateElement("TestInformation");                
                testInformationElement.SetAttribute("Timing", processSetup.TestTiming.ToString());
                foreach (MatchingInformation testMatchingInformation in processSetup.TestMatchingInformationList)
                {
                    XmlElement matchingInformation = xmlDocument.CreateElement("MatchingInformation");
                    matchingInformation.SetAttribute("StimuliPack", testMatchingInformation.StimuliPackName);
                    matchingInformation.SetAttribute("Process", testMatchingInformation.ProcessName);
                    foreach (int key in testMatchingInformation.PatternSetup.Keys)
                    {
                        XmlElement patternSetup = xmlDocument.CreateElement("PatternSetup");
                        patternSetup.SetAttribute("Order", key.ToString());
                        patternSetup.SetAttribute("Pattern", testMatchingInformation.PatternSetup[key]);
                        matchingInformation.AppendChild(patternSetup);
                    }
                    testInformationElement.AppendChild(matchingInformation);
                }
                processSetupElement.AppendChild(testInformationElement);

                processSetupListNode.AppendChild(processSetupElement);
            }
            rootNode.AppendChild(processSetupListNode);

            xmlDocument.Save(fileName);
        }
        private void StructureLoad(string fileName)
        {
            learningSetupList.Clear();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            XmlNodeList processSetupNodeList = xmlDocument.SelectNodes("/Root/ProcessSetupList/ProcessSetup");
            foreach (XmlNode processSetupNode in processSetupNodeList)
            {
                LearningSetup loadProcessSetup = new LearningSetup();

                XmlNode trainingInformationNode = processSetupNode.SelectSingleNode("TrainingInformation");                
                loadProcessSetup.TrainingEpoch = int.Parse(trainingInformationNode.Attributes["Epoch"].Value);
                loadProcessSetup.MatrixCalculationSize = int.Parse(trainingInformationNode.Attributes["MatrixCalculationSize"].Value);
                switch (trainingInformationNode.Attributes["Mode"].Value)
                {
                    case "RandomAll":
                        loadProcessSetup.TrainingProcessMode = ProcessMode.RandomAll;
                        break;
                    case "RandomInStimuliPack":
                        loadProcessSetup.TrainingProcessMode = ProcessMode.RandomInStimuliPack;
                        break;
                    case "SequentialAll":
                        loadProcessSetup.TrainingProcessMode = ProcessMode.SequentialAll;
                        break;
                    case "SequentialinStimuliPack":
                        loadProcessSetup.TrainingProcessMode = ProcessMode.SequentialinStimuliPack;
                        break;
                }
                foreach (XmlNode matchingInformationNode in trainingInformationNode.SelectNodes("MatchingInformation"))
                {
                    MatchingInformation loadTrainingMatchingInformation = new MatchingInformation();
                    loadTrainingMatchingInformation.StimuliPackName = matchingInformationNode.Attributes["StimuliPack"].Value;
                    loadTrainingMatchingInformation.ProcessName = matchingInformationNode.Attributes["Process"].Value;
                    foreach (XmlNode patternSetupNode in matchingInformationNode.SelectNodes("PatternSetup"))
                    {
                        loadTrainingMatchingInformation.PatternSetup[int.Parse(patternSetupNode.Attributes["Order"].Value)] = patternSetupNode.Attributes["Pattern"].Value;
                    }
                    loadProcessSetup.TrainingMatchingInformationList.Add(loadTrainingMatchingInformation);
                }
                
                XmlNode testInformationNode = processSetupNode.SelectSingleNode("TestInformation");
                loadProcessSetup.TestTiming = int.Parse(testInformationNode.Attributes["Timing"].Value);                
                foreach (XmlNode matchingInformationNode in testInformationNode.SelectNodes("MatchingInformation"))
                {
                    MatchingInformation loadtestMatchingInformation = new MatchingInformation();
                    loadtestMatchingInformation.StimuliPackName = matchingInformationNode.Attributes["StimuliPack"].Value;
                    loadtestMatchingInformation.ProcessName = matchingInformationNode.Attributes["Process"].Value;
                    foreach (XmlNode patternSetupNode in matchingInformationNode.SelectNodes("PatternSetup"))
                    {
                        loadtestMatchingInformation.PatternSetup[int.Parse(patternSetupNode.Attributes["Order"].Value)] = patternSetupNode.Attributes["Pattern"].Value;
                    }
                    loadProcessSetup.TestMatchingInformationList.Add(loadtestMatchingInformation);
                }

                learningSetupList.Add(loadProcessSetup);                
            }

            ProcessSetupRefresh();
        }

        private void trainingMatchingAddButton_Click(object sender, EventArgs e)
        {
            StimuliPack trainingStimuliPack = simulator.StimuliPackDictionary[(string)trainingStimuliPackComboBox.SelectedItem];
            Process trainingProcess = simulator.ProcessDictionary[(string)trainingProcessComboBox.SelectedItem];

            if (trainingStimuliPackComboBox.SelectedIndex < 0) MessageBox.Show("Select Training Stimluli Pack.");
            else if (trainingProcessComboBox.SelectedIndex < 0) MessageBox.Show("Select Training Process.");
            else if (trainingOrderSetupComboBox.Items.Count > 0) MessageBox.Show("Training patterns are not set.");
            else
            {
                MatchingInformation newMatchingInformation = new MatchingInformation();
                newMatchingInformation.StimuliPackName = (string)trainingStimuliPackComboBox.SelectedItem;
                newMatchingInformation.ProcessName = (string)trainingProcessComboBox.SelectedItem;
                newMatchingInformation.PatternSetup = newTrainingPatternSetup;
                newTrainingMatchingInformationList.Add(newMatchingInformation);

                newTrainingPatternSetup = new Dictionary<int, string>();

                TrainingMatchInforamtionRefresh();
            }
        }
        private void testMatchingAddButton_Click(object sender, EventArgs e)
        {   
            if (testStimuliPackComboBox.SelectedIndex < 0) MessageBox.Show("Select Test Stimuli Pack.");
            else if (testProcessComboBox.SelectedIndex < 0) MessageBox.Show("Select Test Process.");            
            else
            {
                StimuliPack testStimuliPack = simulator.StimuliPackDictionary[(string)testStimuliPackComboBox.SelectedItem];
                Process testProcess = simulator.ProcessDictionary[(string)testProcessComboBox.SelectedItem];

                bool isExistTestOrder = false;
                foreach (Order order in testProcess)
                {
                    if (order.Code == OrderCode.TestValueStore || order.Code == OrderCode.SRNTest)
                    {
                        isExistTestOrder = true;
                        break;
                    }
                }

                if (!isExistTestOrder) MessageBox.Show("There is no test order in Test process. Check Please.");
                else if (testOrderSetupComboBox.Items.Count > 0) MessageBox.Show("Test patterns are not set.");
                else
                {
                    MatchingInformation newMatchingInformation = new MatchingInformation();
                    newMatchingInformation.StimuliPackName = (string)testStimuliPackComboBox.SelectedItem;
                    newMatchingInformation.ProcessName = (string)testProcessComboBox.SelectedItem;
                    newMatchingInformation.PatternSetup = newTestPatternSetup;

                    newTestMatchingInformationList.Add(newMatchingInformation);

                    newTestPatternSetup = new Dictionary<int, string>();

                    TestMatchInforamtionRefresh();
                }
            }
        }

        private void trainingMatchingDeleteButton_Click(object sender, EventArgs e)
        {
            if (trainingMatchingListBox.SelectedIndex >= 0)
            {
                newTrainingMatchingInformationList.RemoveAt(trainingMatchingListBox.SelectedIndex);

                ProcessSetupRefresh();
            }
        }
        private void testMatchingDeleteButton_Click(object sender, EventArgs e)
        {
            if (testMatchingListBox.SelectedIndex >= 0)
            {
                newTestMatchingInformationList.RemoveAt(testMatchingListBox.SelectedIndex);

                ProcessSetupRefresh();
            }
        }

        private void trainingMatchingUpButton_Click(object sender, EventArgs e)
        {
            if (trainingMatchingListBox.SelectedIndex > 0)
            {
                MatchingInformation tempMatchingInformation = newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex];
                newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex] = newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex - 1];
                newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex - 1] = tempMatchingInformation;

                ProcessSetupRefresh();
            }
        }
        private void trainingMatchingDownButton_Click(object sender, EventArgs e)
        {
            if (trainingMatchingListBox.SelectedIndex + 1 < trainingMatchingListBox.Items.Count)
            {
                MatchingInformation tempMatchingInformation = newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex];
                newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex] = newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex + 1];
                newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex + 1] = tempMatchingInformation;

                ProcessSetupRefresh();
            }
        }

        private void trainingMatchingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MatchingInformation selectedMatchingInformation = new MatchingInformation();
            if (isMaking) selectedMatchingInformation = newTrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex];
            else if (learningSetupListBox.SelectedIndex >= 0 && trainingMatchingListBox.SelectedIndex >= 0)
                selectedMatchingInformation = learningSetupList[learningSetupListBox.SelectedIndex].TrainingMatchingInformationList[trainingMatchingListBox.SelectedIndex];

            if (trainingMatchingListBox.SelectedIndex >= 0)
            {
                trainingMatchingInformationRichTextBox.Text = "";
                
                trainingMatchingInformationRichTextBox.Text += "Stimuli Pack Name : " + selectedMatchingInformation.StimuliPackName + "\n";
                trainingMatchingInformationRichTextBox.Text += "Process Name : " + selectedMatchingInformation.ProcessName + "\n";                
                trainingMatchingInformationRichTextBox.Text += "\nMatching(Pattern -> Order)\n";

                foreach (int key in selectedMatchingInformation.PatternSetup.Keys)
                {
                    Process matchedProcess = simulator.ProcessDictionary[selectedMatchingInformation.ProcessName];

                    switch (matchedProcess[key].Code)
                    {
                        case OrderCode.ActivationInput: //Activation Input 
                            trainingMatchingInformationRichTextBox.Text += selectedMatchingInformation.PatternSetup[key] + "->" + key + ": " + matchedProcess[key].Layer1Name + "(Act. In)\n";
                            break;
                        case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                            trainingMatchingInformationRichTextBox.Text += selectedMatchingInformation.PatternSetup[key] + "->" + key + ": " + matchedProcess[key].Layer1Name + "(ER. Cal.)\n";
                            break;
                        case OrderCode.TestValueStore: //Test Value Store
                            trainingMatchingInformationRichTextBox.Text += selectedMatchingInformation.PatternSetup[key] + "->" + key + ": " + matchedProcess[key].Layer1Name + "(Test)\n";
                            break;
                    }
                }
            }
        }
        private void testMatchingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MatchingInformation selectedMatchingInformation = new MatchingInformation();
            if (isMaking) selectedMatchingInformation = newTestMatchingInformationList[testMatchingListBox.SelectedIndex];
            else if (learningSetupListBox.SelectedIndex >= 0 && testMatchingListBox.SelectedIndex >= 0)
                selectedMatchingInformation = learningSetupList[learningSetupListBox.SelectedIndex].TestMatchingInformationList[testMatchingListBox.SelectedIndex];

            if (testMatchingListBox.SelectedIndex >= 0)
            {
                testMatchingInformationRichTextBox.Text = "";

                testMatchingInformationRichTextBox.Text += "Stimuli Pack Name : " + selectedMatchingInformation.StimuliPackName + "\n";
                testMatchingInformationRichTextBox.Text += "Process Name : " + selectedMatchingInformation.ProcessName + "\n";                
                testMatchingInformationRichTextBox.Text += "\nMatching(Pattern -> Order)\n";

                foreach (int key in selectedMatchingInformation.PatternSetup.Keys)
                {
                    Process matchedProcess = simulator.ProcessDictionary[selectedMatchingInformation.ProcessName];

                    switch (matchedProcess[key].Code)
                    {
                        case OrderCode.ActivationInput: //Activation Input 
                            testMatchingInformationRichTextBox.Text += selectedMatchingInformation.PatternSetup[key] + "->" + key + ": " + matchedProcess[key].Layer1Name + "(Act. In)\n";
                            break;
                        case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                            testMatchingInformationRichTextBox.Text += selectedMatchingInformation.PatternSetup[key] + "->" + key + ": " + matchedProcess[key].Layer1Name + "(ER. Cal.)\n";
                            break;
                        case OrderCode.TestValueStore: //Test Value Store
                            testMatchingInformationRichTextBox.Text += selectedMatchingInformation.PatternSetup[key] + "->" + key + ": " + matchedProcess[key].Layer1Name + "(Test)\n";
                            break;
                    }
                }
            }
        }

        private void learningSetupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LearningSetup selectedProcessSetup = new LearningSetup();
            if (!isMaking && learningSetupListBox.SelectedIndex >= 0)
            {
                selectedProcessSetup = learningSetupList[learningSetupListBox.SelectedIndex];

                trainingEpochTextBox.Text = selectedProcessSetup.TrainingEpoch.ToString();
                testTimingTextBox.Text = selectedProcessSetup.TestTiming.ToString();
                matrixCalculationSizeTextBox.Text = selectedProcessSetup.MatrixCalculationSize.ToString();
                switch(selectedProcessSetup.TrainingProcessMode)
                {
                    case ProcessMode.RandomAll:
                        trainingModeComboBox.SelectedIndex = 0;
                        break;
                    case ProcessMode.RandomInStimuliPack:
                        trainingModeComboBox.SelectedIndex = 1;
                        break;
                    case ProcessMode.SequentialAll:
                        trainingModeComboBox.SelectedIndex = 2;
                        break;
                    case ProcessMode.SequentialinStimuliPack:
                        trainingModeComboBox.SelectedIndex = 3;
                        break;
                }

                TrainingMatchInforamtionRefresh();
                TestMatchInforamtionRefresh();
            }
        }        
    }

    class DisplayData
    {
        double sumValue = 0;
        int insertCount = 0;

        public int Epoch
        {
            get;
            set;
        }
        public int TimeStamp
        {
            get;
            set;
        }
        public double Value
        {
            get
            {
                return sumValue / (double)insertCount;
            }
        }

        public void InsertValue(double insertValue)
        {
            sumValue += insertValue;
            insertCount++;
        }
    }
}
