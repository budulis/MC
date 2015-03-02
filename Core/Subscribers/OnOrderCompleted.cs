using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Messages;
using Core.ReadModel;
using System.Threading.Tasks;

namespace Core.Subscribers {
	public sealed class OnOrderCompleted : Subscriber<OrderCompletedNotificationMessage, OrderReadModel> {
		public OnOrderCompleted(IReadModelRepository<OrderReadModel> readModelRepository)
			: base(null, readModelRepository) {
		}

		public override async Task Notify(OrderCompletedNotificationMessage evt) {
			var order = await ReadModelRepository.GetByIdAsync(evt.Id.ToString());
			order.Status = OrderStatus.Completed.ToString();
			await ReadModelRepository.AddAsync(order);
		}
	}
}
