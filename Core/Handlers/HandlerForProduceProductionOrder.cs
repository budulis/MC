using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Production;
using Core.Domain.Contexts.Production.Commands;

namespace Core.Handlers
{
	public sealed class HandlerForProduceProductionOrder : Handler<ProduceProductionOrder> {
		public HandlerForProduceProductionOrder(IEventStore store, IDiscountService discountService, ICardPaymentService cardPaymentService, ILogger logger, IDomainEventDispather domainEventDispather)
			: base(store, discountService, cardPaymentService, logger, domainEventDispather) {
			}

		public override async Task Handle(ProduceProductionOrder command) {
			var order = new ProductionOrder();
			foreach (var evt in await Store.GetAsync<IDomainEvent>(order.GetType(), command.Id)) {
				order.UpdateFromEvent(evt);
			}
			order.Complete();
			await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);

			DomainEventDispather.Dispatch(order.Events.Select(x => x.ToMessage()));
		}
	}
}