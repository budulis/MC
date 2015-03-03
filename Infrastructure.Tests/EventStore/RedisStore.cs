using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Tests.EventStore {
	[TestClass]
	public class RedisStore {
		[TestMethod]
		public void Should_Store_An_Event() {
			var id = Id.New();
			var e = new OrderCreated(id, new[] { new Product("1", "Test Product", 25.4M) },null,null,null);

			var store = Infrastructure.EventStore.EventStores.Redis;

			store.AddAsync(typeof(Order), id, new IDomainEvent[] { e }, 1).ConfigureAwait(false);
		}
	}
}
