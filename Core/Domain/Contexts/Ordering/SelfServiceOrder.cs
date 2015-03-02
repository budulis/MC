using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Events;

namespace Core.Domain.Contexts.Ordering {
	public class SelfServiceOrder : Aggregate<SelfServiceOrder, IDomainEvent> {
		
		public string Name { get; private set; }
		public string Comments { get; private set; }
		public IEnumerable<Product> Products { get; set; }
		public OrderStatus Status { get; set; }
		public Payment Payment { get; private set; }
		public decimal AmountCharged { get; internal set; }
		internal ILogger Logger { get; set; }
		
		internal SelfServiceOrder()
			: base(Id.Null()) {
		}

		public SelfServiceOrder(Id id, IEnumerable<Product> products, string name, string comments)
			: base(id) {
			CreateNewOrder(id, products, name, comments);
		}

		private void CreateNewOrder(Id id, IEnumerable<Product> products, string name, string comments) {
			IDomainEvent evt = new WebOrderCreated(id, products, name, comments);

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		internal override void UpdateFromEvent(IDomainEvent evt) {
			UpdateFromEvent((dynamic)evt);
			base.UpdateFromEvent(evt);
		}

		private void UpdateFromEvent(WebOrderCreated evt) {
			Id = evt.Id;
			Status = OrderStatus.WaitingForPayment;
			Products = evt.Products;
			Name = evt.CustomerName;
			Comments = evt.Comments;
		}
	}
}