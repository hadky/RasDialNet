using System;

namespace RasDialNet
{
    public class ConsoleLogger : ILogger
    {
        private readonly object _lock = new object();

        public void Debug(Func<string> message)
        {
            WriteToConsole("D " + message(), ConsoleColor.Gray);
        }

        public void Info(Func<string> message)
        {
            WriteToConsole("I " + message(), ConsoleColor.White);
        }

        public void Warning(Func<string> message)
        {
            WriteToConsole("W " + message(), ConsoleColor.Yellow);
        }

        public void Error(string message, Exception exception)
        {
            WriteToConsole("E " + message + ", Exception Details:\r\n" + exception, ConsoleColor.Red);
        }

        private void WriteToConsole(string message, ConsoleColor color)
        {
            lock (_lock)
            {
                var currentColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(DateTime.Now.ToString("s") + ": " + message);
                }
                finally
                {
                    Console.ForegroundColor = currentColor;
                }
            }
        }
    }
}