using System;
using Core;
using Core.Domain;

namespace Infrastructure.Services.Discount
{
	public class DiscountService : IDiscountService {
		
		public decimal ApplyDiscount(LoyaltyCardType loyaltyCard, decimal amount,out double discount) {
			//TODO: fix rounding issues
			switch (loyaltyCard) {
				case LoyaltyCardType.None:
				{
					discount = 0;
					return amount;
				}
				case LoyaltyCardType.Silver:
				{
					discount = 0.2D;
					return Math.Round(amount * 0.8M,2);
				}
				case LoyaltyCardType.Gold:
				{
					discount = 0.3D;
					return Math.Round(amount * 0.7M,2);
				}

				default: 
					throw new Exception("Unable to apply discount for " + loyaltyCard + "[" + amount + "]");
			}
		}
	}
}