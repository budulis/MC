using System.Collections.Generic;
using System.Linq;

namespace UI.Web.Models
{
	public class OrderSummaryViewModel {
		public IEnumerable<OrderDataViewModel> Orders { get; set; }

		public OrderSummaryViewModel() {
			Orders = Enumerable.Empty<OrderDataViewModel>();
		}
	}
}