using System;
using System.Collections.Generic;

namespace Core.Domain.Contexts.Ordering.Messages
{
	public class OrderCreatedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public Id CashierId { get; set; }

		public OrderCreatedNotificationMessage()
		{
			Date = DateTime.UtcNow;
		}
	}
}