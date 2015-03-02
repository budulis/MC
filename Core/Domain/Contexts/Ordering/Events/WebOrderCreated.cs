using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Messages;

namespace Core.Domain.Contexts.Ordering.Events
{
	public class WebOrderCreated : IDomainEvent
	{
		public Id Id { get; private set; }
		public IEnumerable<Product> Products { get; private set; }
		public string CustomerName { get; private set; }
		public string Comments { get; private set; }
		
		public WebOrderCreated(Id id, IEnumerable<Product> products, string customerName, string comments)
		{
			Comments = comments;
			CustomerName = customerName;
			Products = products;
			Id = id;
		}

		public IDomainEventNotificationMessage ToMessage()
		{
			return new SelfServiceOrderCreatedNotificationMessage
			{
				Id = Id,
				Products = Products,
				CustomerName = CustomerName,
				Comments = Comments
			};
		}
	}
}