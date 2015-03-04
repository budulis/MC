using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Events;

namespace Core.Domain.Contexts.Ordering {
	public class SelfServiceOrder : Aggregate<SelfServiceOrder, IDomainEvent> {
		public string Name { get; private set; }
		public string Comments { get; private set; }
		public IEnumerable<Product> Products { get; set; }
		public OrderStatus Status { get; set; }
		public Cashier Cashier { get; private set; }
		public Payment Payment { get; private set; }
		public decimal AmountCharged { get; internal set; }
		internal ILogger Logger { get; set; }

		internal SelfServiceOrder()
			: base(Id.Null())
		{
		}

		public SelfServiceOrder(Id id, IEnumerable<Product> products, string name, string comments, string cardNumber)
			: base(id)
		{
			CreateNewOrder(id,products, name, comments, cardNumber);
		}

		private void CreateNewOrder(Id id,IEnumerable<Product> products, string name, string comments, string cardNumber) {

			
			IDomainEvent evt = new SelfServiceOrderCreated(id, products, name, comments, cardNumber);



			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		internal override void UpdateFromEvent(IDomainEvent evt) {
			UpdateFromEvent((dynamic)evt);
			base.UpdateFromEvent(evt);
		}

		private void UpdateFromEvent(SelfServiceOrderCreated evt) {
			Id = evt.Id;
			Status = OrderStatus.WaitingForPayment;
			Products = evt.Products;
			Name = evt.CustomerName;
			Comments = evt.Comments;
		}
	}
}