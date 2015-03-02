using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Queuing;

namespace Infrastructure.Dispatchers
{
	internal class QueuedCommandDispatcher : IDomainCommandDispatcher {

		private readonly ISender<IDomainCommand> _sender;

		public QueuedCommandDispatcher(ILogger logger) {
			_sender = Sender.ForRabbitCommand(logger);
		}

		public async Task Dispatch(IDomainCommand command) {
			await _sender.SendAsync(command);
		}

		public void Dispose() {
			_sender.Dispose();
		}
	}
}