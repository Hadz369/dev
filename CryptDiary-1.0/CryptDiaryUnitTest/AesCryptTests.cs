using System;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using CryptDiary.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptDiaryUnitTest
{
    [TestClass]
    public class AesCryptTests
    {
        private string GetDiaryPath()
        {
            string pathToDiary = AppDomain.CurrentDomain.BaseDirectory;
            pathToDiary = Directory.GetParent(pathToDiary).ToString();
            pathToDiary = Directory.GetParent(pathToDiary).ToString();
            pathToDiary = Directory.GetParent(pathToDiary).ToString();
            pathToDiary += "\\AADiary (Diary für UnitTests)\\";
            if (Directory.Exists(pathToDiary))
            {
                return pathToDiary;
            }
            else
            {
                throw new FileNotFoundException("Pfad " + pathToDiary + " existiert nicht.");
            }
        }

        private XmlDocument LoadDiaryXml(string fileName)
        {
            XmlDocument DiaryEntryXml = new XmlDocument();
            string pathFileName = GetDiaryPath() + fileName;
            DiaryEntryXml.Load(pathFileName);
            return DiaryEntryXml;
        }

        #region DecryptXmlDocument

        [TestMethod]
        public void DecryptXmlDocumentAllValid()
        {
            AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16.xml"), "katze");
        }

        [TestMethod]
        public void DecryptXmlDocumentInvalidPassword()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16.xml"), "wrongpassword");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "Falsches Passwort?");
                return;
            }
            Assert.Fail("Trotz falschem Passwort erfolgreich durchgelaufen");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWithoutEncryptionInformation()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_without_encryption_info.xml"), "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "EncryptionInformation");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWithoutIterationsNode()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_without_iterations_node.xml"), "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "Iteration");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWithoutSaltNode()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_without_salt_node.xml"), "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "Salt");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWithoutKeySizeNode()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_without_keysize_node.xml"), "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "KeySize");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWithoutIVNode()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_without_IV_node.xml"), "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "IV");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWithoutCipherData()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_without_cipher_data.xml"), "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "Verschlüsselung");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWrongIteration()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_wrong_iterations.xml"), "katze");
            }
            catch (XmlException ex)
            {
                StringAssert.Contains(ex.Message, "Iteration");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptInvalidXmlDocumentWrongKeySize()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"2016-05-16_wrong_keysize.xml"), "katze");
            }
            catch (XmlException ex)
            {
                StringAssert.Contains(ex.Message, "KeySize");
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptCompletelyWrongXmlFile()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(LoadDiaryXml(@"completely_wrong_xml_file.xml"), "katze");
            }
            catch (XmlException)
            {
                return;
            }
            Assert.Fail("Es wurde nicht die richtige Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void DecryptNullFile()
        {
            try
            {
                AesCrypt.DecryptXmlDocument(null, "katze");
            }
            catch (ArgumentNullException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
        }

        #endregion

        #region EncryptXmlDocument

        [TestMethod]
        public void EncryptXmlDocument()
        {
            AesCrypt.EncryptXmlDocument(LoadDiaryXml(@"EncryptMe.xml"), "EncryptMe", "anypassword", 1);
        }

        [TestMethod]
        public void EncryptXmlDocumentNull()
        {
            try
            {
                AesCrypt.EncryptXmlDocument(null, "EncryptMe", "anypassword", 1);
            }
            catch (ArgumentNullException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermedung geworfen: " + ex.Message);
            }
            Assert.Fail("Keine Fehlermeldung geworfen");
        }

        [TestMethod]
        public void EncryptXmlDocumentInvalidNode()
        {
            try
            {
                AesCrypt.EncryptXmlDocument(LoadDiaryXml(@"EncryptMe.xml"), "NotAvailableNode", "anypassword", 1);
            }
            catch (XmlException ex)
            {
                StringAssert.Contains(ex.Message, "nicht gefunden");
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Es wurde die falsche Fehlermeldung geworfen :" + ex.Message);
            }
            Assert.Fail("Es wurde keine Fehlermeldung geworfen");
        }

        [TestMethod]
        public void EncryptXmlDocumentEmptyPassword()
        {
            try
            {
                AesCrypt.EncryptXmlDocument(LoadDiaryXml(@"EncryptMe.xml"), "EncryptMe", "", 1);
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.Message, "password");
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Es wurde die falsche Fehlermeldung geworfen: " + ex.Message);
            }
            Assert.Fail("Es wurde keine Fehlermeldung geworfen.");
        }

        #endregion

        #region GetPasswordIterationTime

        [TestMethod]
        public void GetPasswordIterationTime()
        {
            try
            {
                AesCrypt.GetPasswordIterationTime(1000);
            }
            catch (Exception)
            {
                Assert.Fail("Something went terribly wrong!");
            }
        }

        [TestMethod]
        public void GetPasswordIterationTimeZeroIterations()
        {
            try
            {
                AesCrypt.GetPasswordIterationTime(0);
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "Positive Zahl erforderlich");
                return;
            }
            Assert.Fail("Keine Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void GetPasswordIterationTimeNegativeIterations()
        {
            try
            {
                AesCrypt.GetPasswordIterationTime(-1000);
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "Positive Zahl erforderlich");
                return;
            }
            Assert.Fail("Keine Fehlermeldung geworfen.");
        }

        #endregion

        #region GenerateAesKey

        [TestMethod]
        public void GenerateAesKey128()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("anypassword", 1000, 128);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                return;
            }
            if (AesKey.KeySize != 128)
                Assert.Fail("falsche KeySize");
        }

        [TestMethod]
        public void GenerateAesKey192()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("anypassword", 1000, 192);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                return;
            }
            if (AesKey.KeySize != 192)
                Assert.Fail("falsche KeySize");
        }

        [TestMethod]
        public void GenerateAesKey256()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("anypassword", 1000, 256);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                return;
            }
            if (AesKey.KeySize != 256)
                Assert.Fail("falsche KeySize");
        }

        [TestMethod]
        public void GenerateAesKeyInvalidKeySize()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("anypassword", 1000, 127);
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "128, 192 oder 256");
            }
        }

        [TestMethod]
        public void GenerateAesKeySaltNULL()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("anypassword", 1000, 256, null);
            }
            catch (ArgumentNullException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            Assert.Fail("Keine Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void GenerateAesKeySaltTooShort()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("anypassword", 1000, 256, new byte[] { 1, 2, 3, 4 });
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "mindestens acht Byte");
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            Assert.Fail("Keine Fehlermeldung geworfen.");
        }

        [TestMethod]
        public void GenerateAesKeyEmptyPassword()
        {
            AesManaged AesKey;
            try
            {
                AesKey = AesCrypt.GenerateAesKey("", 1000, 256);
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "password");
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung wurde geworfen: " + ex.Message);
            }
            Assert.Fail("Keine Fehlermeldung geworfen");
        }
        #endregion
    }
}
