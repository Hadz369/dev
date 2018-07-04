namespace CryptDiary.Gui
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
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.vLabel = new System.Windows.Forms.Label();
            this.byLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.begLabel = new System.Windows.Forms.Label();
            this.btcAddressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(157, 173);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(121, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK, whatever...";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "CryptDiary";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(25, 43);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(86, 13);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "AssemblyVersion";
            // 
            // vLabel
            // 
            this.vLabel.AutoSize = true;
            this.vLabel.Location = new System.Drawing.Point(12, 43);
            this.vLabel.Name = "vLabel";
            this.vLabel.Size = new System.Drawing.Size(14, 13);
            this.vLabel.TabIndex = 3;
            this.vLabel.Text = "V";
            // 
            // byLabel
            // 
            this.byLabel.AutoSize = true;
            this.byLabel.Location = new System.Drawing.Point(12, 65);
            this.byLabel.Name = "byLabel";
            this.byLabel.Size = new System.Drawing.Size(99, 13);
            this.byLabel.TabIndex = 4;
            this.byLabel.Text = "by Marcus Radlach";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(12, 87);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(169, 13);
            this.copyrightLabel.TabIndex = 5;
            this.copyrightLabel.Text = "No rights reserved. You may copy!";
            // 
            // begLabel
            // 
            this.begLabel.AutoSize = true;
            this.begLabel.Location = new System.Drawing.Point(12, 109);
            this.begLabel.Name = "begLabel";
            this.begLabel.Size = new System.Drawing.Size(270, 13);
            this.begLabel.TabIndex = 6;
            this.begLabel.Text = "Feel free to spend some mBTC if you like this programm.";
            // 
            // btcAddressLabel
            // 
            this.btcAddressLabel.AutoSize = true;
            this.btcAddressLabel.Location = new System.Drawing.Point(12, 131);
            this.btcAddressLabel.Name = "btcAddressLabel";
            this.btcAddressLabel.Size = new System.Drawing.Size(218, 13);
            this.btcAddressLabel.TabIndex = 7;
            this.btcAddressLabel.Text = "13zLZB6HWm3XHg8QcNKTbgcbcarjvtAnrk";
            this.btcAddressLabel.Click += new System.EventHandler(this.btcAddressLabel_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 208);
            this.ControlBox = false;
            this.Controls.Add(this.btcAddressLabel);
            this.Controls.Add(this.begLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.byLabel);
            this.Controls.Add(this.vLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About CryptDiary";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label vLabel;
        private System.Windows.Forms.Label byLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label begLabel;
        private System.Windows.Forms.Label btcAddressLabel;
    }
}