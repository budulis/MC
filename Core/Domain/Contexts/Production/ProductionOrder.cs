using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Production.Commands;
using Core.Domain.Contexts.Production.Events;

namespace Core.Domain.Contexts.Production {
	public class ProductionOrder : Aggregate<ProductionOrder, IDomainEvent> {
		public OrderStatus Status { get; private set; }
		public Chef Chef { get; private set; }
		public OrderType OrderType { get; private set; }
		public IEnumerable<Product> Products { get; private set; }

		internal ProductionOrder()
			: base(Id.Null()) {
			Products = Enumerable.Empty<Product>();
			Chef = null;
		}

		internal ProductionOrder(Id id, IEnumerable<Product> products, Chef chef, OrderType orderType)
			: base(id) {
			CreateNewOrder(id, products, chef,orderType);
		}

		private void CreateNewOrder(Id id, IEnumerable<Product> products, Chef chef, OrderType orderType) {

			IDomainEvent evt = new ProductionOrderCreated(id, products, chef, orderType);

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		public void Complete() {
			IDomainEvent evt = new ProductionOrderCompleted(Id, Chef,OrderType);

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		internal override void UpdateFromEvent(IDomainEvent evt) {
			UpdateFromEvent((dynamic)evt);
			base.UpdateFromEvent(evt);
		}

		private void UpdateFromEvent(ProductionOrderCreated evt) {
			if (evt == null || evt.Id.Equals(Id.Null()))
				throw new Exception("Catastrophic failure!");

			Id = evt.Id;
			Products = evt.Products;
			Chef = evt.Chef;
			OrderType = evt.OrderType;
			Status = OrderStatus.InProcess;
		}

		private void UpdateFromEvent(ProductionOrderCompleted evt) {
			if (evt == null || evt.Id.Equals(Id.Null()))
				throw new Exception("Catastrophic failure!");

			evt.Chef.Cook(this);
			OrderType = evt.OrderType;
			Status = OrderStatus.Completed;
		}
	}
}