using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using Infrastructure.Queuing;

namespace Infrastructure.Dispatchers {
	internal class QueuedEventNotificationDispather : IDomainEventDispather, IApplicationEventDispather {
		private readonly ISender<IDomainEventNotificationMessage> _sender;
		private readonly ISender<IApplicationEventNotificationMessage> _appEventSender;
		public QueuedEventNotificationDispather(ILogger logger) {
			_sender = Senders.ForRabbitEventNotification(logger);
			_appEventSender = Senders.ForRabbitApplicationEventNotification(logger);
		}

		public async Task Dispatch(IEnumerable<IDomainEventNotificationMessage> evt) {
			foreach (var domainEventNotificationMessage in evt)
				await Dispatch(domainEventNotificationMessage);
		}

		public async Task Dispatch(IDomainEventNotificationMessage evt) {
			await _sender.SendAsync(evt);
		}

		public async Task Dispatch(IApplicationEventNotificationMessage evt) {
			await _appEventSender.SendAsync(evt);
		}

		~QueuedEventNotificationDispather() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				_sender.Dispose();
			}
		}
	}
}