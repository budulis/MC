using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;

namespace Core {
	public interface IEventStore {
		Task AddAsync(Type aggregateType,Id aggregateId, IDomainEvent[] events, int eventsLoaded);
		Task<IEnumerable<TDomainEvent>> GetAsync<TDomainEvent>(Type aggregateType, Id aggregateId) where TDomainEvent : IDomainEvent;
	}
}
