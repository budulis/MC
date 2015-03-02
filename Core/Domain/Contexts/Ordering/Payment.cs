using System;
using System.Runtime.InteropServices;

namespace Core.Domain.Contexts.Ordering {
	public abstract class Payment : IValueObject<Payment> {

		public Id Id { get; private set; }
		public LoyaltyCardType LoyaltyCard { get; private set; }

		protected Payment(LoyaltyCardType loyaltyCard) {
			LoyaltyCard = loyaltyCard;
			Id = Id.New();
		}
	}

	public class CashPayment : Payment, IEquatable<CashPayment> {
		public decimal Amount { get; private set; }

		public CashPayment(LoyaltyCardType loyaltyCard, decimal amount)
			: base(loyaltyCard) {
			Amount = amount;
		}

		public bool Equals(CashPayment other) {
			return Amount == other.Amount && LoyaltyCard == other.LoyaltyCard;
		}

		public override string ToString() {
			return String.Format("Amount: {0:C}; LoyaltyCard: {1}; Id: {2}", Amount, LoyaltyCard, Id);
		}
	}

	public class CardPayment : Payment, IEquatable<CardPayment> {
		public CardType CardType { get; private set; }
		public string CardNumber { get; private set; }

		public CardPayment(CardType cardType, LoyaltyCardType loyaltyCard, string cardNumber)
			: base(loyaltyCard) {
			CardNumber = cardNumber;
			CardType = cardType;
		}

		public bool Equals(CardPayment other) {
			return LoyaltyCard == other.LoyaltyCard && CardType == other.CardType;
		}

		public override string ToString() {
			return String.Format("Card Number: {0}; LoyaltyCard: {1}; Id: {2}", CardNumber, LoyaltyCard, Id);
		}
	}
}
