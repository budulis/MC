using System;
using Core.Domain.Contexts.Production.Commands;

namespace Core.Domain.Contexts.Production.Messages
{
	public class ProductionOrderCompletedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public OrderType OrderType { get; set; }
	}
}