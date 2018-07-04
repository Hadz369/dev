using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace StripCDataTags
{
    class Program
    {
        static void Main(string[] args)
        {
            string file;

            if (args.Length == 0)
            {
                Console.WriteLine("Input file not specified");
            }
            else
            {
                file = args[0];

                if (File.Exists(file))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    ConvertCdataToText(doc, doc.DocumentElement);
                    doc.Save(Console.Out);
                }
                else
                {
                    Console.Write("File not found");
                }
            }
        }

        static void ConvertCdataToText(XmlDocument doc, XmlNode root)
        {
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.CDATA)
                {
                    string text = n.InnerText;

                    text = text.Replace(">", "").Replace("<", "");
                    n.InnerText = text;

                    XmlNode foo = doc.CreateNode(XmlNodeType.Text, n.Name, "");
                    foo.InnerText = text;

                    root.ReplaceChild(foo, n);
                }
                else if (n.NodeType == XmlNodeType.Element)
                    ConvertCdataToText(doc, n);
            }
        }
    }
}
