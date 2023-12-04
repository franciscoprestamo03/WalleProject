using System;
using System.Collections.Generic;


namespace Compiler
{
    public abstract class Node
    {


        public virtual IEnumerable<Node> GetChildren()
        {
            yield break;
        }

    }
}