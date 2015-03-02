using System.Linq;
using System.Threading.Tasks;
using Core.Application.Messages;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Domain.Contexts.Production.Commands;
using Core.ReadModel;

namespace Core.Subscribers {
	public sealed class OnOrderStarted : Subscriber<OrderStartedNotificationMessage, OrderReadModel> {
		private readonly IApplicationEventDispather _applicationEventDispather;

		public OnOrderStarted(IDomainCommandDispatcher domainCommandDispatcher,
			IReadModelRepository<OrderReadModel> readModelRepository,
			IApplicationEventDispather applicationEventDispather)
			: base(domainCommandDispatcher, readModelRepository) {
			_applicationEventDispather = applicationEventDispather;
		}

		public override async Task Notify(OrderStartedNotificationMessage evt) {
			var order = await ReadModelRepository.GetByIdAsync(evt.Id.ToString());
			order.Status = OrderStatus.Payed.ToString();
			await ReadModelRepository.AddAsync(order);
			await _applicationEventDispather.Dispatch(evt.ToApplicationNotificatioMessage());
			await DomainCommandDispatcher.Dispatch(new CreateProductionOrder(evt.Id, evt.Products));
		}
	}
}