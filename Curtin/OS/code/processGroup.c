/*////////////////////////////////////////////
 * FILE:        processGroup.c
 * REFER TO:    processGroup.h
 *
 * UNIT:        COMP2006
 * AUTHOR:      Jason Gilbert
 * STUDENT ID:  XXXXXXXX
/*///////////////////////////////////////////
#include "processGroup.h"


/*///////////////////////////////////
 * FUNCTION: initProcessGroup
 * REFER TO: processGroup.h
/*///////////////////////////////////
ProcessGroup* initProcessGroup(int count)
{
    ProcessGroup* pg = malloc(sizeof(ProcessGroup));
    pg->pid = calloc(count, sizeof(int));
    pg->count = count;
    
    return pg;
}

/*///////////////////////////////////
 * FUCTION: freeProcessGroup
 * REFER TO: processGroup.h
/*///////////////////////////////////
void freeProcessGroup(ProcessGroup* ptr)
{
    free(ptr->pid);
    free(ptr);
}

/*///////////////////////////////////
 * FUNCTION: runProcessGroup
 * REFER TO: processGroup.h
/*///////////////////////////////////
void runProcessGroup(ProcessGroup* pg, starterFunc sfunc, void* argSet[])
{
    int pid;
    
    // Create & Launches child processes //
    for (int i = 0; i < pg->count; i++)
    {
        pid = fork();
        if (pid == 0)           // new child process
        {
            (*sfunc)(argSet[i]);    // Do work
            exit(0);                // Terminate child process
        }
        else if (pid > 0)       // original parent process
        {
            pg->pid[i] = pid;       // store child pid
        }
    }
    
    // Wait for the completion of all child processes //
    fflush(stdout);
    printf("> Parent waiting...\n");
    for (int j = 0; j < pg->count; j++)
    {
        wait(NULL);
        printf("\t<processID[%d] complete!>\n", pg->pid[j]);
    }
    printf("> Parent ready!\n");
}

