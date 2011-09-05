using System;
using System.Collections.Generic;
using System.Text;
using MindHarbor.LogAdaptor;

namespace MindHarbor.LogAdaptor {
    public interface IGeneratesLog{
        event LogEventHandler LogGenerated;
    }
}