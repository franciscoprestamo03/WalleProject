using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class ConditionalExpressionNode : Node
    {
        public Node Condition { get; }
        public Node ThenExpr { get; }
        public Node ElseExpr { get; }

        public ConditionalExpressionNode(Node condition, Node thenExpr, Node elseExpr)
        {
            Condition = condition;
            ThenExpr = thenExpr;
            ElseExpr = elseExpr;
        }


        public override IEnumerable<Node> GetChildren()
        {
            yield return Condition;
            yield return ThenExpr;
            yield return ElseExpr;
        }
    }
}