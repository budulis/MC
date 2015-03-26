using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;

namespace Core
{
	public interface IDomainEventDispather 
	{
		Task Dispatch(IEnumerable<IDomainEventNotificationMessage> evt);
		Task Dispatch(IDomainEventNotificationMessage evt);
		IDomainEventDispather Register(Type type, Func<IDomainEventNotificationMessage, Task> subscriber);
	}
}