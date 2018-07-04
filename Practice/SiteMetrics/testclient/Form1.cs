using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fnDataServer;
using System.ServiceModel;

namespace testclient
{
    public partial class Form1 : Form
    {
        ChannelFactory<IService> scf;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            scf = new ChannelFactory<IService>(new NetTcpBinding(), "net.tcp://localhost:8000");
        }

        private void buttonSignOn_Click(object sender, EventArgs e)
        {
            try
            {
                using (IService s = scf.CreateChannel())
                {
                    SignonResponseData rd = s.SignOn(new SignonData(Convert.ToInt32(textBox1.Text), textBox2.Text));

                    if (rd.Authorised)
                    {
                        listBox1.Items.Add(String.Format("Authorised: Handle={0}", rd.Handle));
                        listBox1.Items.Add(String.Format("State=" + scf.State.ToString()));

                        listBox1.Items.Add("Sending data packet");
                        DataPacket dp = new DataPacket(21, rd.Handle, ReadMeters());
                        s.PutDataPacket(dp);

                        listBox1.Items.Add("Sending data packet");
                        dp = new DataPacket(21, rd.Handle, ReadMeters());
                        s.PutDataPacket(dp);

                        listBox1.Items.Add("Sending data packet");
                        dp = new DataPacket(21, rd.Handle, ReadMeters());
                        s.PutDataPacket(dp);

                        s.SignOff(rd.Handle);

                        listBox1.Items.Add("Done");
                    }
                    else
                        listBox1.Items.Add(String.Format("Unauthorised: ErrCode={0}, ErrMessage={1}", rd.ErrorMessage.Code, rd.ErrorMessage.Message));

                }
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(String.Format("Error creating channel. " + ex.Message + ", " + ex.GetType().ToString()));
            }
        }

        DataTable ReadMeters()
        {
            SqlConnection con = new SqlConnection("Server=server.flexinetsystems.com.au;Database=EasyNet_1.0.29;User Id=flexinet;Password=s3cr3t;");
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from dbo.codes", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("codes");
            da.Fill(dt);

            return dt;
        }
    }
}
