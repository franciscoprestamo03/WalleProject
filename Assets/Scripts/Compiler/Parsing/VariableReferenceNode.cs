using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class VariableReferenceNode : Node
    {
        public string Name { get; }

        public VariableReferenceNode(string name)
        {
            Name = name;
        }

    }
}