using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using CryptDiary.Data;
using System.Resources;

namespace CryptDiary.Management
{
    public class AesCrypt
    {
        // localisation of messages
        static ResourceManager MessageManager = new ResourceManager("CryptDiary.Resources.Messages", typeof(AesCrypt).Assembly);

        /// <summary>
        /// Encrypts an Xml-Element with an AES algorithm
        /// </summary>
        /// <param name="doc">Xml document to encrypt</param>
        /// <param name="ElementName">name of the Xml element to encrypt</param>
        /// <param name="password">password to encrypt with</param>
        /// <param name="iterationCount">number of password iterations before creating AES key</param>
        /// <returns>encrypted Xml document</returns>
        static public XmlDocument EncryptXmlDocument(XmlDocument doc, string ElementName, string password, int iterationCount)
        {
            // Check the arguments.  
            if (doc == null)
                throw new ArgumentNullException("doc");
            if (password == null || password == "")
                throw new ArgumentNullException("password");

            // Find the specified element in the XmlDocument object and create a new XmlElement object.
            XmlElement elementToEncrypt = doc.GetElementsByTagName(ElementName)[0] as XmlElement;

            // Throw an XmlException if the element was not found.
            if (elementToEncrypt == null)
            {
                throw new XmlException(MessageManager.GetString("ErrorXmlElementNotFound"));
            }

            // create salt for password iterations
            byte[] salt = Generate128BitsOfRandomEntropy();

            // create Aes-Key
            var aesKey = GenerateAesKey(password, iterationCount, new DiarySettings().KeySize, salt);

            // Create a new instance of the EncryptedXml class and use it to encrypt the XmlElement with the AES key
            EncryptedXml eXml = new EncryptedXml();
            byte[] encryptedElement = eXml.EncryptData(elementToEncrypt, aesKey, false);

            // Construct an EncryptedData object and populate it with the desired encryption information.
            EncryptedData encryptedDataElement = new EncryptedData();
            encryptedDataElement.Type = EncryptedXml.XmlEncElementUrl;

            // Create an encryptionMethod element so that the receiver knows which algorithm to use for decryption.
            // Determine what kind of algorithm is being used and supply the appropriate URL to the encryptionMethod element.
            string encryptionMethod = null;
            switch (aesKey.KeySize)
            {
                case 128:
                    encryptionMethod = EncryptedXml.XmlEncAES128Url;
                    break;
                case 192:
                    encryptionMethod = EncryptedXml.XmlEncAES192Url;
                    break;
                case 256:
                    encryptionMethod = EncryptedXml.XmlEncAES256Url;
                    break;
                default:
                    throw new CryptographicException(MessageManager.GetString("ErrorInvalidKeyLength"));
            }
            encryptedDataElement.EncryptionMethod = new EncryptionMethod(encryptionMethod);

            // Add the encrypted element data to the EncryptedData object.
            encryptedDataElement.CipherData.CipherValue = encryptedElement;

            // Replace the element from the original XmlDocument object with the EncryptedData element.
            EncryptedXml.ReplaceElement(elementToEncrypt, encryptedDataElement, false);

            // append EncryptionInformation after encrypted Element
            #region EncryptionInformation
            XmlNode encryptionInformationNode = doc.CreateElement("EncryptionInformation");
            XmlNode iterationsNode = doc.CreateElement("IterationsUsed");
            XmlNode saltNode = doc.CreateElement("SaltUsed");
            XmlNode ivNode = doc.CreateElement("IVUsed");
            XmlNode keySize = doc.CreateElement("KeySize");

            iterationsNode.InnerText = iterationCount.ToString();
            saltNode.InnerText = Convert.ToBase64String(salt);
            ivNode.InnerText = Convert.ToBase64String(aesKey.IV);
            keySize.InnerText = new DiarySettings().KeySize.ToString();

            encryptionInformationNode.AppendChild(iterationsNode);
            encryptionInformationNode.AppendChild(saltNode);
            encryptionInformationNode.AppendChild(ivNode);
            encryptionInformationNode.AppendChild(keySize);

            doc.LastChild.AppendChild(encryptionInformationNode);
            #endregion

            return doc;
        }

        /// <summary>
        /// tries to decrypts an Xml document with specified password
        /// encryption informations like algorithmus used, key size, iterations used etc. have to be provided by the document
        /// </summary>
        /// <param name="doc">Xml document to decrypt</param>
        /// <param name="password">password to decrypt with</param>
        /// <returns>decrypted Xml document</returns>
        static public XmlDocument DecryptXmlDocument(XmlDocument Doc, string password)
        {
            // check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("doc");

            // find the EncrytptionInformation in the XmlDocument
            int iterationCount = 0;
            byte[] salt = null;
            byte[] iv = null;
            int keySize = 0;
            XmlNode encryptionInfoElement = Doc.GetElementsByTagName("EncryptionInformation")[0];
            if (encryptionInfoElement == null)
            {
                throw new XmlException(MessageManager.GetString("ErrorEncryptionInfoNotFound"));
            }

            // check, if nodes are present
            if (Doc.GetElementsByTagName("IterationsUsed").Count == 0)
            {
                throw new XmlException(MessageManager.GetString("ErrorIterationCountNotParsable"));
            }
            if (Doc.GetElementsByTagName("SaltUsed").Count == 0)
            {
                throw new XmlException(MessageManager.GetString("ErrorSaltNotReadable"));
            }
            if (Doc.GetElementsByTagName("IVUsed").Count == 0)
            {
                throw new XmlException(MessageManager.GetString("ErrorIVNotReadable"));
            }
            if (Doc.GetElementsByTagName("KeySize").Count == 0)
            {
                throw new XmlException(MessageManager.GetString("ErrorKeySizeNotReadable"));
            }
            foreach (XmlNode node in encryptionInfoElement)
            #region big switch case block
            {
                switch (node.Name)
                {
                    case "IterationsUsed":
                        {
                            try
                            {
                                iterationCount = int.Parse(node.InnerText);
                            }
                            catch (FormatException ex)
                            {
                                throw new XmlException(MessageManager.GetString("ErrorIterationCountNotParsable") + "\n" + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + "\n\nError: Failed to get iteration Count from Xml file");
                            }
                            break;
                        }
                    case "SaltUsed":
                        {
                            try
                            {
                                salt = Convert.FromBase64String(node.InnerText);
                            }
                            catch (FormatException ex)
                            {
                                throw new XmlException(MessageManager.GetString("ErrorSaltNotReadable") + "\n" + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + "\n\nError: Failed to get salt from Xml file");
                            }
                            break;
                        }
                    case "IVUsed":
                        {
                            try
                            {
                                iv = Convert.FromBase64String(node.InnerText);
                            }
                            catch (FormatException ex)
                            {
                                throw new XmlException(MessageManager.GetString("ErrorIVNotReadable") + "\n" + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + "\n\nError: Failed to get iv from Xml file");
                            }
                            break;
                        }
                    case "KeySize":
                        {
                            try
                            {
                                keySize = int.Parse(node.InnerText);
                            }
                            catch (FormatException ex)
                            {
                                throw new XmlException(MessageManager.GetString("ErrorKeySizeNotReadable") + "\n" + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + "\n\nError: Failed to get keySize from Xml file");
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            #endregion

            // remove EncryptionInformation from document
            XmlNode xmlNodeToRemove = Doc.GetElementsByTagName("EncryptionInformation")[0];
            try
            {
                Doc.LastChild.RemoveChild(xmlNodeToRemove);
            }
            catch (Exception ex)
            {
                throw new XmlException(ex.Message + "EncryptionInformation could not be removed from Xml file.");
            }

            // build AES key for later decryption
            AesManaged aesManagedInstance = GenerateAesKey(password, iterationCount, keySize, salt);
            aesManagedInstance.IV = iv;

            // find the EncryptedData element in the XmlDocument
            XmlElement encryptedElement = Doc.GetElementsByTagName("EncryptedData")[0] as XmlElement;

            // if the EncryptedData element was not found, throw an exception
            if (encryptedElement == null)
            {
                throw new XmlException(MessageManager.GetString("ErrorXmlElementNotFound"));
            }

            // create an EncryptedData object and populate it
            EncryptedData encryptedDataElement = new EncryptedData();
            encryptedDataElement.LoadXml(encryptedElement);

            // create a new EncryptedXml object
            EncryptedXml encryptedXmlObject = new EncryptedXml();

            // decrypt the element using the AES key
            byte[] decryptedOutput;
            try
            {
                decryptedOutput = encryptedXmlObject.DecryptData(encryptedDataElement, aesManagedInstance);
            }
            catch (CryptographicException)
            {
                throw new CryptographicException(MessageManager.GetString("ErrorWrongPassword"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\nDecryption went wrong for some reason");
            }

            // Replace the encryptedData element with the plaintext XML element.
            encryptedXmlObject.ReplaceData(encryptedElement, decryptedOutput);

            return Doc;
        }

        /// <summary>
        /// benchmark methode to get the time needed for password generation
        /// </summary>
        /// <param name="iterationCount">number of iterations</param>
        /// <returns>time needed in milliseconds</returns>
        public static long GetPasswordIterationTime(int iterationCount)
        {
            Timers.PasswordIterationBenchmark.Restart();
            var testkey = new Rfc2898DeriveBytes("anyPassword", Generate128BitsOfRandomEntropy(), iterationCount).GetBytes(256);
            Timers.PasswordIterationBenchmark.Stop();
            return Timers.PasswordIterationBenchmark.ElapsedMilliseconds;
        }

        /// <summary>
        /// create AES key from password (with salt given) 
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="iterationCount">number of iterations</param>
        /// <param name="keySize">AES key size (128, 192 or 256)</param>
        /// <param name="salt">salt as byte array</param>
        /// <returns></returns>
        public static AesManaged GenerateAesKey(string password, int iterationCount, int keySize, byte[] salt)
        {
            // check Arguments
            if (password == null || password == "")
                throw new ArgumentNullException("password can not be empty");
            if (iterationCount < 1)
                throw new ArgumentException("iterationCount has to be positive");
            if (salt == null)
                throw new ArgumentNullException("salt can not be NULL");

            AesManaged aesManagedInstance = new AesManaged();

            Rfc2898DeriveBytes rfcInstance = new Rfc2898DeriveBytes(password, salt, iterationCount);

            // create Key
            switch (keySize)
            {
                case 128:
                    aesManagedInstance.Key = rfcInstance.GetBytes(16);
                    break;
                case 192:
                    aesManagedInstance.Key = rfcInstance.GetBytes(24);
                    break;
                case 256:
                    aesManagedInstance.Key = rfcInstance.GetBytes(32);
                    break;
                default:
                    throw new CryptographicException(MessageManager.GetString("ErrorInvalidKeyLength"));
            }

            // generate initialisation vector
            aesManagedInstance.GenerateIV();

            return aesManagedInstance;
        }

        /// <summary>
        /// create AES key from password (with random salt) 
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="iterationCount">number of iterations</param>
        /// <param name="keySize">AES key size (128, 192 or 256)</param>
        /// <returns></returns>
        public static AesManaged GenerateAesKey(string password, int iterationCount, int keySize)
        {
            // check arguments
            if (password == null || password == "")
                throw new ArgumentNullException("password can not be empyt");
            if (iterationCount < 1)
                throw new ArgumentException("iterationCount must be positive");

            // generate salt
            byte[] salt = Generate128BitsOfRandomEntropy();

            return GenerateAesKey(password, iterationCount, keySize, salt);
        }

        /// <summary>
        /// generates cryptographically secure random 32 bytes
        /// </summary>
        /// <returns>array of 32 random bytes</returns>
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /// <summary>
        /// generates cryptographically secure random 16 bytes
        /// </summary>
        /// <returns>array of 16 random bytes</returns>
        private static byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 16 Bytes will give us 128 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
