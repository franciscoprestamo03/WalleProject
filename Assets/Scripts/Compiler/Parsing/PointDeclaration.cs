using UnityEngine;

namespace Compiler
{
    public class PointDeclarationNode :Node
    {
        public string Name { get; }
        
        public int X { get; set; }
        public int Y { get; set; }

        public PointDeclarationNode( string name, int x,  int y)
        {
            Name = name;
            X = x;
            Y = y;
        }
        public PointDeclarationNode(string name)
        {
            Name = name;
            X = Random.Range(178, 1019);
            Y = Random.Range(5, 568);
        }
    }
}