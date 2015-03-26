using System;
using System.Collections.Generic;
using System.Linq;
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
using UI.Web.Models;

namespace UI.Web.Modules.Order {

	public class OrderModule : NancyModule {
		private readonly IItemInfoRepository<ProductInfo> _inventoryItemRepository;
		private readonly ILogger _logger;
		private IDomainCommandDispatcher _commandDispatcher;
		public OrderModule(IItemInfoRepository<ProductInfo> inventoryItemRepository, ILogger logger) {
			_inventoryItemRepository = inventoryItemRepository;
			_logger = logger;

			Get["/Order", true] = async (p, ct) => View[await GetOrderViewModel()];
			Post["/Order", true] = async (p, ct) => await PostOrderViewModel(p, ct);
		}

		private async Task<Response> PostOrderViewModel(dynamic p, CancellationToken ct) {
			var viewModel = this.Bind<PostedOrder>();

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

		private Task<Response> SendCommand(IDomainCommand command) {
			Task<Response> response;

			try {
				var appEventDispatcher = EventDispathers.Application.GetQueued(_logger);
				var domainEventDispatcher = EventDispathers.Domain.GetDirect(() => _commandDispatcher, () => appEventDispatcher, _logger);
				domainEventDispatcher = domainEventDispatcher.Register(typeof(OrderCompletedNotificationMessage), x => new OnOrderCompleted().Notify((OrderCompletedNotificationMessage)x));
				domainEventDispatcher = domainEventDispatcher.Register(typeof(SelfServiceOrderStartFailedNotificationMessage), x => new OnOrderFailure().Notify((SelfServiceOrderStartFailedNotificationMessage)x));

				_commandDispatcher = CommandDispatchers.GetDirect(domainEventDispatcher, _logger);
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
			var products = (await new CachedProductInfoRepository(_inventoryItemRepository).GetAllAsync()).Select(x => new ProductViewModel {
				Id = x.Id,
				Name = x.Name,
				Price = x.Price.ToString("#.00")
			});

			return new OrderViewModel { AvailableProducts = products };
		}
	}
}
