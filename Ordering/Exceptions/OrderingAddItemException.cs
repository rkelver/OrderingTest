using System;

namespace Exceptions
{
    public class OrderingAddItemException : OrderingException
    {
        public OrderingAddItemException(params object[] messageInserts) : base(messageInserts)
        {
        }

        public OrderingAddItemException(Exception ex, object[] messageInserts) : base(ex, messageInserts)
        {
        }
    }
}