namespace TestClient
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
            this.btnSignOn = new System.Windows.Forms.Button();
            this.btnSignOff = new System.Windows.Forms.Button();
            this.txtSession = new System.Windows.Forms.TextBox();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.btnGetTiers = new System.Windows.Forms.Button();
            this.btnGetMembers = new System.Windows.Forms.Button();
            this.txtUri = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetTier = new System.Windows.Forms.Button();
            this.btnGetMember = new System.Windows.Forms.Button();
            this.btnMemAtLoc = new System.Windows.Forms.Button();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.btnUpdAcct = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.btnGetMachine = new System.Windows.Forms.Button();
            this.btnGetMachines = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMethod = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGetMachStats = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtContentType = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSignOn
            // 
            this.btnSignOn.Location = new System.Drawing.Point(9, 21);
            this.btnSignOn.Name = "btnSignOn";
            this.btnSignOn.Size = new System.Drawing.Size(88, 23);
            this.btnSignOn.TabIndex = 0;
            this.btnSignOn.Text = "Sign On";
            this.btnSignOn.UseVisualStyleBackColor = true;
            this.btnSignOn.Click += new System.EventHandler(this.btnSignOn_Click);
            // 
            // btnSignOff
            // 
            this.btnSignOff.Location = new System.Drawing.Point(9, 50);
            this.btnSignOff.Name = "btnSignOff";
            this.btnSignOff.Size = new System.Drawing.Size(88, 23);
            this.btnSignOff.TabIndex = 1;
            this.btnSignOff.Text = "Sign Off";
            this.btnSignOff.UseVisualStyleBackColor = true;
            this.btnSignOff.Click += new System.EventHandler(this.btnSignOff_Click);
            // 
            // txtSession
            // 
            this.txtSession.Location = new System.Drawing.Point(177, 17);
            this.txtSession.Name = "txtSession";
            this.txtSession.ReadOnly = true;
            this.txtSession.Size = new System.Drawing.Size(249, 20);
            this.txtSession.TabIndex = 2;
            // 
            // txtRequest
            // 
            this.txtRequest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRequest.Location = new System.Drawing.Point(129, 170);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRequest.Size = new System.Drawing.Size(378, 225);
            this.txtRequest.TabIndex = 3;
            // 
            // btnGetTiers
            // 
            this.btnGetTiers.Location = new System.Drawing.Point(9, 79);
            this.btnGetTiers.Name = "btnGetTiers";
            this.btnGetTiers.Size = new System.Drawing.Size(88, 23);
            this.btnGetTiers.TabIndex = 4;
            this.btnGetTiers.Text = "Get Tiers";
            this.btnGetTiers.UseVisualStyleBackColor = true;
            this.btnGetTiers.Click += new System.EventHandler(this.btnGetTiers_Click);
            // 
            // btnGetMembers
            // 
            this.btnGetMembers.Location = new System.Drawing.Point(9, 137);
            this.btnGetMembers.Name = "btnGetMembers";
            this.btnGetMembers.Size = new System.Drawing.Size(88, 23);
            this.btnGetMembers.TabIndex = 5;
            this.btnGetMembers.Text = "Get Members";
            this.btnGetMembers.UseVisualStyleBackColor = true;
            this.btnGetMembers.Click += new System.EventHandler(this.btnGetMembers_Click);
            // 
            // txtUri
            // 
            this.txtUri.Location = new System.Drawing.Point(177, 68);
            this.txtUri.Multiline = true;
            this.txtUri.Name = "txtUri";
            this.txtUri.Size = new System.Drawing.Size(330, 53);
            this.txtUri.TabIndex = 6;
            this.txtUri.Text = "tp1/signon/?usr=User&psw=pass";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(126, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Uri";
            // 
            // btnGetTier
            // 
            this.btnGetTier.Location = new System.Drawing.Point(9, 108);
            this.btnGetTier.Name = "btnGetTier";
            this.btnGetTier.Size = new System.Drawing.Size(88, 23);
            this.btnGetTier.TabIndex = 8;
            this.btnGetTier.Text = "Get Tier";
            this.btnGetTier.UseVisualStyleBackColor = true;
            this.btnGetTier.Click += new System.EventHandler(this.btnGetTier_Click);
            // 
            // btnGetMember
            // 
            this.btnGetMember.Location = new System.Drawing.Point(9, 166);
            this.btnGetMember.Name = "btnGetMember";
            this.btnGetMember.Size = new System.Drawing.Size(88, 23);
            this.btnGetMember.TabIndex = 9;
            this.btnGetMember.Text = "Get Member";
            this.btnGetMember.UseVisualStyleBackColor = true;
            this.btnGetMember.Click += new System.EventHandler(this.btnGetMember_Click);
            // 
            // btnMemAtLoc
            // 
            this.btnMemAtLoc.Location = new System.Drawing.Point(9, 195);
            this.btnMemAtLoc.Name = "btnMemAtLoc";
            this.btnMemAtLoc.Size = new System.Drawing.Size(88, 37);
            this.btnMemAtLoc.TabIndex = 10;
            this.btnMemAtLoc.Text = "Get Member at Location";
            this.btnMemAtLoc.UseVisualStyleBackColor = true;
            this.btnMemAtLoc.Click += new System.EventHandler(this.btnMemAtLoc_Click);
            // 
            // txtResponse
            // 
            this.txtResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResponse.Location = new System.Drawing.Point(517, 36);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponse.Size = new System.Drawing.Size(528, 456);
            this.txtResponse.TabIndex = 13;
            // 
            // btnUpdAcct
            // 
            this.btnUpdAcct.Location = new System.Drawing.Point(9, 238);
            this.btnUpdAcct.Name = "btnUpdAcct";
            this.btnUpdAcct.Size = new System.Drawing.Size(88, 37);
            this.btnUpdAcct.TabIndex = 14;
            this.btnUpdAcct.Text = "Update Account";
            this.btnUpdAcct.UseVisualStyleBackColor = true;
            this.btnUpdAcct.Click += new System.EventHandler(this.btnUpdAcct_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtMessages.Location = new System.Drawing.Point(129, 418);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Size = new System.Drawing.Size(378, 74);
            this.txtMessages.TabIndex = 15;
            // 
            // btnGetMachine
            // 
            this.btnGetMachine.Location = new System.Drawing.Point(9, 281);
            this.btnGetMachine.Name = "btnGetMachine";
            this.btnGetMachine.Size = new System.Drawing.Size(88, 23);
            this.btnGetMachine.TabIndex = 16;
            this.btnGetMachine.Text = "Get Machine";
            this.btnGetMachine.UseVisualStyleBackColor = true;
            this.btnGetMachine.Click += new System.EventHandler(this.btnGetMachine_Click);
            // 
            // btnGetMachines
            // 
            this.btnGetMachines.Location = new System.Drawing.Point(9, 310);
            this.btnGetMachines.Name = "btnGetMachines";
            this.btnGetMachines.Size = new System.Drawing.Size(88, 23);
            this.btnGetMachines.TabIndex = 17;
            this.btnGetMachines.Text = "Get Machines";
            this.btnGetMachines.UseVisualStyleBackColor = true;
            this.btnGetMachines.Click += new System.EventHandler(this.btnGetMachines_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(177, 43);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(330, 20);
            this.txtAddress.TabIndex = 19;
            this.txtAddress.Text = "http://localhost:12321";
            this.txtAddress.TextChanged += new System.EventHandler(this.txtAddress_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Session";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(126, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Request Body";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(126, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Method";
            // 
            // txtMethod
            // 
            this.txtMethod.Location = new System.Drawing.Point(177, 127);
            this.txtMethod.Name = "txtMethod";
            this.txtMethod.Size = new System.Drawing.Size(61, 20);
            this.txtMethod.TabIndex = 23;
            this.txtMethod.Text = "GET";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(514, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Response Data";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(126, 402);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Messages";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(432, 17);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 21);
            this.btnExecute.TabIndex = 27;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnGetMachStats);
            this.groupBox1.Controls.Add(this.btnSignOn);
            this.groupBox1.Controls.Add(this.btnSignOff);
            this.groupBox1.Controls.Add(this.btnGetTiers);
            this.groupBox1.Controls.Add(this.btnGetMembers);
            this.groupBox1.Controls.Add(this.btnGetTier);
            this.groupBox1.Controls.Add(this.btnGetMember);
            this.groupBox1.Controls.Add(this.btnMemAtLoc);
            this.groupBox1.Controls.Add(this.btnUpdAcct);
            this.groupBox1.Controls.Add(this.btnGetMachine);
            this.groupBox1.Controls.Add(this.btnGetMachines);
            this.groupBox1.Location = new System.Drawing.Point(12, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(108, 475);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // btnGetMachStats
            // 
            this.btnGetMachStats.Location = new System.Drawing.Point(9, 339);
            this.btnGetMachStats.Name = "btnGetMachStats";
            this.btnGetMachStats.Size = new System.Drawing.Size(88, 37);
            this.btnGetMachStats.TabIndex = 18;
            this.btnGetMachStats.Text = "Get Machine Stats";
            this.btnGetMachStats.UseVisualStyleBackColor = true;
            this.btnGetMachStats.Click += new System.EventHandler(this.btnGetMachStats_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(244, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Content Type";
            // 
            // txtContentType
            // 
            this.txtContentType.Location = new System.Drawing.Point(321, 127);
            this.txtContentType.Name = "txtContentType";
            this.txtContentType.Size = new System.Drawing.Size(186, 20);
            this.txtContentType.TabIndex = 29;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 504);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtContentType);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMethod);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUri);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.txtSession);
            this.Name = "Form1";
            this.Text = "Rest Interface Test Harness";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSignOn;
        private System.Windows.Forms.Button btnSignOff;
        private System.Windows.Forms.TextBox txtSession;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.Button btnGetTiers;
        private System.Windows.Forms.Button btnGetMembers;
        private System.Windows.Forms.TextBox txtUri;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetTier;
        private System.Windows.Forms.Button btnGetMember;
        private System.Windows.Forms.Button btnMemAtLoc;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Button btnUpdAcct;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.Button btnGetMachine;
        private System.Windows.Forms.Button btnGetMachines;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMethod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtContentType;
        private System.Windows.Forms.Button btnGetMachStats;
    }
}

