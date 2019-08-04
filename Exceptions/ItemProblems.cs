using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class InvalidItemIdException : Exception
    {
        public InvalidItemIdException(string message) : base(message) { }
        public override string Message
        {
            get
            {
                return "Invalid ID (" + base.Message + ")";
            }
        }
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

    public class InvalidPublishingYear : Exception
    {

    }
}
