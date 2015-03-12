using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Messages;
using Core.Domain.Contexts.Production.Commands;
using Core.ReadModel;

namespace Core.Subscribers
{
	public sealed class OnSelfServiceOrderCreated : Subscriber<SelfServiceOrderCreatedNotificationMessage, OrderReadModel> {
		private readonly IApplicationEventDispather _applicationEventDispather;
		public OnSelfServiceOrderCreated(
			IDomainCommandDispatcher domainCommandDispatcher, 
			IReadModelRepository<OrderReadModel> readModelRepository, 
			IApplicationEventDispather applicationEventDispather)
			: base(domainCommandDispatcher, readModelRepository)
		{
			_applicationEventDispather = applicationEventDispather;
		}

		public override async Task Notify(SelfServiceOrderCreatedNotificationMessage evt) {
			await ReadModelRepository.AddAsync(new OrderReadModel {
				Id = evt.Id.ToString(),
				Products = evt.Products.Select(x => x.ToString()).Aggregate((x, y) => x + "; " + y),
				Total = evt.Products.Sum(x => x.Price),
				Status =  OrderStatus.Payed.ToString()
			});

			await _applicationEventDispather.Dispatch(evt.ToApplicationNotificatioMessage());
			await DomainCommandDispatcher.Dispatch(new CreateProductionOrder(evt.Id, evt.Products,OrderType.Self));
		}
	}
}