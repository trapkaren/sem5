#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/wait.h>
#include <string.h>

#define OK 0
#define ERROR 1
#define SLEEP_TIME 2
#define LEN 32
#define N 2
#define GET 1

int mode = 0;

void check_status(int status, int num_proc);
void change_mode(int sigint);

int main()
{
	setbuf(stdout, NULL);
	
	int fd[N];
	pid_t childpid_1, childpid_2;
	
	const char *const messages[N] = {"qwerty\n", "1 234\n"};
	
	printf("Press Ctrl + C to send messages.\n");
	
	signal(SIGINT, change_mode);
	
	if (pipe(fd) == -1)
	{
	  perror("Can't pipe.\n");
	  return ERROR;
	}

	printf("Parent process PID: %d, GROUP: %d\n", 
	getpid(), getpgrp());

	if ((childpid_1 = fork()) == -1)
	{
	  perror("Can\'t fork.\n");
	  return ERROR;
	}
	
	if (childpid_1 == 0)
	{
	  sleep(SLEEP_TIME);
		
	  if (mode)
	  {
		  close(fd[0]);
		  write(fd[1], messages[0], strlen(messages[0]));
		  printf("Message №1 has been sent to parent.\n");
	  }
	  else
		  printf("No signal.\n");
		
	  return OK;
	}

	if ((childpid_2 = fork()) == -1)
	{
	  perror("Can\'t fork.\n");
	  return ERROR;
	}
	
	if (childpid_2 == 0)
	{
	  sleep(SLEEP_TIME);
		
	  if (mode)
	  {
		  close(fd[0]);
		  write(fd[1], messages[1], strlen(messages[1]));
		  printf("Message №2 has been sent to parent.\n");
	  }
	  else
		  printf("No signal.\n");
		
	  return OK;
	}
	
	int status;
	
	childpid_1 = wait(&status);
	printf("Child process (PID: %d) finished. Status: %d\n", 
	childpid_1, status);
	check_status(status, 1);
	
	childpid_2 = wait(&status);
	printf("Child process (PID: %d) finished. Status: %d\n", 
	childpid_2, status);
	check_status(status, 2);
	
	printf("Press Ctrl + C for read from pipe.\n");
	sleep(SLEEP_TIME);
	
	if (mode)
	{
	  close(fd[1]);
	  char message[LEN] = {0};
		
	  read(fd[0], message, strlen(messages[0]) + strlen(messages[1]));
		
	  printf("Messages:\n");
	  printf("%s", message);
	}
	else
	  printf("Ctrl + C was not pressed.\n");

	printf("Parent Process is dead.\n");

	return OK;
}

void check_status(int status, int num_proc)
{
	if (WIFEXITED(status))
	{
	  printf("Child process №%d finished with code: %d\n", 
	  num_proc, WEXITSTATUS(status));
	}
	
	if (WIFSIGNALED(status))
	{
	  printf("Child process №%d finished from signal: %d\n", 
	  num_proc, WTERMSIG(status));
	}
	
	if (WIFSTOPPED(status))
	{
	  printf("Child process №%d finished stopped with code: %d\n", 
	  num_proc, WSTOPSIG(status));
	}
}

void change_mode(int sigint)
{
	mode = GET;
	printf("SIGINT\n");
}
