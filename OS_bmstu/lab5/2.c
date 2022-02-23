#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <sys/sem.h>
#include <sys/shm.h>
#include <time.h>
#include <unistd.h>
#include <sys/wait.h>

#define PERMS S_IRWXU | S_IRWXG | S_IRWXO

#define COUNT 20
#define WRITERS 3
#define READERS 5

#define ACTIVE_READERS 0    // активные читатели
#define WAIT_WRITERS 2  // ждущие писатели
#define ACTIVE_WRITERS 1    // активные писатели
#define WAIT_READERS 3    // ждущие читатели

#define INC 1
#define DEC -1

struct sembuf start_reading[] = {
   {WAIT_READERS, INC, SEM_UNDO},
   {WAIT_WRITERS, 0, SEM_UNDO},
   {ACTIVE_WRITERS, 0, SEM_UNDO},
   {WAIT_READERS, DEC, SEM_UNDO},
   {ACTIVE_READERS, INC, SEM_UNDO}
};
struct sembuf stop_read[] = {
   {ACTIVE_READERS, DEC, SEM_UNDO}
};
struct sembuf start_writing[] = {
   {WAIT_WRITERS, INC, SEM_UNDO},
   {ACTIVE_WRITERS, 0, SEM_UNDO},
   {ACTIVE_READERS, 0, SEM_UNDO},
   {WAIT_WRITERS, DEC, SEM_UNDO},
   {ACTIVE_WRITERS, INC, SEM_UNDO}
};
struct sembuf stop_write[] = {
   {ACTIVE_WRITERS, DEC, SEM_UNDO}
};

int sem_id = -1;
int shm_id = -1;

int *shm = NULL;
int *shared_value = NULL;
pid_t* child_pids = NULL;

void writer(int number)
{
   int start = semop(sem_id, start_writing, 5);
   if (start == -1) {
       perror("start writing");
       exit(1);
   }
  
   (*shared_value)++;
   printf("Writer #%d, pid=%d wrote value %d\n", number, getpid(), *shared_value);

   int stop = semop(sem_id, stop_write, 1);
   if (stop == -1) {
       perror("stop write");
       exit(1);
   }

   sleep(2);
}

void reader(int number)
{
   int start = semop(sem_id, start_reading, 5);
   if (start == -1) {
       perror("start reading");
       exit(1);
   }
  
   int val = *shared_value;
   printf("Reader #%d, pid=%d read value: %d\n", number, getpid(), val);

   int stop = semop(sem_id, stop_read, 1);
   if (stop == -1) {
       perror("stop read");
       exit(1);
   }

   sleep(1);
}

/* обработчик сигнала ctrl-c */
void catch_sig(int sig_numb)
{
   signal(sig_numb, catch_sig);
   shmctl(shm_id, IPC_RMID, NULL);
   semctl(sem_id, 0, IPC_RMID, 0);
}

int main()
{
   sem_id = semget(IPC_PRIVATE, 4, IPC_CREAT | PERMS);
   if (sem_id == -1) {
       perror("semget");
       exit(1);
   }
  
   if (semctl(sem_id, ACTIVE_READERS, SETVAL, 0) == -1 ||
       semctl(sem_id, WAIT_WRITERS, SETVAL, 0) == -1 ||
       semctl(sem_id, ACTIVE_WRITERS, SETVAL, 0) == -1 ||
       semctl(sem_id, WAIT_READERS, SETVAL, 0) == -1) {
       perror("semctl");
       exit(1);
   }

   shm_id = shmget(IPC_PRIVATE, sizeof(int), IPC_CREAT | PERMS);
   if (shm_id == -1) {
       perror("shmget");
       exit(1);
   }
   shm = shmat(shm_id, 0, 0);
   if (*shm == -1) {
       perror("shmat");
       exit(1);
   }

   shared_value = shm;
   child_pids = shm + 1;
   *shared_value = 0;

   for (int i = 0; i < WRITERS; i++)
   {
       pid_t pid;

       if ((pid = fork()) == -1) {
           printf("Can't fork");
           exit(1);
       }

       if (pid == 0) {
           printf("Writer #%d is running, pid: %d\n", i, getpid());
           while (1) {
               writer(i);
           }
       }
       else {
           child_pids[i] = pid;
       }
   }

   for (int i = 0; i < READERS; i++)
   {
       pid_t pid;

       if ((pid = fork()) == -1) {
           printf("Can't fork");
           exit(1);
       }

       if (pid == 0) {
           printf("Reader #%d created, pid: %d\n", i, getpid());
           while (1) {
               reader(i);
           }
       }
       else {
           child_pids[WRITERS + i] = pid;
       }
   }
  
   signal(SIGINT, catch_sig);

   for (int i = 0; i < WRITERS + READERS; i++)
   {
       int status;
       const pid_t child_pid = wait(&status);
       if (child_pid == -1) {
           perror("wait");
           exit(1);
       }

       if (WIFEXITED(status)) {
           printf("Process %d returns %d\n", child_pid, WEXITSTATUS(status));
       } else if (WIFSIGNALED(status)) {
           printf("Process %d terminated with signal %d\n", child_pid, WTERMSIG(status));
       } else if (WIFSTOPPED(status)) {
           printf("Process %d stopped due signal %d\n", child_pid, WSTOPSIG(status));
       }
   }

   shmctl(shm_id, IPC_RMID, NULL);
   semctl(sem_id, 0, IPC_RMID, 0);

   return 0;
}
