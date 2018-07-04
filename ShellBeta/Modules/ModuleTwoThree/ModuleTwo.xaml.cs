using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Alma.Core;

namespace Alma.Module
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ModuleTwo : ModuleBase
    {
        public ModuleTwo() : base()
        {
            InitializeComponent();
        }

        public override string ModuleDefn
        {
            get { return "MODULETHREE"; }
        }
    }
}
