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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LibProject.Client
{
    public sealed partial class UserCreator : UserControl
    {
        UsersManager _users;
        UserType _typeNewUser;
        Respone<User> _feedback;

        public UserCreator()
        {
            this.InitializeComponent();
            _users = UsersManager.GetInstanse;

            newUserTypeCbx.Items.Clear();
            newUserTypeCbx.ItemsSource = Enum.GetValues(typeof(UserType));
            newUserTypeCbx.SelectedIndex = 2;

            if (_users.CurrentUser.Access != UserType.Admin)
                newUserTypeCbx.IsEnabled = false;

        }

        #region Methods
        private void CreateUser()
        {
            _feedback = CreatorUser.Create(userIdTbx.Text, newLastNameTbx.Text, newFirstNameTbx.Text, newMidNameTbx.Text, newUserPassTbx.Text, _typeNewUser);
        }

        private bool AddTelNumber()
        {
            if (_feedback.Data != null)
            {
                int number;
                // if number not entred - continue add user
                if (String.IsNullOrWhiteSpace(newTelTbx.Text))
                    return true;
                // if number entred but not can be parsed - failed
                if (!int.TryParse(newTelTbx.Text, out number))
                {
                    _feedback.Data = null;
                    _feedback.IsWorking = false;
                    _feedback.Message = "For enter Tel.Number use digits only";
                    return false;
                }
                else
                {
                    _feedback.Data.TelNumber = number;
                    return true;
                }
            }
            else return false;
        }
        #endregion

        #region Events
        // Add
        private void addNewUser_Click(object sender, RoutedEventArgs e)
        {
            // creating
            CreateUser();
            if (!_feedback.IsWorking || !AddTelNumber())
                errorNewUser.Text = _feedback.Message;
            // add to list
            else
            {
                _feedback = _users.AddUser(_feedback.Data);
                if(!_feedback.IsWorking)
                    errorNewUser.Text = _feedback.Message;
                // refresh page
                else
                    ((Frame)Window.Current.Content).Navigate(typeof(AddUser));
            }
        }

        // Change user-type
        private void newUserTypeCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(newUserTypeCbx.SelectedItem is UserType)
                _typeNewUser = (UserType)newUserTypeCbx.SelectedItem;
        }
        #endregion
    }
}
