namespace Laba1.Models
{
    public class TokenInfo
    {
        public int Number { get; set; }
        public string Lexeme { get; set; } = "";
        public string TokenType { get; set; } = "";
        public string Description { get; set; } = "";
        public int Line { get; set; }
        public int Column { get; set; }
    }
}