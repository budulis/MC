using System;
using System.Runtime.Serialization;

namespace Core.Domain.Contexts.Ordering.Exceptions
{
	[Serializable]
	internal sealed class OrderException : DomainException {

		public enum Code
		{
			OrderNotStarted
		}

		public Code Reason { get; internal set; }

		public OrderException() {}

		public OrderException(string msg)
			: base(msg) {
			}

		public OrderException(string msg, DomainException ex)
			: base(msg, ex) {
			}

		public OrderException(SerializationInfo si, StreamingContext context)
			: base(si, context) {
			}
	}
}