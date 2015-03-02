using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering.Exceptions;

namespace Core.Domain.Contexts.Ordering.Commands {
	public class CreateOrder : IOrderCommand {
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public string CustomerName { get; private set; }
		public string Comments { get; private set; }
		public string CashierId { get; private set; }

		public CreateOrder(Id id, Product[] products, string customerName, string comments, string cashierId) {
			CashierId = cashierId;
			Comments = comments;
			CustomerName = customerName;
			Products = products;
			Id = id;
		}
	}
}