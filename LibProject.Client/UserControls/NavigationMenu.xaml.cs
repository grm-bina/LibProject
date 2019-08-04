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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LibProject.Client
{

    public sealed partial class NavigationMenu : UserControl
    {
        User _currentUser;

        public NavigationMenu()
        {
            this.InitializeComponent();
            _currentUser = UsersManager.GetInstanse.CurrentUser;

            // show/hide buttons for different users
            switch (_currentUser.Access)
            {
                case UserType.Admin:
                    searchBookBtn.Visibility = Visibility.Visible;
                    myBookBtn.Visibility = Visibility.Visible;
                    borrowBtn.Visibility = Visibility.Visible;
                    addBookBtn.Visibility = Visibility.Visible;
                    addUserBtn.Visibility = Visibility.Visible;
                    setBtn.Visibility = Visibility.Visible;
                    break;
                case UserType.Librarian:
                    searchBookBtn.Visibility = Visibility.Visible;
                    myBookBtn.Visibility = Visibility.Visible;
                    borrowBtn.Visibility = Visibility.Visible;
                    addBookBtn.Visibility = Visibility.Visible;
                    addUserBtn.Visibility = Visibility.Visible;
                    setBtn.Visibility = Visibility.Collapsed;
                    break;
                case UserType.Reader:
                    searchBookBtn.Visibility = Visibility.Visible;
                    myBookBtn.Visibility = Visibility.Visible;
                    borrowBtn.Visibility = Visibility.Collapsed;
                    addBookBtn.Visibility = Visibility.Collapsed;
                    addUserBtn.Visibility = Visibility.Collapsed;
                    setBtn.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void searchBookBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(SearchBook));
        }

        private void myBookBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MyBooks));
        }

        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Borrow));
        }

        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AddBook));
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AddUser));

        }

        private void setBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Settings));

        }
    }
}
