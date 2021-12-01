using System.Collections.Generic;

using Components.Exceptions;
using Components.Tokens;
using Components.Ast;
using Components.Ast.ExpressionNodes;

namespace Components
{
    public class Parser : IResetable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Lexer _lexer;

        /// <summary>
        /// 
        /// </summary>
        private Token _currentToken;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lexer"></param>
        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.NextToken();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void Reset(string stream)
        {
            _lexer.Reset(stream);
            _currentToken = _lexer.NextToken();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public void Match(TokenType type)
        {
            if (_currentToken == null) throw new ParserErrorException("Unexpected end of file", -1);

            if (_currentToken.Type == type) _currentToken = _lexer.NextToken();
            else throw new ParserErrorException($"Expected {type} token", _currentToken.Line);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Program Parse()
        {
            Program programFunctions = new();

            while (_currentToken != null) programFunctions.AddFunction(Function());

            return programFunctions;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Function Function()
        {
            IdentifierToken name = Identifier();
            List<IdentifierToken> args = new();
            Expression expression;

            Match(TokenType.LBracket);
            while (_currentToken.Type != TokenType.RBracket) args.Add(Identifier());
            Match(TokenType.RBracket);

            Match(TokenType.ReturnOp);
            expression = Expression();
            Match(TokenType.Semicolon);

            return new Function(name, args.ToArray(), expression);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression Expression() => _currentToken.Type switch
        {
            TokenType.If            => IfExpression(),
            TokenType.LCurlyBracket => BlockExpression(),
            _ => ValueExpression()
        };


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression IfExpression()
        {
            Expression condition, trueBranch, falseBranch;

            Match(TokenType.If);
            condition = Expression();

            Match(TokenType.Then);
            trueBranch = Expression();

            Match(TokenType.Else);
            falseBranch = Expression();

            return new IfExpression(condition, trueBranch, falseBranch);
        }


        /// <summary>
        /// Parses block expressions
        /// </summary>
        /// <returns>Block of expressions</returns>
        private Expression BlockExpression()
        {
            List<Expression> blockExpressions = new();

            Match(TokenType.LCurlyBracket);
            while (_currentToken.Type != TokenType.RCurlyBracket)
            {
                blockExpressions.Add(Expression());
                Match(TokenType.Semicolon);
            }
            Match(TokenType.RCurlyBracket);

            return new BlockExpression(blockExpressions.ToArray());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression ValueExpression()
        {
            Expression relExpression = AddTerm();

            while (_currentToken.Type == TokenType.Bigger || _currentToken.Type == TokenType.Lower || _currentToken.Type == TokenType.Equal)
            {
                Token op = _currentToken;
                Match(_currentToken.Type);
                relExpression = new BinaryExpression(op, relExpression, ValueExpression(), ExpressionType.Relation);
            }

            return relExpression;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression AddTerm()
        {
            Expression addExpression = MultTerm();

            while (_currentToken.Type == TokenType.Plus || _currentToken.Type == TokenType.Minus)
            {
                Token op = _currentToken;
                Match(_currentToken.Type);
                addExpression = new BinaryExpression(op, addExpression, AddTerm(), ExpressionType.Binary);
            }

            return addExpression;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression MultTerm()
        {
            Expression multExpression = UnaryTerm();

            while (_currentToken.Type == TokenType.Mult || _currentToken.Type == TokenType.Div || _currentToken.Type == TokenType.Mod)
            {
                Token op = _currentToken;
                Match(_currentToken.Type);
                multExpression = new BinaryExpression(op, multExpression, MultTerm(), ExpressionType.Binary);
            }

            return multExpression;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression UnaryTerm()
        {
            if (_currentToken.Type == TokenType.Plus || _currentToken.Type == TokenType.Minus)
            {
                Token op = _currentToken;
                Match(_currentToken.Type);
                return new UnaryExpression(op, UnaryTerm());
            }

            return Factor();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Expression Factor()
        {
            if (_currentToken.Type == TokenType.LBracket)
            {
                Match(TokenType.LBracket);
                Expression factor = Expression();
                Match(TokenType.RBracket);
                return factor;
            }
            else if (_currentToken.Type == TokenType.Number)
            {
                Token number = _currentToken;
                Match(TokenType.Number);
                return new Constant(((NumberToken)number).Value);
            }
            else
            {
                IdentifierToken name = Identifier();
                if (_currentToken.Type == TokenType.LBracket)
                {
                    List<Expression> argsValues = new();

                    Match(TokenType.LBracket);
                    while (_currentToken.Type != TokenType.RBracket) argsValues.Add(Expression());
                    Match(TokenType.RBracket);

                    return new CallExpression(name, argsValues.ToArray());
                }
                else if (_currentToken.Type == TokenType.Colon)
                {
                    Match(TokenType.Colon);
                    return new InitExpression(name, Expression());
                }
                return new Identifier(name);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IdentifierToken Identifier()
        {
            if (_currentToken.Type != TokenType.Identifier) throw new ParserErrorException("Expected identifier", _currentToken.Line);
            IdentifierToken identifier = (IdentifierToken)_currentToken;
            Match(TokenType.Identifier);
            return identifier;
        }
    }
}
