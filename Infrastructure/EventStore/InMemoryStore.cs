using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Newtonsoft.Json;

namespace Infrastructure.EventStore {
	internal class InMemoryStore : IEventStore {

		class StoreEvent {
			public int Sequence { get; set; }
			public Id AggregateId { get; set; }
			public string AggregateTypeName { get; set; }
			public IDomainEvent Data { get; set; }
			public string StringData { get; set; }
		}

		private readonly static object SyncBlock = new object();

		private static readonly List<StoreEvent> Events;

		static InMemoryStore() {
			Events = new List<StoreEvent>();
		}

		public Task AddAsync(Type aggregateType, Id aggregateId, IDomainEvent[] events, int eventsLoaded) {
			lock (SyncBlock) {
				var currentSequence =
					Events.Where(x => x.AggregateId.Equals(aggregateId)
														&& x.AggregateTypeName.Equals(aggregateType.FullName))
						.OrderByDescending(x => x.Sequence)
						.Select(x => x.Sequence)
						.DefaultIfEmpty(0)
						.First();

				if (eventsLoaded - currentSequence != events.Count()) {
					throw new Exception("Concurrency exception!");
				}

				foreach (var domainEvent in events) {
					Events.Add(new StoreEvent {
						AggregateId = aggregateId,
						Data = domainEvent,
						AggregateTypeName = aggregateType.FullName,
						Sequence = (currentSequence + 1),
						StringData = JsonConvert.SerializeObject(domainEvent)
					});
				}
				return Task.FromResult(true);
			}
		}

		public Task<IEnumerable<TDomainEvent>> GetAsync<TDomainEvent>(Type aggregateType, Id aggregateId) where TDomainEvent : IDomainEvent
		{
			lock (SyncBlock)
			{
				 return Task.FromResult(
					(IEnumerable<TDomainEvent>)Events.Where(x => x.AggregateId.Equals(aggregateId) && x.AggregateTypeName.Equals(aggregateType.FullName))
						.Select(x => (TDomainEvent) x.Data).ToArray());
			}
		}
	}
}