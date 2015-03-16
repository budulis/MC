using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Commands;

namespace Core.Handlers {
	public sealed class HandlerForCreateSelfServiceOrder : Handler<CreateSelfServiceOrder> {
		private readonly IInfrastructureService _infrastructureService;
		public HandlerForCreateSelfServiceOrder(IInfrastructureService infrastructureService, IDomainEventDispather domainEventDispather)
			: base(infrastructureService.EventStore,
				infrastructureService.DiscountService,
				infrastructureService.CardPaymentService,
				infrastructureService.Logger,
				domainEventDispather) {
			_infrastructureService = infrastructureService;
		}

		public override async Task Handle(CreateSelfServiceOrder command) {
			
			var oi = new OrderInfo {
				Comments = command.Comments,
				Id = command.Id,
				Name = command.CustomerName,
				Products = command.Products
			};

			var pi = new PaymentInfo
			{
				CardNumber = command.CardNumber,
				LoyaltyCardNumber = command.LoyaltyCardNumber
			};

			var order = new SelfServiceOrder(_infrastructureService.SelfServicePaymentService, Logger, oi, pi);
			await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);

			await DomainEventDispather.Dispatch(order.Events.Select(x => x.ToMessage()));
		}
	}
}