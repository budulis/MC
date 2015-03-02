using System;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Infrastructure.Serialization;
using RabbitMQ.Client;

namespace Infrastructure.Queuing.Rabbit {
	internal class Sender : ISender<IDomainCommand>, ISender<IDomainEventNotificationMessage> {
		private readonly Func<IDomainCommand, byte[]> _commandSerializer;
		private readonly ILogger _logger;
		private readonly Func<IDomainEventNotificationMessage, byte[]> _eventSerializer;
		private readonly IConnection _connection;

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
