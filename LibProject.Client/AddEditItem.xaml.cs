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
using LibProject.Exceptions;
using LibProject.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibProject.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEditItem : Page
    {
        AddEditMode _mode;
        int _currentItemId;
        List<Enum> _subGeners;
        Genre _mainGenre;
        private LibraryManager _library;
        private AbstractItem _item;
        ItemType _type;
        List<HumanName> _authors;
        Respone<AbstractItem> _feedback;


        public AddEditItem()
        {
            this.InitializeComponent();
            _library = LibraryManager.GetInstance;

            _type = ItemType.NotSelected;

            mainGenreCombBox.Items.Clear();
            mainGenreCombBox.ItemsSource = Enum.GetValues(typeof(Genre));
            mainGenreCombBox.SelectedIndex = 0;


        }

        #region Methods
        // Load data to page
        private void LoadData()
        {
            idTbl.Text = $"ISBN / ISSN: {_currentItemId}";

            if (_mode == AddEditMode.Edit)
            {
                // abstract-fields
                titleTbx.Text = _item.Title;
                if (_item.Year != null)
                    yearTbx.Text = _item.Year.ToString();
                if (_item.EditionOrIssue != null)
                    editionTbx.Text = _item.EditionOrIssue.ToString();
                numCopTbx.Text = _item.TotalNumCopies.ToString();
                if (_item.Publisher != null)
                    publisherTbx.Text = _item.Publisher;
                if (_item.PlaceInLib != null)
                    placeTbx.Text = _item.PlaceInLib;
                if (_item.Annotation != null)
                    annotTbx.Text = _item.Annotation;

                // book fields
                Book tempBook = _item as Book;
                if (tempBook != null)
                {
                    bookTypeRbt.IsChecked = true;
                    if(tempBook.GetAuthors() != null && tempBook.GetAuthors().Count > 0)
                    {
                        var tempAuthors = tempBook.GetAuthors();
                        foreach (HumanName author in tempAuthors)
                            authListBx.Items.Add(author);
                    }
                }

                // journal fields
                Journal tempJornal = _item as Journal;
                if (tempJornal != null)
                {
                    JournalTypeRbt.IsChecked = true;
                    volTbx.Text = tempJornal.Volume.ToString();
                }
            }

        }

        // Get SubGeners
        private void GetSubGeners()
        {
            _subGeners = new List<Enum>();
            if(_mainGenre!= Genre.Unknown)
            {
                foreach(CheckBox item in subGenresStack.Children)
                {
                    if (item.IsChecked == true)
                        _subGeners.Add((Enum)item.Content);
                }
            }
        }

        // Sub-Geners Refresh
        private void RefreshSubGenersListBox()
        {
            subGenresStack.Children.Clear();

            if (_mainGenre != Genre.Unknown)
            {
                var temp = _mainGenre.GetSubgeners();
                foreach (Enum item in temp)
                {
                    subGenresStack.Children.Add(new CheckBox() { Content = item });
                }
            }
        }

        // Get authors-list from list-box
        private void GetAuthors()
        {
            _authors = new List<HumanName>();
            if(authListBx.Items!=null && authListBx.Items.Count > 0)
            {
                foreach (var author in authListBx.Items)
                    _authors.Add((HumanName)author);
            }
        }
        #endregion


        #region Events
        // deside mode when page loaded
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var temp = e.Parameter;
            if(temp is int)
            {
                _mode = AddEditMode.Add;
                _currentItemId = (int)temp;
                curentPageTxt.Text = "Add Book";
                addItemkBtn.Content = "Add Item";
            }
            else if(temp is AbstractItem)
            {
                _mode = AddEditMode.Edit;
                _item = (AbstractItem)temp;
                _currentItemId = _item.ID;
                curentPageTxt.Text = "Edit Book";
                mainGenreCombBox.IsEnabled = false;
                bookTypeRbt.IsEnabled = false;
                JournalTypeRbt.IsEnabled = false;
                numCopTbx.IsEnabled = false;
                addItemkBtn.Content = "Save";
                mainGenreCombBox.SelectedItem = _item.Category;
            }

            LoadData();

        }

        // Select Main Genre
        private void mainGenreCombBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(mainGenreCombBox.SelectedItem is Genre)
            {
                _mainGenre = (Genre)mainGenreCombBox.SelectedItem;
                RefreshSubGenersListBox();

                // load subgeners if this edit-mode (this part of code work only here and fix for check-boxes that not checked when loaded data from item)
                if (_mode == AddEditMode.Edit && _item.GetSubgeneres() != null && _item.GetSubgeneres().Count > 0)
                {
                    foreach (CheckBox item in subGenresStack.Children)
                    {
                        if (_item.GetSubgeneres().Contains(item.Content))
                            item.IsChecked = true;
                    }
                }
            }
        }

        // Add Book - selected
        private void bookTypeRbt_Checked(object sender, RoutedEventArgs e)
        {
            _type = ItemType.Book;
            volTbx.Text = "";
            volTbx.IsEnabled = false;
            lastNameTbx.IsEnabled = true;
            firstNameTbx.IsEnabled = true;
            midNameTbx.IsEnabled = true;
            addAuthorBtn.IsEnabled = true;
            delAuthor.IsEnabled = false;
            authListBx.Items.Clear();
        }

        // Add Journal - selected
        private void JournalTypeRbt_Checked(object sender, RoutedEventArgs e)
        {
            _type = ItemType.Journal;
            volTbx.Text = "";
            volTbx.IsEnabled = true;
            lastNameTbx.IsEnabled = false;
            firstNameTbx.IsEnabled = false;
            midNameTbx.IsEnabled = false;
            addAuthorBtn.IsEnabled = false;
            delAuthor.IsEnabled = false;
            authListBx.Items.Clear();
        }

        // add author to list-box
        private void addAuthorBtn_Click(object sender, RoutedEventArgs e)
        {
            HumanName author = new HumanName();
            author.LastName = lastNameTbx.Text;
            author.FirstName = firstNameTbx.Text;
            author.MiddleNameOrPatronymic = midNameTbx.Text;
            authListBx.Items.Add(author);
            lastNameTbx.Text = "";
            firstNameTbx.Text = "";
            midNameTbx.Text = "";
        }

        // when selected something in authors-list
        private void authListBx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            delAuthor.IsEnabled = true;
        }

        // delete selected author
        private void delAuthor_Click(object sender, RoutedEventArgs e)
        {
            authListBx.Items.Remove(authListBx.SelectedItem);
        }


        #endregion

        // add or edit item button
        private void addItemkBtn_Click(object sender, RoutedEventArgs e)
        {
            // get data from check-boxes and autors-listbox
            GetAuthors();
            GetSubGeners();

            // create new item
            if(_mode == AddEditMode.Add)
            {
                _feedback = ItemFactoryAndEditor.TryCreateItem(_type, titleTbx.Text, _currentItemId, _mainGenre, numCopTbx.Text, volTbx.Text);
                if (_feedback.IsWorking)
                    _item = _feedback.Data;
            }
            // edit item or add to created item fields than not in constructor
            if(_mode == AddEditMode.Edit || _feedback.IsWorking)
            {
                _feedback = ItemFactoryAndEditor.TryEdit(_item, titleTbx.Text, publisherTbx.Text, annotTbx.Text, placeTbx.Text, yearTbx.Text, editionTbx.Text, _subGeners, _authors, volTbx.Text);
                if (_feedback.IsWorking)
                    _item = _feedback.Data;
            }
            // add new item to library
            if(_feedback.IsWorking && _mode == AddEditMode.Add)
            {
                Respone<IEnumerable<Guid>> tempFeedback = _library.AddItem(_item);
                if (!tempFeedback.IsWorking)
                    errorTbx.Text = tempFeedback.Message;
                else
                    this.Frame.Navigate(typeof(SearchBook), _item);
            }
            // go to library if item successfully edited
            if(_feedback.IsWorking && _mode == AddEditMode.Edit)
                this.Frame.Navigate(typeof(SearchBook), _item);

            if (!_feedback.IsWorking)
                errorTbx.Text = _feedback.Message;

        }

        // go back
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
