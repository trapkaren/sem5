#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>

#define OK 0
#define ERROR 1
#define SLEEP_TIME 2
#define ERROR_FORK -1

int main()
{
    int childpid_1, childpid_2;

    printf("Parent process PID: %d , GROUP: %d\n ", getpid(), getpgrp());

    if ((childpid_1 = fork()) == ERROR_FORK)
    {
        perror("Can’t fork.\n");
        return ERROR;
    }

    if (childpid_1 == 0)
    {
        sleep(SLEEP_TIME);
        printf("Child Process №1 PID: %d, PPID: %d, GROUP: %d\n", getpid(), getppid(), getpgrp());
        return OK;
    }

    if ((childpid_2 = fork()) == ERROR_FORK)
    {
        perror("Can’t fork.\n");
        return ERROR;
    }

    if (childpid_2 == 0)
    {
        sleep(SLEEP_TIME);
        printf("Child Process №2 PID: %d, PPID: %d, GROUP: %d\n", getpid(), getppid(), getpgrp());
        return OK;
    }

    printf("Parent Process, Children PID: child1 %d, child2 %d\n\
    Parent Process is dead.\n", childpid_1, childpid_2);

    return OK;
}
