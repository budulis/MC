using Core.Domain;

namespace Core {
	public interface ICardPaymentService {
		void ProcessPayment(decimal amountToPay, string cardNumber);
	}
}
