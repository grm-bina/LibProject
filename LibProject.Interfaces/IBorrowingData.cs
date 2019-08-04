using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Interfaces
{
    // data wich run between borrower and borrowable
    public interface IBorrowingData
    {
        string ItemDescription { get; set; }
        DateTime BorrowedSinse { get; set; }
    }
}
