namespace UI.Web.Models
{
	public class PostedOrder {
		public string[] ProductIDs { get; set; }
		public string CustomerName { get; set; }
		public string Comments { get; set; }
		public string CardNumber { get; set; }
		public string LoyaltyCardNumber { get; set; }
	}
}