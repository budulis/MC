using System;
using Core;
using Core.ReadModel;
using Infrastructure.ReadModel;

namespace Infrastructure.Dispatchers
{
	public class EventDispathers
	{
		public class Domain {
			public static IDomainEventDispather GetDirect(Func<IDomainCommandDispatcher> domainCommandDispatcher, ILogger logger) {
				var factory = new ReadModelRepositoryFactory();
				return new DirectEventNotificationDispatcher(logger, factory, domainCommandDispatcher);
			}
			public static IDomainEventDispather GetQueued(ILogger logger) {
				return new QueuedEventNotificationDispather(logger);
			}
		}
		public class Application {
			public static IApplicationEventDispather GetDirect(ILogger logger) {
				var factory = new ReadModelRepositoryFactory();
				return new DirectApplicationEventNotificationDispatcher(logger, factory.Get<ReceiptReadModel>());
			}
			public static IApplicationEventDispather GetQueued(ILogger logger) {
				return new QueuedEventNotificationDispather(logger);
			}
		}
	}
}