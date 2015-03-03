using System;
using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using RabbitMQ.Client;

namespace Infrastructure.Queuing.Rabbit {
	internal class Sender : 
		ISender<IDomainCommand>, 
		ISender<IDomainEventNotificationMessage>,
		ISender<IApplicationEventNotificationMessage> {

		private readonly IConnection _connection;
		private readonly Func<IDomainCommand, byte[]> _commandSerializer;
		private readonly Func<IDomainEventNotificationMessage, byte[]> _eventSerializer;
		private readonly Func<IApplicationEventNotificationMessage, byte[]> _appEventSerializer;
		private ILogger _logger;

		public Sender(Func<IDomainCommand, byte[]> commandSerializer,ILogger logger)
			: this(logger) {

			if (commandSerializer == null)
				throw new ArgumentNullException("commandSerializer");

			_commandSerializer = commandSerializer;
			}

		public Sender(Func<IDomainEventNotificationMessage, byte[]> eventSerializer, ILogger logger)
			: this(logger) {
			if (eventSerializer == null)
				throw new ArgumentNullException("eventSerializer");

			_eventSerializer = eventSerializer;
		}

		public Sender(Func<IApplicationEventNotificationMessage, byte[]> appEventSerializer, ILogger logger)
			: this(logger) {
				if (appEventSerializer == null)
					throw new ArgumentNullException("appEventSerializer");

			_appEventSerializer = appEventSerializer;
		}

		private Sender(ILogger logger) {
			var connectionFactory = new ConnectionFactory { HostName = "localhost" };
			_connection = connectionFactory.CreateConnection();
			_logger = logger;
		}

		public Task SendAsync(IDomainCommand message) {
			using (var channel = RabbitModelFactory.GetModel(_connection, Params.Queueing.QueueName.ForDomainCommand)) {
				channel.BasicPublish("", Params.Queueing.QueueName.ForDomainCommand, channel.CreateBasicProperties(), _commandSerializer(message));
				return Task.FromResult(true);
			}
		}

		public Task SendAsync(IDomainEventNotificationMessage message) {
			using (var channel = RabbitModelFactory.GetModel(_connection, Params.Queueing.QueueName.ForDomainEvent)) {
				channel.BasicPublish("", Params.Queueing.QueueName.ForDomainEvent, channel.CreateBasicProperties(), _eventSerializer(message));
				return Task.FromResult(true);
			}
		}

		public Task SendAsync(IApplicationEventNotificationMessage message) {
			using (var channel = RabbitModelFactory.GetModel(_connection, Params.Queueing.QueueName.ForApplicationEvent)) {
				channel.BasicPublish("", Params.Queueing.QueueName.ForApplicationEvent, channel.CreateBasicProperties(), _appEventSerializer(message));
				return Task.FromResult(true);
			}
		}

		#region Dispose
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				_connection.Dispose();
			}
		}

		~Sender() {
			Dispose(false);
		} 
		#endregion
	}
}
