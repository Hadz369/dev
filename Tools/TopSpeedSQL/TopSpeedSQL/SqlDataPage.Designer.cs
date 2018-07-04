namespace TopSpeedSQL
{
    partial class SqlDataPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox_SQL = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabControl_SqlResults = new System.Windows.Forms.TabControl();
            this.tabPage_Results = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage_Messages = new System.Windows.Forms.TabPage();
            this.richTextBox_Messages = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl_SqlResults.SuspendLayout();
            this.tabPage_Results.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage_Messages.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_SQL
            // 
            this.richTextBox_SQL.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox_SQL.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_SQL.Name = "richTextBox_SQL";
            this.richTextBox_SQL.Size = new System.Drawing.Size(320, 130);
            this.richTextBox_SQL.TabIndex = 0;
            this.richTextBox_SQL.Text = "";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 130);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(320, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // tabControl_SqlResults
            // 
            this.tabControl_SqlResults.Controls.Add(this.tabPage_Results);
            this.tabControl_SqlResults.Controls.Add(this.tabPage_Messages);
            this.tabControl_SqlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_SqlResults.Location = new System.Drawing.Point(0, 133);
            this.tabControl_SqlResults.Name = "tabControl_SqlResults";
            this.tabControl_SqlResults.SelectedIndex = 0;
            this.tabControl_SqlResults.Size = new System.Drawing.Size(320, 211);
            this.tabControl_SqlResults.TabIndex = 2;
            // 
            // tabPage_Results
            // 
            this.tabPage_Results.Controls.Add(this.dataGridView1);
            this.tabPage_Results.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Results.Name = "tabPage_Results";
            this.tabPage_Results.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Results.Size = new System.Drawing.Size(312, 185);
            this.tabPage_Results.TabIndex = 0;
            this.tabPage_Results.Text = "Results";
            this.tabPage_Results.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(306, 179);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage_Messages
            // 
            this.tabPage_Messages.Controls.Add(this.richTextBox_Messages);
            this.tabPage_Messages.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Messages.Name = "tabPage_Messages";
            this.tabPage_Messages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Messages.Size = new System.Drawing.Size(312, 185);
            this.tabPage_Messages.TabIndex = 1;
            this.tabPage_Messages.Text = "Messages";
            this.tabPage_Messages.UseVisualStyleBackColor = true;
            // 
            // richTextBox_Messages
            // 
            this.richTextBox_Messages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Messages.Location = new System.Drawing.Point(3, 3);
            this.richTextBox_Messages.Name = "richTextBox_Messages";
            this.richTextBox_Messages.Size = new System.Drawing.Size(306, 179);
            this.richTextBox_Messages.TabIndex = 0;
            this.richTextBox_Messages.Text = "";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // SqlDataPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_SqlResults);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.richTextBox_SQL);
            this.Name = "SqlDataPage";
            this.Size = new System.Drawing.Size(320, 344);
            this.tabControl_SqlResults.ResumeLayout(false);
            this.tabPage_Results.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage_Messages.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_SQL;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl tabControl_SqlResults;
        private System.Windows.Forms.TabPage tabPage_Results;
        private System.Windows.Forms.TabPage tabPage_Messages;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RichTextBox richTextBox_Messages;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}
