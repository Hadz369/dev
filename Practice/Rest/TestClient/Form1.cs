using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;

namespace TestClient
{
    public partial class Form1 : Form
    {
        Guid _session = Guid.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Reset()
        {
            txtMessages.Clear();
            txtResponse.Clear();
            btnExecute.BackColor = SystemColors.Control;
        }

        private void btnSignOn_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/signon/?usr=User&psw=pass");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnSignOff_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/signoff/", _session.ToString());
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnGetTiers_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/tiers");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnGetTier_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/tiers/{TierId}");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnGetMembers_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/members/{Surname}");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnGetMember_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/member/{BadgeNo}");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnMemAtLoc_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/memberatlocation/{BaseNo}");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnUpdAcct_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "{\"MemberId\":2066,\"ChangeType\":\"iSSue\",\"Amount\":1000,\"PrizeId\":6}";
            txtUri.Text = String.Concat("tp1/account/", _session.ToString());
            txtMethod.Text = "PUT";
            txtContentType.Text = "application/json";
            HighlightExecute();
        }

        private void btnGetMachine_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/machine/101");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnGetMachines_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/machines");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void btnGetMachStats_Click(object sender, EventArgs e)
        {
            txtRequest.Text = "";
            txtUri.Text = String.Concat("tp1/getdata/", _session.ToString(), "/machinestats/{MachineId}");
            txtMethod.Text = "GET";
            txtContentType.Text = "";
            HighlightExecute();
        }

        private void HighlightExecute()
        {
            btnExecute.BackColor = Color.LightGreen;
        }

        private XmlDocument GetData(Uri uri)
        {
            XmlDocument doc = null;

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                string str;

                using (Stream data = client.OpenRead(uri))
                {
                    using (StreamReader reader = new StreamReader(data))
                    {
                        str = reader.ReadToEnd();
                    }
                }

                doc = new XmlDocument();
                doc.LoadXml(str);

                txtResponse.Text = XmlToString(doc);
            }

            return doc;
        }

        string XmlToString(XmlDocument doc)
        {
            using (var sw = new System.IO.StringWriter())
            {
                using (var xw = new System.Xml.XmlTextWriter(sw))
                {
                    xw.Formatting = System.Xml.Formatting.Indented;
                    //xw.Indentation = indentation;
                    doc.WriteContentTo(xw);
                }
                return sw.ToString();
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            Reset();

            Uri uri = new Uri(String.Concat(txtAddress.Text, "/", txtUri.Text));

            try
            {
                if (txtMethod.Text == "PUT" || txtMethod.Text == "POST")
                {
                    var request = (HttpWebRequest)WebRequest.Create(uri);
                    request.ContentType = txtContentType.Text;
                    request.Method = txtMethod.Text;

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(txtRequest.Text);
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(result);

                        txtResponse.Text = XmlToString(doc);
                    }
                }
                else
                {
                    XmlDocument doc = GetData(uri);
                    
                    if (doc != null)
                    {
                        if (uri.ToString().Contains("signoff"))
                        {
                            _session = Guid.Empty;
                            txtSession.Text = "";
                        }                        
                        else if (uri.ToString().Contains("signon"))
                        {
                            string str = "";

                            XmlNode n = doc.SelectSingleNode("ServiceResponse/ResponseData");

                            if (n != null)
                            {
                                str += String.Concat(str == "" ? "" : Environment.NewLine, n.Name, n.InnerText);
                                _session = new Guid(n.InnerText);
                                txtSession.Text = _session.ToString();
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                txtMessages.Text = ex.Message;
            }

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
