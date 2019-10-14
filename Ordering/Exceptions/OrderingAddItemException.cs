using System;
using Exceptions;

namespace Exception
{
    public class OrderingAddItemException : OrderingException
    {
        public OrderingAddItemException(params object[] messageInserts) : base(messageInserts)
        {
        }

        public OrderingAddItemException(System.Exception ex, object[] messageInserts) : base(ex, messageInserts)
        {
        }
    }
}