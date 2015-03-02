using System;
using System.Threading;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Messages;
using Infrastructure.Queuing.InMemory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Tests.Queueing {
	[TestClass]
	public class InMemoryBrokerTest {
		[TestInitialize]
		public void Init() {
			SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
		}

		[TestMethod]
		public void Should_Post_Item() {
			IDomainEventNotificationMessage command = new OrderCreatedNotificationMessage { Id = Id.New() };
			//Broker.Instance.Accept(command);
		}

		[TestMethod]
		public void Should_Post_And_Get_Item() {
			IDomainEventNotificationMessage command = new OrderCreatedNotificationMessage { Id = Id.New() };
			//Broker.Instance.Accept(command);
			//var s = Broker.Instance.Consume<IDomainEventNotificationMessage>();
			//Assert.AreEqual(command.Id, s.Id);
		}
	}
}
