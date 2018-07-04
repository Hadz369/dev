using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Alma.Updater
{
    public class Updater
    {
        public Updater(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);

            Updates = new List<Update>();
        }

        public int Id { get; private set; }
        public List<Server> Servers { get; private set; }
        public List<Update> Updates { get; private set; }

        public bool Run()
        {
            bool _success = true;

            foreach (Update u in Updates)
            {
                try
                {
                    u.PerformUpdate();
                }
                catch(Exception ex)
                {
                    //todo: Log error message
                    _success = false;
                    break;
                }
            }

            if (!_success)
            {
                foreach (Update u in Updates)
                {
                    if (u.Updated)
                        u.RollbackUpdate();
                }
            }

            return _success;
        }
    }

    public enum ServerType
    {
        HTTP = 0,
        FTP  = 1,
    }

    /// <summary>
    /// The server class holds the details of servers that can be used to download updates from.
    /// </summary>
    public class Server
    {
        public Server(XmlElement xml)
        {
            try
            {
                ServerType = (ServerType)Enum.Parse(typeof(ServerType), xml.Attributes["type"].Value);
                Address = xml.Attributes["name"].Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing server element. One or both of the name or type attributes is invalid.", ex);
            }
        }

        public ServerType ServerType { get; private set; }
        public string     Address    { get; private set; }
    }

    public enum UpdateAction
    {
        Replace = 0,
        Remove  = 1,
        Merge   = 2,
        Append  = 3,
    }

    public class Update
    {
        bool _updated = false;

        public Update(XmlElement xml)
        {
            if (xml.HasAttributes)
            {
                foreach (XmlAttribute a in xml.Attributes)
                {
                    switch (a.Name.ToLower())
                    {
                        case "action":
                            UpdateAction = (UpdateAction)Enum.Parse(typeof(UpdateAction), a.Value, true);
                            break;
                        case "filename":
                            FileName = a.Value;
                            break;
                        case "filehash":
                            FileHash = a.Value;
                            break;
                        case "sourcepath":
                            SourcePath = a.Value;
                            break;
                        case "targetpath":
                            TargetPath = a.Value;
                            break;
                    }
                }
            }
        }
                
        public UpdateAction UpdateAction { get; private set; }
        public string       FileName     { get; private set; }
        public string       FileHash     { get; private set; }
        public string       SourcePath   { get; private set; }
        public string       TargetPath   { get; private set; }
        public bool         Updated      { get { return _updated; } }

        public void PerformUpdate()
        {
            _updated = true;
        }

        public void RollbackUpdate()
        {
        }
    }
}
