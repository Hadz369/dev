﻿#pragma checksum "..\..\Shell.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F77180583AC1821F422C628E2EEFA817"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Alma.Controls;
using Alma.Core;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Alma.Module {
    
    
    /// <summary>
    /// Shell
    /// </summary>
    public partial class Shell : Alma.Core.ShellBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 75 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid RootWindow;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border TitleBar;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnHistory;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition navigationColumn;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbxQuickSearch;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnQuickSearch;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MinButton;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MaxButton;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
        #line default
        #line hidden
        
        
        #line 137 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tcModules;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbLogView;
        
        #line default
        #line hidden
        
        
        #line 157 "..\..\Shell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblCursorPosition;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ModernShell;component/shell.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Shell.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\Shell.xaml"
            ((Alma.Module.Shell)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RootWindow = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.TitleBar = ((System.Windows.Controls.Border)(target));
            
            #line 81 "..\..\Shell.xaml"
            this.TitleBar.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TitleBar_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 91 "..\..\Shell.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnHistory = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.navigationColumn = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 7:
            this.tbxQuickSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 106 "..\..\Shell.xaml"
            this.tbxQuickSearch.GotFocus += new System.Windows.RoutedEventHandler(this.tbxQuickSearch_GotFocus);
            
            #line default
            #line hidden
            
            #line 106 "..\..\Shell.xaml"
            this.tbxQuickSearch.LostFocus += new System.Windows.RoutedEventHandler(this.tbxQuickSearch_LostFocus);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnQuickSearch = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\Shell.xaml"
            this.btnQuickSearch.Click += new System.Windows.RoutedEventHandler(this.btnQuickSearch_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.MinButton = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\Shell.xaml"
            this.MinButton.Click += new System.Windows.RoutedEventHandler(this.MinimizeButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.MaxButton = ((System.Windows.Controls.Button)(target));
            
            #line 113 "..\..\Shell.xaml"
            this.MaxButton.Click += new System.Windows.RoutedEventHandler(this.MaximizeButton_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            
            #line 114 "..\..\Shell.xaml"
            this.CloseButton.Click += new System.Windows.RoutedEventHandler(this.CloseButton_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.tcModules = ((System.Windows.Controls.TabControl)(target));
            return;
            case 13:
            this.lbLogView = ((System.Windows.Controls.ListBox)(target));
            return;
            case 14:
            this.lblCursorPosition = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

