namespace Components.Ast.ExpressionNodes
{
    public class Constant : Expression
    {
        public long Value { get; }

        public Constant(long value) : base(ExpressionType.Constant) => Value = value;
    }
}
