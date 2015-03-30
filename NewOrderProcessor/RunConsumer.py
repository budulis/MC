from Consumer import Consumer
import json
from Db import Db
from Configuration import QueueSettings

def main():
    c = Consumer(QueueSettings.HOST, QueueSettings.USER_ID, QueueSettings.PASSWORD)
    c.declare_queue(queue_name=QueueSettings.QUEUE_NAME, routing_key = QueueSettings.ROUTING_KEY)

    def get_product_info(lst):
         data = ''
         for l in lst:
             data+= str(l["Id"]) + ' - ' + str(l["Name"]) + ';'
         return data
    
    def parse_json_message(msg):
        id = msg["Id"]["Value"]
        amount = msg["Amount"]
        loyalty_card = msg["LoyaltyCard"]
        amount_charged = msg["AmountCharged"]
        discount = msg["Discount"]
        products = get_product_info(msg["Products"])
        payment_type = msg["Payment"]

        return (id,loyalty_card,amount_charged,discount,products,amount,payment_type)
    

    def on_message(message):
        msg = parse_json_message(json.loads(message.body))
        Db().insert_reciept_info(msg)
        print msg[0] + ' inserted'
        c.channel.basic_ack(message.delivery_tag)


    c.start_consuming(on_message)
    print 'Consumer started...'
    c.wait()

if __name__ == "__main__":
    main()