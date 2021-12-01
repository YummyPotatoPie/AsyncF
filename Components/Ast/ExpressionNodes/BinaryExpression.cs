using Components.Tokens;

namespace Components.Ast.ExpressionNodes
{
    public class BinaryExpression : Expression
    {
        public Token OperatorType { get; }

        public Expression LeftNode { get; }

        public Expression RightNode { get; }

        public BinaryExpression(Token operatorType, Expression leftNode, Expression rightNode, ExpressionType type) : base(type)
        {
            OperatorType = operatorType;
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
}
