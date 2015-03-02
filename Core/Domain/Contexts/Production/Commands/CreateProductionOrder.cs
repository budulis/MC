using System.Collections.Generic;

namespace Core.Domain.Contexts.Production.Commands {
	public class CreateProductionOrder : IOrderCommand{
		
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }

		public CreateProductionOrder(Id id, IEnumerable<Product> products)
		{
			Products = products;
			Id = id;
		}
	}
}
