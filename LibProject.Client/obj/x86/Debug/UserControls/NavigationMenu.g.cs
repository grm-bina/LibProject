﻿#pragma checksum "C:\документы\учеба\села\C# in depth\проект\LibProject\LibProject.Client\UserControls\NavigationMenu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6085409089FFC03B2257FB80A2084A6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibProject.Client
{
    partial class NavigationMenu : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // UserControls\NavigationMenu.xaml line 21
                {
                    this.searchBookBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.searchBookBtn).Click += this.searchBookBtn_Click;
                }
                break;
            case 3: // UserControls\NavigationMenu.xaml line 22
                {
                    this.myBookBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.myBookBtn).Click += this.myBookBtn_Click;
                }
                break;
            case 4: // UserControls\NavigationMenu.xaml line 23
                {
                    this.borrowBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.borrowBtn).Click += this.borrowBtn_Click;
                }
                break;
            case 5: // UserControls\NavigationMenu.xaml line 24
                {
                    this.addBookBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.addBookBtn).Click += this.addBookBtn_Click;
                }
                break;
            case 6: // UserControls\NavigationMenu.xaml line 25
                {
                    this.addUserBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.addUserBtn).Click += this.addUserBtn_Click;
                }
                break;
            case 7: // UserControls\NavigationMenu.xaml line 26
                {
                    this.setBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.setBtn).Click += this.setBtn_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
