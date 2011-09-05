using System;

namespace MindHarbor.LogAdaptor {
    public enum LogLevel {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL
    }

    public class LogEventArgs : EventArgs {
        private readonly string message;
        private LogLevel level;

        public LogEventArgs(string message, LogLevel level){
            this.message = message;
            this.level = level;
        }

        public LogEventArgs(string message) : this(message, 0) {}

        public LogLevel Level{
            get { return level; }
            set { level = value; }
        }

        public string Message {
            get { return message; }
        }
    }

    public delegate void LogEventHandler(object sender, LogEventArgs arg);
}