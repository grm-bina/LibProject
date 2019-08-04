using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Exceptions;
using LibProject.Interfaces;
using LibProject.Globals;

namespace LibProject.Models
{
    public class User : IBorrower<HardCopy>, IEquatable<User>
    {
        #region Data
        private HumanName _name;
        private List<HardCopy> _borrowedItems;
        private string _password;
        #endregion

        #region Ctor
        public User(int id, HumanName name, string password, UserType access = UserType.Reader)
        {
            Access = access;
            Id = id;
            Name = name;
            _password = password;
            _borrowedItems = new List<HardCopy>();
            if (String.IsNullOrWhiteSpace(password))
                throw new EmptyPasswordException();
            if (String.IsNullOrEmpty(name.LastName))
                throw new EmptyLastNameException();
        }
        #endregion

        #region Property
        public UserType Access { get; set; }
        public int TelNumber { get; set; }
        public int Id { get; }

        // get list of borrowed items
        public List<HardCopy> GetBorroewedItems()
        {
            if (IsKeepItems)
            {
                return _borrowedItems;
            }
            else
                return null;
        }

        // change name
        public HumanName Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value.LastName))
                    throw new EmptyLastNameException();
                else
                    _name = value;
            }
        }

        // check if keeps items more time than maximum
        public bool IsLateReturn
        {
            get
            {
                foreach(HardCopy item in _borrowedItems)
                {
                    TimeSpan keepsItem = DateTime.Now.Subtract(item.BorrowedSinse);
                    if (TimeSpan.Compare(keepsItem, Settings.MaxBorrowingTime) >= 0)
                        return true;
                }
                return false;
            }
        }

        // check if keeps any item
        public bool IsKeepItems
        {
            get
            {
                return _borrowedItems.Count > 0;
            }
        }

        #endregion

        #region Methods

        // primitive password-cheking without any hashing
        public bool CheckPassword(string password)
        {
            return String.Equals(_password, password);
        }

        // to string (return only name)
        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append(_name.LastName);
            if (!String.IsNullOrWhiteSpace(_name.FirstName))
                temp.Append($" {_name.FirstName}");
            if (!String.IsNullOrWhiteSpace(_name.MiddleNameOrPatronymic))
                temp.Append($" {_name.MiddleNameOrPatronymic}");

            return temp.ToString();
        }

        // equality by ID
        public bool Equals(User other)
        {
            return Id == other.Id;
        }

        #endregion

        #region IBorrower Interface

        // user can borrow items if he don't late return already boroowed items and if borrowed less than maximum
        public bool IsCanBorrow
        {
            get
            {
                return (_borrowedItems.Count < Settings.MaxBorrowedItemsPerUser) && !IsLateReturn;
            }
        }

        // borrow item
        public bool BorrowAt(HardCopy data)
        {
            if (IsCanBorrowAt(data))
            {
                _borrowedItems.Add(data);
                return true;
            }
            return false;
        }

        // check if user don't keeps copy of the same item 
        public bool IsCanBorrowAt(HardCopy data)
        {
            if (IsCanBorrow)
            {
                foreach(HardCopy item in _borrowedItems)
                {
                    if (item.ItemId == data.ItemId)
                        return false;
                }
                return true;
            }
            return false;
        }

        // return item
        public bool ReturnAt(HardCopy data)
        {
            return _borrowedItems.Remove(data);
        }

        #endregion
    }
}
