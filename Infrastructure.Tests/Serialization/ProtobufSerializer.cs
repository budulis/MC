using System;
using System.Collections.Generic;
using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Messages;
using Infrastructure.Initialization;
using Infrastructure.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf.Meta;

namespace Infrastructure.Tests.Serialization {
	[TestClass]
	public class ProtobufSerializer {

		[TestInitialize]
		public void Initialize() {
			Bootstrapper.Default.RunInitializationTasks();
		}

		[TestMethod]
		public void Should_Serialize_SimpleType() {

			//var product1 = new Product("aaa", "Some product", 45.44M);
			//var product2 = new Product("aaa", "Some product", 45.44M);
			//var product3 = new Product("aaa", "Some product", 45.44M);

			//ISerializer<IEnumerable<Product>> serializer = Serializer<IEnumerable<Product>>.ProtoBuf;

			//var result = serializer.Serialize(new[] { product1, product2, product3 });

			//Assert.IsNotNull(result);
		}
		[TestMethod]
		public void Should_Deserialize_SimpleType() {

			//var product1 = new Product("aaa", "Some product", 45.44M);
			//var product2 = new Product("aaa", "Some product", 45.44M);
			//var product3 = new Product("aaa", "Some product", 45.44M);

			//ISerializer<IEnumerable<Product>> serializer = Serializer<IEnumerable<Product>>.ProtoBuf;

			//var serializedResult = serializer.Serialize(new[] { product1, product2, product3 });

			//IDeserializer<IEnumerable<Product>> deserializer = Serializer<IEnumerable<Product>>.ProtoBuf;

			//var deserializedResult = deserializer.Deserialize(serializedResult);

			//Assert.IsNotNull(deserializedResult);
		}
		[TestMethod]
		public void Shoul_Serialize_ComplexType() {
			//var id = Id.New();
			//var e = new OrderCreatedNotificationMessage
			//{
			//	Id = id, 
			//	Products = new[] { new Product("1", "Test Product", 25.4M) }, 
			//	CashierId = Id.New()
			//};

			//ISerializer<OrderCreatedNotificationMessage> serializer = Serializer<OrderCreatedNotificationMessage>.ProtoBuf;

			//var result = serializer.Serialize(e);

			//Assert.IsNotNull(result);
		}
		[TestMethod]
		public void Shoul_Derialize_ComplexType() {
			//var id = Id.New();
			//var e = new OrderCreatedNotificationMessage {
			//	Id = id,
			//	Products = new[] { new Product("1", "Test Product", 25.4M) },
			//	CashierId = Id.New()
			//};

			//ISerializer<OrderCreatedNotificationMessage> serializer = Serializer<OrderCreatedNotificationMessage>.ProtoBuf;

			//var result = serializer.Serialize(e);
			//IDeserializer<IDomainEventNotificationMessage> deserializer = Serializer<IDomainEventNotificationMessage>.ProtoBuf;

			//var d = deserializer.Deserialize(result);
			//Assert.IsNotNull(d.Id);
		}
	}
}
