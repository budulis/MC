using System.Threading.Tasks;
using Core.Domain;

namespace Core.Handlers
{
	public abstract class Handler<TCommand> : IHandler<TCommand> where TCommand : IDomainCommand {
		protected IEventStore Store { get; set; }
		protected IDiscountService DiscountService { get; set; }
		protected ICardPaymentService CardPaymentService { get; set; }
		protected ILogger Logger { get; set; }
		protected IDomainEventDispather DomainEventDispather { get; set; }

		protected Handler(IEventStore store,
			IDiscountService discountService,
			ICardPaymentService cardPaymentService,
			ILogger logger,
			IDomainEventDispather domainEventDispather) {
			Store = store;
			DiscountService = discountService;
			CardPaymentService = cardPaymentService;
			Logger = logger;
			DomainEventDispather = domainEventDispather;
			}

		public abstract Task Handle(TCommand command);
	}
}