namespace KoreanRepresentationMakerBasedYou2015
{
    partial class KorWordPatternMaker
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFileBrowserButton = new System.Windows.Forms.Button();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.txtFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.makingButton = new System.Windows.Forms.Button();
            this.nonLocationFeatureUseCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtFileBrowserButton
            // 
            this.txtFileBrowserButton.Location = new System.Drawing.Point(250, 39);
            this.txtFileBrowserButton.Name = "txtFileBrowserButton";
            this.txtFileBrowserButton.Size = new System.Drawing.Size(75, 23);
            this.txtFileBrowserButton.TabIndex = 0;
            this.txtFileBrowserButton.Text = "Browser...";
            this.txtFileBrowserButton.UseVisualStyleBackColor = true;
            this.txtFileBrowserButton.Click += new System.EventHandler(this.txtFileBrowserButton_Click);
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(12, 39);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.ReadOnly = true;
            this.fileNameTextBox.Size = new System.Drawing.Size(232, 21);
            this.fileNameTextBox.TabIndex = 1;
            // 
            // txtFileDialog
            // 
            this.txtFileDialog.FileName = "openFileDialog1";
            // 
            // makingButton
            // 
            this.makingButton.Location = new System.Drawing.Point(12, 126);
            this.makingButton.Name = "makingButton";
            this.makingButton.Size = new System.Drawing.Size(313, 40);
            this.makingButton.TabIndex = 3;
            this.makingButton.Text = "Making";
            this.makingButton.UseVisualStyleBackColor = true;
            this.makingButton.Click += new System.EventHandler(this.makingButton_Click);
            // 
            // nonLocationFeatureUseCheckBox
            // 
            this.nonLocationFeatureUseCheckBox.AutoSize = true;
            this.nonLocationFeatureUseCheckBox.Location = new System.Drawing.Point(12, 88);
            this.nonLocationFeatureUseCheckBox.Name = "nonLocationFeatureUseCheckBox";
            this.nonLocationFeatureUseCheckBox.Size = new System.Drawing.Size(232, 16);
            this.nonLocationFeatureUseCheckBox.TabIndex = 4;
            this.nonLocationFeatureUseCheckBox.Text = "Use Additional Non-Location Feature";
            this.nonLocationFeatureUseCheckBox.UseVisualStyleBackColor = true;
            // 
            // KorWordPatternMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 184);
            this.Controls.Add(this.nonLocationFeatureUseCheckBox);
            this.Controls.Add(this.makingButton);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.txtFileBrowserButton);
            this.Name = "KorWordPatternMaker";
            this.Text = "Korean Word Pattern Maker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button txtFileBrowserButton;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.OpenFileDialog txtFileDialog;
        private System.Windows.Forms.Button makingButton;
        private System.Windows.Forms.CheckBox nonLocationFeatureUseCheckBox;
    }
}

