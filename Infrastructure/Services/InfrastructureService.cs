using Core;

namespace Infrastructure.Services
{
	public class InfrastructureService : IInfrastructureService{
		public IEventStore EventStore { get; private set; }
		public IDiscountService DiscountService { get; private set; }
		public ICardPaymentService CardPaymentService { get; private set; }
		public ILogger Logger { get; private set; }
		public ICashierRepository Cashiers { get; private set; }
		public IChefRepository Chefs { get; private set; }

		public InfrastructureService(IEventStore eventStore, IDiscountService discountService, ICardPaymentService cardPaymentService, ILogger logger, ICashierRepository cashiers, IChefRepository chefs)
		{
			Chefs = chefs;
			Cashiers = cashiers;
			Logger = logger;
			CardPaymentService = cardPaymentService;
			DiscountService = discountService;
			EventStore = eventStore;
		}

	}
}