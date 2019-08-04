using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Exceptions;


namespace LibProject.Models
{
    public class Journal: AbstractItem
    {
        #region Data
        private int _volume;
        #endregion

        #region Ctor
        public Journal(int volume, string title, int issn, Genre genre, int copies) : base(title, issn, genre, copies)
        {
            Volume = volume;
        }
        #endregion

        #region Prop
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value > 0)
                    _volume = value;
                else
                    throw new InvalidVolumException();
            }
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append($"\"{this.Title}\" Vol. {Volume}");
            if (this.EditionOrIssue != null)
                temp.Append($" Issue {this.EditionOrIssue}");
            if (this.Year != null)
                temp.Append($" ({this.Year})");

            return temp.ToString();
        }

        // method for create backup and restore from backup (for using in Editor) - check type for validation
        public override bool RestoreFromBackup(AbstractItem backup)
        {
            Journal backupJournal = backup as Journal;
            if (backupJournal != null)
            {
                if (base.RestoreFromBackup(backup))
                {
                    _volume = backupJournal.Volume;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
