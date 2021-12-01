namespace Components.Ast.ExpressionNodes
{
    public class Expression
    {
        public ExpressionType Type { get; }

        public Expression(ExpressionType type) => Type = type;
    }
}
