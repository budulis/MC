from amqplib import client_0_8 as amqp


class Consumer(object):
    def __init__(self, host, userid, password):
        """
        Constructor. Initiate a connection to the RabbitMQ server.

        @param host RabbitMQ server host
        @param userid RabbitMQ server username
        @param password RabbitMQ server user's password
        """
        self.connection = amqp.Connection(host=host, userid=userid, password=password, virtual_host="/", insist=False)
        self.channel = self.connection.channel()
        self.exchange_name = ''
        
    def close(self):
        """
        Close channel and connection
        """
        self.channel.close()
        self.connection.close()

    def set_exchange(self, exchange_name):
        self.exchange_name = exchange_name

    def declare_exchange(self, exchange_name, durable=False, auto_delete=False):
        """
        Create exchange.

        @param exchange_name name of the exchange
        @param durable will the server survive a server restart
        @param auto_delete should the server delete the exchange when it is
        no longer in use
        """
        self.channel.exchange_declare(exchange=self.exchange_name, type='direct')

    def declare_queue(self, queue_name, routing_key, durable=False, exclusive=False, auto_delete=False):
        """
        Create a queue and bind it to the exchange.

        @param queue_name Name of the queue to create
        @param routing_key binding key
        @param durable will the queue service a server restart
        @param exclusive only 1 client can work with it
        @param auto_delete should the server delete the exchange when it is
                no longer in use
        """
        self.queue_name = queue_name
        self.routing_key = routing_key
        self.channel.queue_declare(queue=self.queue_name, durable=durable,exclusive=exclusive, auto_delete=auto_delete)
        if self.exchange_name:
            self.channel.queue_bind(queue=self.queue_name, exchange=self.exchange_name, routing_key=self.routing_key)

    def start_consuming(self, callback, queue_name=None, consumer_tag='consumer'):
        """
        Start a consumer and register a function to be called when a message is consumed

        @param callback function to call
        @param queue_name name of the queue
        @param consumer_tag a client-generated consumer tag to establish context
        """
        if hasattr(self, 'queue_name') or queue_name:
            self.channel.basic_consume(queue=getattr(self, 'queue_name', queue_name), callback=callback, consumer_tag=consumer_tag)

    def stop_consuming(self, consumer_tag='consumer'):
        """
        Cancel a consumer.

        @param consumer_tag a client-generated consumer tag to establish context
        """
        self.channel.basic_cancel(consumer_tag)

    def wait(self):
        """
        Wait for activity on the channel.
        """
        while True:
            self.channel.wait()