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
    public partial class ProcessControlForm : Form
    {
        Simulator simulator;
        Process newProcess;
        bool isMaking = false;
        int selectedOrderIndex = -1;

        public ProcessControlForm()
        {
            InitializeComponent();
            this.FormClosed += ProcessControlForm_FormClosed;

            simulator = SimulatorAccessor.simulator;
            newProcess = new Process();

            ProcessRefresh();
            ControlRefresh();
            OrderRefresh();
        }

        private void processDeleteButton_Click(object sender, EventArgs e)
        {
            if (processListBox.SelectedIndex >= 0)
            {
                simulator.ProcessDictionary.Remove((string)processListBox.SelectedItem);
                ProcessRefresh();
                ControlRefresh();
                OrderRefresh();
            }
        }
        private void processCreateButton_Click(object sender, EventArgs e)
        {
            if (processNameTextBox.Text == "" ) MessageBox.Show("Insert process name.");
            else if (simulator.ProcessDictionary.ContainsKey(processNameTextBox.Text)) MessageBox.Show("there is already the process name in list.");
            else 
            {
                processNameTextBox.Enabled = false;
                processCreateButton.Enabled = false;
                processEditButton.Enabled = false;
                processCancelButton.Enabled = true;
                processFinishButton.Enabled = true;

                processListBox.Enabled = false;
                processDeleteButton.Enabled = false;

                processListBox.Enabled = false;

                layerControlGroupBox.Enabled = true;
                connectionControlGroupBox.Enabled = true;
                orderTabControl.Enabled = true;

                newProcess = new Process();
                newProcess.Name = processNameTextBox.Text;
                foreach (string key in simulator.LayerDictionary.Keys)
                {
                    newProcess.LayerStateDictionary[key] = LayerState.On;
                    newProcess.LayerDamagedSDDictionary[key] = 0;
                }
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    newProcess.ConnectionStateDictionary[key] = ConnectionState.On;
                    newProcess.ConnectionDamagedSDDictionary[key] = 0;
                }

                isMaking = true;

                ControlRefresh();
                OrderRefresh();
            }
        }
        private void processEditButton_Click(object sender, EventArgs e)
        {
            processNameTextBox.Enabled = false;
            processCreateButton.Enabled = false;
            processEditButton.Enabled = false;
            processCancelButton.Enabled = true;
            processFinishButton.Enabled = true;

            processListBox.Enabled = false;
            processDeleteButton.Enabled = false;

            layerControlGroupBox.Enabled = true;
            connectionControlGroupBox.Enabled = true;
            orderTabControl.Enabled = true;

            newProcess.Name = simulator.ProcessDictionary[(string)processListBox.SelectedItem].Name;
            foreach (Order order in simulator.ProcessDictionary[(string)processListBox.SelectedItem])
            {
                Order newOrder = new Order();
                newOrder.Code = order.Code;
                newOrder.Layer1Name = order.Layer1Name;
                newOrder.Layer2Name = order.Layer2Name;
                newOrder.Connection1Name = order.Connection1Name;
                newOrder.Connection2Name = order.Connection2Name;

                newProcess.Add(newOrder);
            }
            foreach (string key in simulator.ProcessDictionary[(string)processListBox.SelectedItem].LayerStateDictionary.Keys)
            {
                newProcess.LayerStateDictionary[key] = simulator.ProcessDictionary[(string)processListBox.SelectedItem].LayerStateDictionary[key];
                newProcess.LayerDamagedSDDictionary[key] = simulator.ProcessDictionary[(string)processListBox.SelectedItem].LayerDamagedSDDictionary[key];
            }
            foreach (string key in simulator.ProcessDictionary[(string)processListBox.SelectedItem].ConnectionStateDictionary.Keys)
            {
                newProcess.ConnectionStateDictionary[key] = simulator.ProcessDictionary[(string)processListBox.SelectedItem].ConnectionStateDictionary[key];
                newProcess.ConnectionDamagedSDDictionary[key] = simulator.ProcessDictionary[(string)processListBox.SelectedItem].ConnectionDamagedSDDictionary[key];
            }

            isMaking = true;

            ControlRefresh();
            OrderRefresh();
        }
        private void processCancelButton_Click(object sender, EventArgs e)
        {
            newProcess = new Process();

            ProcessRefresh();
            ControlRefresh();
            OrderRefresh();

            processNameTextBox.Enabled = true;
            processCreateButton.Enabled = true;
            processEditButton.Enabled = false;
            processCancelButton.Enabled = false;
            processFinishButton.Enabled = false;
            
            processListBox.Enabled = true;
            processDeleteButton.Enabled = true;
            
            layerControlGroupBox.Enabled = false;
            connectionControlGroupBox.Enabled = false;
            orderTabControl.Enabled = false;
            //bpGroupBox.Enabled = false;
            //bpttGroupBox.Enabled = false;
            //srnGroupBox.Enabled = false;
            //customOrderGroupBox.Enabled = false;

            isMaking = false;
        }
        private void processFinishButton_Click(object sender, EventArgs e)
        {
            if (newProcess.Count == 0) MessageBox.Show("There is no order.");
            else if (newProcess[newProcess.Count - 1].Code != OrderCode.EndInitialize) MessageBox.Show("Last order must be 'End & Initialize'");
            else
            {
                simulator.ProcessDictionary[newProcess.Name] = newProcess;
                newProcess = new Process();

                ProcessRefresh();
                ControlRefresh();
                OrderRefresh();

                processNameTextBox.Enabled = true;
                processCreateButton.Enabled = true;
                processEditButton.Enabled = false;
                processCancelButton.Enabled = false;
                processFinishButton.Enabled = false;

                processListBox.Enabled = true;
                processDeleteButton.Enabled = true;

                layerControlGroupBox.Enabled = false;
                connectionControlGroupBox.Enabled = false;
                orderTabControl.Enabled = false;
                //bpGroupBox.Enabled = false;
                //bpttGroupBox.Enabled = false;
                //srnGroupBox.Enabled = false;
                //customOrderGroupBox.Enabled = false;

                isMaking = false;
            }
        }
        
        private void processListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(processListBox.SelectedIndex > -1)
            {
                processEditButton.Enabled = true;
                ControlRefresh();
                OrderRefresh();
            }
            else processEditButton.Enabled = false;
        }
        
        private void layerOnButton_Click(object sender, EventArgs e)
        {
            if (layerControlListBox.SelectedIndex >= 0)
            {
                string selectedLayerName = (string)layerControlListBox.SelectedItem;
                selectedLayerName = selectedLayerName.Remove(selectedLayerName.LastIndexOf(" ->"));
                newProcess.LayerStateDictionary[selectedLayerName] = LayerState.On;
                newProcess.LayerDamagedSDDictionary[selectedLayerName] = 0;

                ControlRefresh();
            }
        }
        private void layerOffButton_Click(object sender, EventArgs e)
        {
            if (layerControlListBox.SelectedIndex >= 0)
            {
                string selectedLayerName = (string)layerControlListBox.SelectedItem;
                selectedLayerName = selectedLayerName.Remove(selectedLayerName.LastIndexOf(" ->"));
                newProcess.LayerStateDictionary[selectedLayerName] = LayerState.Off;
                newProcess.LayerDamagedSDDictionary[selectedLayerName] = 0;

                ControlRefresh();
            }
        }        
        private void layerDamageButton_Click(object sender, EventArgs e)
        {
            if (layerControlListBox.SelectedIndex >= 0)
            {
                if (!RegularExpression.DoubleCheck(layerDamagedSDTextBox.Text))
                {
                    MessageBox.Show("SD has to be a real number.");
                }
                else
                {
                    string selectedLayerName = (string)layerControlListBox.SelectedItem;
                    selectedLayerName = selectedLayerName.Remove(selectedLayerName.LastIndexOf(" ->"));
                    newProcess.LayerStateDictionary[selectedLayerName] = LayerState.Damaged;
                    newProcess.LayerDamagedSDDictionary[selectedLayerName] = double.Parse(layerDamagedSDTextBox.Text);

                    ControlRefresh();
                }
            }
        }

        private void connectionOnButton_Click(object sender, EventArgs e)
        {
            if (connectionControlListBox.SelectedIndex >= 0)
            {
                string selectedConnectionName = (string)connectionControlListBox.SelectedItem;
                selectedConnectionName = selectedConnectionName.Remove(selectedConnectionName.LastIndexOf(" ->"));
                newProcess.ConnectionStateDictionary[selectedConnectionName] = ConnectionState.On;
                newProcess.ConnectionDamagedSDDictionary[selectedConnectionName] = 0;

                ControlRefresh();
            }
        }
        private void connectionOffButton_Click(object sender, EventArgs e)
        {
            if (connectionControlListBox.SelectedIndex >= 0)
            {
                string selectedConnectionName = (string)connectionControlListBox.SelectedItem;
                selectedConnectionName = selectedConnectionName.Remove(selectedConnectionName.LastIndexOf(" ->"));
                newProcess.ConnectionStateDictionary[selectedConnectionName] = ConnectionState.Off;
                newProcess.ConnectionDamagedSDDictionary[selectedConnectionName] = 0;

                ControlRefresh();
            }
        }
        private void connectionDamageButton_Click(object sender, EventArgs e)
        {
            if (connectionControlListBox.SelectedIndex >= 0)
            {
                if (!RegularExpression.DoubleCheck(connectionDamagedSDTextBox.Text))
                {
                    MessageBox.Show("SD has to be a real number.");
                }
                else
                {
                    string selectedConnectionName = (string)connectionControlListBox.SelectedItem;
                    selectedConnectionName = selectedConnectionName.Remove(selectedConnectionName.LastIndexOf(" ->"));
                    newProcess.ConnectionStateDictionary[selectedConnectionName] = ConnectionState.Damaged;
                    newProcess.ConnectionDamagedSDDictionary[selectedConnectionName] = double.Parse(connectionDamagedSDTextBox.Text);

                    ControlRefresh();
                }
            }
        }                

        private void activationInsertButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();

                newOrder.Code = OrderCode.ActivationInput;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void activationCalculateSigmoidButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void activationCalculateSoftmaxButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.ActivationCalculate_Softmax;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void activationSendButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.ActivationSend;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void outputLayerErrorCalculateSigmoidButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Sigmoid;                
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void outputLayerErrorCalculateSoftmaxButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Softmax;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }        
        private void hiddenLayerErrorCalculateSigmoidButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Sigmoid;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void hiddenLayerErrorCalculateSoftmaxButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Softmax;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void interactButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.Interact;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void interconnectionRenewalButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.InterconnectionWeightRenewal;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;                

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void layerDuplicateButton_Click(object sender, EventArgs e)
        {
            if (orderLayer1ComboBox.SelectedIndex >= 0 && orderLayer2ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.LayerDuplicate;
                newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;
                newOrder.Layer2Name = (string)orderLayer2ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void connectionDuplicateButton_Click(object sender, EventArgs e)
        {
            if (orderConnection1ComboBox.SelectedIndex >= 0 && orderConnection2ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.ConnectionDuplicate;
                newOrder.Connection1Name = (string)orderConnection1ComboBox.SelectedItem;
                newOrder.Connection2Name = (string)orderConnection2ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void transposedConnectionDuplicateButton_Click(object sender, EventArgs e)
        {
            if (orderConnection1ComboBox.SelectedIndex >= 0 && orderConnection2ComboBox.SelectedIndex >= 0)
            {
                Order newOrder = new Order();
                newOrder.Code = OrderCode.TransposedConnectionDuplicate;
                newOrder.Connection1Name = (string)orderConnection1ComboBox.SelectedItem;
                newOrder.Connection2Name = (string)orderConnection2ComboBox.SelectedItem;

                newProcess.Add(newOrder);
                OrderRefresh();
            }
        }
        private void biasRenewalButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.BiasRenewal;
            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;
            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void weightRenewalButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.WeightRenewal;
            newOrder.Connection1Name = (string)orderConnection1ComboBox.SelectedItem;
            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void testValueStoreButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.TestValueStore;
            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void layerInitializeButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.LayerInitialize;
            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void connectionInitializeButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.ConnectionInitialize;
            newOrder.Connection1Name = (string)orderConnection1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void endInitializeButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.EndInitialize;

            newProcess.Add(newOrder);
            OrderRefresh();
        }

        private void layerToCleanupProcessButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.LayerToCleanUpForwardProcess;
            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void cleanupToLayerProcessButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.CleanUpToLayerForwardProcess;
            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void cleanupBackwardProcessButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.CleanUpBackwardProcess;
            
            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void tickInButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.TickIn;

            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void tickProgressButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.TickProgress;

            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void tickOutButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.TickOut;

            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }
        private void baseWeightRenewalButton_Click(object sender, EventArgs e)
        {
            Order newOrder = new Order();
            newOrder.Code = OrderCode.BaseWeightRenewal;

            newOrder.Layer1Name = (string)orderLayer1ComboBox.SelectedItem;

            newProcess.Add(newOrder);
            OrderRefresh();
        }

        private void orderDeleteButton_Click(object sender, EventArgs e)
        {
            if (orderListBox.SelectedIndex >= 0)
            {
                if (orderListBox.SelectedIndex < orderListBox.Items.Count - 1) selectedOrderIndex = orderListBox.SelectedIndex;
                else selectedOrderIndex = orderListBox.SelectedIndex - 1;

                newProcess.RemoveAt(orderListBox.SelectedIndex);
                OrderRefresh();
            }
        }
        private void orderDownButton_Click(object sender, EventArgs e)
        {
            if (orderListBox.SelectedIndex + 1 < orderListBox.Items.Count)
            {
                selectedOrderIndex = orderListBox.SelectedIndex + 1;

                Order tempOrder = newProcess[orderListBox.SelectedIndex];
                newProcess[orderListBox.SelectedIndex] = newProcess[orderListBox.SelectedIndex + 1];
                newProcess[orderListBox.SelectedIndex + 1] = tempOrder;

                OrderRefresh();
            }
        }
        private void orderUpButton_Click(object sender, EventArgs e)
        {
            if (orderListBox.SelectedIndex > 0)
            {
                selectedOrderIndex = orderListBox.SelectedIndex - 1;

                Order tempOrder = newProcess[orderListBox.SelectedIndex];
                newProcess[orderListBox.SelectedIndex] = newProcess[orderListBox.SelectedIndex - 1];
                newProcess[orderListBox.SelectedIndex - 1] = tempOrder;

                OrderRefresh();
            }
        }
        
        private void ProcessRefresh()
        {
            processCreateButton.Enabled = true;
            processEditButton.Enabled = false;
            processCancelButton.Enabled = false;
            processFinishButton.Enabled = false;

            processListBox.Items.Clear();
            processNameTextBox.Text = "";

            foreach (string key in simulator.ProcessDictionary.Keys) processListBox.Items.Add(key);
        }
        private void ControlRefresh()
        {
            layerControlListBox.Items.Clear();
            connectionControlListBox.Items.Clear();

            Process selectedProcess = new Process();
            if (isMaking) selectedProcess = newProcess;
            else if(processListBox.SelectedIndex >= 0) selectedProcess = simulator.ProcessDictionary[(string)processListBox.SelectedItem];

            foreach (string key in selectedProcess.LayerStateDictionary.Keys)
            {
                switch(selectedProcess.LayerStateDictionary[key])
                {
                    case LayerState.On:
                        layerControlListBox.Items.Add(key + " -> On");
                        break;
                    case LayerState.Off:
                        layerControlListBox.Items.Add(key + " -> Off");
                        break;
                    case LayerState.Damaged:
                        layerControlListBox.Items.Add(key + " -> Damage(" + selectedProcess.LayerDamagedSDDictionary[key] + ")");
                        break;
                }
            }
            foreach (string key in selectedProcess.ConnectionStateDictionary.Keys)
            {
                switch (selectedProcess.ConnectionStateDictionary[key])
                {
                    case ConnectionState.On:
                        connectionControlListBox.Items.Add(key + " -> On");
                        break;
                    case ConnectionState.Off:
                        connectionControlListBox.Items.Add(key + " -> Off");
                        break;
                    case ConnectionState.Damaged:
                        connectionControlListBox.Items.Add(key + " -> Damage(" + selectedProcess.ConnectionDamagedSDDictionary[key] + ")");
                        break;
                }
            }
        }
        private void OrderRefresh()
        {
            orderDeleteButton.Enabled = false;
            orderUpButton.Enabled = false;
            orderDownButton.Enabled = false;

            orderListBox.Items.Clear();
            orderLayer1ComboBox.Items.Clear();
            orderLayer2ComboBox.Items.Clear();
            orderConnection1ComboBox.Items.Clear();
            orderConnection2ComboBox.Items.Clear();

            foreach (string key in simulator.LayerDictionary.Keys) orderLayer1ComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) orderLayer2ComboBox.Items.Add(key);
            foreach (string key in simulator.ConnectionDictionary.Keys) orderConnection1ComboBox.Items.Add(key);
            foreach (string key in simulator.ConnectionDictionary.Keys) orderConnection2ComboBox.Items.Add(key);

            bpInputLayerComboBox.Items.Clear();
            bpHiddenLayerComboBox.Items.Clear();
            bpOutputLayerComboBox.Items.Clear();

            foreach (string key in simulator.LayerDictionary.Keys) bpInputLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) bpHiddenLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) bpOutputLayerComboBox.Items.Add(key);

            bpttInputLayerComboBox.Items.Clear();
            bpttHiddenLayerComboBox.Items.Clear();
            bpttOutputLayerComboBox.Items.Clear();

            foreach (string key in simulator.LayerDictionary.Keys) bpttInputLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) bpttHiddenLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) bpttOutputLayerComboBox.Items.Add(key);

            srnInputLayerComboBox.Items.Clear();
            srnHiddenLayerComboBox.Items.Clear();
            srnContextLayerComboBox.Items.Clear();
            srnOutputLayerComboBox.Items.Clear();

            foreach (string key in simulator.LayerDictionary.Keys) srnInputLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) srnHiddenLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) srnContextLayerComboBox.Items.Add(key);
            foreach (string key in simulator.LayerDictionary.Keys) srnOutputLayerComboBox.Items.Add(key);

            forwardLayerListBox.Items.Clear();
            backwardLayerListBox.Items.Clear();
            ForwardLayerComboboxListMaker();
            BackwardLayerComboboxListMaker();

            bpTestCheckBox.Checked = false;

            srnInputStimuliNameTextBox.Text = "";
            srnOutputStimuliNameTextBox.Text = "";
            srnTestCheckBox.Checked = false;

            forwardFirstLayerInputRadioButton.Checked = true;
            forwardFirstLayerHiddenRadioButton.Checked = false;
            forwardLastLayerHiddenRadioButton.Checked = false;
            forwardLastLayerOutputSigmoidRadioButton.Checked = true;
            backwardFirstLayerHiddenRadioButton.Checked = false;
            backwardFirstLayerOutputSigmoidRadioButton.Checked = true;

            if (isMaking)
            {
                orderDeleteButton.Enabled = true;
                orderUpButton.Enabled = true;
                orderDownButton.Enabled = true;
            }

            Process selectedProcess = new Process();
            if (isMaking) selectedProcess = newProcess;
            else if (processListBox.SelectedIndex >= 0) selectedProcess = simulator.ProcessDictionary[(string)processListBox.SelectedItem];

            Dictionary<string, int> displayedTick = new Dictionary<string, int>();
            foreach (Order order in selectedProcess)
            {
                Layer selectedLayer;
                switch (order.Code)
                {
                    case OrderCode.ActivationInput: //Activation Input          
                        orderListBox.Items.Add("Activation Input (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.ActivationCalculate_Sigmoid: //Activation Calculate
                        orderListBox.Items.Add("Activation Calculate Sigmoid (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.ActivationCalculate_Softmax: //Activation Calculate
                        orderListBox.Items.Add("Activation Calculate Softmax (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.ActivationSend: //Activation Send
                        orderListBox.Items.Add("Activation Send (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.OutputErrorRateCalculate_for_Sigmoid: //Output ErrorRate Calculate
                        orderListBox.Items.Add("Error Rate Calculate as Ouput Layer Sigmoid (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.OutputErrorRateCalculate_for_Softmax: //Output ErrorRate Calculate
                        orderListBox.Items.Add("Error Rate Calculate as Ouput Layer Softmax (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.HiddenErrorRateCalculate_for_Sigmoid: //Hidden ErrorRate Calculcate
                        orderListBox.Items.Add("Error Rate Calculate as Hidden Layer Sigmoid (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.HiddenErrorRateCalculate_for_Softmax: //Hidden ErrorRate Calculcate
                        orderListBox.Items.Add("Error Rate Calculate as Hidden Layer Softmax (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.Interact: //Inner Unit Interact
                        orderListBox.Items.Add("Inner Unit Interact (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.InterconnectionWeightRenewal: //Inner Unit Interact WeightChange
                        orderListBox.Items.Add("Inner Unit Interaction Weight Change (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.LayerDuplicate: //Layer Duplicate
                        orderListBox.Items.Add("Layer Duplicate From (" + order.Layer1Name + ") To (" + order.Layer2Name + ")");
                        break;
                    case OrderCode.ConnectionDuplicate: //Connection Duplicate
                        orderListBox.Items.Add("Connection Duplicate From (" + order.Connection1Name + ") To (" + order.Connection2Name + ")");
                        break;
                    case OrderCode.TransposedConnectionDuplicate: //Connection Duplicate
                        orderListBox.Items.Add("Transposed Connection Duplicate From (" + order.Connection1Name + ") To (" + order.Connection2Name + ")");
                        break;
                    case OrderCode.BiasRenewal: //Weight Renewal
                        orderListBox.Items.Add("Bias Renewal (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.WeightRenewal: //Weight Renewal
                        orderListBox.Items.Add("Weight Renewal (" + order.Connection1Name + ")");
                        break;
                    case OrderCode.TestValueStore: //Test Value Store
                        orderListBox.Items.Add("Test as Ouput Layer (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.LayerInitialize:
                        if (displayedTick.ContainsKey(order.Layer1Name)) displayedTick[order.Layer1Name] = 0;
                        orderListBox.Items.Add("Layer Initialize (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.ConnectionInitialize:
                        orderListBox.Items.Add("Connection Initialize (" + order.Connection1Name + ")");
                        break;
                    case OrderCode.EndInitialize: //End Initialize                        
                        displayedTick.Clear();
                        orderListBox.Items.Add("End & Initialize");
                        break;
                    case OrderCode.LayerToCleanUpForwardProcess:
                        orderListBox.Items.Add("Layer To Clean Up Forward Process (" + order.Layer1Name + ")");                        
                        break;
                    case OrderCode.CleanUpToLayerForwardProcess:
                        orderListBox.Items.Add("Clean Up To Layer Forward Process (" + order.Layer1Name + ")");
                        break;
                    case OrderCode.CleanUpBackwardProcess:
                        orderListBox.Items.Add("Clean Up Renewal (" + order.Layer1Name + ")");                        
                        break;
                    case OrderCode.TickIn:
                        selectedLayer = simulator.LayerDictionary[order.Layer1Name];

                        displayedTick[order.Layer1Name] = 0;
                        orderListBox.Items.Add("Tick In (" + order.Layer1Name + "(Tick " + displayedTick[order.Layer1Name].ToString() + "/" + ((BPTTLayer)selectedLayer).Tick.ToString() + "))");
                        break;
                    case OrderCode.TickProgress:
                        selectedLayer = simulator.LayerDictionary[order.Layer1Name];

                        displayedTick[order.Layer1Name]++;
                        orderListBox.Items.Add("Tick Progress (" + order.Layer1Name + "(Tick " + displayedTick[order.Layer1Name].ToString() + "/" + ((BPTTLayer)selectedLayer).Tick.ToString() + "))");
                        break;
                    case OrderCode.TickOut:
                        orderListBox.Items.Add("Tick Out (" + order.Layer1Name + "(Tick Finished))");
                        break;
                    case OrderCode.BaseWeightRenewal:
                        orderListBox.Items.Add("Base Weight Renewal (" + order.Layer1Name + "(Tick Finished))");
                        break;                   
                    case OrderCode.SRNTraining:
                        orderListBox.Items.Add("SRN Training");
                        break;
                    case OrderCode.SRNTest:
                        orderListBox.Items.Add("SRN Test");
                        break;
                }
            }

            orderLayer2ComboBox.Enabled = false;
            orderConnection2ComboBox.Enabled = false;

            activationInsertButton.Enabled = false;
            activationCalculateSigmoidButton.Enabled = false;
            activationCalculateSoftmaxButton.Enabled = false;
            activationSendButton.Enabled = false;
            outputLayerErrorCalculateSigmoidButton.Enabled = false;
            outputLayerErrorCalculateSoftmaxButton.Enabled = false;
            hiddenLayerErrorCalculateSigmoidButton.Enabled = false;
            hiddenLayerErrorCalculateSoftmaxButton.Enabled = false;
            interactButton.Enabled = false;
            interconnectionRenewalButton.Enabled = false;
            layerDuplicateButton.Enabled = false;
            connectionDuplicateButton.Enabled = false;
            transposedConnectionDuplicateButton.Enabled = false;
            testValueStoreButton.Enabled = false;
            weightRenewalButton.Enabled = false;
            biasRenewalButton.Enabled = false;
            layerToCleanupProcessButton.Enabled = false;
            cleanupToLayerProcessButton.Enabled = false;
            cleanupBackwardProcessButton.Enabled = false;
            tickInButton.Enabled = false;
            tickProgressButton.Enabled = false;
            tickOutButton.Enabled = false;
            layerInitializeButton.Enabled = false;

            orderListBox.SelectedIndex = selectedOrderIndex;
            selectedOrderIndex = -1;
        }

        private void orderLayer1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            activationInsertButton.Enabled = false;
            activationCalculateSigmoidButton.Enabled = false;
            activationCalculateSoftmaxButton.Enabled = false;
            activationSendButton.Enabled = false;
            outputLayerErrorCalculateSigmoidButton.Enabled = false;
            outputLayerErrorCalculateSoftmaxButton.Enabled = false;
            hiddenLayerErrorCalculateSigmoidButton.Enabled = false;            
            hiddenLayerErrorCalculateSoftmaxButton.Enabled = false;
            biasRenewalButton.Enabled = false;
            interactButton.Enabled = false;
            interconnectionRenewalButton.Enabled = false;
            layerDuplicateButton.Enabled = false;
            testValueStoreButton.Enabled = false;
            layerToCleanupProcessButton.Enabled = false;
            cleanupToLayerProcessButton.Enabled = false;
            cleanupBackwardProcessButton.Enabled = false;
            tickInButton.Enabled = false;
            tickProgressButton.Enabled = false;
            tickOutButton.Enabled = false;
            layerInitializeButton.Enabled = false;            

            if(orderLayer1ComboBox.SelectedIndex > -1)
            {
                activationInsertButton.Enabled = true;
                layerInitializeButton.Enabled = true;

                orderConnection1ComboBox.SelectedIndex = -1;
                orderConnection2ComboBox.SelectedIndex = -1;
                orderConnection2ComboBox.Enabled = false;

                orderLayer2ComboBox.Enabled = true;
                
                activationCalculateSigmoidButton.Enabled = true;
                activationCalculateSoftmaxButton.Enabled = true;

                bool isActivationInsert = false;
                foreach(Order order in newProcess)
                {
                    if ((order.Code == OrderCode.ActivationInput || order.Code == OrderCode.ActivationCalculate_Sigmoid || order.Code == OrderCode.ActivationCalculate_Softmax || order.Code == OrderCode.TickOut) && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem)
                    {
                        isActivationInsert = true;
                    }
                    else if(order.Code == OrderCode.LayerDuplicate && order.Layer2Name == (string)orderLayer1ComboBox.SelectedItem)
                    {
                        isActivationInsert = true;
                    }
                    else if ((order.Code == OrderCode.LayerInitialize && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem))
                    {
                        isActivationInsert = false;
                    }
                }

                if (isActivationInsert)
                {
                    activationSendButton.Enabled = true;                    
                    outputLayerErrorCalculateSigmoidButton.Enabled = true;
                    outputLayerErrorCalculateSoftmaxButton.Enabled = true;
                    hiddenLayerErrorCalculateSigmoidButton.Enabled = true;
                    hiddenLayerErrorCalculateSoftmaxButton.Enabled = true;

                    interactButton.Enabled = true;
                    layerToCleanupProcessButton.Enabled = true;
                    
                    testValueStoreButton.Enabled = true;
                }

                bool isLayerToCleanup = false;
                foreach (Order order in newProcess)
                {
                    if ((order.Code == OrderCode.LayerToCleanUpForwardProcess) && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem)
                    {
                        isLayerToCleanup = true;
                    }
                    else if ((order.Code == OrderCode.LayerInitialize && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem))
                    {
                        isLayerToCleanup = false;
                    }
                }
                if (isLayerToCleanup) cleanupToLayerProcessButton.Enabled = true;


                bool isErrorRateCalculate = false;
                foreach (Order order in newProcess)
                {
                    if ((order.Code == OrderCode.OutputErrorRateCalculate_for_Sigmoid || order.Code == OrderCode.OutputErrorRateCalculate_for_Softmax) && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem)
                    {
                        isErrorRateCalculate = true;
                    }
                    if ((order.Code == OrderCode.HiddenErrorRateCalculate_for_Sigmoid || order.Code == OrderCode.HiddenErrorRateCalculate_for_Softmax) && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem)
                    {
                        isErrorRateCalculate = true;
                    }
                    else if(order.Code == OrderCode.LayerDuplicate && order.Layer2Name == (string)orderLayer1ComboBox.SelectedItem)
                    {
                        isErrorRateCalculate = true;
                    }
                    else if ((order.Code == OrderCode.LayerInitialize && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem))
                    {
                        isErrorRateCalculate = false;
                    }
                }
                if (isErrorRateCalculate)
                {
                    interconnectionRenewalButton.Enabled = true;                    
                    cleanupBackwardProcessButton.Enabled = true;
                    biasRenewalButton.Enabled = true;
                }


                if (simulator.LayerDictionary[(string)orderLayer1ComboBox.SelectedItem].LayerType == LayerType.BPTTLayer)
                {
                    activationCalculateSigmoidButton.Enabled = false;
                    activationCalculateSoftmaxButton.Enabled = false;
                    tickInButton.Enabled = true;

                    int currentTick = 0;
                    foreach (Order order in newProcess)
                    {
                        if (order.Code == OrderCode.TickIn && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem)
                        {
                            currentTick = 0;
                            tickProgressButton.Enabled = true;
                        }
                        else if (order.Code == OrderCode.TickProgress && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem) currentTick++;
                        else if (order.Code == OrderCode.LayerInitialize && order.Layer1Name == (string)orderLayer1ComboBox.SelectedItem)
                        {
                            currentTick = 0;
                            tickProgressButton.Enabled = false;
                        }

                    }
                    if (currentTick == ((BPTTLayer)simulator.LayerDictionary[(string)orderLayer1ComboBox.SelectedItem]).Tick)
                    {
                        tickProgressButton.Enabled = false;
                        tickOutButton.Enabled = true;
                    }
                    if (isErrorRateCalculate) baseWeightRenewalButton.Enabled = true;
                }
            }            
        }
        private void orderLayer2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            layerDuplicateButton.Enabled = false;

            if (orderLayer1ComboBox.SelectedIndex > -1) layerDuplicateButton.Enabled = true;
        }
        private void orderConnection1ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            weightRenewalButton.Enabled = false;

            if (orderConnection1ComboBox.SelectedIndex > -1)
            {
                orderLayer1ComboBox.SelectedIndex = -1;
                orderLayer2ComboBox.SelectedIndex = -1;
                orderLayer2ComboBox.Enabled = false;

                orderConnection2ComboBox.Enabled = true;

                Connection selectedConnection = simulator.ConnectionDictionary[(string)orderConnection1ComboBox.SelectedItem];
                foreach(Order order in newProcess)
                {
                    if ((order.Code == OrderCode.OutputErrorRateCalculate_for_Sigmoid || order.Code == OrderCode.HiddenErrorRateCalculate_for_Sigmoid || order.Code == OrderCode.OutputErrorRateCalculate_for_Softmax || order.Code == OrderCode.HiddenErrorRateCalculate_for_Softmax) && order.Layer1Name == selectedConnection.ReceiveLayer.Name)
                    {
                        weightRenewalButton.Enabled = true;
                    }
                    if (order.Code == OrderCode.ConnectionInitialize && order.Layer1Name == selectedConnection.ReceiveLayer.Name)
                    {
                        weightRenewalButton.Enabled = false;
                    }
                }
            }
        }
        private void orderConnection2ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            connectionDuplicateButton.Enabled = false;
            transposedConnectionDuplicateButton.Enabled = false;

            if (orderConnection1ComboBox.SelectedIndex > -1)
            {
                connectionDuplicateButton.Enabled = true;
                transposedConnectionDuplicateButton.Enabled = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Process Control File(*.PRCXML)|*.PRCXML";
            saveFileDialog.InitialDirectory = Application.StartupPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                simulator.Process_Save(saveFileDialog.FileName);
            }
        }
        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Process Control File(*.PRCXML)|*.PRCXML";
            openFileDialog.Multiselect = false;
            openFileDialog.InitialDirectory = Application.StartupPath;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Current Process information will be deleted.", "Caution", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    processCancelButton_Click(sender, e);

                    simulator.Process_Load(openFileDialog.FileName);

                    ProcessRefresh();
                    ControlRefresh();
                    OrderRefresh();
                }
            }
        }
        private void exitButton_Click(object sender, EventArgs e)
        {   
            this.Close();
        }

        private void ProcessControlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Visible = true;
        }
        
        private void bpButton_Click(object sender, EventArgs e)
        {
            Order newOrder;

            string inputLayerName ="";
            string hiddenLayerName="";
            string outputLayerName="";

            string ihConnectionName="";
            string hoConnectionName="";

            if (bpInputLayerComboBox.SelectedIndex < 0 || bpHiddenLayerComboBox.SelectedIndex < 0 || bpOutputLayerComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Select all position.");
            }
            else
            {
                inputLayerName = (string)bpInputLayerComboBox.SelectedItem;
                hiddenLayerName = (string)bpHiddenLayerComboBox.SelectedItem;
                outputLayerName = (string)bpOutputLayerComboBox.SelectedItem;

                bool ihCheck = false;
                foreach(string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[inputLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[hiddenLayerName])
                    {
                        ihConnectionName = key;
                        ihCheck = true;
                    }
                }
                bool hoCheck = false;
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[hiddenLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[outputLayerName])
                    {
                        hoConnectionName = key;
                        hoCheck = true;
                    }
                }

                if(!ihCheck || !hoCheck)
                {
                    MessageBox.Show("This structure is wrong.");
                }
                else
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = inputLayerName;
                    newOrder.Code = OrderCode.ActivationInput;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = inputLayerName;
                    newOrder.Code = OrderCode.ActivationSend;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = hiddenLayerName;
                    newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = hiddenLayerName;
                    newOrder.Code = OrderCode.ActivationSend;
                    newProcess.Add(newOrder);

                    if (bpErrorSigmoidRadioButton.Checked)
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                        newProcess.Add(newOrder);
                    }
                    else if (bpErrorSoftmaxRadioButton.Checked)
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.ActivationCalculate_Softmax;
                        newProcess.Add(newOrder);
                    }

                    if (!bpTestCheckBox.Checked)
                    {
                        if (bpErrorSigmoidRadioButton.Checked)
                        {
                            newOrder = new Order();
                            newOrder.Layer1Name = outputLayerName;
                            newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Sigmoid;
                            newProcess.Add(newOrder);
                        }
                        else if (bpErrorSoftmaxRadioButton.Checked)
                        {
                            newOrder = new Order();
                            newOrder.Layer1Name = outputLayerName;
                            newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Softmax;
                            newProcess.Add(newOrder);
                        }

                        newOrder = new Order();
                        newOrder.Layer1Name = hiddenLayerName;
                        newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Sigmoid;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Connection1Name = hoConnectionName;
                        newOrder.Code = OrderCode.WeightRenewal;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Connection1Name = ihConnectionName;
                        newOrder.Code = OrderCode.WeightRenewal;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.BiasRenewal;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Layer1Name = hiddenLayerName;
                        newOrder.Code = OrderCode.BiasRenewal;
                        newProcess.Add(newOrder);
                    }
                    else
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.TestValueStore;
                        newProcess.Add(newOrder);
                    }

                    newOrder = new Order();
                    newOrder.Code = OrderCode.EndInitialize;
                    newProcess.Add(newOrder);

                    OrderRefresh();
                }
            }
        }
        private void srnButton_Click(object sender, EventArgs e)
        {
            Order newOrder;

            string inputLayerName = "";
            string contextLayerName = "";
            string hiddenLayerName = "";
            string outputLayerName = "";

            string ihConnectionName = "";
            string chConnectionName = "";
            string hoConnectionName = "";

            if (srnInputLayerComboBox.SelectedIndex < 0 || srnContextLayerComboBox.SelectedIndex < 0 || srnHiddenLayerComboBox.SelectedIndex < 0 || srnOutputLayerComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Select all position.");
            }
            else if (srnInputStimuliNameTextBox.Text == "" || srnOutputStimuliNameTextBox.Text == "")
            {
                MessageBox.Show("Insert the both of stimuli mames.");
            }
            else
            {
                inputLayerName = (string)srnInputLayerComboBox.SelectedItem;
                contextLayerName = (string)srnContextLayerComboBox.SelectedItem;
                hiddenLayerName = (string)srnHiddenLayerComboBox.SelectedItem;
                outputLayerName = (string)srnOutputLayerComboBox.SelectedItem;

                bool ihCheck = false;
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[inputLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[hiddenLayerName])
                    {
                        ihConnectionName = key;
                        ihCheck = true;
                    }
                }

                bool chCheck = false;
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[contextLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[hiddenLayerName])
                    {
                        chConnectionName = key;
                        chCheck = true;
                    }
                }

                bool hoCheck = false;
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[hiddenLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[outputLayerName])
                    {
                        hoConnectionName = key;
                        hoCheck = true;
                    }
                }

                bool chLayerCheck = false;
                if (simulator.LayerDictionary[contextLayerName].UnitCount == simulator.LayerDictionary[hiddenLayerName].UnitCount) chLayerCheck = true;

                if (!ihCheck || !chCheck || !hoCheck || !chLayerCheck)
                {
                    MessageBox.Show("This structure is wrong.");
                }
                else
                {
                    newOrder = new Order();
                    newOrder.SRNInputLayerName = inputLayerName;
                    newOrder.SRNContextLayerName = contextLayerName;
                    newOrder.SRNHiddenLayerName = hiddenLayerName;
                    newOrder.SRNOutputLayerName = outputLayerName;
                    newOrder.SRNIHConnectionName = ihConnectionName;
                    newOrder.SRNCHConnectionName = chConnectionName;
                    newOrder.SRNHOConnectionName = hoConnectionName;
                    newOrder.SRNInputPatternName = srnInputStimuliNameTextBox.Text;
                    newOrder.SRNOutputPatternName = srnOutputStimuliNameTextBox.Text;
                    if(srnErrorSigmoidRadioButton.Checked) newOrder.SRNErrorCalculation = "Sigmoid";
                    else if (srnErrorSoftmaxRadioButton.Checked) newOrder.SRNErrorCalculation = "Softmax";
                    
                    if (srnTestCheckBox.Checked) newOrder.Code = OrderCode.SRNTest;
                    else newOrder.Code = OrderCode.SRNTraining;
                    

                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Code = OrderCode.EndInitialize;
                    newProcess.Add(newOrder);

                    OrderRefresh();
                }
            }
        }
        private void bpttButton_Click(object sender, EventArgs e)
        {
            Order newOrder;

            string inputLayerName = "";
            string hiddenLayerName = "";
            string outputLayerName = "";

            string ihConnectionName = "";
            string hoConnectionName = "";

            if (bpttInputLayerComboBox.SelectedIndex < 0 || bpttHiddenLayerComboBox.SelectedIndex < 0 || bpttOutputLayerComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Select all position.");
            }
            else
            {
                inputLayerName = (string)bpttInputLayerComboBox.SelectedItem;
                hiddenLayerName = (string)bpttHiddenLayerComboBox.SelectedItem;
                outputLayerName = (string)bpttOutputLayerComboBox.SelectedItem;

                bool ihCheck = false;
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[inputLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[hiddenLayerName])
                    {
                        ihConnectionName = key;
                        ihCheck = true;
                    }
                }
                bool hoCheck = false;
                foreach (string key in simulator.ConnectionDictionary.Keys)
                {
                    if (simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[hiddenLayerName] && simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[outputLayerName])
                    {
                        hoConnectionName = key;
                        hoCheck = true;
                    }
                }
                bool bpttLayerCheck = false;
                if (simulator.LayerDictionary[(string)bpttHiddenLayerComboBox.SelectedItem].LayerType == LayerType.BPTTLayer) bpttLayerCheck = true;

                if (!ihCheck || !hoCheck || !bpttLayerCheck)
                {
                    MessageBox.Show("This structure is wrong.");
                }
                else
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = inputLayerName;
                    newOrder.Code = OrderCode.ActivationInput;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = inputLayerName;
                    newOrder.Code = OrderCode.ActivationSend;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = hiddenLayerName;
                    newOrder.Code = OrderCode.TickIn;
                    newProcess.Add(newOrder);

                    for(int progressIndex =0; progressIndex < ((BPTTLayer)simulator.LayerDictionary[(string)bpttHiddenLayerComboBox.SelectedItem]).Tick ;progressIndex++)
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = inputLayerName;
                        newOrder.Code = OrderCode.ActivationInput;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Layer1Name = inputLayerName;
                        newOrder.Code = OrderCode.ActivationSend;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Layer1Name = hiddenLayerName;
                        newOrder.Code = OrderCode.TickProgress;
                        newProcess.Add(newOrder);
                    }

                    newOrder = new Order();
                    newOrder.Layer1Name = hiddenLayerName;
                    newOrder.Code = OrderCode.TickOut;
                    newProcess.Add(newOrder);
                    
                    newOrder = new Order();
                    newOrder.Layer1Name = hiddenLayerName;
                    newOrder.Code = OrderCode.ActivationSend;
                    newProcess.Add(newOrder);

                    if (bpttErrorSigmoidRadioButton.Checked)
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                        newProcess.Add(newOrder);
                    }
                    else if (bpttErrorSoftmaxRadioButton.Checked)
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.ActivationCalculate_Softmax;
                        newProcess.Add(newOrder);
                    }


                    if (!bpttTestCheckBox.Checked)
                    {
                        if (bpttErrorSigmoidRadioButton.Checked)
                        {
                            newOrder = new Order();
                            newOrder.Layer1Name = outputLayerName;
                            newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Sigmoid;
                            newProcess.Add(newOrder);
                        }
                        else if (bpttErrorSoftmaxRadioButton.Checked)
                        {
                            newOrder = new Order();
                            newOrder.Layer1Name = outputLayerName;
                            newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Softmax;
                            newProcess.Add(newOrder);
                        }
                        newOrder = new Order();
                        newOrder.Layer1Name = hiddenLayerName;
                        newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Sigmoid;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Connection1Name = hoConnectionName;
                        newOrder.Code = OrderCode.WeightRenewal;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Connection1Name = ihConnectionName;
                        newOrder.Code = OrderCode.WeightRenewal;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.BiasRenewal;
                        newProcess.Add(newOrder);

                        newOrder = new Order();
                        newOrder.Layer1Name = hiddenLayerName;
                        newOrder.Code = OrderCode.BaseWeightRenewal;
                        newProcess.Add(newOrder);
                    }
                    else
                    {
                        newOrder = new Order();
                        newOrder.Layer1Name = outputLayerName;
                        newOrder.Code = OrderCode.TestValueStore;
                        newProcess.Add(newOrder);
                    }

                    newOrder = new Order();
                    newOrder.Code = OrderCode.EndInitialize;
                    newProcess.Add(newOrder);

                    OrderRefresh();
                }
            }
        }

        private void forwardLayerAddButton_Click(object sender, EventArgs e)
        {
            if (forwardLayerComboBox.SelectedIndex > -1)
            {
                forwardLayerListBox.Items.Add(forwardLayerComboBox.SelectedItem);
                ForwardLayerComboboxListMaker();
            }
        }
        private void forwardLastDeleteButton_Click(object sender, EventArgs e)
        {
            if(forwardLayerListBox.Items.Count > 0)
            {
                forwardLayerListBox.Items.RemoveAt(forwardLayerListBox.Items.Count - 1);
                ForwardLayerComboboxListMaker();
            }
        }
        private void forwardApplyButton_Click(object sender, EventArgs e)
        {
            Order newOrder;
            
            if (forwardLayerListBox.Items.Count <= 0)
            {
                MessageBox.Show("Insert a layer at least.");
            }
            else
            {
                if(forwardFirstLayerInputRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[0];
                    newOrder.Code = OrderCode.ActivationInput;
                    newProcess.Add(newOrder);
                }
                else if (forwardFirstLayerHiddenRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[0];
                    newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                    newProcess.Add(newOrder);
                }

                newOrder = new Order();
                newOrder.Layer1Name = (string)forwardLayerListBox.Items[0];
                newOrder.Code = OrderCode.ActivationSend;
                newProcess.Add(newOrder);

                for (int layerIndex = 1; layerIndex < forwardLayerListBox.Items.Count - 1; layerIndex++)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[layerIndex];
                    newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[layerIndex];
                    newOrder.Code = OrderCode.ActivationSend;
                    newProcess.Add(newOrder);
                }

                if (forwardLastLayerHiddenRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[forwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                    newProcess.Add(newOrder);

                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[forwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.ActivationSend;
                    newProcess.Add(newOrder);
                }
                else if(forwardLastLayerOutputSigmoidRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[forwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.ActivationCalculate_Sigmoid;
                    newProcess.Add(newOrder);
                }
                else if(forwardLastLayerOutputSoftmaxRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[forwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.ActivationCalculate_Softmax;
                    newProcess.Add(newOrder);
                }                

                if(linearTestCheckBox.Checked && !forwardLastLayerHiddenRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)forwardLayerListBox.Items[forwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.TestValueStore;
                    newProcess.Add(newOrder);
                }

                OrderRefresh();                
            }
        }

        private void ForwardLayerComboboxListMaker()
        {
            forwardLayerComboBox.Items.Clear();
            if(forwardLayerListBox.Items.Count <= 0)
            {
                foreach (string key in simulator.LayerDictionary.Keys) forwardLayerComboBox.Items.Add(key);
            }
            else
            {
                string lastLayerName = (string)forwardLayerListBox.Items[forwardLayerListBox.Items.Count - 1];
                foreach(string key in simulator.LayerDictionary[lastLayerName].SendConnectionDictionary.Keys)
                {
                    forwardLayerComboBox.Items.Add(simulator.ConnectionDictionary[key].ReceiveLayer.Name);
                }
            }
        }

        private void backwardLayerAddButton_Click(object sender, EventArgs e)
        {
            if (backwardLayerComboBox.SelectedIndex > -1)
            {
                backwardLayerListBox.Items.Add(backwardLayerComboBox.SelectedItem);
                BackwardLayerComboboxListMaker();
            }
        }
        private void backwardLastDeleteButton_Click(object sender, EventArgs e)
        {
            if (backwardLayerListBox.Items.Count > 0)
            {
                backwardLayerListBox.Items.RemoveAt(backwardLayerListBox.Items.Count - 1);
                BackwardLayerComboboxListMaker();
            }
        }
        private void backwardApplyButton_Click(object sender, EventArgs e)
        {
            Order newOrder;

            if (backwardLayerListBox.Items.Count <= 0)
            {
                MessageBox.Show("Insert a layer at least.");
            }
            else
            {
                if (backwardFirstLayerHiddenRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[0];
                    newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Sigmoid;
                    newProcess.Add(newOrder);
                }
                else if (backwardFirstLayerOutputSigmoidRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[0];
                    newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Sigmoid;
                    newProcess.Add(newOrder);
                }
                else if (backwardFirstLayerOutputSoftmaxRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[0];
                    newOrder.Code = OrderCode.OutputErrorRateCalculate_for_Softmax;
                    newProcess.Add(newOrder);
                }
                
                for (int layerIndex = 1; layerIndex < backwardLayerListBox.Items.Count - 1; layerIndex++)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[layerIndex];
                    newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Sigmoid;
                    newProcess.Add(newOrder);
                }

                if (backwardLastLayerHiddenRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[backwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.HiddenErrorRateCalculate_for_Sigmoid;
                    newProcess.Add(newOrder);                    
                }

                OrderRefresh();
            }
        }

        private void BackwardLayerComboboxListMaker()
        {
            backwardLayerComboBox.Items.Clear();
            if (backwardLayerListBox.Items.Count <= 0)
            {
                foreach (string key in simulator.LayerDictionary.Keys) backwardLayerComboBox.Items.Add(key);
            }
            else
            {
                string lastLayerName = (string)backwardLayerListBox.Items[backwardLayerListBox.Items.Count - 1];
                foreach (string key in simulator.LayerDictionary[lastLayerName].ReceiveConnectionDictionary.Keys)
                {
                    backwardLayerComboBox.Items.Add(simulator.ConnectionDictionary[key].SendLayer.Name);
                }
            }
        }

        private void backwardErrorRenewalButton_Click(object sender, EventArgs e)
        {
            Order newOrder;

            if (backwardLayerListBox.Items.Count <= 0)
            {
                MessageBox.Show("Insert a layer at least.");
            }
            else
            {

                for (int layerIndex = 0; layerIndex < backwardLayerListBox.Items.Count - 1; layerIndex++)
                {
                    string connectionName = "";

                    foreach (string key in simulator.ConnectionDictionary.Keys)
                    {
                        if (simulator.ConnectionDictionary[key].ReceiveLayer == simulator.LayerDictionary[(string)backwardLayerListBox.Items[layerIndex]] && simulator.ConnectionDictionary[key].SendLayer == simulator.LayerDictionary[(string)backwardLayerListBox.Items[layerIndex + 1]])
                        {
                            connectionName = key;
                        }
                    }

                    newOrder = new Order();
                    newOrder.Connection1Name = connectionName;
                    newOrder.Code = OrderCode.WeightRenewal;
                    newProcess.Add(newOrder);
                }

                for (int layerIndex = 0; layerIndex < backwardLayerListBox.Items.Count - 1; layerIndex++)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[layerIndex];
                    newOrder.Code = OrderCode.BiasRenewal;
                    newProcess.Add(newOrder);
                }

                if (backwardLastLayerHiddenRadioButton.Checked)
                {
                    newOrder = new Order();
                    newOrder.Layer1Name = (string)backwardLayerListBox.Items[backwardLayerListBox.Items.Count - 1];
                    newOrder.Code = OrderCode.BiasRenewal;
                    newProcess.Add(newOrder);
                }

                OrderRefresh();
            }
        }       
    }
}
