using System;
using System.Collections;
using System.Threading.Tasks;

using Components.Tokens;
using Components.Exceptions;

using Components.Ast;
using Components.Ast.ExpressionNodes;

namespace Components
{
    /// <summary>
    /// Class for evaluating AsyncF programs
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Output function name
        /// </summary>
        private static readonly string OutputSpecialFunction = "output";

        /// <summary>
        /// Input function name
        /// </summary>
        private static readonly string InputSpecialFunction = "input";

        /// <summary>
        /// Functions table
        /// </summary>
        private static Hashtable Functions;

        /// <summary>
        /// Local scope for function evaluating 
        /// </summary>
        private static Environment LocalScope;

        /// <summary>
        /// Sets functions hashtable
        /// </summary>
        /// <param name="functions">Parsed functions</param>
        public static void SetEvaluateFunctionsTable(Hashtable functions)
        {
            Functions = functions;
            LocalScope = new(null);
        } 


        /// <summary>
        /// Evaluates expressions 
        /// </summary>
        /// <param name="expression">Function expression</param>
        /// <returns>Number - result of evaluating</returns>
        public static async Task<long> Evaluate(Expression expression) => expression.Type switch
        {
            ExpressionType.If         => await EvaluateIf((IfExpression)expression),
            ExpressionType.Binary     => await EvaluateBinary((BinaryExpression)expression),
            ExpressionType.Relation   => await EvaluateRelation((BinaryExpression)expression),
            ExpressionType.Unary      => await EvaluateUnary((UnaryExpression)expression),
            ExpressionType.Factor     => await Evaluate(((Factor)expression).Expression),
            ExpressionType.Call       => await EvaluateCall((CallExpression)expression),
            ExpressionType.Constant   => ((Constant)expression).Value,
            ExpressionType.Identifier => EvaluateIdentifier((Identifier)expression),
            ExpressionType.Init       => await EvaluateInit((InitExpression)expression),
            ExpressionType.Block      => await EvaluateBlock((BlockExpression)expression),
            _ => 0
        };


        /// <summary>
        /// Evaluates if statement
        /// </summary>
        /// <param name="expression">If statemtnt expression</param>
        /// <returns>Number - result of evaluating</returns>
        private static async Task<long> EvaluateIf(IfExpression expression)
        {
            if (await Evaluate(expression.Condition) > 0) return await Evaluate(expression.TrueBranch);
            return await Evaluate(expression.FalseBranch);
        }


        /// <summary>
        /// Evaluates binary operator expressions
        /// </summary>
        /// <param name="expression">Binary operator expression</param>
        /// <returns>Number - result of evaluating</returns>
        private static async Task<long> EvaluateBinary(BinaryExpression expression) => expression.OperatorType.Type switch
        {
            TokenType.Plus  => await Evaluate(expression.LeftNode) + await Evaluate(expression.RightNode),
            TokenType.Minus => await Evaluate(expression.LeftNode) - await Evaluate(expression.RightNode),
            TokenType.Mult  => await Evaluate(expression.LeftNode) * await Evaluate(expression.RightNode),
            TokenType.Div   => await Evaluate(expression.LeftNode) / await Evaluate(expression.RightNode),
            TokenType.Mod   => await Evaluate(expression.LeftNode) % await Evaluate(expression.RightNode),
            _ => 0
        };


        /// <summary>
        /// Evaluates relation expression
        /// </summary>
        /// <param name="expression">Relation expression</param>
        /// <returns>Number - result of evaluating</returns>
        private static async Task<long> EvaluateRelation(BinaryExpression expression) => expression.OperatorType.Type switch
        {
            TokenType.Equal  => await Evaluate(expression.LeftNode) == await Evaluate(expression.RightNode) ? 1 : 0,
            TokenType.Lower  => await Evaluate(expression.LeftNode) <  await Evaluate(expression.RightNode) ? 1 : 0,
            TokenType.Bigger => await Evaluate(expression.LeftNode) >  await Evaluate(expression.RightNode) ? 1 : 0,
            _ => 0
        };


        /// <summary>
        /// Evaluates unary expressions
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Number - result of evaluating</returns>
        private static async Task<long> EvaluateUnary(UnaryExpression expression) =>
            expression.UnaryOperator.Type == TokenType.Plus ? +(await Evaluate(expression.Node)) : -(await Evaluate(expression.Node));


        /// <summary>
        /// Evaluate initialization expression
        /// </summary>
        /// <param name="expression">Initialization expression</param>
        /// <returns>Named constant value</returns>
        private static async Task<long> EvaluateInit(InitExpression expression)
        {
            if (LocalScope.GetConstant(expression.NamedConstant.Name) != null) 
                throw new EvaluateException($"Named constant '{expression.NamedConstant.Name}' already exist at local scope and cannot change value", expression.NamedConstant.Line);

            LocalScope.PutConstant(expression.NamedConstant.Name, await Evaluate(expression.ConstantExpression));
            return (long)LocalScope.GetConstant(expression.NamedConstant.Name);
        }


        /// <summary>
        /// Evaluate block expression
        /// </summary>
        /// <param name="expression">Block expression</param>
        /// <remarks>If block doesnt have any expression returns 0</remarks>
        /// <returns>Last block expression evaluate result</returns>
        private static async Task<long> EvaluateBlock(BlockExpression expression)
        {
            if (expression.BlockExpressions.Length == 0) return 0;

            LocalScope = new Environment(LocalScope);
            for (int i = 0; i < expression.BlockExpressions.Length - 1; i++) await Evaluate(expression.BlockExpressions[i]);
            long blockResult = await Evaluate(expression.BlockExpressions[^1]);

            LocalScope = LocalScope.Previous;
            return blockResult;
        }



        /// <summary>
        /// Evaluates function call expressions
        /// </summary>
        /// <param name="expression">Function call expression</param>
        /// <returns>Number - result of evaluating</returns>
        private static async Task<long> EvaluateCall(CallExpression expression)
        {
            Function functionAst = (Function)Functions[expression.CallFunctionName.Name];
            if (functionAst == null)
            {
                if (expression.CallFunctionName.Name == OutputSpecialFunction)
                {
                    foreach (Expression arg in expression.ArgsValues) Console.WriteLine(await Evaluate(arg));
                    return 0;
                }
                else if (expression.CallFunctionName.Name == InputSpecialFunction)
                {
                    if (long.TryParse(Console.ReadLine(), out long inputValue)) return inputValue;
                    return 0;
                }
                else throw new EvaluateException($"Function '{expression.CallFunctionName.Name}' does not exist", expression.CallFunctionName.Line);
            }

            LocalScope = new Environment(LocalScope);
            if (functionAst.Args.Length != expression.ArgsValues.Length) throw new Exception();
            for (int i = 0; i < functionAst.Args.Length; i++) LocalScope.PutConstant(functionAst.Args[i].Name, await Evaluate(expression.ArgsValues[i]));

            long callResult = await Evaluate(functionAst.Body);
            LocalScope = LocalScope.Previous;
            return callResult;
        }


        /// <summary>
        /// Evaluates identifier expression
        /// </summary>
        /// <param name="expression">Identifier expression</param>
        /// <returns>Number - result of evaluating</returns>
        private static long EvaluateIdentifier(Identifier expression)
        {
            try
            {
                return (long)LocalScope.GetConstant(expression.IdentifierInfo.Name);
            }
            catch
            {
                throw new EvaluateException($"Constant '{expression.IdentifierInfo.Name}' does not exist", expression.IdentifierInfo.Line);
            }
        }
    }
}
