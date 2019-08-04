using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibProject.Exceptions
{
    public class InvalidItemIdException : Exception
    {
        public InvalidItemIdException(string message) : base(message) { }
    }

    public class EmptyTitleException : Exception
    {
        public EmptyTitleException() { }
        public override string Message
        {
            get
            {
                return "The Title CAN'T be empty";
            }
        }
    }

    public class InvalidPublishingDateException : Exception
    {
        public InvalidPublishingDateException(string message) : base(message) { }
}

    public class InvalidGenersException : Exception
    {
        public InvalidGenersException(string message) : base(message) { }
    }

    public class InvalidNumCopiesException : Exception
    {
        public InvalidNumCopiesException(string message) : base(message) { }
    }

    public class InvalidEditionOrIssueException : Exception
    {
        public InvalidEditionOrIssueException() { }
        public override string Message
        {
            get
            {
                return "Edition or Issue must be great than 0 or empty";
            }
        }
    }

    public class EmptyLastNameException : Exception
    {
        public EmptyLastNameException() { }
        public override string Message
        {
            get
            {
                return "Error: Empty Last Name";
            }
        }
    }

    public class InvalidVolumException : Exception
    {
        public InvalidVolumException() { }
        public override string Message
        {
            get
            {
                return "Error: Journal must have a number of Volume great than 0";
            }
        }
    }

    public class EmptyPasswordException: Exception
    {
        public EmptyPasswordException() { }
        public override string Message
        {
            get
            {
                return "Error: password can't be empty";
            }
        }
    }

    public class InvalidMaxBorrowedItemsPerUserSettings : Exception
    {
        public InvalidMaxBorrowedItemsPerUserSettings() { }
        public override string Message
        {
            get
            {
                return "Maximum number of borrowed items per user must be great than 0";
            }
        }

    }

    public class InvalidMaxBorrowingTimeSettings : Exception
    {
        public InvalidMaxBorrowingTimeSettings() { }
        public override string Message
        {
            get
            {
                return "Maximum time of borrowing must be at least 1 day";
            }
        }

    }



}
