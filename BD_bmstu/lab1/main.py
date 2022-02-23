import csv
import random
from faker import Faker
import datetime


records = 1000
clients = []
tarologists = []
sex = ['М', 'Ж', 'Иное']
cards = []
predictions = []
ts = []
level_knowledges = []

def fill_tarologists():
    fake = Faker('ru_RU')
    for i in range(records):
        str_tarologists = []
        str_tarologists.append(str(i + 1))
        str_tarologists.append(fake.first_name())
        str_tarologists.append(fake.address())
        str_tarologists.append(fake.phone_number())
        str_tarologists.append(random.randint(1, 5))

        tarologists.append(str_tarologists)

    file = open('tarologists.csv', 'w')
    with file:
        writer = csv.writer(file)
        writer.writerows(tarologists)

def fill_clients():
    fake = Faker('ru_RU')
    for i in range(records):
        str_clients = []
        str_clients.append(str(i + 1))
        str_clients.append(fake.first_name())

        start_date = datetime.date(1960, 10, 1)
        end_date = datetime.date(2004, 10, 1)

        time_between_dates = end_date - start_date
        days_between_dates = time_between_dates.days
        random_number_of_days = random.randrange(days_between_dates)
        random_date = start_date + datetime.timedelta(days=random_number_of_days)

        str_clients.append(random_date)

        #str_clients.append(fake.date())
        str_clients.append(random.choice(sex))
        str_clients.append(random.randint(0, 20000))

        str_clients.append(random.randint(1, 1000))

        clients.append(str_clients)

    file = open('/Users/trapkaren/Desktop/BD_bmstu/lab1/clients.csv', 'w')
    with file:
        writer = csv.writer(file)
        writer.writerows(clients)

def fill_cards():
    fake = Faker('ru_RU')

    i = 1

    file = open('tarot.csv', 'r')
    with file:
        reader = csv.reader(file)
        for row in reader:
            row.insert(0, str(i))
            i+= 1

            cards.append(row)

    print(cards)

    file = open('cards.csv', 'w')
    with file:
        writer = csv.writer(file)
        writer.writerows(cards)

def fill_predictions():
    for i in range(records):
        str_predictions = []
        str_predictions.append(random.randint(1,78))
        str_predictions.append(random.randint(1,1000))

        predictions.append(str_predictions)

    file = open('predictions.csv', 'w')
    with file:
        writer = csv.writer(file)
        writer.writerows(predictions)

    print(predictions)

def fill_ts():
    for i in range(records + 1000):
        str_ts = []
        str_ts.append(random.randint(1, 1000))
        str_ts.append(random.randint(1,15))

        ts.append(str_ts)

    file = open('ts.csv', 'w')
    with file:
        writer = csv.writer(file)
        writer.writerows(ts)

def fill_level_knowledges():
    for i in range(records):
        str_level = []
        str_level.append(random.randint(1,5))
        str_level.append(random.randint(1,15))

        level_knowledges.append(str_level)

    file = open('level_knowledges.csv', 'w')
    with file:
        writer = csv.writer(file)
        writer.writerows(level_knowledges)


if __name__ == '__main__':
   # fill_tarologists()
   # fill_clients()
   # fill_cards()
   # fill_predictions()
   # fill_ts()
   # fill_level_knowledges()

