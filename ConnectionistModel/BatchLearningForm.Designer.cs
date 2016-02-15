namespace ConnectionistModel
{
    partial class BatchLearningForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.testDisplayChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.yAxisBetweenLabel = new System.Windows.Forms.Label();
            this.minYAxisTestDisplayTextBox = new System.Windows.Forms.TextBox();
            this.useTimeStampCheckBox = new System.Windows.Forms.CheckBox();
            this.maxYAxisTestDisplayTextBox = new System.Windows.Forms.TextBox();
            this.testDisplayButton = new System.Windows.Forms.Button();
            this.testDisplayModeComboBox = new System.Windows.Forms.ComboBox();
            this.currentStimulusTextBox = new System.Windows.Forms.TextBox();
            this.currentStimulusLabel = new System.Windows.Forms.Label();
            this.currentStimuliPackTextBox = new System.Windows.Forms.TextBox();
            this.currentProcessTextBox = new System.Windows.Forms.TextBox();
            this.currentStimuliPackLabel = new System.Windows.Forms.Label();
            this.currentProcessLabel = new System.Windows.Forms.Label();
            this.currentEpochTextBox = new System.Windows.Forms.TextBox();
            this.yAxisDisplayLabel = new System.Windows.Forms.Label();
            this.testDisplayModeLabel = new System.Windows.Forms.Label();
            this.currentEpochLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.saveLayerActivationCheckBox = new System.Windows.Forms.CheckBox();
            this.saveWeightCheckBox = new System.Windows.Forms.CheckBox();
            this.BatchDataListBox = new System.Windows.Forms.ListBox();
            this.timeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.testDisplayChart)).BeginInit();
            this.statusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // testDisplayChart
            // 
            chartArea1.Name = "ChartArea1";
            this.testDisplayChart.ChartAreas.Add(chartArea1);
            this.testDisplayChart.Location = new System.Drawing.Point(6, 20);
            this.testDisplayChart.Name = "testDisplayChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.IsVisibleInLegend = false;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            this.testDisplayChart.Series.Add(series1);
            this.testDisplayChart.Size = new System.Drawing.Size(696, 313);
            this.testDisplayChart.TabIndex = 1;
            this.testDisplayChart.Text = "chart1";
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Controls.Add(this.timeLabel);
            this.statusGroupBox.Controls.Add(this.yAxisBetweenLabel);
            this.statusGroupBox.Controls.Add(this.minYAxisTestDisplayTextBox);
            this.statusGroupBox.Controls.Add(this.useTimeStampCheckBox);
            this.statusGroupBox.Controls.Add(this.maxYAxisTestDisplayTextBox);
            this.statusGroupBox.Controls.Add(this.testDisplayButton);
            this.statusGroupBox.Controls.Add(this.testDisplayModeComboBox);
            this.statusGroupBox.Controls.Add(this.currentStimulusTextBox);
            this.statusGroupBox.Controls.Add(this.currentStimulusLabel);
            this.statusGroupBox.Controls.Add(this.currentStimuliPackTextBox);
            this.statusGroupBox.Controls.Add(this.currentProcessTextBox);
            this.statusGroupBox.Controls.Add(this.currentStimuliPackLabel);
            this.statusGroupBox.Controls.Add(this.currentProcessLabel);
            this.statusGroupBox.Controls.Add(this.currentEpochTextBox);
            this.statusGroupBox.Controls.Add(this.yAxisDisplayLabel);
            this.statusGroupBox.Controls.Add(this.testDisplayModeLabel);
            this.statusGroupBox.Controls.Add(this.currentEpochLabel);
            this.statusGroupBox.Controls.Add(this.testDisplayChart);
            this.statusGroupBox.Location = new System.Drawing.Point(12, 12);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(878, 348);
            this.statusGroupBox.TabIndex = 2;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "Status";
            // 
            // yAxisBetweenLabel
            // 
            this.yAxisBetweenLabel.AutoSize = true;
            this.yAxisBetweenLabel.Location = new System.Drawing.Point(782, 79);
            this.yAxisBetweenLabel.Name = "yAxisBetweenLabel";
            this.yAxisBetweenLabel.Size = new System.Drawing.Size(14, 12);
            this.yAxisBetweenLabel.TabIndex = 12;
            this.yAxisBetweenLabel.Text = "~";
            // 
            // minYAxisTestDisplayTextBox
            // 
            this.minYAxisTestDisplayTextBox.Location = new System.Drawing.Point(710, 76);
            this.minYAxisTestDisplayTextBox.Name = "minYAxisTestDisplayTextBox";
            this.minYAxisTestDisplayTextBox.Size = new System.Drawing.Size(65, 21);
            this.minYAxisTestDisplayTextBox.TabIndex = 11;
            this.minYAxisTestDisplayTextBox.Text = "0.0";
            this.minYAxisTestDisplayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // useTimeStampCheckBox
            // 
            this.useTimeStampCheckBox.AutoSize = true;
            this.useTimeStampCheckBox.Location = new System.Drawing.Point(710, 103);
            this.useTimeStampCheckBox.Name = "useTimeStampCheckBox";
            this.useTimeStampCheckBox.Size = new System.Drawing.Size(115, 16);
            this.useTimeStampCheckBox.TabIndex = 8;
            this.useTimeStampCheckBox.Text = "Use TimeStamp";
            this.useTimeStampCheckBox.UseVisualStyleBackColor = true;
            // 
            // maxYAxisTestDisplayTextBox
            // 
            this.maxYAxisTestDisplayTextBox.Location = new System.Drawing.Point(804, 76);
            this.maxYAxisTestDisplayTextBox.Name = "maxYAxisTestDisplayTextBox";
            this.maxYAxisTestDisplayTextBox.Size = new System.Drawing.Size(65, 21);
            this.maxYAxisTestDisplayTextBox.TabIndex = 6;
            this.maxYAxisTestDisplayTextBox.Text = "1.0";
            this.maxYAxisTestDisplayTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // testDisplayButton
            // 
            this.testDisplayButton.Location = new System.Drawing.Point(793, 125);
            this.testDisplayButton.Name = "testDisplayButton";
            this.testDisplayButton.Size = new System.Drawing.Size(76, 23);
            this.testDisplayButton.TabIndex = 5;
            this.testDisplayButton.Text = "Display";
            this.testDisplayButton.UseVisualStyleBackColor = true;
            this.testDisplayButton.Click += new System.EventHandler(this.testDisplayButton_Click);
            // 
            // testDisplayModeComboBox
            // 
            this.testDisplayModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.testDisplayModeComboBox.FormattingEnabled = true;
            this.testDisplayModeComboBox.Items.AddRange(new object[] {
            "Squared Error",
            "Semantic Stress",
            "Cross Entropy",
            "Accuracy",
            "Target 1 Unit Activation",
            "Target 0 Unit Activation"});
            this.testDisplayModeComboBox.Location = new System.Drawing.Point(710, 35);
            this.testDisplayModeComboBox.Name = "testDisplayModeComboBox";
            this.testDisplayModeComboBox.Size = new System.Drawing.Size(159, 20);
            this.testDisplayModeComboBox.TabIndex = 4;
            // 
            // currentStimulusTextBox
            // 
            this.currentStimulusTextBox.Location = new System.Drawing.Point(708, 310);
            this.currentStimulusTextBox.Name = "currentStimulusTextBox";
            this.currentStimulusTextBox.ReadOnly = true;
            this.currentStimulusTextBox.Size = new System.Drawing.Size(159, 21);
            this.currentStimulusTextBox.TabIndex = 3;
            // 
            // currentStimulusLabel
            // 
            this.currentStimulusLabel.AutoSize = true;
            this.currentStimulusLabel.Location = new System.Drawing.Point(708, 295);
            this.currentStimulusLabel.Name = "currentStimulusLabel";
            this.currentStimulusLabel.Size = new System.Drawing.Size(99, 12);
            this.currentStimulusLabel.TabIndex = 2;
            this.currentStimulusLabel.Text = "Current Stimulus";
            // 
            // currentStimuliPackTextBox
            // 
            this.currentStimuliPackTextBox.Location = new System.Drawing.Point(708, 228);
            this.currentStimuliPackTextBox.Name = "currentStimuliPackTextBox";
            this.currentStimuliPackTextBox.ReadOnly = true;
            this.currentStimuliPackTextBox.Size = new System.Drawing.Size(159, 21);
            this.currentStimuliPackTextBox.TabIndex = 3;
            // 
            // currentProcessTextBox
            // 
            this.currentProcessTextBox.Location = new System.Drawing.Point(708, 189);
            this.currentProcessTextBox.Name = "currentProcessTextBox";
            this.currentProcessTextBox.ReadOnly = true;
            this.currentProcessTextBox.Size = new System.Drawing.Size(159, 21);
            this.currentProcessTextBox.TabIndex = 3;
            // 
            // currentStimuliPackLabel
            // 
            this.currentStimuliPackLabel.AutoSize = true;
            this.currentStimuliPackLabel.Location = new System.Drawing.Point(708, 213);
            this.currentStimuliPackLabel.Name = "currentStimuliPackLabel";
            this.currentStimuliPackLabel.Size = new System.Drawing.Size(120, 12);
            this.currentStimuliPackLabel.TabIndex = 2;
            this.currentStimuliPackLabel.Text = "Current Stimuli Pack";
            // 
            // currentProcessLabel
            // 
            this.currentProcessLabel.AutoSize = true;
            this.currentProcessLabel.Location = new System.Drawing.Point(708, 174);
            this.currentProcessLabel.Name = "currentProcessLabel";
            this.currentProcessLabel.Size = new System.Drawing.Size(97, 12);
            this.currentProcessLabel.TabIndex = 2;
            this.currentProcessLabel.Text = "Current Process";
            // 
            // currentEpochTextBox
            // 
            this.currentEpochTextBox.Location = new System.Drawing.Point(708, 267);
            this.currentEpochTextBox.Name = "currentEpochTextBox";
            this.currentEpochTextBox.ReadOnly = true;
            this.currentEpochTextBox.Size = new System.Drawing.Size(159, 21);
            this.currentEpochTextBox.TabIndex = 3;
            // 
            // yAxisDisplayLabel
            // 
            this.yAxisDisplayLabel.AutoSize = true;
            this.yAxisDisplayLabel.Location = new System.Drawing.Point(708, 58);
            this.yAxisDisplayLabel.Name = "yAxisDisplayLabel";
            this.yAxisDisplayLabel.Size = new System.Drawing.Size(44, 12);
            this.yAxisDisplayLabel.TabIndex = 2;
            this.yAxisDisplayLabel.Text = "Y-Axis";
            // 
            // testDisplayModeLabel
            // 
            this.testDisplayModeLabel.AutoSize = true;
            this.testDisplayModeLabel.Location = new System.Drawing.Point(708, 20);
            this.testDisplayModeLabel.Name = "testDisplayModeLabel";
            this.testDisplayModeLabel.Size = new System.Drawing.Size(83, 12);
            this.testDisplayModeLabel.TabIndex = 2;
            this.testDisplayModeLabel.Text = "Display Mode";
            // 
            // currentEpochLabel
            // 
            this.currentEpochLabel.AutoSize = true;
            this.currentEpochLabel.Location = new System.Drawing.Point(708, 252);
            this.currentEpochLabel.Name = "currentEpochLabel";
            this.currentEpochLabel.Size = new System.Drawing.Size(86, 12);
            this.currentEpochLabel.TabIndex = 2;
            this.currentEpochLabel.Text = "Current Epoch";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(506, 366);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(189, 40);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Enabled = false;
            this.pauseButton.Location = new System.Drawing.Point(506, 414);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(189, 40);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(701, 393);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(189, 40);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // saveLayerActivationCheckBox
            // 
            this.saveLayerActivationCheckBox.AutoSize = true;
            this.saveLayerActivationCheckBox.Location = new System.Drawing.Point(303, 366);
            this.saveLayerActivationCheckBox.Name = "saveLayerActivationCheckBox";
            this.saveLayerActivationCheckBox.Size = new System.Drawing.Size(146, 16);
            this.saveLayerActivationCheckBox.TabIndex = 6;
            this.saveLayerActivationCheckBox.Text = "Save Layer Activation";
            this.saveLayerActivationCheckBox.UseVisualStyleBackColor = true;
            // 
            // saveWeightCheckBox
            // 
            this.saveWeightCheckBox.AutoSize = true;
            this.saveWeightCheckBox.Location = new System.Drawing.Point(303, 388);
            this.saveWeightCheckBox.Name = "saveWeightCheckBox";
            this.saveWeightCheckBox.Size = new System.Drawing.Size(93, 16);
            this.saveWeightCheckBox.TabIndex = 7;
            this.saveWeightCheckBox.Text = "Save Weight";
            this.saveWeightCheckBox.UseVisualStyleBackColor = true;
            // 
            // BatchDataListBox
            // 
            this.BatchDataListBox.FormattingEnabled = true;
            this.BatchDataListBox.HorizontalScrollbar = true;
            this.BatchDataListBox.ItemHeight = 12;
            this.BatchDataListBox.Location = new System.Drawing.Point(12, 366);
            this.BatchDataListBox.Name = "BatchDataListBox";
            this.BatchDataListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.BatchDataListBox.Size = new System.Drawing.Size(285, 88);
            this.BatchDataListBox.TabIndex = 8;
            // 
            // timeLabel
            // 
            this.timeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLabel.BackColor = System.Drawing.SystemColors.Window;
            this.timeLabel.Location = new System.Drawing.Point(601, 322);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(101, 11);
            this.timeLabel.TabIndex = 12;
            this.timeLabel.Text = "0.000000";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BatchLearningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(896, 463);
            this.ControlBox = false;
            this.Controls.Add(this.BatchDataListBox);
            this.Controls.Add(this.saveWeightCheckBox);
            this.Controls.Add(this.saveLayerActivationCheckBox);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.statusGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BatchLearningForm";
            this.Text = "Batch Learning";
            ((System.ComponentModel.ISupportInitialize)(this.testDisplayChart)).EndInit();
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart testDisplayChart;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.TextBox currentStimuliPackTextBox;
        private System.Windows.Forms.Label currentStimuliPackLabel;
        private System.Windows.Forms.TextBox currentEpochTextBox;
        private System.Windows.Forms.Label currentEpochLabel;
        private System.Windows.Forms.TextBox currentStimulusTextBox;
        private System.Windows.Forms.Label currentStimulusLabel;
        private System.Windows.Forms.TextBox currentProcessTextBox;
        private System.Windows.Forms.Label currentProcessLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.ComboBox testDisplayModeComboBox;
        private System.Windows.Forms.Label testDisplayModeLabel;
        private System.Windows.Forms.TextBox maxYAxisTestDisplayTextBox;
        private System.Windows.Forms.Button testDisplayButton;
        private System.Windows.Forms.Label yAxisDisplayLabel;
        private System.Windows.Forms.CheckBox saveLayerActivationCheckBox;
        private System.Windows.Forms.CheckBox saveWeightCheckBox;
        private System.Windows.Forms.CheckBox useTimeStampCheckBox;
        private System.Windows.Forms.ListBox BatchDataListBox;
        private System.Windows.Forms.Label yAxisBetweenLabel;
        private System.Windows.Forms.TextBox minYAxisTestDisplayTextBox;
        private System.Windows.Forms.Label timeLabel;
    }
}