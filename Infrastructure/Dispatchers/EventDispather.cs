using Core;
using Core.Application.Messages;
using Core.Domain;
using Core.ReadModel;
using Infrastructure.ReadModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Dispatchers {
	public abstract class EventDispather : IDomainEventDispather, IApplicationEventDispather {

		public virtual async Task Dispatch(IEnumerable<IDomainEventNotificationMessage> evt) {
			foreach (var domainEventNotificationMessage in evt)
				await Dispatch(domainEventNotificationMessage);
		}
		public abstract Task Dispatch(IDomainEventNotificationMessage evt);
		public abstract Task Dispatch(IApplicationEventNotificationMessage evt);

		#region Factory
		public class Domain {
			public static IDomainEventDispather GetDirect(Func<IDomainCommandDispatcher> domainCommandDispatcher, ILogger logger) {
				var factory = new ReadModelRepositoryFactory();
				return new DirectEventNotificationDispatcher(logger, factory, domainCommandDispatcher);
			}
			public static IDomainEventDispather GetQueued(ILogger logger) {
				return new QueuedEventDispather(logger);
			}
		}

		public class Application {
			public static IApplicationEventDispather GetDirect(ILogger logger) {
				var factory = new ReadModelRepositoryFactory();
				return new DirectApplicationEventNotificationDispatcher(logger, factory.Get<ReceiptReadModel>());
			}
			public static IApplicationEventDispather GetQueued(ILogger logger) {
				throw new NotImplementedException();
			}
		}
		#endregion
		#region IDisposable
		~EventDispather() {
			Dispose(false);
		}
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected abstract void Dispose(bool disposing);
		#endregion
	}
}