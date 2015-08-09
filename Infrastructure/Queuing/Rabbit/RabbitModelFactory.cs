using RabbitMQ.Client;

namespace Infrastructure.Queuing.Rabbit {
	internal class RabbitModelFactory {

		public class Direct
		{
			public static IModel GetModel(IConnection connection, string queueName) {
				var model = connection.CreateModel();

				model.QueueDeclare(queueName, false, false, false, null);
				model.BasicQos(0, 2, false);
				return model;
			}
		}

		public class FanOut
		{
			public static IModel GetModel(IConnection connection, string exchangeName, string queueName) {
				var model = connection.CreateModel();
				model.ExchangeDeclare(exchangeName,"fanout");
				model.QueueDeclare(queueName, false, false, false, null);
				model.QueueBind(queueName, exchangeName, "");

				model.BasicQos(0, 2, false);
				return model;
			}			
		}
	}	
}