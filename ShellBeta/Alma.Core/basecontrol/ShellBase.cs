using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Alma.Core
{
    public abstract class ShellBase : Window, IShell
    {
        ModuleManager _mm;

        public ShellBase()
        {
            _mm = ModuleManager.Instance;
        }

        public ModuleManager ModuleManager { get { return _mm; } }

        public abstract string ModuleDefn { get; }
    }
}
