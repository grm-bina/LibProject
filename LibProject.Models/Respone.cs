using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibProject.Exceptions;

namespace LibProject.Models
{
    // for get feedback about action
    public class Respone<T>
    {
        public T Data;
        public string Message;
        public bool IsWorking;
        public WrongData WrongDataType;
    }
}
