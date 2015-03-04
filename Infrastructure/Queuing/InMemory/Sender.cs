using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;

namespace Infrastructure.Queuing.InMemory {
	internal class Sender : 
		ISender<IDomainCommand>, 
		ISender<IDomainEventNotificationMessage>,
	ISender<IApplicationEventNotificationMessage> {
		private readonly ILogger _logger;

		public Sender(ILogger logger)
		{
			_logger = logger;
		}

		public Task SendAsync(IDomainCommand message)
		{
			Broker.Instance.Accept(message);
			return Task.FromResult(true);
		}

		public Task SendAsync(IDomainEventNotificationMessage message)
		{
			Broker.Instance.Accept(message);
			return Task.FromResult(true);
		}

		public Task SendAsync(IApplicationEventNotificationMessage message)
		{
			Broker.Instance.Accept(message);
			return Task.FromResult(true);
		}

		public void Dispose()
		{
		}
	}
}
