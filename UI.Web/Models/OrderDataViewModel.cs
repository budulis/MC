using System;

namespace UI.Web.Models
{
	public class OrderDataViewModel
	{
		public string Id { get; set; }
		public string Products { get; set; }
		public decimal Total { get; set; }
		public string Status { get; set; }
		public DateTime Created { get; set; }
	}
}