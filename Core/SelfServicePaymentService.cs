using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using Core.Domain.Contexts.Ordering;

namespace Core {
	public class SelfServicePaymentService : ISelfServicePaymentService {
		public IDiscountService DiscountService { get; private set; }
		public ICardPaymentService CardPaymentService { get; private set; }
		public ILoyaltyProgrammService LoyaltyProgrammService { get; private set; }

		public SelfServicePaymentService(IDiscountService discountService, ICardPaymentService cardPaymentService, ILoyaltyProgrammService loyaltyProgrammService) {
			LoyaltyProgrammService = loyaltyProgrammService;
			CardPaymentService = cardPaymentService;
			DiscountService = discountService;
		}

		public void ProcessPayment(SelfServiceOrder order,IEnumerable<Product> products, string paymentCardNumber,string loyaltyCardNumber) {
			decimal amountToPay;
			var discount = 0D;

			var li = new LoyaltyProgrammInfo(LoyaltyCardType.None);

			if (LoyaltyProgrammService != null)
				li = LoyaltyProgrammService.GetInfo(loyaltyCardNumber);

			if (DiscountService != null)
				amountToPay = DiscountService.ApplyDiscount(li.CardType, products.Sum(x => x.Price), out discount);
			else
				amountToPay = products.Sum(x => x.Price);

			ProcessPayment(amountToPay, paymentCardNumber);
			order.AmountCharged = amountToPay;
			order.Discount = discount;
		}

		private void ProcessPayment(decimal amountToPay, string paymentCardNumber) {
			if (CardPaymentService == null)
				throw new PaymentServiceException("Payment service unavailable");

			CardPaymentService.ProcessPayment(amountToPay, paymentCardNumber);
		}
	}
}