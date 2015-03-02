using System;
using System.Runtime.Serialization;

namespace Core.Domain.Contexts.Ordering.Exceptions
{
	[Serializable]
	class CashierException : DomainException {
		public CashierException() {

		}

		public CashierException(string msg)
			: base(msg) {
			}

		public CashierException(string msg, DomainException ex)
			: base(msg, ex) {
			}

		public CashierException(SerializationInfo si, StreamingContext context)
			: base(si, context) {
			}
	}
}