using System;
using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using Infrastructure.Serialization;

namespace Infrastructure.Queuing {
	public class Senders{
		
		public static ISender<IDomainCommand> ForRabbitCommand(ILogger logger) {
			return new Rabbit.Sender(Serializers.Json<IDomainCommand>().Serialize, logger);
		}
		public static ISender<IDomainEventNotificationMessage> ForRabbitEventNotification(ILogger logger) {
			return new Rabbit.Sender(Serializers.Json<IDomainEventNotificationMessage>().Serialize, logger);
		}

		public static ISender<IApplicationEventNotificationMessage> ForRabbitApplicationEventNotification(ILogger logger) {
			return new Rabbit.Sender(Serializers.Json<IApplicationEventNotificationMessage>().Serialize, logger);
		}

		public static ISender<IDomainCommand> ForInMemoryCommand(ILogger logger) {
			return new InMemory.Sender(logger);
		}
		public static ISender<IDomainEventNotificationMessage> ForInMemoryEventNotification(ILogger logger) {
			return new InMemory.Sender(logger);
		}

	}
}
