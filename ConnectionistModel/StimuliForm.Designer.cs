namespace ConnectionistModel
{
    partial class StimuliForm
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
            this.stimuliPackFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.stimuliPackListBox = new System.Windows.Forms.ListBox();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.stimuliPackInformationRichTextBox = new System.Windows.Forms.RichTextBox();
            this.fileBrowserButton = new System.Windows.Forms.Button();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.insertButton = new System.Windows.Forms.Button();
            this.stimuliPackNameTextBox = new System.Windows.Forms.TextBox();
            this.stimuliPackNameLabel = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stimuliPackFileDialog
            // 
            this.stimuliPackFileDialog.FileName = "openFileDialog1";
            // 
            // stimuliPackListBox
            // 
            this.stimuliPackListBox.FormattingEnabled = true;
            this.stimuliPackListBox.ItemHeight = 12;
            this.stimuliPackListBox.Location = new System.Drawing.Point(12, 12);
            this.stimuliPackListBox.Name = "stimuliPackListBox";
            this.stimuliPackListBox.Size = new System.Drawing.Size(114, 160);
            this.stimuliPackListBox.TabIndex = 0;
            this.stimuliPackListBox.SelectedIndexChanged += new System.EventHandler(this.stimuliPackListBox_SelectedIndexChanged);
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(132, 223);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.ReadOnly = true;
            this.fileNameTextBox.Size = new System.Drawing.Size(176, 21);
            this.fileNameTextBox.TabIndex = 1;
            // 
            // stimuliPackInformationRichTextBox
            // 
            this.stimuliPackInformationRichTextBox.Location = new System.Drawing.Point(132, 12);
            this.stimuliPackInformationRichTextBox.Name = "stimuliPackInformationRichTextBox";
            this.stimuliPackInformationRichTextBox.ReadOnly = true;
            this.stimuliPackInformationRichTextBox.Size = new System.Drawing.Size(263, 196);
            this.stimuliPackInformationRichTextBox.TabIndex = 2;
            this.stimuliPackInformationRichTextBox.Text = "";
            // 
            // fileBrowserButton
            // 
            this.fileBrowserButton.Location = new System.Drawing.Point(320, 221);
            this.fileBrowserButton.Name = "fileBrowserButton";
            this.fileBrowserButton.Size = new System.Drawing.Size(75, 23);
            this.fileBrowserButton.TabIndex = 3;
            this.fileBrowserButton.Text = "Browser...";
            this.fileBrowserButton.UseVisualStyleBackColor = true;
            this.fileBrowserButton.Click += new System.EventHandler(this.fileBrowserButton_Click);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(101, 226);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(25, 12);
            this.fileNameLabel.TabIndex = 4;
            this.fileNameLabel.Text = "File";
            // 
            // insertButton
            // 
            this.insertButton.Location = new System.Drawing.Point(244, 249);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(151, 39);
            this.insertButton.TabIndex = 5;
            this.insertButton.Text = "Insert";
            this.insertButton.UseVisualStyleBackColor = true;
            this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
            // 
            // stimuliPackNameTextBox
            // 
            this.stimuliPackNameTextBox.Location = new System.Drawing.Point(132, 249);
            this.stimuliPackNameTextBox.Name = "stimuliPackNameTextBox";
            this.stimuliPackNameTextBox.Size = new System.Drawing.Size(106, 21);
            this.stimuliPackNameTextBox.TabIndex = 6;
            // 
            // stimuliPackNameLabel
            // 
            this.stimuliPackNameLabel.AutoSize = true;
            this.stimuliPackNameLabel.Location = new System.Drawing.Point(55, 252);
            this.stimuliPackNameLabel.Name = "stimuliPackNameLabel";
            this.stimuliPackNameLabel.Size = new System.Drawing.Size(71, 12);
            this.stimuliPackNameLabel.TabIndex = 4;
            this.stimuliPackNameLabel.Text = "Pack Name";
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(12, 178);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(114, 30);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(106, 303);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(194, 46);
            this.exitButton.TabIndex = 8;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // StimuliForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 359);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.stimuliPackNameTextBox);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.stimuliPackNameLabel);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.fileBrowserButton);
            this.Controls.Add(this.stimuliPackInformationRichTextBox);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.stimuliPackListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StimuliForm";
            this.Text = "Stimuli Making";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog stimuliPackFileDialog;
        private System.Windows.Forms.ListBox stimuliPackListBox;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.RichTextBox stimuliPackInformationRichTextBox;
        private System.Windows.Forms.Button fileBrowserButton;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button insertButton;
        private System.Windows.Forms.TextBox stimuliPackNameTextBox;
        private System.Windows.Forms.Label stimuliPackNameLabel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button exitButton;
    }
}