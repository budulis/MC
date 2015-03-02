using System;
using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using Infrastructure.Queuing;

namespace Infrastructure.Dispatchers {
	internal class QueuedEventDispather : EventDispather {
		private readonly ISender<IDomainEventNotificationMessage> _sender;
		public QueuedEventDispather(ILogger logger) {
			_sender = Sender.ForRabbitEventNotification(logger);
		}

		public override async Task Dispatch(IDomainEventNotificationMessage evt) {
			await _sender.SendAsync(evt);
		}

		public override Task Dispatch(IApplicationEventNotificationMessage evt) {
			throw new NotImplementedException();
		}

		protected override void Dispose(bool disposing) {
			if (disposing)
				if (_sender != null)
					_sender.Dispose();
				
		}
	}
}