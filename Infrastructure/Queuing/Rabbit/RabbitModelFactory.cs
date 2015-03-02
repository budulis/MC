using RabbitMQ.Client;

namespace Infrastructure.Queuing.Rabbit {
	internal class RabbitModelFactory {
		public static IModel GetModel(IConnection connection, string queueName) {
			var model = connection.CreateModel();

			model.QueueDeclare(queueName, false, false, false, null);
			model.BasicQos(0, 2, false);
			return model;
		}
	}	
}