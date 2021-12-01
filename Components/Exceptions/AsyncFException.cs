using System;

namespace Components.Exceptions
{
    public class AsyncFException : Exception
    {
        public AsyncFException(string message) : base(message) { }
    }
}
