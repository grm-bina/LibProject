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
using LibProject.Globals;
using LibProject.Exceptions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibProject.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        int _numOfItems;
        int _numOfDays;


        public Settings()
        {

            this.InitializeComponent();
            curentPageTxt.Text = "Library Settings";
            _numOfItems = LibProject.Globals.Settings.MaxBorrowedItemsPerUser;
            _numOfDays = LibProject.Globals.Settings.MaxBorrowingTime.Days;
            UpdateTextBoxes();
        }

        #region Methods
        // show data, update text-boxes and lock minus-buttons if need
        private void UpdateTextBoxes()
        {
            messageTbl.Text = "";
            errorTbl.Text = "";

            numOfDays.Text = _numOfDays.ToString();
            numOfItems.Text = _numOfItems.ToString();

            if (_numOfDays == 1)
                minusDayBtn.IsEnabled = false;
            else
                minusDayBtn.IsEnabled = true;
            if (_numOfItems == 1)
                minusItemBtn.IsEnabled = false;
            else
                minusItemBtn.IsEnabled = true;
        }

        // save changes
        private void UpdateSettings()
        {
            try
            {
                LibProject.Globals.Settings.SetMaxBorrowedItemsPerUser(_numOfItems);
                LibProject.Globals.Settings.SetMaxBorrowingTime(_numOfDays);
                errorTbl.Text = "";
                messageTbl.Text = "Settings saved";
            }
            catch(Exception e)
            {
                errorTbl.Text = e.Message;
            }
        }
        #endregion


        #region Events
        // minus day
        private void minusDayBtn_Click(object sender, RoutedEventArgs e)
        {

            if (_numOfDays > 1)
                _numOfDays--;
            UpdateTextBoxes();
        }

        // plus day
        private void plusDayBtn_Click(object sender, RoutedEventArgs e)
        {
            _numOfDays++;
            UpdateTextBoxes();
        }

        // minus item
        private void minusItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_numOfItems > 1)
                _numOfItems--;
            UpdateTextBoxes();
        }

        // plus item
        private void plusItemBtn_Click(object sender, RoutedEventArgs e)
        {
            _numOfItems++;
            UpdateTextBoxes();
        }

        // save
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateSettings();
        }

        #endregion

    }
}
