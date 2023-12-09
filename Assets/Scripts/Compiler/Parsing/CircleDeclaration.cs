using UnityEngine;

namespace Compiler
{
    public class CircleDeclarationNode : Node
    {
        public string Name { get; }
        public PointDeclarationNode CenterRef { get; set; }
        public Node Radius { get; }
        public string CenterStrRef { get; }


        public CircleDeclarationNode(string name, string pointRef, Node radius)
        {
            Name = name;
            CenterStrRef = pointRef;
            Radius = radius;
            CenterRef = null;
        }
        public CircleDeclarationNode(string name, Point pointRef, Node radius)
        {
            Name = name;
            CenterRef = new PointDeclarationNode(pointRef.Name, new NumberNode(pointRef.X), new NumberNode(pointRef.Y));
            Radius = radius;
            CenterRef = null;
        }
        public CircleDeclarationNode(string name)
        {
            Name = name;
            CenterRef = new PointDeclarationNode("center"+name);
            Radius = new NumberNode(Random.Range(20, 40));
        }
    }
}