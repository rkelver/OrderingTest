using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions
{
    public class OrderingAddItemException : OrderingException
    {
        public OrderingAddItemException(params object[] messageInserts) : base(messageInserts)
        {
            
        }

        public OrderingAddItemException(Exception ex, object[] messageInserts) : base(ex,messageInserts)
        {

        }
    }
}
