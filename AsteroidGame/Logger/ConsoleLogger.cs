using System;
using System.Diagnostics;

namespace AsteroidGame.Logger
{
    internal class ConsoleLogger : Logger
    {
        public override void Log(string Message)
        {
            Console.WriteLine(Message);
            Debug.WriteLine(Message);
        }
    }
}
