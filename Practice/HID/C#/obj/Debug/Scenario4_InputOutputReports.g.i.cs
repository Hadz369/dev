﻿

#pragma checksum "C:\private\dev\project\VS\Practice\HID\C#\Scenario4_InputOutputReports.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "60EE3C123BDE48283D8A0F9DBB7F0069"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomHidDeviceAccess
{
    partial class InputOutputReports : global::SDKTemplate.Common.LayoutAwarePage
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid LayoutRoot; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid Input; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid Output; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState OutputFullScreenLandscape; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState OutputFilled; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState OutputFullScreenPortrait; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState OutputSnapped; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel DeviceScenarioContainer; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel SuperMuttScenario; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button ButtonGetBooleanInputReport; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button ButtonSendBooleanOutputReport; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ComboBox BooleanValueToWrite; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock SuperMuttScenarioText; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button ButtonGetNumericInputReport; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button ButtonSendNumericOutputReport; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ComboBox NumericValueToWrite; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState InputFullScreenLandscape; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState InputFilled; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState InputFullScreenPortrait; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState InputSnapped; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///Scenario4_InputOutputReports.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            LayoutRoot = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("LayoutRoot");
            Input = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("Input");
            Output = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("Output");
            OutputFullScreenLandscape = (global::Windows.UI.Xaml.VisualState)this.FindName("OutputFullScreenLandscape");
            OutputFilled = (global::Windows.UI.Xaml.VisualState)this.FindName("OutputFilled");
            OutputFullScreenPortrait = (global::Windows.UI.Xaml.VisualState)this.FindName("OutputFullScreenPortrait");
            OutputSnapped = (global::Windows.UI.Xaml.VisualState)this.FindName("OutputSnapped");
            DeviceScenarioContainer = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("DeviceScenarioContainer");
            SuperMuttScenario = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("SuperMuttScenario");
            ButtonGetBooleanInputReport = (global::Windows.UI.Xaml.Controls.Button)this.FindName("ButtonGetBooleanInputReport");
            ButtonSendBooleanOutputReport = (global::Windows.UI.Xaml.Controls.Button)this.FindName("ButtonSendBooleanOutputReport");
            BooleanValueToWrite = (global::Windows.UI.Xaml.Controls.ComboBox)this.FindName("BooleanValueToWrite");
            SuperMuttScenarioText = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("SuperMuttScenarioText");
            ButtonGetNumericInputReport = (global::Windows.UI.Xaml.Controls.Button)this.FindName("ButtonGetNumericInputReport");
            ButtonSendNumericOutputReport = (global::Windows.UI.Xaml.Controls.Button)this.FindName("ButtonSendNumericOutputReport");
            NumericValueToWrite = (global::Windows.UI.Xaml.Controls.ComboBox)this.FindName("NumericValueToWrite");
            InputFullScreenLandscape = (global::Windows.UI.Xaml.VisualState)this.FindName("InputFullScreenLandscape");
            InputFilled = (global::Windows.UI.Xaml.VisualState)this.FindName("InputFilled");
            InputFullScreenPortrait = (global::Windows.UI.Xaml.VisualState)this.FindName("InputFullScreenPortrait");
            InputSnapped = (global::Windows.UI.Xaml.VisualState)this.FindName("InputSnapped");
        }
    }
}



