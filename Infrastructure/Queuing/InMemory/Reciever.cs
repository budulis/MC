using System;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Exceptions;
using Core.Handlers.Exceptions;

namespace Infrastructure.Queuing.InMemory {
	internal class Reciever : IReciever<IDomainCommand>, IReciever<IDomainEventNotificationMessage> {

		private readonly ILogger _logger;
		public Reciever(ILogger logger) {
			_logger = logger;
		}

		public void Recieve(Action<Task<IDomainCommand>> onMessageRecieved, CancellationToken cancellationToken) {
			while (!cancellationToken.IsCancellationRequested) {
				var command = Broker.Instance.Consume<IDomainCommand>();
				try {
					onMessageRecieved(Task.FromResult(command));
				}
				catch (CommandExecutionException ex) {
					_logger.Error("[" + command.GetType().Name + ";" + command.Id + "]" + ex);
					//TODO: Send to dead-letter queue
				}
				catch (Exception ex) {
					_logger.Error(ex);
				}
			}
		}

		public void Recieve(Action<Task<IDomainEventNotificationMessage>> onMessageRecieved, CancellationToken cancellationToken) {
			while (!cancellationToken.IsCancellationRequested) {
				while (!cancellationToken.IsCancellationRequested) {
					var command = Broker.Instance.Consume<IDomainEventNotificationMessage>();
					onMessageRecieved(Task.FromResult(command));
				}
			}
		}

		~Reciever()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				//means this one is called from within the managed code 
				//and it is safe to free any used managed resources
			}
			//below is the place to free any unmanaged resources
		}

	}
}