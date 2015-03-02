using System;

namespace Core {
	[Serializable]
	public class PaymentServiceException : Exception{
		public PaymentServiceException(){}
		public PaymentServiceException(string message):base(message) {}
	}
}
