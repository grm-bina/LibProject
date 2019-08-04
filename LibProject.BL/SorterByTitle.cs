using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Models;

namespace LibProject.BL
{
    internal class SorterByTitle : IComparer<AbstractItem>
    {
        public int Compare(AbstractItem x, AbstractItem y)
        {
            return String.Compare(x.Title, y.Title, true);
        }
    }
}
