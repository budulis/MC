using Core.Domain.Contexts.Production.Messages;

namespace Core.Domain.Contexts.Production.Events
{
	public class ProductionOrderCompleted : IDomainEvent {
		public Id Id { get; private set; }
		public Chef Chef { get; private set; }
		
		public ProductionOrderCompleted(Id id, Chef chef)
		{
			Chef = chef;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage() {
			return new ProductionOrderCompletedNotificationMessage{Id =Id};
		}
	}
}