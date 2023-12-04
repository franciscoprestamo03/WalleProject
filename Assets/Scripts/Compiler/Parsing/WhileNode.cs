using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class WhileNode : Node
    {
        public Node Condition { get; }
        public List<Node> BodyStatements { get; }

        public WhileNode(Node condition, List<Node> bodyStatements)
        {
            Condition = condition;
            BodyStatements = bodyStatements;
        }

    }
}