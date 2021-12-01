namespace Components.Tokens
{
    public class NumberToken : Token
    {
        public long Value { get; }

        public NumberToken(long value, int line) : base(TokenType.Number, line) => Value = value;

        public override string ToString() => $"Type: {Type}, Value: {Value}, Line: {Line}";
    }
}
