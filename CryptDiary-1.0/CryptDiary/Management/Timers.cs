using System.Diagnostics;
using System.Windows.Forms;

namespace CryptDiary.Management
{
    /// <summary>
    /// contains all timers necessary for the project
    /// </summary>
    static public class Timers
    {
        static public Stopwatch PasswordIterationBenchmark = new Stopwatch();
        static public Timer AutoLockTimer = new Timer();
        static public Timer AutoSaveTimer = new Timer();

        /// <summary>
        /// restart AutoLockTimer
        /// </summary>
        static public void RestartAutoLockTimer()
        {
            Timers.AutoLockTimer.Stop();
            Timers.AutoLockTimer.Start();
        }

        /// <summary>
        /// restart AutoSaveTimer
        /// </summary>
        static public void RestartAutoSaveTimer()
        {
            Timers.AutoSaveTimer.Stop();
            Timers.AutoSaveTimer.Start();
        }
    }
}
