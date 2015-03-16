using System;
using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using Infrastructure.Serialization;

namespace Infrastructure.Queuing {
	public class Senders{
		
		public static ISender<IDomainCommand> ForRabbitCommand(ILogger logger) {
			return new Rabbit.Sender(x=>Serializers.Json<IDomainCommand>().Serialize(x), logger);
		}
		public static ISender<IDomainEventNotificationMessage> ForRabbitEventNotification(ILogger logger) {
			return new Rabbit.Sender(x => Serializers.Json<IDomainEventNotificationMessage>().Serialize(x), logger);
		}

		public static ISender<IApplicationEventNotificationMessage> ForRabbitApplicationEventNotification(ILogger logger) {
			return new Rabbit.Sender(x => Serializers.JsonNoTypeInfo<IApplicationEventNotificationMessage>().Serialize(x), logger);
		}

		public static ISender<IDomainCommand> ForInMemoryCommand(ILogger logger) {
			return new InMemory.Sender(logger);
		}
		public static ISender<IDomainEventNotificationMessage> ForInMemoryEventNotification(ILogger logger) {
			return new InMemory.Sender(logger);
		}

		public static ISender<IApplicationEventNotificationMessage> ForInMemoryApplicationEventNotification(ILogger logger) {
			return new InMemory.Sender(logger);
		}
	}
}
