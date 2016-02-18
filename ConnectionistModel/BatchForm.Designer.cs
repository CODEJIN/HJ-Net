namespace ConnectionistModel
{
    partial class BatchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.batchDataListBox = new System.Windows.Forms.ListBox();
            this.BatchDataInformationRichTextBox = new System.Windows.Forms.RichTextBox();
            this.architectureLabel = new System.Windows.Forms.Label();
            this.architectureTextBox = new System.Windows.Forms.TextBox();
            this.architectureBrowserButton = new System.Windows.Forms.Button();
            this.processLabel = new System.Windows.Forms.Label();
            this.processTextBox = new System.Windows.Forms.TextBox();
            this.processBrowserButton = new System.Windows.Forms.Button();
            this.learningLabel = new System.Windows.Forms.Label();
            this.learningTextBox = new System.Windows.Forms.TextBox();
            this.learningBrowserButton = new System.Windows.Forms.Button();
            this.stimuliPackListBox = new System.Windows.Forms.ListBox();
            this.stimuliPackBrowserButton = new System.Windows.Forms.Button();
            this.stimuliPackTextBox = new System.Windows.Forms.TextBox();
            this.stimuliPackLabel = new System.Windows.Forms.Label();
            this.stimuliPackAddButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.processNameLabel = new System.Windows.Forms.Label();
            this.processNameTextBox = new System.Windows.Forms.TextBox();
            this.totalVariousSizeLabel = new System.Windows.Forms.Label();
            this.repeatLabel = new System.Windows.Forms.Label();
            this.stepLabel = new System.Windows.Forms.Label();
            this.endPointLabel = new System.Windows.Forms.Label();
            this.layerBundleNameLabel = new System.Windows.Forms.Label();
            this.startPointLabel = new System.Windows.Forms.Label();
            this.variableLabel = new System.Windows.Forms.Label();
            this.repeatTextBox = new System.Windows.Forms.TextBox();
            this.stepTextBox = new System.Windows.Forms.TextBox();
            this.endPointTextBox = new System.Windows.Forms.TextBox();
            this.startPointTextBox = new System.Windows.Forms.TextBox();
            this.variableDeleteButton = new System.Windows.Forms.Button();
            this.variableAddButton = new System.Windows.Forms.Button();
            this.layerBundleNameTextBox = new System.Windows.Forms.TextBox();
            this.variableComboBox = new System.Windows.Forms.ComboBox();
            this.variableListBox = new System.Windows.Forms.ListBox();
            this.stimuliPackDeleteButton = new System.Windows.Forms.Button();
            this.stimuliPackNameTextBox = new System.Windows.Forms.TextBox();
            this.batchAddButton = new System.Windows.Forms.Button();
            this.batchDataDeleteButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.learningButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // batchDataListBox
            // 
            this.batchDataListBox.FormattingEnabled = true;
            this.batchDataListBox.ItemHeight = 12;
            this.batchDataListBox.Location = new System.Drawing.Point(12, 12);
            this.batchDataListBox.Name = "batchDataListBox";
            this.batchDataListBox.Size = new System.Drawing.Size(133, 172);
            this.batchDataListBox.TabIndex = 0;
            this.batchDataListBox.SelectedIndexChanged += new System.EventHandler(this.batchDataListBox_SelectedIndexChanged);
            // 
            // BatchDataInformationRichTextBox
            // 
            this.BatchDataInformationRichTextBox.Location = new System.Drawing.Point(151, 12);
            this.BatchDataInformationRichTextBox.Name = "BatchDataInformationRichTextBox";
            this.BatchDataInformationRichTextBox.ReadOnly = true;
            this.BatchDataInformationRichTextBox.Size = new System.Drawing.Size(750, 208);
            this.BatchDataInformationRichTextBox.TabIndex = 1;
            this.BatchDataInformationRichTextBox.Text = "";
            this.BatchDataInformationRichTextBox.WordWrap = false;
            // 
            // architectureLabel
            // 
            this.architectureLabel.AutoSize = true;
            this.architectureLabel.Location = new System.Drawing.Point(6, 14);
            this.architectureLabel.Name = "architectureLabel";
            this.architectureLabel.Size = new System.Drawing.Size(72, 12);
            this.architectureLabel.TabIndex = 2;
            this.architectureLabel.Text = "Architecture";
            // 
            // architectureTextBox
            // 
            this.architectureTextBox.Location = new System.Drawing.Point(89, 11);
            this.architectureTextBox.Name = "architectureTextBox";
            this.architectureTextBox.ReadOnly = true;
            this.architectureTextBox.Size = new System.Drawing.Size(323, 21);
            this.architectureTextBox.TabIndex = 3;
            // 
            // architectureBrowserButton
            // 
            this.architectureBrowserButton.Location = new System.Drawing.Point(430, 9);
            this.architectureBrowserButton.Name = "architectureBrowserButton";
            this.architectureBrowserButton.Size = new System.Drawing.Size(75, 23);
            this.architectureBrowserButton.TabIndex = 4;
            this.architectureBrowserButton.Text = "Browser...";
            this.architectureBrowserButton.UseVisualStyleBackColor = true;
            this.architectureBrowserButton.Click += new System.EventHandler(this.architectureBrowserButton_Click);
            // 
            // processLabel
            // 
            this.processLabel.AutoSize = true;
            this.processLabel.Location = new System.Drawing.Point(26, 43);
            this.processLabel.Name = "processLabel";
            this.processLabel.Size = new System.Drawing.Size(52, 12);
            this.processLabel.TabIndex = 2;
            this.processLabel.Text = "Process";
            // 
            // processTextBox
            // 
            this.processTextBox.Location = new System.Drawing.Point(89, 40);
            this.processTextBox.Name = "processTextBox";
            this.processTextBox.ReadOnly = true;
            this.processTextBox.Size = new System.Drawing.Size(323, 21);
            this.processTextBox.TabIndex = 3;
            // 
            // processBrowserButton
            // 
            this.processBrowserButton.Location = new System.Drawing.Point(430, 38);
            this.processBrowserButton.Name = "processBrowserButton";
            this.processBrowserButton.Size = new System.Drawing.Size(75, 23);
            this.processBrowserButton.TabIndex = 4;
            this.processBrowserButton.Text = "Browser...";
            this.processBrowserButton.UseVisualStyleBackColor = true;
            this.processBrowserButton.Click += new System.EventHandler(this.processBrowserButton_Click);
            // 
            // learningLabel
            // 
            this.learningLabel.AutoSize = true;
            this.learningLabel.Location = new System.Drawing.Point(24, 72);
            this.learningLabel.Name = "learningLabel";
            this.learningLabel.Size = new System.Drawing.Size(54, 12);
            this.learningLabel.TabIndex = 2;
            this.learningLabel.Text = "Learning";
            // 
            // learningTextBox
            // 
            this.learningTextBox.Location = new System.Drawing.Point(89, 69);
            this.learningTextBox.Name = "learningTextBox";
            this.learningTextBox.ReadOnly = true;
            this.learningTextBox.Size = new System.Drawing.Size(323, 21);
            this.learningTextBox.TabIndex = 3;
            // 
            // learningBrowserButton
            // 
            this.learningBrowserButton.Location = new System.Drawing.Point(430, 67);
            this.learningBrowserButton.Name = "learningBrowserButton";
            this.learningBrowserButton.Size = new System.Drawing.Size(75, 23);
            this.learningBrowserButton.TabIndex = 4;
            this.learningBrowserButton.Text = "Browser...";
            this.learningBrowserButton.UseVisualStyleBackColor = true;
            this.learningBrowserButton.Click += new System.EventHandler(this.learningBrowserButton_Click);
            // 
            // stimuliPackListBox
            // 
            this.stimuliPackListBox.FormattingEnabled = true;
            this.stimuliPackListBox.ItemHeight = 12;
            this.stimuliPackListBox.Location = new System.Drawing.Point(89, 96);
            this.stimuliPackListBox.Name = "stimuliPackListBox";
            this.stimuliPackListBox.Size = new System.Drawing.Size(106, 76);
            this.stimuliPackListBox.TabIndex = 5;
            // 
            // stimuliPackBrowserButton
            // 
            this.stimuliPackBrowserButton.Location = new System.Drawing.Point(430, 96);
            this.stimuliPackBrowserButton.Name = "stimuliPackBrowserButton";
            this.stimuliPackBrowserButton.Size = new System.Drawing.Size(75, 23);
            this.stimuliPackBrowserButton.TabIndex = 8;
            this.stimuliPackBrowserButton.Text = "Browser...";
            this.stimuliPackBrowserButton.UseVisualStyleBackColor = true;
            this.stimuliPackBrowserButton.Click += new System.EventHandler(this.stimuliPackBrowserButton_Click);
            // 
            // stimuliPackTextBox
            // 
            this.stimuliPackTextBox.Location = new System.Drawing.Point(201, 96);
            this.stimuliPackTextBox.Name = "stimuliPackTextBox";
            this.stimuliPackTextBox.ReadOnly = true;
            this.stimuliPackTextBox.Size = new System.Drawing.Size(211, 21);
            this.stimuliPackTextBox.TabIndex = 7;
            // 
            // stimuliPackLabel
            // 
            this.stimuliPackLabel.AutoSize = true;
            this.stimuliPackLabel.Location = new System.Drawing.Point(3, 96);
            this.stimuliPackLabel.Name = "stimuliPackLabel";
            this.stimuliPackLabel.Size = new System.Drawing.Size(75, 12);
            this.stimuliPackLabel.TabIndex = 6;
            this.stimuliPackLabel.Text = "Stimuli Pack";
            // 
            // stimuliPackAddButton
            // 
            this.stimuliPackAddButton.Location = new System.Drawing.Point(430, 125);
            this.stimuliPackAddButton.Name = "stimuliPackAddButton";
            this.stimuliPackAddButton.Size = new System.Drawing.Size(75, 34);
            this.stimuliPackAddButton.TabIndex = 9;
            this.stimuliPackAddButton.Text = "Stimuli Add";
            this.stimuliPackAddButton.UseVisualStyleBackColor = true;
            this.stimuliPackAddButton.Click += new System.EventHandler(this.stimuliPackAddButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.processNameLabel);
            this.groupBox1.Controls.Add(this.processNameTextBox);
            this.groupBox1.Controls.Add(this.totalVariousSizeLabel);
            this.groupBox1.Controls.Add(this.repeatLabel);
            this.groupBox1.Controls.Add(this.stepLabel);
            this.groupBox1.Controls.Add(this.endPointLabel);
            this.groupBox1.Controls.Add(this.layerBundleNameLabel);
            this.groupBox1.Controls.Add(this.startPointLabel);
            this.groupBox1.Controls.Add(this.variableLabel);
            this.groupBox1.Controls.Add(this.repeatTextBox);
            this.groupBox1.Controls.Add(this.stepTextBox);
            this.groupBox1.Controls.Add(this.endPointTextBox);
            this.groupBox1.Controls.Add(this.startPointTextBox);
            this.groupBox1.Controls.Add(this.variableDeleteButton);
            this.groupBox1.Controls.Add(this.variableAddButton);
            this.groupBox1.Controls.Add(this.layerBundleNameTextBox);
            this.groupBox1.Controls.Add(this.variableComboBox);
            this.groupBox1.Controls.Add(this.variableListBox);
            this.groupBox1.Controls.Add(this.stimuliPackDeleteButton);
            this.groupBox1.Controls.Add(this.stimuliPackNameTextBox);
            this.groupBox1.Controls.Add(this.architectureLabel);
            this.groupBox1.Controls.Add(this.stimuliPackAddButton);
            this.groupBox1.Controls.Add(this.architectureTextBox);
            this.groupBox1.Controls.Add(this.stimuliPackBrowserButton);
            this.groupBox1.Controls.Add(this.processLabel);
            this.groupBox1.Controls.Add(this.stimuliPackTextBox);
            this.groupBox1.Controls.Add(this.architectureBrowserButton);
            this.groupBox1.Controls.Add(this.stimuliPackLabel);
            this.groupBox1.Controls.Add(this.learningLabel);
            this.groupBox1.Controls.Add(this.stimuliPackListBox);
            this.groupBox1.Controls.Add(this.processTextBox);
            this.groupBox1.Controls.Add(this.learningBrowserButton);
            this.groupBox1.Controls.Add(this.processBrowserButton);
            this.groupBox1.Controls.Add(this.learningTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 226);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(889, 247);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // processNameLabel
            // 
            this.processNameLabel.AutoSize = true;
            this.processNameLabel.Location = new System.Drawing.Point(786, 128);
            this.processNameLabel.Name = "processNameLabel";
            this.processNameLabel.Size = new System.Drawing.Size(90, 12);
            this.processNameLabel.TabIndex = 23;
            this.processNameLabel.Text = "Process Name";
            // 
            // processNameTextBox
            // 
            this.processNameTextBox.Location = new System.Drawing.Point(788, 142);
            this.processNameTextBox.Name = "processNameTextBox";
            this.processNameTextBox.Size = new System.Drawing.Size(91, 21);
            this.processNameTextBox.TabIndex = 15;
            // 
            // totalVariousSizeLabel
            // 
            this.totalVariousSizeLabel.Location = new System.Drawing.Point(779, 94);
            this.totalVariousSizeLabel.Name = "totalVariousSizeLabel";
            this.totalVariousSizeLabel.Size = new System.Drawing.Size(100, 26);
            this.totalVariousSizeLabel.TabIndex = 21;
            this.totalVariousSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // repeatLabel
            // 
            this.repeatLabel.AutoSize = true;
            this.repeatLabel.Location = new System.Drawing.Point(34, 222);
            this.repeatLabel.Name = "repeatLabel";
            this.repeatLabel.Size = new System.Drawing.Size(44, 12);
            this.repeatLabel.TabIndex = 20;
            this.repeatLabel.Text = "Repeat";
            // 
            // stepLabel
            // 
            this.stepLabel.AutoSize = true;
            this.stepLabel.Location = new System.Drawing.Point(786, 172);
            this.stepLabel.Name = "stepLabel";
            this.stepLabel.Size = new System.Drawing.Size(30, 12);
            this.stepLabel.TabIndex = 20;
            this.stepLabel.Text = "Step";
            // 
            // endPointLabel
            // 
            this.endPointLabel.AutoSize = true;
            this.endPointLabel.Location = new System.Drawing.Point(669, 172);
            this.endPointLabel.Name = "endPointLabel";
            this.endPointLabel.Size = new System.Drawing.Size(59, 12);
            this.endPointLabel.TabIndex = 19;
            this.endPointLabel.Text = "End Point";
            // 
            // layerBundleNameLabel
            // 
            this.layerBundleNameLabel.AutoSize = true;
            this.layerBundleNameLabel.Location = new System.Drawing.Point(669, 128);
            this.layerBundleNameLabel.Name = "layerBundleNameLabel";
            this.layerBundleNameLabel.Size = new System.Drawing.Size(120, 12);
            this.layerBundleNameLabel.TabIndex = 18;
            this.layerBundleNameLabel.Text = "Layer/Bundle Name";
            // 
            // startPointLabel
            // 
            this.startPointLabel.AutoSize = true;
            this.startPointLabel.Location = new System.Drawing.Point(552, 172);
            this.startPointLabel.Name = "startPointLabel";
            this.startPointLabel.Size = new System.Drawing.Size(62, 12);
            this.startPointLabel.TabIndex = 18;
            this.startPointLabel.Text = "Start Point";
            // 
            // variableLabel
            // 
            this.variableLabel.AutoSize = true;
            this.variableLabel.Location = new System.Drawing.Point(552, 128);
            this.variableLabel.Name = "variableLabel";
            this.variableLabel.Size = new System.Drawing.Size(51, 12);
            this.variableLabel.TabIndex = 18;
            this.variableLabel.Text = "Variable";
            // 
            // repeatTextBox
            // 
            this.repeatTextBox.Location = new System.Drawing.Point(89, 219);
            this.repeatTextBox.Name = "repeatTextBox";
            this.repeatTextBox.Size = new System.Drawing.Size(91, 21);
            this.repeatTextBox.TabIndex = 100;
            this.repeatTextBox.Text = "1";
            this.repeatTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // stepTextBox
            // 
            this.stepTextBox.Location = new System.Drawing.Point(788, 187);
            this.stepTextBox.Name = "stepTextBox";
            this.stepTextBox.Size = new System.Drawing.Size(90, 21);
            this.stepTextBox.TabIndex = 18;
            this.stepTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // endPointTextBox
            // 
            this.endPointTextBox.Location = new System.Drawing.Point(671, 187);
            this.endPointTextBox.Name = "endPointTextBox";
            this.endPointTextBox.Size = new System.Drawing.Size(90, 21);
            this.endPointTextBox.TabIndex = 17;
            this.endPointTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // startPointTextBox
            // 
            this.startPointTextBox.Location = new System.Drawing.Point(554, 187);
            this.startPointTextBox.Name = "startPointTextBox";
            this.startPointTextBox.Size = new System.Drawing.Size(90, 21);
            this.startPointTextBox.TabIndex = 16;
            this.startPointTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // variableDeleteButton
            // 
            this.variableDeleteButton.Location = new System.Drawing.Point(554, 96);
            this.variableDeleteButton.Name = "variableDeleteButton";
            this.variableDeleteButton.Size = new System.Drawing.Size(204, 23);
            this.variableDeleteButton.TabIndex = 25;
            this.variableDeleteButton.Text = "Delete";
            this.variableDeleteButton.UseVisualStyleBackColor = true;
            this.variableDeleteButton.Click += new System.EventHandler(this.variableDeleteButton_Click);
            // 
            // variableAddButton
            // 
            this.variableAddButton.Location = new System.Drawing.Point(727, 214);
            this.variableAddButton.Name = "variableAddButton";
            this.variableAddButton.Size = new System.Drawing.Size(151, 23);
            this.variableAddButton.TabIndex = 19;
            this.variableAddButton.Text = "Add";
            this.variableAddButton.UseVisualStyleBackColor = true;
            this.variableAddButton.Click += new System.EventHandler(this.variableAddButton_Click);
            // 
            // layerBundleNameTextBox
            // 
            this.layerBundleNameTextBox.Location = new System.Drawing.Point(671, 143);
            this.layerBundleNameTextBox.Name = "layerBundleNameTextBox";
            this.layerBundleNameTextBox.Size = new System.Drawing.Size(90, 21);
            this.layerBundleNameTextBox.TabIndex = 14;
            // 
            // variableComboBox
            // 
            this.variableComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.variableComboBox.FormattingEnabled = true;
            this.variableComboBox.Items.AddRange(new object[] {
            "Layer-Unit",
            "Layer-DamagedSD",
            "Bundle-DamagedSD",
            "Learning Rate",
            "Initial Weight"});
            this.variableComboBox.Location = new System.Drawing.Point(553, 143);
            this.variableComboBox.Name = "variableComboBox";
            this.variableComboBox.Size = new System.Drawing.Size(108, 20);
            this.variableComboBox.TabIndex = 13;
            this.variableComboBox.SelectedIndexChanged += new System.EventHandler(this.variableComboBox_SelectedIndexChanged);
            // 
            // variableListBox
            // 
            this.variableListBox.FormattingEnabled = true;
            this.variableListBox.HorizontalScrollbar = true;
            this.variableListBox.ItemHeight = 12;
            this.variableListBox.Location = new System.Drawing.Point(554, 14);
            this.variableListBox.Name = "variableListBox";
            this.variableListBox.Size = new System.Drawing.Size(325, 76);
            this.variableListBox.TabIndex = 12;
            // 
            // stimuliPackDeleteButton
            // 
            this.stimuliPackDeleteButton.Location = new System.Drawing.Point(89, 178);
            this.stimuliPackDeleteButton.Name = "stimuliPackDeleteButton";
            this.stimuliPackDeleteButton.Size = new System.Drawing.Size(106, 23);
            this.stimuliPackDeleteButton.TabIndex = 11;
            this.stimuliPackDeleteButton.Text = "Delete";
            this.stimuliPackDeleteButton.UseVisualStyleBackColor = true;
            this.stimuliPackDeleteButton.Click += new System.EventHandler(this.stimuliPackDeleteButton_Click);
            // 
            // stimuliPackNameTextBox
            // 
            this.stimuliPackNameTextBox.Location = new System.Drawing.Point(312, 123);
            this.stimuliPackNameTextBox.Name = "stimuliPackNameTextBox";
            this.stimuliPackNameTextBox.Size = new System.Drawing.Size(100, 21);
            this.stimuliPackNameTextBox.TabIndex = 10;
            // 
            // batchAddButton
            // 
            this.batchAddButton.Location = new System.Drawing.Point(147, 485);
            this.batchAddButton.Name = "batchAddButton";
            this.batchAddButton.Size = new System.Drawing.Size(120, 40);
            this.batchAddButton.TabIndex = 11;
            this.batchAddButton.Text = "Add";
            this.batchAddButton.UseVisualStyleBackColor = true;
            this.batchAddButton.Click += new System.EventHandler(this.batchAddButton_Click);
            // 
            // batchDataDeleteButton
            // 
            this.batchDataDeleteButton.Location = new System.Drawing.Point(12, 190);
            this.batchDataDeleteButton.Name = "batchDataDeleteButton";
            this.batchDataDeleteButton.Size = new System.Drawing.Size(133, 30);
            this.batchDataDeleteButton.TabIndex = 12;
            this.batchDataDeleteButton.Text = "Delete";
            this.batchDataDeleteButton.UseVisualStyleBackColor = true;
            this.batchDataDeleteButton.Click += new System.EventHandler(this.batchDataDeleteButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(771, 485);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(120, 40);
            this.exitButton.TabIndex = 13;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // learningButton
            // 
            this.learningButton.Location = new System.Drawing.Point(355, 485);
            this.learningButton.Name = "learningButton";
            this.learningButton.Size = new System.Drawing.Size(120, 40);
            this.learningButton.TabIndex = 12;
            this.learningButton.Text = "Learning";
            this.learningButton.UseVisualStyleBackColor = true;
            this.learningButton.Click += new System.EventHandler(this.learningButton_Click);
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 537);
            this.Controls.Add(this.learningButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.batchDataDeleteButton);
            this.Controls.Add(this.batchAddButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BatchDataInformationRichTextBox);
            this.Controls.Add(this.batchDataListBox);
            this.Name = "BatchForm";
            this.Text = "Batch Mode";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox batchDataListBox;
        private System.Windows.Forms.RichTextBox BatchDataInformationRichTextBox;
        private System.Windows.Forms.Label architectureLabel;
        private System.Windows.Forms.TextBox architectureTextBox;
        private System.Windows.Forms.Button architectureBrowserButton;
        private System.Windows.Forms.Label processLabel;
        private System.Windows.Forms.TextBox processTextBox;
        private System.Windows.Forms.Button processBrowserButton;
        private System.Windows.Forms.Label learningLabel;
        private System.Windows.Forms.TextBox learningTextBox;
        private System.Windows.Forms.Button learningBrowserButton;
        private System.Windows.Forms.ListBox stimuliPackListBox;
        private System.Windows.Forms.Button stimuliPackBrowserButton;
        private System.Windows.Forms.TextBox stimuliPackTextBox;
        private System.Windows.Forms.Label stimuliPackLabel;
        private System.Windows.Forms.Button stimuliPackAddButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button batchAddButton;
        private System.Windows.Forms.TextBox stimuliPackNameTextBox;
        private System.Windows.Forms.Button stimuliPackDeleteButton;
        private System.Windows.Forms.Button batchDataDeleteButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button learningButton;
        private System.Windows.Forms.Label stepLabel;
        private System.Windows.Forms.Label endPointLabel;
        private System.Windows.Forms.Label layerBundleNameLabel;
        private System.Windows.Forms.Label startPointLabel;
        private System.Windows.Forms.Label variableLabel;
        private System.Windows.Forms.TextBox stepTextBox;
        private System.Windows.Forms.TextBox endPointTextBox;
        private System.Windows.Forms.TextBox startPointTextBox;
        private System.Windows.Forms.Button variableDeleteButton;
        private System.Windows.Forms.Button variableAddButton;
        private System.Windows.Forms.TextBox layerBundleNameTextBox;
        private System.Windows.Forms.ComboBox variableComboBox;
        private System.Windows.Forms.ListBox variableListBox;
        private System.Windows.Forms.Label totalVariousSizeLabel;
        private System.Windows.Forms.Label repeatLabel;
        private System.Windows.Forms.TextBox repeatTextBox;
        private System.Windows.Forms.Label processNameLabel;
        private System.Windows.Forms.TextBox processNameTextBox;
    }
}