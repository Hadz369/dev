using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CryptDiary.Management;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Resources;

namespace CryptDiary.Gui
{
    public partial class MainForm : Form
    {
        // localisation of messages and identifiers
        private ResourceManager messageManager = new ResourceManager("CryptDiary.Resources.Messages", typeof(MainForm).Assembly);
        private ResourceManager identifierManager = new ResourceManager("CryptDiary.Resources.Identifiers", typeof(MainForm).Assembly);

        // declarations
        private DiaryManager diaryManager;
        bool diaryIsLocked = false;

        // standard constructor
        public MainForm()
        {
            // Lokalisation der Gui
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(new DiarySettings().CultureInfo);
            InitializeComponent();
        }

        // initialisation
        private void formMain_Load(object sender, EventArgs e)
        {
            // start with 75% of screen size in center of screen
            float resolutionFactor = 0.75f;
            float locationFactor = (1 - resolutionFactor) / 2;
            Size screenResolution = Screen.PrimaryScreen.WorkingArea.Size;
            this.Size = new Size((int)(screenResolution.Width * resolutionFactor), (int)(screenResolution.Height * resolutionFactor));
            this.Location = new Point((int)(screenResolution.Width * locationFactor), (int)(screenResolution.Height * locationFactor));

            // initiate with locked gui
            LockGui(true);

            // check, if workDirectory saved in Settings exists. If yes, open this diary
            if (Directory.Exists(new DiarySettings().WorkDirectory))
            {
                OpenExistingDiary(new DiarySettings().WorkDirectory);
            }
        }

        /// <summary>
        /// opens a directory browser diaglog and asks the user for an empty folder
        /// afterwars a new diaryManager instance will be created and the user has to give it a password
        /// GUI adaptions will be made and today's date will be preselected
        /// </summary>
        private void OpenNewDiary()
        {
            // determine workDirectory (must be empty)
            string workDirectory = GetWorkDirectory(true);

            if (workDirectory != null)
            {
                // close possibly open Diary
                CloseDiary();

                // init diaryManager with chosen workDirectory
                diaryManager = new DiaryManager(this);
                diaryManager.WorkDirectory = workDirectory; // workDirectory will be scanned for diary entry files automatically

                // ask for password
                string password = PromptForPassword();
                if (password != null)
                {
                    diaryManager.Password = password;

                    // unlock Gui
                    LockGui(false);

                    // show diary name in title bar
                    SetFormText();

                    // change date to today
                    ChangeDate(DateTime.Today);

                    // init timers
                    InitAutoLockTimer();
                }
            }
        }

        /// <summary>
        /// creates a new diaryManager instance with the directory specified in "workDirectory" parameter as its workDirectory
        /// user will be asked for password, which then gets validated
        /// GUI adaptions will be made and today's diaryEntry will be selected
        /// </summary>
        /// <param name="workDirectory">workDirectory, which contains the diary's Xml files</param>
        private void OpenExistingDiary(string workDirectory)
        {
            if (workDirectory != null)
            {
                // close possibly open diary
                CloseDiary();

                // create diaryManager instance and give it the workDirectory
                diaryManager = new DiaryManager(this);
                diaryManager.WorkDirectory = workDirectory; // scannes directory and draw dates with diary entries bolded

                // ask for password and validate it
                string password = PromptForPasswordAndValidate(diaryManager);
                if (password != null) // which means also that password is valid
                {
                    // give password to diaryManager
                    diaryManager.Password = password;

                    // unlock Gui
                    LockGui(false);

                    // show diary name in title bar
                    SetFormText();

                    // show today's diaryEntry
                    ChangeDate(DateTime.Today);

                    // init timers
                    InitAutoLockTimer();
                }
                else
                {
                    // wrong password or password prompt cancelld: reset calendar's bolded dates
                    MonthCalendar.BoldedDates = null;
                }
            }
        }

        /// <summary>
        /// creates a new diaryManager instance, user gets asked for workDirectory
        /// user will be asked for password, which then gets validated
        /// GUI adaptions will be made and today's diaryEntry will be selected
        /// </summary>
        private void OpenExistingDiary()
        {
            // ask user for workDirectory and open diary
            string workDirectory = GetWorkDirectory(false);
            OpenExistingDiary(workDirectory);
        }

        /// <summary>
        /// removes diaryManager instance from memory and sets the GUI to a "fresh" state
        /// </summary>
        private void CloseDiary()
        {
            // save CurrentDiaryEntry
            if (diaryManager != null && !diaryIsLocked)
            {
                diaryManager.SaveCurrentDiaryEntry();
            }
            if (diaryManager != null)
            {
                // TODO: remove AutoSaveTimer delegate within diaryManager's Dispose method
                diaryManager.UnInitAutoSaveTimer();
            }
            // remove diaryManager from Memory
            diaryManager = null;
            // clear RichTextBox
            diaryEntryRichTextBox.Clear();
            // unlock Diary if locked (just for Gui purposes)
            if (diaryIsLocked)
                UnlockDiary();
            // lock gui
            LockGui(true);
            // remove diary name from title bar
            SetFormText();
        }

        /// <summary>
        /// asks user for a workDirectory
        /// </summary>
        /// <param name="emptyFolderNeeded">if true, chosen Directory must be empty</param>
        /// <returns>workDirectory, which contains diary's Xml files</returns>
        private string GetWorkDirectory(bool emptyFolderNeeded)
        {
            workDirectoryBrowserDialog.Description = identifierManager.GetString("ChooseWorkDirectory");
            if (!emptyFolderNeeded)
            {
                // show dialog for choosing the workDirectory
                if (workDirectoryBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return workDirectoryBrowserDialog.SelectedPath;
                }
                else
                    return null;
            }
            else
            {
                if (workDirectoryBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // check if directory is empty
                    if (Directory.GetFiles(workDirectoryBrowserDialog.SelectedPath).Length == 0)
                    {
                        return workDirectoryBrowserDialog.SelectedPath;
                    }
                    else
                    {
                        MessageBox.Show(messageManager.GetString("InfoDirectoryNotEmpty"));
                        return GetWorkDirectory(emptyFolderNeeded);
                    }
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// ask user for password
        /// </summary>
        /// <returns>password</returns>
        private string PromptForPassword()
        {
            PasswordForm passwordForm = new PasswordForm();

            if (passwordForm.ShowDialog() == DialogResult.OK)
            {
                if (passwordForm.passwordTextBox.Text != "")
                {
                    return passwordForm.passwordTextBox.Text;
                }
                else
                {
                    return PromptForPassword();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ask user for password and validate it
        /// </summary>
        /// <param name="diaryManager">diaryManager instance, which can read and write Xml files in workDirectory</param>
        /// <returns>valid password or "null" if cancelled or no diaryManager instance given</returns>
        private string PromptForPasswordAndValidate(DiaryManager diaryManager)
        {
            // execute only if a diaryManager was instantiated
            if (diaryManager != null)
            {
                PasswordForm passwordForm = new PasswordForm();

                // if user clicked OK
                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    // check, if password was entered
                    if (passwordForm.passwordTextBox.Text != "")
                    {
                        // validate password
                        if (diaryManager.IsPasswordValid(passwordForm.passwordTextBox.Text))
                        {
                            // return password if valid
                            return passwordForm.passwordTextBox.Text;
                        }
                        else
                        {
                            // invalid password -> show error message and ask again for password
                            MessageBox.Show(messageManager.GetString("ErrorWrongPassword"));
                            return PromptForPasswordAndValidate(diaryManager);
                        }
                    }
                    else
                    {
                        // password empty -> show error message and ask again for password
                        MessageBox.Show(messageManager.GetString("ErrorEmptyPassword"));
                        return PromptForPassword();
                    }
                }
                else // password prompt was cancelled
                {
                    return null;
                }
            }
            else // no diaryManager instance
            {
                return null;
            }
        }

        /// <summary>
        /// asks user for old and new password and encrypts all diary entries with new password
        /// </summary>
        private void ChangePassword()
        {
            // execute only if diaryManager instance exists
            if (diaryManager != null)
            {
                // show password change form
                ChangePasswordForm changePasswordForm = new ChangePasswordForm();
                if (changePasswordForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // if both new password entries are identical, execute password change
                    if (changePasswordForm.newPasswordTextBox1.Text == changePasswordForm.newPasswordTextBox2.Text)
                    {
                        try
                        {
                            diaryManager.ChangePassword(changePasswordForm.oldPasswordTextBox.Text, changePasswordForm.newPasswordTextBox1.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// if user chooses a date from calendar, the current diaryEntry will be saved, the diaryEntry of
        /// the chosen date will be loaded and the RichTextBox gets the focus
        /// </summary>
        /// <param name="date"></param>
        private void ChangeDate(DateTime date)
        {
            if (diaryManager != null)
            {
                if (diaryManager.CurrentDiaryEntry != null)
                {
                    try
                    {
                        diaryManager.SaveCurrentDiaryEntry();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                try
                {
                    diaryManager.LoadDiaryEntry(date);
                    diaryEntryRichTextBox.Text = diaryManager.CurrentDiaryEntry.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                diaryEntryRichTextBox.Focus();
            }
        }

        /// <summary>
        /// lock diary: save diaryEntry, disable all Gui elements except unlock-button and "close diary" menu entry.
        /// reset password, make lock button to unlock button
        /// stop AutoLockTimer and AutoSaveTimer
        /// </summary>
        private void LockDiary()
        {
            if (!diaryIsLocked)
            {
                // that action brings the gui in the diarylocked state
                Action LockUiElements = delegate
                {
                    diaryEntryRichTextBox.Text = messageManager.GetString("InfoDiaryLocked");
                    diaryEntryRichTextBox.Enabled = false;
                    MonthCalendar.BoldedDates = null;
                    calendarPanel.Enabled = false;
                    buttonsPanel.Enabled = false;
                    editToolStripMenuItem.Enabled = false;
                    newToolStripMenuItem.Enabled = false;
                    openToolStripMenuItem.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    changePasswordToolMenuItem.Enabled = false;
                    rescanHashtagsMenuItem.Enabled = false;
                    exportDiaryToolStripMenuItem.Enabled = false;
                    hashtagsLabel.Visible = false;
                    HashtagsListBox.Visible = false;

                    diaryManager.Password = "";
                    lockDiaryButton.Image = Properties.Resources.ic_lock_black_36dp;
                    lockDiaryButton.Click -= LockDiaryButton_Click;
                    lockDiaryButton.Click += UnlockDiaryButton_Click;
                    Timers.AutoLockTimer.Stop();
                    Timers.AutoSaveTimer.Stop();
                };
                if (diaryManager != null)
                {
                    try
                    {
                        diaryManager.SaveCurrentDiaryEntry();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    LockUiElements();
                    // set global diaryIsLocked flag
                    diaryIsLocked = true;
                }
            }
        }

        /// <summary>
        /// unlock diary: ask user for password and validate it, enable calendar and RichTextBox,
        /// make unlock button to lock button
        /// start AutoLockTimer and AutoSaveTimer
        /// </summary>
        private void UnlockDiary()
        {
            if (diaryIsLocked)
            {
                // that action brings the gui in the diaryunlocked state
                Action EnableUiElements = delegate
                {
                    diaryEntryRichTextBox.Enabled = true;
                    calendarPanel.Enabled = true;
                    buttonsPanel.Enabled = true;
                    editToolStripMenuItem.Enabled = true;
                    newToolStripMenuItem.Enabled = true;
                    openToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    changePasswordToolMenuItem.Enabled = true;
                    rescanHashtagsMenuItem.Enabled = true;
                    exportDiaryToolStripMenuItem.Enabled = true;
                    hashtagsLabel.Visible = true;
                    HashtagsListBox.Visible = true;

                    lockDiaryButton.Click -= UnlockDiaryButton_Click;
                    lockDiaryButton.Click += LockDiaryButton_Click;
                    lockDiaryButton.Image = Properties.Resources.ic_lock_open_black_36dp;
                    Timers.AutoLockTimer.Start();
                    Timers.AutoSaveTimer.Start();
                };
                if (diaryManager != null)
                {
                    string password = PromptForPasswordAndValidate(diaryManager);
                    if (password != null)
                    {
                        diaryManager.Password = password;
                        try
                        {
                            diaryManager.LoadDiaryEntry(MonthCalendar.SelectionStart);
                            diaryEntryRichTextBox.Text = diaryManager.CurrentDiaryEntry.Text;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        MonthCalendar.BoldedDates = diaryManager.AvailableDates.ToArray();
                        EnableUiElements();
                        diaryIsLocked = false;
                    }
                }
                else // no diaryManager instance -> diary is closed or not yet opened -> set gui to diaryunlocked state
                {
                    diaryEntryRichTextBox.Text = "";
                    EnableUiElements();
                    diaryIsLocked = false;
                }
            }
        }

        /// <summary>
        /// initiate AutoLockTimer with values from settings file 
        /// </summary>
        private void InitAutoLockTimer()
        {
            Timers.AutoLockTimer.Interval = new DiarySettings().AutoLockSeconds * 1000;
            Timers.AutoLockTimer.Tick += LockDiaryButton_Click;
            Timers.AutoLockTimer.Start();
        }

        /// <summary>
        /// adapt GUI depending on if diary is opened or not
        /// </summary>
        /// <param name="lockGui"></param>
        private void LockGui(bool lockGui)
        {
            // this action switches between locked and unlocked state
            // locked means only elements are available that don't need a diaryManager instance (new, open, exit, options, about)
            Action<bool> UnlockGuiElements = delegate (bool unlockGui)
            {
                editToolStripMenuItem.Enabled = unlockGui;
                saveToolStripMenuItem.Enabled = unlockGui;
                saveButton.Enabled = unlockGui;
                calendarPanel.Enabled = unlockGui;
                diaryEntryRichTextBox.Enabled = unlockGui;
                lockDiaryButton.Enabled = unlockGui;
                closeDiaryToolStripMenuItem.Enabled = unlockGui;
                changePasswordToolMenuItem.Enabled = unlockGui;
                rescanHashtagsMenuItem.Enabled = unlockGui;
                exportDiaryToolStripMenuItem.Enabled = unlockGui;
                hashtagsLabel.Visible = unlockGui;
                HashtagsListBox.Visible = unlockGui;


                // at locking the bolded dates will be removed from calendar
                if (!unlockGui)
                    MonthCalendar.BoldedDates = null;
            };
            if (diaryManager != null)
            {
                if (!lockGui)
                {
                    UnlockGuiElements(true);
                }
                else
                {
                    UnlockGuiElements(false);
                }
            }
            else // no diaryManager instance -> diary is closed or not yet opened -> set gui to locked state
            {
                UnlockGuiElements(false);
            }
        }

        /// <summary>
        /// select previous date wir a diaryEntry
        /// </summary>
        private void SetPreviousDate()
        {
            DateTime dateToSet = diaryManager.GetNextDate(MonthCalendar.SelectionStart, false);
            MonthCalendar.SelectionStart = dateToSet;
        }

        /// <summary>
        /// select next date with a diaryEntry 
        /// </summary>
        private void SetNextDate()
        {
            DateTime dateToSet = diaryManager.GetNextDate(MonthCalendar.SelectionStart, true);
            MonthCalendar.SelectionStart = dateToSet;
        }

        /// <summary>
        /// append workDirectory to Titlebar if a Diary is opened
        /// </summary>
        private void SetFormText()
        {
            if (diaryManager != null)
            {
                string formText = "CryptDiary ";
                string workDirectory = new DiarySettings().WorkDirectory;
                if (Directory.Exists(workDirectory))
                {
                    string[] directories = workDirectory.Split('\\');
                    formText += "- " + directories[directories.Length - 1];
                }
                this.Text = formText;
            }
            else
            {
                this.Text = "CryptDiary";
            }
        }

        /// <summary>
        /// exports the workDirectory's xml files to a single .zip file
        /// </summary>
        private void ExportWorkDirectoryToZipFile()
        {
            if (diaryManager != null)
            {
                string directory = diaryManager.WorkDirectory;
                exportFileDialog.InitialDirectory = directory;
                // File name = diaryname-yyyy-MM-dd-hhmm.zip
                string currentDateTime = String.Format("{0,4:D4}-{1,2:D2}-{2,2:D2}-{3,2:D2}{4,2:D2}",
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    DateTime.Now.Hour,
                    DateTime.Now.Minute);
                    
                exportFileDialog.FileName = directory.Split('\\')[directory.Split('\\').Length - 1] + "-" + currentDateTime + ".zip";
                if (exportFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        diaryManager.ExportDiaryToZip(exportFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        #region EventHandlers

        // depending on the sender object, a new or a existing directory will be set as workDirectory
        private void openFolderButton_Click(object sender, EventArgs e)
        {
            if (sender == newToolStripMenuItem)
            {
                OpenNewDiary();
            }
            else
            {
                OpenExistingDiary();
            }
        }

        // Exit = close diary, then close main form
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDiary();
            this.Close();
        }

        // key pressed in RichTextBox prevents AutoLock + Keyboard-Shortcuts for next and previous diaryEntry and LockDiary
        private void diaryEntryRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                previousDiaryEntryButton_Click(diaryEntryRichTextBox, null);
            }

            if (e.KeyCode == Keys.Right && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                nextDiaryEntryButton_Click(diaryEntryRichTextBox, null);
            }

            if (e.KeyCode == Keys.L && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                LockDiaryButton_Click(diaryEntryRichTextBox, null);
            }

            Timers.RestartAutoLockTimer();
        }

        // mouse movement prevents AutoLock
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!diaryIsLocked)
                Timers.RestartAutoLockTimer();
        }

        // write the text displayed at the GUI into the CurrentDiaryEntry object
        private void diaryEntryRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (diaryManager != null && !diaryIsLocked)
            {
                diaryManager.CurrentDiaryEntry.Text = diaryEntryRichTextBox.Text;
            }
        }

        /// <summary>
        /// gets the date from the selected ListBox Item and sets this date on the MonthCalendar, simulating a manual click on this date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatesBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox datesBox = sender as ListBox;
            DateTime selectedDate = new DateTime();
            if (datesBox.SelectedItem != null)
            {
                try
                {
                    selectedDate = DateTime.Parse(datesBox.SelectedItem.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (diaryManager != null)
                {
                    MonthCalendar.SelectionStart = selectedDate;
                }
            }
            datesBox.Dispose();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (diaryManager != null && !diaryIsLocked)
                diaryManager.SaveCurrentDiaryEntry();
        }

        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            ChangeDate((sender as MonthCalendar).SelectionStart);
        }

        private void LockDiaryButton_Click(object sender, EventArgs e)
        {
            if (sender != Timers.AutoLockTimer)
                LockDiary();
            else
                if (new DiarySettings().AutoLock)
                LockDiary();
        }

        private void UnlockDiaryButton_Click(object sender, EventArgs e)
        {
            UnlockDiary();
        }

        private void changePasswordToolMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword();
        }

        private void previousDiaryEntryButton_Click(object sender, EventArgs e)
        {
            SetPreviousDate();
        }

        private void nextDiaryEntryButton_Click(object sender, EventArgs e)
        {
            SetNextDate();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenExistingDiary();
        }

        private void closeDiaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDiary();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void rescanHashtagsMenuItem_Click(object sender, EventArgs e)
        {
            if (diaryManager != null)
            {
                diaryManager.RescanHashtags();
            }
        }

        private void HashtagsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawHashtagDatesListBox();
        }

        private void DatesBox_LostFocus(object sender, EventArgs e)
        {
            CloseAllDateListBoxes();
        }

        private void exportDiaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportWorkDirectoryToZipFile();
        }

        #endregion

        #region copy paste etc
        private void einfügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diaryEntryRichTextBox.Paste();
        }

        private void rückgängigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diaryEntryRichTextBox.Undo();
        }

        private void wiederholenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diaryEntryRichTextBox.Redo();
        }

        private void ausschneidenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diaryEntryRichTextBox.Cut();
        }

        private void kopierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diaryEntryRichTextBox.Copy();
        }

        private void alleauswählenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diaryEntryRichTextBox.SelectAll();
        }
        #endregion

        /// <summary>
        /// Draws a List with dates corresponding to the currently selected hashtag in HashtagsListBox
        /// </summary>
        private void DrawHashtagDatesListBox()
        {
            if (diaryManager != null)
            {
                // close possibly opened datesBoxes on MainForm
                CloseAllDateListBoxes();

                // create new ListBox with dates
                ListBox datesListBox = new ListBox();
                datesListBox.Name = "datesListBox";
                datesListBox.MinimumSize = new Size(85, datesListBox.ItemHeight);
                int yOffset = HashtagsListBox.SelectedIndex * HashtagsListBox.ItemHeight;
                datesListBox.MaximumSize = new Size(85, HashtagsListBox.Height - yOffset);

                // location should be top aligned with selected hashtag and right to it

                Point datesBoxLocation = new Point();
                datesBoxLocation.X = HashtagsListBox.Location.X + HashtagsListBox.Width;
                datesBoxLocation.Y = HashtagsListBox.Location.Y + yOffset;
                datesListBox.Location = datesBoxLocation;

                datesListBox.LostFocus += DatesBox_LostFocus;
                datesListBox.SelectedValueChanged += DatesBox_SelectedValueChanged;

                // get hashtagText
                string hashtagText = HashtagsListBox.SelectedItem.ToString();

                // get hashtagDates
                List<DateTime> hashtagDates = new List<DateTime>();
                foreach (DateTime date in diaryManager.HashtagDictionary[new Data.Hashtag(hashtagText)])
                {
                    hashtagDates.Add(date);
                }
                hashtagDates.Sort();

                // write dates in list
                foreach (var date in hashtagDates)
                {
                    datesListBox.Items.Add(date.ToShortDateString());
                }

                // display list
                datesListBox.Height = (datesListBox.Items.Count + 1) * datesListBox.ItemHeight;
                this.Controls.Add(datesListBox);
                datesListBox.Show();
                datesListBox.BringToFront();
                datesListBox.Focus();
            }
        }

        /// <summary>
        /// closes all open datesListBox objects on the MainForm
        /// </summary>
        private void CloseAllDateListBoxes()
        {
            // close possibly opened datesBoxes on MainForm
            var datesListBoxesToClose = this.Controls.Find("datesListBox", false);

            foreach (var listBox in datesListBoxesToClose)
            {
                listBox.Dispose();
            }
        }


        // TODO: add tooltips with diary entry preview texts for each date

        #region Spielwiese

        #endregion

    }
}
