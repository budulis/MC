using System;

namespace Core {
	[Serializable]
	public class PaymentServiceException : Exception{
		public PaymentServiceException(){}
		public PaymentServiceException(string message):base(message) {}
	}

	[Serializable]
	public class LoyaltyServiceException : Exception{
		public LoyaltyServiceException(){}
		public LoyaltyServiceException(string message) : base(message) { }
	}

	[Serializable]
	public class DiscountServiceException : Exception {
		public DiscountServiceException() { }
		public DiscountServiceException(string message) : base(message) { }
	}
}
