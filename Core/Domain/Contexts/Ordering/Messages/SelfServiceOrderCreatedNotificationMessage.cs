using System;
using System.Collections.Generic;
using System.Linq;
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
		public decimal Discount { get; set; }

		public IApplicationEventNotificationMessage ToApplicationNotificatioMessage()
		{
			var amount = Products.Sum(x => x.Price);
			return new OrderStartedApplicationNotificationMessage
			{
				Id = Id,
				Amount = amount,
				AmountCharged = AmountCharged,
				Date = Date,
				Discount = amount - AmountCharged,
				LoyaltyCard = LoyaltyCardNumber,
				Payment = PaymentType.Card.ToString(),
				Products = Products,
				ReplyTo = ReplyTo,
			};
		}
	}
}