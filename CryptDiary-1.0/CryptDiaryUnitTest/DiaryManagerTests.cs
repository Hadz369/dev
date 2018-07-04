using System;
using System.IO;
using CryptDiary.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CryptDiaryUnitTest
{
    [TestClass]
    public class DiaryManagerTests
    {
        //string pathToDiary = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\AADiary";

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

        private List<DateTime> GetSomeDates()
        {
            List<DateTime> dates = new List<DateTime>();
            dates.Add(DateTime.Today);
            dates.Add(DateTime.Today.AddDays(1));
            dates.Add(DateTime.Today.AddDays(-1));
            dates.Add(DateTime.Today.AddYears(-1));
            dates.Add(DateTime.Today.AddYears(1));
            return dates;
        }

        #region ChangePassword

        [TestMethod]
        public void ChangePasswordValid()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            try
            {
                diaryManager.ChangePassword("katze", "katze");
            }
            catch (Exception ex)
            {
                Assert.Fail("Es ist ein Fehler aufgetreten: " + ex.Message);
            }
        }

        [TestMethod]
        public void ChangePasswordInvalid()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            try
            {
                diaryManager.ChangePassword("wrongpassword", "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message.ToLower(), "password");
                return;
            }
            Assert.Fail("Es wurde keine Exception geworfen.");
        }

        [TestMethod]
        public void ChangePasswordNoOldPassword() // it's the same as giving a wrong password, it just cannot decrypt
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            try
            {
                diaryManager.ChangePassword("", "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message.ToLower(), "password");
                return;
            }
            Assert.Fail("Es wurde keine Exception geworfen.");
        }

        [TestMethod]
        public void ChangePasswordNoNewPassword()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            try
            {
                diaryManager.ChangePassword("katze", "");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message.ToLower(), "password");
                return;
            }
            Assert.Fail("Es wurde keine Exception geworfen.");
        }

        [TestMethod]
        public void ChangePasswordWrongDirectory()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath() + "somethingSenseless";
            try
            {
                diaryManager.ChangePassword("katze", "katze");
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message.ToLower(), "workdir");
                return;
            }
            Assert.Fail("Es wurde keine Exception geworfen.");
        }

        #endregion

        #region DeleteDiaryEntry

        [TestMethod]
        public void DeleteDiaryEntryValidDate()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            // make a copy to repeat the test again and again
            try
            {
                File.Copy(GetDiaryPath() + "\\filetodelete.xml", GetDiaryPath() + "\\2016-05-23.xml");

            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("already exists"))
                {
                    Assert.Fail("Something went wrong: " + ex.Message);
                }
            }
            try
            {
                diaryManager.DeleteDiaryEntry(new DateTime(2016, 5, 23));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        public void DeleteDiaryEntryValidDiaryEntry()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            // make a copy to repeat the test again and again
            try
            {
                File.Copy(GetDiaryPath() + "\\filetodelete.xml", GetDiaryPath() + "\\2016-05-23.xml");

            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("already exists"))
                {
                    Assert.Fail("Something went wrong: " + ex.Message);
                }
            }

            diaryManager.Password = "katze";
            diaryManager.LoadDiaryEntry(new DateTime(2016, 5, 23));

            try
            {
                diaryManager.DeleteDiaryEntry(diaryManager.CurrentDiaryEntry);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void DeleteDiaryEntryUnavailableDate() // should just do nothing if file doesn't exist
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            // make a copy to repeat the test again and again
            try
            {
                File.Copy(GetDiaryPath() + "\\filetodelete.xml", GetDiaryPath() + "\\2016-05-23.xml");

            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("already exists"))
                {
                    Assert.Fail("Something went wrong: " + ex.Message);
                }
            }
            try
            {
                diaryManager.DeleteDiaryEntry(new DateTime(2050, 1, 1));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void DeleteDiaryEntryReadOnlyFile() // should just do nothing if file doesn't exist
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();

            try
            {
                diaryManager.DeleteDiaryEntry(new DateTime(2099, 1, 1));
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "konnte nicht gelöscht werden");
                return;
            }
        }

        #endregion

        #region GetNextDate

        [TestMethod]
        public void GetNextDateValid()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.AvailableDates = GetSomeDates();
            DateTime tomorrow = diaryManager.GetNextDate(DateTime.Today, true);
            DateTime yesterday = diaryManager.GetNextDate(DateTime.Today, false);
            DateTime aYearAgoUp = diaryManager.GetNextDate(DateTime.Today.AddYears(-3), true);
            DateTime aYearAgoDown = diaryManager.GetNextDate(DateTime.Today.AddYears(-3), false);
            DateTime aYearFromNowUp = diaryManager.GetNextDate(DateTime.Today.AddYears(3), true);
            DateTime aYearFromNowDown = diaryManager.GetNextDate(DateTime.Today.AddYears(3), false);

            if (tomorrow != DateTime.Today.AddDays(1))
            {
                Assert.Fail("upwards: Es sollte das morgige Datum ausgegeben werden. Stattdessen: " + tomorrow.ToShortDateString());
            }
            if (yesterday != DateTime.Today.AddDays(-1))
            {
                Assert.Fail("downwards: Es sollte das gestrige Datum ausgegeben werden. Stattdessen: " + yesterday.ToShortDateString());
            }
            if (aYearAgoUp != DateTime.Today.AddYears(-1))
            {
                Assert.Fail("upwards: Es sollte \"heute vor einem Jahr\" ausgegeben werden. Stattdessen: " + aYearAgoUp.ToShortDateString());
            }
            if (aYearAgoDown != DateTime.Today.AddYears(-1))
            {
                Assert.Fail("downwards: Es sollte \"heute vor einem Jahr\" ausgegeben werden. Stattdessen: " + aYearAgoDown.ToShortDateString());
            }
            if (aYearFromNowUp != DateTime.Today.AddYears(1))
            {
                Assert.Fail("upwards: Es sollte \"heute in einem Jahr\" ausgegeben werden. Stattdessen: " + aYearFromNowUp.ToShortDateString());
            }
            if (aYearFromNowDown != DateTime.Today.AddYears(1))
            {
                Assert.Fail("downwards: Es sollte \"heute in einem Jahr\" ausgegeben werden. Stattdessen: " + aYearFromNowDown.ToShortDateString());
            }
        }

        [TestMethod]
        public void GetNextDateOneDateAvailable()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            List<DateTime> justYesterday = new List<DateTime>();
            justYesterday.Add(DateTime.Today.AddDays(-1)); // nur "Gestern" vorhanden
            diaryManager.AvailableDates = justYesterday;
            DateTime yesterdayUp = diaryManager.GetNextDate(DateTime.Today, true);
            DateTime yesterdayDown = diaryManager.GetNextDate(DateTime.Today, false);
            if (yesterdayUp != DateTime.Today.AddDays(-1))
            {
                Assert.Fail("Es sollte \"gestern\" zurückgegeben werden. Stattdessen: " + yesterdayUp);
            }
            if (yesterdayDown != DateTime.Today.AddDays(-1))
            {
                Assert.Fail("Es sollte \"gestern\" zurückgegeben werden. Stattdessen: " + yesterdayDown);
            }
        }

        [TestMethod]
        public void GetNextDateNoDatesAvailable()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.AvailableDates = new List<DateTime>();
            DateTime resultUp = diaryManager.GetNextDate(DateTime.Today, true);
            DateTime resultDown = diaryManager.GetNextDate(DateTime.Today, false);
            if (resultUp != DateTime.Today)
            {
                Assert.Fail("Es sollte \"heute\" zurückgegeben werden. Stattdessen: " + resultUp);
            }
            if (resultDown != DateTime.Today)
            {
                Assert.Fail("Es sollte \"heute\" zurückgegeben werden. Stattdessen: " + resultDown);
            }
        }

        #endregion

        #region IsPasswordValid

        [TestMethod]
        public void IsPasswordValidWithValidPassword()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.AvailableDates = new List<DateTime>();
            diaryManager.AvailableDates.Add(new DateTime(2016, 5, 23));
            bool passwordIsValid = false;
            try
            {
                passwordIsValid = diaryManager.IsPasswordValid("katze");
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
            if (!passwordIsValid)
            {
                Assert.Fail("korrektes Passwort wurde nicht als korrekt erkannt");
            }
        }

        [TestMethod]
        public void IsPasswordValidWithInvalidPassword()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.AvailableDates = new List<DateTime>();
            diaryManager.AvailableDates.Add(new DateTime(2016, 5, 23));
            bool passwordIsValid = false;
            try
            {
                passwordIsValid = diaryManager.IsPasswordValid("wrongpassword");
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
            if (passwordIsValid)
            {
                Assert.Fail("falsches Passwort wurde als korrekt erkannt");
            }
        }

        [TestMethod]
        public void IsPasswordValidWithNullPassword()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.AvailableDates = new List<DateTime>();
            diaryManager.AvailableDates.Add(new DateTime(2016, 5, 23));
            bool passwordIsValid = false;
            try
            {
                passwordIsValid = diaryManager.IsPasswordValid(null);
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
            if (passwordIsValid)
            {
                Assert.Fail("\"NULL\"-Passwort wurde als korrekt erkannt");
            }
        }

        #endregion

        #region LoadDiaryEntry

        [TestMethod]
        public void LoadDiaryEntryAllValid()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.Password = "katze";

            try
            {
                diaryManager.LoadDiaryEntry(new DateTime(2016, 6, 1));
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
            if (diaryManager.CurrentDiaryEntry.Date == new DateTime(2016, 6, 1) && diaryManager.CurrentDiaryEntry.Text == "I am a DiaryEntryText")
            {
                return;
            }
            Assert.Fail(
                "DiaryEntry entspricht nicht den Erwartungen: Date = "
                + diaryManager.CurrentDiaryEntry.Date.ToShortDateString() + ", Text = " + diaryManager.CurrentDiaryEntry.Text);
        }

        [TestMethod]
        public void LoadDiaryEntryUnavailableDate()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.Password = "katze";

            try
            {
                diaryManager.LoadDiaryEntry(new DateTime(2000, 6, 1));
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
            // a new DiaryEntry should exist now with given date and no text
            if ((diaryManager.CurrentDiaryEntry.Text == null || diaryManager.CurrentDiaryEntry.Text == "") &&
                diaryManager.CurrentDiaryEntry.Date == new DateTime(2000, 6, 1))
            {
                return;
            }
            else
            {
                Assert.Fail(
                    "DiaryEntry wurde nicht ordentlich angelegt. " +
                    "Date: " + diaryManager.CurrentDiaryEntry.Date.ToShortDateString() +
                    ", Text: " + diaryManager.CurrentDiaryEntry.Text);
            }
        }

        [TestMethod]
        public void LoadDiaryEntryAvailableDateInvalidPassword()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.Password = "wrongpassword";

            try
            {
                diaryManager.LoadDiaryEntry(new DateTime(2016, 5, 23));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("could not be loaded"))
                {
                    StringAssert.Contains(ex.Message, "could not be loaded");
                    return;
                }
                else
                {
                    Assert.Fail("Falsche Fehlermeldung: " + ex.Message);
                }
            }
        }


        #endregion

        #region SaveCurrentDiaryEntry

        [TestMethod]
        public void SaveCurrentDiaryEntryAllValid()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.Password = "katze";
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry();
            diaryEntry.Date = new DateTime(2016, 6, 2);
            diaryEntry.Text = "I am a DiaryEntryText";
            diaryManager.CurrentDiaryEntry = diaryEntry;

            try
            {
                diaryManager.SaveCurrentDiaryEntry();
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }

            // Zur Überprüfung Eintrag wieder laden
            //CryptDiary.Data.DiaryEntry CurrentDiaryEntry = new CryptDiary.Data.DiaryEntry();
            try
            {
                diaryManager.LoadDiaryEntry(new DateTime(2016, 6, 2));
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong at re-loading DiaryEntry: " + ex.Message);
            }
            if (diaryManager.CurrentDiaryEntry.Date == diaryEntry.Date && diaryManager.CurrentDiaryEntry.Text == diaryEntry.Text)
            {
                return;
            }
            else
            {
                Assert.Fail("Gespeicherter und geladener DiaryEntry stimmen nicht überein:\n" +
                    "gespeichert: Date: " + diaryEntry.Date.ToShortDateString() + ", Text: " + diaryEntry.Text +
                    "\ngeladen: Date: " + diaryManager.CurrentDiaryEntry.Date.ToShortDateString() + ", Text: " + diaryManager.CurrentDiaryEntry.Text);
            }
        }

        [TestMethod]
        public void SaveCurrentDiaryEntryNull()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());
            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.Password = "katze";
            CryptDiary.Data.DiaryEntry diaryEntry = new CryptDiary.Data.DiaryEntry();

            try
            {
                diaryManager.SaveCurrentDiaryEntry(); // should just do nothing, if CurrentDiaryEntry == null
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }
        }

        #endregion

        #region RescanHashtags

        [TestMethod]
        public void RescanHashtagsAllValid()
        {
            DiaryManager diaryManager = new DiaryManager(new CryptDiary.Gui.MainForm());

            diaryManager.WorkDirectory = GetDiaryPath();
            diaryManager.Password = "katze";

            try
            {
                diaryManager.RescanHashtags();
            }
            catch (Exception ex)
            {
                Assert.Fail("something went wrong: " + ex.Message);
            }


        }

        #endregion
    }
}
