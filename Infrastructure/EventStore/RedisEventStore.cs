using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Services.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.EventStore {
	internal class RedisEventStore : IEventStore {
		class StoreEvent {
			public string Type { get; set; }
			public string Data { get; set; }
		}

		private readonly ConnectionMultiplexer _redis;
		public ILogger Logger { get; set; }

		public static RedisEventStore Instance { get; private set; }

		private RedisEventStore() {
			_redis = ConnectionMultiplexer.Connect("localhost");
			Logger = new NullLogger();
		}

		static RedisEventStore() {
			Instance = new RedisEventStore();
		}

		public async Task AddAsync(Type aggregateType, Id aggregateId, IDomainEvent[] events, int eventsLoaded) {
			var db = _redis.GetDatabase();
			var key = String.Format("{0}:{1}", aggregateType.FullName, aggregateId);

			if (eventsLoaded - await db.ListLengthAsync(key) != events.Count()) {
				throw new Exception("Concurrency exception!");
			}

			var transaction = db.CreateTransaction();

			var settings = new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Objects
			};

			foreach (var domainEvent in events) {
				Logger.Audit(new { key, domainEvent });
				var evt = JsonConvert.SerializeObject(domainEvent, settings);
				await db.ListRightPushAsync(key, JsonConvert.SerializeObject(new StoreEvent { Data = evt, Type = domainEvent.GetType().AssemblyQualifiedName }));
			}

			await transaction.ExecuteAsync();
		}

		public async Task<IEnumerable<TDomainEvent>> GetAsync<TDomainEvent>(Type aggregateType, Id aggregateId) where TDomainEvent : IDomainEvent {
			var db = _redis.GetDatabase();
			var key = String.Format("{0}:{1}", aggregateType.FullName, aggregateId);
			var events = await db.ListRangeAsync(key);

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