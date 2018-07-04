namespace CryptDiary.Gui
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.rescanHashtagsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportDiaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.closeDiaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.MonthCalendar = new System.Windows.Forms.MonthCalendar();
            this.lockDiaryButton = new System.Windows.Forms.Button();
            this.nextDiaryEntryButton = new System.Windows.Forms.Button();
            this.previousDiaryEntryButton = new System.Windows.Forms.Button();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.workDirectoryBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.calendarPanel = new System.Windows.Forms.Panel();
            this.buttonsPanel = new System.Windows.Forms.Panel();
            this.HashtagsListBox = new System.Windows.Forms.ListBox();
            this.hashtagsLabel = new System.Windows.Forms.Label();
            this.generalTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.diaryEntryRichTextBox = new System.Windows.Forms.RichTextBox();
            this.previewTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.exportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.mainMenuStrip.SuspendLayout();
            this.calendarPanel.SuspendLayout();
            this.buttonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.rescanHashtagsMenuItem,
            this.changePasswordToolMenuItem,
            this.toolStripSeparator2,
            this.exportDiaryToolStripMenuItem,
            this.toolStripSeparator6,
            this.closeDiaryToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_create_new_folder_black_36dp;
            resources.ApplyResources(this.newToolStripMenuItem, "newToolStripMenuItem");
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_folder_open_black_36dp;
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_file_download_black_36dp;
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // rescanHashtagsMenuItem
            // 
            this.rescanHashtagsMenuItem.Name = "rescanHashtagsMenuItem";
            resources.ApplyResources(this.rescanHashtagsMenuItem, "rescanHashtagsMenuItem");
            this.rescanHashtagsMenuItem.Click += new System.EventHandler(this.rescanHashtagsMenuItem_Click);
            // 
            // changePasswordToolMenuItem
            // 
            this.changePasswordToolMenuItem.Image = global::CryptDiary.Properties.Resources.ic_vpn_key_36pt;
            this.changePasswordToolMenuItem.Name = "changePasswordToolMenuItem";
            resources.ApplyResources(this.changePasswordToolMenuItem, "changePasswordToolMenuItem");
            this.changePasswordToolMenuItem.Click += new System.EventHandler(this.changePasswordToolMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // exportDiaryToolStripMenuItem
            // 
            this.exportDiaryToolStripMenuItem.Name = "exportDiaryToolStripMenuItem";
            resources.ApplyResources(this.exportDiaryToolStripMenuItem, "exportDiaryToolStripMenuItem");
            this.exportDiaryToolStripMenuItem.Click += new System.EventHandler(this.exportDiaryToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // closeDiaryToolStripMenuItem
            // 
            this.closeDiaryToolStripMenuItem.Name = "closeDiaryToolStripMenuItem";
            resources.ApplyResources(this.closeDiaryToolStripMenuItem, "closeDiaryToolStripMenuItem");
            this.closeDiaryToolStripMenuItem.Click += new System.EventHandler(this.closeDiaryToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_exit_to_app_black_36dp;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_undo_black_36dp;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            resources.ApplyResources(this.undoToolStripMenuItem, "undoToolStripMenuItem");
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.rückgängigToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_redo_black_36dp;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            resources.ApplyResources(this.redoToolStripMenuItem, "redoToolStripMenuItem");
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.wiederholenToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_content_cut_black_36dp;
            resources.ApplyResources(this.cutToolStripMenuItem, "cutToolStripMenuItem");
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.ausschneidenToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_content_copy_black_36dp;
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.kopierenToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_content_paste_black_36dp;
            resources.ApplyResources(this.pasteToolStripMenuItem, "pasteToolStripMenuItem");
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.einfügenToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_select_all_black_36dp;
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.alleauswählenToolStripMenuItem_Click);
            // 
            // extrasToolStripMenuItem
            // 
            this.extrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
            resources.ApplyResources(this.extrasToolStripMenuItem, "extrasToolStripMenuItem");
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_build_black_36dp;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.infoToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Image = global::CryptDiary.Properties.Resources.ic_info_black_36dp;
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.extrasToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // MonthCalendar
            // 
            resources.ApplyResources(this.MonthCalendar, "MonthCalendar");
            this.MonthCalendar.MaxSelectionCount = 1;
            this.MonthCalendar.Name = "MonthCalendar";
            this.MonthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateChanged);
            this.MonthCalendar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // lockDiaryButton
            // 
            resources.ApplyResources(this.lockDiaryButton, "lockDiaryButton");
            this.lockDiaryButton.Image = global::CryptDiary.Properties.Resources.ic_lock_open_black_36dp;
            this.lockDiaryButton.Name = "lockDiaryButton";
            this.generalTooltip.SetToolTip(this.lockDiaryButton, resources.GetString("lockDiaryButton.ToolTip"));
            this.lockDiaryButton.UseVisualStyleBackColor = true;
            this.lockDiaryButton.Click += new System.EventHandler(this.LockDiaryButton_Click);
            this.lockDiaryButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // nextDiaryEntryButton
            // 
            this.nextDiaryEntryButton.Image = global::CryptDiary.Properties.Resources.ic_arrow_forward_black_18dp;
            resources.ApplyResources(this.nextDiaryEntryButton, "nextDiaryEntryButton");
            this.nextDiaryEntryButton.Name = "nextDiaryEntryButton";
            this.generalTooltip.SetToolTip(this.nextDiaryEntryButton, resources.GetString("nextDiaryEntryButton.ToolTip"));
            this.nextDiaryEntryButton.UseVisualStyleBackColor = true;
            this.nextDiaryEntryButton.Click += new System.EventHandler(this.nextDiaryEntryButton_Click);
            this.nextDiaryEntryButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // previousDiaryEntryButton
            // 
            this.previousDiaryEntryButton.Image = global::CryptDiary.Properties.Resources.ic_arrow_back_black_18dp;
            resources.ApplyResources(this.previousDiaryEntryButton, "previousDiaryEntryButton");
            this.previousDiaryEntryButton.Name = "previousDiaryEntryButton";
            this.generalTooltip.SetToolTip(this.previousDiaryEntryButton, resources.GetString("previousDiaryEntryButton.ToolTip"));
            this.previousDiaryEntryButton.UseVisualStyleBackColor = true;
            this.previousDiaryEntryButton.Click += new System.EventHandler(this.previousDiaryEntryButton_Click);
            this.previousDiaryEntryButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // openFolderButton
            // 
            this.openFolderButton.Image = global::CryptDiary.Properties.Resources.ic_folder_open_black_36dp;
            resources.ApplyResources(this.openFolderButton, "openFolderButton");
            this.openFolderButton.Name = "openFolderButton";
            this.generalTooltip.SetToolTip(this.openFolderButton, resources.GetString("openFolderButton.ToolTip"));
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            this.openFolderButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // saveButton
            // 
            this.saveButton.Image = global::CryptDiary.Properties.Resources.ic_file_download_black_36dp;
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.generalTooltip.SetToolTip(this.saveButton, resources.GetString("saveButton.ToolTip"));
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            this.saveButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            // 
            // workDirectoryBrowserDialog
            // 
            this.workDirectoryBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // calendarPanel
            // 
            this.calendarPanel.Controls.Add(this.MonthCalendar);
            this.calendarPanel.Controls.Add(this.nextDiaryEntryButton);
            this.calendarPanel.Controls.Add(this.previousDiaryEntryButton);
            resources.ApplyResources(this.calendarPanel, "calendarPanel");
            this.calendarPanel.Name = "calendarPanel";
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Controls.Add(this.openFolderButton);
            this.buttonsPanel.Controls.Add(this.saveButton);
            resources.ApplyResources(this.buttonsPanel, "buttonsPanel");
            this.buttonsPanel.Name = "buttonsPanel";
            // 
            // HashtagsListBox
            // 
            resources.ApplyResources(this.HashtagsListBox, "HashtagsListBox");
            this.HashtagsListBox.FormattingEnabled = true;
            this.HashtagsListBox.Name = "HashtagsListBox";
            this.HashtagsListBox.Sorted = true;
            this.HashtagsListBox.SelectedIndexChanged += new System.EventHandler(this.HashtagsListBox_SelectedIndexChanged);
            // 
            // hashtagsLabel
            // 
            resources.ApplyResources(this.hashtagsLabel, "hashtagsLabel");
            this.hashtagsLabel.Name = "hashtagsLabel";
            // 
            // diaryEntryRichTextBox
            // 
            resources.ApplyResources(this.diaryEntryRichTextBox, "diaryEntryRichTextBox");
            this.diaryEntryRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.diaryEntryRichTextBox.Name = "diaryEntryRichTextBox";
            // 
            // exportFileDialog
            // 
            this.exportFileDialog.DefaultExt = "zip";
            resources.ApplyResources(this.exportFileDialog, "exportFileDialog");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.diaryEntryRichTextBox);
            this.Controls.Add(this.HashtagsListBox);
            this.Controls.Add(this.buttonsPanel);
            this.Controls.Add(this.hashtagsLabel);
            this.Controls.Add(this.calendarPanel);
            this.Controls.Add(this.lockDiaryButton);
            this.Controls.Add(this.mainMenuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.formMain_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.calendarPanel.ResumeLayout(false);
            this.buttonsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button previousDiaryEntryButton;
        private System.Windows.Forms.Button nextDiaryEntryButton;
        private System.Windows.Forms.Button lockDiaryButton;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.FolderBrowserDialog workDirectoryBrowserDialog;
        public System.Windows.Forms.MonthCalendar MonthCalendar;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeDiaryToolStripMenuItem;
        private System.Windows.Forms.Panel calendarPanel;
        private System.Windows.Forms.Panel buttonsPanel;
        private System.Windows.Forms.Label hashtagsLabel;
        public System.Windows.Forms.ListBox HashtagsListBox;
        private System.Windows.Forms.ToolStripMenuItem rescanHashtagsMenuItem;
        private System.Windows.Forms.ToolTip generalTooltip;
        private System.Windows.Forms.ToolTip previewTooltip;
        private System.Windows.Forms.RichTextBox diaryEntryRichTextBox;
        private System.Windows.Forms.SaveFileDialog exportFileDialog;
        private System.Windows.Forms.ToolStripMenuItem exportDiaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}

