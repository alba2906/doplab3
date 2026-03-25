using Antlr4.Runtime;
using Laba1.Models;
using System.Collections.Generic;
using System.IO;

namespace Laba1
{
    public class CollectingErrorListener : BaseErrorListener
    {
        public List<ErrorInfo> Errors { get; } = new();

        public override void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            Errors.Add(new ErrorInfo
            {
                Line = line,
                Column = charPositionInLine,
                Message = msg
            });
        }
    }
}