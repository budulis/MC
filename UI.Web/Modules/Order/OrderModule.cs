using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Commands;
using Infrastructure.Dispatchers;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Product;
using Nancy;
using Nancy.ModelBinding;
using Nancy.ModelBinding.DefaultBodyDeserializers;
using Nancy.Responses;
using Nancy.ViewEngines.Razor;
using UI.Web.Models;

namespace UI.Web.Modules.Order {

	public class OrderModule : NancyModule {
		private readonly IItemInfoRepository<ProductInfo> _inventoryItemRepository;

		public OrderModule(IItemInfoRepository<ProductInfo> inventoryItemRepository) {
			_inventoryItemRepository = inventoryItemRepository;

			Get["/Order"] = p => View[GetOrderViewModel()];
			Post["/Order", true] =  async (p,ct) => {

				var viewModel = this.Bind<PostedOrder>();
				var products = new List<Product>();

				foreach (var id in viewModel.ProductIDs) {
					var item = await _inventoryItemRepository.GetByIdAsync(id);
					var prd = new Product(id, item.Name, item.Price);
					products.Add(prd);
				}

				var orderId = Id.New();
				var command = new CreateSelfServiceOrder(orderId, products.ToArray(), viewModel.CustomerName, viewModel.Comments);

				Task<Response> response;

				try {
					await CommandDispatcher.GetQueued(LoggerFactory.Default).Dispatch(command);

					response = Task.FromResult(new Response {
						StatusCode = HttpStatusCode.Accepted,
						Headers = new Dictionary<string, string> { { "location", orderId.ToString() } }
					});
				}
				catch (Exception ex) {
					Console.WriteLine(ex);
					response = Task.FromResult(new Response {
						StatusCode = HttpStatusCode.InternalServerError,
						Headers = new Dictionary<string, string> { { "location", orderId.ToString() } }
					});
				}

				return response;
			};
		}

		private async Task<OrderViewModel> GetOrderViewModel() {
			var products = (await _inventoryItemRepository.GetAllAsync()).Select(x => new ProductViewModel {
				Id = x.Id,
				Name = x.Name,
				Price = x.Price.ToString("#.00")
			});

			return new OrderViewModel { AvailableProducts = products };
		}
	}
}
