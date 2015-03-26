import random
from Producer import Producer


p = Producer(exchange_name='direct', host='localhost', userid='guest', password='guest')
counter = 0
while counter < 20:
    counter += 1
    # generate a random integer between 1 and 100 included
    i = random.randint(1, 100)
    if i % 2 == 0:
        key = 'even'
    else:
        key = 'odd'
    p.publish(str(i), '')
    print '>: %d' % i
    '''time.sleep(1)'''