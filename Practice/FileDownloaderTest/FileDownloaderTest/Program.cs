using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace FileDownloaderTest
{
    class Program
    {
        /*
        public static void Main ()
        {
            string path = "ftp://ftp.flexinetsystems.com.au//publish/release/1.0.30/";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);

            request.Credentials = new NetworkCredential("flexinet", "flexi");

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        Console.WriteLine(reader.ReadToEnd());
                    }
                }
                Console.WriteLine("Download Complete, status {0}", response.StatusDescription);
            }
            Console.ReadLine();
        }
        */
        internal class MyWebClient : WebClient 
        {        
            protected override WebRequest GetWebRequest(Uri address) 
            {
                FtpWebRequest req = (FtpWebRequest)base.GetWebRequest(address);
                req.UsePassive = false;
                return req;
            }
        }

        static void Main(string[] args)
        {
            CredentialCache cache = new CredentialCache();
            NetworkCredential credential = new NetworkCredential("flexinet", "flexi");

            //FtpWebRequest ftp = WebRequest.Create("ftp://ftp.flexinetsystems.com.au//publish/release/1.0.30/FlexiNetSetup.exe") as FtpWebRequest;
            //ftp.Credentials = credential;

            //ftp.Method = WebRequestMethods.Ftp.GetFileSize;
            //FtpWebResponse r = ftp.GetResponse() as FtpWebResponse;
            //Console.WriteLine(r.ContentLength);

            MyWebClient client = new MyWebClient();
            client.Credentials = credential;

            Uri uri = new Uri("ftp://ftp.flexinetsystems.com.au");
            
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);

            client.DownloadFileAsync(uri, "/publish/release/1.0.31/FlexiNetSetup_1.0.31.21.exe");
            //System.Diagnostics.Process.Start("1.jpg");

            Console.ReadLine();
        }

        static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage.ToString());
        }
    }
}
