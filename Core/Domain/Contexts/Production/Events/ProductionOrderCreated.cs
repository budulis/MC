using System.Collections.Generic;
using Core.Domain.Contexts.Production.Messages;

namespace Core.Domain.Contexts.Production.Events
{
	public class ProductionOrderCreated : IDomainEvent
	{
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public Chef Chef { get; private set; }

		public ProductionOrderCreated(Id id, IEnumerable<Product> products, Chef chef)
		{
			Chef = chef;
			Products = products;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage()
		{
			return new ProductionOrderCreatedNotificationMessage {Id = Id, Products = Products};
		}
	}
}