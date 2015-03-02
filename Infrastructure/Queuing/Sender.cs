using System;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Serialization;

namespace Infrastructure.Queuing {
	public abstract class Sender : ISender<IDomainCommand>, ISender<IDomainEventNotificationMessage> {
		public abstract Task SendAsync(IDomainCommand message);
		public abstract Task SendAsync(IDomainEventNotificationMessage message);

		#region Factory
		public static ISender<IDomainCommand> ForRabbitCommand(ILogger logger) {
			return new Rabbit.Sender(Serializer<IDomainCommand>.Json.Serialize, logger);
		}
		public static ISender<IDomainEventNotificationMessage> ForRabbitEventNotification(ILogger logger) {
			return new Rabbit.Sender(Serializer<IDomainEventNotificationMessage>.Json.Serialize, logger);
		}
		public static ISender<IDomainCommand> ForInMemoryCommand(ILogger logger) {
			return new InMemory.Sender(logger);
		}
		public static ISender<IDomainEventNotificationMessage> ForInMemoryEventNotification(ILogger logger) {
			return new InMemory.Sender(logger);
		}

		#endregion
		#region IDisposable
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {

			}
		}

		~Sender() {
			Dispose(false);
		}
		#endregion
	}
}
