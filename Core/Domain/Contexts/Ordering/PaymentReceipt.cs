using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Contexts.Ordering {
	[Obsolete("Not needed so far")]
	public class PaymentReceipt {
		private readonly StringBuilder _sb;
		public Id Id { get; set; }
		public string LoyaltyCard { get; set; }
		public decimal AmountCharged { get; set; }
		public double Discount { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public decimal? Amount { get; set; }

		public PaymentReceipt(Id id, decimal? amount, string loyaltyCard,decimal amountCharged, double discount, IEnumerable<Product> products ) {
			_sb = new StringBuilder();
			Id = id;
			LoyaltyCard = loyaltyCard;
			AmountCharged = amountCharged;
			Discount = discount;
			Products = products;
			Amount = amount;

		}
		public override string ToString() {
			_sb.Append("Id:");
			_sb.Append(Id);

			if (Amount.HasValue) {
				_sb.AppendLine("Amount:");
				_sb.Append(Amount);
			}

			_sb.AppendLine("Loyalty Card:");
			_sb.Append(LoyaltyCard);
			_sb.AppendLine("Amount charged:");
			_sb.Append(AmountCharged);
			_sb.AppendLine("Discount:");
			_sb.Append(Discount);
			_sb.AppendLine("Date:");
			_sb.Append(DateTime.Now);

			_sb.AppendLine("Products:");
			foreach (var product in Products)
				_sb.Append(product);

			return _sb.ToString();
		}
	}
}
