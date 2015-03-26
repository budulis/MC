from Consumer import Consumer
import json

c = Consumer(host='localhost', userid='guest',password='guest')
c.declare_queue(queue_name='Application_Events', routing_key='')
print 'Exchange declared'

def message_callback(message):
    j = json.loads(message.body)
    print '%s' % j["Id"]["Value"]
    c.channel.basic_ack(message.delivery_tag)


c.start_consuming(message_callback)
c.wait()

while 1:
    row = cursor.fetchone()
    if not row:
        break
    print row.OrderId
cnxn.close()