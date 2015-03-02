﻿using System;
using System.Collections.Generic;

namespace Core.Domain.Contexts.Ordering.Messages
{
	public class SelfServiceOrderCreatedNotificationMessage : IDomainEventNotificationMessage {
		public Id Id { get; set; }
		public DateTime Date { get; set; }
		public string ReplyTo { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public string CustomerName { get; set; }
		public string Comments { get; set; }
	}
}