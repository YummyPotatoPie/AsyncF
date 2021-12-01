namespace Components.Ast.ExpressionNodes
{
    public class IfExpression : Expression
    {
        public Expression Condition { get; }

        public Expression TrueBranch { get; }

        public Expression FalseBranch { get; }

        public IfExpression(Expression condition, Expression trueBranch, Expression falseBranch) : base(ExpressionType.If)
        {
            Condition = condition;
            TrueBranch = trueBranch;
            FalseBranch = falseBranch;
        }
    }
}
