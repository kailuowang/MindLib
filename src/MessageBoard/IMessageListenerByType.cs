using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.MessageBoard
{
    public interface IMessageListenerByType : IMessageListener
    {
        Type MessageType { get;}
    }
}
