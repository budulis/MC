namespace Core.Domain.Contexts.Ordering.Commands {
	public class PayForOrder : IOrderCommand {
		public Id Id { get; private set; }
		public decimal Amount { get; private set; }
		public PaymentType PaymentType { get; private set; }
		public CardType CardType { get; private set; }
		public LoyaltyCardType LoyaltyCard { get; private set; }
		public string CardNumber { get; private set; }

		public PayForOrder(Id id, decimal amount, PaymentType paymentType, LoyaltyCardType loyaltyCard, CardType cardType, string cardNumber) {
			CardNumber = cardNumber;
			CardType = cardType;
			Amount = amount;
			PaymentType = paymentType;
			LoyaltyCard = loyaltyCard;
			Id = id;
		}
	}
}