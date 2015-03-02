using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering.Exceptions;
using Core.Handlers.Exceptions;
using Infrastructure.Serialization;
using RabbitMQ.Client;

namespace Infrastructure.Queuing.Rabbit {
	internal class Reciever : IReciever<IDomainCommand>, IReciever<IDomainEventNotificationMessage> {
		private Func<byte[], IDomainCommand> _commandDeserializer;
		private readonly ILogger _logger;
		private Func<byte[], IDomainEventNotificationMessage> _eventDeserializer;
		private readonly IConnection _connection;

		public Reciever(Func<byte[], IDomainCommand> deserializer, ILogger logger)
			: this() {

			if (deserializer == null)
				throw new ArgumentNullException("deserializer");

			_commandDeserializer = deserializer;
			_logger = logger;
		}

		public Reciever(Func<byte[], IDomainEventNotificationMessage> deserializer, ILogger logger)
			: this() {

			if (deserializer == null)
				throw new ArgumentNullException("deserializer");

			_eventDeserializer = deserializer;
			_logger = logger;
		}

		private Reciever() {
			var connectionFactory = new ConnectionFactory { HostName = "localhost" };
			_connection = connectionFactory.CreateConnection();
		}

		public void Recieve(Action<Task<IDomainCommand>> onMessageRecieved, CancellationToken cancellationToken) {
			using (var channel = RabbitModelFactory.GetModel(_connection, Params.Queueing.QueueName.ForDomainCommand)) {

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
			using (var channel = RabbitModelFactory.GetModel(_connection, Params.Queueing.QueueName.ForDomainEvent)) {

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
