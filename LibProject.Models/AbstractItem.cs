using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Interfaces;
using LibProject.Exceptions;


namespace LibProject.Models
{
    // abstract base-class for books & journals

    public abstract class AbstractItem : IBorrowable<HardCopy>, IComparable<AbstractItem>, IEquatable<AbstractItem>

    {
        #region Data
        private string _title;
        private int? _year;
        private List<Guid> _copiesID;
        private List<Enum> _subgeners;
        private int? _editionOrIssue;

        #endregion

        #region Ctor
        public AbstractItem(string title, int id, Genre mainGenre, int copies)
        {
            ID = id;
            if (String.IsNullOrWhiteSpace(title))
                throw new EmptyTitleException();
            _title = title;
            if (mainGenre == Genre.Unknown)
                throw new InvalidGenersException("Invalid Genre (the main category must be selected)");
            Category = mainGenre;
            if (copies <= 0)
                throw new InvalidNumCopiesException("Invalid Number of Copies (must be positive)");
            TotalNumCopies = 0;
            _copiesID = new List<Guid>();
            AddCopies(copies);
        }
        #endregion

        #region Prop

        public int ID { get; } // ISBN for books or ISSN for journals
        public Genre Category { get; } // can't be changed
        public int TotalNumCopies { get; private set; } // total num of hard-copies(borrowed-copies + avaiable copies)
        public string Publisher { get; set; } // where/by whom the book was printed
        public string Annotation { get; set; } // what is the book about
        public string PlaceInLib { get; set; }  // fisical place in the library

        // the name of the book
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new EmptyTitleException();
                _title = value;
            }
        }

        // when book was published
        public int? Year
        {
            get
            {
                return _year;
            }
            set
            {
                if (value < 0)
                    throw new InvalidPublishingDateException("Invalid Publishing Year (we don't keep so old goods)");
                if (value > DateTime.Today.Year)
                    throw new InvalidPublishingDateException("Invalid Publishing Year (bringing stuff from the future may break the Space-Time Continuum)");
                _year = value;
            }
        }

        // Edition Number
        public int? EditionOrIssue
        {
            get
            {
                return _editionOrIssue;
            }
            set
            {
                if (value > 0)
                    _editionOrIssue = value;
                else throw new InvalidEditionOrIssueException();
            }
        }

        // num of book's copies avaible in the library (NOT total number of copies belong to library)
        public int AvaibleCopies
        {
            get
            {
                return _copiesID.Count;
            }
        }


        // for get ID when instance created & if need to find/check/delete unfinding copy
        public IEnumerable<Guid> AviableCopiesID
        {
            get
            {
                return _copiesID.ToArray();
            }
        }

        // if 1 or more copies is borrowed
        public bool IsBorrowed
        {
            get
            {
                if (TotalNumCopies > AvaibleCopies)
                    return true;
                else if (TotalNumCopies == AvaibleCopies)
                    return false;
                else throw new InvalidNumCopiesException("Error: total number of copies less than number of avaible copies");
            }
        }


        #endregion

        #region IBorrowable-Implementation

        // users can borrow book if its 1 or more copies in the library
        public bool IsAvailable
        {
            get
            {
                if (AvaibleCopies > 0)
                    return true;
                else return false;
            }
        }

        // take book
        public bool BorrowMe(HardCopy hardCopy)
        {
            if (IsAvailable)
            {
                if ((hardCopy.ItemId != this.ID) || !_copiesID.Remove(hardCopy.CopyId))
                    throw new InvalidItemIdException("Invalid Item-ID or Hard-Copy-ID: don't match");
                else return true;
            }
            else return false;
        }

        // return book
        public bool ReturnMe(HardCopy hardCopy)
        {
            if (hardCopy.ItemId != this.ID)
                throw new InvalidItemIdException("Invalid ID: don't match");

            if (IsBorrowed && !_copiesID.Contains(hardCopy.CopyId))
            {
                _copiesID.Add(hardCopy.CopyId);
                return true;
            }
            else return false;
        }
        #endregion

        #region Methods
        // Get Copy of Subgeners-List
        public List<Enum> GetSubgeneres()
        {
            if (_subgeners != null)
            {
                List<Enum> temp = new List<Enum>();
                temp.AddRange(_subgeners);
                return temp;
            }
            else return null;
        }

        // Replace Subgeners-List by new subgeners
        public void ReplaceSubGeneres(List<Enum> newList)
        {
            // delete current subgeners
            if (newList == null)
                _subgeners = null;
            // try replace subjeners (if they dont belong to book category - dont changing subgeners)
            else
            {
                List<Enum> temp = new List<Enum>();
                List<Enum> validation = Category.GetSubgeners();
                foreach (Enum item in newList)
                {
                    if (validation.Contains(item))
                        temp.Add(item);
                    else throw new InvalidGenersException("Error: subgeners don't belong to main-geners category");
                }
                temp.Sort();
                _subgeners = temp;
            }
        }

        // default-sorting-criteria: YEAR (newest first)
        public int CompareTo(AbstractItem other)
        {
            if (Year < other.Year)
                return 1;
            else if (Year > other.Year)
                return -1;
            else
                return 0;
        }

        // add copy/s (return only new-created id)
        public List<Guid> AddCopies(int numOfcopies)
        {
            if (numOfcopies > 0)
            {
                List<Guid> newCopies = new List<Guid>();
                for (int i = 0; i < numOfcopies; i++)
                {
                    Guid copyID = Guid.NewGuid();
                    _copiesID.Add(copyID);  // add guids to list of this item
                    newCopies.Add(copyID);  // new guids that will be returned
                }
                TotalNumCopies += numOfcopies;
                return newCopies;
            }
            else return null;
        }

        // delete single copy
        public bool RemoveCopy(HardCopy id)
        {
            if (id.ItemId != this.ID)
                throw new InvalidItemIdException("Invalid ID: don't match");

            if (_copiesID.Remove(id.CopyId))
            {
                _copiesID.Remove(id.CopyId); // delete from list if it not borrowed
                TotalNumCopies--;
                return true;
            }
            else return false;
        }

        // items is equals if has the same ID (ISBN / ISSN)
        public bool Equals(AbstractItem other)
        {
            return ID == other.ID;
        }

        // method for create backup and restore from backup (for using in Editor) - use id & genre for validation
        public virtual bool RestoreFromBackup(AbstractItem backup)
        {
            if (backup.ID != this.ID || backup.Category != this.Category)
                return false;
            else
            {
                TotalNumCopies = backup.TotalNumCopies;
                Publisher = backup.Publisher;
                Annotation = backup.Annotation;
                PlaceInLib = backup.Annotation;
                _title = backup.Title;
                _year = backup.Year;
                _editionOrIssue = backup.EditionOrIssue;
                _copiesID.Clear();
                _copiesID.AddRange(backup.AviableCopiesID);
                _subgeners = backup.GetSubgeneres();

                return true;
            }
        }  

        #endregion

        #region Abstract Methods


        // short representation
        public abstract override string ToString();


        #endregion
    }
}
