using Core.Domain;

namespace Core {
	public interface ICardPaymentService
	{
		void ProcessCardPayment(decimal amountToPay, CardType cardType,string cardNumber);
	}
}
