using System.Threading.Tasks;
using Core.Domain;
using Core.ReadModel;

namespace Core.Subscribers
{
	public abstract class Subscriber<TDomainEvent,TReadModel> : ISubscriber<TDomainEvent> where TReadModel : IReadModel
	{
		protected IDomainCommandDispatcher DomainCommandDispatcher { get; set; }
		protected IReadModelRepository<TReadModel> ReadModelRepository { get; set; }

		protected Subscriber(IDomainCommandDispatcher domainCommandDispatcher, IReadModelRepository<TReadModel> readModelRepository) {
			DomainCommandDispatcher = domainCommandDispatcher;
			ReadModelRepository = readModelRepository;
		}

		public abstract Task Notify(TDomainEvent evt);
	}
}