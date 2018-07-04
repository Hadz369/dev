using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace fnDataServer.Client
{
    class Client
    {
        static ChannelFactory<IService> scf;

        static void Main(string[] args)
        {
            scf = new ChannelFactory<IService>(new NetTcpBinding(),
                String.Format("net.tcp://{0}:8000", Properties.Settings.Default.Server));

            while (true)
            {

                Console.Write("CLIENT - Name: ");

                string name = Console.ReadLine();

                if (name == "") break;

                try
                {
                    using(IService s = scf.CreateChannel())
                    {
                        SignonResponseData rd = s.SignOn(new SignonData(56, "My Site"));

                        if (rd.Authorised)
                        {
                            Console.WriteLine("Authorised: Handle={0}", rd.Handle);
                            Console.WriteLine("State=" + scf.State.ToString());

                            Console.WriteLine("Sending data packet");
                            DataPacket dp = new DataPacket(21, rd.Handle, ReadMeters());
                            s.PutDataPacket(dp);

                            Console.WriteLine("Sending data packet");
                            dp = new DataPacket(21, rd.Handle, ReadMeters());
                            s.PutDataPacket(dp);

                            Console.WriteLine("Sending data packet");
                            dp = new DataPacket(21, rd.Handle, ReadMeters());
                            s.PutDataPacket(dp);
                            
                            s.SignOff(rd.Handle);

                            /*
                            Console.WriteLine("Sending meters");
                            s.PutDailyMeters(rd.Handle, ReadMeters());
                            */
                            Console.WriteLine("Done");
                        }
                        else
                            Console.WriteLine("Unauthorised: ErrCode={0}, ErrMessage={1}", rd.ErrorMessage.Code, rd.ErrorMessage.Message);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating channel. " + ex.Message + ", " + ex.GetType().ToString());
                }
            }
        }

        static DataTable ReadMeters()
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
