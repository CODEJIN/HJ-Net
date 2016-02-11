namespace ConnectionistModel
{
    partial class AboutForm
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
            this.closeButton = new System.Windows.Forms.Button();
            this.msAGLTapPage = new System.Windows.Forms.TabPage();
            this.msAGLPictureBox = new System.Windows.Forms.PictureBox();
            this.msAGLLicenseDisplayTextBox = new System.Windows.Forms.TextBox();
            this.mainTabPage = new System.Windows.Forms.TabPage();
            this.cclPictureBox = new System.Windows.Forms.PictureBox();
            this.mainLicenseDisplayTextBox = new System.Windows.Forms.TextBox();
            this.licenseDisplayTapControl = new System.Windows.Forms.TabControl();
            this.mathDotNetTapPage = new System.Windows.Forms.TabPage();
            this.mathDotNetLicenseTextBox = new System.Windows.Forms.TextBox();
            this.mathNetPictureBox = new System.Windows.Forms.PictureBox();
            this.msAGLTapPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msAGLPictureBox)).BeginInit();
            this.mainTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cclPictureBox)).BeginInit();
            this.licenseDisplayTapControl.SuspendLayout();
            this.mathDotNetTapPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mathNetPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(124, 558);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(150, 30);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // msAGLTapPage
            // 
            this.msAGLTapPage.Controls.Add(this.msAGLPictureBox);
            this.msAGLTapPage.Controls.Add(this.msAGLLicenseDisplayTextBox);
            this.msAGLTapPage.Location = new System.Drawing.Point(4, 22);
            this.msAGLTapPage.Name = "msAGLTapPage";
            this.msAGLTapPage.Padding = new System.Windows.Forms.Padding(3);
            this.msAGLTapPage.Size = new System.Drawing.Size(372, 514);
            this.msAGLTapPage.TabIndex = 1;
            this.msAGLTapPage.Text = "Microsoft AGL License";
            this.msAGLTapPage.UseVisualStyleBackColor = true;
            // 
            // msAGLPictureBox
            // 
            this.msAGLPictureBox.Image = global::ConnectionistModel.Properties.Resources.MIT;
            this.msAGLPictureBox.Location = new System.Drawing.Point(271, 473);
            this.msAGLPictureBox.Name = "msAGLPictureBox";
            this.msAGLPictureBox.Size = new System.Drawing.Size(95, 35);
            this.msAGLPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.msAGLPictureBox.TabIndex = 2;
            this.msAGLPictureBox.TabStop = false;
            // 
            // msAGLLicenseDisplayTextBox
            // 
            this.msAGLLicenseDisplayTextBox.Location = new System.Drawing.Point(6, 7);
            this.msAGLLicenseDisplayTextBox.Multiline = true;
            this.msAGLLicenseDisplayTextBox.Name = "msAGLLicenseDisplayTextBox";
            this.msAGLLicenseDisplayTextBox.ReadOnly = true;
            this.msAGLLicenseDisplayTextBox.Size = new System.Drawing.Size(360, 461);
            this.msAGLLicenseDisplayTextBox.TabIndex = 1;
            // 
            // mainTabPage
            // 
            this.mainTabPage.Controls.Add(this.cclPictureBox);
            this.mainTabPage.Controls.Add(this.mainLicenseDisplayTextBox);
            this.mainTabPage.Location = new System.Drawing.Point(4, 22);
            this.mainTabPage.Name = "mainTabPage";
            this.mainTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mainTabPage.Size = new System.Drawing.Size(372, 514);
            this.mainTabPage.TabIndex = 0;
            this.mainTabPage.Text = "Program License";
            this.mainTabPage.UseVisualStyleBackColor = true;
            // 
            // cclPictureBox
            // 
            this.cclPictureBox.Image = global::ConnectionistModel.Properties.Resources.CCL;
            this.cclPictureBox.Location = new System.Drawing.Point(271, 473);
            this.cclPictureBox.Name = "cclPictureBox";
            this.cclPictureBox.Size = new System.Drawing.Size(95, 35);
            this.cclPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cclPictureBox.TabIndex = 14;
            this.cclPictureBox.TabStop = false;
            // 
            // mainLicenseDisplayTextBox
            // 
            this.mainLicenseDisplayTextBox.Location = new System.Drawing.Point(6, 6);
            this.mainLicenseDisplayTextBox.Multiline = true;
            this.mainLicenseDisplayTextBox.Name = "mainLicenseDisplayTextBox";
            this.mainLicenseDisplayTextBox.ReadOnly = true;
            this.mainLicenseDisplayTextBox.Size = new System.Drawing.Size(360, 461);
            this.mainLicenseDisplayTextBox.TabIndex = 0;
            // 
            // licenseDisplayTapControl
            // 
            this.licenseDisplayTapControl.Controls.Add(this.mainTabPage);
            this.licenseDisplayTapControl.Controls.Add(this.msAGLTapPage);
            this.licenseDisplayTapControl.Controls.Add(this.mathDotNetTapPage);
            this.licenseDisplayTapControl.Location = new System.Drawing.Point(9, 12);
            this.licenseDisplayTapControl.Name = "licenseDisplayTapControl";
            this.licenseDisplayTapControl.SelectedIndex = 0;
            this.licenseDisplayTapControl.Size = new System.Drawing.Size(380, 540);
            this.licenseDisplayTapControl.TabIndex = 2;
            // 
            // mathDotNetTapPage
            // 
            this.mathDotNetTapPage.Controls.Add(this.mathNetPictureBox);
            this.mathDotNetTapPage.Controls.Add(this.mathDotNetLicenseTextBox);
            this.mathDotNetTapPage.Location = new System.Drawing.Point(4, 22);
            this.mathDotNetTapPage.Name = "mathDotNetTapPage";
            this.mathDotNetTapPage.Padding = new System.Windows.Forms.Padding(3);
            this.mathDotNetTapPage.Size = new System.Drawing.Size(372, 514);
            this.mathDotNetTapPage.TabIndex = 2;
            this.mathDotNetTapPage.Text = "Math.NET";
            this.mathDotNetTapPage.UseVisualStyleBackColor = true;
            // 
            // mathDotNetLicenseTextBox
            // 
            this.mathDotNetLicenseTextBox.Location = new System.Drawing.Point(6, 6);
            this.mathDotNetLicenseTextBox.Multiline = true;
            this.mathDotNetLicenseTextBox.Name = "mathDotNetLicenseTextBox";
            this.mathDotNetLicenseTextBox.ReadOnly = true;
            this.mathDotNetLicenseTextBox.Size = new System.Drawing.Size(360, 461);
            this.mathDotNetLicenseTextBox.TabIndex = 2;
            // 
            // mathNetPictureBox
            // 
            this.mathNetPictureBox.Image = global::ConnectionistModel.Properties.Resources.MIT;
            this.mathNetPictureBox.Location = new System.Drawing.Point(271, 473);
            this.mathNetPictureBox.Name = "mathNetPictureBox";
            this.mathNetPictureBox.Size = new System.Drawing.Size(95, 35);
            this.mathNetPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mathNetPictureBox.TabIndex = 3;
            this.mathNetPictureBox.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 597);
            this.Controls.Add(this.licenseDisplayTapControl);
            this.Controls.Add(this.closeButton);
            this.Name = "AboutForm";
            this.Text = "About";
            this.msAGLTapPage.ResumeLayout(false);
            this.msAGLTapPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msAGLPictureBox)).EndInit();
            this.mainTabPage.ResumeLayout(false);
            this.mainTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cclPictureBox)).EndInit();
            this.licenseDisplayTapControl.ResumeLayout(false);
            this.mathDotNetTapPage.ResumeLayout(false);
            this.mathDotNetTapPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mathNetPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TabPage msAGLTapPage;
        private System.Windows.Forms.TextBox msAGLLicenseDisplayTextBox;
        private System.Windows.Forms.TabPage mainTabPage;
        private System.Windows.Forms.TextBox mainLicenseDisplayTextBox;
        private System.Windows.Forms.TabControl licenseDisplayTapControl;
        private System.Windows.Forms.PictureBox cclPictureBox;
        private System.Windows.Forms.PictureBox msAGLPictureBox;
        private System.Windows.Forms.TabPage mathDotNetTapPage;
        private System.Windows.Forms.TextBox mathDotNetLicenseTextBox;
        private System.Windows.Forms.PictureBox mathNetPictureBox;
    }
}