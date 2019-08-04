using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LibProject.BL;
using LibProject.Models;
using LibProject.Globals;
using LibProject.Exceptions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LibProject.Client
{
    public sealed partial class BorrowedItemsCurrentUser : UserControl
    {
        User _currentUser;

        public BorrowedItemsCurrentUser()
        {
            this.InitializeComponent();
            _currentUser = UsersManager.GetInstanse.CurrentUser;
            LoadBorrowesItems();
        }


        #region Methods
        // load data to lists
        private void LoadBorrowesItems()
        {
            if (_currentUser != null && _currentUser.IsKeepItems)
            {
                foreach (HardCopy item in _currentUser.GetBorroewedItems())
                {
                    descriptionListBx.Items.Add(item);
                    int days;
                    TimeSpan keepTime = DateTime.Now.Subtract(item.BorrowedSinse);
                    days = LibProject.Globals.Settings.MaxBorrowingTime.Days - keepTime.Days;
                    if (days > 0)
                    {
                        returnTimeListBx.Items.Add(days);
                        lateTimeListBx.Items.Add(0);
                    }
                    else
                    {
                        days = (-days);
                        lateTimeListBx.Items.Add(days);
                        returnTimeListBx.Items.Add(0);
                    }
                }
            }
        }

        // load 1-item data to text-boxes
        private void LoadItemIdData()
        {
            if (descriptionListBx.SelectedItem != null && descriptionListBx.SelectedItem is HardCopy)
            {
                itemIdTbl.Text = ((HardCopy)descriptionListBx.SelectedItem).ItemId.ToString();
                copyIdTbl.Text = ((HardCopy)descriptionListBx.SelectedItem).CopyId.ToString();
            }
        }
        #endregion

        #region Events

        // sinchronize selection
        private void descriptionListBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            returnTimeListBx.SelectedIndex = descriptionListBx.SelectedIndex;
            lateTimeListBx.SelectedIndex = descriptionListBx.SelectedIndex;

            LoadItemIdData();
        }
        private void returnTimeListBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            descriptionListBx.SelectedIndex = returnTimeListBx.SelectedIndex;
            lateTimeListBx.SelectedIndex = returnTimeListBx.SelectedIndex;

            LoadItemIdData();
        }
        private void lateTimeListBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            returnTimeListBx.SelectedIndex = lateTimeListBx.SelectedIndex;
            descriptionListBx.SelectedIndex = lateTimeListBx.SelectedIndex;

            LoadItemIdData();
        }

        #endregion

    }
}
