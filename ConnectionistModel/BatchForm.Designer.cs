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
            this.label1 = new System.Windows.Forms.Label();
            this.architectureTextBox = new System.Windows.Forms.TextBox();
            this.architectureBrowserButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.processTextBox = new System.Windows.Forms.TextBox();
            this.processBrowserButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.learningTextBox = new System.Windows.Forms.TextBox();
            this.learningBrowserButton = new System.Windows.Forms.Button();
            this.stimuliPackListBox = new System.Windows.Forms.ListBox();
            this.stimuliPackBrowserButton = new System.Windows.Forms.Button();
            this.stimuliPackTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.stimuliPackAddButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stimuliPackDeleteButton = new System.Windows.Forms.Button();
            this.stimuliPackNameTextBox = new System.Windows.Forms.TextBox();
            this.batchAddButton = new System.Windows.Forms.Button();
            this.batchDataDeleteButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.currentResetButton = new System.Windows.Forms.Button();
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
            this.batchDataListBox.Size = new System.Drawing.Size(133, 208);
            this.batchDataListBox.TabIndex = 0;
            this.batchDataListBox.SelectedIndexChanged += new System.EventHandler(this.batchDataListBox_SelectedIndexChanged);
            // 
            // BatchDataInformationRichTextBox
            // 
            this.BatchDataInformationRichTextBox.Location = new System.Drawing.Point(151, 12);
            this.BatchDataInformationRichTextBox.Name = "BatchDataInformationRichTextBox";
            this.BatchDataInformationRichTextBox.ReadOnly = true;
            this.BatchDataInformationRichTextBox.Size = new System.Drawing.Size(390, 244);
            this.BatchDataInformationRichTextBox.TabIndex = 1;
            this.BatchDataInformationRichTextBox.Text = "";
            this.BatchDataInformationRichTextBox.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Architecture";
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Process";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Learning";
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Stimuli Pack";
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
            this.groupBox1.Controls.Add(this.stimuliPackDeleteButton);
            this.groupBox1.Controls.Add(this.stimuliPackNameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.stimuliPackAddButton);
            this.groupBox1.Controls.Add(this.architectureTextBox);
            this.groupBox1.Controls.Add(this.stimuliPackBrowserButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.stimuliPackTextBox);
            this.groupBox1.Controls.Add(this.architectureBrowserButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.stimuliPackListBox);
            this.groupBox1.Controls.Add(this.processTextBox);
            this.groupBox1.Controls.Add(this.learningBrowserButton);
            this.groupBox1.Controls.Add(this.processBrowserButton);
            this.groupBox1.Controls.Add(this.learningTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 262);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(529, 211);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // stimuliPackDeleteButton
            // 
            this.stimuliPackDeleteButton.Location = new System.Drawing.Point(89, 182);
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
            this.batchAddButton.Location = new System.Drawing.Point(13, 479);
            this.batchAddButton.Name = "batchAddButton";
            this.batchAddButton.Size = new System.Drawing.Size(120, 50);
            this.batchAddButton.TabIndex = 11;
            this.batchAddButton.Text = "Add";
            this.batchAddButton.UseVisualStyleBackColor = true;
            this.batchAddButton.Click += new System.EventHandler(this.batchAddButton_Click);
            // 
            // batchDataDeleteButton
            // 
            this.batchDataDeleteButton.Location = new System.Drawing.Point(12, 226);
            this.batchDataDeleteButton.Name = "batchDataDeleteButton";
            this.batchDataDeleteButton.Size = new System.Drawing.Size(133, 30);
            this.batchDataDeleteButton.TabIndex = 12;
            this.batchDataDeleteButton.Text = "Delete";
            this.batchDataDeleteButton.UseVisualStyleBackColor = true;
            this.batchDataDeleteButton.Click += new System.EventHandler(this.batchDataDeleteButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(421, 479);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(120, 50);
            this.exitButton.TabIndex = 13;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // currentResetButton
            // 
            this.currentResetButton.Location = new System.Drawing.Point(285, 479);
            this.currentResetButton.Name = "currentResetButton";
            this.currentResetButton.Size = new System.Drawing.Size(120, 50);
            this.currentResetButton.TabIndex = 14;
            this.currentResetButton.Text = "Current Reset";
            this.currentResetButton.UseVisualStyleBackColor = true;
            this.currentResetButton.Click += new System.EventHandler(this.currentResetButton_Click);
            // 
            // learningButton
            // 
            this.learningButton.Location = new System.Drawing.Point(149, 479);
            this.learningButton.Name = "learningButton";
            this.learningButton.Size = new System.Drawing.Size(120, 50);
            this.learningButton.TabIndex = 12;
            this.learningButton.Text = "Learning";
            this.learningButton.UseVisualStyleBackColor = true;
            this.learningButton.Click += new System.EventHandler(this.learningButton_Click);
            // 
            // BatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 537);
            this.Controls.Add(this.learningButton);
            this.Controls.Add(this.currentResetButton);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox architectureTextBox;
        private System.Windows.Forms.Button architectureBrowserButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox processTextBox;
        private System.Windows.Forms.Button processBrowserButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox learningTextBox;
        private System.Windows.Forms.Button learningBrowserButton;
        private System.Windows.Forms.ListBox stimuliPackListBox;
        private System.Windows.Forms.Button stimuliPackBrowserButton;
        private System.Windows.Forms.TextBox stimuliPackTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button stimuliPackAddButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button batchAddButton;
        private System.Windows.Forms.TextBox stimuliPackNameTextBox;
        private System.Windows.Forms.Button stimuliPackDeleteButton;
        private System.Windows.Forms.Button batchDataDeleteButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button currentResetButton;
        private System.Windows.Forms.Button learningButton;
    }
}