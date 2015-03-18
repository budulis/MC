using System;
using Core;
using Core.ReadModel;
using Infrastructure.ReadModel;

namespace Infrastructure.Dispatchers {
	public class EventDispathers {
		private static IApplicationEventDispather _appEventDispatcher;
		private static readonly object SyncBlock = new object();
		public class Domain {
			public static IDomainEventDispather GetDirect(Func<IDomainCommandDispatcher> domainCommandDispatcher, Func<IApplicationEventDispather> applicationEventDispather, ILogger logger) {
				var factory = new ReadModelRepositoryFactory();
				return new DirectEventNotificationDispatcher(logger, factory, domainCommandDispatcher, applicationEventDispather);
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
				lock (SyncBlock)
				{
					return _appEventDispatcher ?? (_appEventDispatcher = new QueuedEventNotificationDispather(logger));
				}
			}
		}
	}
}