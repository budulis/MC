using System;
using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events
{
	public class SelfServiceOrderCreated : IDomainEvent
	{
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public string CustomerName { get; private set; }
		public string Comments { get; private set; }
		public string CardNumber { get; private set; }
		public string LoyaltyCardNumber { get; private set; }
		public decimal AmountCharged { get; private set; }
		public double Discount { get; private set; }

		public SelfServiceOrderCreated(Id id, IEnumerable<Product> products, string customerName, string comments, string cardNumber, string loyaltyCardNumber, double discount, decimal amountCharged)
		{
			AmountCharged = amountCharged;
			Discount = discount;
			LoyaltyCardNumber = loyaltyCardNumber;
			CardNumber = cardNumber;
			Comments = comments;
			CustomerName = customerName;
			Products = products;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage()
		{
			return new SelfServiceOrderCreatedNotificationMessage
			{
				Id = Id,
				Products = Products,
				CustomerName = CustomerName,
				Comments = Comments,
				CardNumber = CardNumber,
				LoyaltyCardNumber = LoyaltyCardNumber,
				AmountCharged = AmountCharged,
				Discount = Discount,
				Date = DateTime.Now
			};
		}
	}
}