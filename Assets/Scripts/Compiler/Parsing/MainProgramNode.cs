using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compiler
{
    public class MainProgramNode : Node
    {
        public List<Node> Body { get; }


        public MainProgramNode(List<Node> body)
        {

            Body = body;
        }
    }
}