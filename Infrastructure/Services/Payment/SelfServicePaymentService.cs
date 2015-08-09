using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering;

namespace Infrastructure.Services.Payment {
	public class SelfServicePaymentService : ISelfServicePaymentService {
		public IDiscountService DiscountService { get; private set; }
		public ICardPaymentService CardPaymentService { get; private set; }
		public ILoyaltyProgrammService LoyaltyProgrammService { get; private set; }

		public SelfServicePaymentService(IDiscountService discountService, ICardPaymentService cardPaymentService, ILoyaltyProgrammService loyaltyProgrammService) {
			LoyaltyProgrammService = loyaltyProgrammService;
			CardPaymentService = cardPaymentService;
			DiscountService = discountService;
		}

		public SelfServicePaymentResult ProcessPayment(IEnumerable<Core.Domain.Product> products, string paymentCardNumber, string loyaltyCardNumber)
		{
			decimal initialAmount = products.Sum(x => x.Price);
			decimal amountToPay;
			var discount = 0D;

			var li = new LoyaltyProgrammInfo(LoyaltyCardType.None);

			if (LoyaltyProgrammService != null)
				li = LoyaltyProgrammService.GetInfo(loyaltyCardNumber);

			if (DiscountService != null)
				amountToPay = DiscountService.ApplyDiscount(li.CardType, initialAmount, out discount);
			else
				amountToPay = initialAmount;

			ProcessPayment(amountToPay, paymentCardNumber);

			var result = new SelfServicePaymentResult
			{
				AmountCharged = amountToPay,
				Discount = initialAmount - amountToPay
			};

			return result;
		}

		private void ProcessPayment(decimal amountToPay, string paymentCardNumber) {
			if (CardPaymentService == null)
				throw new PaymentServiceException("Payment service unavailable");

			CardPaymentService.ProcessPayment(amountToPay, paymentCardNumber);
		}
	}
}