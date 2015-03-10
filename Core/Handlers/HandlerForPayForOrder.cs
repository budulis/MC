using System;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Commands;
using Core.Domain.Contexts.Ordering.Exceptions;
using System.Linq;
using Core.Handlers.Exceptions;
using Polly;

namespace Core.Handlers {
	public sealed class HandlerForPayForOrder : Handler<PayForOrder> {

		private readonly ContextualPolicy _policy;

		public HandlerForPayForOrder(IEventStore store, IDiscountService discountService, ICardPaymentService cardPaymentService, ILogger logger, IDomainEventDispather domainEventDispather)
			: base(store, discountService, cardPaymentService, logger, domainEventDispather) {

			_policy = Policy
					.Handle<OrderException>(x => x.Reason == OrderException.Code.OrderNotStarted)
					.WaitAndRetry(
					3,
					retryAttempt => TimeSpan.FromSeconds(2 * retryAttempt),
					(exception, timeSpan, context) => {
						throw new CommandExecutionException("", exception);
					});
		}

		public override async Task Handle(PayForOrder command) {
			await _policy.Execute(() => HandleCommand(command));
		}

		private async Task HandleCommand(PayForOrder command) {
			try
			{
				var order = new Order {Logger = Logger};
				foreach (var evt in await Store.GetAsync<IDomainEvent>(order.GetType(), command.Id))
				{
					order.UpdateFromEvent(evt);
				}

				if (command.PaymentType == PaymentType.Card)
				{
					var payment = new CardPayment(command.LoyaltyCard, command.CardNumber);
					order.AsignPayment(payment);
				}
				else
				{
					var payment = new CashPayment(command.LoyaltyCard, command.Amount);
					order.AsignPayment(payment);
				}

				await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);

				await DomainEventDispather.Dispatch(order.Events.Select(x => x.ToMessage()));
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
		}
	}
}