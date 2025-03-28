using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class ServerExceptions
    {
        public class InternalServerException : Exception
        {
            public InternalServerException(string message) : base(message) { }
        }
    }
}
