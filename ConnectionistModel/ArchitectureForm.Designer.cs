namespace ConnectionistModel
{
    partial class ArchitectureForm
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
            this.layerListBox = new System.Windows.Forms.ListBox();
            this.layerSetupGroupBox = new System.Windows.Forms.GroupBox();
            this.bpttUseCheckBox = new System.Windows.Forms.CheckBox();
            this.tickTextBox = new System.Windows.Forms.TextBox();
            this.tickLabel = new System.Windows.Forms.Label();
            this.cleanUpUnitAmountTextBox = new System.Windows.Forms.TextBox();
            this.cleanUpUnitAmountLabel = new System.Windows.Forms.Label();
            this.layerDeleteButton = new System.Windows.Forms.Button();
            this.layerUnitAmountTextBox = new System.Windows.Forms.TextBox();
            this.layerNameTextBox = new System.Windows.Forms.TextBox();
            this.layerUnitAmountLabel = new System.Windows.Forms.Label();
            this.layerNameLabel = new System.Windows.Forms.Label();
            this.layerAddButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.connectionSetupGroupBox = new System.Windows.Forms.GroupBox();
            this.connectionNameTextBox = new System.Windows.Forms.TextBox();
            this.connectionNameLabel = new System.Windows.Forms.Label();
            this.connectionAddButton = new System.Windows.Forms.Button();
            this.connectionToComboBox = new System.Windows.Forms.ComboBox();
            this.connectionToLabel = new System.Windows.Forms.Label();
            this.connectionFromComboBox = new System.Windows.Forms.ComboBox();
            this.connectionFromLabel = new System.Windows.Forms.Label();
            this.connectionDeleteButton = new System.Windows.Forms.Button();
            this.connectionListBox = new System.Windows.Forms.ListBox();
            this.configGroupBox = new System.Windows.Forms.GroupBox();
            this.configSubmitButton = new System.Windows.Forms.Button();
            this.learningRateTextBox = new System.Windows.Forms.TextBox();
            this.learningRateLabel = new System.Windows.Forms.Label();
            this.initialWeightRangeTextBox = new System.Windows.Forms.TextBox();
            this.initialWeightRangeLabel = new System.Windows.Forms.Label();
            this.decayRateTextBox = new System.Windows.Forms.TextBox();
            this.decayRateLabel = new System.Windows.Forms.Label();
            this.inactivationCriterionTextBox = new System.Windows.Forms.TextBox();
            this.inactivationCriterionLabel = new System.Windows.Forms.Label();
            this.activationCriterionTextBox = new System.Windows.Forms.TextBox();
            this.activationCriterionLabel = new System.Windows.Forms.Label();
            this.momentumTextBox = new System.Windows.Forms.TextBox();
            this.momentumLabel = new System.Windows.Forms.Label();
            this.structureLoadButton = new System.Windows.Forms.Button();
            this.structureSaveButton = new System.Windows.Forms.Button();
            this.layerSetupGroupBox.SuspendLayout();
            this.connectionSetupGroupBox.SuspendLayout();
            this.configGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // layerListBox
            // 
            this.layerListBox.FormattingEnabled = true;
            this.layerListBox.ItemHeight = 12;
            this.layerListBox.Location = new System.Drawing.Point(6, 20);
            this.layerListBox.Name = "layerListBox";
            this.layerListBox.Size = new System.Drawing.Size(250, 100);
            this.layerListBox.TabIndex = 999;
            this.layerListBox.TabStop = false;
            // 
            // layerSetupGroupBox
            // 
            this.layerSetupGroupBox.Controls.Add(this.bpttUseCheckBox);
            this.layerSetupGroupBox.Controls.Add(this.tickTextBox);
            this.layerSetupGroupBox.Controls.Add(this.tickLabel);
            this.layerSetupGroupBox.Controls.Add(this.cleanUpUnitAmountTextBox);
            this.layerSetupGroupBox.Controls.Add(this.cleanUpUnitAmountLabel);
            this.layerSetupGroupBox.Controls.Add(this.layerDeleteButton);
            this.layerSetupGroupBox.Controls.Add(this.layerUnitAmountTextBox);
            this.layerSetupGroupBox.Controls.Add(this.layerNameTextBox);
            this.layerSetupGroupBox.Controls.Add(this.layerUnitAmountLabel);
            this.layerSetupGroupBox.Controls.Add(this.layerNameLabel);
            this.layerSetupGroupBox.Controls.Add(this.layerAddButton);
            this.layerSetupGroupBox.Controls.Add(this.layerListBox);
            this.layerSetupGroupBox.Enabled = false;
            this.layerSetupGroupBox.Location = new System.Drawing.Point(12, 194);
            this.layerSetupGroupBox.Name = "layerSetupGroupBox";
            this.layerSetupGroupBox.Size = new System.Drawing.Size(262, 322);
            this.layerSetupGroupBox.TabIndex = 1;
            this.layerSetupGroupBox.TabStop = false;
            this.layerSetupGroupBox.Text = "Layer Setup";
            // 
            // bpttUseCheckBox
            // 
            this.bpttUseCheckBox.AutoSize = true;
            this.bpttUseCheckBox.Location = new System.Drawing.Point(210, 249);
            this.bpttUseCheckBox.Name = "bpttUseCheckBox";
            this.bpttUseCheckBox.Size = new System.Drawing.Size(46, 16);
            this.bpttUseCheckBox.TabIndex = 1003;
            this.bpttUseCheckBox.Text = "Use";
            this.bpttUseCheckBox.UseVisualStyleBackColor = true;
            this.bpttUseCheckBox.CheckedChanged += new System.EventHandler(this.bpttUseCheckBox_CheckedChanged);
            // 
            // tickTextBox
            // 
            this.tickTextBox.Enabled = false;
            this.tickTextBox.Location = new System.Drawing.Point(75, 245);
            this.tickTextBox.Name = "tickTextBox";
            this.tickTextBox.Size = new System.Drawing.Size(129, 21);
            this.tickTextBox.TabIndex = 1002;
            // 
            // tickLabel
            // 
            this.tickLabel.AutoSize = true;
            this.tickLabel.Location = new System.Drawing.Point(4, 250);
            this.tickLabel.Name = "tickLabel";
            this.tickLabel.Size = new System.Drawing.Size(65, 12);
            this.tickLabel.TabIndex = 1001;
            this.tickLabel.Text = "BPTT Tick";
            // 
            // cleanUpUnitAmountTextBox
            // 
            this.cleanUpUnitAmountTextBox.Location = new System.Drawing.Point(75, 218);
            this.cleanUpUnitAmountTextBox.Name = "cleanUpUnitAmountTextBox";
            this.cleanUpUnitAmountTextBox.Size = new System.Drawing.Size(129, 21);
            this.cleanUpUnitAmountTextBox.TabIndex = 9;
            // 
            // cleanUpUnitAmountLabel
            // 
            this.cleanUpUnitAmountLabel.AutoSize = true;
            this.cleanUpUnitAmountLabel.Location = new System.Drawing.Point(16, 221);
            this.cleanUpUnitAmountLabel.Name = "cleanUpUnitAmountLabel";
            this.cleanUpUnitAmountLabel.Size = new System.Drawing.Size(53, 12);
            this.cleanUpUnitAmountLabel.TabIndex = 1000;
            this.cleanUpUnitAmountLabel.Text = "CleanUp";
            // 
            // layerDeleteButton
            // 
            this.layerDeleteButton.Location = new System.Drawing.Point(6, 126);
            this.layerDeleteButton.Name = "layerDeleteButton";
            this.layerDeleteButton.Size = new System.Drawing.Size(250, 30);
            this.layerDeleteButton.TabIndex = 999;
            this.layerDeleteButton.TabStop = false;
            this.layerDeleteButton.Text = "Delete";
            this.layerDeleteButton.UseVisualStyleBackColor = true;
            this.layerDeleteButton.Click += new System.EventHandler(this.layerDeleteButton_Click);
            // 
            // layerUnitAmountTextBox
            // 
            this.layerUnitAmountTextBox.Location = new System.Drawing.Point(75, 190);
            this.layerUnitAmountTextBox.Name = "layerUnitAmountTextBox";
            this.layerUnitAmountTextBox.Size = new System.Drawing.Size(129, 21);
            this.layerUnitAmountTextBox.TabIndex = 8;
            // 
            // layerNameTextBox
            // 
            this.layerNameTextBox.Location = new System.Drawing.Point(75, 162);
            this.layerNameTextBox.Name = "layerNameTextBox";
            this.layerNameTextBox.Size = new System.Drawing.Size(181, 21);
            this.layerNameTextBox.TabIndex = 7;
            // 
            // layerUnitAmountLabel
            // 
            this.layerUnitAmountLabel.AutoSize = true;
            this.layerUnitAmountLabel.Location = new System.Drawing.Point(43, 193);
            this.layerUnitAmountLabel.Name = "layerUnitAmountLabel";
            this.layerUnitAmountLabel.Size = new System.Drawing.Size(26, 12);
            this.layerUnitAmountLabel.TabIndex = 3;
            this.layerUnitAmountLabel.Text = "Unit";
            // 
            // layerNameLabel
            // 
            this.layerNameLabel.AutoSize = true;
            this.layerNameLabel.Location = new System.Drawing.Point(30, 165);
            this.layerNameLabel.Name = "layerNameLabel";
            this.layerNameLabel.Size = new System.Drawing.Size(39, 12);
            this.layerNameLabel.TabIndex = 2;
            this.layerNameLabel.Text = "Name";
            // 
            // layerAddButton
            // 
            this.layerAddButton.Location = new System.Drawing.Point(6, 286);
            this.layerAddButton.Name = "layerAddButton";
            this.layerAddButton.Size = new System.Drawing.Size(250, 30);
            this.layerAddButton.TabIndex = 10;
            this.layerAddButton.Text = "Add";
            this.layerAddButton.UseVisualStyleBackColor = true;
            this.layerAddButton.Click += new System.EventHandler(this.layerAddButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(392, 522);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(160, 39);
            this.exitButton.TabIndex = 17;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // connectionSetupGroupBox
            // 
            this.connectionSetupGroupBox.Controls.Add(this.connectionNameTextBox);
            this.connectionSetupGroupBox.Controls.Add(this.connectionNameLabel);
            this.connectionSetupGroupBox.Controls.Add(this.connectionAddButton);
            this.connectionSetupGroupBox.Controls.Add(this.connectionToComboBox);
            this.connectionSetupGroupBox.Controls.Add(this.connectionToLabel);
            this.connectionSetupGroupBox.Controls.Add(this.connectionFromComboBox);
            this.connectionSetupGroupBox.Controls.Add(this.connectionFromLabel);
            this.connectionSetupGroupBox.Controls.Add(this.connectionDeleteButton);
            this.connectionSetupGroupBox.Controls.Add(this.connectionListBox);
            this.connectionSetupGroupBox.Enabled = false;
            this.connectionSetupGroupBox.Location = new System.Drawing.Point(282, 194);
            this.connectionSetupGroupBox.Name = "connectionSetupGroupBox";
            this.connectionSetupGroupBox.Size = new System.Drawing.Size(270, 322);
            this.connectionSetupGroupBox.TabIndex = 3;
            this.connectionSetupGroupBox.TabStop = false;
            this.connectionSetupGroupBox.Text = "Connection Setup";
            // 
            // connectionNameTextBox
            // 
            this.connectionNameTextBox.Location = new System.Drawing.Point(51, 162);
            this.connectionNameTextBox.Name = "connectionNameTextBox";
            this.connectionNameTextBox.Size = new System.Drawing.Size(213, 21);
            this.connectionNameTextBox.TabIndex = 11;
            // 
            // connectionNameLabel
            // 
            this.connectionNameLabel.AutoSize = true;
            this.connectionNameLabel.Location = new System.Drawing.Point(6, 165);
            this.connectionNameLabel.Name = "connectionNameLabel";
            this.connectionNameLabel.Size = new System.Drawing.Size(39, 12);
            this.connectionNameLabel.TabIndex = 13;
            this.connectionNameLabel.Text = "Name";
            // 
            // connectionAddButton
            // 
            this.connectionAddButton.Location = new System.Drawing.Point(6, 286);
            this.connectionAddButton.Name = "connectionAddButton";
            this.connectionAddButton.Size = new System.Drawing.Size(258, 30);
            this.connectionAddButton.TabIndex = 14;
            this.connectionAddButton.Text = "Add";
            this.connectionAddButton.UseVisualStyleBackColor = true;
            this.connectionAddButton.Click += new System.EventHandler(this.connectionAddButton_Click);
            // 
            // connectionToComboBox
            // 
            this.connectionToComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionToComboBox.FormattingEnabled = true;
            this.connectionToComboBox.Location = new System.Drawing.Point(50, 219);
            this.connectionToComboBox.Name = "connectionToComboBox";
            this.connectionToComboBox.Size = new System.Drawing.Size(214, 20);
            this.connectionToComboBox.TabIndex = 13;
            // 
            // connectionToLabel
            // 
            this.connectionToLabel.AutoSize = true;
            this.connectionToLabel.Location = new System.Drawing.Point(25, 222);
            this.connectionToLabel.Name = "connectionToLabel";
            this.connectionToLabel.Size = new System.Drawing.Size(20, 12);
            this.connectionToLabel.TabIndex = 10;
            this.connectionToLabel.Text = "To";
            // 
            // connectionFromComboBox
            // 
            this.connectionFromComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.connectionFromComboBox.FormattingEnabled = true;
            this.connectionFromComboBox.Location = new System.Drawing.Point(50, 189);
            this.connectionFromComboBox.Name = "connectionFromComboBox";
            this.connectionFromComboBox.Size = new System.Drawing.Size(214, 20);
            this.connectionFromComboBox.TabIndex = 12;
            // 
            // connectionFromLabel
            // 
            this.connectionFromLabel.AutoSize = true;
            this.connectionFromLabel.Location = new System.Drawing.Point(11, 192);
            this.connectionFromLabel.Name = "connectionFromLabel";
            this.connectionFromLabel.Size = new System.Drawing.Size(34, 12);
            this.connectionFromLabel.TabIndex = 8;
            this.connectionFromLabel.Text = "From";
            // 
            // connectionDeleteButton
            // 
            this.connectionDeleteButton.Location = new System.Drawing.Point(6, 126);
            this.connectionDeleteButton.Name = "connectionDeleteButton";
            this.connectionDeleteButton.Size = new System.Drawing.Size(258, 30);
            this.connectionDeleteButton.TabIndex = 999;
            this.connectionDeleteButton.TabStop = false;
            this.connectionDeleteButton.Text = "Delete";
            this.connectionDeleteButton.UseVisualStyleBackColor = true;
            this.connectionDeleteButton.Click += new System.EventHandler(this.connectionDeleteButton_Click);
            // 
            // connectionListBox
            // 
            this.connectionListBox.FormattingEnabled = true;
            this.connectionListBox.ItemHeight = 12;
            this.connectionListBox.Location = new System.Drawing.Point(6, 20);
            this.connectionListBox.Name = "connectionListBox";
            this.connectionListBox.Size = new System.Drawing.Size(258, 100);
            this.connectionListBox.TabIndex = 999;
            this.connectionListBox.TabStop = false;
            // 
            // configGroupBox
            // 
            this.configGroupBox.Controls.Add(this.configSubmitButton);
            this.configGroupBox.Controls.Add(this.learningRateTextBox);
            this.configGroupBox.Controls.Add(this.learningRateLabel);
            this.configGroupBox.Controls.Add(this.initialWeightRangeTextBox);
            this.configGroupBox.Controls.Add(this.initialWeightRangeLabel);
            this.configGroupBox.Controls.Add(this.decayRateTextBox);
            this.configGroupBox.Controls.Add(this.decayRateLabel);
            this.configGroupBox.Controls.Add(this.inactivationCriterionTextBox);
            this.configGroupBox.Controls.Add(this.inactivationCriterionLabel);
            this.configGroupBox.Controls.Add(this.activationCriterionTextBox);
            this.configGroupBox.Controls.Add(this.activationCriterionLabel);
            this.configGroupBox.Controls.Add(this.momentumTextBox);
            this.configGroupBox.Controls.Add(this.momentumLabel);
            this.configGroupBox.Location = new System.Drawing.Point(12, 12);
            this.configGroupBox.Name = "configGroupBox";
            this.configGroupBox.Size = new System.Drawing.Size(540, 176);
            this.configGroupBox.TabIndex = 4;
            this.configGroupBox.TabStop = false;
            this.configGroupBox.Text = "Simulator Config";
            // 
            // configSubmitButton
            // 
            this.configSubmitButton.Location = new System.Drawing.Point(361, 14);
            this.configSubmitButton.Name = "configSubmitButton";
            this.configSubmitButton.Size = new System.Drawing.Size(147, 156);
            this.configSubmitButton.TabIndex = 6;
            this.configSubmitButton.Text = "Submit";
            this.configSubmitButton.UseVisualStyleBackColor = true;
            this.configSubmitButton.Click += new System.EventHandler(this.configSubmitButton_Click);
            // 
            // learningRateTextBox
            // 
            this.learningRateTextBox.Location = new System.Drawing.Point(130, 149);
            this.learningRateTextBox.Name = "learningRateTextBox";
            this.learningRateTextBox.Size = new System.Drawing.Size(166, 21);
            this.learningRateTextBox.TabIndex = 5;
            this.learningRateTextBox.Text = "0.1";
            // 
            // learningRateLabel
            // 
            this.learningRateLabel.AutoSize = true;
            this.learningRateLabel.Location = new System.Drawing.Point(43, 152);
            this.learningRateLabel.Name = "learningRateLabel";
            this.learningRateLabel.Size = new System.Drawing.Size(83, 12);
            this.learningRateLabel.TabIndex = 0;
            this.learningRateLabel.Text = "Learning Rate";
            // 
            // initialWeightRangeTextBox
            // 
            this.initialWeightRangeTextBox.Location = new System.Drawing.Point(130, 122);
            this.initialWeightRangeTextBox.Name = "initialWeightRangeTextBox";
            this.initialWeightRangeTextBox.Size = new System.Drawing.Size(166, 21);
            this.initialWeightRangeTextBox.TabIndex = 4;
            this.initialWeightRangeTextBox.Text = "1.0";
            // 
            // initialWeightRangeLabel
            // 
            this.initialWeightRangeLabel.AutoSize = true;
            this.initialWeightRangeLabel.Location = new System.Drawing.Point(11, 125);
            this.initialWeightRangeLabel.Name = "initialWeightRangeLabel";
            this.initialWeightRangeLabel.Size = new System.Drawing.Size(115, 12);
            this.initialWeightRangeLabel.TabIndex = 0;
            this.initialWeightRangeLabel.Text = "Initial Weight Range";
            // 
            // decayRateTextBox
            // 
            this.decayRateTextBox.Location = new System.Drawing.Point(130, 95);
            this.decayRateTextBox.Name = "decayRateTextBox";
            this.decayRateTextBox.Size = new System.Drawing.Size(166, 21);
            this.decayRateTextBox.TabIndex = 3;
            this.decayRateTextBox.Text = "0.000001";
            // 
            // decayRateLabel
            // 
            this.decayRateLabel.AutoSize = true;
            this.decayRateLabel.Location = new System.Drawing.Point(56, 98);
            this.decayRateLabel.Name = "decayRateLabel";
            this.decayRateLabel.Size = new System.Drawing.Size(70, 12);
            this.decayRateLabel.TabIndex = 0;
            this.decayRateLabel.Text = "Decay Rate";
            // 
            // inactivationCriterionTextBox
            // 
            this.inactivationCriterionTextBox.Location = new System.Drawing.Point(130, 68);
            this.inactivationCriterionTextBox.Name = "inactivationCriterionTextBox";
            this.inactivationCriterionTextBox.Size = new System.Drawing.Size(166, 21);
            this.inactivationCriterionTextBox.TabIndex = 2;
            this.inactivationCriterionTextBox.Text = "0.4";
            // 
            // inactivationCriterionLabel
            // 
            this.inactivationCriterionLabel.AutoSize = true;
            this.inactivationCriterionLabel.Location = new System.Drawing.Point(7, 71);
            this.inactivationCriterionLabel.Name = "inactivationCriterionLabel";
            this.inactivationCriterionLabel.Size = new System.Drawing.Size(119, 12);
            this.inactivationCriterionLabel.TabIndex = 0;
            this.inactivationCriterionLabel.Text = "Inactivation Criterion";
            // 
            // activationCriterionTextBox
            // 
            this.activationCriterionTextBox.Location = new System.Drawing.Point(130, 41);
            this.activationCriterionTextBox.Name = "activationCriterionTextBox";
            this.activationCriterionTextBox.Size = new System.Drawing.Size(166, 21);
            this.activationCriterionTextBox.TabIndex = 1;
            this.activationCriterionTextBox.Text = "0.6";
            // 
            // activationCriterionLabel
            // 
            this.activationCriterionLabel.AutoSize = true;
            this.activationCriterionLabel.Location = new System.Drawing.Point(16, 44);
            this.activationCriterionLabel.Name = "activationCriterionLabel";
            this.activationCriterionLabel.Size = new System.Drawing.Size(110, 12);
            this.activationCriterionLabel.TabIndex = 0;
            this.activationCriterionLabel.Text = "Activation Criterion";
            // 
            // momentumTextBox
            // 
            this.momentumTextBox.Location = new System.Drawing.Point(130, 14);
            this.momentumTextBox.Name = "momentumTextBox";
            this.momentumTextBox.Size = new System.Drawing.Size(166, 21);
            this.momentumTextBox.TabIndex = 0;
            this.momentumTextBox.Text = "0.9";
            // 
            // momentumLabel
            // 
            this.momentumLabel.AutoSize = true;
            this.momentumLabel.Location = new System.Drawing.Point(57, 17);
            this.momentumLabel.Name = "momentumLabel";
            this.momentumLabel.Size = new System.Drawing.Size(69, 12);
            this.momentumLabel.TabIndex = 0;
            this.momentumLabel.Text = "Momentum";
            // 
            // structureLoadButton
            // 
            this.structureLoadButton.Location = new System.Drawing.Point(202, 522);
            this.structureLoadButton.Name = "structureLoadButton";
            this.structureLoadButton.Size = new System.Drawing.Size(160, 39);
            this.structureLoadButton.TabIndex = 16;
            this.structureLoadButton.Text = "Structure Load";
            this.structureLoadButton.UseVisualStyleBackColor = true;
            this.structureLoadButton.Click += new System.EventHandler(this.structureLoadButton_Click);
            // 
            // structureSaveButton
            // 
            this.structureSaveButton.Location = new System.Drawing.Point(12, 522);
            this.structureSaveButton.Name = "structureSaveButton";
            this.structureSaveButton.Size = new System.Drawing.Size(160, 39);
            this.structureSaveButton.TabIndex = 15;
            this.structureSaveButton.Text = "Structure Save";
            this.structureSaveButton.UseVisualStyleBackColor = true;
            this.structureSaveButton.Click += new System.EventHandler(this.structureSaveButton_Click);
            // 
            // ArchitectureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 573);
            this.ControlBox = false;
            this.Controls.Add(this.structureSaveButton);
            this.Controls.Add(this.structureLoadButton);
            this.Controls.Add(this.configGroupBox);
            this.Controls.Add(this.connectionSetupGroupBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.layerSetupGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ArchitectureForm";
            this.Text = "Architecture";
            this.layerSetupGroupBox.ResumeLayout(false);
            this.layerSetupGroupBox.PerformLayout();
            this.connectionSetupGroupBox.ResumeLayout(false);
            this.connectionSetupGroupBox.PerformLayout();
            this.configGroupBox.ResumeLayout(false);
            this.configGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox layerListBox;
        private System.Windows.Forms.GroupBox layerSetupGroupBox;
        private System.Windows.Forms.Button layerDeleteButton;
        private System.Windows.Forms.TextBox layerUnitAmountTextBox;
        private System.Windows.Forms.TextBox layerNameTextBox;
        private System.Windows.Forms.Label layerUnitAmountLabel;
        private System.Windows.Forms.Label layerNameLabel;
        private System.Windows.Forms.Button layerAddButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.GroupBox connectionSetupGroupBox;
        private System.Windows.Forms.ComboBox connectionToComboBox;
        private System.Windows.Forms.Label connectionToLabel;
        private System.Windows.Forms.ComboBox connectionFromComboBox;
        private System.Windows.Forms.Label connectionFromLabel;
        private System.Windows.Forms.Button connectionDeleteButton;
        private System.Windows.Forms.ListBox connectionListBox;
        private System.Windows.Forms.Button connectionAddButton;
        private System.Windows.Forms.GroupBox configGroupBox;
        private System.Windows.Forms.TextBox momentumTextBox;
        private System.Windows.Forms.Label momentumLabel;
        private System.Windows.Forms.TextBox inactivationCriterionTextBox;
        private System.Windows.Forms.Label inactivationCriterionLabel;
        private System.Windows.Forms.TextBox activationCriterionTextBox;
        private System.Windows.Forms.Label activationCriterionLabel;
        private System.Windows.Forms.Button configSubmitButton;
        private System.Windows.Forms.TextBox decayRateTextBox;
        private System.Windows.Forms.Label decayRateLabel;
        private System.Windows.Forms.TextBox initialWeightRangeTextBox;
        private System.Windows.Forms.Label initialWeightRangeLabel;
        private System.Windows.Forms.TextBox connectionNameTextBox;
        private System.Windows.Forms.Label connectionNameLabel;
        private System.Windows.Forms.Button structureLoadButton;
        private System.Windows.Forms.Button structureSaveButton;
        private System.Windows.Forms.TextBox learningRateTextBox;
        private System.Windows.Forms.Label learningRateLabel;
        private System.Windows.Forms.TextBox cleanUpUnitAmountTextBox;
        private System.Windows.Forms.Label cleanUpUnitAmountLabel;
        private System.Windows.Forms.CheckBox bpttUseCheckBox;
        private System.Windows.Forms.TextBox tickTextBox;
        private System.Windows.Forms.Label tickLabel;
    }
}