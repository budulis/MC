using System.Collections.Generic;

namespace Core.Domain.Contexts.Production.Commands {

	public enum OrderType
	{
		Regular,Self
	}

	public class CreateProductionOrder : IOrderCommand{
		
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public OrderType OrderType { get; private set; }

		public CreateProductionOrder(Id id, IEnumerable<Product> products, OrderType orderType)
		{
			OrderType = orderType;
			Products = products;
			Id = id;
		}
	}
}
