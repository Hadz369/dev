﻿

#pragma checksum "C:\private\dev\project\VS\Practice\HID\C#\Scenario1_DeviceConnect.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "21F7CC17A8BC4AAAD91031A00EB65E6F"
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
    partial class DeviceConnect : global::SDKTemplate.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 40 "..\..\Scenario1_DeviceConnect.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ConnectToDevice_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 41 "..\..\Scenario1_DeviceConnect.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.DisconnectFromDevice_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


