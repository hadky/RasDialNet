using System;

namespace RasDialNet
{
    public interface ILogger
    {
        void Debug(Func<string> message);
        void Info(Func<string> message);
        void Warning(Func<string> message);
        void Error(string message, Exception exception);
    }
}