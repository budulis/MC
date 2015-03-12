using System;
using System.Collections.Generic;
using Core.Domain;

namespace Core.Application.Messages {
	public class OrderStartedApplicationNotificationMessage : IApplicationEventNotificationMessage {
		public Id Id { get; set; }
		public string Sender { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public string Payment { get; set; }
		public decimal? Amount { get; set; }
		public decimal AmountCharged { get; set; }
		public double Discount { get; set; }
		public string LoyaltyCard { get; set; }

		public OrderStartedApplicationNotificationMessage() {
			Date = DateTime.UtcNow;
		}
	}
}