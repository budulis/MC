using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Commands;
using Core.Domain.Contexts.Production.Commands;
using Core.Domain.Contexts.Production.Events;
using Core.Domain.Contexts.Production.Messages;
using Core.ReadModel;

namespace Core.Subscribers
{
	public sealed class OnProductionOrderCompleted : Subscriber<ProductionOrderCompletedNotificationMessage, OrderReadModel> {
		public OnProductionOrderCompleted(IDomainCommandDispatcher domainCommandDispatcher)
			: base(domainCommandDispatcher, null) {
			}

		public override async Task Notify(ProductionOrderCompletedNotificationMessage evt)
		{
			IDomainCommand command;

			if (evt.OrderType == OrderType.Regular)
				command = new CompleteOrder(evt.Id);
			else
				command = new CompleteSelfServiceOrder(evt.Id);

			await DomainCommandDispatcher.Dispatch(command);
			await Task.FromResult(true);
		}
	}
}