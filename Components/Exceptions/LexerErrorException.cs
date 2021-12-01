using System;

namespace Components.Exceptions
{
    public class LexerErrorException : AsyncFException
    {
        public LexerErrorException(string message, int line) : base($"Lexer error: <{message}> at line {line}.") { }
    }
}
