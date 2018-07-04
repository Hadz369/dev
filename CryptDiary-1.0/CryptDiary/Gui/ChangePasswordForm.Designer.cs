namespace CryptDiary.Gui
{
    partial class ChangePasswordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textLabel1 = new System.Windows.Forms.Label();
            this.workDirectoryLabel = new System.Windows.Forms.Label();
            this.oldPasswordTextBox = new System.Windows.Forms.TextBox();
            this.textLabel2 = new System.Windows.Forms.Label();
            this.newPasswordTextBox1 = new System.Windows.Forms.TextBox();
            this.textLabel3 = new System.Windows.Forms.Label();
            this.newPasswordTextBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // textLabel1
            // 
            resources.ApplyResources(this.textLabel1, "textLabel1");
            this.textLabel1.Name = "textLabel1";
            // 
            // workDirectoryLabel
            // 
            resources.ApplyResources(this.workDirectoryLabel, "workDirectoryLabel");
            this.workDirectoryLabel.Name = "workDirectoryLabel";
            // 
            // oldPasswordTextBox
            // 
            resources.ApplyResources(this.oldPasswordTextBox, "oldPasswordTextBox");
            this.oldPasswordTextBox.Name = "oldPasswordTextBox";
            this.oldPasswordTextBox.UseSystemPasswordChar = true;
            this.oldPasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextBox_KeyDown);
            // 
            // textLabel2
            // 
            resources.ApplyResources(this.textLabel2, "textLabel2");
            this.textLabel2.Name = "textLabel2";
            // 
            // newPasswordTextBox1
            // 
            resources.ApplyResources(this.newPasswordTextBox1, "newPasswordTextBox1");
            this.newPasswordTextBox1.Name = "newPasswordTextBox1";
            this.newPasswordTextBox1.UseSystemPasswordChar = true;
            this.newPasswordTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextBox_KeyDown);
            // 
            // textLabel3
            // 
            resources.ApplyResources(this.textLabel3, "textLabel3");
            this.textLabel3.Name = "textLabel3";
            // 
            // newPasswordTextBox2
            // 
            resources.ApplyResources(this.newPasswordTextBox2, "newPasswordTextBox2");
            this.newPasswordTextBox2.Name = "newPasswordTextBox2";
            this.newPasswordTextBox2.UseSystemPasswordChar = true;
            this.newPasswordTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextBox_KeyDown);
            // 
            // ChangePasswordForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.textLabel3);
            this.Controls.Add(this.newPasswordTextBox2);
            this.Controls.Add(this.textLabel2);
            this.Controls.Add(this.newPasswordTextBox1);
            this.Controls.Add(this.workDirectoryLabel);
            this.Controls.Add(this.textLabel1);
            this.Controls.Add(this.oldPasswordTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ChangePasswordForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.PasswordForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label textLabel1;
        private System.Windows.Forms.Label workDirectoryLabel;
        public System.Windows.Forms.TextBox oldPasswordTextBox;
        private System.Windows.Forms.Label textLabel2;
        public System.Windows.Forms.TextBox newPasswordTextBox1;
        private System.Windows.Forms.Label textLabel3;
        public System.Windows.Forms.TextBox newPasswordTextBox2;
    }
}