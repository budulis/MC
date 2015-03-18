using Core;
using Infrastructure.EventStore;
using Infrastructure.Services;
using Infrastructure.Services.Discount;
using Infrastructure.Services.Payment;
using Infrastructure.Services.Staff;

namespace Infrastructure.Dispatchers {
	public class CommandDispatchers {
		private static readonly object SyncRoot;
		private static IDomainCommandDispatcher _queued;

		static CommandDispatchers() {
			SyncRoot = new object();
		}

		public static IDomainCommandDispatcher GetDirect(IDomainEventDispather domainEventDispather, ILogger logger) {

			IEventStore eventStore = EventStores.Redis;
			IDiscountService discountService = new DiscountService();
			ICardPaymentService paymentService = new CardPaymentService();
			ICashierRepository cashiers = new CashierRepository();
			IChefRepository chefs = new ChefRepository();
			ILoyaltyProgrammService loyalstyService = new LoyaltyProgrammService();
			ISelfServicePaymentService selfServicepaymentService = new SelfServicePaymentService(discountService, paymentService, loyalstyService);
			IInfrastructureService infrastructureService = new InfrastructureService(eventStore, discountService, paymentService, logger, cashiers, chefs, selfServicepaymentService);

			return new DirectCommandDispatcher(infrastructureService, domainEventDispather);
		}

		public static IDomainCommandDispatcher GetDirect(IInfrastructureService services, IDomainEventDispather eventDispather) {
			return new DirectCommandDispatcher(services, eventDispather);
		}

		public static IDomainCommandDispatcher GetQueued(ILogger logger) {

			lock (SyncRoot) {
				if (_queued == null)
					_queued = new QueuedCommandDispatcher(logger);
			}

			return _queued;
		}
	}
}