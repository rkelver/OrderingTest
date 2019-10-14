using System;
using OrderException.Interfaces;

namespace Exceptions
{
    public abstract class OrderingException : System.Exception, IOrderException
    {
        protected OrderingException(params object[] messageInserts)
        {
            ErrorMessage = string.Format(ErrorMessage, messageInserts);
        }

        protected OrderingException(System.Exception ex, params object[] messageInserts)
        {
            RootException = ex;
            ErrorMessage = string.Format(ErrorMessage, messageInserts);
        }

        private System.Exception RootException { get; set; }
        public string ErrorMessage { get; } = "Unable to {0} to the queue of type {1}";
    }
}