import numpy as np
import random
import csv
from faker import Faker

def generate_data(n, filename):
    fake = Faker()
    with open(filename, 'w') as csvfile:
        fieldnames = ['email', 'id']
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
        writer.writeheader()
        for i in range(n):
            email = fake.email()
            id = random.randint(100000, 900000)
            writer.writerow({'email': email, 'id': id})

def read_csv(filename):
    emails = dict()
    with open(filename) as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            emails[row['email']] = row['id']
    return emails

def sort(emails, reverse):
    tmp = dict()
    list_d = list(emails.items())
    if reverse == False:
        list_d.sort(key = lambda i: i[1], reverse = False)
    else:
        list_d.sort(key = lambda i: i[1], reverse = True)
    for i in list_d:
        tmp[i[0]] = i[1]
    return tmp

def full_iteration(emails, key):
    for dict_key in emails.keys():
        if key == dict_key:
            return emails[key]
    return None

def damerau_levenshtein_distance(a: str, b: str) -> int:
    matr = np.eye(len(a) + 1, len(b) + 1)

    for i in range(len(a) + 1):
        matr[i][0] = i # (1)
    for j in range(len(b) + 1):
        matr[0][j] = j # (2)

    for i in range(len(a)):
        for j in range(len(b)):
            d1 = matr[i + 1][j] + 1 # (3)
            d2 = matr[i][j + 1] + 1 #(4)
            if a[i] == b[j]:
                d3 = matr[i][j] # (5)
            else:
                d3 = matr[i][j] + 1 #(6)
            if a[i] == b[j - 1] and a[i - 1] == b[j] and i > 0 and j > 0:
                d4 = matr[i - 1][j - 1] + 1 # (7)
            else:
                d4 = d1 # (8)
            matr[i + 1][j + 1] = min(d1, d2, d3, d4) # (9)
    return matr[-1][-1]

def levenshtein_distance(a: str, b: str) -> int:
    m, n = len(a), len(b)
    d = np.zeros((m+1, n+1), dtype=int)

    for i in range(m+1): d[i, 0] = i
    for i in range(n+1): d[0, i] = i

    for j in range(1, n+1):
        for i in range(1, m+1):
            cost = 1 if a[i-1] != b[j-1] else 0
            d[i, j] = min(d[i-1, j-1] + cost,
                          d[i, j - 1] + 1,
                          d[i - 1, j] + 1)
    return d[m, n]

# куда девается середина

def binary(sorted_dict, key):
    sorted_keys = list(sorted_dict.keys())

    i = 0
    j = len(sorted_dict) - 1
    m = int(j / 2)

    while sorted_keys[m] != key and i < j:
        if key > sorted_keys[m]:
            i = m + 1
        else:
            j = m - 1
        m = int((i + j) / 2)

    if i > j:
        return None
    else:
        return sorted_dict[sorted_keys[m]]

def get_sorted_dict(dict):
    sorted_list = sorted(dict.items())
    sorted_dict = {}
    for i in range(len(sorted_list)):
        sorted_dict[sorted_list[i][0]] = sorted_list[i][1]
    return sorted_dict

def possible_replacements_words(key, errors, keys):
    possible = []
    for i in range (len(keys)):
        tmp = damerau_levenshtein_distance(key, keys[i])
        if int(tmp) == 0:
            print("Вы не сделали ошибку!")
            possible.clear()
            possible.append(key)
            return possible
        if int(tmp) <= errors:
            possible.append(keys[i])
    return possible

if __name__ == "__main__":
    emails = read_csv('1000.csv')
    print(emails)
    print("\nРазмер словаря: ", len(emails))

    key = input("Введите email для поиска номера: ")

    errors = int(input("Введите количество ошибок, которые вы могли допустить: "))

    print("--------------------------------------------------------")

    sorted_dict = get_sorted_dict(emails)
    sorted_keys = list(sorted_dict.keys())

    possible = possible_replacements_words(key, errors, sorted_keys)

    print("Возможные email'ы для замены: ", possible)

    for i in range(len(possible)):
        print("---------------------------", i + 1, "---------------------------")
        print("Для: ", possible[i])
        print("Результат после полного перебора: ", full_iteration(emails, possible[i]))
        print("Результат после бинарного поиска: ", binary(sorted_dict, possible[i]))





