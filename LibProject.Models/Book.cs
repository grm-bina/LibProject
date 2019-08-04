using LibProject.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Models
{
    public class Book : AbstractItem
    {
        #region Data
        private List<HumanName> _authors;
        #endregion

        #region Ctor
        public Book(string title, int isbn, Genre genre, int copies) : base(title, isbn, genre, copies) { }
        #endregion

        #region Methods
        // Get copy of authors-list
        public List<HumanName> GetAuthors()
        {
            if (_authors != null)
            {
                List<HumanName> temp = new List<HumanName>();
                temp.AddRange(_authors);
                return temp;
            }
            else return null;
        }

        // replace or delete authors-list
        public void ReplaceAuthors(List<HumanName> newList)
        {
            // delete current authors-list
            if (newList == null || newList.Count == 0)
                _authors = null;
            // replace
            else
            {
                List<HumanName> temp = new List<HumanName>();
                // check if it not empty (if all strings in HumanName is empty - just go to next, if only Lasname is empty but exist first or middle-name - break the method and throw exception)
                foreach (HumanName name in newList)
                {
                    if (!String.IsNullOrWhiteSpace(name.LastName))
                        temp.Add(name);
                    else if (!String.IsNullOrWhiteSpace(name.MiddleNameOrPatronymic) || !String.IsNullOrWhiteSpace(name.FirstName))
                        throw new EmptyLastNameException();
                }
                _authors = temp;
                // sort authors by last name
                _authors.Sort();
            }
        }

        // short representation
        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            if (_authors != null)
            {
                for (int i = 0; i < _authors.Count; i++)
                {
                    temp.Append(_authors[i].LastName);

                    if (!String.IsNullOrWhiteSpace(_authors[i].FirstName) || !String.IsNullOrWhiteSpace(_authors[i].MiddleNameOrPatronymic))
                    {
                        temp.Append(" ");
                        if (!String.IsNullOrWhiteSpace(_authors[i].FirstName))
                            temp.Append($"{_authors[i].FirstName[0]}.");
                        if (!String.IsNullOrWhiteSpace(_authors[i].MiddleNameOrPatronymic))
                            temp.Append($"{_authors[i].MiddleNameOrPatronymic[0]}.");
                    }
                    if (i != _authors.Count-1)
                        temp.Append(", ");
                }
                if (this.Year != null)
                    temp.Append($"({this.Year})");
                temp.Append($" \"{this.Title}\"");
                if (this.EditionOrIssue != null)
                    temp.Append($" Edition {this.EditionOrIssue}");
            }
            else
            {
                temp.Append($"\"{this.Title}\"");
                if (this.EditionOrIssue != null)
                    temp.Append($" Edition {this.EditionOrIssue}");
                if (this.Year != null)
                    temp.Append($" ({this.Year})");
            }

            return temp.ToString();
        }

        // method for create backup and restore from backup (for using in Editor) - check type for validation
        public override bool RestoreFromBackup(AbstractItem backup)
        {
            Book backupBook = backup as Book;
            if (backupBook != null)
            {
                if (base.RestoreFromBackup(backup))
                {
                    _authors = backupBook.GetAuthors();
                    return true;
                }
            }
            return false;
        }
        #endregion

    }
}
