namespace WindowsClient
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
      this.reportSalesButton = new System.Windows.Forms.Button();
      this.sayHelloButton = new System.Windows.Forms.Button();
      this.GroupBox1 = new System.Windows.Forms.GroupBox();
      this.wsHttpRadioButton = new System.Windows.Forms.RadioButton();
      this.tcpRadioButton = new System.Windows.Forms.RadioButton();
      this.GroupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // reportSalesButton
      // 
      this.reportSalesButton.Location = new System.Drawing.Point(196, 132);
      this.reportSalesButton.Name = "reportSalesButton";
      this.reportSalesButton.Size = new System.Drawing.Size(125, 39);
      this.reportSalesButton.TabIndex = 2;
      this.reportSalesButton.Text = "Report sales";
      this.reportSalesButton.UseVisualStyleBackColor = true;
      this.reportSalesButton.Click += new System.EventHandler(this.reportSalesButton_Click);
      // 
      // sayHelloButton
      // 
      this.sayHelloButton.Location = new System.Drawing.Point(63, 132);
      this.sayHelloButton.Name = "sayHelloButton";
      this.sayHelloButton.Size = new System.Drawing.Size(125, 39);
      this.sayHelloButton.TabIndex = 1;
      this.sayHelloButton.Text = "Say hello";
      this.sayHelloButton.UseVisualStyleBackColor = true;
      this.sayHelloButton.Click += new System.EventHandler(this.sayHelloButton_Click);
      // 
      // GroupBox1
      // 
      this.GroupBox1.Controls.Add(this.wsHttpRadioButton);
      this.GroupBox1.Controls.Add(this.tcpRadioButton);
      this.GroupBox1.Location = new System.Drawing.Point(63, 43);
      this.GroupBox1.Name = "GroupBox1";
      this.GroupBox1.Size = new System.Drawing.Size(258, 54);
      this.GroupBox1.TabIndex = 0;
      this.GroupBox1.TabStop = false;
      this.GroupBox1.Text = "Select a binding";
      // 
      // wsHttpRadioButton
      // 
      this.wsHttpRadioButton.AutoSize = true;
      this.wsHttpRadioButton.Location = new System.Drawing.Point(132, 22);
      this.wsHttpRadioButton.Name = "wsHttpRadioButton";
      this.wsHttpRadioButton.Size = new System.Drawing.Size(121, 21);
      this.wsHttpRadioButton.TabIndex = 1;
      this.wsHttpRadioButton.Text = "WSHttpBinding";
      this.wsHttpRadioButton.UseVisualStyleBackColor = true;
      // 
      // tcpRadioButton
      // 
      this.tcpRadioButton.AutoSize = true;
      this.tcpRadioButton.Checked = true;
      this.tcpRadioButton.Location = new System.Drawing.Point(7, 23);
      this.tcpRadioButton.Name = "tcpRadioButton";
      this.tcpRadioButton.Size = new System.Drawing.Size(119, 21);
      this.tcpRadioButton.TabIndex = 0;
      this.tcpRadioButton.TabStop = true;
      this.tcpRadioButton.Text = "NetTcpBinding";
      this.tcpRadioButton.UseVisualStyleBackColor = true;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 215);
      this.Controls.Add(this.GroupBox1);
      this.Controls.Add(this.sayHelloButton);
      this.Controls.Add(this.reportSalesButton);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Authentication and Authorization Demo";
      this.GroupBox1.ResumeLayout(false);
      this.GroupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    internal System.Windows.Forms.Button reportSalesButton;
    internal System.Windows.Forms.Button sayHelloButton;
    internal System.Windows.Forms.GroupBox GroupBox1;
    internal System.Windows.Forms.RadioButton wsHttpRadioButton;
    internal System.Windows.Forms.RadioButton tcpRadioButton;
  }
}

