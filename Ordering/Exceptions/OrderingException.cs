using System;

namespace Exceptions
{
    public abstract class OrderingException : Exception
    {
        protected OrderingException(params object[] messageInserts)
        {
            ErrorMessage = string.Format(ErrorMessage, messageInserts);
        }

        protected OrderingException(Exception ex, params object[] messageInserts)
        {
            RootException = ex;
            ErrorMessage = string.Format(ErrorMessage, messageInserts);
        }

        public Exception RootException { get; set; }
        public string ErrorMessage { get; } = "Unable to {0} to the queue of type {1}";
    }
}