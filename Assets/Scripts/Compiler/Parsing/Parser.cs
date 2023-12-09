using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Compiler
{
    public class ParserM
    {
        private List<TokenM> tokens;
        private int currentTokenIndex;
        private Dictionary<TokenType, int> precedence;

        public ParserM(List<TokenM> tokens)
        {
            this.tokens = tokens;
            this.currentTokenIndex = 0;
            this.precedence = new Dictionary<TokenType, int>
        {
            { TokenType.EqualityToken, 1 },
            { TokenType.InequalityToken, 1 },
            { TokenType.LogicalAndToken,1},
            { TokenType.LogicalOrToken,1},
            { TokenType.LessThanToken, 2 },
            { TokenType.LessThanOrEqualToken, 2 },
            { TokenType.GreaterThanToken, 2 },
            { TokenType.GreaterThanOrEqualToken, 2 },
            { TokenType.AdditionToken, 3 },
            { TokenType.SubtractionToken, 3 },
            { TokenType.ArrobaToken,3},
            { TokenType.MultiplicationToken, 4 },
            { TokenType.DivisionToken, 4 },
            { TokenType.PowToken,5},
            { TokenType.ModulusToken,5},
        };
        }

        public MainProgramNode Parse()
        {
            List<Node> statements = new List<Node>();
            while (!IsAtEnd())
            {
                Node statement = ParseStatement();

                if (statement != null)
                {
                    statements.Add(statement);
                }
            }
            MainProgramNode mainProgram = new MainProgramNode(statements);
            return mainProgram;
        }

        private Node ParseStatement()
        {
            if (Match(TokenType.PrintToken))
            {
                Node expr = ParseExpression();
                Consume(TokenType.SemicolonToken, "Expected ';' after expression.");
                return new PrintNode(expr);
            }
            else if (Match(TokenType.LeftBraceToken))
            {
                TokenM SequenceDesignator1 = Advance();
                Debug.Log("secuencia tipo: "+SequenceDesignator1.Value+ " de tipo: "+SequenceDesignator1.Type);

                if (SequenceDesignator1.Type == TokenType.NumberToken)
                {
                    TokenM SequenceDesignator2 = Advance();
                    if (SequenceDesignator2.Type == TokenType.ThreeDotsToken)
                    {
                        TokenM SequenceDesignator3 = Advance();
                            
                        if (SequenceDesignator3.Type == TokenType.NumberToken)
                        {
                            Consume(TokenType.RightBraceToken, "Expected '}' after sequence declaration");
                            return new SequenceNode(new List<Node>(),"number",0,false,int.Parse(SequenceDesignator1.Value),int.Parse(SequenceDesignator3.Value));
                        }
                        else if(SequenceDesignator3.Type == TokenType.RightBraceToken)
                        {
                            return new SequenceNode(new List<Node>(),"number",0,true,int.Parse(SequenceDesignator1.Value),0);
                        }
                        else
                        {
                                
                        }
                    }
                    else if(SequenceDesignator2.Type == TokenType.NumberToken)
                    {

                    }
                }
                else if(SequenceDesignator1.Type == TokenType.StringToken)
                {
                    
                }
                else if (SequenceDesignator1.Type == TokenType.RightBraceToken)
                {
                    return new SequenceNode(new List<Node>(),"");
                }
                else if (SequenceDesignator1.Type == TokenType.IdentifierToken)
                {
                    Debug.Log("Sequence with identifier$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                    TokenM SequenceDesignator2 = Advance();
                    if (SequenceDesignator2.Type == TokenType.CommaToken)
                    {
                        Debug.Log("todo ok $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                        List<Node> nodesA = new List<Node>();
                        nodesA.Add(new VariableReferenceNode(SequenceDesignator1.Value));
                        
                        do
                        {
                            TokenM actualToken = Advance();

                            if (actualToken.Type == TokenType.IdentifierToken)
                            {
                                nodesA.Add(new VariableReferenceNode(actualToken.Value));
                            }
                            
                        } while (Match(TokenType.CommaToken));
                        Consume(TokenType.RightBraceToken, "Expected '}' after sequence declaration");
                        Debug.Log($"{nodesA.Count} $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
                        return new SequenceNode(nodesA, "ids");
                    }
                }


                throw new Exception("Bad Sequence Declaration");
            }
            else if (Match(TokenType.PointDeclaration))
            {
                if (Check(TokenType.IdentifierToken))
                {
                    TokenM pointName = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                    PointDeclarationNode point = new PointDeclarationNode(pointName.Value);
                    return new VariableDeclarationNode(pointName.Value, point, VariableType.Point);
                }
                else if(Match(TokenType.SequenceToken))
                {
                    TokenM pointName = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                    SequenceNode sequenceNode = new SequenceNode(new List<Node>(),"point", 0, true);
                    return new VariableDeclarationNode(pointName.Value, sequenceNode, VariableType.Implicit);
                }else if (Match(TokenType.LeftParenthesisToken))
                {
                    Node id1 = ParseStatement();
                    Consume(TokenType.CommaToken, "Expected ',' ");
                    Node id2 = ParseStatement();

                    Consume(TokenType.RightParenthesisToken, "Expected ')' after line parameters.");

                    return new PointDeclarationNode("point", id1, id2);

                }


                throw new Exception("Bad point declaration");
            }
            else if (Match(TokenType.CircleDeclaration))
            {

                if (Check(TokenType.IdentifierToken))
                {
                    TokenM cicleName = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                    
                    CircleDeclarationNode circle = new CircleDeclarationNode(cicleName.Value);
                    return new VariableDeclarationNode(cicleName.Value, circle, VariableType.Circle);
                }
                else if (Match(TokenType.LeftParenthesisToken))
                {
                    TokenM id1 = Consume(TokenType.IdentifierToken, "Expected point name after 'circle' keyword.");
                    Consume(TokenType.CommaToken, "Expected ',' ");
                    TokenM id2 = Consume(TokenType.IdentifierToken, "Expected point name after 'circle' keyword.");

                    Consume(TokenType.RightParenthesisToken, "Expected ')' after line parameters.");

                    return new CircleDeclarationNode("circle"+id1.Value,id1.Value, new VariableReferenceNode(id2.Value));

                }




                throw new Exception("Bad circle declaration");
            }
            else if (Match(TokenType.LineDeclaration))
            {

                if (Match(TokenType.LeftParenthesisToken))
                {
                    TokenM pointName1 = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                    Consume(TokenType.CommaToken, "Expected ',' ");
                    TokenM pointName2 = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");

                    Consume(TokenType.RightParenthesisToken, "Expected ')' after line parameters.");

                    return new LineDeclarationNode(pointName1.Value, pointName2.Value);

                }

                throw new Exception("Bad line declaration");
            }
            else if (Match(TokenType.DrawToken))
            {
                Debug.Log("Draw token aaaaaa");
                List<Node> instanciables = new List<Node>();
                if (Check(TokenType.IdentifierToken))
                {
                    TokenM identifier = Consume(TokenType.IdentifierToken, "alooooooo");
                    instanciables.Add(new VariableReferenceNode(identifier.Value));
                }
                else if (Match(TokenType.PointDeclaration))
                {
                    if (Check(TokenType.IdentifierToken))
                    {
                        TokenM pointName = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                        PointDeclarationNode point = new PointDeclarationNode(pointName.Value);
                        instanciables.Add(new VariableDeclarationNode(pointName.Value, point, VariableType.Point));
                    }
                }
                else if (Match(TokenType.LineDeclaration))
                {

                    Debug.Log(tokens[currentTokenIndex].Type);
                    if (Match(TokenType.LeftParenthesisToken))
                    {
                        TokenM pointName1 = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                        Consume(TokenType.CommaToken, "Expected ',' ");
                        TokenM pointName2 = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");

                        Consume(TokenType.RightParenthesisToken, "Expected ')' after line parameters.");

                        string lineName1 = pointName1.Value + pointName2.Value;
                        LineDeclarationNode lineNode = new LineDeclarationNode(pointName1.Value, pointName2.Value);
                        instanciables.Add(new VariableDeclarationNode(lineName1, lineNode, VariableType.Line));


                    }
                    else
                    {
                        throw new Exception("Bad line declaration");
                    }
                    
                }
                else if (Match(TokenType.LeftBraceToken))
                {
                    do
                    {
                        if (Check(TokenType.IdentifierToken))
                        {
                            TokenM identifier = Consume(TokenType.IdentifierToken, "alooooooo");
                            instanciables.Add(new VariableReferenceNode(identifier.Value));
                        }
                        else if (Match(TokenType.PointDeclaration))
                        {
                            if (Check(TokenType.IdentifierToken))
                            {
                                TokenM pointName = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                                PointDeclarationNode point = new PointDeclarationNode(pointName.Value);
                                instanciables.Add(new VariableDeclarationNode(pointName.Value, point, VariableType.Point));
                            }
                        }
                        else if (Match(TokenType.LineDeclaration))
                        {

                            Debug.Log(tokens[currentTokenIndex].Type);
                            if (Match(TokenType.LeftParenthesisToken))
                            {
                                TokenM pointName1 = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");
                                Consume(TokenType.CommaToken, "Expected ',' ");
                                TokenM pointName2 = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");

                                Consume(TokenType.RightParenthesisToken, "Expected ')' after line parameters.");

                                string lineName1 = pointName1.Value + pointName2.Value;
                                LineDeclarationNode lineNode = new LineDeclarationNode(pointName1.Value, pointName2.Value);
                                instanciables.Add(new VariableDeclarationNode(lineName1, lineNode, VariableType.Line));


                            }
                            else
                            {
                                throw new Exception("Bad line declaration");
                            }
                    
                        }
                    } while (Match(TokenType.CommaToken));

                    Consume(TokenType.RightBraceToken, "Expected '}' token");
                }
                
                return new DrawNode(instanciables);
            }
            else if (Match(TokenType.IfToken))
            {
                Node? condition = ParseExpression();
                if (condition == null)
                {
                    return null;
                }
                List<Node>? thenStatements = new List<Node>();

                if (Check(TokenType.LeftBraceToken))
                {
                    Consume(TokenType.LeftBraceToken, "Expected '{' after if condition.");
                    thenStatements = ParseBlock();
                    if (thenStatements == null)
                    {
                        return null;
                    }
                }
                else
                {
                    Node? exp = ParseExpression();
                    if (exp == null)
                    {
                        return null;
                    }
                    thenStatements.Add(exp);
                }

                List<Node>? elseStatements = null;

                if (Match(TokenType.ElseToken))
                {
                    if (Check(TokenType.LeftBraceToken))
                    {
                        Consume(TokenType.LeftBraceToken, "Expected '{' after else keyword.");
                        elseStatements = ParseBlock();
                        if (elseStatements == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        elseStatements = new List<Node>();
                        Node? exp = ParseExpression();
                        if (exp == null)
                        {
                            return null;
                        }
                        elseStatements.Add(exp);
                    }
                }
                return new IfNode(condition, thenStatements, elseStatements);

            }
            else if (Match(TokenType.WhileToken))
            {
                Node condition = ParseExpression();
                Consume(TokenType.LeftBraceToken, "Expected '{' after while condition.");
                List<Node> bodyStatements = ParseBlock();
                return new WhileNode(condition, bodyStatements);
            }
            else if (Match(TokenType.FunctionToken))
            {
                TokenM functionName = Consume(TokenType.IdentifierToken, "Expected function name after 'function' keyword.");
                Consume(TokenType.LeftParenthesisToken, "Expected '(' after function name.");
                List<VariableDeclarationNode> variables = new List<VariableDeclarationNode>();
                if (!Check(TokenType.RightParenthesisToken))
                {
                    do
                    {
                        Node expr = ParseExpression();

                        VariableType variableType = VariableType.Implicit;

                        if (Check(TokenType.TwoDotsToken))
                        {
                            Advance();
                            if (Check(TokenType.StringTypeToken))
                            {
                                variableType = VariableType.String;
                                Advance();
                            }
                            else if (Check(TokenType.NumberTypeToken))
                            {
                                variableType = VariableType.Number;
                                Advance();
                            }
                            else
                            {
                                throw new Exception("Expected variable type after : ");
                            }
                        }


                        if (Match(TokenType.AssignmentToken))
                        {
                            Node right = ParseExpression();

                            variables.Add(new VariableDeclarationNode(((VariableReferenceNode)expr).Name, right, variableType));
                        }
                        else
                        {
                            variables.Add(new VariableDeclarationNode(((VariableReferenceNode)expr).Name, null, variableType));
                        }



                    } while (Match(TokenType.CommaToken));
                }
                Consume(TokenType.RightParenthesisToken, "Expected ')' after function parameters.");
                if (Match(TokenType.ArrowToken))
                {
                    Node returnNode2 = ParseExpression();
                    Consume(TokenType.SemicolonToken, "Expected ';' after return statement.");
                    return new FunctionDeclarationNode(functionName.Value, variables, null, returnNode2);
                }


                Consume(TokenType.LeftBraceToken, "Expected '{' before function body.");



                List<Node> body = new List<Node>();
                Node returnNode = null;
                while (!Check(TokenType.RightBraceToken) && !IsAtEnd())
                {
                    if (Check(TokenType.ReturnToken))
                    {
                        Advance();
                        Node returnValue = ParseStatement();
                        Consume(TokenType.SemicolonToken, "Expected ';' after return statement.");
                        returnNode = returnValue;
                        break;
                    }
                    Node statement = ParseStatement();
                    if (statement != null)
                    {
                        body.Add(statement);
                    }
                }
                Consume(TokenType.RightBraceToken, "Expected '}' after block.");

                Console.WriteLine(returnNode);
                if (returnNode == null)
                {
                    throw new Exception("Expected return in a function declaration");
                }
                return new FunctionDeclarationNode(functionName.Value, variables, body, returnNode);
            }
            else if (Match(TokenType.LetToken))
            {
                List<Node> variables = new List<Node>();
                List<Node> initializers = new List<Node>();

                do
                {
                    Node expr = ParseExpression();

                    VariableType variableType = VariableType.Implicit;

                    if (Check(TokenType.TwoDotsToken))
                    {
                        Advance();
                        if (Check(TokenType.StringTypeToken))
                        {
                            variableType = VariableType.String;
                            Advance();
                        }
                        else if (Check(TokenType.NumberTypeToken))
                        {
                            variableType = VariableType.Number;
                            Advance();
                        }
                        else
                        {
                            throw new Exception("Expected variable type after : ");
                        }
                    }


                    if (Match(TokenType.AssignmentToken))
                    {
                        Node right = ParseExpression();

                        variables.Add(new VariableDeclarationNode(((VariableReferenceNode)expr).Name, right, variableType));
                    }
                    else
                    {
                        throw new Exception("Expected assignation value after variable");
                    }
                    /*
                    if (Match(TokenType.AssignmentToken))
                    {
                        Node initializer = ParseExpression();
                        initializers.Add(initializer);
                    }
                    */


                } while (Match(TokenType.CommaToken));

                Consume(TokenType.InToken, "Expected 'in' keyword after let bindings.");
                List<Node> body = new List<Node>();
                if (Match(TokenType.LeftBraceToken))
                {
                    body = ParseBlock();
                }
                else
                {
                    body.Add(ParseExpression());
                    Consume(TokenType.SemicolonToken, "Expected ';' after variable declaration.");
                }

                return new LetNode(variables, body);
            }
            else
            {
                Node expr = ParseExpression();
                if (Match(TokenType.AssignmentTwoDotsToken))
                {
                    Node right = ParseExpression();
                    Consume(TokenType.SemicolonToken, "Expected ';' after variable assignment.");
                    return new VariableAssignmentNode(((VariableReferenceNode)expr).Name, right);
                }
                else
                {
                    //Consume(TokenType.SemicolonToken, "Expected ';' after expression.");
                    return expr;
                }
            }
        }

        private List<Node> ParseBlock()
        {
            List<Node> statements = new List<Node>();
            while (!Check(TokenType.RightBraceToken) && !IsAtEnd())
            {
                Node statement = ParseStatement();
                if (statement != null)
                {
                    statements.Add(statement);
                }
            }
            Consume(TokenType.RightBraceToken, "Expected '}' after block.");
            return statements;
        }

        private Node ParseExpression()
        {
            return ParseBinaryExpression(0);
        }



        private Node ParseBinaryExpression(int minPrecedence)
        {
            Node left = ParseUnaryExpression();
            while (true)
            {
                TokenM operator1 = Peek();
                TokenType operatorType = operator1.Type;
                if (!precedence.ContainsKey(operatorType) || precedence[operatorType] < minPrecedence)
                {
                    break;
                }
                Advance();
                Node right = ParseBinaryExpression(precedence[operatorType] + 1);
                left = new BinaryExpressionNode(left, operator1, right);
            }
            return left;
        }

        private Node ParseUnaryExpression()
        {
            if (Match(TokenType.SubtractionToken))
            {
                TokenM op = Previous();
                Node expr = ParseUnaryExpression();
                return new UnaryExpressionNode(op, expr);
            }
            else if (Match(TokenType.LogicalNotToken))
            {
                TokenM op = Previous();
                Node expr = ParseUnaryExpression();
                return new UnaryExpressionNode(op, expr);
            }
            else
            {
                return ParsePrimaryExpression();
            }
        }

        private Node ParsePrimaryExpression()
        {

            if (Match(TokenType.IfToken))
            {
                Node? condition = ParseExpression();
                if (condition == null)
                {
                    return null;
                }
                List<Node>? thenStatements = new List<Node>();

                if (Check(TokenType.LeftBraceToken))
                {
                    Consume(TokenType.LeftBraceToken, "Expected '{' after if condition.");
                    thenStatements = ParseBlock();
                    if (thenStatements == null)
                    {
                        return null;
                    }
                }
                else
                {
                    Node? exp = ParseExpression();
                    if (exp == null)
                    {
                        return null;
                    }
                    thenStatements.Add(exp);
                }

                List<Node>? elseStatements = null;


                if (Match(TokenType.ElseToken))
                {
                    if (Check(TokenType.LeftBraceToken))
                    {
                        Consume(TokenType.LeftBraceToken, "Expected '{' after else keyword.");
                        elseStatements = ParseBlock();
                        if (elseStatements == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        elseStatements = new List<Node>();
                        Node? exp = ParseExpression();
                        if (exp == null)
                        {
                            return null;
                        }
                        elseStatements.Add(exp);
                    }

                }

                return new IfNode(condition, thenStatements, elseStatements);
            }
            else if (Match(TokenType.PointDeclaration))
            {
                TokenM pointName = Consume(TokenType.IdentifierToken, "Expected point name after 'point' keyword.");

                return new PointDeclarationNode(pointName.Value);
            }
            else if (Match(TokenType.LetToken))
            {
                List<Node> variables = new List<Node>();
                List<Node> initializers = new List<Node>();

                do
                {
                    Node expr = ParseExpression();

                    VariableType variableType = VariableType.Implicit;

                    if (Check(TokenType.TwoDotsToken))
                    {
                        Advance();
                        if (Check(TokenType.StringTypeToken))
                        {
                            variableType = VariableType.String;
                            Advance();
                        }
                        else if (Check(TokenType.NumberTypeToken))
                        {
                            variableType = VariableType.Number;
                            Advance();
                        }
                        else
                        {
                            throw new Exception("Expected variable type after : ");
                        }
                    }


                    if (Match(TokenType.AssignmentToken))
                    {
                        Node right = ParseExpression();

                        variables.Add(new VariableDeclarationNode(((VariableReferenceNode)expr).Name, right, variableType));
                    }
                    else
                    {
                        throw new Exception("Expected assignation value after variable");
                    }
                    /*
                    if (Match(TokenType.AssignmentToken))
                    {
                        Node initializer = ParseExpression();
                        initializers.Add(initializer);
                    }
                    */


                } while (Match(TokenType.CommaToken));

                Consume(TokenType.InToken, "Expected 'in' keyword after let bindings.");
                List<Node> body = new List<Node>();
                if (Match(TokenType.LeftBraceToken))
                {
                    body = ParseBlock();
                }
                else
                {
                    body.Add(ParseExpression());
                    //let as expresion for example in a variable declaration does not need ;
                    //Consume(TokenType.SemicolonToken, "Expected ';' after variable declaration.");
                }

                return new LetNode(variables, body);
            }
            else if (Match(TokenType.FalseToken))
            {
                return new BooleanNode(false);
            }
            else if (Match(TokenType.TrueToken))
            {
                return new BooleanNode(true);
            }
            else if (Match(TokenType.NumberToken))
            {
                return new NumberNode(double.Parse(Previous().Value));
            }
            else if (Match(TokenType.IdentifierToken))
            {
                TokenM identifier = Previous();
                VariableType variableType = VariableType.Implicit;
                if (Match(TokenType.LeftParenthesisToken))
                {
                    List<Node> arguments = new List<Node>();
                    if (!Check(TokenType.RightParenthesisToken))
                    {
                        do
                        {
                            Node argument = ParseStatement();
                            arguments.Add(argument);
                        } while (Match(TokenType.CommaToken));
                    }
                    Consume(TokenType.RightParenthesisToken, "Expected ')' after function arguments.");
                    return new FunctionCallNode(identifier.Value, arguments);
                }
                else if (Match(TokenType.AssignmentToken))
                {
                    if (Check(TokenType.PointDeclaration))
                    {
                        variableType = VariableType.Point;
                    }
                    else if (Check(TokenType.CircleDeclaration))
                    {
                        variableType = VariableType.Circle;
                    }
                    Node initializer = ParseStatement();
                    Consume(TokenType.SemicolonToken, "Expected ';' after variable declaration.");
                    return new VariableDeclarationNode(identifier.Value, initializer, variableType);
                }
                else if (Match(TokenType.CommaToken))
                {
                    int ind = this.currentTokenIndex;
                    List<TokenM> ids = new List<TokenM>();
                    ids.Add(identifier);

                    do
                    {
                        if (Match(TokenType.IdentifierToken))
                        {
                            TokenM identifier2 = Previous();
                            ids.Add(identifier2);
                        }
                        else
                        {
                            
                        }
                    } while (Match(TokenType.CommaToken));

                    if (Match(TokenType.AssignmentToken))
                    {
                        Node sequenceNode = ParseStatement();

                        return new MultipleVariableDeclarationNode(ids, sequenceNode);
                    }
                    else
                    {
                        this.currentTokenIndex=ind-1;
                        return new VariableReferenceNode(identifier.Value);
                    }
                    
                }
                else
                {
                    return new VariableReferenceNode(identifier.Value);
                }
            }
            else if (Match(TokenType.LeftParenthesisToken))
            {
                Node expr = ParseExpression();
                Consume(TokenType.RightParenthesisToken, "Expected ')' after expression.");
                return new GroupingNode(expr);
            }
            else if (Match(TokenType.StringToken))
            {
                return new StringNode(Previous().Value);
            }
            else
            {
                throw new Exception("Expected expression but get " + Peek().Type + " (" + Peek().Value + ")");
            }
        }

        private TokenM Consume(TokenType expectedType, string errorMessage)
        {
            if (Check(expectedType))
            {
                return Advance();
            }
            else
            {
                throw new Exception(errorMessage);
            }
        }

        private bool Match(TokenType type)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
            {
                return false;
            }
            else
            {
                return Peek().Type == type;
            }
        }

        private TokenM Advance()
        {
            if (!IsAtEnd())
            {
                currentTokenIndex++;
            }
            return Previous();
        }

        private bool IsAtEnd()
        {
            return currentTokenIndex >= tokens.Count - 1;
        }

        private TokenM Peek()
        {
            return tokens[currentTokenIndex];
        }

        private TokenM Previous()
        {
            return tokens[currentTokenIndex - 1];
        }
    }
}