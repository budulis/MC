using Core;
using Core.Domain;
using Infrastructure.DataBase;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.EventStore
{
	internal class SqlServerEventStore : IEventStore {
		private readonly DataBaseContext _db;

		class StoreEvent {
			public string Type { get; set; }
			public string Data { get; set; }
		}

		public ILogger Logger { get; set; }

		public static SqlServerEventStore Instance { get; private set; }

		private SqlServerEventStore() {
			Logger = new NullLogger();
			_db = new DataBaseContext(Logger);
		}

		static SqlServerEventStore() {
			Instance = new SqlServerEventStore();
		}

		public async Task AddAsync(Type aggregateType, Id aggregateId, IDomainEvent[] events, int eventsLoaded) {
			var key = String.Format("{0}:{1}", aggregateType.FullName, aggregateId);

			if (eventsLoaded - await _db.SelectEventCountByKey.ExecuteAsync(key) != events.Count()) {
				throw new Exception("Concurrency exception!");
			}

			var settings = new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Objects
			};

			var eventData = from e in events
				let evt = JsonConvert.SerializeObject(e, settings)
				select JsonConvert.SerializeObject(new StoreEvent {Data = evt, Type = e.GetType().AssemblyQualifiedName});

			await _db.InsertEvents.ExecuteAsync(aggregateId.ToString(), key, eventData);
		}

		public async Task<IEnumerable<TDomainEvent>> GetAsync<TDomainEvent>(Type aggregateType, Id aggregateId) where TDomainEvent : IDomainEvent {
			
			var key = String.Format("{0}:{1}", aggregateType.FullName, aggregateId);
			var events = await _db.SelectAllEventsByKey.ExecuteAsync(key);

			var settings = new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Objects
			};

			var result = new List<TDomainEvent>();

			foreach (var evt in events) {
				var storeEvent = JsonConvert.DeserializeObject<StoreEvent>(evt, settings);
				var eventType = Type.GetType(storeEvent.Type);
				result.Add((TDomainEvent)JsonConvert.DeserializeObject(storeEvent.Data, eventType, settings));
			}

			return result;
		}
	}
}