using Core.Domain;

namespace Core {
	public interface IDiscountService {
		decimal ApplyDiscount(LoyaltyCardType loyaltyCard, decimal amount,out double dicount);
	}
}
