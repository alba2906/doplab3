using Antlr4.Runtime;
using Laba1.Models;
using System.Collections.Generic;
using System.IO;

namespace Laba1
{
    public class LexerErrorListener : IAntlrErrorListener<int>
    {
        public List<ErrorInfo> Errors { get; } = new();

        public void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            int offendingSymbol,
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