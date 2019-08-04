using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Exceptions;

namespace LibProject.Globals
{
    public static class Settings
    {
        // get-properties & default-value
        public static int MaxBorrowedItemsPerUser { get; private set; } = 5;

        public static TimeSpan MaxBorrowingTime { get; private set; } = new TimeSpan(7, 0, 0, 0);

        // methods for change settings
        public static void SetMaxBorrowedItemsPerUser(int max)
        {
            if (max > 0)
                MaxBorrowedItemsPerUser = max;
            else
                throw new InvalidMaxBorrowedItemsPerUserSettings();
        }

        public static void SetMaxBorrowingTime(int days)
        {
            if (days > 0)
                MaxBorrowingTime = new TimeSpan(days, 0, 0, 0);
            else
                throw new InvalidMaxBorrowingTimeSettings();
        }
    }
}
