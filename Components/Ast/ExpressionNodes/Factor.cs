namespace Components.Ast.ExpressionNodes
{
    public class Factor : Expression
    {
        public Expression Expression { get; }

        public Factor(Expression expression) : base(ExpressionType.Factor) => Expression = expression;
    }
}
