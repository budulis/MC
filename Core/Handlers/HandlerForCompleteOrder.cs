using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Commands;
using Core.Domain.Contexts.Ordering.Events;

namespace Core.Handlers {
	public sealed class HandlerForCompleteOrder : Handler<CompleteOrder>
	{
		public HandlerForCompleteOrder(IEventStore store, IDiscountService discountService, ICardPaymentService cardPaymentService, ILogger logger, IDomainEventDispather domainEventDispather) : base(store, discountService, cardPaymentService, logger, domainEventDispather)
		{
		}

		public override async Task Handle(CompleteOrder command)
		{
			var order = new Order { Logger = Logger };
			foreach (var evt in await Store.GetAsync<IDomainEvent>(order.GetType(), command.Id)) {
				order.UpdateFromEvent(evt);
			}
			order.Complete();
			await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);

			DomainEventDispather.Dispatch(order.Events.Select(x=>x.ToMessage()));
		}
	}
}
