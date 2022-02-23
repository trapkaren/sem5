#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <iostream>

#define N 2 /* N потоков */
#define M 20 /* Размер массива */

int a[M];
int b[1] = {1};
int c[100] = {26,76,3,44,3,30,71,43,26,44,90,54,50,52,96,80,95,70,77,16,20,57,46,22,81,32,75,67,29,46,53,77,23,50,52,91,55,87,85,2,26,58,47,94,30,72,30,61,43,36,57,67,27,37,39,85,77,26,28,68,17,92,6,13,82,73,58,5,17,40,46,31,46,1,10,89,17,73,89,18,34,57,91,22,19,14,58,59,68,68,2,74,9,84,10,44,71,91,75,62};
int d[100] = {96,95,94,92,91,91,91,90,89,89,87,85,85,84,82,81,80,77,77,77,76,75,75,74,73,73,72,71,71,70,68,68,68,67,67,62,61,59,58,58,58,57,57,57,55,54,53,52,52,50,50,47,46,46,46,46,44,44,44,43,43,40,39,37,36,34,32,31,30,30,30,29,28,27,26,26,26,26,23,22,22,20,19,18,17,17,17,16,14,13,10,10,9,6,5,3,3,2,2,1};
int e[100] = {1,2,2,3,3,5,6,9,10,10,13,14,16,17,17,17,18,19,20,22,22,23,26,26,26,26,27,28,29,30,30,30,31,32,34,36,37,39,40,43,43,44,44,44,46,46,46,46,47,50,50,52,52,53,54,55,57,57,57,58,58,58,59,61,62,67,67,68,68,68,70,71,71,72,73,73,74,75,75,76,77,77,77,80,81,82,84,85,85,87,89,89,90,91,91,91,92,94,95,96};

typedef struct Arr {
    int low;
    int high;
} ArrayIndex;

using namespace std;

void generate_array()
{
    srand(time(0));

    for (int i = 0; i < M; i++)
    {
        a[i] = 1 + rand() % 100;
    }
}

/* подпрограмма образования упорядоченного результирующего
 * массива путем слияния двух также отсортированных
 * массивов меньших размеров
*/
void merge(int first, int last)
{
    int *mas = new int[M];

    int middle = (first + last) / 2;
    int start = first; //начало левой части
    int final = middle + 1; //начало правой части

    int j;
    for(j = first; j <= last; j++)
    {
        if ((start <= middle) && ((final > last) || (a[start] < a[final])))
        {
            mas[j] = a[start];
            start++;
        }
        else
        {
            mas[j] = a[final];
            final++;
        }
    }
    //возвращение результата в список
    for (j = first; j <= last; j++) a[j] = mas[j];
    delete []mas;
}


void * mergesort_threads(void *a)
{
    ArrayIndex *pa = (ArrayIndex *)a;
    int mid = (pa->low + pa->high)/2;

    ArrayIndex aIndex[N];
    pthread_t thread[N];

    aIndex[0].low = pa->low;
    aIndex[0].high = mid;

    aIndex[1].low = mid+1;
    aIndex[1].high = pa->high;

    if (pa->low >= pa->high) return 0;

    int i;
    for(i = 0; i < N; i++) pthread_create(&thread[i], NULL, mergesort_threads, &aIndex[i]);
    for(i = 0; i < N; i++) pthread_join(thread[i], NULL);

    merge(pa->low, pa->high);

    return 0;
}

int mergesort(void *a, int first, int last)
{
    if (first < last)
    {
        mergesort(a, first, (first+last) / 2); //сортировка левой части
        mergesort(a, (first+last) / 2 + 1, last); //сортировка правой части

        merge(first, last); //слияние двух частей
    }
}

void test_time()
{
    ArrayIndex ai;
    ai.low = 0;
    ai.high = sizeof(a)/sizeof(a[0])-1;

    generate_array();

    double Mergetime = 0;

    for (int j = 0; j < 10; j++)
    {
        auto time1 = chrono::steady_clock::now();
        mergesort(a, ai.low, ai.high);
        auto time2 = chrono::steady_clock::now();

        Mergetime += chrono::duration_cast<chrono::microseconds>(time2 - time1).count() / 1000.0;
    }
    std::cout << "Размер массива: " << M << endl;
    std::cout << "Время сортировки однопоточно: " << Mergetime / 10 << endl;
    std::cout << endl;

    Mergetime = 0;

    generate_array();

    pthread_t thread;

    for (int j = 0; j < 10; j++)
    {
        auto time1 = chrono::steady_clock::now();
        pthread_create(&thread, NULL, mergesort_threads, &ai);
        pthread_join(thread, NULL);
        auto time2 = chrono::steady_clock::now();

        Mergetime += chrono::duration_cast<chrono::microseconds>(time2 - time1).count() / 1000.0;
    }
    std::cout << "Размер массива: " << M << endl;
    std::cout << "Время сортировки "<< N << " потоками: " << Mergetime / 10 << endl;
    std::cout << endl;
}

void print_array()
{
    int i;
    for (i = 0; i < M; i++) printf ("%d ", a[i]);
    cout << endl;
    cout << endl;
}

void test_mergesort()
{
    ArrayIndex bi;
    bi.low = 0;
    bi.high = sizeof(b)/sizeof(b[0])-1;

    mergesort(b, bi.low, bi.high);

    if (b[0] == 1)
        printf("Test Merge Sort 1 = OK\n");
    else
        printf("Test Merge Sort 1 = ERROR\n");

    ArrayIndex ci;
    ci.low = 0;
    ci.high = sizeof(c)/sizeof(c[0])-1;

    mergesort(c, ci.low, ci.high);

    int error = 0;

    for (int i = 0; i < 100; i++)
    {
        if (c[i] != e[i])
            error += 1;
    }

    if (error)
        printf("Test Merge Sort 2 = ERROR\n");
    else
        printf("Test Merge Sort 2 = OK\n");

    ArrayIndex di;
    di.low = 0;
    di.high = sizeof(d)/sizeof(d[0])-1;

    mergesort(d, di.low, di.high);

    error = 0;

    for (int i = 0; i < 100; i++)
    {
        if (d[i] != e[i])
            error += 1;
    }

    if (error)
        printf("Test Merge Sort 3 = ERROR\n");
    else
        printf("Test Merge Sort 3 = OK\n");

    ArrayIndex ei;
    ei.low = 0;
    ei.high = sizeof(d)/sizeof(d[0])-1;

    mergesort(e, ei.low, ei.high);

    error = 0;

    for (int i = 0; i < 100; i++)
    {
        if (e[i] != c[i])
            error += 1;
    }

    if (error)
        printf("Test Merge Sort 4 = ERROR\n");
    else
        printf("Test Merge Sort 4 = OK\n");
}

void test_parallel_mergesort()
{
    ArrayIndex bi;
    bi.low = 0;
    bi.high = sizeof(b)/sizeof(b[0])-1;

    pthread_t thread1;

    pthread_create(&thread1, NULL, mergesort_threads, &bi);
    pthread_join(thread1, NULL);

    if (b[0] == 1)
        printf("Test Merge Sort 1 = OK\n");
    else
        printf("Test Merge Sort 1 = ERROR\n");

    ArrayIndex ci;
    ci.low = 0;
    ci.high = sizeof(c)/sizeof(c[0])-1;

    pthread_t thread2;

    pthread_create(&thread2, NULL, mergesort_threads, &ci);
    pthread_join(thread2, NULL);

    int error = 0;

    for (int i = 0; i < 100; i++)
    {
        if (c[i] != e[i])
            error += 1;
    }

    if (error)
        printf("Test Merge Sort 2 = ERROR\n");
    else
        printf("Test Merge Sort 2 = OK\n");

    ArrayIndex di;
    di.low = 0;
    di.high = sizeof(d)/sizeof(d[0])-1;

    pthread_t thread3;

    pthread_create(&thread3, NULL, mergesort_threads, &di);
    pthread_join(thread3, NULL);

    error = 0;

    for (int i = 0; i < 100; i++)
    {
        if (d[i] != e[i])
            error += 1;
    }

    if (error)
        printf("Test Merge Sort 3 = ERROR\n");
    else
        printf("Test Merge Sort 3 = OK\n");

    ArrayIndex ei;
    ei.low = 0;
    ei.high = sizeof(d)/sizeof(d[0])-1;

    pthread_t thread4;

    pthread_create(&thread4, NULL, mergesort_threads, &ei);
    pthread_join(thread4, NULL);

    error = 0;

    for (int i = 0; i < 100; i++)
    {
        if (e[i] != c[i])
            error += 1;
    }

    if (error)
        printf("Test Merge Sort 4 = ERROR\n");
    else
        printf("Test Merge Sort 4 = OK\n");
}

int main()
{
    ArrayIndex ai;
    ai.low = 0;
    ai.high = sizeof(a)/sizeof(a[0])-1;

    generate_array();
    printf("Исходный массив:\n");
    print_array();


    printf("Массив, отстортированный сортировкой слиянием:\n");
    // для стандартной
    mergesort(a, ai.low, ai.high);
    print_array();

    printf("Массив, отсортированный многопоточной версией сортироваки слиянием:\n");
    // для многопоточной
    pthread_t thread;

    pthread_create(&thread, NULL, mergesort_threads, &ai);
    pthread_join(thread, NULL);

    print_array();

    //test_time();

    //test_mergesort();

    //test_parallel_mergesort();

    return 0;
}