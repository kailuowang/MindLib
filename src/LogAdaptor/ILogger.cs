using System;
using System.Collections.Generic;
using System.Text;
using MindHarbor.LogAdaptor;

namespace MindHarbor.LogAdaptor {
    public interface ILogger{
        void Log(string msg, LogLevel level);
    }
}