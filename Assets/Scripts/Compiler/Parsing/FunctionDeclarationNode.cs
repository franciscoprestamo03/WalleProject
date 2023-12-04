using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class FunctionDeclarationNode : Node
    {
        public string Name { get; }
        public List<VariableDeclarationNode> Parameters;
        public List<Node>? Body { get; }
        public Node? ReturnNode { get; }

        public FunctionDeclarationNode(string name, List<VariableDeclarationNode> parameters, List<Node>? body, Node? returnNode)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
            ReturnNode = returnNode;
        }

    }
}