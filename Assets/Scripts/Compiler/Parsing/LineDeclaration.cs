using UnityEngine;

namespace Compiler
{
    public class LineDeclarationNode : Node
    {

        public string pointName1 { get; }
        public string pointName2 { get; }

        public LineDeclarationNode(string pointA, string pointB)
        {
            pointName1 = pointA;
            pointName2 = pointB;
        }
    }
}