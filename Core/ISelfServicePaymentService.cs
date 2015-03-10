using System.Collections.Generic;
using Core.Domain;
using Core.Domain.Contexts.Ordering;

namespace Core {
	public interface ISelfServicePaymentService
	{
		void ProcessPayment(SelfServiceOrder order, IEnumerable<Product> products, string paymentCardNumber,
			string loyaltyCardNumber);
	}
}