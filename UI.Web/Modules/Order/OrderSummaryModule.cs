using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ReadModel;
using Nancy;
using UI.Web.Models;

namespace UI.Web.Modules.Order {
	public class OrderSummaryModule : NancyModule {
		private readonly IReadModelRepository<OrderReadModel> _repository;

		public OrderSummaryModule(IReadModelRepository<OrderReadModel> repository) {
			_repository = repository;
			Get["/Orders/Summary/{top}", c => c.Request.Query.Top != null, true] = async (p, ct) => View[new OrderSummaryViewModel { Orders = await GetOrders((int)p.top) }];
			Get["/Orders/Summary", true] = async (p, ct) => View[new OrderSummaryViewModel { Orders = await GetOrders() }];
		}

		private async Task<IEnumerable<OrderDataViewModel>> GetOrders(int top = 100) {
			return (await _repository.GetAllAsync(top)).GroupBy(x => new { x.Id, x.Products, x.Total }).Select(x => new OrderDataViewModel {
				Id = x.Key.Id,
				Products = x.Key.Products,
				Total = x.Key.Total,
				Status = x.First().Status,
				Created = x.Max(z => z.Created)
			});
		}
	}
}