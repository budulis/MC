using System;

namespace Core.ReadModel {
	public class OrderReadModel : IReadModel {
		public string Id { get; set; }
		public string Products { get; set; }
		public decimal Total { get; set; }
		public string Status { get; set; }
		public DateTime Created { get; set; }

		public override string ToString() {
			return String.Format("{0} [{1}] {2} [{3}]", Id, Products, Total,Status);
		}
	}
}