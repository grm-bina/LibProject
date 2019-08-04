using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Interfaces
{
    // implement it in Items that user can barrow
    public interface IBorrowable<T>
    {
        bool BorrowMe(T data);
        bool ReturnMe(T data);
        bool IsAvailable { get; }
    }

}
