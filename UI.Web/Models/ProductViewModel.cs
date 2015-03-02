using System;
using System.Text;
using System.Threading.Tasks;

namespace UI.Web.Models {
	public class ProductViewModel {
		public string Id { get; set; }
		public string Name { get; set; }
		public string Price { get; set; }
		public bool IsSelected { get; set; }
	}
}
