using System;
using System.Collections.Generic;
using Core.Application.Messages;

namespace Core.Domain.Contexts.Ordering.Messages
{
	public class SelfServiceOrderCreatedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public string CustomerName { get; set; }
		public string Comments { get; set; }
		public string CardNumber { get; set; }
		public string LoyaltyCardNumber { get; set; }
		public decimal AmountCharged { get; set; }
		public double Discount { get; set; }

		public IApplicationEventNotificationMessage ToApplicationNotificatioMessage()
		{
			return new OrderStartedApplicationNotificationMessage
			{
				Id = Id,
				Amount = AmountCharged,
				AmountCharged = AmountCharged,
				Date = Date,
				Discount = Discount,
				LoyaltyCard = LoyaltyCardNumber,
				Payment = PaymentType.Card.ToString(),
				Products = Products,
				ReplyTo = ReplyTo,
				Sender = GetType().ToString()
			};
		}
	}
}