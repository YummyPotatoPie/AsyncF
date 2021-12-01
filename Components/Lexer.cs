using System;
using System.Text;
using System.Collections.Generic;

using Components.Tokens;
using Components.Exceptions;

namespace Components
{
    /// <summary>
    /// 
    /// </summary>
    public class Lexer : IResetable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string _specialSymbols = "();:{}";

        /// <summary>
        /// 
        /// </summary>
        private readonly string _operatorSymbols = "-+*/%<>=";

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, TokenType> _keywords = new()
        {
            { "if", TokenType.If },
            { "then", TokenType.Then },
            { "else", TokenType.Else }
        };

        /// <summary>
        /// 
        /// </summary>
        private int _position = 0;

        /// <summary>
        /// 
        /// </summary>
        private  string _stream;

        /// <summary>
        /// 
        /// </summary>
        private int _line = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public Lexer(string stream) => _stream = stream;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public void Reset(string stream)
        {
            _stream = stream;
            _position = 0;
            _line = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Token NextToken()
        {
            SkipWhiteSpacesAndComments();

            if (Eof()) return null;
            if (char.IsLetter(_stream[_position]) || _stream[_position] == '_') return NextWord();
            if (char.IsDigit(_stream[_position])) return NextNumber();
            if (_specialSymbols.Contains(_stream[_position])) return NextSpecialToken();
            if (_operatorSymbols.Contains(_stream[_position])) return NextOperator();
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Token NextWord()
        {
            StringBuilder word = new();

            while (!Eof() && ((char.IsLetterOrDigit(_stream[_position]) || _stream[_position] == '_')))
            {
                word.Append(_stream[_position]);
                _position++;
            }

            if (_keywords.ContainsKey(word.ToString())) return new Token(_keywords[word.ToString()], _line);
            return new IdentifierToken(word.ToString(), _line);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private NumberToken NextNumber()
        {
            StringBuilder number = new();

            while (char.IsDigit(_stream[_position]) && !Eof())
            {
                number.Append(_stream[_position]);
                _position++;
            }

            try
            {
                return new NumberToken(long.Parse(number.ToString()), _line);
            }
            catch (OverflowException)
            {
                throw new LexerErrorException("Number is too big for 64 bits value", _line);
            }
            catch (ArgumentException)
            {
                throw new LexerErrorException("Invalid number format", _line);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Token NextSpecialToken()
        {
            Token token = null;

            switch (_stream[_position])
            {
                case '(':
                    token = new(TokenType.LBracket, _line);
                    break;
                case ')':
                    token = new(TokenType.RBracket, _line);
                    break;
                case ';':
                    token = new(TokenType.Semicolon, _line);
                    break;
                case ':':
                    token = new(TokenType.Colon, _line);
                    break;
                case '{':
                    token = new(TokenType.LCurlyBracket, _line);
                    break;
                case '}':
                    token = new(TokenType.RCurlyBracket, _line);
                    break;
            }
            _position++;
            return token;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Token NextOperator()
        {
            Token token = null;

            switch (_stream[_position])
            {
                case '-':
                    _position++;
                    if (!Eof() && _stream[_position] == '>')
                    {
                        token = new(TokenType.ReturnOp, _line);
                        break;
                    }
                    _position--;
                    token = new(TokenType.Minus, _line);
                    break;
                case '+':
                    token = new(TokenType.Plus, _line);
                    break;
                case '*':
                    token = new(TokenType.Mult, _line);
                    break;
                case '/':
                    token = new(TokenType.Div, _line);
                    break;
                case '%':
                    token = new(TokenType.Mod, _line);
                    break;
                case '<':
                    token = new(TokenType.Lower, _line);
                    break;
                case '>':
                    token = new(TokenType.Bigger, _line);
                    break;
                case '=':
                    token = new(TokenType.Equal, _line);
                    break;
            }
            _position++;
            return token;
        }


        /// <summary>
        /// 
        /// </summary>
        private void SkipWhiteSpacesAndComments()
        {
            while (!Eof() && ((char.IsWhiteSpace(_stream[_position]) || _stream[_position] == '#')))
            {
                SkipWhiteSpaces();
                if (!Eof() && _stream[_position] == '#') SkipComments();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void SkipWhiteSpaces()
        {
            while (!Eof() && char.IsWhiteSpace(_stream[_position]))
            {
                if (_stream[_position] == '\n') _line++;
                _position++;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void SkipComments()
        {
            while (!Eof() && _stream[_position] != '\n') _position++;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool Eof() => _position >= _stream.Length;
    }
}
