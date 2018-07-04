namespace CryptDiary.Gui
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.autoSaveIntervalLabel = new System.Windows.Forms.Label();
            this.autoSaveIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoSaveCheckBox = new System.Windows.Forms.CheckBox();
            this.autoSaveSecondsLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.defaultsButton = new System.Windows.Forms.Button();
            this.autoSavePanel = new System.Windows.Forms.Panel();
            this.autoLockPanel = new System.Windows.Forms.Panel();
            this.autoLockCheckBox = new System.Windows.Forms.CheckBox();
            this.autoLockTextLabel1 = new System.Windows.Forms.Label();
            this.autoLockTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoLockTextLabel2 = new System.Windows.Forms.Label();
            this.pwIterationPanel = new System.Windows.Forms.Panel();
            this.passwordIterationInfoTextBox = new System.Windows.Forms.TextBox();
            this.iterationTimeLabel = new System.Windows.Forms.Label();
            this.benchmarkButton = new System.Windows.Forms.Button();
            this.passwordIterationLabel = new System.Windows.Forms.Label();
            this.passwordIterationsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.languageApplyInfoTextBox = new System.Windows.Forms.TextBox();
            this.languagesComboBox = new System.Windows.Forms.ComboBox();
            this.languageSelectionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.autoSaveIntervalNumericUpDown)).BeginInit();
            this.autoSavePanel.SuspendLayout();
            this.autoLockPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoLockTimeNumericUpDown)).BeginInit();
            this.pwIterationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordIterationsNumericUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoSaveIntervalLabel
            // 
            resources.ApplyResources(this.autoSaveIntervalLabel, "autoSaveIntervalLabel");
            this.autoSaveIntervalLabel.Name = "autoSaveIntervalLabel";
            // 
            // autoSaveIntervalNumericUpDown
            // 
            resources.ApplyResources(this.autoSaveIntervalNumericUpDown, "autoSaveIntervalNumericUpDown");
            this.autoSaveIntervalNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.autoSaveIntervalNumericUpDown.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.autoSaveIntervalNumericUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.autoSaveIntervalNumericUpDown.Name = "autoSaveIntervalNumericUpDown";
            this.autoSaveIntervalNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // autoSaveCheckBox
            // 
            resources.ApplyResources(this.autoSaveCheckBox, "autoSaveCheckBox");
            this.autoSaveCheckBox.Name = "autoSaveCheckBox";
            this.autoSaveCheckBox.UseVisualStyleBackColor = true;
            this.autoSaveCheckBox.CheckedChanged += new System.EventHandler(this.autoSaveCheckBox_CheckedChanged);
            // 
            // autoSaveSecondsLabel
            // 
            resources.ApplyResources(this.autoSaveSecondsLabel, "autoSaveSecondsLabel");
            this.autoSaveSecondsLabel.Name = "autoSaveSecondsLabel";
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // applyButton
            // 
            resources.ApplyResources(this.applyButton, "applyButton");
            this.applyButton.Name = "applyButton";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // defaultsButton
            // 
            resources.ApplyResources(this.defaultsButton, "defaultsButton");
            this.defaultsButton.Name = "defaultsButton";
            this.defaultsButton.UseVisualStyleBackColor = true;
            this.defaultsButton.Click += new System.EventHandler(this.defaultsButton_Click);
            // 
            // autoSavePanel
            // 
            resources.ApplyResources(this.autoSavePanel, "autoSavePanel");
            this.autoSavePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoSavePanel.Controls.Add(this.autoSaveCheckBox);
            this.autoSavePanel.Controls.Add(this.autoSaveIntervalLabel);
            this.autoSavePanel.Controls.Add(this.autoSaveIntervalNumericUpDown);
            this.autoSavePanel.Controls.Add(this.autoSaveSecondsLabel);
            this.autoSavePanel.Name = "autoSavePanel";
            // 
            // autoLockPanel
            // 
            resources.ApplyResources(this.autoLockPanel, "autoLockPanel");
            this.autoLockPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.autoLockPanel.Controls.Add(this.autoLockCheckBox);
            this.autoLockPanel.Controls.Add(this.autoLockTextLabel1);
            this.autoLockPanel.Controls.Add(this.autoLockTimeNumericUpDown);
            this.autoLockPanel.Controls.Add(this.autoLockTextLabel2);
            this.autoLockPanel.Name = "autoLockPanel";
            // 
            // autoLockCheckBox
            // 
            resources.ApplyResources(this.autoLockCheckBox, "autoLockCheckBox");
            this.autoLockCheckBox.Name = "autoLockCheckBox";
            this.autoLockCheckBox.UseVisualStyleBackColor = true;
            this.autoLockCheckBox.CheckedChanged += new System.EventHandler(this.autoLockCheckBox_CheckedChanged);
            // 
            // autoLockTextLabel1
            // 
            resources.ApplyResources(this.autoLockTextLabel1, "autoLockTextLabel1");
            this.autoLockTextLabel1.Name = "autoLockTextLabel1";
            // 
            // autoLockTimeNumericUpDown
            // 
            resources.ApplyResources(this.autoLockTimeNumericUpDown, "autoLockTimeNumericUpDown");
            this.autoLockTimeNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.autoLockTimeNumericUpDown.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.autoLockTimeNumericUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.autoLockTimeNumericUpDown.Name = "autoLockTimeNumericUpDown";
            this.autoLockTimeNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // autoLockTextLabel2
            // 
            resources.ApplyResources(this.autoLockTextLabel2, "autoLockTextLabel2");
            this.autoLockTextLabel2.Name = "autoLockTextLabel2";
            // 
            // pwIterationPanel
            // 
            resources.ApplyResources(this.pwIterationPanel, "pwIterationPanel");
            this.pwIterationPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pwIterationPanel.Controls.Add(this.passwordIterationInfoTextBox);
            this.pwIterationPanel.Controls.Add(this.iterationTimeLabel);
            this.pwIterationPanel.Controls.Add(this.benchmarkButton);
            this.pwIterationPanel.Controls.Add(this.passwordIterationLabel);
            this.pwIterationPanel.Controls.Add(this.passwordIterationsNumericUpDown);
            this.pwIterationPanel.Name = "pwIterationPanel";
            // 
            // passwordIterationInfoTextBox
            // 
            resources.ApplyResources(this.passwordIterationInfoTextBox, "passwordIterationInfoTextBox");
            this.passwordIterationInfoTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.passwordIterationInfoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordIterationInfoTextBox.Name = "passwordIterationInfoTextBox";
            this.passwordIterationInfoTextBox.ReadOnly = true;
            this.passwordIterationInfoTextBox.MouseEnter += new System.EventHandler(this.passwordIterationInfoTextBox_MouseEnter);
            this.passwordIterationInfoTextBox.MouseLeave += new System.EventHandler(this.passwordIterationInfoTextBox_MouseLeave);
            // 
            // iterationTimeLabel
            // 
            resources.ApplyResources(this.iterationTimeLabel, "iterationTimeLabel");
            this.iterationTimeLabel.Name = "iterationTimeLabel";
            // 
            // benchmarkButton
            // 
            resources.ApplyResources(this.benchmarkButton, "benchmarkButton");
            this.benchmarkButton.Name = "benchmarkButton";
            this.benchmarkButton.UseVisualStyleBackColor = true;
            this.benchmarkButton.Click += new System.EventHandler(this.benchmarkButton_Click);
            // 
            // passwordIterationLabel
            // 
            resources.ApplyResources(this.passwordIterationLabel, "passwordIterationLabel");
            this.passwordIterationLabel.Name = "passwordIterationLabel";
            // 
            // passwordIterationsNumericUpDown
            // 
            resources.ApplyResources(this.passwordIterationsNumericUpDown, "passwordIterationsNumericUpDown");
            this.passwordIterationsNumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.passwordIterationsNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.passwordIterationsNumericUpDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.passwordIterationsNumericUpDown.Name = "passwordIterationsNumericUpDown";
            this.passwordIterationsNumericUpDown.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.languageApplyInfoTextBox);
            this.panel1.Controls.Add(this.languagesComboBox);
            this.panel1.Controls.Add(this.languageSelectionLabel);
            this.panel1.Name = "panel1";
            // 
            // languageApplyInfoTextBox
            // 
            resources.ApplyResources(this.languageApplyInfoTextBox, "languageApplyInfoTextBox");
            this.languageApplyInfoTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.languageApplyInfoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.languageApplyInfoTextBox.Name = "languageApplyInfoTextBox";
            this.languageApplyInfoTextBox.ReadOnly = true;
            // 
            // languagesComboBox
            // 
            resources.ApplyResources(this.languagesComboBox, "languagesComboBox");
            this.languagesComboBox.FormattingEnabled = true;
            this.languagesComboBox.Name = "languagesComboBox";
            // 
            // languageSelectionLabel
            // 
            resources.ApplyResources(this.languageSelectionLabel, "languageSelectionLabel");
            this.languageSelectionLabel.Name = "languageSelectionLabel";
            // 
            // OptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pwIterationPanel);
            this.Controls.Add(this.autoLockPanel);
            this.Controls.Add(this.autoSavePanel);
            this.Controls.Add(this.defaultsButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.autoSaveIntervalNumericUpDown)).EndInit();
            this.autoSavePanel.ResumeLayout(false);
            this.autoSavePanel.PerformLayout();
            this.autoLockPanel.ResumeLayout(false);
            this.autoLockPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoLockTimeNumericUpDown)).EndInit();
            this.pwIterationPanel.ResumeLayout(false);
            this.pwIterationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.passwordIterationsNumericUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label autoSaveIntervalLabel;
        private System.Windows.Forms.NumericUpDown autoSaveIntervalNumericUpDown;
        private System.Windows.Forms.CheckBox autoSaveCheckBox;
        private System.Windows.Forms.Label autoSaveSecondsLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button defaultsButton;
        private System.Windows.Forms.Panel autoSavePanel;
        private System.Windows.Forms.Panel autoLockPanel;
        private System.Windows.Forms.CheckBox autoLockCheckBox;
        private System.Windows.Forms.Label autoLockTextLabel1;
        private System.Windows.Forms.NumericUpDown autoLockTimeNumericUpDown;
        private System.Windows.Forms.Label autoLockTextLabel2;
        private System.Windows.Forms.Panel pwIterationPanel;
        private System.Windows.Forms.Label passwordIterationLabel;
        private System.Windows.Forms.NumericUpDown passwordIterationsNumericUpDown;
        private System.Windows.Forms.Button benchmarkButton;
        private System.Windows.Forms.Label iterationTimeLabel;
        private System.Windows.Forms.TextBox passwordIterationInfoTextBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox languageApplyInfoTextBox;
        private System.Windows.Forms.ComboBox languagesComboBox;
        private System.Windows.Forms.Label languageSelectionLabel;
    }
}