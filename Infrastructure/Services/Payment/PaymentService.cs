using Core;
using Core.Domain;

namespace Infrastructure.Services.Payment {
	public class CardPaymentService : ICardPaymentService {
		public void ProcessPayment(decimal amountToPay, string cardNumber) {
			if (cardNumber.Equals("111111"))
				throw new PaymentServiceException();
		}
	}
}