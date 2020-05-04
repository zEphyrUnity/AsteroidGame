using System;
using System.IO;

namespace AsteroidGame.Logger
{
    internal class TextFileLogger : Logger
    {
        private readonly TextWriter _Writer;

        public TextFileLogger(string FileName)
        {
            _Writer = File.CreateText(FileName);
        }

        private int _Counter;
        public override void Log(string Message)
        {
            _Writer.WriteLine("{0}>{1}", _Counter++, Message);
        }

        public override void Flush()
        {
            _Writer.Flush();
        }
    }
}
