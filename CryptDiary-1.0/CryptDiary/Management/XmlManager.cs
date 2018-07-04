using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using CryptDiary.Data;
using System.Resources;

namespace CryptDiary.Management
{
    public class XmlManager
    {
        // localisation of messages
        private static ResourceManager messageManager = new ResourceManager("CryptDiary.Resources.Messages", typeof(XmlManager).Assembly);

        /// <summary>
        /// creates a filename (pattern workDirectory\yyyy-MM-dd.xml)
        /// </summary>
        /// <param name="date">date to create file name from</param>
        /// <returns>filename workDirectory\yyyy-MM-dd.xml</returns>
        static public string DateToXmlFileName(DateTime date)
        {
            // put together file name from date parts
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            if (month.Length < 2)
                month = "0" + month;
            if (day.Length < 2)
                day = "0" + day;
            string fileName = year + "-" + month + "-" + day + ".xml";
            return fileName;
        }

        /// <summary>
        /// saves a diaryEntry object to an encrypted xml file (yyyy-MM-dd.xml) in the workDirectory
        /// </summary>
        /// <param name="diaryEntry">diaryEntry to save</param>
        /// <param name="password">password uses for encryption</param>
        /// <param name="workDir">directory in which file will be saved</param>
        static public void DiaryEntryToEncryptedXmlFile(DiaryEntry diaryEntry, string password, string workDir)
        {
            #region check arguments
            // check arguments
            if (diaryEntry == null)
            {
                throw new ArgumentNullException("diaryEntry");
            }
            if (password == null || password == "")
            {
                throw new ArgumentNullException("password cannot be NULL or empty");
            }
            if (workDir == null || workDir == "")
            {
                throw new ArgumentNullException("workDir cannot be NULL or empty");
            }
            if (!Directory.Exists(workDir))
            {
                throw new ArgumentOutOfRangeException("workDir " + workDir + " doesn't exist.");
            }
            #endregion

            // create plaintext xml document (only in memory)
            XmlDocument doc = DiaryEntryToPlainXml(diaryEntry);

            // encrypt plaintext xml document
            try
            {
                AesCrypt.EncryptXmlDocument(doc, Properties.Resources.XmlRootNodeNameDiary, password, new DiarySettings().PasswordIterations);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\nError at encrypting Xml document.");
            }

            // get file name to store the encrypted Xml document
            string fileName = workDir + "\\" + DateToXmlFileName(diaryEntry.Date);

            try
            {
                doc.Save(fileName);
            }
            catch (Exception)
            {
                throw new Exception(messageManager.GetString("ErrorFileCanNotBeWritten"));
            }
        }

        /// <summary>
        /// decrypts an xml file for a specified date and return a diaryEntry object
        /// </summary>
        /// <param name="date">date, to which a corresponding Xml file will be searched</param>
        /// <param name="password">password, which was used at encryption</param>
        /// <param name="workDir">directory, where to look for Xml file</param>
        /// <returns>diaryEntry</returns>
        static public DiaryEntry EncryptedXmlFileToDiaryEntry(DateTime date, string password, string workDir)
        {
            // check arguments
            if (date == new DateTime())
            {
                throw new ArgumentOutOfRangeException("no date was given");
            }
            if (password == "" || password == null)
            {
                throw new ArgumentNullException("empty password not allowed");
            }
            if (workDir == "" || workDir == null)
            {
                throw new ArgumentNullException("workDir can not be empty");
            }
            if (!Directory.Exists(workDir))
            {
                throw new ArgumentOutOfRangeException("workDir " + workDir + "doesn't exist");
            }

            string fileName = workDir + "\\" + DateToXmlFileName(date);

            // get informations from Xml file
            // load Xml file
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            // decrypt Xml file
            try
            {
                doc = AesCrypt.DecryptXmlDocument(doc, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // convert decrypted document to diaryEntry
            DiaryEntry diaryEntry = new DiaryEntry();
            diaryEntry = PlainXmlToDiaryEntry(doc);
            return diaryEntry;
        }

        /// <summary>
        /// takes the hashtags of a diaryEntry and adds it to the encrypted hashtags.xml file
        /// checkes also for doubled hashtags, dates, orphaned hashtags etc.
        /// </summary>
        /// <param name="diaryEntry">diaryEntry which contains the hashtags to be added</param>
        /// <param name="password">password needed for encryption of hashtags.xml</param>
        /// <param name="workDir">directory, where the diaryEntries and the hashtags.xml are</param>
        static public void DiaryEntryHashtagsToEncryptedXmlFile(DiaryEntry diaryEntry, string password, string workDir)
        {
            // check arguments
            if (diaryEntry == null)
            {
                throw new ArgumentNullException("diaryEntry can not be NULL");
            }
            if (password == null || password == "")
            {
                throw new ArgumentNullException("password can not be NULL or empty");
            }
            if (workDir == null)
            {
                throw new ArgumentNullException("workDir can not be NULL");
            }
            if (!Directory.Exists(workDir))
            {
                throw new ArgumentOutOfRangeException("workDir " + workDir + " doesn't exist");
            }

            try
            {
                // get a plain Xml document, which contains all "old" hashtags and the ones from the given diaryEntry
                XmlDocument doc = DiaryEntryHashtagsToPlainXml(diaryEntry, password, workDir);
                // encrypt this document and save it as hashtags.xml
                doc = AesCrypt.EncryptXmlDocument(doc, Properties.Resources.XmlRootNodeNameHashtags, password, new DiarySettings().PasswordIterations);
                doc.Save(workDir + "\\" + Properties.Resources.HashtagsFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// takes collected hashtags from hashtagsDictionary and writes it to encrypted hashtags.xml file
        /// </summary>
        /// <param name="hashtagDictionary"></param>
        /// <param name="password"></param>
        /// <param name="workDir"></param>
        static public void HashtagDictionaryToEncryptedXmlFile(HashtagDictionary hashtagDictionary, string password, string workDir)
        {
            // check arguments
            if (hashtagDictionary == null)
            {
                throw new ArgumentNullException("hashtagDictionary can not be NULL");
            }
            if (password == null || password == "")
            {
                throw new ArgumentNullException("password can not be NULL or empty");
            }
            if (workDir == null)
            {
                throw new ArgumentNullException("workDir can not be NULL");
            }
            if (!Directory.Exists(workDir))
            {
                throw new ArgumentOutOfRangeException("workDir " + workDir + " doesn't exist");
            }

            // make a plain XmlDocument out of the hashtagDictionary
            XmlDocument doc = HashtagsDictionaryToPlainXml(hashtagDictionary);

            // encrypt it
            try
            {
                doc = AesCrypt.EncryptXmlDocument(doc, Properties.Resources.XmlRootNodeNameHashtags, password, new DiarySettings().PasswordIterations);

            }
            catch (Exception ex)
            {
                throw new XmlException("error at encrypting the hashtags document. " + ex.Message);
            }

            // and save it
            try
            {
                doc.Save(workDir + "\\" + Properties.Resources.HashtagsFileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Encrypted hashtags.xml file could not be saved. " + ex.Message);
            }
        }

        /// <summary>
        /// gets all hashtags with its corresponding dates from an encrypted hashtags.xml file
        /// </summary>
        /// <param name="password">password to decrypt hashtags.xml</param>
        /// <param name="workDir">directory, which contains hashtags.xml</param>
        /// <returns>HashtagDictionary</returns>
        static public HashtagDictionary EncryptedXmlFileToHashtagDictionary(string password, string workDir)
        {
            // check arguments
            if (password == null || password == "")
            {
                throw new ArgumentNullException("password can not be NULL or empty");
            }
            if (workDir == null)
            {
                throw new ArgumentNullException("workDir can not be NULL");
            }
            if (!Directory.Exists(workDir))
            {
                throw new ArgumentOutOfRangeException("workDir " + workDir + " doesn't exist");
            }

            // decrypt hashtags.xml if it exists, else return null
            if (File.Exists(workDir + "\\" + Properties.Resources.HashtagsFileName))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(workDir + "\\" + Properties.Resources.HashtagsFileName);
                }
                catch (Exception ex)
                {
                    throw new FileLoadException("hashtags.xml exists but could not be loaded." + ex.Message);
                }
                try
                {
                    doc = AesCrypt.DecryptXmlDocument(doc, password);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                HashtagDictionary hashtagDictionary = new HashtagDictionary();
                try
                {
                    hashtagDictionary = PlainXmlToHashtagDictionary(doc);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return hashtagDictionary;
            }
            else return null;
        }

        /// <summary>
        /// generates an Xml document from a diaryEntry object
        /// </summary>
        /// <param name="diaryEntry">diaryEntry to be converted</param>
        /// <returns>plaintext Xml document</returns>
        static private XmlDocument DiaryEntryToPlainXml(DiaryEntry diaryEntry)
        {
            XmlDocument doc = GetNewXmlDocumentWithDeclarationAndRootNode(Properties.Resources.XmlRootNodeNameDiary);

            // diaryEntry node
            XmlNode diaryEntryNode = doc.CreateElement("Entry");

            // date attribute
            XmlAttribute dateAttribute = doc.CreateAttribute("Date");
            dateAttribute.Value = diaryEntry.Date.ToShortDateString();

            // text node
            XmlNode textNode = doc.CreateElement("Text");
            textNode.InnerText = diaryEntry.Text;

            // put document together
            diaryEntryNode.Attributes.Append(dateAttribute);
            diaryEntryNode.AppendChild(textNode);
            doc.LastChild.AppendChild(diaryEntryNode);

            return doc;
        }

        /// <summary>
        /// generates a diaryEntry from a plaintext Xml file
        /// </summary>
        /// <param name="doc">plaintext Xml document</param>
        /// <returns></returns>
        static private DiaryEntry PlainXmlToDiaryEntry(XmlDocument doc)
        {
            DiaryEntry diaryEntry = new DiaryEntry();

            // read informations from xml structure
            XmlNode diaryEntryNode = doc.GetElementsByTagName("Entry")[0];
            string date = "";
            try
            {
                date = diaryEntryNode.Attributes["Date"].Value;
            }
            catch (XmlException ex)
            {
                throw new XmlException(messageManager.GetString("ErrorXmlAttributeDateNotFound") + "\n" + ex.Message);
            }
            XmlNode textNode = doc.GetElementsByTagName("Text")[0];

            // fill diaryEntry
            try
            {
                diaryEntry.Date = DateTime.Parse(date);
            }
            catch (FormatException)
            {
                throw new FormatException(messageManager.GetString("ErrorWrongDateFormat"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\nError at parsing date.");
            }

            diaryEntry.Text = textNode.InnerText;

            return diaryEntry;
        }

        /// <summary>
        /// takes a diaryEntry and "merges" its hashtags into already available hashtags in encrypted hashtags.xml file,
        /// so decryption is neede prior
        /// </summary>
        /// <param name="diaryEntry">DiaryEntry with hashtags</param>
        /// <param name="password">password to decrypt hashtags.xml</param>
        /// <param name="workDir">directory which contains hashtags.xml</param>
        /// <returns></returns>
        static private XmlDocument DiaryEntryHashtagsToPlainXml(DiaryEntry diaryEntry, string password, string workDir)
        {
            // check arguments
            if (diaryEntry == null)
            {
                throw new ArgumentNullException("diary entry can not be NULL.");
            }
            if (workDir == null || workDir == "")
            {
                throw new ArgumentNullException("workDir can not be NULL.");
            }
            if (!Directory.Exists(workDir))
            {
                throw new ArgumentOutOfRangeException("workDir " + workDir + " doesn't exist.");
            }
            HashtagDictionary hashtagDictionary = new HashtagDictionary();

            XmlDocument doc = new XmlDocument();

            // check, if hashtags-file already exists and try to decrypt it
            string pathFileName = workDir + "\\" + Properties.Resources.HashtagsFileName;
            if (File.Exists(pathFileName))
            {
                try
                {
                    doc.Load(pathFileName);
                    doc = AesCrypt.DecryptXmlDocument(doc, password);
                }
                catch (Exception ex)
                {
                    throw new XmlException("Hashtags could not be decrypted. " + ex.Message);
                }

                hashtagDictionary = PlainXmlToHashtagDictionary(doc);
            }

            // update hashtagDictionary (merge it with diaryEntry's hashtags
            hashtagDictionary = Hashtag.EditHashtagsDictionary(hashtagDictionary, diaryEntry);

            // now create a new Xml document, which gets updated hashtag informations
            doc = HashtagsDictionaryToPlainXml(hashtagDictionary);

            return doc;
        }

        /// <summary>
        /// converts a hashtagDictionary object into an Xml document
        /// </summary>
        /// <param name="hashtagDictionary">HashtagDictionary object to convert</param>
        /// <returns></returns>
        static private XmlDocument HashtagsDictionaryToPlainXml(HashtagDictionary hashtagDictionary)
        {
            // check arguments
            if (hashtagDictionary == null)
            {
                throw new ArgumentNullException("hashtagDictionary can not be NULL");
            }

            XmlDocument doc = GetNewXmlDocumentWithDeclarationAndRootNode(Properties.Resources.XmlRootNodeNameHashtags);

            // create new XmlNode for every Hashtag
            foreach (var hashtag in hashtagDictionary)
            {
                XmlNode hashtagNode = doc.CreateElement("Hashtag");
                XmlNode hashtagTextNode = doc.CreateElement("Text");
                hashtagTextNode.InnerText = hashtag.Key.Text;
                hashtagNode.AppendChild(hashtagTextNode);

                foreach (var date in hashtag.Value)
                {
                    XmlNode hashtagDateNode = doc.CreateElement("Date");
                    hashtagDateNode.InnerText = date.ToShortDateString();
                    hashtagNode.AppendChild(hashtagDateNode);
                }
                doc.LastChild.AppendChild(hashtagNode);
            }

            return doc;
        }

        /// <summary>
        /// Gets a Hashtag dictionary out of an Xml document
        /// </summary>
        /// <param name="doc">the Xml document to search for hashtags</param>
        /// <returns></returns>
        static private HashtagDictionary PlainXmlToHashtagDictionary(XmlDocument doc)
        {
            HashtagDictionary hashtagDictionary = new HashtagDictionary();

            // get all "Hashtag" nodes
            var hashtagNodes = doc.GetElementsByTagName("Hashtag");
            if (hashtagNodes == null)
            {
                throw new XmlException("no \"Hashtag\" node found in document");
            }
            foreach (XmlNode hashtagNode in hashtagNodes)
            {
                string hashtagText = "";
                List<DateTime> hashtagDates = new List<DateTime>();
                foreach (XmlNode childNode in hashtagNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case ("Text"):
                            {
                                hashtagText = childNode.InnerText;
                                break;
                            }
                        case ("Date"):
                            {
                                DateTime date = new DateTime();
                                try
                                {
                                    date = DateTime.Parse(childNode.InnerText);
                                }
                                catch (Exception ex)
                                {
                                    throw new XmlException("Date " + childNode.InnerText + " could not be parsed. " + ex.Message);
                                }
                                hashtagDates.Add(date);
                                break;
                            }
                        default:
                            break;
                    }
                }
                hashtagDictionary.Add(new Hashtag(hashtagText), hashtagDates);
            }

            return hashtagDictionary;
        }

        /// <summary>
        /// creates a new Xml document with the declaration node and a root node with the given name
        /// </summary>
        /// <param name="rootNodeName">name of the Xml document's root node</param>
        /// <returns></returns>
        static private XmlDocument GetNewXmlDocumentWithDeclarationAndRootNode(string rootNodeName)
        {
            XmlDocument doc = new XmlDocument();

            // declarationNode node
            XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "utf-8", "no");
            doc.AppendChild(declarationNode);

            // root node
            XmlNode rootNode = doc.CreateElement(rootNodeName);
            // append root node to document
            doc.AppendChild(rootNode);

            return doc;
        }
    }
}
