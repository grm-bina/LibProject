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
using LibProject.Models;
using LibProject.BL;
using LibProject.Exceptions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LibProject.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();


        }

        private void logInBtn_Click(object sender, RoutedEventArgs e)
        {
           Respone<User> respone = UsersManager.GetInstanse.LogIn(idTbx.Text, passTbx.Text);
            if(respone.IsWorking)
                ((Frame)Window.Current.Content).Navigate(typeof(SearchBook));
            else
                errorTb.Text = respone.Message;
        }
    }
}
