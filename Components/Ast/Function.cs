using Components.Tokens;
using Components.Ast.ExpressionNodes;

namespace Components.Ast
{
    public class Function
    {
        public IdentifierToken Name { get; }

        public IdentifierToken[] Args { get; }

        public Expression Body { get; }

        public Function(IdentifierToken name, IdentifierToken[] args, Expression body)
        {
            Name = name;
            Args = args;
            Body = body;
        }
    }
}
