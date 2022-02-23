from main import *


def compare_arr(arr1, arr2):
    if len(arr1) != len(arr2):
        return False
    for i in range(len(arr1)):
        if arr1[i] != arr2[i]:
            return False
    return True



def full_test():

    arr1 = [0, 0, 0, 0]
    arr2 = [1, 2, 3, 4, 5]
    arr3 = [5, 4, 3, 2, 1]
    arr4 = [ 4, 2, 3, 5, 1]
    arr5 = [2, 3, 2, 3, 2, 3]

    arr1ok = [0, 0, 0, 0]
    arr2ok = [1, 2, 3, 4, 5]
    arr3ok = [2, 2, 2, 3, 3, 3]

    arr_copy = arr1.copy()
    BubbleSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr1ok):
        print("bubble first test: ok")
    else:
        print("bubble first test: error")
    arr_copy = arr1.copy()
    SelectionSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr1ok):
        print("select first test: ok")
    else:
        print("select first test: error")
    arr_copy = arr1.copy()
    InsertSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr1ok):
        print("insert first test: ok")
    else:
        print("insert first test: error")

    arr_copy = arr2.copy()
    BubbleSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("bubble second test: ok")
    else:
        print("bubble second test: error")
    arr_copy = arr2.copy()
    SelectionSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("select second test: ok")
    else:
        print("select second test: error")
    arr_copy = arr2.copy()
    InsertSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("insert second test: ok")
    else:
        print("insert second test: error")

    arr_copy = arr3.copy()
    BubbleSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("bubble third test: ok")
    else:
        print("bubble thirdd test: error")
    arr_copy = arr3.copy()
    SelectionSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("select third test: ok")
    else:
        print("select third test: error")
    arr_copy = arr3.copy()
    InsertSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("insert third test: ok")
    else:
        print("insert third test: error")

    arr_copy = arr4.copy()
    BubbleSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("bubble fourth test: ok")
    else:
        print("bubble fourth test: error")
    arr_copy = arr4.copy()
    SelectionSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("select fourth test: ok")
    else:
        print("select fourth test: error")
    arr_copy = arr4.copy()
    InsertSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr2ok):
        print("insert fourth test: ok")
    else:
        print("insert fourth test: error")

    arr_copy = arr5.copy()
    BubbleSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr3ok):
        print("bubble fifth test: ok")
    else:
        print("bubble fifth test: error")
    arr_copy = arr5.copy()
    SelectionSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr3ok):
        print("select fifth test: ok")
    else:
        print("select fifth test: error")
    arr_copy = arr5.copy()
    InsertSort(arr_copy, len(arr_copy))
    if compare_arr(arr_copy, arr3ok):
        print("insert fifth test: ok")
    else:
        print("insert fifth test: error")

def print_matrix(n,a):
    for i in range(n):
        for j in range(n):
            print(a[i][j],end=' ')
        print()

if __name__ == '__main__':
    full_test()

    n = int(input())
    a = [[None] * n for i in range(n)]
    x = y = k = 0
    t = 1
    for i in range(n * n):
        if k % 3 == 0:
            if 0 <= y + 1 < n and a[x][y + 1] == None:
                a[x][y] = t
                t += 1
                y += 1
            else:
                k += 1
                continue
        elif k % 3 == 1:
            if 0 <= x + 1 < n and 0 <= y - 1 < n and a[x + 1][y - 1] == None:
                a[x][y] = t
                t += 1
                x += 1
                y -= 1
            else:
                k += 1
        else:
            if 0 <= x - 1 < n and a[x - 1][y] == None:
                a[x][y] = t
                t += 1
                x -= 1
            else:
                k += 1
                continue
    for i in range(n):
        for j in range(n):
            if i > n - j - 1:
                a[i][j] = 0
    for i in range(n):
        for j in range(n):
            if a[i][j] == None:
                a[i][j] = t
    print_matrix(n, a)