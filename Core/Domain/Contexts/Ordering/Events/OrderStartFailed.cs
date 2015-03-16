using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events {
	public class OrderStartFailed : IDomainEvent {
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public Payment Payment { get; private set; }
		public decimal AmountCharged { get; private set; }
		public string Reason { get; private set; }

		public OrderStartFailed(Id id, IEnumerable<Product> products, Payment payment, decimal amountCharged, string reason) {
			AmountCharged = amountCharged;
			Reason = reason;
			Payment = payment;
			Products = products;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage() {
			return new OrderStartFailedNotificationMessage
			{
				Id = Id, 
				Products=Products,
				Payment = Payment.ToString(), 
				AmountCharged = AmountCharged, 
				Reason = Reason
			};
		}
	}
}