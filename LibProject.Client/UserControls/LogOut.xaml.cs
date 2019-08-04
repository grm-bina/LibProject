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
    public sealed partial class LogOut : UserControl
    {
        public LogOut()
        {
            this.InitializeComponent();

            currentUserTb.Text = UsersManager.GetInstanse.CurrentUser.ToString();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            UsersManager.GetInstanse.LogOut();
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));

        }
    }
}
