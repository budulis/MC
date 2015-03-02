using System;
using System.Collections.Generic;

namespace Core.Domain.Contexts.Production.Messages
{
	public class ProductionOrderCreatedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }

		public ProductionOrderCreatedNotificationMessage()
		{
			Date = DateTime.UtcNow;
		}
	}
}