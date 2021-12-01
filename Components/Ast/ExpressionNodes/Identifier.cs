using Components.Tokens;

namespace Components.Ast.ExpressionNodes
{
    public class Identifier : Expression
    {
        public IdentifierToken IdentifierInfo { get; }

        public Identifier(IdentifierToken identifierInfo) : base(ExpressionType.Identifier) => IdentifierInfo = identifierInfo;
    }
}
