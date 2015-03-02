using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Production.Commands;
using Core.Domain.Contexts.Production.Events;
using Core.Domain.Contexts.Production.Messages;
using Core.ReadModel;

namespace Core.Subscribers
{
	public sealed class OnProductionOrderCreated : Subscriber<ProductionOrderCreatedNotificationMessage, OrderReadModel> {
		public OnProductionOrderCreated(IDomainCommandDispatcher domainCommandDispatcher, IReadModelRepository<OrderReadModel> readModelRepository)
			: base(domainCommandDispatcher, readModelRepository) {
			}

		public override async Task Notify(ProductionOrderCreatedNotificationMessage evt) {
			var order = await ReadModelRepository.GetByIdAsync(evt.Id.ToString());
			order.Status = OrderStatus.InProcess.ToString();
			await ReadModelRepository.AddAsync(order);
			await DomainCommandDispatcher.Dispatch(new ProduceProductionOrder(evt.Id, evt.Products));
		}
	}
}