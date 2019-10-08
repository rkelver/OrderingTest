using System;

namespace Exceptions
{
    public abstract class OrderingException : Exception
    {
        public Exception RootException { get; set; }
        public string ErrorMessage { get; } = "Unable to {0} to the queue of type {1}";

        protected OrderingException(params object[] messageInserts)
        {
            ErrorMessage = String.Format(ErrorMessage, messageInserts);
        }

        protected OrderingException(Exception ex,params object[] messageInserts)
        {
            RootException = ex;
            ErrorMessage = String.Format(ErrorMessage, messageInserts);
        }
    }
}
