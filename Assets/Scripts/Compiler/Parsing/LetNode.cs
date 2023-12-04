using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class LetNode : Node
    {
        public List<Node> VarDeclarations { get; }
        public List<Node> Body { get; }


        public LetNode(Node varDeclaration, List<Node> body)
        {
            VarDeclarations = new List<Node>();
            VarDeclarations.Add(varDeclaration);
            Body = body;
        }

        public LetNode(List<Node> varDeclarations, List<Node> body)
        {
            VarDeclarations = varDeclarations;
            Body = body;
        }
    }
}