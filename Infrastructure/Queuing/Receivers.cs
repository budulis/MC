using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using Infrastructure.Serialization;
using System;
using System.Threading;

namespace Infrastructure.Queuing {
	public class Receivers
	{
		public static IReciever<IDomainCommand> ForRabbitCommand(ILogger logger) {
			return new Rabbit.Reciever(x => Serializers.Json<IDomainCommand>().Deserialize(x), logger);
		}

		public static IReciever<IDomainEventNotificationMessage> ForRabbitEventNotification(ILogger logger) {
			return new Rabbit.Reciever(x => Serializers.Json<IDomainEventNotificationMessage>().Deserialize(x), logger);
		}

		public static IReciever<IApplicationEventNotificationMessage> ForRabbitApplicationEventNotification(ILogger logger) {
			return new Rabbit.Reciever(x => Serializers.Json<IApplicationEventNotificationMessage>().Deserialize(x), logger);
		}

		public static IReciever<IDomainCommand> ForInMemoryCommand(ILogger logger) {
			return new InMemory.Reciever(logger);
		}

		public static IReciever<IDomainEventNotificationMessage> ForInMemoryEventNotification(ILogger logger) {
			return new InMemory.Reciever(logger);
		}
	}
}
