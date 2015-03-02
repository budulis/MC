using System;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering.Exceptions;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Domain.Contexts.Production.Messages;
using Core.ReadModel;
using Core.Subscribers;
using Infrastructure.ReadModel;
using Infrastructure.Services.Reporting;
using Microsoft.CSharp.RuntimeBinder;

namespace Infrastructure.Dispatchers {
	internal class DirectEventNotificationDispatcher : IDomainEventDispather {
		private readonly ILogger _logger;
		private readonly IReadModelRepository<OrderReadModel> _orderReadModelRepository;
		private readonly IReadModelRepository<ReceiptReadModel> _receiptReadModelRepository;
		private readonly Func<IDomainCommandDispatcher> _domainCommandDispatcher;

		public DirectEventNotificationDispatcher(ILogger logger, 
			ReadModelRepositoryFactory factory, 
			Func<IDomainCommandDispatcher> domainCommandDispatcher) {
			_logger = logger;
			_orderReadModelRepository = factory.Get<OrderReadModel>();
			_receiptReadModelRepository = factory.Get<ReceiptReadModel>();
			_domainCommandDispatcher = domainCommandDispatcher;
			}

		public void Dispose() {
		}

		public async Task Dispatch(IEnumerable<IDomainEventNotificationMessage> evt) {
			foreach (var domainEvent in evt) {
				await Dispatch(domainEvent);
			}
		}

		public async Task Dispatch(IDomainEventNotificationMessage evt) {
			try {
				_logger.Audit(evt.GetType().Name.Replace("NotificationMessage", ""));
				await DispatchNotification((dynamic)evt);
			}
			catch (RuntimeBinderException e) {
				_logger.Error(e);
			}
		}

		private Task DispatchNotification(OrderCreatedNotificationMessage m) {
			return new OnOrderCreated(_orderReadModelRepository).Notify(m);
		}

		private Task DispatchNotification(SelfServiceOrderCreatedNotificationMessage m) {
			return new OnSelfServiceOrderCreated(_orderReadModelRepository).Notify(m);
		}

		private Task DispatchNotification(OrderStartedNotificationMessage m) {
			var dispather = EventDispather.Application.GetDirect(_logger);
			return new OnOrderStarted(_domainCommandDispatcher(), _orderReadModelRepository, dispather).Notify(m);
		}

		private Task DispatchNotification(OrderStartFailedNotificationMessage m) {
			return new OnOrderStartFailed(_orderReadModelRepository).Notify(m);
		}

		private Task DispatchNotification(OrderCompletedNotificationMessage m) {
			return new OnOrderCompleted(_orderReadModelRepository).Notify(m);
		}

		private Task DispatchNotification(ProductionOrderCreatedNotificationMessage m) {
			return new OnProductionOrderCreated(_domainCommandDispatcher(), _orderReadModelRepository).Notify(m);
		}

		private Task DispatchNotification(ProductionOrderCompletedNotificationMessage m) {
			return new OnProductionOrderCompleted(_domainCommandDispatcher()).Notify(m);
		}
	}
}