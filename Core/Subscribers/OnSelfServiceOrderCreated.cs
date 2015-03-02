using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Messages;
using Core.ReadModel;

namespace Core.Subscribers
{
	public sealed class OnSelfServiceOrderCreated : Subscriber<SelfServiceOrderCreatedNotificationMessage, OrderReadModel> {
		public OnSelfServiceOrderCreated(IReadModelRepository<OrderReadModel> readModelRepository)
			: base(null, readModelRepository) {
			}

		public override async Task Notify(SelfServiceOrderCreatedNotificationMessage evt) {
			await ReadModelRepository.AddAsync(new OrderReadModel {
				Id = evt.Id.ToString(),
				Products = evt.Products.Select(x => x.ToString()).Aggregate((x, y) => x + "; " + y),
				Total = evt.Products.Sum(x => x.Price),
				Status = OrderStatus.WaitingForPayment.ToString()
			});
		}
	}
}