using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Interfaces;

namespace LibProject.Models
{
    // struct-type for borrow&return
    public struct HardCopy : IBorrowingData, IEquatable<HardCopy>
    {
        // get from input
        public int ItemId { get; set; } 
        public Guid CopyId { get; set; }
        public int BorrowerId { get; set; }

        // update when borrowed
        public string ItemDescription { get; set; }
        public DateTime BorrowedSinse { get; set; }

        // equality by copy's id
        public bool Equals(HardCopy other)
        {
            return CopyId.Equals(other.CopyId);
        }

        // to string
        public override string ToString()
        {
            return ItemDescription;
        }
    }
}
