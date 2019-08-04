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
using LibProject.Exceptions;
using LibProject.Models;
using LibProject.BL;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibProject.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Borrow : Page
    {
        Respone<string> _feedback;
        HardCopy _input;
        BorrowReturnMode _mode;
        LibraryManager _library;

        public Borrow()
        {
            this.InitializeComponent();
            _library = LibraryManager.GetInstance;
            curentPageTxt.Text = "Borrow and Return Items";
            undoBtn.Visibility = Visibility.Collapsed;
            okBtn.Visibility = Visibility.Collapsed;
            _feedback = new Respone<string>();
        }

        #region Methods
        // Refresh feedback / enable continue
        private void RefreshFeedback()
        {
            if (!_feedback.IsWorking)
                errorTbl.Text = _feedback.Message;
            else
            {
                borrowRadioBtn.IsEnabled = false;
                returnRadioBtn.IsEnabled = false;
                userIdTbx.IsEnabled = false;
                itemIdTbx.IsEnabled = false;
                copyIdTbx.IsEnabled = false;
                errorTbl.Text = "";
                messageTbl.Text = _feedback.Data;
                undoBtn.Visibility = Visibility.Visible;
                okBtn.Visibility = Visibility.Visible;
                borrowReturnBtn.IsEnabled = false;

                if (_mode == BorrowReturnMode.Return)
                    undoBtn.IsEnabled = false;
            }
        }

        // Get input from text-boxes
        private void GetInput()
        {
            _input = new HardCopy();

            int temp;
            if (int.TryParse(userIdTbx.Text, out temp))
                _input.BorrowerId = temp;
            if (int.TryParse(itemIdTbx.Text, out temp))
                _input.ItemId = temp;

            Guid tempGuid;
            if (Guid.TryParse(copyIdTbx.Text, out tempGuid))
                _input.CopyId = tempGuid;
        }

        #endregion

        #region Events
        // Select - Borrow-Mode
        private void borrowRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            userIdTbx.IsEnabled = true;
            borrowReturnBtn.Content = "Borrow";
            _mode = BorrowReturnMode.Borrow;
        }

        // Select - Return-Mode
        private void returnRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            userIdTbx.Text = "";
            userIdTbx.IsEnabled = false;
            borrowReturnBtn.Content = "Return";
            _mode = BorrowReturnMode.Return;

        }

        // Undo
        private void undoBtn_Click(object sender, RoutedEventArgs e)
        {
            _library.Return(_input);

            borrowRadioBtn.IsEnabled = true;
            returnRadioBtn.IsEnabled = true;
            if (_mode == BorrowReturnMode.Borrow)
                userIdTbx.IsEnabled = true;
            itemIdTbx.IsEnabled = true;
            copyIdTbx.IsEnabled = true;
            messageTbl.Text = "";
            undoBtn.Visibility = Visibility.Collapsed;
            okBtn.Visibility = Visibility.Collapsed;
            borrowReturnBtn.IsEnabled = true;

        }

        // Borrow or Return Operation
        private void borrowReturnBtn_Click(object sender, RoutedEventArgs e)
        {
            GetInput();
            switch (_mode)
            {
                case BorrowReturnMode.Borrow:
                    {
                        _feedback = _library.Borrow(_input);
                        break;
                    }
                case BorrowReturnMode.Return:
                    {
                        _feedback = _library.Return(_input);
                        break;
                    }
            }
            RefreshFeedback();
        }

        // reload page
        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Borrow), _mode);
        }

        // deside mode when page loaded
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var temp = e.Parameter;
            if(temp is BorrowReturnMode)
            {
                if ((BorrowReturnMode)temp == BorrowReturnMode.Borrow)
                    borrowRadioBtn.IsChecked = true;
                else
                    returnRadioBtn.IsChecked = true;
            }
            else
                borrowRadioBtn.IsChecked = true;
        }

        #endregion

    }
}
