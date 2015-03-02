using System.Collections.Generic;
using System.Linq;

namespace UI.Web.Models
{
	public class PostedOrder {
		public string[] ProductIDs { get; set; }
		public string CustomerName { get; set; }
		public string Comments { get; set; }
	}

	public class OrderViewModel
	{
		public IEnumerable<ProductViewModel> AvailableProducts { get; set; }
		public IEnumerable<ProductViewModel> SelectedProducts { get; set; }
		public string Comments { get; set; }

		public OrderViewModel()
		{
			AvailableProducts = Enumerable.Empty<ProductViewModel>();
			SelectedProducts = Enumerable.Empty<ProductViewModel>();
		}
	}
}