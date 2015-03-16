using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Core {
	public class PaymentInfo {
		public string CardNumber { get; set; }
		public string LoyaltyCardNumber { get; set; }

		public override string ToString() {
			return String.Format("CardNumber:{0};LoyaltyCardNumber:{1}", CardNumber, LoyaltyCardNumber);
		}
	}

	public class OrderInfo {
		public Id Id { get; set; }
		public IEnumerable<Product> Products { get; set; }
		public string Name { get; set; }
		public string Comments { get; set; }
	}
}
