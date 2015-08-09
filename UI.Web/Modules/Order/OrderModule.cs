using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Events;
using Core.Domain.Contexts.Ordering.Messages;
using Infrastructure.Dispatchers;
using Infrastructure.Services.Product;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using UI.Web.Hubs;
using UI.Web.Models;

namespace UI.Web.Modules.Order {

	public class OrderModule : NancyModule {
		private readonly IItemInfoRepository<ProductInfo> _inventoryItemRepository;
		private readonly ILogger _logger;
		private readonly IDomainCommandDispatcher _commandDispatcher;
		public OrderModule(IItemInfoRepository<ProductInfo> inventoryItemRepository, ILogger logger) {
			_inventoryItemRepository = new CachedProductInfoRepository(inventoryItemRepository);
			_logger = logger;
			_commandDispatcher = Bootstrapper.GetCommandDispatcher(logger, () => _commandDispatcher);

			Get["/Order", true] = async (p, ct) => View[await GetOrderViewModel()];
			Post["/Order", true] = async (p, ct) => await PostOrderViewModel(p, ct);
		}

		private async Task<Response> PostOrderViewModel(dynamic p, CancellationToken ct) {
			var viewModel = this.Bind<PostedOrder>();
			var result = this.Validate(viewModel);

			if (!result.IsValid)
				return await GetValidationError(result);

			var products = new List<Product>();

			foreach (var id in viewModel.ProductIDs) {
				var item = await _inventoryItemRepository.GetByIdAsync(id);
				var prd = new Product(id, item.Name, item.Price);
				products.Add(prd);
			}

			var orderId = Id.New();
			var command = viewModel.ToCommand(orderId, products.ToArray());
			return await SendCommand(command);
		}

		private Task<Response> GetValidationError(ModelValidationResult result)
		{
			return Task.FromResult(new Response {
				StatusCode = HttpStatusCode.BadRequest,
				ReasonPhrase = result.Errors.Values.Select(x=>x.First().ErrorMessage).Aggregate((x,y)=>x + "<br/>" + y)
			});
		}

		private Task<Response> SendCommand(IDomainCommand command) {
			Task<Response> response;

			try {
				_commandDispatcher.Dispatch(command);
				
				response = Task.FromResult(new Response {
					StatusCode = HttpStatusCode.Accepted,
					Headers = new Dictionary<string, string> { { "location", command.Id.ToString() } }
				});
			}
			catch (Exception ex) {
				Console.WriteLine(ex);
				response = Task.FromResult(new Response {
					StatusCode = HttpStatusCode.InternalServerError,
					Headers = new Dictionary<string, string> { { "location", command.Id.ToString() } }
				});
			}

			return response;
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
