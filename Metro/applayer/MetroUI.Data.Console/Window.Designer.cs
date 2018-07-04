namespace Metro
{
    partial class Window
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAsyncTest = new System.Windows.Forms.Button();
            this.btnAsyncCallback = new System.Windows.Forms.Button();
            this.btnSynchronous = new System.Windows.Forms.Button();
            this.rbGetMemberList = new System.Windows.Forms.RadioButton();
            this.rbGetMemberTiers = new System.Windows.Forms.RadioButton();
            this.rbGetMachineList = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDateFrom = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tbCardNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSignOn = new System.Windows.Forms.Button();
            this.tbVendor = new System.Windows.Forms.TextBox();
            this.tbDevice = new System.Windows.Forms.TextBox();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.btnSignOff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "General";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(195, 157);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(577, 392);
            this.textBox1.TabIndex = 1;
            // 
            // btnAsyncTest
            // 
            this.btnAsyncTest.Location = new System.Drawing.Point(12, 268);
            this.btnAsyncTest.Name = "btnAsyncTest";
            this.btnAsyncTest.Size = new System.Drawing.Size(102, 23);
            this.btnAsyncTest.TabIndex = 2;
            this.btnAsyncTest.Text = "Async Wait";
            this.btnAsyncTest.UseVisualStyleBackColor = true;
            this.btnAsyncTest.Click += new System.EventHandler(this.btnAsyncTest_Click);
            // 
            // btnAsyncCallback
            // 
            this.btnAsyncCallback.Location = new System.Drawing.Point(12, 297);
            this.btnAsyncCallback.Name = "btnAsyncCallback";
            this.btnAsyncCallback.Size = new System.Drawing.Size(102, 23);
            this.btnAsyncCallback.TabIndex = 3;
            this.btnAsyncCallback.Text = "Async Callback";
            this.btnAsyncCallback.UseVisualStyleBackColor = true;
            this.btnAsyncCallback.Click += new System.EventHandler(this.btnAsyncCallback_Click);
            // 
            // btnSynchronous
            // 
            this.btnSynchronous.Location = new System.Drawing.Point(12, 239);
            this.btnSynchronous.Name = "btnSynchronous";
            this.btnSynchronous.Size = new System.Drawing.Size(102, 23);
            this.btnSynchronous.TabIndex = 4;
            this.btnSynchronous.Text = "Synchronous";
            this.btnSynchronous.UseVisualStyleBackColor = true;
            this.btnSynchronous.Click += new System.EventHandler(this.btnSynchronous_Click);
            // 
            // rbGetMemberList
            // 
            this.rbGetMemberList.AutoSize = true;
            this.rbGetMemberList.Checked = true;
            this.rbGetMemberList.Location = new System.Drawing.Point(13, 39);
            this.rbGetMemberList.Name = "rbGetMemberList";
            this.rbGetMemberList.Size = new System.Drawing.Size(96, 17);
            this.rbGetMemberList.TabIndex = 10;
            this.rbGetMemberList.TabStop = true;
            this.rbGetMemberList.Text = "GetMemberList";
            this.rbGetMemberList.UseVisualStyleBackColor = true;
            // 
            // rbGetMemberTiers
            // 
            this.rbGetMemberTiers.AutoSize = true;
            this.rbGetMemberTiers.Location = new System.Drawing.Point(13, 62);
            this.rbGetMemberTiers.Name = "rbGetMemberTiers";
            this.rbGetMemberTiers.Size = new System.Drawing.Size(103, 17);
            this.rbGetMemberTiers.TabIndex = 11;
            this.rbGetMemberTiers.Text = "GetMemberTiers";
            this.rbGetMemberTiers.UseVisualStyleBackColor = true;
            // 
            // rbGetMachineList
            // 
            this.rbGetMachineList.AutoSize = true;
            this.rbGetMachineList.Location = new System.Drawing.Point(13, 85);
            this.rbGetMachineList.Name = "rbGetMachineList";
            this.rbGetMachineList.Size = new System.Drawing.Size(99, 17);
            this.rbGetMachineList.TabIndex = 12;
            this.rbGetMachineList.Text = "GetMachineList";
            this.rbGetMachineList.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "DateFrom";
            // 
            // tbDateFrom
            // 
            this.tbDateFrom.Location = new System.Drawing.Point(230, 38);
            this.tbDateFrom.Name = "tbDateFrom";
            this.tbDateFrom.Size = new System.Drawing.Size(122, 20);
            this.tbDateFrom.TabIndex = 14;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(13, 108);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(98, 17);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.Text = "GetMemberInfo";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tbCardNumber
            // 
            this.tbCardNumber.Location = new System.Drawing.Point(230, 107);
            this.tbCardNumber.Name = "tbCardNumber";
            this.tbCardNumber.Size = new System.Drawing.Size(122, 20);
            this.tbCardNumber.TabIndex = 16;
            this.tbCardNumber.Text = "953=000011";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Card Number";
            // 
            // btnSignOn
            // 
            this.btnSignOn.Location = new System.Drawing.Point(12, 10);
            this.btnSignOn.Name = "btnSignOn";
            this.btnSignOn.Size = new System.Drawing.Size(97, 23);
            this.btnSignOn.TabIndex = 18;
            this.btnSignOn.Text = "Sign On";
            this.btnSignOn.UseVisualStyleBackColor = true;
            this.btnSignOn.Click += new System.EventHandler(this.btnSignOn_Click);
            // 
            // tbVendor
            // 
            this.tbVendor.Location = new System.Drawing.Point(116, 11);
            this.tbVendor.Name = "tbVendor";
            this.tbVendor.Size = new System.Drawing.Size(92, 20);
            this.tbVendor.TabIndex = 19;
            this.tbVendor.Text = "TestVendor";
            this.tbVendor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbDevice
            // 
            this.tbDevice.Location = new System.Drawing.Point(214, 11);
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Size = new System.Drawing.Size(92, 20);
            this.tbDevice.TabIndex = 20;
            this.tbDevice.Text = "101";
            this.tbDevice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbToken
            // 
            this.tbToken.Location = new System.Drawing.Point(312, 11);
            this.tbToken.Name = "tbToken";
            this.tbToken.ReadOnly = true;
            this.tbToken.Size = new System.Drawing.Size(93, 20);
            this.tbToken.TabIndex = 21;
            // 
            // btnSignOff
            // 
            this.btnSignOff.Location = new System.Drawing.Point(411, 9);
            this.btnSignOff.Name = "btnSignOff";
            this.btnSignOff.Size = new System.Drawing.Size(97, 23);
            this.btnSignOff.TabIndex = 22;
            this.btnSignOff.Text = "Sign Off";
            this.btnSignOff.UseVisualStyleBackColor = true;
            this.btnSignOff.Click += new System.EventHandler(this.btnSignOff_Click);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btnSignOff);
            this.Controls.Add(this.tbToken);
            this.Controls.Add(this.tbDevice);
            this.Controls.Add(this.tbVendor);
            this.Controls.Add(this.btnSignOn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbCardNumber);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.tbDateFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbGetMachineList);
            this.Controls.Add(this.rbGetMemberTiers);
            this.Controls.Add(this.rbGetMemberList);
            this.Controls.Add(this.btnSynchronous);
            this.Controls.Add(this.btnAsyncCallback);
            this.Controls.Add(this.btnAsyncTest);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Window";
            this.Text = "Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAsyncTest;
        private System.Windows.Forms.Button btnAsyncCallback;
        private System.Windows.Forms.Button btnSynchronous;
        private System.Windows.Forms.RadioButton rbGetMemberList;
        private System.Windows.Forms.RadioButton rbGetMemberTiers;
        private System.Windows.Forms.RadioButton rbGetMachineList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDateFrom;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox tbCardNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSignOn;
        private System.Windows.Forms.TextBox tbVendor;
        private System.Windows.Forms.TextBox tbDevice;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Button btnSignOff;
    }
}