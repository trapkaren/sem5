#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/wait.h>

#define OK 0
#define ERROR 1
#define SLEEP_TIME 2
#define ERROR_FORK -1

void check_status(int status, int num_proc);

int main()
{
    setbuf(stdout, NULL);

    pid_t childpid_1, childpid_2;

    printf(" Parent process PID: %d, GROUP: %d\n", getpid(), getpgrp());

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
        printf("Child Process №1 PID: %d, PPID: %d, GROUP: %d\n", getpid(), getppid(), getpgrp());
        return OK;
    }

    int status;

    childpid_1 = wait(&status);
    printf("Child process (PID: %d) finished. Status: %d\n", childpid_1, status);
    check_status(status, 1);

    childpid_2 = wait(&status);
    printf("Child process (PID: %d) finished. Status: %d\n", childpid_2, status);
    check_status(status, 2);

    printf("Parent Process, Children PID: child1 %d, child2 %d\n\
    Parent Process is dead.\n",
           childpid_1, childpid_2);

    return OK;
}

void check_status(int status, int num_proc)
{
    if (WIFEXITED(status))
        printf("Child process №%d finished with code: %d\n", num_proc, WEXITSTATUS(status));

    if (WIFSIGNALED(status))
        printf("Child process №%d finished from signal: %d\n", num_proc, WTERMSIG(status));

    if (WIFSTOPPED(status))
        printf("Child process №%d finished stopped with code: %d\n", num_proc, WSTOPSIG(status));
}
