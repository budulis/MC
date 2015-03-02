using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Production.Events;

namespace Core.Domain.Contexts.Production {
	public class ProductionOrder : Aggregate<ProductionOrder, IDomainEvent> {
		public OrderStatus Status { get; private set; }
		public Chef Chef { get; private set; }

		public IEnumerable<Product> Products { get; private set; }

		internal ProductionOrder()
			: base(Id.Null()) {
			Products = Enumerable.Empty<Product>();
			Chef = null;
		}

		internal ProductionOrder(Id id, IEnumerable<Product> products, Chef chef)
			: base(id) {
			CreateNewOrder(id, products, chef);
		}

		private void CreateNewOrder(Id id, IEnumerable<Product> products, Chef chef) {

			IDomainEvent evt = new ProductionOrderCreated(id, products, chef);

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		public void Complete() {
			IDomainEvent evt = new ProductionOrderCompleted(Id, Chef);

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

			Status = OrderStatus.InProcess;
		}

		private void UpdateFromEvent(ProductionOrderCompleted evt) {
			if (evt == null || evt.Id.Equals(Id.Null()))
				throw new Exception("Catastrophic failure!");

			evt.Chef.Cook(this);

			Status = OrderStatus.Completed;
		}
	}
}