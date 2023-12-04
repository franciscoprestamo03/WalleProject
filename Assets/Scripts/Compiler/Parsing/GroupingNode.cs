using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class GroupingNode : Node
    {
        public Node Expression { get; }

        public GroupingNode(Node expression)
        {
            Expression = expression;
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return Expression;
        }
    }
}