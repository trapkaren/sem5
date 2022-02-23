from time import process_time
from random import randint


def BubbleSort(a, n):
    i = 0
    while i < n:
        j = 0
        while j < n - i - 1:
            if a[j] > a[j + 1]:
                temp = a[j]
                a[j] = a[j + 1]
                a[j + 1] = temp
            j += 1
        i += 1


def InsertSort(a, n):
    i = 1
    while i < n:
        key = a[i]
        j = i - 1
        while j >= 0 and key < a[j]:
            a[j + 1] = a[j]
            j -= 1
        a[j + 1] = key
        i += 1

def SelectionSort(a, n):
    i = n
    while i > 1:
        maximum = 0
        j = 0
        while j < i:
            if a[j] > a[maximum]:
                maximum = j
            j += 1
        temp = a[i - 1]
        a[i - 1] = a[maximum]
        a[maximum] = temp
        i -= 1


def time_analyse():
    size = (int(input("Введите размер: ")))
    iteration = (int(input("Введите кол-во итерации: ")))
    timeBubble = 0
    timeInsert = 0
    timeSelect = 0
    for i in range(iteration):
        arr = [randint(-100, 100) for i in range(size)]

        a1 = arr.copy()
        a2 = arr.copy()
        a3 = arr.copy()

        start_time = process_time()
        BubbleSort(a1, size)
        end_time = process_time()
        timeBubble += end_time - start_time

        start_time = process_time()
        InsertSort(a2, size)
        end_time = process_time()
        timeInsert += end_time - start_time

        start_time = process_time()
        SelectionSort(a3, size)
        end_time = process_time()
        timeSelect += end_time - start_time
    return timeBubble / iteration, timeInsert/iteration, timeSelect/iteration




if __name__ == "__main__":
    n = int(input("Введите размер массива n: "))
    arr = [randint(-100, 100) for i in range(n)]
    print(arr)
    arr_copy = arr.copy()

    print("\nРезультат сортировки пузырьком c флагом: ")
    BubbleSort(arr_copy, n)
    print(arr_copy)

    arr_copy = arr.copy()

    print("\nРезультат сортировки вставками: ")
    InsertSort(arr_copy, n)
    print(arr_copy)

    arr_copy = arr.copy()

    print("\nРезультат сортировки выбором: ")
    SelectionSort(arr_copy, n)
    print(arr_copy)

    print("Анализ времени")
    timeBubble, timeInsert, timeSelect = time_analyse()
    print("Время работы сортировки пузырьком: ", "{0:.8f}".format(timeBubble), 'сек')
    print("Время работы сортировки вставками: ", "{0:.8f}".format(timeInsert), 'сек')
    print("Время работы сортировки выбором: ", "{0:.8f}".format(timeSelect), 'сек')
