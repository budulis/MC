using System;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events {
	public class OrderCompleted : IDomainEvent {
		public Id Id { get; private set; }
		public OrderCompleted(Id id) {
			if (id is NullId)
				throw new Exception("id must not be default or empty");

			Id = id;
		}
		public IDomainEventNotificationMessage ToMessage() {
			return new OrderCompletedNotificationMessage { Id = Id };
		}
	}
}