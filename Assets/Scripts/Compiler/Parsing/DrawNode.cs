using System.Collections;
using System.Collections.Generic;

namespace Compiler
{
    public class DrawNode : Node
    {
        public List<Node> Instanciables { get; }

        public DrawNode(List<Node> instanciables)
        {
            Instanciables = instanciables;
        }



        public override IEnumerable<Node> GetChildren()
        {
            foreach(Node intan in Instanciables)
            {
                yield return intan;
            }
            
        }
    }
}