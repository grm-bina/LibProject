﻿#pragma checksum "C:\документы\учеба\села\C# in depth\проект\LibProject\LibProject.Client\UserControls\BorrowedItemsCurrentUser.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0CA2D25607B94A101221EA1C40F3BF02"
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
    partial class BorrowedItemsCurrentUser : 
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
            case 2: // UserControls\BorrowedItemsCurrentUser.xaml line 26
                {
                    this.itemIdTbl = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // UserControls\BorrowedItemsCurrentUser.xaml line 27
                {
                    this.copyIdTbl = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // UserControls\BorrowedItemsCurrentUser.xaml line 41
                {
                    this.descriptionListBx = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.descriptionListBx).SelectionChanged += this.descriptionListBx_SelectionChanged;
                }
                break;
            case 5: // UserControls\BorrowedItemsCurrentUser.xaml line 43
                {
                    this.returnTimeListBx = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.returnTimeListBx).SelectionChanged += this.returnTimeListBx_SelectionChanged;
                }
                break;
            case 6: // UserControls\BorrowedItemsCurrentUser.xaml line 45
                {
                    this.lateTimeListBx = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.lateTimeListBx).SelectionChanged += this.lateTimeListBx_SelectionChanged;
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

