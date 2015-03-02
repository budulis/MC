using System;
using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Exceptions;

namespace Core.Domain.Contexts.Ordering {
	public class Order : Aggregate<Order, IDomainEvent> {

		public IEnumerable<Product> Products { get; private set; }
		public OrderStatus Status { get; set; }
		public Cashier Cashier { get; private set; }
		public Payment Payment { get; private set; }
		public decimal AmountCharged { get; internal set; }
		public double Discount { get; internal set; }
		internal ILogger Logger { get; set; }

		#region ctor
		internal Order()
			: base(Id.Null()) { }

		public Order(Id id, IEnumerable<Product> products, Cashier cashier, string name, string comments)
			: base(id) {
			CreateNewOrder(id, products, cashier, name, comments);
		}
		#endregion

		public void Complete() {
			IDomainEvent evt = new OrderCompleted(Id);
			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		public void AsignPayment<TPayment>(TPayment payment) where TPayment : Payment {
			if (Status != OrderStatus.WaitingForPayment) {
				throw new OrderException { Reason = OrderException.Code.OrderNotStarted };
			}

			IDomainEvent evt;
			try {
				Cashier.ProcessOrder(this, payment);
				evt = new OrderStarted(Id, Products, payment, AmountCharged, Discount);
			}
			catch (CashierException ex) {
				evt = new OrderStartFailed(Id, Products, payment, AmountCharged, ex.Message);
				Logger.Error(ex.Message);
			}

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		private void CreateNewOrder(Id id, IEnumerable<Product> products, Cashier cashier, string name, string comments) {

			IDomainEvent evt = new OrderCreated(id, products, cashier, name, comments);

			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		internal override void UpdateFromEvent(IDomainEvent evt) {
			UpdateFromEvent((dynamic)evt);
			base.UpdateFromEvent(evt);
		}

		private void UpdateFromEvent(OrderCreated evt) {
			Id = evt.Id;
			Status = OrderStatus.WaitingForPayment;
			Products = evt.Products;
			Cashier = evt.Cashier;
		}

		private void UpdateFromEvent(OrderCompleted evt) {
			if (!evt.Id.Equals(Id))
				throw new Exception("Catastrophic failure!");

			Status = OrderStatus.Completed;
		}

		private void UpdateFromEvent(OrderStarted evt) {
			if (!evt.Id.Equals(Id))
				throw new Exception("Catastrophic failure!");

			Status = OrderStatus.InProcess;
			Payment = evt.Payment;
		}

		private void UpdateFromEvent(OrderStartFailed evt) {
			if (!evt.Id.Equals(Id))
				throw new Exception("Catastrophic failure!");

			Status = OrderStatus.Failed;
			Payment = evt.Payment;
		}
	}
}
