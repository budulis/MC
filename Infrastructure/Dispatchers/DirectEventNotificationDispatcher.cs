using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Domain.Contexts.Production.Messages;
using Core.ReadModel;
using Core.Subscribers;
using Infrastructure.ReadModel;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Infrastructure.Dispatchers {
	internal class DirectEventNotificationDispatcher : IDomainEventDispather {
		private readonly ILogger _logger;
		private readonly IReadModelRepository<OrderReadModel> _orderReadModelRepository;
		private readonly Func<IDomainCommandDispatcher> _domainCommandDispatcher;
		private readonly Func<IApplicationEventDispather> _applicationEventDispatherFunc;
		private readonly ReadModelRepositoryFactory _factory;
		private IDictionary<Type, List<Func<IDomainEventNotificationMessage, Task>>> _subscribers;

		public DirectEventNotificationDispatcher(ILogger logger,
			ReadModelRepositoryFactory factory,
			Func<IDomainCommandDispatcher> domainCommandDispatcher,
			Func<IApplicationEventDispather> applicationEventDispatherFunc) {
			_logger = logger;
			_factory = factory;
			_orderReadModelRepository = _factory.Get<OrderReadModel>();
			_domainCommandDispatcher = domainCommandDispatcher;
			_applicationEventDispatherFunc = applicationEventDispatherFunc;

			_subscribers = new Dictionary<Type, List<Func<IDomainEventNotificationMessage, Task>>>
			{
				{
					typeof (OrderCreatedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x => new OnOrderCreated(_orderReadModelRepository).Notify((OrderCreatedNotificationMessage) x)
					}
				},
				{
					typeof (SelfServiceOrderCreatedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x =>
							new OnSelfServiceOrderCreated(_domainCommandDispatcher(), _orderReadModelRepository,
								_applicationEventDispatherFunc()).Notify((SelfServiceOrderCreatedNotificationMessage) x)
					}
				},
				{
					typeof (OrderStartedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x =>
							new OnOrderStarted(_domainCommandDispatcher(), _orderReadModelRepository, _applicationEventDispatherFunc())
								.Notify((OrderStartedNotificationMessage) x)
					}
				},
				{
					typeof (OrderStartFailedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x => new OnOrderStartFailed(_orderReadModelRepository).Notify((OrderStartFailedNotificationMessage) x)
					}
				},
				{
					typeof (SelfServiceOrderStartFailedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x =>
							new OnSelfServiceOrderStartFailed(_orderReadModelRepository).Notify(
								(SelfServiceOrderStartFailedNotificationMessage) x)
					}
				},
				{
					typeof (OrderCompletedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x => new OnOrderCompleted(_orderReadModelRepository).Notify((OrderCompletedNotificationMessage) x)
					}
				},
				{
					typeof (ProductionOrderCreatedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x =>
							new OnProductionOrderCreated(_domainCommandDispatcher(), _orderReadModelRepository).Notify(
								(ProductionOrderCreatedNotificationMessage) x)
					}
				},
				{
					typeof (ProductionOrderCompletedNotificationMessage),
					new List<Func<IDomainEventNotificationMessage, Task>>
					{
						x =>
							new OnProductionOrderCompleted(_domainCommandDispatcher()).Notify((ProductionOrderCompletedNotificationMessage) x)
					}
				}
			};

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
				var results = _subscribers[evt.GetType()].Select(x => x(evt));
				await Task.WhenAll(results);
			}
			catch (KeyNotFoundException e) {
				_logger.Error(e);
			}
		}

		public IDomainEventDispather Register(Type type, Func<IDomainEventNotificationMessage, Task> subscriber) {
			var d = new DirectEventNotificationDispatcher(_logger, _factory, _domainCommandDispatcher, _applicationEventDispatherFunc) {
				_subscribers = AddSubscriber(type, subscriber)
			};
			return d;
		}

		private IDictionary<Type, List<Func<IDomainEventNotificationMessage, Task>>> AddSubscriber(Type type,
			Func<IDomainEventNotificationMessage, Task> subscriber) {

			var d = _subscribers.ToDictionary(x => x.Key, y => y.Value.Select(x => x).ToList());

			try {
				List<Func<IDomainEventNotificationMessage, Task>> subscribers;
				if (d.TryGetValue(type, out subscribers)) 
					subscribers.Add(subscriber);
				else 
					subscribers = new List<Func<IDomainEventNotificationMessage, Task>>(new[] { subscriber });

				d[type] = subscribers;
			}
			catch (ArgumentException e) {
				throw new Exception("Can not add subscriber.", e);
			}

			return d;
		}
	}
}