namespace Laba1.Models
{
    public class AnalysisRow
    {
        public int Number { get; set; }
        public string Lexeme { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int Line { get; set; }
        public int Column { get; set; }
    }
}