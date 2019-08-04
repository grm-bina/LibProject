using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Models
{
    // use for authors names & users names
    public struct HumanName : IComparable<HumanName>, IEquatable<HumanName>
    {
        public string FirstName;
        public string MiddleNameOrPatronymic;
        public string LastName;

        // use for sorting by last name
        public int CompareTo(HumanName other)
        {
            return String.Compare(LastName, other.LastName, true);
        }

        // use for check if this the same person ONLY BY LASTNAME
        public bool Equals(HumanName other)
        {
            return String.Compare(LastName, other.LastName, true)==0;
        }

        // to string
        public override string ToString()
        {
            return $"{FirstName} {LastName} {MiddleNameOrPatronymic}";
        }
    }


}
