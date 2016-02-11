using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ConnectionistModel
{
    public partial class BatchForm : Form
    {
        List<BatchData> batchDataList;

        Random random;
        Simulator newSimulator;
        string batchName;
        Dictionary<string, string> newStimuliPackFileDictionary;

        public BatchForm()
        {
            InitializeComponent();
            this.FormClosed += BatchForm_FormClosed;

            batchDataList = SimulatorAccessor.batchDataList;
            batchDataList.Clear();

            random = new Random();
            newSimulator = new Simulator(random);
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
            else if (newSimulator.StimuliImport(stimuliPackNameTextBox.Text, stimuliPackTextBox.Text))
            {
                newStimuliPackFileDictionary[stimuliPackNameTextBox.Text] = stimuliPackTextBox.Text;
                StimuliPackList_Refresh();
            }
            else MessageBox.Show("Stimuli Data pack is worng. Check Please.");
        }
        private void stimuliPackDeleteButton_Click(object sender, EventArgs e)
        {
            if (stimuliPackListBox.SelectedIndex >= 0)
            {
                newSimulator.StimuliPackDictionary.Remove((string)stimuliPackListBox.SelectedItem);
                newStimuliPackFileDictionary.Remove((string)stimuliPackListBox.SelectedItem);

                StimuliPackList_Refresh();
            }
        }
        private void StimuliPackList_Refresh()
        {
            stimuliPackListBox.Items.Clear();
            stimuliPackTextBox.Text = "";
            stimuliPackNameTextBox.Text = "";

            foreach (string key in newSimulator.StimuliPackDictionary.Keys) stimuliPackListBox.Items.Add(key);
        }

        private void batchAddButton_Click(object sender, EventArgs e)
        {
            if (architectureTextBox.Text.Trim() == "" || processTextBox.Text.Trim() == "" || learningTextBox.Text.Trim() == "" || newSimulator.StimuliPackDictionary.Count < 1)
            {
                MessageBox.Show("Set the simulation files.");
            }
            else
            {
                BatchData newBatchData = new BatchData();

                newBatchData.Name = batchName;
                newBatchData.ArchitectureFile = architectureTextBox.Text;
                newBatchData.ProcessFile = processTextBox.Text;
                newBatchData.LearningSetupFile = learningTextBox.Text;
                newBatchData.StimuliPackFileDictionary = newStimuliPackFileDictionary;

                newSimulator.Architecture_Load(architectureTextBox.Text);
                newSimulator.Process_Load(processTextBox.Text);

                newBatchData.Simulator = newSimulator;

                newBatchData.LearningSetupList = LearningSetup_Load(learningTextBox.Text);

                batchDataList.Add(newBatchData);

                newSimulator = new Simulator(random);
                newStimuliPackFileDictionary = new Dictionary<string, string>();

                foreach(string key in batchDataList[batchDataList.Count-1].StimuliPackFileDictionary.Keys)
                {
                    newSimulator.StimuliImport(key, batchDataList[batchDataList.Count - 1].StimuliPackFileDictionary[key]);                
                    newStimuliPackFileDictionary[key] = batchDataList[batchDataList.Count - 1].StimuliPackFileDictionary[key];
                }                

                StimuliPackList_Refresh();
                BatchDataList_Refresh();
            }
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

        private void currentResetButton_Click(object sender, EventArgs e)
        {
            architectureTextBox.Text ="";
            processTextBox.Text = "";
            learningTextBox.Text = "";
            newSimulator.StimuliPackDictionary.Clear();
            newStimuliPackFileDictionary.Clear();
            StimuliPackList_Refresh();
        }

        private void batchDataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BatchDataInformationRichTextBox.Text = "";

            if (batchDataListBox.SelectedIndex >= 0)
            {
                BatchData selectedBatchData = batchDataList[batchDataListBox.SelectedIndex];

                BatchDataInformationRichTextBox.Text += "Architecture File: " + selectedBatchData.ArchitectureFile + "\n";
                BatchDataInformationRichTextBox.Text += "Process File: " + selectedBatchData.ProcessFile + "\n";
                BatchDataInformationRichTextBox.Text += "Learning Setup File: " + selectedBatchData.LearningSetupFile + "\n";
                BatchDataInformationRichTextBox.Text += "Stimuli Pack Files \n";
                foreach(string key in selectedBatchData.StimuliPackFileDictionary.Keys) BatchDataInformationRichTextBox.Text += "- " + key + ": "  + selectedBatchData.StimuliPackFileDictionary[key] + "\n";

                BatchDataInformationRichTextBox.Text += "\n";
                BatchDataInformationRichTextBox.Text += "Layer List \n";
                foreach (string key in selectedBatchData.Simulator.LayerList.Keys) BatchDataInformationRichTextBox.Text += "- " + key + "(" + selectedBatchData.Simulator.LayerList[key].UnitCount + ")\n";

                BatchDataInformationRichTextBox.Text += "\n";
                BatchDataInformationRichTextBox.Text += "Connection List \n";
                foreach (string key in selectedBatchData.Simulator.BundleList.Keys) BatchDataInformationRichTextBox.Text += "- " + key + "(" + selectedBatchData.Simulator.BundleList[key].SendLayer.Name + " -> " + selectedBatchData.Simulator.BundleList[key].ReceiveLayer.Name + ")\n";

                BatchDataInformationRichTextBox.Text += "\n";
                BatchDataInformationRichTextBox.Text += "Process List \n";
                foreach (string key in selectedBatchData.Simulator.ProcessDictionary.Keys) BatchDataInformationRichTextBox.Text += "- " + key + "\n";

                BatchDataInformationRichTextBox.Text += "\n";
                BatchDataInformationRichTextBox.Text += "Stimuli Pack List \n";
                foreach (string key in selectedBatchData.Simulator.StimuliPackDictionary.Keys) BatchDataInformationRichTextBox.Text += "- " + key + "(" + selectedBatchData.Simulator.StimuliPackDictionary[key].Count + ")\n";

                BatchDataInformationRichTextBox.Text += "\n";
                BatchDataInformationRichTextBox.Text += "Learning Setup List \n";
                foreach (LearningSetup learningSetup in selectedBatchData.LearningSetupList) BatchDataInformationRichTextBox.Text += "-Training Info.(" + learningSetup.TrainingMatchingInformationList.Count + ")/" + "Test Info.(" + learningSetup.TestMatchingInformationList.Count + ")";
                        
            }
            
        }

        private void learningButton_Click(object sender, EventArgs e)
        {
            BatchLearningForm batchLearningForm = new BatchLearningForm();
            batchLearningForm.Owner = this.Owner;
            batchLearningForm.Show();
            this.FormClosed -= BatchForm_FormClosed;
            this.Close();
        }
    }

    class BatchData
    {
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
    }
}
