#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <iostream>
using namespace std;

/* заменить на ввод */
#define N 16 /* N потоков */
#define M 20

/* заменить на генерацию */
int a[M];

void generate_array()
{
    srand(time(0));
    
    for (int i = 0; i < M; i++)
    {
        a[i] = 1 + rand() % 100;
    }
}

/* структура для индекса массива */
typedef struct Arr {
    int low;
    int high;
} ArrayIndex;

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

void print_array()
{
    int i;
    for (i = 0; i < M; i++) printf ("%d ", a[i]);
    cout << endl;
}

void test_time()
{
    ArrayIndex ai;
    ai.low = 0;
    ai.high = sizeof(a)/sizeof(a[0])-1;
    
    generate_array();
    
    pthread_t thread;
    
    double Mergetime = 0;
    
    for (int j = 0; j < 10; j++)
    {
        auto time1 = chrono::steady_clock::now();
        mergesort(a, ai.low, ai.high);
        auto time2 = chrono::steady_clock::now();
        /*auto time1 = chrono::steady_clock::now();
         pthread_create(&thread, NULL, mergesort_threads, &ai);
         pthread_join(thread, NULL);
         
         auto time2 = chrono::steady_clock::now();*/
        Mergetime += chrono::duration_cast<chrono::microseconds>(time2 - time1).count() / 1000.0;
    }
    std::cout << "Размер массива: " << M << endl;
    std::cout << "Время сортировки однопоточно: " << Mergetime / 10 << endl;
    std::cout << endl;
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
    pthread_t thread;
    
    pthread_create(&thread, NULL, mergesort_threads, &ai);
    pthread_join(thread, NULL);
    print_array();
    // для многопоточной
    
    // test_time();
    
    /*  printf("Test merge sort: OK\n");
     printf("Test merge sort: OK\n");
     printf("Test merge sort: OK\n");
     printf("Test merge sort: OK\n");
     printf("Test parallel merge sort: OK\n");
     printf("Test parallel merge sort: OK\n");
     printf("Test parallel merge sort: OK\n");
     printf("Test parallel merge sort: OK\n"); */
    
    return 0;
}
