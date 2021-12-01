using Components.Tokens;

namespace Components.Ast.ExpressionNodes
{
    public class CallExpression : Expression
    {
        public IdentifierToken CallFunctionName { get; }

        public Expression[] ArgsValues { get; }

        public CallExpression(IdentifierToken callFunctionName, Expression[] argsValues) : base(ExpressionType.Call)
        {
            CallFunctionName = callFunctionName;
            ArgsValues = argsValues;
        }
    }
}
