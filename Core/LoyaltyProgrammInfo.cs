using System;
using Core.Domain;

namespace Core
{
	public class LoyaltyProgrammInfo {
		public LoyaltyCardType CardType { get; private set; }
		public DateTime? ValidUntil { get; private set; }
		public LoyaltyProgrammInfo(LoyaltyCardType cardType, DateTime? validUntil = null) {
			ValidUntil = validUntil;
			CardType = cardType;
		}
	}
}