using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Messages;
using Core.ReadModel;

namespace Core.Subscribers
{
	public sealed class OnOrderStartFailed : Subscriber<OrderStartFailedNotificationMessage, OrderReadModel> {
		public OnOrderStartFailed(IReadModelRepository<OrderReadModel> readModelRepository)
			: base(null, readModelRepository) {
			}

		public override async Task Notify(OrderStartFailedNotificationMessage evt) {
			var order = await ReadModelRepository.GetByIdAsync(evt.Id.ToString());
			order.Status = OrderStatus.Failed.ToString();
			await ReadModelRepository.AddAsync(order);
		}
	}

	public sealed class OnSelfServiceOrderStartFailed : Subscriber<SelfServiceOrderStartFailedNotificationMessage, OrderReadModel>
	{
		public OnSelfServiceOrderStartFailed(IReadModelRepository<OrderReadModel> readModelRepository)
			: base(null, readModelRepository)
		{
		}

		public override async Task Notify(SelfServiceOrderStartFailedNotificationMessage evt)
		{
			await ReadModelRepository.AddAsync(new OrderReadModel {
				Id = evt.Id.ToString(),
				Products = evt.Products.Select(x => x.ToString()).Aggregate((x, y) => x + "; " + y),
				Total = evt.Products.Sum(x => x.Price),
				Status = OrderStatus.Failed.ToString()
			});
		}
	}
}