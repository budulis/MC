using System;

namespace Core.Domain.Contexts.Ordering.Messages
{
	public class OrderCompletedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }

		public OrderCompletedNotificationMessage()
		{
			Date = DateTime.UtcNow;
		}
	}
}