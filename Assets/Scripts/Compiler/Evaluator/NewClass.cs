using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Compiler
{
    public class Evaluator
    {
        public Stack<Dictionary<string, object>> scopes;

        public Stack<Dictionary<string, FunctionDeclarationNode>> functionScopes;

        public Stack<Dictionary<string, Instaciable>> instanciablesScopes;

        public List<Instaciable> result;

        public string output;
        public int whileIterations;

        public int iterations;

        private int Counter;

        public Evaluator()
        {
            result = new List<Instaciable>();
            Counter = 0;
            whileIterations = 0;
            output = "";
            this.scopes = new Stack<Dictionary<string, object>>();
            this.scopes.Push(new Dictionary<string, object>());
            this.functionScopes = new Stack<Dictionary<string, FunctionDeclarationNode>>();
            this.functionScopes.Push(new Dictionary<string, FunctionDeclarationNode>());
            this.instanciablesScopes = new Stack<Dictionary<string, Instaciable>>();
            this.instanciablesScopes.Push(new Dictionary<string, Instaciable>());
        }

        public string EvaluateMain(MainProgramNode node)
        {
            output = "";
            iterations = 1;
            System.Console.WriteLine(iterations++);
            Evaluate(node);

            return output;
        }

        public object? Evaluate(Node node)
        {
            Counter++;
            if (Counter > 80)
            {
                throw new Exception("Hulk Stack Overflow");
            }

            Dictionary<string, FunctionDeclarationNode> scope;
            switch (node)
            {
                case MainProgramNode mainNode:
                    EnterScope();
                    object result = EvaluateBlock(mainNode.Body);
                    ExitScope();
                    Counter--;
                    return result;
                case SequenceNode sequenceNode:
                    return sequenceNode;
                case MultipleVariableDeclarationNode multipleVariableDeclarationNode:
                    int j = 0;
                    Debug.Log("fase 0");
                    Debug.Log(Evaluate(multipleVariableDeclarationNode.SequenceN));
                    SequenceNode sequence = (SequenceNode)Evaluate(multipleVariableDeclarationNode.SequenceN);
                    int prevIndex = sequence.Index;
                    foreach (var item2 in sequence)
                    {
                        object item;
                        if (item2 is Node node02)
                        {
                            item = Evaluate(node02);
                        }
                        else
                        {
                            item = item2;
                        }
                        
                        Debug.Log("fase 1  item = " + item + " type " + item.GetType());
                        if (j == multipleVariableDeclarationNode.VariableNames.Count - 1)
                        {
                            var seq = new SequenceNode(sequence.Elements, sequence.Type, sequence.Index - 1,
                                sequence.IsInfinite, sequence.Initial, sequence.End);
                            //seq.Index = sequence.Index + j;
                            Evaluate(new VariableDeclarationNode(multipleVariableDeclarationNode.VariableNames[j].Value,
                                seq));
                            break;
                        }
                        else if (item is int x)
                        {
                            Debug.Log("fase 2");
                            Evaluate(new VariableDeclarationNode(multipleVariableDeclarationNode.VariableNames[j].Value,
                                new NumberNode(x)));
                        }
                        else if (item is string s)
                        {
                            Debug.Log("fase 2");
                            Evaluate(new VariableDeclarationNode(multipleVariableDeclarationNode.VariableNames[j].Value,
                                new StringNode(s)));
                        }
                        else if (item is Point p)
                        {
                            Debug.Log($"fase 2 :::::::::::::  {p.X} {p.Y}");
                            instanciablesScopes.Peek().Add(multipleVariableDeclarationNode.VariableNames[j].Value, p);
                            Evaluate(new VariableDeclarationNode(multipleVariableDeclarationNode.VariableNames[j].Value,
                                new PointDeclarationNode(p.Name, new NumberNode(p.X), new NumberNode(p.Y))));
                        }else if( item is VariableReferenceNode varRef)
                        {
                            Debug.Log("::::::::::::::::::::::::::::::::::::::"+item.GetType());
                            Evaluate(new VariableDeclarationNode(multipleVariableDeclarationNode.VariableNames[j].Value, varRef));
                        }
                        else
                        {
                            Debug.Log("::::::::::::::::::::::::::::::::::::::"+item.GetType());
                        }

                        j++;
                    }

                    sequence.Index = prevIndex;
                    return null;
                case PointDeclarationNode pointDeclarationNode:
                    Counter--;

                    double x1 = (double)Evaluate(pointDeclarationNode.X);
                    double y1 = (double)Evaluate(pointDeclarationNode.Y);
                    
                    Debug.Log($" {pointDeclarationNode.Name}================================================================>>>>>> {Evaluate(pointDeclarationNode.X)}");
                    Debug.Log($"{pointDeclarationNode.Name}================================================================>>>>>> {Evaluate(pointDeclarationNode.Y)}");
                    
                    Point point = new Point(x1.ToString()+y1.ToString(),(int)x1 ,(int)y1);
                    string s1 = x1.ToString() + y1.ToString();
                    if (!instanciablesScopes.Peek().ContainsKey(s1))
                    {
                        instanciablesScopes.Peek().Add(x1.ToString()+y1.ToString(), point);
                    }
                    
                    return point;
                case CircleDeclarationNode circleDeclarationNode:
                    Counter--;
                    if (circleDeclarationNode.CenterRef is null)
                    {
                        Point center = null;
                        var instance03 = FindVariableScope(circleDeclarationNode.CenterStrRef);
                        if (instance03 is not null && instance03[circleDeclarationNode.CenterStrRef] is Point pointP0)
                        {
                            center = pointP0;
                            Debug.Log("circle node log");

                            Measure r = (Measure)Evaluate(circleDeclarationNode.Radius);

                            Circle circle = new Circle(circleDeclarationNode.Name, center, r.Value);

                            string circleName = "circle" + center.ToString() + r.Value.ToString();
                            if (!instanciablesScopes.Peek()
                                    .ContainsKey(circleName))
                            {
                                instanciablesScopes.Peek().Add(circleName, circle);
                            }
                            

                            return circle;
                        }
                        else
                        {
                            throw new Exception($"Point '{circleDeclarationNode.CenterStrRef}' doesen't exists.");
                        }
                    }
                    else
                    {
                        Point center = (Point)Evaluate(circleDeclarationNode.CenterRef);

                        double r = (double)Evaluate(circleDeclarationNode.Radius);

                        Circle circle = new Circle(circleDeclarationNode.Name, center, r);

                        instanciablesScopes.Peek().Add(circleDeclarationNode.Name, circle);

                        return circle;
                    }

                case LineDeclarationNode lineDeclarationNode:
                    Counter--;
                    Debug.Log("line declaration " + lineDeclarationNode.pointName1 + " " +
                              lineDeclarationNode.pointName2);
                    string lineName = lineDeclarationNode.pointName1 + lineDeclarationNode.pointName2;
                    if (instanciablesScopes.Peek().ContainsKey(lineName))
                    {
                        output += $"Line '{lineName}' already exists.";
                        throw new Exception($"Line '{lineName}' already exists.");
                    }

                    Point point4 = null;
                    Point point5 = null;
                    var SearchInstance1 = FindVariableScope(lineDeclarationNode.pointName1);

                    if (SearchInstance1 is not null && SearchInstance1[lineDeclarationNode.pointName1] is Point pointP1)
                    {
                        point4 = pointP1;
                        Debug.Log("LineNode log");
                    }
                    else
                    {
                    }

                    var SearchInstance2 = FindVariableScope(lineDeclarationNode.pointName1);
                    if (SearchInstance2 is not null && SearchInstance2[lineDeclarationNode.pointName2] is Point pointP2)
                    {
                        point5 = pointP2;
                        Debug.Log("LineNode log");
                    }
                    else
                    {
                        throw new Exception($"Point '{lineDeclarationNode.pointName2}' doesen't exists.");
                    }


                    Line line = new Line(lineName, point4, point5);

                    instanciablesScopes.Peek().Add(line.Name, line);

                    return line;
                case DrawNode drawNode:
                    Counter--;
                    Debug.Log("draw node log 00001");
                    foreach (var item in drawNode.Instanciables)
                    {
                        switch (item)
                        {
                            case VariableReferenceNode varRef:

                                Dictionary<string, object>? dic1 = FindVariableScope(varRef.Name);
                                Debug.Log(
                                    $"haciendo referencia ===============================> {varRef.Name} dictionary =====> {dic1 is null}");


                                if (dic1 is not null)
                                {
                                    Debug.Log("referencia " + varRef.Name);
                                    object val = dic1[varRef.Name];
                                    Debug.Log("hey hello");
                                    if (val is Line lineA)
                                    {
                                        this.result.Add(lineA);
                                    }
                                    else if (val is Point pointA)
                                    {
                                        this.result.Add(pointA);
                                    }
                                    else if (val is Circle c)
                                    {
                                        Debug.Log($"dibujando circulo {c.Name}  {c.Center} {c.Radius}");
                                        this.result.Add(c);
                                    }

                                    break;
                                }

                                Dictionary<string, Instaciable>? dic = FindINstanciableScope(varRef.Name);
                                if (dic is null)
                                {
                                    throw new Exception("Hay algo mal ");
                                }

                                Instaciable instance = dic[varRef.Name];

                                if (instance.Type == InstanciableType.point)
                                {
                                    Point point2 = (Point)instance;
                                    Debug.Log("drawNode log");
                                    this.result.Add(point2);
                                }
                                else if (instance.Type == InstanciableType.circle)
                                {
                                    Circle circle1 = (Circle)instance;
                                    Point point2 = circle1.Center;
                                    this.result.Add(point2);
                                    this.result.Add(circle1);
                                }


                                break;
                            case VariableDeclarationNode variableDeclarationNode:
                                Debug.Log("draw node log 00002");
                                switch (variableDeclarationNode.VarType)
                                {
                                    case VariableType.Point:
                                        Point point01 = (Point)Evaluate(variableDeclarationNode.Initializer);
                                        this.result.Add(point01);
                                        break;
                                    case VariableType.Line:
                                        Line line01 = (Line)Evaluate(variableDeclarationNode.Initializer);
                                        this.result.Add(line01);
                                        break;


                                    default:
                                        break;
                                }

                                break;
                            default:

                                break;
                        }
                    }

                    return null;

                case BinaryExpressionNode binaryExpressionNode:
                    Counter--;
                    return EvaluateBinaryExpression(binaryExpressionNode);
                case UnaryExpressionNode unaryExpressionNode:
                    Counter--;
                    return EvaluateUnaryExpression(unaryExpressionNode);
                case GroupingNode groupingNode:
                    Counter--;
                    return Evaluate(groupingNode.Expression);
                case NumberNode numberNode:
                    Counter--;
                    return numberNode.Value;
                case StringNode stringNode:
                    Counter--;
                    return stringNode.Value;
                case BooleanNode booleanNode:
                    Counter--;
                    return booleanNode.Value;
                case FunctionDeclarationNode functionDeclarationNode:
                    if (functionScopes.Peek().ContainsKey(functionDeclarationNode.Name))
                    {
                        output += $"Function '{functionDeclarationNode.Name}' already exists.";
                        throw new Exception($"Function '{functionDeclarationNode.Name}' already exists.");
                    }
                    else
                    {
                        functionScopes.Peek().Add(functionDeclarationNode.Name, functionDeclarationNode);
                        Counter--;
                        return null;
                    }
                case FunctionCallNode functionCallNode:
                    if (functionCallNode.Name == "sin")
                    {
                        var argument1 = Evaluate(functionCallNode.Arguments[0]);
                        if (argument1 is Double)
                        {
                            Counter--;
                            return Math.Sin((double)argument1);
                        }
                        else
                        {
                            output += $"Function sen(x) receive a double not a {argument1.GetType()}";
                            throw new Exception($"Function sen(x) receive a double not a {argument1.GetType()}");
                        }
                    }
                    else if (functionCallNode.Name == "cos")
                    {
                        var argument1 = Evaluate(functionCallNode.Arguments[0]);
                        if (argument1 is Double)
                        {
                            Counter--;
                            return Math.Cos((double)argument1);
                        }
                        else
                        {
                            output += $"Function cos(x) receive a double not a {argument1.GetType()}";
                            throw new Exception($"Function cos(x) receive a double not a {argument1.GetType()}");
                        }
                    }
                    else if (functionCallNode.Name == "print")
                    {
                        System.Console.WriteLine("Function print");
                        object? value2 = null;
                        foreach (Node argument in functionCallNode.Arguments)
                        {
                            value2 = Evaluate(argument);
                            Console.WriteLine("value => " + value2);
                            Debug.Log("printing =====> " + value2);
                            output += $"{value2}";
                        }

                        System.Console.WriteLine("Out Function print");
                        Counter--;
                        return value2;
                    }
                    else if (functionCallNode.Name == "printLine")
                    {
                        System.Console.WriteLine("Function printLine");
                        object? value3 = null;
                        foreach (Node argument in functionCallNode.Arguments)
                        {
                            value3 = Evaluate(argument);
                            Console.WriteLine("value => " + value3);
                            output += $"{value3}\n";
                        }

                        Counter--;
                        return value3;
                    }
                    else if (functionCallNode.Name == "intersect")
                    {
                        Debug.Log("Intersecciones 1 =======================================================");
                        if (functionCallNode.Arguments.Count == 2)
                        {
                            var a1 = Evaluate(functionCallNode.Arguments[0]);
                            var a2 = Evaluate(functionCallNode.Arguments[1]);
                            Debug.Log("Intersecciones 2");

                            if (a1 is Circle circle1)
                            {
                                if (a2 is Circle circle2)
                                {
                                    List<Point> intersections = FindIntersectionPoints(circle1.Center.X,
                                        circle1.Center.Y, circle1.Radius, circle2.Center.X, circle2.Center.Y,
                                        circle2.Radius);
                                    Debug.Log("Intersecciones 3");
                                    List<Node> nodesB = new List<Node>();
                                    foreach (var item in intersections)
                                    {
                                        
                                        nodesB.Add(new PointDeclarationNode(item.Name,new NumberNode(item.X),new NumberNode(item.Y)));
                                    }
                                    return new SequenceNode(nodesB, "intersect");
                                }
                            }

                            throw new Exception("Bad Intersect declaration");
                        }
                        else
                        {
                            throw new Exception("Bad Intersect declaration");
                        }
                    }
                    else if (functionCallNode.Name == "measure")
                    {
                        if (functionCallNode.Arguments.Count == 2)
                        {
                            if (functionCallNode.Arguments[0] is VariableReferenceNode pointBRef)
                            {
                                if (functionCallNode.Arguments[1] is VariableReferenceNode pointCRef)
                                {
                                    var searchPoint1 = FindVariableScope(pointBRef.Name);
                                    if (searchPoint1 is null)
                                    {
                                        throw new Exception($"{pointBRef.Name} doesn't exist");
                                    }

                                    if (searchPoint1[pointBRef.Name] is Point pointB)
                                    {
                                        var searchPoint2 = FindVariableScope(pointCRef.Name);
                                        if (searchPoint2 is null)
                                        {
                                            throw new Exception($"{pointCRef.Name} doesn't exist");
                                        }

                                        if (searchPoint2[pointCRef.Name] is Point pointC)
                                        {
                                            double m = Math.Sqrt((pointB.X - pointC.X) * (pointB.X - pointC.X) +
                                                                 (pointB.Y - pointC.Y) * (pointB.Y - pointC.Y));
                                            Debug.Log($"medida es igual a {m}");
                                            return new Measure(m);
                                        }
                                        else
                                        {
                                            throw new Exception($"{pointCRef.Name} isent't a Point");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"{pointBRef.Name} isn't a Point");
                                    }
                                }
                                else
                                {
                                    throw new Exception($"Bad 2nd argument declaration in measure function");
                                }
                            }
                            else
                            {
                                throw new Exception($"Bad 1st argument declaration in measure function");
                            }
                        }
                        else
                        {
                            output += $"Function measure(x,y) receive two arguments";
                            throw new Exception($"Function measure(x,y) receive two arguments");
                        }
                    }
                    else if ((scope = FindFuntionScope(functionCallNode.Name)) != null)
                    {
                        FunctionDeclarationNode function = scope[functionCallNode.Name];

                        EnterScope();

                        if (function.Parameters.Count != functionCallNode.Arguments.Count)
                        {
                            throw new Exception($"Function requires exact {function.Parameters.Count} arguments");
                        }

                        for (int i = 0; i < function.Parameters.Count; i++)
                        {
                            VariableDeclarationNode variableDeclaration = function.Parameters[i];
                            variableDeclaration.Initializer = functionCallNode.Arguments[i];

                            Evaluate(variableDeclaration);
                        }

                        //object? result4 = function.Body != null ? EvaluateBlock(function.Body) : null;
                        if (function.Body is not null)
                        {
                            foreach (var item in function.Body)
                            {
                                Debug.Log($"====================================.......................................{item}");
                                Evaluate(item);
                            }
                        }

                        object? result5 = Evaluate(function.ReturnNode);

                        ExitScope();
                        Counter--;
                        return result5;
                    }
                    else
                    {
                        output += $"Function '{functionCallNode.Name}' does not exist.";
                        throw new Exception($"Function '{functionCallNode.Name}' does not exist.");
                    }
                case VariableReferenceNode variableReferenceNode:
                    if (FindVariableScope(variableReferenceNode.Name) != null)
                    {
                        Counter--;
                        return FindVariableScope(variableReferenceNode.Name)[variableReferenceNode.Name];
                    }
                    else
                    {
                        output += $"Variable '{variableReferenceNode.Name}' does not exist.";
                        throw new Exception($"Variable '{variableReferenceNode.Name}' does not exist.");
                    }
                case VariableDeclarationNode variableDeclarationNode:
                    Debug.Log(variableDeclarationNode.Name);
                    Debug.Log(variableDeclarationNode.Initializer.GetType());


                    if (scopes.Peek().ContainsKey(variableDeclarationNode.Name))
                    {
                        output += $"Variable '{variableDeclarationNode.Name}' already exists.";
                        throw new Exception($"Variable '{variableDeclarationNode.Name}' already exists.");
                    }
                    else
                    {
                        object initialValue = Evaluate(variableDeclarationNode.Initializer);

                        //type cheking
                        Console.WriteLine($"verificando valor {initialValue.GetType()} {initialValue}");

                        Debug.Log("done");
                        scopes.Peek().Add(variableDeclarationNode.Name, initialValue);
                        Counter--;
                        return initialValue;
                    }
                case VariableAssignmentNode variableAssignmentNode:
                    if (FindVariableScope(variableAssignmentNode.Name) != null)
                    {
                        object? valueToAssign = Evaluate(variableAssignmentNode.Value);
                        FindVariableScope(variableAssignmentNode.Name)[variableAssignmentNode.Name] = valueToAssign;
                        Counter--;
                        return null;
                    }
                    else
                    {
                        output += $"Variable '{variableAssignmentNode.Name}' does not exist.";
                        throw new Exception($"Variable '{variableAssignmentNode.Name}' does not exist.");
                    }

                case IfNode ifNode:
                    object? conditionValue = Evaluate(ifNode.Condition);
                    if (conditionValue is bool conditionBool && conditionBool)
                    {
                        EnterScope();
                        object result1 = EvaluateBlock(ifNode.ThenStatements);
                        ExitScope();
                        Counter--;
                        return result1;
                    }
                    else if (ifNode.ElseStatements != null)
                    {
                        EnterScope();
                        object result2 = EvaluateBlock(ifNode.ElseStatements);
                        ExitScope();
                        Counter--;
                        return result2;
                    }
                    else
                    {
                        Counter--;
                        return null;
                    }
                case WhileNode whileNode:
                    object? loopConditionValue = Evaluate(whileNode.Condition);
                    while (loopConditionValue is bool loopConditionBool && loopConditionBool)
                    {
                        if (whileIterations == 100000)
                        {
                            throw new Exception("Hulk Stack overflow");
                        }
                        else
                        {
                            whileIterations++;
                        }

                        EnterScope();
                        System.Console.WriteLine("before " + iterations++);
                        EvaluateBlock(whileNode.BodyStatements);
                        System.Console.WriteLine("after " + iterations++);
                        ExitScope();
                        loopConditionValue = Evaluate(whileNode.Condition);
                    }

                    whileIterations = 0;
                    Counter--;
                    return null;
                case LetNode letNode:
                    EnterScope();
                    foreach (VariableDeclarationNode variableDeclaration in letNode.VarDeclarations)
                    {
                        Evaluate(variableDeclaration);
                    }

                    object result3 = EvaluateBlock(letNode.Body);
                    ExitScope();
                    Counter--;
                    return result3;
                default:
                    throw new Exception($"Unhandled node type: {node}");
            }
        }


        private Dictionary<string, FunctionDeclarationNode> FindFuntionScope(string functionName)
        {
            foreach (Dictionary<string, FunctionDeclarationNode> scope in functionScopes)
            {
                if (scope.ContainsKey(functionName))
                {
                    return scope;
                }
            }

            return null;
        }

        private Dictionary<string, object> FindVariableScope(string variableName)
        {
            foreach (Dictionary<string, object> scope in scopes)
            {
                if (scope.ContainsKey(variableName))
                {
                    return scope;
                }
            }

            // If the variable is not found in the local scope,
            // recursively search in the parent scope (the previous scope in the stack).
            // if (scopes.Count > 1)
            // {
            //     scopes.Pop();
            //     Dictionary<string, object> parentScope = FindVariableScope(variableName);
            //     scopes.Push(parentScope);
            //     return parentScope;
            // }

            return null;
        }

        private Dictionary<string, Instaciable> FindINstanciableScope(string variableName)
        {
            foreach (Dictionary<string, Instaciable> scope in instanciablesScopes)
            {
                if (scope.ContainsKey(variableName))
                {
                    return scope;
                }
            }

            return null;
        }

        private void EnterScope()
        {
            System.Console.WriteLine($"Entering scope {scopes.Count}");
            this.scopes.Push(new Dictionary<string, object>());
            //se puede eliminar para que todas las funciones que se declaren sean globales
            this.functionScopes.Push(new Dictionary<string, FunctionDeclarationNode>());

            this.instanciablesScopes.Push(new Dictionary<string, Instaciable>());
        }

        private void ExitScope()
        {
            this.scopes.Pop();
            this.functionScopes.Pop();
            this.instanciablesScopes.Pop();
            System.Console.WriteLine($"Exit scope {scopes.Count}");
        }

        private object EvaluateBinaryExpression(BinaryExpressionNode node)
        {
            object left = Evaluate(node.Left);
            object right = Evaluate(node.Right);

            switch (node.Operator.Type)
            {
                case TokenType.AdditionToken:
                    if (left is double leftDouble && right is double rightDouble)
                    {
                        return leftDouble + rightDouble;
                    }
                    else if (left is string leftString && right is string rightString)
                    {
                        return leftString + rightString;
                    }
                    else
                    {
                        output += $"Cannot add {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot add {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.SubtractionToken:
                    if (left is double leftDoubleSub && right is double rightDoubleSub)
                    {
                        return leftDoubleSub - rightDoubleSub;
                    }
                    else
                    {
                        output += $"Cannot subtract {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot subtract {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.MultiplicationToken:
                    if (left is double leftDoubleMul1 && right is Measure mesR1)
                    {
                        int a = (int)mesR1.Value;
                        return new Measure(a * leftDoubleMul1);
                    }
                    else if (left is Measure mesL1 && right is double rightDoubleMul1)
                    {
                        int a = (int)mesL1.Value;
                        return new Measure(a * rightDoubleMul1);
                    }
                    else if (left is Measure mesL2 && right is Measure mesR2)
                    {
                        int a = (int)mesL2.Value;
                        int b = (int)mesR2.Value;
                        return new Measure(a * b);
                    }
                    else if (left is double leftDoubleMul && right is double rightDoubleMul)
                    {
                        return leftDoubleMul * rightDoubleMul;
                    }
                    else
                    {
                        output += $"Cannot multiply {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot multiply {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.DivisionToken:
                    if (left is double leftDoubleDiv && right is double rightDoubleDiv)
                    {
                        return leftDoubleDiv / rightDoubleDiv;
                    }
                    else
                    {
                        output += $"Cannot divide {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot divide {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.ArrobaToken:
                    if (left != null && right != null)
                    {
                        return left.ToString() + right.ToString();
                    }
                    else
                    {
                        throw new Exception($"Cannot concatenate {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.ModulusToken:
                    if (left is double leftDoubleMod && right is double rightDoubleMod)
                    {
                        return leftDoubleMod % rightDoubleMod;
                    }
                    else
                    {
                        throw new Exception(
                            $"Cannot apply modulus to {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.PowToken:
                    if (left is double leftDoublePow && right is double rightDoublePow)
                    {
                        return Math.Pow(leftDoublePow, rightDoublePow);
                    }
                    else
                    {
                        throw new Exception(
                            $"Cannot apply exponenciation to {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.EqualityToken:
                    return left.Equals(right);
                case TokenType.InequalityToken:
                    return !left.Equals(right);
                case TokenType.LessThanToken:
                    if (left is double leftDoubleLT && right is double rightDoubleLT)
                    {
                        return leftDoubleLT < rightDoubleLT;
                    }
                    else
                    {
                        output += $"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.LessThanOrEqualToken:
                    if (left is double leftDoubleLTE && right is double rightDoubleLTE)
                    {
                        return leftDoubleLTE <= rightDoubleLTE;
                    }
                    else
                    {
                        output += $"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.GreaterThanToken:
                    if (left is double leftDoubleGT && right is double rightDoubleGT)
                    {
                        return leftDoubleGT > rightDoubleGT;
                    }
                    else
                    {
                        output += $"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.GreaterThanOrEqualToken:
                    if (left is double leftDoubleGTE && right is double rightDoubleGTE)
                    {
                        return leftDoubleGTE >= rightDoubleGTE;
                    }
                    else
                    {
                        output += $"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception($"Cannot compare {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.LogicalAndToken:
                    if (left is bool leftBool && right is bool rightBool)
                    {
                        return leftBool && rightBool;
                    }
                    else
                    {
                        output += $"Cannot perform logical AND on {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception(
                            $"Cannot perform logical AND on {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                case TokenType.LogicalOrToken:
                    if (left is bool leftBoolOr && right is bool rightBoolOr)
                    {
                        return leftBoolOr || rightBoolOr;
                    }
                    else
                    {
                        output += $"Cannot perform logical OR on {left?.GetType().Name} and {right?.GetType().Name}";
                        throw new Exception(
                            $"Cannot perform logical OR on {left?.GetType().Name} and {right?.GetType().Name}");
                    }
                default:
                    output += $"Unknown binary operator {node.Operator.Value}";
                    throw new Exception($"Unknown binary operator {node.Operator.Value}");
            }
        }

        private object EvaluateUnaryExpression(UnaryExpressionNode node)
        {
            object operand = Evaluate(node.Expression);

            switch (node.Operator.Type)
            {
                case TokenType.SubtractionToken:
                    if (operand is double operandDouble)
                    {
                        return -operandDouble;
                    }
                    else
                    {
                        output += $"Cannot negate {operand?.GetType().Name}";
                        throw new Exception($"Cannot negate {operand?.GetType().Name}");
                    }
                case TokenType.LogicalNotToken:
                    if (operand is bool operandBool)
                    {
                        return !operandBool;
                    }
                    else
                    {
                        output += $"Cannot perform logical NOT on {operand?.GetType().Name}";
                        throw new Exception($"Cannot perform logical NOT on {operand?.GetType().Name}");
                    }
                default:
                    throw new Exception($"Unknown unary operator {node.Operator.Value}");
            }
        }

        private object EvaluateBlock(List<Node> statements)
        {
            object result = null;
            foreach (Node statement in statements)
            {
                result = Evaluate(statement);
            }

            return result;
        }

        public static List<Point> FindIntersectionPoints(double x1, double y1, double r1, double x2, double y2,
            double r2)
        {
            List<Point> intersections = new List<Point>();

            // Calculamos la distancia entre los centros de las circunferencias
            double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            // Verificamos si las circunferencias no se intersectan
            if (distance > r1 + r2 || distance < Math.Abs(r1 - r2))
            {
                return intersections;
            }

            // Calculamos las coordenadas de los puntos de intersección
            double a = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(distance, 2)) / (2 * distance);
            double h = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(a, 2));
            double x3 = x1 + (a / distance) * (x2 - x1);
            double y3 = y1 + (a / distance) * (y2 - y1);
            double x4 = x3 + (h / distance) * (y2 - y1);
            double y4 = y3 - (h / distance) * (x2 - x1);
            double x5 = x3 - (h / distance) * (y2 - y1);
            double y5 = y3 + (h / distance) * (x2 - x1);


            // Agregamos los puntos de intersección a la lista
            intersections.Add(new Point(x5.ToString() + y5.ToString(), (int)x5, (int)y5));
            intersections.Add(new Point(x4.ToString() + y4.ToString(), (int)x4, (int)y4));

            return intersections;
        }
    }
}