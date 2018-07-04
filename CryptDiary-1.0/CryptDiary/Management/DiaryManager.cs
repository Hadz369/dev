using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptDiary.Gui;
using CryptDiary.Data;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.IO.Compression;


namespace CryptDiary.Management
{
    public class DiaryManager
    {
        // localisation of messages
        ResourceManager messageManager = new ResourceManager("CryptDiary.Resources.Messages", typeof(DiaryManager).Assembly);

        private string password;
        // automatically check if password is valid at set
        public string Password
        {
            get
            {
                return password;
            }
            // check, if password is valid
            set
            {
                if (value == "" || value == null)
                {
                    password = null;
                }
                else
                {
                    if (IsPasswordValid(value))
                    {
                        password = value;
                    }
                    else
                    {
                        password = null;
                    }
                }
            }
        }
        private string workDirectory;
        // scan directory and set AvailableDates at set
        public string WorkDirectory
        {
            get
            {
                return workDirectory;
            }
            set
            {
                // validate workDirectory
                if (Directory.Exists(value))
                {
                    workDirectory = value;

                    // write workDirectory to settings file
                    DiarySettings Settings = new DiarySettings();
                    Settings.WorkDirectory = value;
                    Settings.Save();

                    // scan workDirectory
                    ScanWorkDirectory();
                }
            }
        }
        private List<DateTime> availableDates;
        // adapt MonthCalendar at mainForm and sort List at set
        public List<DateTime> AvailableDates
        {
            get
            {
                return availableDates;
            }
            set
            {
                value.Sort();
                availableDates = value;
                if (mainForm != null)
                {
                    mainForm.MonthCalendar.BoldedDates = value.ToArray();
                    mainForm.MonthCalendar.Invalidate();
                }
            }
        }
        private MainForm mainForm;
        // for performance reasons, diaryEntries already loaded will be kept in Dictionary, so they have not to be decrypted again
        private Dictionary<DateTime, DiaryEntry> diaryEntries { get; set; }
        private DiaryEntry currentDiaryEntry;
        public DiaryEntry CurrentDiaryEntry
        {
            get
            {
                return currentDiaryEntry;
            }
            set
            {
                // check, if date and text are set
                if (value.Text == null)
                {
                    throw new ArgumentNullException("Error at setting CurrentDiaryEntry: Text can not be NULL.");
                }
                if (value.Date == new DateTime())
                {
                    throw new NullReferenceException("Error at setting CurrentDiaryEntry: Date was not set.");
                }

                currentDiaryEntry = value;
            }
        }
        public HashtagDictionary HashtagDictionary { get; set; }
        //public Dictionary<Hashtag, List<DateTime>> HashtagDictionary { get; set; }

        /// <summary>
        /// constructor: diaryManager gets to know mainForm to be able to write to it
        /// </summary>
        /// <param name="mainForm"></param>
        public DiaryManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
            DiarySettings Settings = new DiarySettings();
            diaryEntries = new Dictionary<DateTime, DiaryEntry>();
            InitAutoSaveTimer();
        }

        /// <summary>
        /// saves the current diaryEntry
        /// </summary>
        public void SaveCurrentDiaryEntry()
        {
            if (this.CurrentDiaryEntry != null)
            {
                try
                {
                    SaveDiaryEntry(CurrentDiaryEntry.Date, CurrentDiaryEntry.Text);
                    XmlManager.DiaryEntryHashtagsToEncryptedXmlFile(CurrentDiaryEntry, this.Password, this.WorkDirectory);
                    GetAllHashtags();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// deletes the diaryEntry and its corresponding xml file
        /// </summary>
        /// <param name="diaryEntry">diaryEntry to delete</param>
        public void DeleteDiaryEntry(DiaryEntry diaryEntry)
        {
            DeleteDiaryEntry(diaryEntry.Date);
        }

        /// <summary>
        /// deletes the diaryEntry at specified date and its xml file, if entry exists
        /// </summary>
        /// <param name="date">date to delete corresponding diaryEntry</param>
        public void DeleteDiaryEntry(DateTime date)
        {
            // get file name of file to delete
            string fileToDelete = XmlManager.DateToXmlFileName(date);

            // if file exists
            if (File.Exists(fileToDelete))
            {
                // try to delete it
                try
                {
                    File.Delete(fileToDelete);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "\n\nTagebucheintragsdatei konnte nicht gelöscht werden");
                }
            }
            // remove corresponding date from AvailableDates and refresh bolded dates at calendar
            this.AvailableDates.Remove(date);
            SetBoldedDatesOnMainForm();
            // Eintrag aus Dictionary entfernen
            diaryEntries.Remove(date);
        }

        /// <summary>
        /// loads a diaryEntry from encrypted xml file
        /// </summary>
        /// <param name="date">date to look for</param>
        /// <returns>diaryEntry corresponding to specified date or new diaryEntry, if no such diaryEntry exists</returns>
        public void LoadDiaryEntry(DateTime date)
        {
            // if no time was set yet, use today's date
            if (date == new DateTime())
            {
                date = DateTime.Today;
            }

            DiaryEntry diaryEntry = new DiaryEntry(date, "");

            // lookup dictionary, if diaryEntry was loaded before
            if (diaryEntries.ContainsKey(date))
            {
                this.CurrentDiaryEntry = diaryEntries[date];

                // restart AutoSaveTimer
                Timers.RestartAutoSaveTimer();

                return; // this.CurrentDiaryEntry;
            }

            // dictionary doesn't contain diaryEntry , so load it from xml file
            if (AvailableDates.Contains(date))
            {
                try
                {
                    diaryEntry = XmlManager.EncryptedXmlFileToDiaryEntry(date, this.Password, this.WorkDirectory);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "\n\n" + messageManager.GetString("ErrorDiaryEntryFileNotLoadable"));
                }
            }
            // add diaryEntry to dictionary respectively edit it
            if (diaryEntries.ContainsKey(date))
            {
                diaryEntries[date] = diaryEntry;
            }
            else
            {
                diaryEntries.Add(date, diaryEntry);
            }
            this.CurrentDiaryEntry = diaryEntry;

            // get all hashtags
            GetAllHashtags();

            // restart the AutoSaveTimer
            Timers.RestartAutoSaveTimer();
        }

        /// <summary>
        /// goes through all diaryEntries and encrypts the corresponding xml files with the new password
        /// </summary>
        /// <param name="oldPassword">old password</param>
        /// <param name="newPassword">new password</param>
        public void ChangePassword(string oldPassword, string newPassword)
        {
            ScanWorkDirectory();

            // reencrypt all DiaryEntries
            foreach (DateTime date in AvailableDates)
            {
                try
                {
                    DiaryEntry diaryEntry = XmlManager.EncryptedXmlFileToDiaryEntry(date, oldPassword, this.WorkDirectory);
                    XmlManager.DiaryEntryToEncryptedXmlFile(diaryEntry, newPassword, this.WorkDirectory);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "\n\npassword change could not be executed for date: " + date.ToShortDateString());
                }
            }

            // reencrypt hashtags file
            try
            {
                HashtagDictionary = XmlManager.EncryptedXmlFileToHashtagDictionary(oldPassword, this.workDirectory);
                XmlManager.HashtagDictionaryToEncryptedXmlFile(this.HashtagDictionary, newPassword, this.workDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\npassword change could not be executed for hashtags.xml");
            }
            // TODO: statistics file
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\npassword change could not be executed for statistics.xml");
            }
            this.Password = newPassword;
        }

        /// <summary>
        /// gets next available date with a diaryEntry relative to the specified base date
        /// </summary>
        /// <param name="baseDate">baseDate from where the method should search</param>
        /// <param name="searchUpwards">determines search direction</param>
        /// <returns>the next date with a diaryEntry in specified search direction</returns>
        public DateTime GetNextDate(DateTime baseDate, bool searchUpwards)
        {
            // sort AvailableDates list
            SortAvailableDates();
            // if list is empty, return today
            if (this.AvailableDates == null || this.AvailableDates.Count == 0)
            {
                return DateTime.Today;
            }

            // if list contains baseDate, return next date in given search direction
            if (AvailableDates.Contains(baseDate))
            {
                if (searchUpwards)
                    if (AvailableDates.IndexOf(baseDate) >= AvailableDates.Count - 1)
                        return AvailableDates[AvailableDates.Count - 1];
                    else
                        return AvailableDates[AvailableDates.IndexOf(baseDate) + 1];
                else
                    if (AvailableDates.IndexOf(baseDate) <= 0)
                    return AvailableDates[0];
                else
                    return AvailableDates[AvailableDates.IndexOf(baseDate) - 1];
            }

            // if list has just one element, return that
            if (AvailableDates.Count == 1)
            {
                return AvailableDates[0];
            }

            // if current date is smaller than smallest date in list, return first list entry
            if (baseDate < AvailableDates[0])
            {
                return AvailableDates[0];
            }

            // if current date is bigger than biggest dat in list, return last list entry
            if (baseDate > AvailableDates[AvailableDates.Count - 1])
            {
                return AvailableDates[AvailableDates.Count - 1];
            }

            // else find nearest neighbor
            else
            {
                // go through list from last to fist until difference AvailableDates[i] - baseDate is negative
                if (searchUpwards)
                {
                    for (int i = AvailableDates.Count - 1; i > 0; i--)
                    {
                        TimeSpan Difference = AvailableDates[i] - baseDate;
                        if (Difference.Days < 0)
                        {
                            return AvailableDates[i];
                        }
                    }
                }
                else
                // go through list from first to last until difference AvailableDates[i] - baseDate is positive
                {
                    for (int i = 0; i < AvailableDates.Count; i++)
                    {
                        TimeSpan Difference = AvailableDates[i] - baseDate;
                        if (Difference.Days > 0)
                        {
                            return AvailableDates[i];
                        }
                    }
                }
            }
            // Code should never reach this point
            throw new IndexOutOfRangeException(messageManager.GetString("ErrorNeighborDateNotFound"));
        }

        /// <summary>
        /// checks, if password is valid by trying to decrypt the latest diaryEntry
        /// </summary>
        /// <param name="password">password to validate</param>
        /// <returns>true, if diaryEntry was decrypted successfully</returns>
        public bool IsPasswordValid(string password)
        {
            // take latest diaryEntry and try to decrypt it
            if (AvailableDates.Count > 0)
            {
                try
                {
                    XmlManager.EncryptedXmlFileToDiaryEntry(this.AvailableDates[AvailableDates.Count - 1], password, this.WorkDirectory);
                    return true;
                }
                // if exception occurs, password is not valid
                catch (Exception)
                {
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true; // no diaryEntries -> every password is valid
            }
        }

        /// <summary>
        /// deletes all current hashtag infos in this instance and regets it from DiaryEntry files
        /// </summary>
        public void RescanHashtags()
        {
            // if hashtag.xml exists, delete it
            string fileName = this.WorkDirectory + "\\" + Properties.Resources.HashtagsFileName;
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    throw new IOException("hashtags.xml could not be deleted. " + ex.Message);
                }
            }

            // clear HashtagDictionary
            HashtagDictionary = null;

            // scan directory and reget all hashtags from Diary Entries
            ScanWorkDirectory();
            GetAllHashtags();
        }

        /// <summary>
        /// gives a preview text of (default) 1024 chars length of the corresponding DiaryEntry
        /// </summary>
        /// <param name="date">date to look for</param>
        /// <returns></returns>
        public string GetDiaryEntryPreview(DateTime date)
        {
            if (AvailableDates.Contains(date))
            {
                // lookup dictionary, if DiaryEntry is already loaded
                if (diaryEntries.ContainsKey(date))
                {
                    return diaryEntries[date].Text.Substring(0, new DiarySettings().PreviewTextLength);
                }

                // load DiaryEntry from file and return the preview text
                else
                {
                    DiaryEntry diaryEntry = new DiaryEntry();
                    try
                    {
                        diaryEntry = XmlManager.EncryptedXmlFileToDiaryEntry(date, this.Password, this.WorkDirectory);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("preview text could not be determinded from xml file. " + ex.Message);
                    }
                    return diaryEntry.Text.Substring(0, new DiarySettings().PreviewTextLength);
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// scans workDirectory for xml files with file name pattern yyyy-MM-dd.xml and writes found dates into AvailableDates list
        /// </summary>
        /// <returns>List of dates with diaryEntries</returns>
        private void ScanWorkDirectory()
        {
            // scan workDirectory for xml files with ????-??-??.xml pattern
            string[] fileNames;
            try
            {
                fileNames = Directory.GetFiles(WorkDirectory, "????-??-??.xml");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\nFiles in WorkDir could not be read");
            }

            // extract date from filename
            for (int i = 0; i < fileNames.Length; i++)
            {
                try
                {
                    // split path at '\' to get the file name
                    string[] pathParts = fileNames[i].Split('\\');
                    // file name = last part of path
                    fileNames[i] = pathParts[pathParts.Length - 1];
                    // cut extension
                    pathParts = fileNames[i].Split(new string[] { ".xml" }, StringSplitOptions.None);
                    fileNames[i] = pathParts[0];
                }
                catch (Exception)
                {
                    throw new FormatException(messageManager.GetString("ErrorInvalidFileNameFormat") + fileNames[i]);
                }
            }

            // parse string to DateTime
            List<DateTime> dates = new List<DateTime>();
            foreach (string date in fileNames)
            {
                try
                {
                    dates.Add(DateTime.Parse(date));
                }
                catch (Exception)
                {
                    throw new FormatException(messageManager.GetString("ErrorInvalidDateString") + date);
                }
            }
            this.AvailableDates = dates;
        }

        /// <summary>
        /// saves a new diary Entry to encrypted xml file
        /// </summary>
        /// <param name="date">date of the diaryEntry</param>
        /// <param name="diaryEntryText">text of the diaryEntry</param>
        private void SaveDiaryEntry(DateTime date, string diaryEntryText)
        {
            if (date != new DateTime())
            {
                // text is empty -> delete diaryEntry, text is not empty -> save it
                if (diaryEntryText == "" || diaryEntryText == null)
                {
                    try
                    {
                        DeleteDiaryEntry(date);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "\n\ndiaryEntry could not be deleted");
                    }
                }
                else
                // text is not empty
                {
                    // save logic begins
                    DiaryEntry diaryEntry = null;

                    // check, if chosen date is in AvailableDates
                    foreach (DateTime AvailableDate in this.AvailableDates)
                    {
                        if (AvailableDate == date)
                        {
                            // date exists in AvailableDates: load diaryEntry and replace text
                            try
                            {
                                diaryEntry = XmlManager.EncryptedXmlFileToDiaryEntry(date, this.Password, this.WorkDirectory);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + "\n\ndiaryEntry could not be decrypted");
                            }
                            diaryEntry.Text = diaryEntryText;
                            break;
                        }
                    }

                    // date doesn't exist in AvailableDates: create new diaryEntry with date and text
                    if (diaryEntry == null)
                    {
                        diaryEntry = new DiaryEntry();
                        diaryEntry.Text = diaryEntryText;
                        diaryEntry.Date = date;
                    }
                    // add diaryEntry to dictionary (performance purposes)
                    if (diaryEntries.ContainsKey(date))
                    {
                        diaryEntries[date] = diaryEntry;
                    }
                    else
                    {
                        diaryEntries.Add(date, diaryEntry);
                    }

                    // save diaryEntry in xml file
                    try
                    {
                        XmlManager.DiaryEntryToEncryptedXmlFile(diaryEntry, this.Password, this.WorkDirectory);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + "\n\ndiaryEntry could not be encrypted");
                    }

                    // if everythin was succussfull, date will be added to AvailableDates
                    AddDateToAvailableDates(date);
                }
            }
        }

        /// <summary>
        /// sorts the list of available dates (oldest on top)
        /// </summary>
        private void SortAvailableDates()
        {
            AvailableDates.Sort();
        }

        /// <summary>
        /// checks, if specified date is already in list of available dates and adds it, if not
        /// </summary>
        /// <param name="date">date to add to AvailableDates</param>
        private void AddDateToAvailableDates(DateTime date)
        {
            bool dateAlreadyExists = false;
            foreach (DateTime availableDate in this.AvailableDates)
            {
                if (date == availableDate)
                {
                    dateAlreadyExists = true;
                    break;
                }
            }
            if (!dateAlreadyExists)
            {
                this.AvailableDates.Add(date);
                SetBoldedDatesOnMainForm();
            }
        }

        /// <summary>
        /// marks AvailableDates as bold on MonthCalendar in mainForm
        /// </summary>
        private void SetBoldedDatesOnMainForm()
        {
            mainForm.MonthCalendar.BoldedDates = this.AvailableDates.ToArray();
        }

        /// <summary>
        /// initiates the AutoSaveTimer with interval value from settings file
        /// </summary>
        private void InitAutoSaveTimer()
        {
            Timers.AutoSaveTimer.Interval = new DiarySettings().AutoSaveInterval * 1000;
            Timers.AutoSaveTimer.Tick += AutoSaveTimer_Tick;
            Timers.AutoSaveTimer.Start();
        }

        private void UpdateHashtagsListBox()
        {
            if (HashtagDictionary != null)
            {
                mainForm.HashtagsListBox.Items.Clear();
                foreach (var hashtag in HashtagDictionary)
                {
                    mainForm.HashtagsListBox.Items.Add(hashtag.Key.Text);
                }
            }
        }

        private void GetAllHashtags()
        {
            // check, if hashtags file exists and is valid
            if (File.Exists(this.WorkDirectory + "\\" + Properties.Resources.HashtagsFileName))
            {
                try
                {
                    this.HashtagDictionary = XmlManager.EncryptedXmlFileToHashtagDictionary(this.Password, this.WorkDirectory);

                }
                catch (Exception ex)
                {
                    throw new Exception("hashtags.xml could not be opened or decrypted: " + ex.Message);
                }
                UpdateHashtagsListBox();
                return;
            }

            // if no hashtags file exists
            HashtagDictionary = new HashtagDictionary();
            foreach (DateTime date in AvailableDates)
            {
                LoadDiaryEntry(date);
                HashtagDictionary = Hashtag.EditHashtagsDictionary(HashtagDictionary, CurrentDiaryEntry);
            }
            // save HashtagDictionary to file
            try
            {
                XmlManager.HashtagDictionaryToEncryptedXmlFile(this.HashtagDictionary, this.Password, this.WorkDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("saving to encrypted hashtags.xml went wrong: " + ex.Message);
            }

            UpdateHashtagsListBox();
        }

        #region EventHandler
        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (new DiarySettings().AutoSave)
                SaveCurrentDiaryEntry();
        }
        #endregion

        // TODO: delete Spielwiese
        #region Spielwiese
        // das muss noch irgendwie schöner gehen, kann ja nicht angehen, dass ich aus der 
        // MainForm diese Methode aufrufen muss, um die AutoSaveTimer delegates loszuwerden
        public void UnInitAutoSaveTimer()
        {
            Timers.AutoSaveTimer.Stop();

            Timers.AutoSaveTimer.Tick -= AutoSaveTimer_Tick;
            new Form().Close();
        }

        /// <summary>
        /// exports the work directory into a zip file
        /// </summary>
        /// <param name="fileName">filename including path to store the backed up diary</param>
        public void ExportDiaryToZip(string fileName)
        {
            // check arguments
            if (fileName == "" || fileName == null)
                throw new ArgumentNullException("fileName can not be empty or null");
            if (!fileName.Contains(".zip"))
                throw new ArgumentOutOfRangeException("fileName must be a .zip file");

            // create a list of all .xml-files
            List<string> fileNames = new List<string>();
            string[] fileNamesTemp = Directory.GetFiles(this.WorkDirectory);
            foreach (string file in fileNamesTemp)
            {
                if (file.Contains(".xml"))
                {
                    fileNames.Add(file);
                }
            }

            if (fileNames.Count > 0)
            {
                // put these xml-files into a new zip archive
                try
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        using (ZipArchive exportArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                        {
                            foreach (var file in fileNames)
                            {
                                exportArchive.CreateEntryFromFile(file, file.Split('\\')[file.Split('\\').Length - 1]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }




        #endregion
    }
}
