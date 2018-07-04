using System;
using System.IO;
using CryptDiary.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Collections.Generic;

namespace CryptDiaryUnitTest
{
    [TestClass]
    public class XmlManagerTests
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

        #region DateToXmlFileName

        [TestMethod]
        public void DateToXmlFileName()
        {
            string fileName = XmlManager.DateToXmlFileName(new DateTime(2016, 6, 1));
            // pathFileName sollte 2016-06-01.xml bestehen

            if (fileName != "2016-06-01.xml")
            {
                Assert.Fail("Es wird ein falscher Dateiname zurückgegeben: " + fileName);
            }
            // weiterer Test mit zweistelligen Monats- und Tageszahlen
            fileName = XmlManager.DateToXmlFileName(new DateTime(2016, 12, 12));
            if (fileName != "2016-12-12.xml")
            {
                Assert.Fail("Es wird ein falscher Dateiname zurückgegeben: " + fileName);
            }
        }

        #endregion

        #region DiaryEntryToEncryptdXmlFile

        [TestMethod]
        public void DiaryEntryToEncryptedXmlFileAllValid()
        {
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry();
            diaryEntry.Date = new DateTime(2016, 6, 3);
            diaryEntry.Text = "I am a DiaryEntryText";
            try
            {
                XmlManager.DiaryEntryToEncryptedXmlFile(diaryEntry, "katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong at encryption: " + ex.Message);
            }

            // zur Überprüfung wieder entschlüsseln
            CryptDiary.Data.DiaryEntry decryptedDiaryEntry = new CryptDiary.Data.DiaryEntry();
            try
            {
                decryptedDiaryEntry = XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(2016, 6, 3), "katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong at re-decryption: " + ex.Message);
            }

            if (diaryEntry.Text == decryptedDiaryEntry.Text && diaryEntry.Date == decryptedDiaryEntry.Date)
            {
                return;
            }
            else
            {
                Assert.Fail("Quelleintrag und entschlüsselter Eintrag stimmen nicht überein:\n" +
                    "Quelle: Date: " + diaryEntry.Date + ", Text: " + diaryEntry.Text +
                    "\nentschlüsselt: Date: " + decryptedDiaryEntry.Date + ", Text: " + decryptedDiaryEntry.Text);
            }
        }

        [TestMethod]
        public void DiaryEntryToEncryptedXmlFileWrongArguments()
        {
            bool exceptionThrown = false;
            // diaryEntry = null
            try
            {
                XmlManager.DiaryEntryToEncryptedXmlFile(null, "katze", GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("diaryEntry"))
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }

            // password = null
            try
            {
                XmlManager.DiaryEntryToEncryptedXmlFile(
                    new CryptDiary.Data.DiaryEntry(new DateTime(2016, 6, 4), "I am a DiaryEntryText"), null, GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("password"))
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }

            // workDir = null
            try
            {
                XmlManager.DiaryEntryToEncryptedXmlFile(
                    new CryptDiary.Data.DiaryEntry(new DateTime(2016, 6, 5), "I am a DiaryEntryText"), "katze", null);
            }
            catch (ArgumentNullException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("workDir"))
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }

            // workDir = null
            try
            {
                XmlManager.DiaryEntryToEncryptedXmlFile(
                    new CryptDiary.Data.DiaryEntry(new DateTime(2016, 6, 5), "I am a DiaryEntryText"), "katze", @"X:\Dir\does\not\exist");
            }
            catch (ArgumentException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("workDir"))
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
            }
            if (!exceptionThrown)
            {
                Assert.Fail("Es wurde keine Fehlermeldung geworfen");
            }
        }

        #endregion

        #region EncryptedXmlFileToDiaryEntry

        [TestMethod]
        public void EncryptedXmlFileToDiaryEntryAllValid()
        {
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry();
            try
            {
                diaryEntry = XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(2016, 5, 23), "katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong at decrypting: " + ex.Message);
            }

            if (diaryEntry.Date != new DateTime(2016, 5, 23))
            {
                Assert.Fail("Tagebucheintrag hat nicht das Datum 23.5.2016, sondern: " + diaryEntry.Date.ToShortDateString());
            }
            if (diaryEntry.Text != "der dreiundzwanzigste")
            {
                Assert.Fail("Tagebucheintrag hat nicht den Text \"der dreiundzwanzigste\", sondern: " + diaryEntry.Text);
            }
        }

        [TestMethod]
        public void EncryptedXmlFileToDiaryEntryWrongArguments()
        {
            bool exceptionThrown = false;

            // invalid date
            try
            {
                XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(), "katze", GetDiaryPath());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("date"))
                    Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-Date: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-Date: " + ex.Message);
            }

            // null-password
            try
            {
                XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(2016, 5, 23), null, GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("password"))
                    Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-Password: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-password: " + ex.Message);
            }

            // null-workdir
            try
            {
                XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(2016, 5, 23), "katze", null);
            }
            catch (ArgumentNullException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("workDir"))
                    Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-WorkDir: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-workDir: " + ex.Message);
            }

            // wrong workdir
            try
            {
                XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(2016, 5, 23), "katze", @"X:\Dir\does\not\exist");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                exceptionThrown = true;
                if (!ex.Message.Contains("workDir"))
                    Assert.Fail("Falsche Fehlermeldung geworfen bei NULL-WorkDir: " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Falsche Fehlermeldung geworfen bei wrong WorkDir: " + ex.Message);
            }

            if (!exceptionThrown)
            {
                Assert.Fail("Es wurde keine Fehlermeldung geworfen.");
            }
        }

        [TestMethod]
        public void EncryptedXmlFileToDiaryEntryWrongPassword()
        {
            try
            {
                XmlManager.EncryptedXmlFileToDiaryEntry(new DateTime(2016, 5, 23), "wrongpassword", GetDiaryPath());
            }
            catch (Exception ex)
            {
                if (!(ex.Message.Contains("password") || ex.Message.Contains("Passwort")))
                {
                    Assert.Fail("Es wurde die falsche Fehlermeldung geworfen: " + ex.Message);
                }
            }
        }

        #endregion

        #region DiaryEntryHashtagsToEncryptedXmlFile

        [TestMethod]
        public void DiaryEntryHashtagsToEncryptedXmlFileAllValid()
        {
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry(DateTime.Today, "#hashtag1 #hashtag2 #hashtag3");
            try
            {
                XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(diaryEntry, "katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // check result
            Dictionary<CryptDiary.Data.Hashtag, List<DateTime>> hashtagDictionary = new Dictionary<CryptDiary.Data.Hashtag, List<DateTime>>();
            try
            {
                hashtagDictionary = XmlManager.EncryptedXmlFileToHashtagDictionary("katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // dictionary should contain 3 entries
            if (hashtagDictionary.Count != 3)
            {
                Assert.Fail("Falsche Anzahl an hashtags in Dictionary. Erwartet: 3, tatsächlich: " + hashtagDictionary.Count.ToString());
            }

            // dictionary entries should be hashtag1, hashtag2, hashtag3
            if (!hashtagDictionary.ContainsKey(new CryptDiary.Data.Hashtag("hashtag1")))
            {
                Assert.Fail("hashtag1 nicht gefunden.");
            }
            if (!hashtagDictionary.ContainsKey(new CryptDiary.Data.Hashtag("hashtag2")))
            {
                Assert.Fail("hashtag2 nicht gefunden.");
            }
            if (!hashtagDictionary.ContainsKey(new CryptDiary.Data.Hashtag("hashtag3")))
            {
                Assert.Fail("hashtag3 nicht gefunden.");
            }
        }

        [TestMethod]
        public void DiaryEntryHashtagsToEncryptedXmlWrongArguments()
        {
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry(DateTime.Today, "blablabla #hashtag");
            bool exceptionThrown = false;
            try
            {
                XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(null, "katze", GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("diaryEntry"))
                {
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            try
            {
                XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(diaryEntry, "", GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("password"))
                {
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            try
            {
                XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(diaryEntry, "katze", null);
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("workDir"))
                {
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            try
            {
                XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(diaryEntry, "katze", @"X:\path\not\existent");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                if (!ex.Message.Contains("workDir"))
                {
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            try
            {
                XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(diaryEntry, "wrongpassword", GetDiaryPath());
            }
            catch (Exception ex)
            {
                if (!(ex.Message.Contains("password") || ex.Message.Contains("Passwort")))
                {
                    Assert.Fail("Falsche Fehlermeldung geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            if (!exceptionThrown)
            {
                Assert.Fail("Es wurde keine Fehlermeldung geworfen.");
            }
        }

        #endregion

        #region HashtagsDictionaryToEncryptedXmlFile

        [TestMethod]
        public void HashtagDictionaryToEncryptedXmlFileAllValid()
        {
            CryptDiary.Data.HashtagDictionary hashtagDictionary = new CryptDiary.Data.HashtagDictionary();
            CryptDiary.Data.Hashtag hashtag1 = new CryptDiary.Data.Hashtag("hashtag1");
            CryptDiary.Data.Hashtag hashtag2 = new CryptDiary.Data.Hashtag("hashtag2");

            hashtagDictionary.Add(hashtag1, new List<DateTime>() { DateTime.Today, DateTime.Today.AddDays(1) });
            hashtagDictionary.Add(hashtag2, new List<DateTime>() { DateTime.Today.AddDays(-1) });

            try
            {
                XmlManager.HashtagDictionaryToEncryptedXmlFile(hashtagDictionary, "katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // compare
            CryptDiary.Data.HashtagDictionary compareDictionary = new CryptDiary.Data.HashtagDictionary();
            try
            {
                compareDictionary = XmlManager.EncryptedXmlFileToHashtagDictionary("katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong at decrypting hashtags.xml file: " + ex.Message);
            }

            // compareDictionary should contain 2 hashtags
            if (compareDictionary.Count != 2)
            {
                Assert.Fail("Falsche Anzahl Einträge. Erwartet: 2, tatsächlich: " + compareDictionary.Count.ToString());
            }

            // hashtags vergleichen
            if (!(compareDictionary.ContainsKey(new CryptDiary.Data.Hashtag("hashtag1")) || compareDictionary.ContainsKey(new CryptDiary.Data.Hashtag("hashtag2"))))
            {
                Assert.Fail("nicht hashtag1 und hashtag2 steht in Dictionary");
            }

            // Daten vergleichen
            List<DateTime> Dates1 = compareDictionary[new CryptDiary.Data.Hashtag("hashtag1")];
            List<DateTime> Dates2 = compareDictionary[new CryptDiary.Data.Hashtag("hashtag2")];
            if (!(Dates1.Contains(DateTime.Today) && Dates1.Contains(DateTime.Today.AddDays(1))))
            {
                Assert.Fail("Hashtag1-Daten sind nicht korrekt. Erwartet: Heute und morgen, tatsächlich: " +
                    Dates1[0].ToShortDateString() + ", " + Dates1[1].ToShortDateString());
            }
            if (!(Dates2.Contains(DateTime.Today.AddDays(-1))))
            {
                Assert.Fail("Hashtag2-Daten sind nicht korrekt. Erwartet: gestern, tatsächlich: " +
                    Dates2[0].ToShortDateString());
            }
        }

        [TestMethod]
        public void HashtagDictionaryToEncryptedXmlFileWrongArguments()
        {
            bool exceptionThrown = false;
            try
            {
                XmlManager.HashtagDictionaryToEncryptedXmlFile(null, "katze", GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("hashtagDictionary"))
                {
                    Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            catch (Exception ex)
            {
                Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
            }

            try
            {
                XmlManager.HashtagDictionaryToEncryptedXmlFile(new CryptDiary.Data.HashtagDictionary(), "", GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("password"))
                {
                    Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            catch (Exception ex)
            {
                Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
            }

            try
            {
                XmlManager.HashtagDictionaryToEncryptedXmlFile(new CryptDiary.Data.HashtagDictionary(), "katze", null);
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("workDir"))
                {
                    Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            catch (Exception ex)
            {
                Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
            }

            try
            {
                XmlManager.HashtagDictionaryToEncryptedXmlFile(new CryptDiary.Data.HashtagDictionary(), "katze", @"X:\does\not\exist\");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                if (!ex.Message.Contains("workDir"))
                {
                    Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }
            catch (Exception ex)
            {
                Assert.Fail("Es wurde die falsche Exception geworfen: " + ex.Message);
            }
            if (!exceptionThrown)
            {
                Assert.Fail("Es wurde keine Fehlermeldung geworfen");
            }
        }

        #endregion

        #region EncryptedXmlFileToHashtagDictionary

        [TestMethod]
        public void EncryptedXmlFileToHashtagDictionaryAllValid()
        {
            Dictionary<CryptDiary.Data.Hashtag, List<DateTime>> hashtagDictionary = new Dictionary<CryptDiary.Data.Hashtag, List<DateTime>>();
            try
            {
                hashtagDictionary = XmlManager.EncryptedXmlFileToHashtagDictionary("katze", GetDiaryPath());
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
        }

        public void EncryptedXmlFileToHashtagDictionaryWrongArguments()
        {
            bool exceptionThrown = false;
            try
            {
                XmlManager.EncryptedXmlFileToHashtagDictionary("wrongpassword", GetDiaryPath());
            }
            catch (Exception ex)
            {
                if (!(ex.Message.Contains("password") || ex.Message.Contains("Passwort")))
                {
                    Assert.Fail("Falsche Fehlermeldung wurde geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }

            try
            {
                XmlManager.EncryptedXmlFileToHashtagDictionary(null, GetDiaryPath());
            }
            catch (ArgumentNullException ex)
            {
                if (!ex.Message.Contains("password"))
                {
                    Assert.Fail("Falsche Fehlermeldung wurde geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }

            try
            {
                XmlManager.EncryptedXmlFileToHashtagDictionary("katze", null);
            }
            catch (Exception ex)
            {
                if (!(ex.Message.Contains("password") || ex.Message.Contains("Passwort")))
                {
                    Assert.Fail("Falsche Fehlermeldung wurde geworfen: " + ex.Message);
                }
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                Assert.Fail("Es wurde keine Fehlermeldung geworfen.");
            }
        }

        #endregion
    }
}
