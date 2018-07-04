using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using WindowsClient.SecureService;

using System.ServiceModel.Security;

namespace WindowsClient
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private SecureServiceClient proxy = null;

    private void CreateProxy()
    {
      if (tcpRadioButton.Checked == true)
      {
        proxy = new SecureServiceClient("SecureService_Tcp");
      }
      else 
      {
        proxy = new SecureServiceClient("SecureService_WsHttp");
      }
    }

    private void sayHelloButton_Click(object sender, EventArgs e)
    {
      CreateProxy();
      try
      {
        MessageBox.Show(proxy.SayHello());
      }
      catch (FaultException faultEx)
      {
        MessageBox.Show(faultEx.Message);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private void reportSalesButton_Click(object sender, EventArgs e)
    {
      CreateProxy();
      try
      {
          MessageBox.Show(String.Format(
            "Today's sales are {0:C}", proxy.ReportSales()));
      }
      catch (SecurityAccessDeniedException securityEx)
      {
          MessageBox.Show(securityEx.Message);
      }
      catch (FaultException faultEx)
      {
          MessageBox.Show(faultEx.Message);
      }
      catch (Exception ex)
      {
          MessageBox.Show(ex.Message);
      }
    }

  }
}
