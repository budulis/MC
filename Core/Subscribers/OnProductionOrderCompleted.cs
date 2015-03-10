using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering.Commands;
using Core.Domain.Contexts.Production.Events;
using Core.Domain.Contexts.Production.Messages;
using Core.ReadModel;

namespace Core.Subscribers
{
	public sealed class OnProductionOrderCompleted : Subscriber<ProductionOrderCompletedNotificationMessage, OrderReadModel> {
		public OnProductionOrderCompleted(IDomainCommandDispatcher domainCommandDispatcher)
			: base(domainCommandDispatcher, null) {
			}

		public override async Task Notify(ProductionOrderCompletedNotificationMessage evt) {
			await DomainCommandDispatcher.Dispatch(new CompleteOrder(evt.Id,evt.OrderType));
			await Task.FromResult(true);
		}
	}
}