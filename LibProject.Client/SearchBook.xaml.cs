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
using LibProject.BL;
using LibProject.Models;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibProject.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchBook : Page
    {
        // data for representation
        private AbstractItem _selectdItem;
        private LibraryManager _library;
        Respone<List<AbstractItem>> _feedback;
        Enum _selectedGenre;
        SearchCriteria _criteria;


        public SearchBook()
        {
            this.InitializeComponent();
            mainGenreCombBox.Items.Clear();
            mainGenreCombBox.ItemsSource = Enum.GetValues(typeof(Genre));
            RefreshPage();

            // show/hide controllers for different users
            switch (UsersManager.GetInstanse.CurrentUser.Access)
            {
                case UserType.Admin:
                    delBtn.Visibility = Visibility.Visible;
                    editBtn.Visibility = Visibility.Visible;
                    byIdRBtn.Visibility = Visibility.Visible;
                    break;
                case UserType.Librarian:
                    delBtn.Visibility = Visibility.Collapsed;
                    editBtn.Visibility = Visibility.Visible;
                    byIdRBtn.Visibility = Visibility.Visible;
                    break;
                case UserType.Reader:
                    delBtn.Visibility = Visibility.Collapsed;
                    editBtn.Visibility = Visibility.Collapsed;
                    byIdRBtn.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        #region Methods
        // prepare/refresh page - start contition
        private void RefreshPage()
        {
            curentPageTxt.Text = "Search Book";
            byTitleRBtn.IsChecked = true;
            _selectdItem = null;
            _library = LibraryManager.GetInstance;

            mainGenreCombBox.SelectedIndex = 0;
            _criteria = SearchCriteria.Title;

            _feedback = _library.Search(_criteria, " ");
            RefreshListBoxSearchResults();
        }


        // Show search-result
        private void RefreshListBoxSearchResults()
        {
            searchResultsLBx.Items.Clear();

            if (_feedback != null)
            {
                if (!_feedback.IsWorking)
                {
                    errorTBl.Text = _feedback.Message;
                }
                else
                {
                    errorTBl.Text = "";
                    foreach (AbstractItem item in _feedback.Data)
                    {
                        searchResultsLBx.Items.Add(item);
                    }
                }
            }

            _selectdItem = null;
            ClearSelectedItemRepresentationTextBoxes();
            editBtn.IsEnabled = false;
            delBtn.IsEnabled = false;
        }

        // Clean Item-Data-Boxes
        private void ClearSelectedItemRepresentationTextBoxes()
        {
            titleTb.Text = "";
            authorTb.Text = "";
            yearTb.Text = "";
            volTb.Text = "";
            issueTb.Text = "";
            aviableTb.Text = "";
            annotTb.Text = "";
            genreTb.Text = "";
            placeTb.Text = "";
            typeTb.Text = "";
            publishTb.Text = "";
            idTb.Text = "";
            copyIdTb.Text = "";
        }

        // Show Item-Data
        private void LoadSelectedItemDataToRepresentationTextBoxes()
        {
            if (_selectdItem != null)
            {
                // load abstract-item data
                titleTb.Text = _selectdItem.Title;
                if(_selectdItem.Year!=null)
                    yearTb.Text = _selectdItem.Year.ToString();
                if(_selectdItem.EditionOrIssue!=null)
                    issueTb.Text = _selectdItem.EditionOrIssue.ToString();
                aviableTb.Text = $"{_selectdItem.AvaibleCopies} (of {_selectdItem.TotalNumCopies})";
                if (_selectdItem.Annotation != null)
                    annotTb.Text = _selectdItem.Annotation;

                StringBuilder genre = new StringBuilder();
                genre.Append(_selectdItem.Category.ToString());
                var subgeners = _selectdItem.GetSubgeneres();
                if (subgeners != null)
                {
                    foreach (var item in subgeners)
                        genre.Append($", {item}");
                }
                genreTb.Text = genre.ToString();

                if (_selectdItem.PlaceInLib!=null)
                    placeTb.Text = _selectdItem.PlaceInLib;
                if(_selectdItem.Publisher!=null)
                    publishTb.Text = _selectdItem.Publisher;
                idTb.Text = _selectdItem.ID.ToString();

                StringBuilder copyId = new StringBuilder();
                var tempCopyID = _selectdItem.AviableCopiesID;
                foreach (var item in tempCopyID)
                    copyId.AppendLine(item.ToString());
                copyIdTb.Text = copyId.ToString();

                // load book-data
                Book tempBook = _selectdItem as Book;
                if (tempBook != null)
                {
                    typeTb.Text = "Book";
                    StringBuilder authors = new StringBuilder();
                    var tempAuthors = tempBook.GetAuthors();
                    if (tempAuthors != null)
                    {
                        foreach (HumanName item in tempAuthors)
                            authors.AppendLine($"{item.LastName} {item.FirstName} {item.MiddleNameOrPatronymic}");
                    }
                    authorTb.Text = authors.ToString();
                }

                // load journal-data
                Journal tempJournal = _selectdItem as Journal;
                if (tempJournal != null)
                {
                    typeTb.Text = "Journal";
                    volTb.Text = tempJournal.Volume.ToString();
                }
            }
        }

        // Sub-Geners Refresh
        private void RefreshSubGenersListBox()
        {
            if (subGenrList != null)
            {
                subGenrList.ItemsSource = null;
                subGenrList.Items.Clear();
            }

            switch (_selectedGenre)
            {
                case Genre.Academic:
                    {
                        subGenrList.ItemsSource = Enum.GetValues(typeof(Academic));
                        break;
                    }
                case Genre.Fiction:
                    {
                        subGenrList.ItemsSource = Enum.GetValues(typeof(Fiction));
                        break;
                    }
                case Genre.NonFiction:
                    {
                        subGenrList.ItemsSource = Enum.GetValues(typeof(NonFiction));
                        break;
                    }
            }
        }

        #endregion

        #region Events
        // Search criteria - ID
        private void byIdRBtn_Checked(object sender, RoutedEventArgs e)
        {
            searchBookBtn.IsEnabled = false;
            searchJournBtn.IsEnabled = false;
            searchAllBtn.IsEnabled = true;
            inputTbx.PlaceholderText = "Enter ISBN / ISSN";
            _criteria = SearchCriteria.ISBN;
        }

        // Search criteria - Author
        private void byAuthorRBtn_Checked(object sender, RoutedEventArgs e)
        {
            searchBookBtn.IsEnabled = true;
            searchJournBtn.IsEnabled = false;
            searchAllBtn.IsEnabled = false;
            inputTbx.PlaceholderText = "Enter Author's Lastname";
            _criteria = SearchCriteria.Author;

        }

        // Search criteria - Title
        private void byTitleRBtn_Checked(object sender, RoutedEventArgs e)
        {
            searchBookBtn.IsEnabled = true;
            searchJournBtn.IsEnabled = true;
            searchAllBtn.IsEnabled = true;
            inputTbx.PlaceholderText = "Enter Title";
            _criteria = SearchCriteria.Title;

        }

        // Select Item
        private void searchResultsLBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectdItem = searchResultsLBx.SelectedItem as AbstractItem;
            ClearSelectedItemRepresentationTextBoxes();
            LoadSelectedItemDataToRepresentationTextBoxes();
            editBtn.IsEnabled = true;
            delBtn.IsEnabled = true;
        }

        // Select Main Genre
        private void mainGenreCombBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedGenre = mainGenreCombBox.SelectedItem as Enum;
            RefreshSubGenersListBox();
        }

        // Select Sub-Genre
        private void subGenrList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedGenre = subGenrList.SelectedItem as Enum;
        }

        // Search Books and Journals
        private void searchAllBtn_Click(object sender, RoutedEventArgs e)
        {
            _feedback = _library.Search(_criteria, inputTbx.Text);
            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByCurrentUser(_feedback.Data);

            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByGenre(_selectedGenre, _feedback.Data);

            RefreshListBoxSearchResults();
        }

        // Search Books
        private void searchBookBtn_Click(object sender, RoutedEventArgs e)
        {
            _feedback = _library.Search(_criteria, inputTbx.Text);
            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByCurrentUser(_feedback.Data);

            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else if (_criteria != SearchCriteria.Author)
                _feedback = Filter.ByType(ItemType.Book, _feedback.Data);

            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByGenre(_selectedGenre, _feedback.Data);

            RefreshListBoxSearchResults();

        }

        // Search Journal
        private void searchJournBtn_Click(object sender, RoutedEventArgs e)
        {
            _feedback = _library.Search(_criteria, inputTbx.Text);
            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByType(ItemType.Journal, _feedback.Data);


            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByGenre(_selectedGenre, _feedback.Data);

            if (!_feedback.IsWorking)
                RefreshListBoxSearchResults();
            else
                _feedback = Filter.ByCurrentUser(_feedback.Data);

            RefreshListBoxSearchResults();

        }

        // go to edit-item-page
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsersManager.GetInstanse.CurrentUser.Access == UserType.Reader)
                errorTBl.Text = "Readers can't edit items";
            else
            {
                this.Frame.Navigate(typeof(AddEditItem), _selectdItem);
            }
        }

        // when navigate and get new/edited item
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var temp = e.Parameter;
            AbstractItem item = temp as AbstractItem;
            if (item != null)
            {
                searchResultsLBx.SelectedItem = item;
            }
        }

        // Delete Item 
        private void delBtn_Click(object sender, RoutedEventArgs e)
        {
            // check if this user can delete item
            if (UsersManager.GetInstanse.CurrentUser.Access != UserType.Admin)
                errorTBl.Text = "Only administrator can delete items";
            else
            {
                Respone<AbstractItem> feedbackDel = _library.RemoveItem(((AbstractItem)searchResultsLBx.SelectedItem).ID);
                if (!feedbackDel.IsWorking)
                    errorTBl.Text = feedbackDel.Message;
                else
                    RefreshPage();
            }
        }

        #endregion

    }
}
