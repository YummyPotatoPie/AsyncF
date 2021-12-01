using System;

namespace Components.Exceptions
{
    public class EvaluateException : AsyncFException
    {
        public EvaluateException(string message, int line) : base($"Evaluate error: {message} at line {line}.") { }
    }
}
