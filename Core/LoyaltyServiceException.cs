using System;

namespace Core
{
	[Serializable]
	public class LoyaltyServiceException : Exception{
		public LoyaltyServiceException(){}
		public LoyaltyServiceException(string message) : base(message) { }
	}
}