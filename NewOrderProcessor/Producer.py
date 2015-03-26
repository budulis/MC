from amqplib import client_0_8 as amqp


class Producer(object):
    def __init__(self, exchange_name, host, userid, password):
        """
        Constructor. Initiate connection with the RabbitMQ server.

        @param exchange_name name of the exchange to send messages to
        @param host RabbitMQ server host
        @param userid RabbitMQ server username
        @param password RabbitMQ server user's password
        """
        self.exchange_name = exchange_name
        self.connection = amqp.Connection(host=host, userid=userid, password=password, virtual_host="/", insist=False)
        self.channel = self.connection.channel()

    def publish(self, message, routing_key):
        """
        Publish message to exchange using routing key

        @param text message to publish
        @param routing_key message routing key
        """
        msg = amqp.Message(message)
        msg.properties["content_type"] = "text/plain"
        msg.properties["delivery_mode"] = 2
        self.channel.basic_publish(exchange=self.exchange_name, routing_key=routing_key, msg=msg)

    def close(self):
        """
        Close channel and connection
        """
        self.channel.close()
        self.connection.close()

