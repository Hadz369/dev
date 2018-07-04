using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using Ruby.Core;
using System.IO;
using System.Data;

namespace Ruby.Data
{
    public static class XmlSerialiser
    {
        public static string DataSetToXml(DataSet dataSet)
        {
            string xml = String.Format("<dataset name=\"{0}\" tables=\"{1}\">\n", dataSet.DataSetName, dataSet.Tables.Count);

            foreach (DataTable dt in dataSet.Tables)
            {
                xml = String.Concat(xml, DataTableToXml(dt));
            }

            xml = String.Concat(xml, "</dataset>");

            return PrepareXml(xml);
        }

        public static DataSet DataSetFromXml(string xml)
        {
            DataSet ds = null;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlElement root = doc.DocumentElement;
            if (root.Name == "dataset")
            {
                ds = new DataSet(root.GetAttribute("name"));

                XmlNodeList tables = root.GetElementsByTagName("datatable");

                foreach (XmlElement e in tables)
                {
                    ds.Tables.Add(DataTableFromXmlElement(e));
                }
            }

            return ds;
        }

        public static string DataTableToXml(DataTable dataTable)
        {
            int cx = 0, rx = 0;

            string xml = String.Format("<datatable name=\"{0}\" columns=\"{1}\" rows=\"{2}\">\n", 
                dataTable.TableName, dataTable.Columns.Count, dataTable.Rows.Count);

            string row = "", cols = "";

            foreach (DataColumn c in dataTable.Columns)
            {
                cols = String.Format("{0}<col ix=\"{1}\" name=\"{2}\" type=\"{3}\" />",
                    cols, cx++, c.ColumnName, c.DataType.FullName);
            }
            row = String.Concat(new String[] { "<columns>", cols, "</columns>" });
            xml = String.Concat(xml, row);

            xml = String.Concat(xml, "<rows>");

            foreach (DataRow r in dataTable.Rows)
            {
                cols = "";
                cx = 0;
                foreach (DataColumn c in dataTable.Columns)
                {
                    cols = String.Format("{0}<cell ix=\"{1}\" value=\"{2}\" />", cols, cx++, r[c].ToString());
                }
                row = String.Format("<row ix=\"{0}\">{1}</row>", rx++, cols);
                xml = String.Concat(xml, row);
            }
            
            xml = String.Concat(xml, "</rows>");
            xml = String.Concat(xml, "</datatable>");

            return PrepareXml(xml);
        }

        public static DataTable DataTableFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return DataTableFromXmlElement(doc.DocumentElement);
        }

        private static DataTable DataTableFromXmlElement(XmlElement element)
        {
            DataTable dt = null;

            if (element.Name == "datatable")
            {
                dt = new DataTable(element.GetAttribute("name"));

                XmlNodeList columns = element.GetElementsByTagName("columns");
                if (columns.Count > 0)
                {
                    foreach (XmlElement e in columns[0].ChildNodes)
                    {
                        dt.Columns.Add(new DataColumn(e.GetAttribute("name"), Type.GetType(e.GetAttribute("type"))));
                    }
                }

                XmlNodeList rows = element.GetElementsByTagName("rows");
                if (rows.Count > 0)
                {
                    int x = 0, y = 0;

                    foreach (XmlNode row in rows[0].ChildNodes)
                    {
                        DataRow r = dt.NewRow();

                        y = 0;

                        foreach (XmlElement cell in row.ChildNodes)
                        {
                            r[y++] = cell.GetAttribute("value");
                        }

                        dt.Rows.Add(r);

                        x++;
                    }
                }
            }

            return dt;
        }

        public static string Serialise(object obj)
        {
            string str = "";

            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xs = new XmlSerializer(obj.GetType());
                xs.Serialize(ms, obj);

                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    str = sr.ReadToEnd();
                }
            }

            return str;
        }
        
        public static String PrepareXml(String xml)
        {
            string result = "";

            using (MemoryStream mStream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {

                    XmlDocument document = new XmlDocument();

                    try
                    {
                        // Load the XmlDocument with the XML.
                        document.LoadXml(xml);

                        writer.Formatting = Formatting.Indented;

                        // Write the XML into a formatting XmlTextWriter
                        document.WriteContentTo(writer);
                        writer.Flush();
                        mStream.Flush();

                        // Have to rewind the MemoryStream in order to read its contents.
                        mStream.Position = 0;

                        // Read MemoryStream contents into a StreamReader.
                        StreamReader sReader = new StreamReader(mStream);

                        // Extract the text from the StreamReader.
                        String FormattedXML = sReader.ReadToEnd();

                        result = FormattedXML;
                    }
                    catch (XmlException ex)
                    {
                        Tracer.Error("Error reformatting XML", ex);
                    }

                    writer.Close();
                }

                mStream.Close();
            }

            return result;
        }
    }
}
