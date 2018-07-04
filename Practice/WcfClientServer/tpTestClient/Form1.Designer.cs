namespace IG.ThirdParty
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSignOn = new System.Windows.Forms.Button();
            this.btnSiteDetails = new System.Windows.Forms.Button();
            this.btnAllMembers = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 20);
            this.textBox1.TabIndex = 0;
            // 
            // btnSignOn
            // 
            this.btnSignOn.Location = new System.Drawing.Point(197, 13);
            this.btnSignOn.Name = "btnSignOn";
            this.btnSignOn.Size = new System.Drawing.Size(75, 23);
            this.btnSignOn.TabIndex = 1;
            this.btnSignOn.Text = "Sign On";
            this.btnSignOn.UseVisualStyleBackColor = true;
            this.btnSignOn.Click += new System.EventHandler(this.btnSignOn_Click);
            // 
            // btnSiteDetails
            // 
            this.btnSiteDetails.Location = new System.Drawing.Point(197, 42);
            this.btnSiteDetails.Name = "btnSiteDetails";
            this.btnSiteDetails.Size = new System.Drawing.Size(75, 23);
            this.btnSiteDetails.TabIndex = 2;
            this.btnSiteDetails.Text = "Site Details";
            this.btnSiteDetails.UseVisualStyleBackColor = true;
            this.btnSiteDetails.Click += new System.EventHandler(this.btnSiteDetails_Click);
            // 
            // btnAllMembers
            // 
            this.btnAllMembers.Location = new System.Drawing.Point(197, 71);
            this.btnAllMembers.Name = "btnAllMembers";
            this.btnAllMembers.Size = new System.Drawing.Size(75, 23);
            this.btnAllMembers.TabIndex = 3;
            this.btnAllMembers.Text = "All Members";
            this.btnAllMembers.UseVisualStyleBackColor = true;
            this.btnAllMembers.Click += new System.EventHandler(this.btnAllMembers_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 112);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(259, 134);
            this.listBox1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnAllMembers);
            this.Controls.Add(this.btnSiteDetails);
            this.Controls.Add(this.btnSignOn);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSignOn;
        private System.Windows.Forms.Button btnSiteDetails;
        private System.Windows.Forms.Button btnAllMembers;
        private System.Windows.Forms.ListBox listBox1;
    }
}

