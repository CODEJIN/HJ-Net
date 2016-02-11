namespace ConnectionistModel
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.architectureButtion = new System.Windows.Forms.Button();
            this.stimuliPackButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.processButton = new System.Windows.Forms.Button();
            this.learningButton = new System.Windows.Forms.Button();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.bundleStatusLabel = new System.Windows.Forms.Label();
            this.processStatusLabel = new System.Windows.Forms.Label();
            this.stimuliPackStatusLabel = new System.Windows.Forms.Label();
            this.layerStatusLabel = new System.Windows.Forms.Label();
            this.processStatusListBox = new System.Windows.Forms.ListBox();
            this.stimuliPackStatusListBox = new System.Windows.Forms.ListBox();
            this.bundleStatusListBox = new System.Windows.Forms.ListBox();
            this.layerStatusListBox = new System.Windows.Forms.ListBox();
            this.licenseTextBox = new System.Windows.Forms.TextBox();
            this.architectureGraphViewButton = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.cclPictureBox = new System.Windows.Forms.PictureBox();
            this.batchModeButton = new System.Windows.Forms.Button();
            this.statusGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cclPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // architectureButtion
            // 
            this.architectureButtion.Location = new System.Drawing.Point(12, 12);
            this.architectureButtion.Name = "architectureButtion";
            this.architectureButtion.Size = new System.Drawing.Size(297, 40);
            this.architectureButtion.TabIndex = 0;
            this.architectureButtion.Text = "Architecture Setup";
            this.architectureButtion.UseVisualStyleBackColor = true;
            this.architectureButtion.Click += new System.EventHandler(this.architectureButtion_Click);
            // 
            // stimuliPackButton
            // 
            this.stimuliPackButton.Location = new System.Drawing.Point(12, 59);
            this.stimuliPackButton.Name = "stimuliPackButton";
            this.stimuliPackButton.Size = new System.Drawing.Size(297, 40);
            this.stimuliPackButton.TabIndex = 1;
            this.stimuliPackButton.Text = "Stimuli Pack Setup";
            this.stimuliPackButton.UseVisualStyleBackColor = true;
            this.stimuliPackButton.Click += new System.EventHandler(this.stimuliPackButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(12, 294);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(297, 40);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(12, 106);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(297, 40);
            this.processButton.TabIndex = 5;
            this.processButton.Text = "Process Setup";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // learningButton
            // 
            this.learningButton.Location = new System.Drawing.Point(12, 153);
            this.learningButton.Name = "learningButton";
            this.learningButton.Size = new System.Drawing.Size(297, 40);
            this.learningButton.TabIndex = 7;
            this.learningButton.Text = "Learning";
            this.learningButton.UseVisualStyleBackColor = true;
            this.learningButton.Click += new System.EventHandler(this.learningButton_Click);
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Controls.Add(this.bundleStatusLabel);
            this.statusGroupBox.Controls.Add(this.processStatusLabel);
            this.statusGroupBox.Controls.Add(this.stimuliPackStatusLabel);
            this.statusGroupBox.Controls.Add(this.layerStatusLabel);
            this.statusGroupBox.Controls.Add(this.processStatusListBox);
            this.statusGroupBox.Controls.Add(this.stimuliPackStatusListBox);
            this.statusGroupBox.Controls.Add(this.bundleStatusListBox);
            this.statusGroupBox.Controls.Add(this.layerStatusListBox);
            this.statusGroupBox.Location = new System.Drawing.Point(315, 12);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(380, 277);
            this.statusGroupBox.TabIndex = 11;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "Status";
            // 
            // bundleStatusLabel
            // 
            this.bundleStatusLabel.AutoSize = true;
            this.bundleStatusLabel.Location = new System.Drawing.Point(6, 144);
            this.bundleStatusLabel.Name = "bundleStatusLabel";
            this.bundleStatusLabel.Size = new System.Drawing.Size(44, 12);
            this.bundleStatusLabel.TabIndex = 15;
            this.bundleStatusLabel.Text = "Bundle";
            // 
            // processStatusLabel
            // 
            this.processStatusLabel.AutoSize = true;
            this.processStatusLabel.Location = new System.Drawing.Point(186, 144);
            this.processStatusLabel.Name = "processStatusLabel";
            this.processStatusLabel.Size = new System.Drawing.Size(52, 12);
            this.processStatusLabel.TabIndex = 16;
            this.processStatusLabel.Text = "Process";
            // 
            // stimuliPackStatusLabel
            // 
            this.stimuliPackStatusLabel.AutoSize = true;
            this.stimuliPackStatusLabel.Location = new System.Drawing.Point(188, 17);
            this.stimuliPackStatusLabel.Name = "stimuliPackStatusLabel";
            this.stimuliPackStatusLabel.Size = new System.Drawing.Size(79, 12);
            this.stimuliPackStatusLabel.TabIndex = 17;
            this.stimuliPackStatusLabel.Text = "Stimuli  Pack";
            // 
            // layerStatusLabel
            // 
            this.layerStatusLabel.AutoSize = true;
            this.layerStatusLabel.Location = new System.Drawing.Point(6, 17);
            this.layerStatusLabel.Name = "layerStatusLabel";
            this.layerStatusLabel.Size = new System.Drawing.Size(37, 12);
            this.layerStatusLabel.TabIndex = 18;
            this.layerStatusLabel.Text = "Layer";
            // 
            // processStatusListBox
            // 
            this.processStatusListBox.FormattingEnabled = true;
            this.processStatusListBox.ItemHeight = 12;
            this.processStatusListBox.Location = new System.Drawing.Point(188, 159);
            this.processStatusListBox.Name = "processStatusListBox";
            this.processStatusListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.processStatusListBox.Size = new System.Drawing.Size(178, 100);
            this.processStatusListBox.TabIndex = 11;
            // 
            // stimuliPackStatusListBox
            // 
            this.stimuliPackStatusListBox.FormattingEnabled = true;
            this.stimuliPackStatusListBox.ItemHeight = 12;
            this.stimuliPackStatusListBox.Location = new System.Drawing.Point(190, 32);
            this.stimuliPackStatusListBox.Name = "stimuliPackStatusListBox";
            this.stimuliPackStatusListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.stimuliPackStatusListBox.Size = new System.Drawing.Size(178, 100);
            this.stimuliPackStatusListBox.TabIndex = 12;
            // 
            // bundleStatusListBox
            // 
            this.bundleStatusListBox.FormattingEnabled = true;
            this.bundleStatusListBox.ItemHeight = 12;
            this.bundleStatusListBox.Location = new System.Drawing.Point(4, 159);
            this.bundleStatusListBox.Name = "bundleStatusListBox";
            this.bundleStatusListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.bundleStatusListBox.Size = new System.Drawing.Size(178, 100);
            this.bundleStatusListBox.TabIndex = 13;
            // 
            // layerStatusListBox
            // 
            this.layerStatusListBox.FormattingEnabled = true;
            this.layerStatusListBox.ItemHeight = 12;
            this.layerStatusListBox.Location = new System.Drawing.Point(6, 32);
            this.layerStatusListBox.Name = "layerStatusListBox";
            this.layerStatusListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.layerStatusListBox.Size = new System.Drawing.Size(178, 100);
            this.layerStatusListBox.TabIndex = 14;
            // 
            // licenseTextBox
            // 
            this.licenseTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.licenseTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.licenseTextBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.licenseTextBox.Location = new System.Drawing.Point(315, 293);
            this.licenseTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.licenseTextBox.Multiline = true;
            this.licenseTextBox.Name = "licenseTextBox";
            this.licenseTextBox.ReadOnly = true;
            this.licenseTextBox.Size = new System.Drawing.Size(282, 45);
            this.licenseTextBox.TabIndex = 14;
            this.licenseTextBox.Text = "This program by \"Hee-Jo You\" is licensed under\r\na Creative Commons Attribution-Sh" +
    "areAlike 4.0\r\nInternational License.";
            this.licenseTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // architectureGraphViewButton
            // 
            this.architectureGraphViewButton.Font = new System.Drawing.Font("굴림", 5F);
            this.architectureGraphViewButton.Location = new System.Drawing.Point(701, 108);
            this.architectureGraphViewButton.Name = "architectureGraphViewButton";
            this.architectureGraphViewButton.Size = new System.Drawing.Size(17, 87);
            this.architectureGraphViewButton.TabIndex = 15;
            this.architectureGraphViewButton.Text = "◀";
            this.architectureGraphViewButton.UseVisualStyleBackColor = true;
            this.architectureGraphViewButton.Click += new System.EventHandler(this.architectureGraphViewButton_Click);
            // 
            // aboutButton
            // 
            this.aboutButton.Location = new System.Drawing.Point(12, 247);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(297, 40);
            this.aboutButton.TabIndex = 16;
            this.aboutButton.Text = "About";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // cclPictureBox
            // 
            this.cclPictureBox.Image = global::ConnectionistModel.Properties.Resources.CCL;
            this.cclPictureBox.Location = new System.Drawing.Point(600, 295);
            this.cclPictureBox.Name = "cclPictureBox";
            this.cclPictureBox.Size = new System.Drawing.Size(95, 35);
            this.cclPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cclPictureBox.TabIndex = 13;
            this.cclPictureBox.TabStop = false;
            this.cclPictureBox.Click += new System.EventHandler(this.cclPictureBox_Click);
            // 
            // batchModeButton
            // 
            this.batchModeButton.Location = new System.Drawing.Point(12, 200);
            this.batchModeButton.Name = "batchModeButton";
            this.batchModeButton.Size = new System.Drawing.Size(297, 40);
            this.batchModeButton.TabIndex = 17;
            this.batchModeButton.Text = "Batch Mode";
            this.batchModeButton.UseVisualStyleBackColor = true;
            this.batchModeButton.Click += new System.EventHandler(this.batchModeButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 342);
            this.ControlBox = false;
            this.Controls.Add(this.batchModeButton);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.architectureGraphViewButton);
            this.Controls.Add(this.licenseTextBox);
            this.Controls.Add(this.cclPictureBox);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.learningButton);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.stimuliPackButton);
            this.Controls.Add(this.architectureButtion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "HJ-Net for Connectionist Modeling Ver. 1.0.3.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cclPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button architectureButtion;
        private System.Windows.Forms.Button stimuliPackButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.Button learningButton;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.Label bundleStatusLabel;
        private System.Windows.Forms.Label processStatusLabel;
        private System.Windows.Forms.Label stimuliPackStatusLabel;
        private System.Windows.Forms.Label layerStatusLabel;
        private System.Windows.Forms.ListBox processStatusListBox;
        private System.Windows.Forms.ListBox stimuliPackStatusListBox;
        private System.Windows.Forms.ListBox bundleStatusListBox;
        private System.Windows.Forms.ListBox layerStatusListBox;
        private System.Windows.Forms.PictureBox cclPictureBox;
        private System.Windows.Forms.TextBox licenseTextBox;
        private System.Windows.Forms.Button architectureGraphViewButton;
        private System.Windows.Forms.Button aboutButton;
        private System.Windows.Forms.Button batchModeButton;
    }
}

