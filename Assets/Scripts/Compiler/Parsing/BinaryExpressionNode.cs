using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class BinaryExpressionNode : Node
    {
        public Node Left { get; set; }
        public TokenM Operator { get; set; }
        public Node Right { get; set; }

        public BinaryExpressionNode(Node left, TokenM op, Node right)
        {
            this.Left = left;
            this.Operator = op;
            this.Right = right;
        }


        public override IEnumerable<Node> GetChildren()
        {
            yield return Left;
            yield return Right;
        }
    }
}