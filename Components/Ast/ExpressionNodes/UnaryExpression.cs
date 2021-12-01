using Components.Tokens;

namespace Components.Ast.ExpressionNodes
{
    public class UnaryExpression : Expression
    {
        public Token UnaryOperator { get; }

        public Expression Node { get; }

        public UnaryExpression(Token unaryOperator, Expression node) : base(ExpressionType.Unary)
        {
            UnaryOperator = unaryOperator;
            Node = node;
        }
    }
}
