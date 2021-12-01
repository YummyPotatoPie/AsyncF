using System;

namespace Components.Exceptions
{
    public class ParserErrorException : AsyncFException
    {
        public ParserErrorException(string message, int line) : base($"Parser error: {message} at line {line}.") { }
    }
}
