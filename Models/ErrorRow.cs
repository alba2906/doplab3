namespace Laba1.Models
{
    public class ErrorRow
    {
        public string ErrorType { get; set; } = "";
        public string Message { get; set; } = "";
        public int Line { get; set; }
        public int Column { get; set; }
    }
}