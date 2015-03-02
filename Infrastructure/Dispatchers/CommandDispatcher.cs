using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Services.Discount;
using Infrastructure.Services.Logging;
using Infrastructure.Services.Payment;
using Infrastructure.Services.Staff;

namespace Infrastructure.Dispatchers {
	public abstract class CommandDispatcher : IDomainCommandDispatcher {
		
		public abstract Task Dispatch(IDomainCommand command);

		private static readonly object SyncRoot;

		static CommandDispatcher() {
			SyncRoot = new object();
		}

		private static IDomainCommandDispatcher _queued;

		public static IDomainCommandDispatcher GetDirect(IDomainEventDispather domainEventDispather,ILogger logger) {

			IEventStore eventStore = EventStore.EventStore.InMemory;
			IDiscountService discountService = new DiscountService();
			ICardPaymentService paymentService = new CardPaymentService();
			ICashierRepository cashiers = new CashierRepository();
			IChefRepository chefs = new ChefRepository();
			IInfrastructureService infrastructureService = new InfrastructureService(eventStore, discountService, paymentService, logger, cashiers, chefs);

			return new DirectCommandDispatcher(infrastructureService,domainEventDispather);
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