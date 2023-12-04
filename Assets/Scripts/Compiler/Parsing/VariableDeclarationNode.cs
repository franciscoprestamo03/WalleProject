using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class VariableDeclarationNode : Node
    {
        public string Name { get; }
        public Node? Initializer;

        public VariableType VarType { get; }

        public VariableDeclarationNode(string name, Node? initializer, VariableType type = VariableType.Implicit)
        {
            Name = name;
            Initializer = initializer;
            VarType = type;
        }



        public override IEnumerable<Node> GetChildren()
        {
            yield return Initializer;
        }
    }
}