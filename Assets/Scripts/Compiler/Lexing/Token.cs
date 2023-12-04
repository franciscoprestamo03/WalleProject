namespace Compiler
{
    public class TokenM
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public TokenM(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }   
    }
}