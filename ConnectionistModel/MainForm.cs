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
    public partial class MainForm : Form
    {
        Simulator simulator;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosed += MainForm_FormClosed;

            this.simulator = SimulatorAccessor.simulator;

            ArchitectureGraphAccessor.architectureGraphViewer.Dock = DockStyle.Fill;            
            ArchitectureGraphAccessor.architectureGraphViewer.Graph = ArchitectureGraphAccessor.architectureGraph;
            ArchitectureGraphAccessor.architectureGraphViewer.Graph.Attr.LayerDirection = Microsoft.Msagl.Drawing.LayerDirection.BT;
            ArchitectureGraphAccessor.architectureGraphForm.Controls.Add(ArchitectureGraphAccessor.architectureGraphViewer);            
            ArchitectureGraphAccessor.architectureGraphForm.Show();

            ArchitectureGraphAccessor.architectureGraphViewer.Width = 400;
            ArchitectureGraphAccessor.architectureGraphViewer.Height = 600;
            ArchitectureGraphAccessor.architectureGraphForm.Width = 400;
            ArchitectureGraphAccessor.architectureGraphForm.Height = 600;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void architectureButtion_Click(object sender, EventArgs e)
        {
            ArchitectureForm architectureForm = new ArchitectureForm();
            architectureForm.Owner = this;
            architectureForm.Show();
            this.Visible = false;
        }

        private void stimuliPackButton_Click(object sender, EventArgs e)
        {
            StimuliForm stimuliForm = new StimuliForm();
            stimuliForm.Owner = this;
            stimuliForm.Show();
            this.Visible = false;
        }

        private void processButton_Click(object sender, EventArgs e)
        {
            ProcessControlForm processControlForm = new ProcessControlForm();
            processControlForm.Owner = this;
            processControlForm.Show();
            this.Visible = false;
        }

        private void learningButton_Click(object sender, EventArgs e)
        {
            LearningForm learningForm = new LearningForm();
            learningForm.Owner = this;
            learningForm.Show();
            this.Visible = false;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_VisibleChanged(object sender, System.EventArgs e)
        {
            layerStatusListBox.Items.Clear();
            connectionStatusListBox.Items.Clear();
            stimuliPackStatusListBox.Items.Clear();
            processStatusListBox.Items.Clear();

            foreach (string key in simulator.LayerDictionary.Keys) layerStatusListBox.Items.Add(key + "(" + simulator.LayerDictionary[key].UnitCount + ")");
            foreach (string key in simulator.ConnectionDictionary.Keys) connectionStatusListBox.Items.Add(key + "(" + simulator.ConnectionDictionary[key].SendLayer.Name + " -> " + simulator.ConnectionDictionary[key].ReceiveLayer.Name + ")");

            foreach (string key in simulator.ProcessDictionary.Keys) processStatusListBox.Items.Add(key);
            foreach (string key in simulator.StimuliPackDictionary.Keys) stimuliPackStatusListBox.Items.Add(key);
        }

        void licenseTextBox_Click(object sender, System.EventArgs e)
        {
            CCL_Link();
        }

        private void cclPictureBox_Click(object sender, EventArgs e)
        {
            CCL_Link();
        }

        public void CCL_Link()
        {
            try
            {
                System.Diagnostics.Process.Start("https://creativecommons.org/licenses/by-sa/4.0/");
            }
            catch { }
        }

        private void architectureGraphViewButton_Click(object sender, EventArgs e)
        {
            ArchitectureGraphAccessor.architectureGraphForm.Visible = !ArchitectureGraphAccessor.architectureGraphForm.Visible;
            if (ArchitectureGraphAccessor.architectureGraphForm.Visible)
            {
                architectureGraphViewButton.Text = "◀";
                ArchitectureGraphAccessor.architectureGraphForm.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            }
            else architectureGraphViewButton.Text = "▶";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ArchitectureGraphAccessor.architectureGraphForm.Location = new Point(Location.X + this.Width, this.Location.Y);
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Owner = this;
            aboutForm.Show();
            this.Visible = false;
        }

        private void batchModeButton_Click(object sender, EventArgs e)
        {
            ArchitectureGraphAccessor.architectureGraphForm.Visible = false;
            architectureGraphViewButton.Text = "▶";

            BatchForm batchForm = new BatchForm();
            batchForm.Owner = this;
            batchForm.Show();
            this.Visible = false;
        }
    }
}
