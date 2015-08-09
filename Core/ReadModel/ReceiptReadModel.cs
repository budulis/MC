using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain;

namespace Core.ReadModel {
	public class ReceiptReadModel : IReadModel {
		private readonly StringBuilder _sb;

		public ReceiptReadModel() {
			_sb = new StringBuilder();
		}

		public string Id { get; set; }
		public string LoyaltyCard { get; set; }
		public decimal AmountCharged { get; set; }
		public decimal Discount { get; set; }
		public string Products { get; set; }
		public decimal? Amount { get; set; }
		public string PaymentType { get; set; }

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