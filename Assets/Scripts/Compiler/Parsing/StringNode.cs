namespace Compiler
{
    public class StringNode : Node
    {
        public string Value { get; }

        public StringNode(string value)
        {
            this.Value = value;
        }

    }
}