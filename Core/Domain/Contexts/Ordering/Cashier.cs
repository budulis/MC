using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Contexts.Ordering.Exceptions;

namespace Core.Domain.Contexts.Ordering {
	public class Cashier : IEntity<Cashier>, ISerializable
	{

		public Id Id { get; private set; }
		public string Name { get; private set; }

		public IDiscountService DiscountService { get; set; }
		public ICardPaymentService PaymentService { get; set; }

		private readonly List<KeyValuePair<Type, Delegate>> _paymentProcessors;

		public Cashier(Id id, string name)
		{
			Id = id;
			Name = name;
			DiscountService = null;
			PaymentService = null;
			_paymentProcessors = new List<KeyValuePair<Type, Delegate>>(2)
			{
				new KeyValuePair<Type, Delegate>(typeof (CashPayment), new Action<decimal, CashPayment>(ProcessPayment)),
				new KeyValuePair<Type, Delegate>(typeof (CardPayment), new Action<decimal, CardPayment>(ProcessPayment))
			};
		}

		public void ProcessOrder<TPayment>(Order order, TPayment payment) where TPayment : Payment
		{
			var action = _paymentProcessors.Single(p => p.Key == payment.GetType()).Value;

			decimal amountToPay;
			var discount = 0D;

			if (DiscountService != null)
				amountToPay = DiscountService.ApplyDiscount(payment.LoyaltyCard, order.Products.Sum(x => x.Price), out discount);
			else
				amountToPay = order.Products.Sum(x => x.Price);

			order.Discount = discount;

			try
			{
				((Action<decimal, TPayment>) action).Invoke(amountToPay, payment);
				order.AmountCharged = amountToPay;
			}
			catch (PaymentServiceException ex)
			{
				throw new CashierException(ex.Message);
			}

		}

		public bool Equals(Cashier other)
		{
			return Id.Equals(other.Id);
		}

		private void ProcessPayment(decimal amountToPay, CashPayment payment)
		{
			Validate(amountToPay, payment);
		}

		private void ProcessPayment(decimal amountToPay, CardPayment payment)
		{
			if (PaymentService == null)
				throw new PaymentServiceException("Payment service unavailable");

			PaymentService.ProcessPayment(amountToPay, payment.CardNumber);
		}

		private static void Validate(decimal amountToPay, CashPayment payment)
		{
			if (payment.Amount <= 0)
				throw new CashierException("The payment amount is not valid")
				{
					Context = new {amountToPay, payment}
				};

			if (payment.Amount < amountToPay)
			{
				throw new CashierException("Not enough money")
				{
					Context = new {amountToPay, payment}
				};
			}
		}
	}
}
