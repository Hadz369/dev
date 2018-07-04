using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TabControlTest
{
    public static class WindowManager
    {
        static List<Window> _windows = new List<Window>();
        static Object _locker = new object();
        static bool _appClosing = false;

        public static bool ApplicationClosing { get { return _appClosing; } set { _appClosing = value; } }

        public static IEnumerable<Window> Windows { get { return _windows; } }

        public static void Add(Window window)
        {
            lock (_locker)
            {
                _windows.Add(window);
            }
        }

        public static void Remove(Window window)
        {
            if (!_appClosing)
            {
                lock (_locker)
                {
                    _windows.Remove(window);
                }
            }
        }
    }
}
