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
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibProject.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddBook : Page
    {
        int _numOfcopies;
        int _id;
        LibraryManager _items;
        AbstractItem _foundedItem;
        List<Guid> _newCopiesId;

        public AddBook()
        {
            this.InitializeComponent();
            curentPageTxt.Text = "Add Book";
            HideAddCopiesControllers();
            newCopiesId.Visibility = Visibility.Collapsed;
            finishBtn.Visibility = Visibility.Collapsed;
            _items = LibraryManager.GetInstance;
        }

        #region Methods
        // For hide/show add-copies-controllers
        // Hide
        private void HideAddCopiesControllers()
        {
            // enable id-text-box and continue-button
            idTbx.IsEnabled = true;
            addItemBtn.IsEnabled = true;

            // hide add-copy part
            copyTbl.Visibility = Visibility.Collapsed;
            minusBtn.Visibility = Visibility.Collapsed;
            numOfCopiesTbx.Visibility = Visibility.Collapsed;
            plusBtn.Visibility = Visibility.Collapsed;
            addCopBtn.Visibility = Visibility.Collapsed;
            foundedItemTbl.Text = "";
            undoBtn.Visibility = Visibility.Collapsed;

        }
        // Show
        private void ShowAddCopiesControllers()
        {
            // disable id-textbox and continue-button
            idTbx.IsEnabled = false;
            addItemBtn.IsEnabled = false;

            // show messages that item exist
            if (_foundedItem != null)
            {
                errorTbl.Text = "The item is already exist";
                foundedItemTbl.Text = _foundedItem.ToString();
            }

            // show add copy part
            copyTbl.Visibility = Visibility.Visible;
            minusBtn.Visibility = Visibility.Visible;
            minusBtn.IsEnabled = false;
            _numOfcopies = 1;
            numOfCopiesTbx.Visibility = Visibility.Visible;
            numOfCopiesTbx.Text = _numOfcopies.ToString();
            plusBtn.Visibility = Visibility.Visible;
            addCopBtn.Visibility = Visibility.Visible;
            undoBtn.Visibility = Visibility.Visible;
        }
        // Disable
        private void DisableAddCopiesControllers()
        {
            errorTbl.Text = "";
            minusBtn.IsEnabled = false;
            numOfCopiesTbx.IsEnabled = false;
            plusBtn.IsEnabled = false;
            addCopBtn.IsEnabled = false;
            undoBtn.IsEnabled = false;
        }




        #endregion

        #region Events

        // Check if item exist and go to create new item or stay in this page and continue add copies
        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(idTbx.Text, out _id))
                errorTbl.Text = "For enter ID use digits only";
            else if(UsersManager.GetInstanse.CurrentUser.Access==UserType.Reader)
                errorTbl.Text = "Readers can't add items";
            else
            {
                if (_items.Exist(_id))
                {
                    _foundedItem = _items.GetItem(_id);
                    ShowAddCopiesControllers();
                }
                else
                {
                    this.Frame.Navigate(typeof(AddEditItem), _id);
                }
            }
        }

        // decrease number of adding-copies
        private void minusBtn_Click(object sender, RoutedEventArgs e)
        {
            _numOfcopies--;
            numOfCopiesTbx.Text = _numOfcopies.ToString();
            if (_numOfcopies == 1)
                minusBtn.IsEnabled = false;
        }

        // increase number of adding-copies
        private void plusBtn_Click(object sender, RoutedEventArgs e)
        {
            _numOfcopies++;
            numOfCopiesTbx.Text = _numOfcopies.ToString();
            if(_numOfcopies > 1)
                minusBtn.IsEnabled = true;
        }

        // undo "add copies" - back to enter other id
        private void undoBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAddCopiesControllers();
        }

        // remove error-message when entering other-id
        private void idTbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorTbl.Text = "";
        }

        // finish add copies, refresh this page
        private void finishBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddBook));
        }

        // add copies
        private void addCopBtn_Click(object sender, RoutedEventArgs e)
        {
            _newCopiesId = _foundedItem.AddCopies(_numOfcopies);
            if (_newCopiesId == null)
                errorTbl.Text = "Error: nothing added";
            else
            {
                DisableAddCopiesControllers();
                StringBuilder representCopyId = new StringBuilder();
                foreach (var item in _newCopiesId)
                    representCopyId.AppendLine(item.ToString());
                newCopiesId.Visibility = Visibility.Visible;
                newCopiesId.Text = representCopyId.ToString();
                finishBtn.Visibility = Visibility.Visible;
            }
        }

        #endregion

    }
}
