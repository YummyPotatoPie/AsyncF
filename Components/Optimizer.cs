using System.Threading.Tasks;

using Components.Ast;
using Components.Ast.ExpressionNodes;

namespace Components
{
    public static class Optimizer
    {
        /*
        public static async Task<Function> Optimize(Function function) => new Function(function.Name, function.Args, await OptimizeExpression(function.Body));

        private static async Task<Expression> OptimizeExpression(Expression expression) => expression.Type switch
            {
                ExpressionType.If      =>  await OptimizeIfExpression((IfExpression)expression),
                ExpressionType.Binary  =>  await OptimizeBinaryExpression((BinaryExpression)expression),
                ExpressionType.Unary   =>  await OptimizeUnaryExpression((UnaryExpression)expression),
                ExpressionType.Call    =>  await OptimizeCallExpression((CallExpression)expression),
                ExpressionType.Factor  =>  await OptimizeFactorExpression((Factor)expression),
                _ => expression
            };

        private static async Task<Expression> OptimizeIfExpression(IfExpression ifExpression) =>
            new IfExpression(await OptimizeExpression(ifExpression.Condition), await OptimizeExpression(ifExpression.TrueBranch), await OptimizeExpression (ifExpression.FalseBranch));

        private static async Task<Expression> OptimizeBinaryExpression(BinaryExpression expression)
        {
            
        }

        private static async Task<Expression> EvaluateBinaryNode(BinaryExpression expression)
        {
            Expression 
        }
        */
    }
}
