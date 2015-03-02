using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events {
	public class OrderCreated : IDomainEvent {
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public Cashier Cashier { get; set; }
		public string CustomerName { get; private set; }
		public string Comments { get; private set; }

		public OrderCreated(Id id, IEnumerable<Product> products, Cashier cashier, string customerName, string comments) {
			Comments = comments;
			CustomerName = customerName;
			Products = products;
			Id = id;
			Cashier = cashier;
		}

		public IDomainEventNotificationMessage ToMessage() {
			return new OrderCreatedNotificationMessage{Id = Id, Products = Products, CashierId = Cashier.Id  };
		}
	}
}