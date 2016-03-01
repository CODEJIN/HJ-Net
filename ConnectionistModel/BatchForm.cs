using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ConnectionistModel
{
    public partial class BatchForm : Form
    {
        List<BatchData> batchDataList;
        List<Variable> variableList;

        Random random;
        string batchName;
        Dictionary<string, string> newStimuliPackFileDictionary;

        public BatchForm()
        {
            InitializeComponent();
            this.FormClosed += BatchForm_FormClosed;

            batchDataList = SimulatorAccessor.batchDataList;
            batchDataList.Clear();

            variableList = new List<Variable>();

            random = new Random();
            newStimuliPackFileDictionary = new Dictionary<string, string>();
        }

        private void architectureBrowserButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Architecture XML File(*.ACTXML)|*.ACTXML";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                architectureTextBox.Text = openFileDialog.FileName;
                batchName = openFileDialog.SafeFileName.Substring(0, openFileDialog.SafeFileName.IndexOf('.'));
            }
        }
        private void processBrowserButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Process Control File(*.PRCXML)|*.PRCXML";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                processTextBox.Text = openFileDialog.FileName;
            }
        }
        private void learningBrowserButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Learning Setup XML File(*.LSXML)|*.LSXML";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                learningTextBox.Text = openFileDialog.FileName;
            }
        }
        private void stimuliPackBrowserButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "TXT File (*.TXT)|*.TXT";
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                stimuliPackTextBox.Text = openFileDialog.FileName;
                stimuliPackNameTextBox.Text = openFileDialog.SafeFileName.Substring(0, openFileDialog.SafeFileName.IndexOf('.'));
            }
        }

        private List<LearningSetup> LearningSetup_Load(string fileName)
        {
            List<LearningSetup> learningSetupList = new List<LearningSetup>();

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

            return learningSetupList;
        }

        private void stimuliPackAddButton_Click(object sender, EventArgs e)
        {
            if (stimuliPackNameTextBox.Text.Trim() == "") MessageBox.Show("Stimuli Data pack Name is worng. Check Please.");            
            newStimuliPackFileDictionary[stimuliPackNameTextBox.Text] = stimuliPackTextBox.Text;
            StimuliPackList_Refresh();            
        }
        private void stimuliPackDeleteButton_Click(object sender, EventArgs e)
        {
            if (stimuliPackListBox.SelectedIndex >= 0)
            {   
                newStimuliPackFileDictionary.Remove((string)stimuliPackListBox.SelectedItem);
                StimuliPackList_Refresh();
            }
        }
        private void StimuliPackList_Refresh()
        {
            stimuliPackListBox.Items.Clear();
            stimuliPackTextBox.Text = "";
            stimuliPackNameTextBox.Text = "";

            foreach (string key in newStimuliPackFileDictionary.Keys) stimuliPackListBox.Items.Add(key);
        }

        private void batchAddButton_Click(object sender, EventArgs e)
        {
            if (architectureTextBox.Text.Trim() == "" || processTextBox.Text.Trim() == "" || learningTextBox.Text.Trim() == "" || newStimuliPackFileDictionary.Count < 1)
            {
                MessageBox.Show("Set the simulation files.");
            }
            else if (!RegularExpression.PositiveIntCheck(repeatTextBox.Text))
            {
                MessageBox.Show("Repeat has to be a positive integer.\nIf you do not want repeat, insert 1.");
            }
            else
            {
                List<BatchData> newBatchDataList = new List<BatchData>();
                for (int repeatIndex = 0; repeatIndex < int.Parse(repeatTextBox.Text); repeatIndex++)
                {
                    BatchData newBatchData = new BatchData();

                    newBatchData.Name = batchName;
                    newBatchData.ArchitectureFile = architectureTextBox.Text;
                    newBatchData.ProcessFile = processTextBox.Text;
                    newBatchData.LearningSetupFile = learningTextBox.Text;
                    newBatchData.StimuliPackFileDictionary = newStimuliPackFileDictionary;

                    Simulator newSimulator = new Simulator(random);
                    newSimulator.Architecture_Load(architectureTextBox.Text);
                    newSimulator.Process_Load(processTextBox.Text);
                    foreach (string key in newBatchData.StimuliPackFileDictionary.Keys) newSimulator.StimuliImport(key, newBatchData.StimuliPackFileDictionary[key]);
                    newBatchData.Simulator = newSimulator;
                    newBatchData.LearningSetupList = LearningSetup_Load(learningTextBox.Text);
                    newBatchDataList.Add(newBatchData);                    
                }
                VariableApply(newBatchDataList);
                
                newStimuliPackFileDictionary = new Dictionary<string, string>();
                architectureTextBox.Text = "";
                processTextBox.Text = "";
                learningTextBox.Text = "";
                repeatTextBox.Text = "1";

                StimuliPackList_Refresh();
                BatchDataList_Refresh();
            }
        }
        private void VariableApply(List<BatchData> batchDataList)
        {
            int totalVariousSize = 1;
            foreach (Variable variable in variableList) totalVariousSize *= variable.VariousSize;

            List<List<Change>> changeListList = new List<List<Change>>();
            for (int index = 0; index < totalVariousSize; index++)
            {
                List<Change> newChangeList = new List<Change>();
                changeListList.Add(newChangeList);
            }
            
            for (int variableIndex = 0; variableIndex < variableList.Count; variableIndex++)
            {
                for (int index = 0; index < totalVariousSize; index += variableList[variableIndex].VariousSize)
                {
                    int subIndex = 0;
                    for (decimal point = (decimal)variableList[variableIndex].StartPoint; point <= (decimal)variableList[variableIndex].EndPoint; point += (decimal)variableList[variableIndex].Step)
                    {
                        Change newChange = new Change();
                        newChange.LayerConnectionName = variableList[variableIndex].LayerConnectionName;
                        newChange.ProcessName = variableList[variableIndex].ProcessName;
                        newChange.VariableType = variableList[variableIndex].VariableType;
                        newChange.Point = (double)point;

                        changeListList[index + subIndex].Add(newChange);
                        subIndex++;
                    }
                }   
            }


            List<BatchData> variableBatchData = new List<BatchData>();
            foreach (BatchData batchData in batchDataList)
            {
                for (int index = 0; index < changeListList.Count; index++)
                {
                    BatchData newBatchData = new BatchData();

                    newBatchData.Name = batchName;
                    newBatchData.ArchitectureFile = batchData.ArchitectureFile;
                    newBatchData.ProcessFile = batchData.ProcessFile;
                    newBatchData.LearningSetupFile = batchData.LearningSetupFile;
                    foreach (string key in batchData.StimuliPackFileDictionary.Keys) newBatchData.StimuliPackFileDictionary[key] = batchData.StimuliPackFileDictionary[key];

                    Simulator newBatchSimulator = new Simulator(random);
                    newBatchSimulator.Architecture_Load(newBatchData.ArchitectureFile);
                    newBatchSimulator.Process_Load(newBatchData.ProcessFile);
                    foreach (string key in newBatchData.StimuliPackFileDictionary.Keys) newBatchSimulator.StimuliImport(key, newBatchData.StimuliPackFileDictionary[key]);
                    newBatchData.Simulator = newBatchSimulator;
                    newBatchData.LearningSetupList = LearningSetup_Load(newBatchData.LearningSetupFile);
                    newBatchData.ChangeList = changeListList[index];

                    variableBatchData.Add(newBatchData);
                }                
            }

            foreach (BatchData batchData in variableBatchData)
            {
                foreach (Change change in batchData.ChangeList)
                {
                    switch (change.VariableType)
                    {
                        case VariableType.Layer_Unit:
                            List<string> renewalConnectionKeyList = new List<string>();
                            foreach (string key in batchData.Simulator.ConnectionList.Keys)
                            {
                                if (batchData.Simulator.ConnectionList[key].SendLayer.Name == change.LayerConnectionName || batchData.Simulator.ConnectionList[key].ReceiveLayer.Name == change.LayerConnectionName) renewalConnectionKeyList.Add(key);
                            }
                            batchData.Simulator.LayerMaking(change.LayerConnectionName, (int)change.Point, batchData.Simulator.LayerList[change.LayerConnectionName].CleanupUnitCount);                            
                            foreach (string key in renewalConnectionKeyList)
                            {
                                batchData.Simulator.ConnectionMaking(key, batchData.Simulator.ConnectionList[key].SendLayer.Name, batchData.Simulator.ConnectionList[key].ReceiveLayer.Name);
                            }
                            break;
                        case VariableType.Layer_DamagedSD:
                            if (change.Point != 0) batchData.Simulator.ProcessDictionary[change.ProcessName].LayerStateDictionary[change.LayerConnectionName] = LayerState.Damaged;
                            else batchData.Simulator.ProcessDictionary[change.ProcessName].LayerStateDictionary[change.LayerConnectionName] = LayerState.On;
                            batchData.Simulator.ProcessDictionary[change.ProcessName].LayerDamagedSDDictionary[change.LayerConnectionName] = change.Point;
                            break;
                        case VariableType.Connection_DamagedSD:
                            if (change.Point != 0) batchData.Simulator.ProcessDictionary[change.ProcessName].ConnectionStateDictionary[change.LayerConnectionName] = ConnectionState.Damaged;
                            else batchData.Simulator.ProcessDictionary[change.ProcessName].ConnectionStateDictionary[change.LayerConnectionName] = ConnectionState.On;
                            batchData.Simulator.ProcessDictionary[change.ProcessName].ConnectionDamagedSDDictionary[change.LayerConnectionName] = change.Point;
                            break;
                        case VariableType.LearningRate:
                            batchData.Simulator.LearningRate = change.Point;
                            break;
                        case VariableType.InitialWeight:
                            batchData.Simulator.WeightRange = change.Point;
                            break;
                    }
                }
                batchData.Simulator.SimulatorRenewal();
            }

            this.batchDataList.AddRange(variableBatchData);
        }

        private void BatchDataList_Refresh()
        {
            batchDataListBox.Items.Clear();

            for (int index = 0; index < batchDataList.Count; index++)
            {
                batchDataListBox.Items.Add("[" + index + "] " + batchDataList[index].Name);
            }
        }

        private void batchDataDeleteButton_Click(object sender, EventArgs e)
        {
            if (batchDataListBox.SelectedIndex >= 0)
            {
                batchDataList.RemoveAt(batchDataListBox.SelectedIndex);

                BatchDataList_Refresh();
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BatchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Visible = true;
        }        

        private void batchDataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            StringBuilder newText = new StringBuilder();

            if (batchDataListBox.SelectedIndex >= 0)
            {
                BatchData selectedBatchData = batchDataList[batchDataListBox.SelectedIndex];

                newText.Append("Architecture File: " + selectedBatchData.ArchitectureFile); newText.AppendLine();
                newText.Append("Process File: " + selectedBatchData.ProcessFile); newText.AppendLine();
                newText.Append("Learning Setup File: " + selectedBatchData.LearningSetupFile); newText.AppendLine();
                newText.AppendLine("Stimuli Pack Files");
                foreach (string key in selectedBatchData.StimuliPackFileDictionary.Keys) newText.Append("- " + key + ": " + selectedBatchData.StimuliPackFileDictionary[key]); newText.AppendLine();

                newText.AppendLine();
                newText.AppendLine("Layer List");
                foreach (string key in selectedBatchData.Simulator.LayerList.Keys) newText.AppendLine("- " + key + "(" + selectedBatchData.Simulator.LayerList[key].UnitCount + ")");

                newText.AppendLine();
                newText.AppendLine("Connection List");
                foreach (string key in selectedBatchData.Simulator.ConnectionList.Keys) newText.AppendLine("- " + key + "(" + selectedBatchData.Simulator.ConnectionList[key].SendLayer.Name + " -> " + selectedBatchData.Simulator.ConnectionList[key].ReceiveLayer.Name + ")");

                newText.AppendLine();
                newText.AppendLine("Process List");
                foreach (string key in selectedBatchData.Simulator.ProcessDictionary.Keys) newText.Append("- " + key); newText.AppendLine();

                newText.AppendLine();
                newText.Append("Stimuli Pack List \n");
                foreach (string key in selectedBatchData.Simulator.StimuliPackDictionary.Keys) newText.AppendLine("- " + key + "(" + selectedBatchData.Simulator.StimuliPackDictionary[key].Count + ")");

                newText.AppendLine();
                newText.Append("Learning Setup List \n");
                foreach (LearningSetup learningSetup in selectedBatchData.LearningSetupList) newText.AppendLine("-Training Info.(" + learningSetup.TrainingMatchingInformationList.Count + ")/" + "Test Info.(" + learningSetup.TestMatchingInformationList.Count + ")");

                newText.AppendLine();
                newText.AppendLine("Applied Variable");
                foreach (Change change in selectedBatchData.ChangeList)
                {
                    switch (change.VariableType)
                    {
                        case VariableType.Layer_Unit:
                            newText.Append("Layer Unit Variable | ");
                            break;
                        case VariableType.Layer_DamagedSD:
                            newText.Append("Layer Damaged SD Variable | ");
                            break;
                        case VariableType.Connection_DamagedSD:
                            newText.Append("Connection Damaged SD Variable | ");
                            break;
                        case VariableType.LearningRate:
                            newText.Append("Learning Rate Variable | ");
                            break;
                        case VariableType.InitialWeight:
                            newText.Append("Initial Weight Variable | ");
                            break;
                    }
                    switch (change.VariableType)
                    {
                        case VariableType.Layer_Unit:
                        case VariableType.Layer_DamagedSD:
                            newText.Append("Layer: " + change.LayerConnectionName + " | ");
                            break;
                        case VariableType.Connection_DamagedSD:
                            newText.Append("Connection: " + change.LayerConnectionName + " | ");
                            break;
                    }
                    switch (change.VariableType)
                    {
                        case VariableType.Layer_DamagedSD:
                        case VariableType.Connection_DamagedSD:
                            newText.Append("Process: " + change.ProcessName + " | ");
                            break;
                    }
                    newText.Append(change.Point.ToString());
                    newText.AppendLine();
                }
            }
            BatchDataInformationRichTextBox.Text = newText.ToString();
        }

        private void learningButton_Click(object sender, EventArgs e)
        {
            if (batchDataList.Count < 1)
            {
                MessageBox.Show("At least, there has to be a batch data.");
            }
            else
            {
                BatchLearningForm batchLearningForm = new BatchLearningForm();
                batchLearningForm.Owner = this.Owner;
                batchLearningForm.Show();
                this.FormClosed -= BatchForm_FormClosed;
                this.Close();
            }
        }

        private void variableDeleteButton_Click(object sender, EventArgs e)
        {
            if (variableListBox.SelectedIndex >= 0)
            {
                variableList.RemoveAt(variableListBox.SelectedIndex);
                VariableList_Refresh();
            }
        }

        private void variableAddButton_Click(object sender, EventArgs e)
        {
            if (!RegularExpression.DoubleCheck(startPointTextBox.Text))
            {
                MessageBox.Show("Start point is wrong. Check Please.");
            }
            else if (!RegularExpression.DoubleCheck(endPointTextBox.Text))
            {
                MessageBox.Show("End point is wrong. Check Please.");
            }
            else if (!RegularExpression.DoubleCheck(stepTextBox.Text))
            {
                MessageBox.Show("Step is wrong. Check Please.");
            }
            else if (double.Parse(startPointTextBox.Text) > double.Parse(endPointTextBox.Text))
            {
                MessageBox.Show("Start point is bigger than end point. Check Please.");
            }
            else
            {
                if ((string)variableComboBox.SelectedItem == "Layer-Unit" && (!RegularExpression.PositiveIntCheck(startPointTextBox.Text) || !RegularExpression.PositiveIntCheck(endPointTextBox.Text) || !RegularExpression.PositiveIntCheck(stepTextBox.Text)))
                {
                    MessageBox.Show("Unit always has to be a positive integer.");
                }
                else if (decimal.Parse(endPointTextBox.Text) - decimal.Parse(startPointTextBox.Text) != 0 && (decimal.Parse(endPointTextBox.Text) - decimal.Parse(startPointTextBox.Text)) % decimal.Parse(stepTextBox.Text) != 0.0m)
                {
                    MessageBox.Show("There has to be no remainder in the dividing of the range and the step.");
                }
                else
                {
                    Variable newVariable = new Variable();

                    newVariable.LayerConnectionName = layerConnectionNameTextBox.Text;
                    newVariable.ProcessName = processNameTextBox.Text;
                    newVariable.StartPoint = double.Parse(startPointTextBox.Text);
                    newVariable.EndPoint = double.Parse(endPointTextBox.Text);
                    newVariable.Step = double.Parse(stepTextBox.Text);

                    switch ((string)variableComboBox.SelectedItem)
                    {
                        case "Layer-Unit":
                            newVariable.VariableType = VariableType.Layer_Unit;
                            newVariable.ProcessName = "";
                            break;
                        case "Layer-DamagedSD":
                            newVariable.VariableType = VariableType.Layer_DamagedSD;
                            break;
                        case "Connection-DamagedSD":
                            newVariable.VariableType = VariableType.Connection_DamagedSD;
                            break;
                        case "Learning Rate":
                            newVariable.VariableType = VariableType.LearningRate;
                            newVariable.LayerConnectionName = "";
                            newVariable.ProcessName = "";
                            break;
                        case "Initial Weight":
                            newVariable.VariableType = VariableType.InitialWeight;
                            newVariable.LayerConnectionName = "";
                            newVariable.ProcessName = "";
                            break;
                    }                    

                    bool isExist = false;
                    foreach (Variable variable in variableList)
                    {
                        if (newVariable.VariableType == variable.VariableType && newVariable.LayerConnectionName == variable.LayerConnectionName)
                        {
                            isExist = true;
                            break;
                        }
                    }

                    if (isExist)
                    {
                        MessageBox.Show("There is already same variable.\nIt is impossible to make another variable which has same name and variable type.");
                    }
                    else
                    {
                        variableList.Add(newVariable);
                        VariableList_Refresh();

                        variableComboBox.SelectedIndex = -1;
                        layerConnectionNameTextBox.Text = "";
                        processNameTextBox.Text = "";
                        startPointTextBox.Text = "";
                        endPointTextBox.Text = "";
                        stepTextBox.Text = "";
                    }
                }
            }
        }

        private void VariableList_Refresh()
        {
            variableListBox.Items.Clear();
            foreach (Variable variable in variableList)
            {
                StringBuilder newItem = new StringBuilder();
                newItem.Append(variable.VariousSize.ToString() + " | ");
                switch (variable.VariableType)
                {
                    case VariableType.Layer_Unit:
                        newItem.Append("Layer Unit Variable | ");
                        break;
                    case VariableType.Layer_DamagedSD:
                        newItem.Append("Layer Damaged SD Variable | ");
                        break;
                    case VariableType.Connection_DamagedSD:
                        newItem.Append("Connection Damaged SD Variable | ");
                        break;
                    case VariableType.LearningRate:
                        newItem.Append("Learning Rate Variable | ");
                        break;
                    case VariableType.InitialWeight:
                        newItem.Append("Initial Weight Variable | ");
                        break;
                }
                switch (variable.VariableType)
                {
                    case VariableType.Layer_Unit:
                    case VariableType.Layer_DamagedSD:
                        newItem.Append("Layer: " + variable.LayerConnectionName + " | ");
                        break;
                    case VariableType.Connection_DamagedSD:
                        newItem.Append("Connection: " + variable.LayerConnectionName + " | ");
                        break;
                }
                switch (variable.VariableType)
                {
                    case VariableType.Layer_DamagedSD:
                    case VariableType.Connection_DamagedSD:
                        newItem.Append("Process: " + variable.ProcessName + " | ");
                        break;
                }
                newItem.Append(variable.StartPoint.ToString() + " to " + variable.EndPoint.ToString() + " | ");
                newItem.Append("Step: " + variable.Step.ToString());

                variableListBox.Items.Add(newItem.ToString());
            }

            int totalVariousSize = 0;
            if (variableList.Count > 0)
            {
                totalVariousSize = 1;
                foreach (Variable variable in variableList)
                {
                    totalVariousSize *= variable.VariousSize;
                }

                totalVariousSizeLabel.Text = "Total: " + totalVariousSize;
            }
            else totalVariousSizeLabel.Text = "";
        }

        private void variableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((string)variableComboBox.SelectedItem)
            {
                case "Layer-Unit":
                    layerConnectionNameTextBox.Enabled = true;
                    processNameTextBox.Enabled = false;
                    break;
                case "Layer-DamagedSD":
                case "Connection-DamagedSD":
                    layerConnectionNameTextBox.Enabled = true;
                    processNameTextBox.Enabled = true;
                    break;
                case "Learning Rate":
                case "Initial Weight":                    
                    layerConnectionNameTextBox.Enabled = false;
                    processNameTextBox.Enabled = false;
                    break;
                default:
                    layerConnectionNameTextBox.Enabled = true;
                    processNameTextBox.Enabled = true;
                    break;
            }
        }
    }

    class BatchData
    {
        public BatchData()
        {
            StimuliPackFileDictionary = new Dictionary<string, string>();
            ChangeList = new List<Change>();
        }
        public Simulator Simulator
        {
            get;
            set;
        }
        public List<LearningSetup> LearningSetupList
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        public string ArchitectureFile
        {
            get;
            set;
        }
        public string ProcessFile
        {
            get;
            set;
        }
        public string LearningSetupFile
        {
            get;
            set;
        }
        public Dictionary<string, string> StimuliPackFileDictionary
        {
            get;
            set;
        }
        public List<Change> ChangeList
        {
            get;
            set;
        }
    }

    class Variable
    {
        public VariableType VariableType
        {
            get;
            set;
        }
        public string LayerConnectionName
        {
            get;
            set;
        }
        public string ProcessName
        {
            get;
            set;
        }
        public double StartPoint
        {
            get;
            set;
        }
        public double EndPoint
        {
            get;
            set;
        }
        public double Step
        {
            get;
            set;
        }

        public int VariousSize
        {
            get
            {
                if ((decimal)Step != 0) return (int)((((decimal)EndPoint - (decimal)StartPoint) / (decimal)Step) + 1m);
                else return 1;
            }
        }
    }
    class Change
    {
        public VariableType VariableType
        {
            get;
            set;
        }
        public string LayerConnectionName
        {
            get;
            set;
        }
        public string ProcessName
        {
            get;
            set;
        }
        public double Point
        {
            get;
            set;
        }        
    }
    public enum VariableType
    {
        Layer_Unit,
        Layer_DamagedSD,
        Connection_DamagedSD,
        LearningRate,
        InitialWeight,
    }
}
