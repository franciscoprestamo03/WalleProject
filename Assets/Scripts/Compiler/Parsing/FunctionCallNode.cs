using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class FunctionCallNode : Node
    {
        public string Name { get; }
        public List<Node> Arguments { get; }

        public FunctionCallNode(string name, List<Node> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public override IEnumerable<Node> GetChildren()
        {
            foreach (var argument in Arguments)
            {
                yield return argument;
            }
        }
    }
}