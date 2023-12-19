using System.Collections.Generic;

namespace Compiler
{
    public class Undefined:Node
    {
        public Undefined()
        {

        }

        public override string ToString()
        {
            return "undefined";
        }
    }
}