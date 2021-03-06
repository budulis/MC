using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Production;
using Core.Domain.Contexts.Production.Commands;

namespace Core.Handlers
{
	public sealed class HandlerForCreateProductionOrder : Handler<CreateProductionOrder> {
		private readonly IInfrastructureService _infrastructureService;

		public HandlerForCreateProductionOrder(IInfrastructureService infrastructureService, IDomainEventDispather domainEventDispather)
			: base(infrastructureService.EventStore, infrastructureService.DiscountService, infrastructureService.CardPaymentService, infrastructureService.Logger, domainEventDispather)
		{
			_infrastructureService = infrastructureService;
		}

		public override async Task Handle(CreateProductionOrder command) {
			var chef = _infrastructureService.Chefs.GetByName("Olivia");
			chef.Logger = Logger;

			var order = new ProductionOrder(command.Id, command.Products, chef,command.OrderType);
			await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);
			await DomainEventDispather.Dispatch(order.Events.Select(x => x.ToMessage()));
		}
	}
}