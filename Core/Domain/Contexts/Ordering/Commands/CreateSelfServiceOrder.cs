using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering.Exceptions;

namespace Core.Domain.Contexts.Ordering.Commands
{
	public class CreateSelfServiceOrder : IOrderCommand {
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public string CustomerName { get; private set; }
		public string Comments { get; private set; }
		public string CardNumber { get; private set; }
		public string LoyaltyCardNumber { get; set; }

		public CreateSelfServiceOrder(Id id, Product[] products, string customerName, string comments, string cardNumber, string loyaltyCardNumber) {
			CardNumber = cardNumber;
			LoyaltyCardNumber = loyaltyCardNumber;
			Comments = comments;
			CustomerName = customerName;
			Products = products;
			Id = id;
		}
	}
}