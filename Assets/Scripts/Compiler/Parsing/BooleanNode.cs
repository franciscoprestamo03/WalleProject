namespace Compiler
{
    public class BooleanNode : Node
    {
        public bool Value { get; }

        public BooleanNode(bool value)
        {
            Value = value;
        }


    }
}