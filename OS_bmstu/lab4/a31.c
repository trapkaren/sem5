#include <stdio.h>
#include <stdlib.h>

int comp(const int *, const int *);
void vivod(int *a, int n)
{
int i;
for (i = 0; i < n; i++)
        printf("%d ", a[i]);
        printf("\n");
}

int main(void)
{
    int i,n,a[10];
    printf("Input numb of arr:");
    scanf("%d",&n);

    printf("Input %d el of array: ", n);

    for (i = 0; i < n; i++)
    	scanf("%d", &a[i]);

    printf("Icx mas:\n");
    vivod(a,n);

    qsort(a, n, sizeof(int), (int (*)(const void *, const void *))comp);

    printf("Sorted array:\n");
        vivod(a,n);
    
    return 0;
}

int comp(const int *i, const int *j)
{
    return *i - *j;
}
