import numpy as np
import string
import random

import sys

from time import process_time

symbols = string.ascii_letters + string.digits
debug = False


def random_string(length):
    return ''.join(random.choice(symbols) for _ in range(length))


def time_analyze(function, iterations, length=5):
    t1 = process_time()
    for _ in range(iterations):
        s1 = random_string(length)
        s2 = random_string(length)
        function(s1, s2, False)
    t2 = process_time()
    return (t2 - t1) / iterations


def input_s1_s2():
    s1 = input('  Введите первое слово: ')
    s2 = input('  Введите второе слово: ')
    return s1, s2


def start_func(function):
    s1, s2 = input_s1_s2()
    value = function(s1, s2, True)
    if value is not None:
        print('\nВозврат: ', value)


def operations(s1, s2, matr):
    def pathfinder(m, i, j):
        if i > 0 and j > 0 and m[i - 1][j - 1] < m[i][j]:
            pathfinder(m, i - 1, j - 1)
            print('R', end=' ')
        elif i > 0 and m[i - 1][j] < m[i][j]:
            pathfinder(m, i - 1, j)
            print('D', end=' ')
        elif j > 0 and m[i][j - 1] < m[i][j]:
            pathfinder(m, i, j - 1)
            print('I', end=' ')
        elif i > 0 and j > 0 and m[i - 1][j - 1] == m[i][j]:
            pathfinder(m, i - 1, j - 1)
            print('M', end=' ')

    print('\nОперация:')
    pathfinder(matr, len(s1), len(s2))


def output_matrix(s1, s2, matr):
    print("   ", end=" ")
    for i in s2:
        print(i, end=" ")

    for i in range(len(matr)):
        if i:
            print("\n" + s1[i - 1], end=" ")
        else:
            print("\n ", end=" ")
        for j in range(len(matr[i])):
            print(int(matr[i][j]), end=" ")


def int_inputer(str, value):
    tmp = input(str)
    if tmp.isnumeric():
        tmp = int(tmp)
    else:
        tmp = value
    return tmp


# Функции подсчета расстояния Левеншейна
# Levenshtein distance
def calc_dist_matrix(s1, s2, printable=False):
    matr = np.eye(len(s1) + 1, len(s2) + 1)

    for i in range(len(s1) + 1):
        matr[i][0] = i
    for j in range(len(s2) + 1):
        matr[0][j] = j

    for i in range(len(s1)):
        for j in range(len(s2)):
            d1 = matr[i + 1][j] + 1
            d2 = matr[i][j + 1] + 1
            if s1[i] == s2[j]:
                d3 = matr[i][j]
            else:
                d3 = matr[i][j] + 1
            matr[i + 1][j + 1] = min(d1, d2, d3)

    if printable:
        print('Матрица:')
        output_matrix(s1, s2, matr)
        operations(s1, s2, matr)
    return matr[-1][-1]


def calc_dist_recur(s1, s2, printable=False):
    if debug or printable:
        print(s1, s2)

    if s1 == '' or s2 == '':
        return abs(len(s1) - len(s2))

    tmp = 0 if (s1[-1] == s2[-1]) else 1
    return min(calc_dist_recur(s1[:-1], s2) + 1,
               calc_dist_recur(s1, s2[:-1]) + 1,
               calc_dist_recur(s1[:-1], s2[:-1]) + tmp)


def calc_dist_recur_matrix(s1, s2, printable=False):
    def calc_value(matr, i, j):
        if matr[i][j] != -1:
            return matr[i][j]
        else:
            tmp = 0 if (s1[i - 1] == s2[j - 1]) else 1
            matr[i][j] = min(calc_value(matr, i - 1, j) + 1,
                             calc_value(matr, i, j - 1) + 1,
                             calc_value(matr, i - 1, j - 1) + tmp)
            return matr[i][j]

    matr = np.full((len(s1) + 1, len(s2) + 1), -1)
    for i in range(len(s1) + 1):
        matr[i][0] = i
    for j in range(len(s2) + 1):
        matr[0][j] = j
    value = calc_value(matr, len(s1), len(s2))
    if printable:
        print('Матрица \n')
        output_matrix(s1, s2, matr)
        operations(s1, s2, matr)
    return value


def calc_dist_damerau(s1, s2, printable=False):
    matr = np.eye(len(s1) + 1, len(s2) + 1)

    for i in range(len(s1) + 1):
        matr[i][0] = i # (1)
    for j in range(len(s2) + 1):
        matr[0][j] = j # (2)

    for i in range(len(s1)):
        for j in range(len(s2)):
            d1 = matr[i + 1][j] + 1 # (3)
            d2 = matr[i][j + 1] + 1 #(4)
            if s1[i] == s2[j]:
                d3 = matr[i][j] # (5)
            else:
                d3 = matr[i][j] + 1 #(6)
            if s1[i] == s2[j - 1] and s1[i - 1] == s2[j] and i > 0 and j > 0:
                d4 = matr[i - 1][j - 1] + 1 # (7)
            else:
                d4 = d1 # (8)
            matr[i + 1][j + 1] = min(d1, d2, d3, d4) # (9)

    if printable:
        print('\nМатрица: ')
        output_matrix(s1, s2, matr)
        operations(s1, s2, matr)
    return matr[-1][-1]


def calc_dist_damerau_recur(s1, s2, printable=False):
    if debug or printable:
        print(s1, s2)

    if s1 == '' or s2 == '':
        return abs(len(s1) - len(s2))

    tmp = 0 if (s1[-1] == s2[-1]) else 1
    if s1[:-1] == s2 and s1 == s2[:-1] and len(s1) > 0 and len(s2) > 0:
        return min(calc_dist_damerau_recur(s1[:-1], s2) + 1,
                   calc_dist_damerau_recur(s1, s2[:-1]) + 1,
                   calc_dist_damerau_recur(s1[:-1], s2[:-1]) + tmp,
                   calc_dist_damerau_recur(s1[:-2], s2[:-2]) + 1)
    else:
        return min(calc_dist_damerau_recur(s1[:-1], s2) + 1,
                   calc_dist_damerau_recur(s1, s2[:-1]) + 1,
                   calc_dist_damerau_recur(s1[:-1], s2[:-1]) + tmp)


calc_func = [calc_dist_matrix, calc_dist_recur, calc_dist_recur_matrix, calc_dist_damerau, calc_dist_damerau_recur]

if __name__ == '__main__':
    while True:
        case = input('\n\nМеню: \n \
1) Матричное нахождение расстояния Левенштейна\n \
2) Рекурсивное нахождение расстояния Левенштейна\n \
3) Рекурсивное нахождение расстояния Левенштейна с заполнением матрицы\n \
4) Матричное нахождение расстояния Дамерау-Левенштейна\n \
5) Рекурсивное нахождение расстояния Дамерау-Левенштейна\n \
5) Анализ времени\n \
7) Анализ времени 1 из методов\n \
Ввод: ')

        if case == '1':
            start_func(calc_dist_matrix)
        elif case == '2':
            start_func(calc_dist_recur)
        elif case == '3':
            start_func(calc_dist_recur_matrix)
        elif case == '4':
            start_func(calc_dist_damerau)
        elif case == '5':
            start_func(calc_dist_damerau_recur)
        elif case == '6':

            iteration = int_inputer('Введите кол-во итераций: ', 100)
            i1 = int_inputer('Введите нижнюю границу: ', 1)
            i2 = int_inputer('Введите верхнюю границу: ', 10)

            for i in range(i1, i2 + 1):
                print("Длина строки: ", i)
                print("   Матричное нахождение расстояния Левенштейна            : ", "{0:.8f}".format(time_analyze(calc_dist_matrix,
                                                                                          iteration, i)))
                print("   Рекурсивное нахождение расстояния Левенштейна          : ", "{0:.8f}".format(time_analyze(calc_dist_recur,
                                                                                          iteration, i)))
                print("   Рекурсивно-матричное нахождение расстояния Левенштейна : ", "{0:.8f}".format(time_analyze(calc_dist_recur_matrix,
                                                                                          iteration, i)))
                print("   Матричное нахождение расстояния Дамерау-Левенштейна    : ", "{0:.8f}".format(time_analyze(calc_dist_damerau,
                                                                                          iteration, i)))
                print("   Рекурсивное нахождение расстояния Дамерау-Левенштейна    : ", "{0:.8f}".format(time_analyze(calc_dist_damerau_recur,
                                                    iteration, i)))
        elif case == '7':
            func = int_inputer('  Введите номер метода: ', 1)
            iteration = int_inputer('  Введите кол-во итераций: ', 100)
            i = int_inputer('  Введите длину слова: ', 1)
            print("🕐 Время: ", "{0:.8f}".format(time_analyze(calc_func[func - 1], iteration, i)))