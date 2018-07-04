using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptDiary.Gui
{
    public partial class OptionsForm : Form
    {
        // localisation
        private Dictionary<string, string> Languages = new Dictionary<string, string>();
        ResourceManager IdentifiersManager = new ResourceManager("CryptDiary.Resources.Identifiers", typeof(OptionsForm).Assembly);

        // thread for running the iteration benchmark
        Thread benchmarkThread;

        public OptionsForm()
        {
            // localisation of the GUI
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(new DiarySettings().CultureInfo);
            InitializeComponent();
        }

        // init options dialogbox
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            // fill Languages dictionary with available languages
            // TODO (optional): add Polski
            Languages["Deutsch"] = "de";
            Languages["English"] = "en";

            // write available languages in combo box
            foreach (var Language in Languages)
            {
                languagesComboBox.Items.Add(Language.Key);
                // preselect current language
                if (new DiarySettings().CultureInfo == Language.Value)
                {
                    languagesComboBox.SelectedItem = Language.Key;
                }
            }

            // get values from settings file
            DiarySettings Settings = new DiarySettings();
            autoSaveCheckBox.Checked = Settings.AutoSave;
            autoSaveIntervalNumericUpDown.Value = Settings.AutoSaveInterval;
            autoLockCheckBox.Checked = Settings.AutoLock;
            autoLockTimeNumericUpDown.Value = Settings.AutoLockSeconds;
            passwordIterationsNumericUpDown.Value = Settings.PasswordIterations;

            // make benchmark result label invisible
            iterationTimeLabel.Visible = false;

            // draw element by using their CheckedChanged EventHandler
            autoSaveCheckBox_CheckedChanged(this, null);
            autoLockCheckBox_CheckedChanged(this, null);
        }

        // At checkbox changes grey out corresponding controls
        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autoSaveIntervalLabel.Enabled = autoSaveCheckBox.Checked;
            autoSaveSecondsLabel.Enabled = autoSaveCheckBox.Checked;
            autoSaveIntervalNumericUpDown.Enabled = autoSaveCheckBox.Checked;
        }

        // At checkbox changes grey out corresponding controls
        private void autoLockCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            autoLockTextLabel1.Enabled = autoLockCheckBox.Checked;
            autoLockTextLabel2.Enabled = autoLockCheckBox.Checked;
            autoLockTimeNumericUpDown.Enabled = autoLockCheckBox.Checked;
        }

        // write values to settings file, restart all timers
        private void applyButton_Click(object sender, EventArgs e)
        {
            DiarySettings Settings = new DiarySettings();
            Settings.AutoSave = autoSaveCheckBox.Checked;
            Settings.AutoSaveInterval = (int)autoSaveIntervalNumericUpDown.Value;
            Settings.AutoLock = autoLockCheckBox.Checked;
            Settings.AutoLockSeconds = (int)autoLockTimeNumericUpDown.Value;
            Settings.PasswordIterations = (int)passwordIterationsNumericUpDown.Value;
            if (languagesComboBox.SelectedItem != null)
            {
                Settings.CultureInfo = Languages[languagesComboBox.SelectedItem.ToString()];
            }
            Settings.Save();

            Management.Timers.AutoLockTimer.Stop();
            Management.Timers.AutoLockTimer.Interval = Settings.AutoLockSeconds * 1000;
            Management.Timers.AutoLockTimer.Start();
            Management.Timers.AutoSaveTimer.Stop();
            Management.Timers.AutoSaveTimer.Interval = Settings.AutoSaveInterval * 1000;
            Management.Timers.AutoSaveTimer.Start();
        }

        // Cancel: close this windows
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // OK = apply + close
        private void okButton_Click(object sender, EventArgs e)
        {
            applyButton_Click(this, null);
            this.Close();
        }

        // restore default values
        private void defaultsButton_Click(object sender, EventArgs e)
        {
            // get values from settings-file
            DiarySettings Settings = new DiarySettings();
            autoSaveCheckBox.Checked = Settings.DefaultAutoSave;
            autoSaveIntervalNumericUpDown.Value = Settings.DefaultAutoSaveInterval;
            autoLockCheckBox.Checked = Settings.DefaultAutoLock;
            autoLockTimeNumericUpDown.Value = Settings.DefaultAutoLockSeconds;
            passwordIterationsNumericUpDown.Value = Settings.DefaultPasswordIterations;
        }

        // a 256 bit AES key will be created in a separate thread and the time needed will be written to the label
        private void benchmarkButton_Click(object sender, EventArgs e)
        {
            // configure and start benchmark thread
            benchmarkThread = new Thread(new ThreadStart(passwordIterationBenchmark));
            benchmarkThread.Start();

            // make benchmark button to cancel button
            benchmarkButton.Text = IdentifiersManager.GetString("Cancel");
            benchmarkButton.Click -= benchmarkButton_Click;
            benchmarkButton.Click += BenchmarkCancelButton_Click;
        }

        // cancels benchmark thread and makes cancel button to benchmark button again
        private void BenchmarkCancelButton_Click(object sender, EventArgs e)
        {
            benchmarkThread.Abort();
            benchmarkButton.Text = IdentifiersManager.GetString("Benchmark");
            benchmarkButton.Click -= BenchmarkCancelButton_Click;
            benchmarkButton.Click += benchmarkButton_Click;
        }

        // password iteration benchmark
        private void passwordIterationBenchmark()
        {
            string labelText = Management.AesCrypt.GetPasswordIterationTime((int)passwordIterationsNumericUpDown.Value).ToString() + " ms";
            this.Invoke((MethodInvoker)delegate
            {
                iterationTimeLabel.Text = labelText;
                iterationTimeLabel.Visible = true;
                // make cancel button to benchmark button
                BenchmarkCancelButton_Click(this, null);
            });
        }

        // set focus to TextBox at mouseover, so it is scrollable with mouse wheel instantly
        private void passwordIterationInfoTextBox_MouseEnter(object sender, EventArgs e)
        {
            passwordIterationInfoTextBox.Focus();
            passwordIterationInfoTextBox.Select(0, 0);
        }

        // give some read only element the focus
        private void passwordIterationInfoTextBox_MouseLeave(object sender, EventArgs e)
        {
            passwordIterationLabel.Focus();
        }
    }
}
