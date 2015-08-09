using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Commands;
using Nancy.Validation;
using Nancy.Validation.FluentValidation;


namespace UI.Web.Models
{
	public class PostedOrder {
		public string[] ProductIDs { get; set; }
		public string CustomerName { get; set; }
		public string Comments { get; set; }
		public string CardNumber { get; set; }
		public string LoyaltyCardNumber { get; set; }

		internal IDomainCommand ToCommand(Id orderId,Product[] products)
		{
			return new CreateSelfServiceOrder(orderId, products, CustomerName, Comments, CardNumber, LoyaltyCardNumber);
		}
	}
}