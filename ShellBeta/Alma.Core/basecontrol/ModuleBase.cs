using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Alma.Core
{
    public abstract class ModuleBase : UserControl, IModule
    {
        ModuleManager _mm;

        public ModuleBase()
        {
            _mm = ModuleManager.Instance;
        }

        public abstract string ModuleDefn { get; }
    }
}