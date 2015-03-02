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
}