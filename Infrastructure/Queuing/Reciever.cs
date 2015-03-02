using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Serialization;
using System;
using System.Threading;

namespace Infrastructure.Queuing {
	public abstract class Reciever : IReciever<IDomainCommand>, IReciever<IDomainEventNotificationMessage> {
		public abstract void Recieve(Action<Task<IDomainCommand>> onMessageRecieved, CancellationToken cancellationToken);
		public abstract void Recieve(Action<Task<IDomainEventNotificationMessage>> onMessageRecieved, CancellationToken cancellationToken);

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
		
		public static IReciever<IDomainCommand> ForRabbitCommand(ILogger logger) {
			return new Rabbit.Reciever(x => Serializer<IDomainCommand>.Json.Deserialize(x), logger);
		}

		public static IReciever<IDomainEventNotificationMessage> ForRabbitEventNotification(ILogger logger) {
			return new Rabbit.Reciever(x => Serializer<IDomainEventNotificationMessage>.Json.Deserialize(x), logger);
		}

		public static IReciever<IDomainCommand> ForInMemoryCommand(ILogger logger) {
			return new InMemory.Reciever(logger);
		}

		public static IReciever<IDomainEventNotificationMessage> ForInMemoryEventNotification(ILogger logger) {
			return new InMemory.Reciever(logger);
		}
	}
}
