using System.Linq;
using Core;
using Core.Domain.Contexts.Ordering;

namespace Infrastructure.Services.Payment
{
	public class SelfServicePaymentService : ISelfServicePaymentService {
		public IDiscountService DiscountService { get; private set; }
		public ICardPaymentService PaymentService { get; private set; }

		public SelfServicePaymentService(IDiscountService discountService, ICardPaymentService paymentService) {
			PaymentService = paymentService;
			DiscountService = discountService;
		}

		public void ProcessPayment(SelfServiceOrder order, CardPayment payment) {
			decimal amountToPay;
			var discount = 0D;

			if (DiscountService != null)
				amountToPay = DiscountService.ApplyDiscount(payment.LoyaltyCard, order.Products.Sum(x => x.Price), out discount);
			else
				amountToPay = order.Products.Sum(x => x.Price);

			//order.Discount = discount;
		}
	}
}