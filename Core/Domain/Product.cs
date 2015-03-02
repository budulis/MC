using System;

namespace Core.Domain {
	public class Product : IValueObject<Product> {
		public string Id { get; private set; }
		public string Name { get; private set; }
		public decimal Price { get; private set; }

		public Product(string id, string name,decimal price) {
			Id = id;
			Name = name;
			Price = price;
		}

		private Product() {
		}

		public bool Equals(Product other) {
			const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
			return Id.Equals(other.Id, comparison) 
				&& Name.Equals(other.Name, comparison)
				&& other.Price == Price;
		}

		public override string ToString() {
			return String.Format("{0}-{1}", Id, Name);
		}
	}
}