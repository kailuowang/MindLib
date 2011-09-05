using NUnit.Framework;

namespace MindHarbor.MessageBoard.Test
{
    [TestFixture]
    public class MessageListenerByTypeFixture
    {
       
        [Test]
        public void BasicTest()
        {
            IMessageBroadcaster broadcaster = new MessageBroadcasterImpl();
            MockListener l = new MockListener();
            broadcaster.Subscribe(l);
            broadcaster.Broadcast(new MockMessage());
            broadcaster.Broadcast(new MockMessage2());

        }



        public class MockListener : MessageListenerByTypeBase<MockMessage>
        {
            protected override void Listen(MockMessage msg)
            {
               
            }
        }

        public class MockMessage :IMessage
        {
        }
        public class MockMessage2 :IMessage
        {
        }
    }


}