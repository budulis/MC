using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Contexts.Ordering;
using Core.Domain.Contexts.Ordering.Commands;

namespace Core.Handlers
{
	public sealed class HandlerForCreateOrder : Handler<CreateOrder>
	{
		private readonly IInfrastructureService _infrastructureService;
		public HandlerForCreateOrder(IInfrastructureService infrastructureService, IDomainEventDispather domainEventDispather)
			: base(infrastructureService.EventStore, 
			infrastructureService.DiscountService, 
			infrastructureService.CardPaymentService, 
			infrastructureService.Logger, 
			domainEventDispather)
		{
			_infrastructureService = infrastructureService;
		}

		public override async Task Handle(CreateOrder command) {

			var cashier = _infrastructureService.Cashiers.GetById(new Id(Guid.Parse(command.CashierId)));
			cashier.DiscountService = _infrastructureService.DiscountService;
			cashier.PaymentService = _infrastructureService.CardPaymentService;

			var order = new Order(command.Id, command.Products, cashier,command.CustomerName,command.Comments) { Logger = Logger };
			await Store.AddAsync(order.GetType(), order.Id, order.Events.ToArray(), order.CurrentSequenceNumber);

			await DomainEventDispather.Dispatch(order.Events.Select(x => x.ToMessage()));
		}
	}
}