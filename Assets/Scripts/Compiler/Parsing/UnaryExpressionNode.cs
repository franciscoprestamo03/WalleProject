using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class UnaryExpressionNode : Node
    {
        public TokenM Operator { get; set; }
        public Node Expression { get; set; }

        public UnaryExpressionNode(TokenM op, Node expr)
        {
            this.Operator = op;
            this.Expression = expr;
        }

        public override IEnumerable<Node> GetChildren()
        {
            yield return Expression;
        }
    }
}