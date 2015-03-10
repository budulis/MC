using Core.Domain.Contexts.Production.Commands;
using Core.Domain.Contexts.Production.Messages;

namespace Core.Domain.Contexts.Production.Events {
	public class ProductionOrderCompleted : IDomainEvent {
		public Id Id { get; private set; }
		public Chef Chef { get; private set; }
		public OrderType OrderType { get; private set; }
		public ProductionOrderCompleted(Id id, Chef chef, OrderType orderType) {
			OrderType = orderType;
			Chef = chef;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage() {
			return new ProductionOrderCompletedNotificationMessage { Id = Id, OrderType = OrderType };
		}
	}
}