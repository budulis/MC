using System;
using System.Collections.Generic;
using Core.Application.Messages;

namespace Core.Domain.Contexts.Ordering.Messages
{
	public class OrderStartedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public string Payment { get; set; }
		public decimal? Amount { get; set; }
		public decimal AmountCharged { get; set; }
		public decimal Discount { get; set; }
		public string LoyaltyCard { get; set; }

		public OrderStartedNotificationMessage()
		{
			Date = DateTime.UtcNow;
		}

		internal IApplicationEventNotificationMessage ToApplicationNotificatioMessage()
		{
			return new OrderStartedApplicationNotificationMessage {
				Id = Id,
				Amount = Amount,
				AmountCharged = AmountCharged,
				Date = Date,
				Discount = Discount,
				LoyaltyCard = LoyaltyCard,
				Payment = Payment,
				Products = Products,
				ReplyTo = ReplyTo,
			};
		}
	}
}