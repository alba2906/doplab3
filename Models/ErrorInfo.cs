namespace Laba1.Models
{
    public class ErrorInfo
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}