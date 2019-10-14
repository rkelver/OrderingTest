using System;

namespace Exceptions
{
    public class OrderingGetNextItemException : OrderingException
    {
        public OrderingGetNextItemException(params object[] messageInserts) : base(messageInserts)
        {
        }

        public OrderingGetNextItemException(System.Exception ex, object[] messageInserts) : base(ex, messageInserts)
        {
        }
    }
}