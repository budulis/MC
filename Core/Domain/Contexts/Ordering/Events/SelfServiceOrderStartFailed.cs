using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events
{
	public class SelfServiceOrderStartFailed : IDomainEvent {
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public PaymentInfo PaymentInfo { get; set; }
		public string Reason { get; private set; }

		public SelfServiceOrderStartFailed(Id id, IEnumerable<Product> products, PaymentInfo paymentInfo,string reason) {
			Reason = reason;
			Products = products;
			PaymentInfo = paymentInfo;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage() {
			return new SelfServiceOrderStartFailedNotificationMessage {
				Id = Id,
				Products = Products,
				Payment = PaymentInfo.ToString(),
				Reason = Reason
			};
		}
	}
}