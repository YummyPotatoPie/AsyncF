namespace Components.Tokens
{
    public class IdentifierToken : Token
    {
        public string Name { get; }

        public IdentifierToken(string name, int line) : base(TokenType.Identifier, line) => Name = name;

        public override string ToString() => $"Type: {Type}, Name: {Name}, Line: {Line}";
    }
}
