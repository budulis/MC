using System;
using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Events;

namespace Core.Domain.Contexts.Ordering {
	public class SelfServiceOrder : Aggregate<SelfServiceOrder, IDomainEvent> {
		public IEnumerable<Product> Products { get; set; }
		public OrderStatus Status { get; set; }
		internal ILogger Logger { get; set; }
		public decimal AmountCharged { get; internal set; }
		public double Discount { get; internal set; }
		public string Name { get; private set; }
		public string Comments { get; private set; }
		public string LoyaltyCardNumber { get; private set; }

		internal SelfServiceOrder()
			: base(Id.Null()) {
		}

		internal SelfServiceOrder(ISelfServicePaymentService paymentService, OrderInfo orderInfo, PaymentInfo paymentInfo)
			: base(orderInfo.Id) {
			CreateNewOrder(paymentService, orderInfo, paymentInfo);
		}

		private void CreateNewOrder(ISelfServicePaymentService paymentService, OrderInfo orderInfo, PaymentInfo paymentInfo) {

			IDomainEvent evt = new SelfServiceOrderCreated(orderInfo.Id, orderInfo.Products, orderInfo.Name, orderInfo.Comments, paymentInfo.CardNumber, paymentInfo.LoyaltyCardNumber);

			paymentService.ProcessPayment(this, orderInfo.Products, paymentInfo.CardNumber, paymentInfo.LoyaltyCardNumber);

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		internal override void UpdateFromEvent(IDomainEvent evt) {
			UpdateFromEvent((dynamic)evt);
			base.UpdateFromEvent(evt);
		}

		private void UpdateFromEvent(SelfServiceOrderCreated evt) {
			Id = evt.Id;
			Status = OrderStatus.Payed;
			Products = evt.Products;
			Name = evt.CustomerName;
			Comments = evt.Comments;
			LoyaltyCardNumber = evt.LoyaltyCardNumber;
		}

		public void Complete() {
			IDomainEvent evt = new OrderCompleted(Id);
			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		private void UpdateFromEvent(OrderCompleted evt) {
			if (!evt.Id.Equals(Id))
				throw new Exception("Catastrophic failure!");

			Status = OrderStatus.Completed;
		}
	}
}