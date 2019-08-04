using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Interfaces
{
    public interface IBorrower<T>
    {
        bool IsCanBorrow { get; }
        bool IsCanBorrowAt(T data);
        bool BorrowAt(T data);
        bool ReturnAt(T data);
    }
}
