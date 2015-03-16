using System;
using System.Collections.Generic;

namespace Core.Domain.Contexts.Ordering.Messages
{
	public class SelfServiceOrderStartFailedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public string Payment { get; set; }
		public decimal AmountCharged { get; set; }
		public string Reason { get; set; }
	}
}