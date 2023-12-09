using UnityEngine;

namespace Compiler
{
    public class LineDeclarationNode : Node
    {

        public string pointName1 { get; }
        public string pointName2 { get; }
        public bool IsSegment { get; }

        public LineDeclarationNode(string pointA, string pointB,bool isSegment = false)
        {
            pointName1 = pointA;
            pointName2 = pointB;
            IsSegment = isSegment;
        }
    }
}