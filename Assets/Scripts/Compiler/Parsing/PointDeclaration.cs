using UnityEngine;

namespace Compiler
{
    public class PointDeclarationNode :Node
    {
        public string Name { get; }
        
        public Node X { get; set; }
        public Node Y { get; set; }

        public PointDeclarationNode( string name, Node x,  Node y)
        {
            Name = name;
            X = x;
            Y = y;
        }
        public PointDeclarationNode(string name)
        {
            Name = name;
            X = new NumberNode(Random.Range(178, 1019));
            Y = new NumberNode(Random.Range(5, 568));
        }
    }
}