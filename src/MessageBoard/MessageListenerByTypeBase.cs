using System;
using System.Collections.Generic;
using System.Text;

namespace MindHarbor.MessageBoard
{
    public abstract class MessageListenerByTypeBase<MessageT> : IMessageListenerByType where MessageT : IMessage
    {
        public Type MessageType
        {
            get { return typeof (MessageT); }
        }

        public void Listen(IMessage msg)
        {
            Listen((MessageT) msg);   
        }
        protected abstract void Listen(MessageT msg);
    }
}
