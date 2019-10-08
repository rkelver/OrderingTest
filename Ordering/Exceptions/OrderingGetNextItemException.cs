using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions
{
    public class OrderingGetNextItemException : OrderingException
    {
        public OrderingGetNextItemException(params object[] messageInserts) : base(messageInserts)
        {

        }

        public OrderingGetNextItemException(Exception ex, object[] messageInserts) : base(ex, messageInserts)
        {

        }
    }
}
