using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Compiler
{
    class LexerM
    {
        private static readonly Dictionary<TokenGroup, Regex> tokenRegexes = new Dictionary<TokenGroup, Regex>
        {
            { TokenGroup.Keyword, new Regex(@"^(let|var|if|else|for|while|return|function|in|true|false|point|circle|draw|line|sequence|import)$") },
            { TokenGroup.VariablesTypes, new Regex(@"^(String|Number)$") },
            { TokenGroup.Variable, new Regex(@"^[_a-zA-Z][_a-zA-Z0-9]*$") },
            {
                TokenGroup.Operator,
                new Regex(
                    @"^(\:|\;|\(|\)|\+|\-|\*|\/|\=\=|\!=|\!|\=|\?|\,|\{|\}|\:\=|\>\=|\>|\=\>|\<|\<\=|\|\||&&|\@|\^|\.\.\.|\.|\.\.)$")
            },
            { TokenGroup.WhiteSpace, new Regex(@"^\s+$") },
            { TokenGroup.Number, new Regex(@"^(\d+\.?\d*)$") },
            { TokenGroup.StringGroup, new Regex(@"^"".*?""$") },
            { TokenGroup.BadStringGroup, new Regex(@"^(""|"".*)$") },
            { TokenGroup.Porcent, new Regex(@"^(%)$") },
        };

        public static List<TokenM> Lex(string input)
        {
            List<string> filesReaded = new List<string>();
            
            var tokens = new List<TokenM>();
            var remainingText = input;
            var currentToken = "";

            while (!string.IsNullOrEmpty(remainingText))
            {
                var currentChar = remainingText[0].ToString();
                remainingText = remainingText.Substring(1);


                var potentialToken = currentToken + currentChar;
                var matchedToken = false;
                if (tokenRegexes[TokenGroup.BadStringGroup].IsMatch(currentToken) && currentChar == "\"")
                {
                    tokens.Add(new TokenM(TokenType.StringToken, currentToken.Substring(1)));
                    currentToken = "";
                    continue;
                }

                foreach (var tokenRegex in tokenRegexes)
                {
                    if (tokenRegex.Value.IsMatch(potentialToken))
                    {
                        currentToken = potentialToken;
                        matchedToken = true;
                    }
                }

                if (!matchedToken)
                {
                    var tokenType = Match(currentToken).Item1;
                    tokens.Add(new TokenM(tokenType, currentToken));
                    currentToken = currentChar;
                }
            }

            if (!string.IsNullOrEmpty(currentToken))
            {
                var tokenType = Match(currentToken).Item1;
                tokens.Add(new TokenM(tokenType, currentToken));
            }

            tokens.Add(new TokenM(TokenType.EOF, ""));
            return tokens;
        }

        private static Tuple<TokenType, string> Match(string input)
        {
            foreach (var tokenRegex in tokenRegexes)
            {
                var match = tokenRegex.Value.Match(input);
                if (match.Success)
                {
                    switch (tokenRegex.Key)
                    {
                        case TokenGroup.VariablesTypes:
                            var varType = GetVariableTypeToken(match.Value);
                            return new Tuple<TokenType, string>(varType, match.Value);
                        case TokenGroup.Keyword:
                            var keywordType = GetKeywordToken(match.Value);
                            return new Tuple<TokenType, string>(keywordType, match.Value);
                        case TokenGroup.Operator:
                            var operatorType = GetOperatorToken(match.Value);
                            return new Tuple<TokenType, string>(operatorType, match.Value);
                        case TokenGroup.Variable:
                            return Tuple.Create(TokenType.IdentifierToken, match.Value);
                        case TokenGroup.Number:
                            return Tuple.Create(TokenType.NumberToken, match.Value);
                        case TokenGroup.WhiteSpace:
                            return Tuple.Create(TokenType.WhiteSpaceToken, match.Value);
                        case TokenGroup.StringGroup:
                            return Tuple.Create(TokenType.StringToken, match.Value);
                        case TokenGroup.BadStringGroup:
                            throw new Exception("Invalid string declaration   " + match.Value);
                        case TokenGroup.Porcent:
                            return Tuple.Create(TokenType.ModulusToken, match.Value);
                        default:
                            break;
                    }
                }
            }

            throw new Exception("Invalid token " + input);
        }

        private static TokenType GetVariableTypeToken(string input)
        {
            switch (input)
            {
                case "String":
                    return TokenType.StringTypeToken;
                case "Number":
                    return TokenType.NumberTypeToken;
            }

            throw new Exception("Invalid token " + input);
        }

        private static TokenType GetKeywordToken(string input)
        {
            switch (input)
            {
                case "if":
                    return TokenType.IfToken;
                case "else":
                    return TokenType.ElseToken;
                case "for":
                    return TokenType.ForToken;
                case "while":
                    return TokenType.WhileToken;
                case "do":
                    return TokenType.DoToken;
                case "switch":
                    return TokenType.SwitchToken;
                case "case":
                    return TokenType.CaseToken;
                case "break":
                    return TokenType.BreakToken;
                case "continue":
                    return TokenType.ContinueToken;
                case "default":
                    return TokenType.DefaultToken;
                case "return":
                    return TokenType.ReturnToken;
                case "true":
                    return TokenType.TrueToken;
                case "false":
                    return TokenType.FalseToken;
                case "null":
                    return TokenType.NullToken;
                case "let":
                    return TokenType.LetToken;
                case "var":
                    return TokenType.VarToken;
                case "function":
                    return TokenType.FunctionToken;
                case "in":
                    return TokenType.InToken;
                case "point":
                    return TokenType.PointDeclaration;
                case "circle":
                    return TokenType.CircleDeclaration;
                case "measure":
                    return TokenType.MeasureDeclaration;
                case "line":
                    return TokenType.LineDeclaration;
                case "draw":
                    return TokenType.DrawToken;
                case "sequence":
                    return TokenType.SequenceToken;
                case "import":
                    return TokenType.ImportToken;
            }

            throw new Exception("Invalid token " + input);
        }

        private static TokenType GetOperatorToken(string input)
        {
            switch (input)
            {
                case ":=":
                    return TokenType.AssignmentTwoDotsToken;
                case ":":
                    return TokenType.TwoDotsToken;
                case ",":
                    return TokenType.CommaToken;
                case "?":
                    return TokenType.QuestionMarkToken;
                case "+":
                    return TokenType.AdditionToken;
                case "-":
                    return TokenType.SubtractionToken;
                case "*":
                    return TokenType.MultiplicationToken;
                case "/":
                    return TokenType.DivisionToken;
                case "=":
                    return TokenType.AssignmentToken;
                case "==":
                    return TokenType.EqualityToken;
                case "!=":
                    return TokenType.InequalityToken;
                case ">":
                    return TokenType.GreaterThanToken;
                case ">=":
                    return TokenType.GreaterThanOrEqualToken;
                case "<":
                    return TokenType.LessThanToken;
                case "<=":
                    return TokenType.LessThanOrEqualToken;
                case "&&":
                    return TokenType.LogicalAndToken;
                case "||":
                    return TokenType.LogicalOrToken;
                case "!":
                    return TokenType.LogicalNotToken;
                case ";":
                    return TokenType.SemicolonToken;
                case "(":
                    return TokenType.LeftParenthesisToken;
                case ")":
                    return TokenType.RightParenthesisToken;
                case "{":
                    return TokenType.LeftBraceToken;
                case "}":
                    return TokenType.RightBraceToken;
                case "=>":
                    return TokenType.ArrowToken;
                case "@":
                    return TokenType.ArrobaToken;
                case "^":
                    return TokenType.PowToken;
                case "...":
                    return TokenType.ThreeDotsToken;
            }

            throw new Exception("Invalid token " + input);
        }
    }
}