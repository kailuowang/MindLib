using System;
using log4net;

namespace MindHarbor.LogAdaptor {
    /// <summary>
    /// A log adaptor to abstract the log implementation
    /// </summary>
    /// <remarks>
    /// you still need the log configuration, the major benifits for this library is to faciliating you create your own logger wrapper
    /// </remarks>
    public class LogAdaptor : ILogger, IGeneratesLog {
        private readonly string name;

        public LogAdaptor(string name) {
            this.name = name;
        }

        public LogAdaptor() : this("MindHarbor.LogAdaptor") {}

        private ILog Logger {
            get { return LogManager.GetLogger(name); }
        }

        #region IGeneratesLog Members

        public event LogEventHandler LogGenerated;

        #endregion

        #region ILogger Members

        public void Log(string msg, LogLevel level) {
            switch (level) {
                case LogLevel.DEBUG:
                    Logger.Debug(msg);
                    break;
                case LogLevel.ERROR:
                    Logger.Error(msg);
                    break;
                case LogLevel.FATAL:
                    Logger.Fatal(msg);
                    break;
                case LogLevel.INFO:
                    Logger.Info(msg);
                    break;
                case LogLevel.WARN:
                    Logger.Warn(msg);
                    break;
                default:
                    throw new NotImplementedException("Log logic for level " + level + " is not implemented yet.");
            }
            if (LogGenerated != null)
                LogGenerated(this, new LogEventArgs(msg,level));
        }

        #endregion

    }
}