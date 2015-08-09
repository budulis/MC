using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events {
	public class OrderStarted : IDomainEvent {
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public Payment Payment { get; private set; }
		public decimal AmountCharged { get; private set; }
		public decimal Discount { get; private set; }

		public OrderStarted(Id id, IEnumerable<Product> products, Payment payment, decimal amountCharged, decimal discount) {
			Discount = discount;
			AmountCharged = amountCharged;
			Payment = payment;
			Products = products;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage()
		{
			var amount = (Payment is CashPayment) ? ((CashPayment) Payment).Amount : (decimal?) null;
			return new OrderStartedNotificationMessage {
				Id = Id,
				Products = Products,
				Payment = Payment.GetType().Name,
				AmountCharged = AmountCharged,
				Amount = amount,
				Date = DateTime.Now,
				Discount = Discount,
				LoyaltyCard = Payment.LoyaltyCard.ToString()
			};
		}

		
	}
}