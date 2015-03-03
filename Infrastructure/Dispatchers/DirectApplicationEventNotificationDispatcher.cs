using Core;
using Core.Application.Messages;
using Core.ReadModel;
using Infrastructure.Services.Reporting;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Dispatchers
{
	internal class DirectApplicationEventNotificationDispatcher : IApplicationEventDispather {
		private readonly ILogger _logger;
		private readonly IReadModelRepository<ReceiptReadModel> _repository;

		public DirectApplicationEventNotificationDispatcher(ILogger logger, IReadModelRepository<ReceiptReadModel> repository)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task Dispatch(IApplicationEventNotificationMessage evt) {
			try {
				_logger.Audit(evt.GetType().Name.Replace("NotificationMessage", ""));
				await DispatchNotification((dynamic)evt);
			}
			catch (RuntimeBinderException ex) {
				_logger.Error(ex);
			}
		}

		private Task DispatchNotification(OrderStartedApplicationNotificationMessage m) {
			return new OnOrderStartedCreateReceipt(_repository).Notify(m);
		}

		#region IDisposable
		~DirectApplicationEventNotificationDispatcher() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				//means this one is called from within the managed code 
				//and it is safe to free any used managed resources
			}
			//below is the place to free any unmanaged resources
		}
		#endregion
	}
}