using System.Collections.Generic;
using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Infrastructure.Services.Payment;

namespace Core {
	public interface ISelfServicePaymentService
	{
		SelfServicePaymentResult ProcessPayment(IEnumerable<Product> products, string paymentCardNumber,string loyaltyCardNumber);
	}
}