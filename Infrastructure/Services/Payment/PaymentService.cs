using Core;
using Core.Domain;

namespace Infrastructure.Services.Payment
{
	public class CardPaymentService : ICardPaymentService
	{
		public void ProcessCardPayment(decimal amountToPay, CardType cardType,string cardNumber)
		{
			if (EnoughMoney(cardNumber) && AllowsNegativeAmount(cardType))
			{

			}
			else
			{
				throw new PaymentServiceException();
			}
		}

		private static bool AllowsNegativeAmount(CardType type)
		{
			return type == CardType.Credit;
		}

		private static bool EnoughMoney(string cardNumber)
		{
			return true;
		}
	}
}