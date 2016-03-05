using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ConnectionistModel
{
    public partial class StimuliForm : Form
    {
        //이후 Target Value가 Activation Criteria, Inactivation Criteria 사이에 있으면 결과는 항상 정상적인 활성화로 인식한다는 점을 메뉴얼에 넣을것

        Simulator simulator;

        public StimuliForm()
        {
            InitializeComponent();
            this.FormClosed += StimuliForm_FormClosed;

            this.simulator = SimulatorAccessor.simulator;

            Refresh();
        }

        private void fileBrowserButton_Click(object sender, EventArgs e)
        {
            stimuliPackFileDialog.Multiselect = false;
            stimuliPackFileDialog.Filter = "Txt File (*.txt)|*.txt";
            stimuliPackFileDialog.InitialDirectory = Application.StartupPath;

            if (stimuliPackFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {   
                fileNameTextBox.Text = stimuliPackFileDialog.FileName;
                stimuliPackNameTextBox.Text = stimuliPackFileDialog.SafeFileName.Substring(0, stimuliPackFileDialog.SafeFileName.IndexOf('.'));
            }
        }

        private void stimuliPackListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stimuliPackListBox.SelectedIndex >= 0)
            {
                stimuliPackInformationRichTextBox.Text = "";

                StimuliPack stimuliPack = simulator.StimuliPackDictionary[(string)stimuliPackListBox.SelectedItem];

                stimuliPackInformationRichTextBox.Text += "Name : " + stimuliPack.Name + "\n";
                stimuliPackInformationRichTextBox.Text += "Stimuli Amount : " + stimuliPack.Count + "\n";
                for (int i = 0; i < stimuliPack.PatternCount(); i++) stimuliPackInformationRichTextBox.Text += "Pattern " + i.ToString() + ": " + stimuliPack.PatternNameList[i] + "\t\t" + stimuliPack.RepresentationSize(stimuliPack.PatternNameList[i]) + "\n";                
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (stimuliPackNameTextBox.Text.Trim() == "") MessageBox.Show("Stimuli Data pack Name is worng. Check Please.");
            else if(simulator.StimuliImport(stimuliPackNameTextBox.Text, fileNameTextBox.Text)) Refresh();
            else MessageBox.Show("Stimuli Data pack is worng. Check Please.");
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (stimuliPackListBox.SelectedIndex >= 0)
            {

                simulator.StimuliPackDictionary.Remove((string)stimuliPackListBox.SelectedItem);

                Refresh();
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void StimuliForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Visible = true;
        }

        private new void Refresh()
        {
            stimuliPackListBox.Items.Clear();
            stimuliPackInformationRichTextBox.Text = "";
            fileNameTextBox.Text = "";
            stimuliPackNameTextBox.Text = "";
            stimuliPackFileDialog.Reset();

            foreach (string key in simulator.StimuliPackDictionary.Keys) stimuliPackListBox.Items.Add(key);
        }
    }
}
