using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Application.Messages;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Exceptions;
using Core.Handlers.Exceptions;
using Infrastructure.Serialization;
using RabbitMQ.Client;

namespace Infrastructure.Queuing.Rabbit {
	internal class Reciever :
		IReciever<IDomainCommand>,
		IReciever<IDomainEventNotificationMessage>,
		IReciever<IApplicationEventNotificationMessage> {

		private Func<byte[], IDomainCommand> _commandDeserializer;
		private Func<byte[], IDomainEventNotificationMessage> _eventDeserializer;
		private Func<byte[], IApplicationEventNotificationMessage> _appEventDeserializer;
		private readonly IConnection _connection;
		private readonly ILogger _logger;

		public Reciever(Func<byte[], IDomainCommand> deserializer, ILogger logger)
			: this(logger) {

			if (deserializer == null)
				throw new ArgumentNullException("deserializer");

			_commandDeserializer = deserializer;
		}

		public Reciever(Func<byte[], IDomainEventNotificationMessage> deserializer, ILogger logger)
			: this(logger) {

			if (deserializer == null)
				throw new ArgumentNullException("deserializer");

			_eventDeserializer = deserializer;
		}

		public Reciever(Func<byte[], IApplicationEventNotificationMessage> deserializer, ILogger logger)
			: this(logger) {

			if (deserializer == null)
				throw new ArgumentNullException("deserializer");

			_appEventDeserializer = deserializer;
		}

		private Reciever(ILogger logger)
		{
			_logger = logger;
			var connectionFactory = new ConnectionFactory { HostName = "localhost" };
			_connection = connectionFactory.CreateConnection();
		}

		public void Recieve(Action<Task<IDomainCommand>> onMessageRecieved, CancellationToken cancellationToken) {
			using (var channel = RabbitModelFactory.Direct.GetModel(_connection, Params.Queueing.QueueName.ForDomainCommand)) {

				var consumer = new QueueingBasicConsumer(channel);
				channel.BasicConsume(Params.Queueing.QueueName.ForDomainCommand, false, consumer);

				_logger.Audit("Start recieving");

				while (!cancellationToken.IsCancellationRequested) {
					var message = consumer.Queue.Dequeue();
					var body = message.Body;
					var command = _commandDeserializer(body);

					try {
						onMessageRecieved(Task.FromResult(command));
						channel.BasicAck(message.DeliveryTag, false);
						_logger.Audit(command);
					}
					catch (CommandExecutionException ex) {
						_logger.Error("[" + command.GetType().Name + ";" + command.Id + "]" + ex);
						//TODO:Send to dead-letter queue
					}
					catch (Exception ex) {
						_logger.Error(ex);
					}
				}

				_logger.Audit("Stop recieving");
			}
		}

		public void Recieve(Action<Task<IDomainEventNotificationMessage>> onMessageRecieved, CancellationToken cancellationToken) {
			using (var channel = RabbitModelFactory.Direct.GetModel(_connection, Params.Queueing.QueueName.ForDomainEvent)) {

				var consumer = new QueueingBasicConsumer(channel);
				channel.BasicConsume(Params.Queueing.QueueName.ForDomainEvent, false, consumer);

				_logger.Audit("Start recieving");

				while (!cancellationToken.IsCancellationRequested) {
					var message = consumer.Queue.Dequeue();
					var body = message.Body;
					var evt = _eventDeserializer(body);

					try {
						onMessageRecieved(Task.FromResult(evt));
						channel.BasicAck(message.DeliveryTag, false);
						_logger.Audit(evt);
					}
					catch (Exception ex) {
						//TODO: Send message to dead-letter queue
						_logger.Error(ex);
					}
				}
			}
		}
		
		public void Recieve(Action<Task<IApplicationEventNotificationMessage>> onMessageRecieved, CancellationToken cancellationToken) {
			using (var channel = RabbitModelFactory.Direct.GetModel(_connection, Params.Queueing.QueueName.ForApplicationEvent)) {

				var consumer = new QueueingBasicConsumer(channel);
				channel.BasicConsume(Params.Queueing.QueueName.ForDomainEvent, false, consumer);

				_logger.Audit("Start recieving");

				while (!cancellationToken.IsCancellationRequested) {
					var message = consumer.Queue.Dequeue();
					var body = message.Body;
					var evt = _appEventDeserializer(body);

					try {
						onMessageRecieved(Task.FromResult(evt));
						channel.BasicAck(message.DeliveryTag, false);
						_logger.Audit(evt);
					}
					catch (Exception ex) {
						//TODO: Send message to dead-letter queue
						_logger.Error(ex);
					}
				}
			}
		}

		#region IDisposable

		~Reciever() {
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				_commandDeserializer = null;
				_eventDeserializer = null;
				if (_connection != null)
					_connection.Dispose();
			}
		}
		#endregion
	}
}
