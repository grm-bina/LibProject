using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Models;
using LibProject.Exceptions;

namespace LibProject.BL
{
    public sealed class UsersManager
    {
        #region Data
        private Dictionary<int, User> _allUsers;
        public User CurrentUser { get; private set; }
        private static UsersManager _instance = null;
        #endregion

        #region Singleton Ctor
        private UsersManager()
        {
            _allUsers = new Dictionary<int, User>();
            CurrentUser = null;

            // Hard-coded new user-administrator
            User me = new User(945, new HumanName() { FirstName = "Albina", LastName = "Gromov" }, "1", UserType.Admin);
            AddUser(me);
        }

        public static UsersManager GetInstanse
        {
            get
            {
                if (_instance == null)
                    _instance = new UsersManager();
                return _instance;
            }
        }
        #endregion

        #region LogIn-LogOut
        // check password and update current-user
        public Respone<User> LogIn(string id, string password)
        {
            Respone<User> feedback = new Respone<User>();

            int idParsed;
            if(!int.TryParse(id, out idParsed))
            {
                feedback.IsWorking = false;
                feedback.Message = "For enter ID use digits only";
                feedback.WrongDataType = WrongData.UserId;
                return feedback;
            }

            User temp;
            if (!_allUsers.TryGetValue(idParsed, out temp))
            {
                feedback.IsWorking = false;
                feedback.Message = "User not found";
                feedback.WrongDataType = WrongData.UserId;
                return feedback;
            }
            else
            {
                if (!temp.CheckPassword(password))
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Wrong Password";
                    feedback.WrongDataType = WrongData.Password;
                }
                else
                {
                    CurrentUser = temp;
                    feedback.Data = temp;
                    feedback.IsWorking = true;
                }
            }

            return feedback;
        }

        // set current user as null
        public void LogOut()
        {
            CurrentUser = null;
        }
        #endregion

        #region User Search
        // check if user is exist in list
        public bool Exist(int userId)
        {
            User temp;
            if (_allUsers.TryGetValue(userId, out temp))
                return true;
            else return false;
        }

        // get user by id
        public User GetUser(int userId)
        {
            User temp;
            if (_allUsers.TryGetValue(userId, out temp))
                return temp;
            else return null;
        }
        #endregion

        #region Add / Remove
        // add new user (with checking if current user can do it)
        public Respone<User> AddUser(User newUser)
        {
            Respone<User> feedback = new Respone<User>();

            if (CurrentUser != null)
            {
                // check if current user can add users or this type of users
                switch (CurrentUser.Access)
                {
                    case UserType.Reader:
                        {
                            feedback.IsWorking = false;
                            feedback.Message = "Reader can't add new users";
                            break;
                        }
                    case UserType.Librarian:
                        {
                            if (newUser.Access == UserType.Reader)
                                feedback.IsWorking = true;
                            else
                            {
                                feedback.IsWorking = false;
                                feedback.Message = "Librarian can add only Readers";
                                feedback.WrongDataType = WrongData.UserType;
                            }
                            break;
                        }
                    case UserType.Admin:
                        {
                            feedback.IsWorking = true;
                            break;
                        }
                }

                if (feedback.IsWorking == false)
                    return feedback;
            }

            // try add
            if (!Exist(newUser.Id))
            {
                if (_allUsers.ContainsKey(newUser.Id))
                    _allUsers[newUser.Id] = newUser;
                else
                    _allUsers.Add(newUser.Id, newUser);
                feedback.IsWorking = true;
                feedback.Data = newUser;
            }
            else
            {
                feedback.IsWorking = false;
                feedback.Message = $"Can't add this user\n(already exist: {GetUser(newUser.Id)})";
                feedback.WrongDataType = WrongData.UserId;
            }
            return feedback;
        }

        // remove user (with checking if current user can do it and if removing-user don't keep items and can be removed)
        public Respone<User> RemoveUser(int id)
        {
            Respone<User> feedback = new Respone<User>();
            User temp;

            // check if user exist and don't keep items
            if (!Exist(id))
            {
                feedback.IsWorking = false;
                feedback.Message = "User not found";
                feedback.WrongDataType = WrongData.UserId;
                return feedback;
            }
            else if (GetUser(id).IsKeepItems)
            {
                feedback.IsWorking = false;
                feedback.Message = "Can't remove user with borrowed items";
                return feedback;
            }
            else
            {
                temp = GetUser(id);
            }

            // check if current user can remove
            if (CurrentUser != null)
            {
                switch (CurrentUser.Access)
                {
                    case UserType.Reader:
                        {
                            feedback.IsWorking = false;
                            feedback.Message = "Reader can't remove users";
                            break;
                        }
                    case UserType.Librarian:
                        {
                            if (temp.Access == UserType.Reader)
                                feedback.IsWorking = true;
                            else
                            {
                                feedback.IsWorking = false;
                                feedback.Message = "Error: Librarian can remove only Readers";
                            }
                            break;
                        }
                    case UserType.Admin:
                        {
                            if (temp.Equals(CurrentUser))
                            {
                                feedback.IsWorking = false;
                                feedback.Message = "You can't remove yourself";
                            }
                            else
                                feedback.IsWorking = true;
                            break;
                        }
                }

                if (feedback.IsWorking == false)
                    return feedback;
            }

            // remove
            feedback.IsWorking = _allUsers.Remove(id);
            feedback.Data = temp;
            return feedback;
        }


        #endregion


    }
}
