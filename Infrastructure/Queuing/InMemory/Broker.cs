using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


namespace Infrastructure.Queuing.InMemory {
	internal class Broker : IDisposable{
		private readonly DataflowBlockOptions _queueOptions;

		private readonly Dictionary<Type, BufferBlock<object>> _queues;

		public static Broker Instance { get; private set; }

		private Broker() {
			_queues = new Dictionary<Type, BufferBlock<object>>();
			_queueOptions = new DataflowBlockOptions {
				BoundedCapacity = 100,
				TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
			};
		}

		static Broker() {
			Instance = new Broker();
		}

		public void Accept<TMessage>(TMessage msg)
		{
			CreatBufferIfNotExists<TMessage>();
			_queues[typeof (TMessage)].Post(msg);
		}

		public TMessage Consume<TMessage>() {
			return (TMessage)_queues[typeof(TMessage)].Receive();
		}

		private void CreatBufferIfNotExists<TMessage>() {
			if (_queues.ContainsKey(typeof(TMessage)))
				return;

			_queues.Add(typeof(TMessage), new BufferBlock<object>(_queueOptions));
		}

		public void Dispose()
		{
			_queues.Clear();
		}
	}
}
