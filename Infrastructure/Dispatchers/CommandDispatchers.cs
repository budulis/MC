using Core;
using Infrastructure.Services.Discount;
using Infrastructure.Services.Payment;
using Infrastructure.Services.Staff;

namespace Infrastructure.Dispatchers
{
	public class CommandDispatchers
	{
		private static readonly object SyncRoot;
		private static IDomainCommandDispatcher _queued;

		static CommandDispatchers() {
			SyncRoot = new object();
		}

		public static IDomainCommandDispatcher GetDirect(IDomainEventDispather domainEventDispather, ILogger logger) {

			IEventStore eventStore = EventStore.EventStores.InMemory;
			IDiscountService discountService = new DiscountService();
			ICardPaymentService paymentService = new CardPaymentService();
			ICashierRepository cashiers = new CashierRepository();
			IChefRepository chefs = new ChefRepository();
			IInfrastructureService infrastructureService = new InfrastructureService(eventStore, discountService, paymentService, logger, cashiers, chefs);

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