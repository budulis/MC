using System;

namespace Core.Domain.Contexts.Production.Messages
{
	public class ProductionOrderCompletedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
	}
}