

namespace Compiler
{
    public class NumberNode : Node
    {
        public double Value { get; set; }

        public NumberNode(double value)
        {
            this.Value = value;
        }

    }
}