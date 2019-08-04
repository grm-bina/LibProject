using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Exceptions;
using LibProject.Models;

namespace LibProject.BL
{
    public sealed class LibraryManager
    {
        #region Data
        private static LibraryManager _instance = null;
        private Dictionary<int, AbstractItem> _itemsById;
        private List<AbstractItem> _itemsByTitle;
        private UsersManager _users;
        private Dictionary<Guid, HardCopy> _borrowedHardCopies;

        #endregion

        #region Singleton Ctor
        private LibraryManager()
        {
            _itemsById = new Dictionary<int, AbstractItem>();
            _itemsByTitle = new List<AbstractItem>();
            _users = UsersManager.GetInstanse;
            _borrowedHardCopies = new Dictionary<Guid, HardCopy>();

            // hardcoded entered books
            Book a1 = new Book("Example book", 111, Genre.Fiction, 1) { Year = 1980, PlaceInLib = "AB12" };
            List<HumanName> authors = new List<HumanName>();
            authors.Add(new HumanName() { FirstName = "Vasya", LastName = "Pupkin" });
            a1.ReplaceAuthors(authors);
            Journal a2 = new Journal(5,"Example journal", 222, Genre.Fiction, 3) { Year = 1960, PlaceInLib = "CD34" };
            AddItem(a1);
            AddItem(a2);

        }

        public static LibraryManager GetInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new LibraryManager();
                return _instance;
            }
        }
        #endregion

        #region Sort

        // refreesh sorting by title - have use it when item added or was edited
        public void ReSort()
        {
            _itemsByTitle.Sort(new SorterByTitle());
        }

        #endregion

        #region Add / Remove Item
        // Add item (return copies-id of new-added item for save/print and use it later for identify each hard-copy)
        public Respone<IEnumerable<Guid>> AddItem(AbstractItem newItem)
        {
            Respone<IEnumerable<Guid>> feedback = new Respone<IEnumerable<Guid>>();

            // check if current user can add items
            if (_users.CurrentUser != null && _users.CurrentUser.Access==UserType.Reader)
            {
                feedback.IsWorking = false;
                feedback.Message = "Readers can't add new items";
                feedback.WrongDataType = WrongData.UserType;
                return feedback;
            }

            // this item is already exist
            AbstractItem temp;
            if (_itemsById.TryGetValue(newItem.ID,out temp))
            {
                feedback.IsWorking = false;
                feedback.Message = "Item is already exist";
                feedback.WrongDataType = WrongData.ItemId;
                return feedback;
            }
            // add book
            else
            {
                if (_itemsById.ContainsKey(newItem.ID))
                    _itemsById[newItem.ID] = newItem;
                else
                    _itemsById.Add(newItem.ID, newItem);
                _itemsByTitle.Add(newItem);
                ReSort();

                feedback.IsWorking = true;
                feedback.Data = newItem.AviableCopiesID;

                return feedback;
            }
        }

        // Remove item
        public Respone<AbstractItem> RemoveItem(int id)
        {
            Respone<AbstractItem> feedback = new Respone<AbstractItem>();

            // check if current user can remove items
            if (_users.CurrentUser != null && _users.CurrentUser.Access != UserType.Admin)
            {
                feedback.IsWorking = false;
                feedback.Message = "Only Administrator can remove items";
                feedback.WrongDataType = WrongData.UserType;
                return feedback;
            }

            // check if item exist
            AbstractItem temp;
            if(!_itemsById.TryGetValue(id, out temp))
            {
                feedback.IsWorking = false;
                feedback.Message = "Item not found";
                feedback.WrongDataType = WrongData.ItemId;
            }
            // check if item not borrowed
            else if (temp.IsBorrowed)
            {
                feedback.IsWorking = false;
                feedback.Message = "Can't remove borrowed item";
            }
            // remove
            else
            {
                if (_itemsByTitle.Remove(temp))
                {
                    _itemsById.Remove(id);

                    feedback.IsWorking = true;
                    feedback.Data = temp;
                }
                else
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Unpredicted Error";
                }
            }

            return feedback;
        }




        #endregion

        #region Search
        // search by id is exist
        public bool Exist(int itemId)
        {
            return _itemsById.ContainsKey(itemId);
        }

        // by Id - return only 1 item
        public AbstractItem GetItem(int itemId)
        {
            AbstractItem temp;
            if (_itemsById.TryGetValue(itemId, out temp))
                return temp;
            else
                return null;
        }

        // return copy of list sorted by publishing-year
        public List<AbstractItem> AllItems()
        {
            List<AbstractItem> temp = new List<AbstractItem>();
            temp.AddRange(_itemsByTitle);
            temp.Sort();
            return temp;
        }

        // Public method for search by criteria and input-string (return list)
        public Respone<List<AbstractItem>> Search(SearchCriteria criteria, string request)
        {
            switch (criteria)
            {
                case SearchCriteria.ISBN:
                    return SearchById(request);
                case SearchCriteria.Author:
                    return SearchByAuthor(request);
                case SearchCriteria.Title:
                    return SearchByTitle(request);
                default:
                    return new Respone<List<AbstractItem>>() { IsWorking = false };
            }
        }

        // private search-methods
        // by id
        private Respone<List<AbstractItem>> SearchById(string id)
        {
            Respone<List<AbstractItem>> feedback = new Respone<List<AbstractItem>>();
            int parsedId;

            // check if parsing
            if(!int.TryParse(id, out parsedId))
            {
                feedback.IsWorking = false;
                feedback.Message = "For enter ISBN / ISSN enter digits only";
                feedback.WrongDataType = WrongData.ItemId;
            }
            else
            {
                if (!Exist(parsedId))
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Nothing found";
                }
                else
                {
                    feedback.Data = new List<AbstractItem>();
                    feedback.Data.Add(GetItem(parsedId));
                    feedback.IsWorking = true;
                }
            }
            return feedback;
        }

        // search by author (return only Books)
        private Respone<List<AbstractItem>> SearchByAuthor(string lastName)
        {
            // filter only books (if not found books or request is empty - return all found book-list as is
            Respone<List<AbstractItem>> feedback = Filter.ByType(ItemType.Book, AllItems());
            if (!feedback.IsWorking || String.IsNullOrWhiteSpace(lastName))
                return feedback;
            // search authors in the book-list
            else
            {
                HumanName author = new HumanName() { LastName = lastName };
                List<AbstractItem> temp = new List<AbstractItem>();
                foreach(Book item in feedback.Data)
                {
                    if (item.GetAuthors().Contains(author))
                        temp.Add(item);
                }
                if (temp.Count == 0)
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Nothing found";
                    feedback.Data = null;
                }
                else
                {
                    feedback.IsWorking = true;
                    feedback.Data = temp;
                }
                return feedback;
            }
        }

        // search by title
        private Respone<List<AbstractItem>> SearchByTitle(string title)
        {
            Respone<List<AbstractItem>> feedback = new Respone<List<AbstractItem>>();

            // if list is empty - return false
            if (_itemsByTitle.Count == 0)
            {
                feedback.IsWorking = false;
                feedback.Message = "Library is Empty";
                return feedback;
            }
            // if request is empty - return all list
            else if (String.IsNullOrWhiteSpace(title))
            {
                feedback.IsWorking = true;
                feedback.Data = AllItems();
                return feedback;
            }
            // search title
            else
            {
                // create sample-item for find index of 1 item in list with the same title
                AbstractItem sample = new Book(title, 111, Genre.Fiction, 1);
                int index = _itemsByTitle.BinarySearch(sample, new SorterByTitle());
                // if exist - add item from list with this index, and check if exist items with the same title before and after
                if (index >= 0)
                {
                    List<AbstractItem> temp = new List<AbstractItem>();
                    temp.Add(_itemsByTitle[index]);

                    // after
                    for (int i = index+1; i < _itemsByTitle.Count-1; i++)
                    {
                        if (_itemsByTitle[i].Title == title)
                            temp.Add(_itemsByTitle[i]);
                        else break;
                    }
                    // before
                    for (int i = index - 1; i >= 0; i--)
                    {
                        if (String.Compare(_itemsByTitle[i].Title, title, true)==0)
                            temp.Add(_itemsByTitle[i]);
                        else break;
                    }

                    temp.Sort();
                    feedback.IsWorking = true;
                    feedback.Data = temp;
                }
                else
                {
                    feedback.IsWorking = false;
                    feedback.Message = "Nothing found";
                }

                return feedback;
            }
        }


        #endregion

        #region Borrow / Return
        // check data and search item and user
        private Respone<string> CheckAndSearchItemAndUser(HardCopy data, out User user, out AbstractItem item)
        {
            Respone<string> feedback = new Respone<string>();
            user = null;
            item = null;

            // check if current user can do it (reader can't borrow by himself)
            if (_users.CurrentUser != null && _users.CurrentUser.Access == UserType.Reader)
            {
                feedback.IsWorking = false;
                feedback.Message = "For borrow or return item, please ask librarian for help";
                feedback.WrongDataType = WrongData.UserType;
                return feedback;
            }

            // check if all data exist in input
            if (data.ItemId == null || data.CopyId == null || data.BorrowerId == null)
            {
                feedback.IsWorking = false;
                feedback.Message = "Missing or invalid data";

                if (data.ItemId == null)
                    feedback.WrongDataType = WrongData.ItemId;
                else if (data.CopyId == null)
                    feedback.WrongDataType = WrongData.CopyId;
                else
                    feedback.WrongDataType = WrongData.UserId;

                return feedback;
            }

            // try find item and user
            user = _users.GetUser(data.BorrowerId);
            if (user == null || !_itemsById.TryGetValue(data.ItemId, out item))
            {
                feedback.IsWorking = false;

                if (user == null)
                {
                    feedback.Message = "User not found";
                    feedback.WrongDataType = WrongData.UserId;
                }
                else
                {
                    feedback.Message = "Item not found";
                    feedback.WrongDataType = WrongData.ItemId;
                }

                return feedback;
            }
            else
            {
                feedback.IsWorking = true;
                return feedback;
            }
        }

        // Borrow (return string with message (user + item) if it's working)
        public Respone<string> Borrow(HardCopy data)
        {
            User user;
            AbstractItem item;
            Respone<string> feedback = CheckAndSearchItemAndUser(data, out user, out item);
            if (!feedback.IsWorking)
                return feedback;
            else
            {
                Respone<HardCopy> tempFeedback = BorrowReturnOperations<HardCopy>.Borrow(data, item, user);

                if (!tempFeedback.IsWorking)
                {
                    feedback.IsWorking = false;
                    feedback.Message = tempFeedback.Message;
                }
                else
                {
                    // add hard-copy data to borrowed-dictionary
                    if (_borrowedHardCopies.ContainsKey(tempFeedback.Data.CopyId))
                        _borrowedHardCopies[tempFeedback.Data.CopyId] = tempFeedback.Data;
                    else
                        _borrowedHardCopies.Add(tempFeedback.Data.CopyId, tempFeedback.Data);

                    feedback.Data = tempFeedback.Message;
                }

                return feedback;
            }
        }

        // Borrow (return string with message (user + item) if it's working)
        public Respone<string> Return(HardCopy data)
        {
            Respone<string> feedback;
            // find user id in borrowed items
            HardCopy tempdata;
            if (!_borrowedHardCopies.TryGetValue(data.CopyId, out tempdata))
            {
                feedback = new Respone<string>();
                feedback.IsWorking = false;
                feedback.Message = "Missing or invalid data";
                feedback.WrongDataType = WrongData.CopyId;
                return feedback;
            }
            else
                data.BorrowerId = tempdata.BorrowerId;

            User user;
            AbstractItem item;
            feedback = CheckAndSearchItemAndUser(data, out user, out item);
            if (!feedback.IsWorking)
                return feedback;
            else
            {
                Respone<HardCopy> tempFeedback = BorrowReturnOperations<HardCopy>.Return(data, item, user);

                if (!tempFeedback.IsWorking)
                {
                    feedback.IsWorking = false;
                    feedback.Message = tempFeedback.Message;
                }
                else
                {
                    // remove hard-copy data from borrowed-dictionary
                    _borrowedHardCopies.Remove(tempFeedback.Data.CopyId);
                    feedback.Data = tempFeedback.Message;
                }

                return feedback;
            }
        }

        #endregion
    }
}
