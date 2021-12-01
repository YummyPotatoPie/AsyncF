namespace Components.Tokens
{
    public class Token
    {
        public TokenType Type { get; }

        public int Line { get; }

        public Token(TokenType type, int line)
        {
            Type = type;
            Line = line;
        }

        public override string ToString() => $"Type: {Type}, Line: {Line}";
    }
}
