using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace HS
{
    // use to get memory available
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }

    public class SysInfo
    {
        #region Interop

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        #endregion

        static TimeSpan GetSystemUpTime()
        {
            try
            {
                PerformanceCounter upTime = new PerformanceCounter("System", "System Up Time");
                upTime.NextValue();
                return TimeSpan.FromSeconds(upTime.NextValue());
            }
            catch (Exception ex)
            {
                Tracer.Error("Error reading system uptime", ex);
            }
            return new TimeSpan(0);
        }

        public override string ToString()
        {
            string str = "<Diag type=\"SysInfo\">";

            try
            {
                str += String.Format("<UserName>{0}</UserName>", SystemInformation.UserName);
                str += String.Format("<OS>{0}</OS>", Environment.OSVersion.ToString());
                str += String.Format("<SysUptime>{0}</SysUptime>", GetSystemUpTime());
                str += String.Format("AppUptime>{0}</AppUptime>", (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());

                MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
                if (GlobalMemoryStatusEx(memStatus))
                {
                    str += String.Format("<TotMem>{0}MB</TotMem>", memStatus.ullTotalPhys / (1024 * 1024));
                    str += String.Format("<AvlMem>{0}MB</AvlMem>", memStatus.ullAvailPhys / (1024 * 1024));
                }

                Process ps = Process.GetCurrentProcess();
                str += String.Format("<ProMem>{0}MB</ProMem>", ps.PrivateMemorySize64 / (1024 * 1024));

                str += "</Diag>";
            }
            catch (Exception ex)
            {
                Tracer.Error("Error formatting system information message", ex);
            }

            return str;
        }
    }
}
