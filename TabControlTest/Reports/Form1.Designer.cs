namespace Reports
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.eps_ctrlDataSet = new Reports.eps_ctrlDataSet();
            this.usp_SECSelModuleFuncBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.usp_SECSelModuleFuncTableAdapter = new Reports.eps_ctrlDataSetTableAdapters.usp_SECSelModuleFuncTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.eps_ctrlDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_SECSelModuleFuncBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.usp_SECSelModuleFuncBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Reports.Report2.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(284, 261);
            this.reportViewer1.TabIndex = 0;
            // 
            // eps_ctrlDataSet
            // 
            this.eps_ctrlDataSet.DataSetName = "eps_ctrlDataSet";
            this.eps_ctrlDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // usp_SECSelModuleFuncBindingSource
            // 
            this.usp_SECSelModuleFuncBindingSource.DataMember = "usp_SECSelModuleFunc";
            this.usp_SECSelModuleFuncBindingSource.DataSource = this.eps_ctrlDataSet;
            // 
            // usp_SECSelModuleFuncTableAdapter
            // 
            this.usp_SECSelModuleFuncTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eps_ctrlDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.usp_SECSelModuleFuncBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource usp_SECSelModuleFuncBindingSource;
        private eps_ctrlDataSet eps_ctrlDataSet;
        private eps_ctrlDataSetTableAdapters.usp_SECSelModuleFuncTableAdapter usp_SECSelModuleFuncTableAdapter;
    }
}