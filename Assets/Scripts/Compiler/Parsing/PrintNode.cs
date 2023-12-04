using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class PrintNode : Node
    {
        public Node Expression { get; }

        public PrintNode(Node expression)
        {
            Expression = expression;
        }

       

        public override IEnumerable<Node> GetChildren()
        {
            yield return Expression;
        }
    }
}