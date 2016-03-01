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
    //Microsoft Automatic Graph Layout 추가
    //http://research.microsoft.com/en-us/projects/msagl/default.aspx

    public partial class ArchitectureForm : Form
    {
        Simulator simulator;
        Microsoft.Msagl.Drawing.Graph architectureGraph;

        public ArchitectureForm()
        {
            InitializeComponent();
            this.FormClosed += ArchitectureForm_FormClosed;

            this.simulator = SimulatorAccessor.simulator;
            if (simulator.IsSet)
            {
                layerSetupGroupBox.Enabled = true;
                connectionSetupGroupBox.Enabled = true;
            }

            this.architectureGraph = ArchitectureGraphAccessor.architectureGraph;
            

            Refresh();
        }

        private void layerAddButton_Click(object sender, EventArgs e)
        {
            if (!RegularExpression.PositiveIntCheck(layerUnitAmountTextBox.Text) || !RegularExpression.UIntCheck(cleanUpUnitAmountTextBox.Text) || (!RegularExpression.PositiveIntCheck(tickTextBox.Text) && bpttUseCheckBox.Checked))
            {
                MessageBox.Show("Inserted Data is worng. Check Please.");

                layerUnitAmountTextBox.Focus();
                layerUnitAmountTextBox.SelectAll();
            }
            else if (layerNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Layer has to be assigned the name.");
            }
            else if (layerUnitAmountTextBox.Text != "")
            {                
                if(simulator.LayerList.ContainsKey(layerNameTextBox.Text))
                {
                    MessageBox.Show("Already the same layer exist");
                }
                else
                {
                    if(!bpttUseCheckBox.Checked) simulator.LayerMaking(layerNameTextBox.Text, int.Parse(layerUnitAmountTextBox.Text), int.Parse(cleanUpUnitAmountTextBox.Text));
                    else simulator.LayerMaking(layerNameTextBox.Text, int.Parse(layerUnitAmountTextBox.Text), int.Parse(cleanUpUnitAmountTextBox.Text), int.Parse(tickTextBox.Text));
                    Refresh();
                }               
            }

            layerNameTextBox.Focus();
            layerNameTextBox.SelectAll();
        }
        private void layerDeleteButton_Click(object sender, EventArgs e)
        {
            if (layerListBox.SelectedIndex >= 0)
            {
                string selectedLayerName = ((string)layerListBox.SelectedItem).Substring(0, ((string)layerListBox.SelectedItem).IndexOf('('));

                if(simulator.LayerList[selectedLayerName].SendConnectionList.Count + simulator.LayerList[selectedLayerName].ReceiveConnectionList.Count > 0)
                {
                    MessageBox.Show("Already selected Layer connected to other layer.\nAt first, delete the connection.");                    
                }
                else
                {
                    if(simulator.LayerList.ContainsKey(selectedLayerName)) simulator.LayerList.Remove(selectedLayerName);
                    Refresh();
                }
            }
        }

        private void connectionAddButton_Click(object sender, EventArgs e)
        {
            if (connectionFromComboBox.SelectedIndex >= 0 && connectionToComboBox.SelectedIndex >= 0)
            {
                bool sameConnectionExist = false;
                foreach (string key in simulator.ConnectionList.Keys) if (simulator.ConnectionList[key].SendLayer.Name == (string)connectionFromComboBox.SelectedItem && simulator.ConnectionList[key].ReceiveLayer.Name == (string)connectionToComboBox.SelectedItem) sameConnectionExist = true;

                if (sameConnectionExist)
                {
                    MessageBox.Show("Already the same connection exist.");
                }
                else if (simulator.ConnectionList.ContainsKey(connectionNameTextBox.Text))
                {
                    MessageBox.Show("Already the same name exist.");
                }
                else if (connectionNameTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Connection has to be assigned the name.");
                }
                else 
                {
                    simulator.ConnectionMaking(connectionNameTextBox.Text, (string)connectionFromComboBox.SelectedItem, (string)connectionToComboBox.SelectedItem);
                    Refresh();
                }
            }
            
            connectionNameTextBox.Focus();
            connectionNameTextBox.SelectAll();
        }
        private void connectionDeleteButton_Click(object sender, EventArgs e)
        {
            if (connectionListBox.SelectedIndex >= 0)
            {
                string selectedConnectionName = ((string)connectionListBox.SelectedItem).Substring(0, ((string)connectionListBox.SelectedItem).IndexOf('('));

                foreach (string key in simulator.LayerList.Keys)
                {
                    simulator.LayerList[key].SendConnectionList.Remove(selectedConnectionName);
                    simulator.LayerList[key].ReceiveConnectionList.Remove(selectedConnectionName);
                }
                simulator.ConnectionList.Remove(selectedConnectionName);
                Refresh();
            }
        }


        private void configSubmitButton_Click(object sender, EventArgs e)
        {
            if (!(RegularExpression.DoubleCheck(momentumTextBox.Text) || RegularExpression.ScientificNotationDoubleCheck(momentumTextBox.Text)))
            {
                MessageBox.Show("Momentum is worng. Check Please.");
                momentumTextBox.Focus();
                momentumTextBox.SelectAll();
            }
            else if (!(RegularExpression.DoubleCheck(activationCriterionTextBox.Text) || RegularExpression.ScientificNotationDoubleCheck(activationCriterionTextBox.Text)))
            {
                MessageBox.Show("Activation Criterion is worng. Check Please.");
                activationCriterionTextBox.Focus();
                activationCriterionTextBox.SelectAll();
            }
            else if (!(RegularExpression.DoubleCheck(inactivationCriterionTextBox.Text) || RegularExpression.ScientificNotationDoubleCheck(inactivationCriterionTextBox.Text)))
            {
                MessageBox.Show("Inactivation Criterion is worng. Check Please.");
                inactivationCriterionTextBox.Focus();
                inactivationCriterionTextBox.SelectAll();
            }
            else if (!(RegularExpression.DoubleCheck(initialWeightRangeTextBox.Text) || RegularExpression.ScientificNotationDoubleCheck(initialWeightRangeTextBox.Text)))
            {
                MessageBox.Show("Initial Weight is worng. Check Please.");
                initialWeightRangeTextBox.Focus();
                initialWeightRangeTextBox.SelectAll();
            }
            else if (!(RegularExpression.DoubleCheck(decayRateTextBox.Text) || RegularExpression.ScientificNotationDoubleCheck(decayRateTextBox.Text)))
            {
                MessageBox.Show("DecayRate Data is worng. Check Please.");
                decayRateTextBox.Focus();
                decayRateTextBox.SelectAll();
            }
            else if (!(RegularExpression.DoubleCheck(learningRateTextBox.Text) || RegularExpression.ScientificNotationDoubleCheck(learningRateTextBox.Text)))
            {
                MessageBox.Show("Learning Rate is worng. Check Please.");
                learningRateTextBox.Focus();
                learningRateTextBox.SelectAll();
            }
            else
            {
                simulator.Momentum = double.Parse(momentumTextBox.Text);
                simulator.ActivationCriterion = double.Parse(activationCriterionTextBox.Text);
                simulator.InactivationCriterion = double.Parse(inactivationCriterionTextBox.Text);
                simulator.DecayRate = double.Parse(decayRateTextBox.Text);
                simulator.WeightRange = double.Parse(initialWeightRangeTextBox.Text);
                simulator.LearningRate = double.Parse(learningRateTextBox.Text);


                simulator.IsSet = true;
                
                layerSetupGroupBox.Enabled = true;
                connectionSetupGroupBox.Enabled = true;                
            }
        }


        private void structureSaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Architecture XML File(*.ACTXML)|*.ACTXML";
            saveFileDialog.InitialDirectory = Application.StartupPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                simulator.Architecture_Save(saveFileDialog.FileName);
            }
        }
        private void structureLoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Architecture XML File(*.ACTXML)|*.ACTXML";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Current layer and connection information will be deleted.", "Caution", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    simulator.Architecture_Load(openFileDialog.FileName);

                    layerSetupGroupBox.Enabled = true;
                    connectionSetupGroupBox.Enabled = true;

                    Refresh();
                }
            }
        }        
        private void exitButton_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
        
        private void ArchitectureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Visible = true;
        }

        private new void Refresh()
        {
            layerListBox.Items.Clear();
            connectionListBox.Items.Clear();
            connectionFromComboBox.Items.Clear();
            connectionToComboBox.Items.Clear();
            layerNameTextBox.Text = "";
            layerUnitAmountTextBox.Text = "";
            cleanUpUnitAmountTextBox.Text = "";
            tickTextBox.Text = "";
            bpttUseCheckBox.Checked = false;
            connectionNameTextBox.Text = "";

            foreach (string key in simulator.LayerList.Keys)
            {
                if(simulator.LayerList[key].LayerType == LayerType.NormalLayer) layerListBox.Items.Add(key + "(" + simulator.LayerList[key].UnitCount + ", " + simulator.LayerList[key].CleanupUnitCount + ")");
                else if (simulator.LayerList[key].LayerType == LayerType.BPTTLayer) layerListBox.Items.Add(key + "(BPTT, " + simulator.LayerList[key].UnitCount + ", " + simulator.LayerList[key].CleanupUnitCount + ", " + ((BPTTLayer)simulator.LayerList[key]).Tick + ")");

                connectionFromComboBox.Items.Add(key);
                connectionToComboBox.Items.Add(key);
            }
            foreach (string key in simulator.ConnectionList.Keys)
            {
                connectionListBox.Items.Add(key + "(" + simulator.ConnectionList[key].SendLayer.Name + " -> " + simulator.ConnectionList[key].ReceiveLayer.Name + ")");
            }
            momentumTextBox.Text = simulator.Momentum.ToString();
            activationCriterionTextBox.Text = simulator.ActivationCriterion.ToString();
            inactivationCriterionTextBox.Text = simulator.InactivationCriterion.ToString();
            decayRateTextBox.Text = simulator.DecayRate.ToString();
            initialWeightRangeTextBox.Text = simulator.WeightRange.ToString();
            learningRateTextBox.Text = simulator.LearningRate.ToString();

            ArchitectureGraphRefresh();
        }
        private void ArchitectureGraphRefresh()
        {
            foreach (string nodeName in ArchitectureGraphAccessor.nodeList) architectureGraph.RemoveNode(architectureGraph.FindNode(nodeName));
            //foreach (string key in simulator.LayerList.Keys)
            //{
            //    architectureGraph.AddEdge(simulator.LayerList[key].Name + "Bias", simulator.LayerList[key].Name);
            //    Microsoft.Msagl.Drawing.Node biasNode = architectureGraph.FindNode(simulator.LayerList[key].Name + "Bias");
                
            //    biasNode.LabelText = "";
            //}
            foreach (string key in simulator.ConnectionList.Keys)
            {
                architectureGraph.AddEdge(simulator.ConnectionList[key].SendLayer.Name, simulator.ConnectionList[key].ReceiveLayer.Name);
                ArchitectureGraphAccessor.nodeList.Add(simulator.ConnectionList[key].SendLayer.Name);
                ArchitectureGraphAccessor.nodeList.Add(simulator.ConnectionList[key].ReceiveLayer.Name);

                Microsoft.Msagl.Drawing.Node sendLayerNode = architectureGraph.FindNode(simulator.ConnectionList[key].SendLayer.Name);
                sendLayerNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
                sendLayerNode.Attr.LabelMargin = 10;
                Microsoft.Msagl.Drawing.Node receiveLayerNode = architectureGraph.FindNode(simulator.ConnectionList[key].ReceiveLayer.Name);
                receiveLayerNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
                receiveLayerNode.Attr.LabelMargin = 10;
            }
            foreach(string key in simulator.LayerList.Keys)
            {
                if(simulator.LayerList[key].LayerType == LayerType.BPTTLayer)
                {
                    BPTTLayer selectedBPTTLayer = (BPTTLayer)simulator.LayerList[key];

                    //Microsoft.Msagl.Drawing.Edge TickInEdge = architectureGraph.AddEdge(selectedBPTTLayer.Name, selectedBPTTLayer.Name + " 0 Tick");
                    for(int i=0;i< selectedBPTTLayer.Tick;i++)
                    {
                        architectureGraph.AddEdge(selectedBPTTLayer.Name, selectedBPTTLayer.Name + " " + i.ToString() + " Tick").Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Dotted);
                        architectureGraph.AddEdge(selectedBPTTLayer.Name + " " + i.ToString() + " Tick", selectedBPTTLayer.Name + " " + (i + 1).ToString() + " Tick");
                        ArchitectureGraphAccessor.nodeList.Add(selectedBPTTLayer.Name);
                        ArchitectureGraphAccessor.nodeList.Add(selectedBPTTLayer.Name + " " + i.ToString() + " Tick");
                        ArchitectureGraphAccessor.nodeList.Add(selectedBPTTLayer.Name + " " + (i + 1).ToString() + " Tick");
                    }
                    Microsoft.Msagl.Drawing.Edge TickOutEdge = architectureGraph.AddEdge(selectedBPTTLayer.Name + " " + selectedBPTTLayer.Tick.ToString() + " Tick", selectedBPTTLayer.Name);

                    //TickInEdge.Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Dotted);
                    TickOutEdge.Attr.AddStyle(Microsoft.Msagl.Drawing.Style.Dotted);

                }
            }

            ArchitectureGraphAccessor.architectureGraphViewer.Graph = ArchitectureGraphAccessor.architectureGraph;
        }

        private void bpttUseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            tickTextBox.Enabled = bpttUseCheckBox.Checked;
        }
    }
    
    
}
