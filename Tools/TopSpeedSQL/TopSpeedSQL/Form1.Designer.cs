namespace TopSpeedSQL
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer_Base = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader_TableName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl_Utility = new System.Windows.Forms.TabControl();
            this.tabPage_DateConvert = new System.Windows.Forms.TabPage();
            this.button_FromClarionDate = new System.Windows.Forms.Button();
            this.button_ToClarionDate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ClarionDate = new System.Windows.Forms.TextBox();
            this.dateTimePicker_CalendarDate = new System.Windows.Forms.DateTimePicker();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_SelectDatabase = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox_Location = new System.Windows.Forms.TextBox();
            this.button_SelectFolder = new System.Windows.Forms.Button();
            this.label_FolderSelect = new System.Windows.Forms.Label();
            this.panel_SQL = new System.Windows.Forms.Panel();
            this.tabControl_Content = new System.Windows.Forms.TabControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_NewQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Execute = new System.Windows.Forms.ToolStripButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_Version = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_VersionVal = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_Base.Panel1.SuspendLayout();
            this.splitContainer_Base.Panel2.SuspendLayout();
            this.splitContainer_Base.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl_Utility.SuspendLayout();
            this.tabPage_DateConvert.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox_SelectDatabase.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel_SQL.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Base
            // 
            this.splitContainer_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Base.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Base.Location = new System.Drawing.Point(0, 25);
            this.splitContainer_Base.Name = "splitContainer_Base";
            // 
            // splitContainer_Base.Panel1
            // 
            this.splitContainer_Base.Panel1.Controls.Add(this.panel2);
            this.splitContainer_Base.Panel1.Controls.Add(this.splitter1);
            this.splitContainer_Base.Panel1.Controls.Add(this.panel1);
            this.splitContainer_Base.Panel1.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            // 
            // splitContainer_Base.Panel2
            // 
            this.splitContainer_Base.Panel2.Controls.Add(this.panel_SQL);
            this.splitContainer_Base.Panel2.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.splitContainer_Base.Size = new System.Drawing.Size(1008, 682);
            this.splitContainer_Base.SplitterDistance = 250;
            this.splitContainer_Base.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 66);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(245, 611);
            this.panel2.TabIndex = 2;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 459);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(245, 7);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_TableName});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(245, 466);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader_TableName
            // 
            this.columnHeader_TableName.Text = "Table Name";
            this.columnHeader_TableName.Width = 144;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 26);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl_Utility);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 466);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(245, 145);
            this.panel3.TabIndex = 1;
            // 
            // tabControl_Utility
            // 
            this.tabControl_Utility.Controls.Add(this.tabPage_DateConvert);
            this.tabControl_Utility.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Utility.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Utility.Name = "tabControl_Utility";
            this.tabControl_Utility.SelectedIndex = 0;
            this.tabControl_Utility.Size = new System.Drawing.Size(245, 145);
            this.tabControl_Utility.TabIndex = 0;
            // 
            // tabPage_DateConvert
            // 
            this.tabPage_DateConvert.Controls.Add(this.button_FromClarionDate);
            this.tabPage_DateConvert.Controls.Add(this.button_ToClarionDate);
            this.tabPage_DateConvert.Controls.Add(this.label2);
            this.tabPage_DateConvert.Controls.Add(this.label1);
            this.tabPage_DateConvert.Controls.Add(this.textBox_ClarionDate);
            this.tabPage_DateConvert.Controls.Add(this.dateTimePicker_CalendarDate);
            this.tabPage_DateConvert.Location = new System.Drawing.Point(4, 22);
            this.tabPage_DateConvert.Name = "tabPage_DateConvert";
            this.tabPage_DateConvert.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_DateConvert.Size = new System.Drawing.Size(237, 119);
            this.tabPage_DateConvert.TabIndex = 0;
            this.tabPage_DateConvert.Text = "Date Conversion";
            this.tabPage_DateConvert.UseVisualStyleBackColor = true;
            // 
            // button_FromClarionDate
            // 
            this.button_FromClarionDate.Location = new System.Drawing.Point(22, 89);
            this.button_FromClarionDate.Name = "button_FromClarionDate";
            this.button_FromClarionDate.Size = new System.Drawing.Size(193, 23);
            this.button_FromClarionDate.TabIndex = 5;
            this.button_FromClarionDate.Text = "From Clarion Date";
            this.button_FromClarionDate.UseVisualStyleBackColor = true;
            this.button_FromClarionDate.Click += new System.EventHandler(this.button_FromClarionDate_Click);
            // 
            // button_ToClarionDate
            // 
            this.button_ToClarionDate.Location = new System.Drawing.Point(22, 60);
            this.button_ToClarionDate.Name = "button_ToClarionDate";
            this.button_ToClarionDate.Size = new System.Drawing.Size(193, 23);
            this.button_ToClarionDate.TabIndex = 4;
            this.button_ToClarionDate.Text = "To Clarion Date";
            this.button_ToClarionDate.UseVisualStyleBackColor = true;
            this.button_ToClarionDate.Click += new System.EventHandler(this.button_ToClarionDate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Clarion Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Calendar Date";
            // 
            // textBox_ClarionDate
            // 
            this.textBox_ClarionDate.Location = new System.Drawing.Point(115, 34);
            this.textBox_ClarionDate.Name = "textBox_ClarionDate";
            this.textBox_ClarionDate.Size = new System.Drawing.Size(100, 20);
            this.textBox_ClarionDate.TabIndex = 1;
            // 
            // dateTimePicker_CalendarDate
            // 
            this.dateTimePicker_CalendarDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_CalendarDate.Location = new System.Drawing.Point(115, 8);
            this.dateTimePicker_CalendarDate.Name = "dateTimePicker_CalendarDate";
            this.dateTimePicker_CalendarDate.Size = new System.Drawing.Size(100, 20);
            this.dateTimePicker_CalendarDate.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(5, 59);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(245, 7);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_SelectDatabase);
            this.panel1.Controls.Add(this.label_FolderSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel1.Size = new System.Drawing.Size(245, 54);
            this.panel1.TabIndex = 0;
            // 
            // groupBox_SelectDatabase
            // 
            this.groupBox_SelectDatabase.Controls.Add(this.panel4);
            this.groupBox_SelectDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_SelectDatabase.Location = new System.Drawing.Point(0, 0);
            this.groupBox_SelectDatabase.Name = "groupBox_SelectDatabase";
            this.groupBox_SelectDatabase.Size = new System.Drawing.Size(245, 51);
            this.groupBox_SelectDatabase.TabIndex = 4;
            this.groupBox_SelectDatabase.TabStop = false;
            this.groupBox_SelectDatabase.Text = "Select the Database Location";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBox_Location);
            this.panel4.Controls.Add(this.button_SelectFolder);
            this.panel4.Location = new System.Drawing.Point(7, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(232, 20);
            this.panel4.TabIndex = 3;
            // 
            // textBox_Location
            // 
            this.textBox_Location.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Location.Location = new System.Drawing.Point(0, 0);
            this.textBox_Location.Name = "textBox_Location";
            this.textBox_Location.Size = new System.Drawing.Size(211, 20);
            this.textBox_Location.TabIndex = 1;
            // 
            // button_SelectFolder
            // 
            this.button_SelectFolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_SelectFolder.Location = new System.Drawing.Point(211, 0);
            this.button_SelectFolder.Name = "button_SelectFolder";
            this.button_SelectFolder.Size = new System.Drawing.Size(21, 20);
            this.button_SelectFolder.TabIndex = 2;
            this.button_SelectFolder.UseVisualStyleBackColor = true;
            this.button_SelectFolder.Click += new System.EventHandler(this.button_SelectFolder_Click);
            // 
            // label_FolderSelect
            // 
            this.label_FolderSelect.AutoSize = true;
            this.label_FolderSelect.Location = new System.Drawing.Point(12, 7);
            this.label_FolderSelect.Name = "label_FolderSelect";
            this.label_FolderSelect.Size = new System.Drawing.Size(148, 13);
            this.label_FolderSelect.TabIndex = 0;
            this.label_FolderSelect.Text = "Select the Database Location";
            // 
            // panel_SQL
            // 
            this.panel_SQL.Controls.Add(this.tabControl_Content);
            this.panel_SQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_SQL.Location = new System.Drawing.Point(0, 5);
            this.panel_SQL.Name = "panel_SQL";
            this.panel_SQL.Size = new System.Drawing.Size(749, 672);
            this.panel_SQL.TabIndex = 1;
            // 
            // tabControl_Content
            // 
            this.tabControl_Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Content.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Content.Name = "tabControl_Content";
            this.tabControl_Content.SelectedIndex = 0;
            this.tabControl_Content.Size = new System.Drawing.Size(749, 672);
            this.tabControl_Content.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_NewQuery,
            this.toolStripButton_Execute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1008, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_NewQuery
            // 
            this.toolStripButton_NewQuery.Enabled = false;
            this.toolStripButton_NewQuery.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_NewQuery.Image")));
            this.toolStripButton_NewQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_NewQuery.Name = "toolStripButton_NewQuery";
            this.toolStripButton_NewQuery.Size = new System.Drawing.Size(86, 22);
            this.toolStripButton_NewQuery.Text = "New Query";
            this.toolStripButton_NewQuery.Click += new System.EventHandler(this.toolStripButton_NewQuery_Click);
            // 
            // toolStripButton_Execute
            // 
            this.toolStripButton_Execute.Enabled = false;
            this.toolStripButton_Execute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Execute.Image")));
            this.toolStripButton_Execute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Execute.Name = "toolStripButton_Execute";
            this.toolStripButton_Execute.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton_Execute.Text = "Execute";
            this.toolStripButton_Execute.Click += new System.EventHandler(this.toolStripButton_Execute_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_Version,
            this.toolStripStatusLabel_VersionVal});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_Version
            // 
            this.toolStripStatusLabel_Version.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel_Version.Name = "toolStripStatusLabel_Version";
            this.toolStripStatusLabel_Version.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusLabel_Version.Text = "Version:";
            // 
            // toolStripStatusLabel_VersionVal
            // 
            this.toolStripStatusLabel_VersionVal.Name = "toolStripStatusLabel_VersionVal";
            this.toolStripStatusLabel_VersionVal.Size = new System.Drawing.Size(62, 17);
            this.toolStripStatusLabel_VersionVal.Text = "<Version>";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.splitContainer_Base);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "Form1";
            this.Text = "TopSpeed SQL Utility";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer_Base.Panel1.ResumeLayout(false);
            this.splitContainer_Base.Panel2.ResumeLayout(false);
            this.splitContainer_Base.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabControl_Utility.ResumeLayout(false);
            this.tabPage_DateConvert.ResumeLayout(false);
            this.tabPage_DateConvert.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox_SelectDatabase.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel_SQL.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Base;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBox_Location;
        private System.Windows.Forms.Button button_SelectFolder;
        private System.Windows.Forms.Label label_FolderSelect;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TabControl tabControl_Content;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel_SQL;
        private System.Windows.Forms.ToolStripButton toolStripButton_NewQuery;
        private System.Windows.Forms.ToolStripButton toolStripButton_Execute;
        private System.Windows.Forms.ColumnHeader columnHeader_TableName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabControl tabControl_Utility;
        private System.Windows.Forms.TabPage tabPage_DateConvert;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ClarionDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_CalendarDate;
        private System.Windows.Forms.Button button_FromClarionDate;
        private System.Windows.Forms.Button button_ToClarionDate;
        private System.Windows.Forms.GroupBox groupBox_SelectDatabase;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Version;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_VersionVal;
    }
}

