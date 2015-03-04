using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Commands;

namespace Core.Handlers {
	public sealed class HandlerForCreateSelfServiceOrder : Handler<CreateSelfServiceOrder> {
		public HandlerForCreateSelfServiceOrder(IInfrastructureService infrastructureService, IDomainEventDispather domainEventDispather)
			: base(infrastructureService.EventStore,
				infrastructureService.DiscountService,
				infrastructureService.CardPaymentService,
				infrastructureService.Logger,
				domainEventDispather) {
		}

		public override async Task Handle(CreateSelfServiceOrder command) {
			var order = new SelfServiceOrder(command.Id, command.Products, command.CustomerName, command.Comments,command.CardNumber) {
				Logger = Logger
			};
			await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);

			await DomainEventDispather.Dispatch(order.Events.Select(x => x.ToMessage()));
		}
	}
}