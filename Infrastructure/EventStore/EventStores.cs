using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Domain;

namespace Infrastructure.EventStore {
	public class EventStores {
		public static IEventStore Redis {
			get { return RedisEventStore.Instance; }
		}

		public static IEventStore SqlServer {
			get { return SqlServerEventStore.Instance; }
		}

		public static IEventStore InMemory {
			get { return new InMemoryStore(); }
		}
	}
}
