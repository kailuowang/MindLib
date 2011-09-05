using System;
using System.Collections.Generic;
using MindHarbor.GenClassLib.MessageBoard;
using NUnit.Framework;

namespace MindHarbor.GenClassLib.Test.MessageBoard {
	[TestFixture]
	public class MessageBroadcasterFixture {
		private void TestFilter(IMessageFilter filter) {
			IMessageBroadcaster broadcaster = new MessageBroadcasterImpl();
			MockListener l = new MockListener();
			broadcaster.Subscribe(l, filter);

			Assert.AreEqual(0, l.NumOfMessagesListened);
			int i = broadcaster.Broadcast(new SuperMockMsg());
			Assert.AreEqual(0, i);
			Assert.AreEqual(0, l.NumOfMessagesListened);
			i = broadcaster.Broadcast(new MockMsg());
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			i = broadcaster.Broadcast(new SubMockMsg());
			Assert.AreEqual(1, i);
			Assert.AreEqual(2, l.NumOfMessagesListened);

			broadcaster.Unsubscribe(l);
			i = broadcaster.Broadcast(new SubMockMsg());
			Assert.AreEqual(0, i);
			Assert.AreEqual(2, l.NumOfMessagesListened);
		}

		private void ResetListeners(IEnumerable<MockListener> ls) {
			foreach (MockListener l in ls) l.NumOfMessagesListened = 0;
		}

		[Test]
		public void BasicTest() {
			IMessageBroadcaster broadcaster = new MessageBroadcasterImpl();
			MockListener l = new MockListener();
			broadcaster.Subscribe(l);
			Assert.AreEqual(0, l.NumOfMessagesListened);
			broadcaster.Broadcast(new MockMsg());
			Assert.AreEqual(1, l.NumOfMessagesListened);
			broadcaster.Broadcast(new MockMsg());
			Assert.AreEqual(2, l.NumOfMessagesListened);
		}

		[Test]
		public void DuplicateMessageWithSourceAndFilterTest() {
			IMessageWithSourceBroadcaster broadcaster = new MessageWithSourceBroadcasterImpl();
			MockListener l = new MockListener();
			MockSource ms = new MockSource();

			broadcaster.Subscribe(l, ms, new Type[] {typeof (MockMsgWithSource1)});
			broadcaster.Subscribe((IMessageListener) l, (object) null, new Type[] {typeof (MockMsgWithSource2)});

			int i = broadcaster.Broadcast(new MockMsgWithSource1(ms));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			l.NumOfMessagesListened = 0;

			i = broadcaster.Broadcast(new MockMsgWithSource1(null));
			Assert.AreEqual(0, i);
			Assert.AreEqual(0, l.NumOfMessagesListened);
			l.NumOfMessagesListened = 0;

			i = broadcaster.Broadcast(new MockMsgWithSource2(null));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			l.NumOfMessagesListened = 0;

			i = broadcaster.Broadcast(new MockMsgWithSource2(ms));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			l.NumOfMessagesListened = 0;
		}

		[Test]
		public void DuplicateMessageWithSourceTest() {
			IMessageWithSourceBroadcaster broadcaster = new MessageWithSourceBroadcasterImpl();
			MockListener l = new MockListener();
			MockListener l2 = new MockListener();
			MockSource ms = new MockSource();
			MockSource ms2 = new MockSource();

			broadcaster.Subscribe(l, ms);
			broadcaster.Subscribe(l2, ms2);
			broadcaster.Subscribe(l, null);

			int i = broadcaster.Broadcast(new MockMsgWithSource1(ms));
			Assert.AreEqual(1, i);
			Assert.AreEqual(0, l2.NumOfMessagesListened);
			Assert.AreEqual(1, l.NumOfMessagesListened);

			ResetListeners(new MockListener[] {l, l2});
			i = broadcaster.Broadcast(new MockMsgWithSource1(null));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			Assert.AreEqual(0, l2.NumOfMessagesListened);

			ResetListeners(new MockListener[] {l, l2});
			i = broadcaster.Broadcast(new MockMsgWithSource1(ms2));
			Assert.AreEqual(2, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			Assert.AreEqual(1, l2.NumOfMessagesListened);
		}

		[Test]
		public void JunctionFilterTest() {
			IMessageFilter f1 = MessageFilters.FilterByType(new Type[] {typeof (MockMsgWithSource1)});
			IMessageFilter f2 = MessageFilters.FilterByType(new Type[] {typeof (MockMsgWithSource2)});
			IMessageFilter f3 =
				MessageFilters.FilterByType(new Type[] {typeof (MockMsgWithSource1), typeof (MockMsgWithSource2)});
			IMessageFilter fdj = MessageFilters.DisJunctionFilter(new IMessageFilter[] {f1, f2});
			IMessageFilter fcj1 = MessageFilters.ConJunctionFilter(new IMessageFilter[] {f1, f2});
			IMessageFilter fcj2 = MessageFilters.ConJunctionFilter(new IMessageFilter[] {f1, f3});
			MockSource ms = new MockSource();
			Assert.IsTrue(fdj.Accept(new MockMsgWithSource1(ms)));
			Assert.IsTrue(fdj.Accept(new MockMsgWithSource2(ms)));
			Assert.IsFalse(fdj.Accept(new MockMsg()));
			Assert.IsFalse(fcj1.Accept(new MockMsgWithSource1(ms)));
			Assert.IsFalse(fcj1.Accept(new MockMsgWithSource2(ms)));
			Assert.IsTrue(fcj2.Accept(new MockMsgWithSource1(ms)));
			Assert.IsFalse(fcj2.Accept(new MockMsgWithSource2(ms)));
		}

		[Test]
		public void ListnerWithFilterTest() {
			TestFilter(new SpecificTypeMessageFilter<MockMsg>());
		}

		[Test]
		public void ListnerWithTypesFilterTest() {
			TestFilter(new SpecificTypesMessageFilter(new Type[] {typeof (MockMsg)}));
		}

		[Test]
		public void ListnerWithTypesNDataFilterTest() {
			IMessageBroadcaster broadcaster = new MessageBroadcasterImpl();
			MockListener l = new MockListener();
			MockData md = new MockData();
			broadcaster.Subscribe(l, new MessageFilterByTypeAndData<MockData>(new Type[] {typeof (MockMsgWithData)}, md));
			broadcaster.Subscribe(l, new MessageFilterByTypeAndData<MockData>(new Type[] {typeof (MockMsgWithData)}, md));

			Assert.AreEqual(0, l.NumOfMessagesListened);
			int i = broadcaster.Broadcast(new MockMsgWithData(new MockData()));
			Assert.AreEqual(0, i);
			Assert.AreEqual(0, l.NumOfMessagesListened);
			i = broadcaster.Broadcast(new MockMsgWithData(md));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			i = broadcaster.Broadcast(new MockMsgWithData(md));
			Assert.AreEqual(1, i);
			Assert.AreEqual(2, l.NumOfMessagesListened);
		}

		[Test]
		public void MessageWithSourceTest() {
			IMessageWithSourceBroadcaster broadcaster = new MessageWithSourceBroadcasterImpl();
			MockListener l = new MockListener();
			MockListener l2 = new MockListener();
			MockSource ms = new MockSource();
			MockSource ms2 = new MockSource();

			broadcaster.Subscribe(l2, ms2);
			int i = broadcaster.Broadcast(new MockMsgWithSource1(ms2));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l2.NumOfMessagesListened);

			i = broadcaster.Broadcast(new MockMsgWithSource1(ms));
			Assert.AreEqual(0, i);
			Assert.AreEqual(0, l.NumOfMessagesListened);
			Assert.AreEqual(1, l2.NumOfMessagesListened);

			broadcaster.Subscribe(l, ms);

			i = broadcaster.Broadcast(new MockMsgWithSource1(new MockSource()));
			Assert.AreEqual(0, i);
			Assert.AreEqual(0, l.NumOfMessagesListened);
			Assert.AreEqual(1, l2.NumOfMessagesListened);

			i = broadcaster.Broadcast(new MockMsgWithSource1(ms));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			Assert.AreEqual(1, l2.NumOfMessagesListened);

			broadcaster.Subscribe(l2, ms);
			i = broadcaster.Broadcast(new MockMsgWithSource1(ms));
			Assert.AreEqual(2, i);
			Assert.AreEqual(2, l.NumOfMessagesListened);
			Assert.AreEqual(2, l2.NumOfMessagesListened);

			i = broadcaster.Broadcast(new MockMsgWithSource1(null));
			Assert.AreEqual(0, i);
			Assert.AreEqual(2, l.NumOfMessagesListened);
			Assert.AreEqual(2, l2.NumOfMessagesListened);

			broadcaster.Subscribe(l2, null);
			i = broadcaster.Broadcast(new MockMsgWithSource1(null));
			Assert.AreEqual(1, i);
			Assert.AreEqual(2, l.NumOfMessagesListened);
			Assert.AreEqual(3, l2.NumOfMessagesListened);
		}

		[Test]
		public void ReSubscribeTest() {
			IMessageWithSourceBroadcaster broadcaster = new MessageWithSourceBroadcasterImpl();
			MockListener l = new MockListener();
			MockSource ms = new MockSource();
			broadcaster.Subscribe(l, ms, new Type[] {typeof (MockMsgWithSource1)});
			broadcaster.Subscribe(l, ms, new Type[] {typeof (MockMsgWithSource2)});

			int i = broadcaster.Broadcast(new MockMsgWithSource1(ms));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			l.NumOfMessagesListened = 0;

			i = broadcaster.Broadcast(new MockMsgWithSource2(ms));
			Assert.AreEqual(1, i);
			Assert.AreEqual(1, l.NumOfMessagesListened);
			l.NumOfMessagesListened = 0;
		}
	}

	public class MockSource {}

	public class MockData {}

	public class MockMsgWithSource1 : GenericMessageWithSource<MockSource> {
		public MockMsgWithSource1(MockSource src) : base(src) {}
	}

	public class MockMsgWithSource2 : GenericMessageWithSource<MockSource> {
		public MockMsgWithSource2(MockSource src) : base(src) {}
	}

	public class MockMsgWithData : GenericMessage<MockData> {
		public MockMsgWithData(MockData data) : base(data) {}
	}

	public class MockMsg : SuperMockMsg {}

	public class SubMockMsg : MockMsg {}

	public class SuperMockMsg : IMessage {}

	public class MockListener : IMessageListener {
		private int numOfMessagesListened = 0;

		public int NumOfMessagesListened {
			get { return numOfMessagesListened; }
			set { numOfMessagesListened = value; }
		}

		#region IMessageListener Members

		public void Listen(IMessage msg) {
			Console.WriteLine(msg.GetType() + " listened.");
			numOfMessagesListened++;
		}

		#endregion
	}
}