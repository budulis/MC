using System;
using System.Collections.Generic;
using Core.Domain.Contexts.Ordering.Events;
using Infrastructure.Services.Payment;

namespace Core.Domain.Contexts.Ordering {
	internal class SelfServiceOrder : Aggregate<SelfServiceOrder, IDomainEvent> {
		public IEnumerable<Product> Products { get; set; }
		public OrderStatus Status { get; set; }
		public ILogger Logger { get; set; }
		public decimal AmountCharged { get; internal set; }
		public double Discount { get; internal set; }
		public string Name { get; private set; }
		public string Comments { get; private set; }
		public string LoyaltyCardNumber { get; private set; }

		internal SelfServiceOrder()
			: base(Id.Null()) {
		}

		internal SelfServiceOrder(ISelfServicePaymentService paymentService, ILogger logger, OrderInfo orderInfo, PaymentInfo paymentInfo)
			: base(orderInfo.Id) {
			Logger = logger;
			CreateNewOrder(paymentService, orderInfo, paymentInfo);
		}

		private void CreateNewOrder(ISelfServicePaymentService paymentService, OrderInfo orderInfo, PaymentInfo paymentInfo) {
			IDomainEvent evt;
			try {
				var result = paymentService.ProcessPayment(orderInfo.Products, paymentInfo.CardNumber, paymentInfo.LoyaltyCardNumber);
				evt = new SelfServiceOrderCreated(orderInfo.Id,
				orderInfo.Products,
				orderInfo.Name,
				orderInfo.Comments,
				paymentInfo.CardNumber,
				paymentInfo.LoyaltyCardNumber,
				result.Discount,
				result.AmountCharged);
			}
			catch (LoyaltyServiceException ex) {
				evt = GetFailEvent(orderInfo.Products, paymentInfo, ex);
				Logger.Error(ex);
			}
			catch (PaymentServiceException ex) {
				evt = GetFailEvent(orderInfo.Products, paymentInfo, ex);
				Logger.Error(ex);
			}

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
			AmountCharged = evt.AmountCharged;
			Discount = evt.Discount;
		}

		public void Complete() {
			IDomainEvent evt = new SelfServiceOrderCompleted(Id);
			UpdateFromEvent(evt);
			ApplyEvent(evt);
		}

		private void UpdateFromEvent(SelfServiceOrderCompleted evt) {
			if (!evt.Id.Equals(Id))
				throw new Exception("Catastrophic failure!");

			if (Status == OrderStatus.Payed)
				Status = OrderStatus.Completed;
			else
				throw new NotSupportedException("Order status must be payed");
		}

		private void UpdateFromEvent(SelfServiceOrderStartFailed evt) {
			Status = OrderStatus.Failed;
		}

		private IDomainEvent GetFailEvent(IEnumerable<Product> products, PaymentInfo paymentInfo, Exception ex) {
			return new SelfServiceOrderStartFailed(Id, products, paymentInfo, ex.Message);
		}
	}
}