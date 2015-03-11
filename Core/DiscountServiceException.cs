using System;

namespace Core
{
	[Serializable]
	public class DiscountServiceException : Exception {
		public DiscountServiceException() { }
		public DiscountServiceException(string message) : base(message) { }
	}
}