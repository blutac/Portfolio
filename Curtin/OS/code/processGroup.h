/*////////////////////////////////////////////
 * FILE:    processGroup.h
 * PURPOSE: Provides a data structures and methods
 *          to handle a group of child processes as a unit
 *
 * UNIT:    COMP2006
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
/*///////////////////////////////////////////
#ifndef PROCESSGROUP_H
#define PROCESSGROUP_H
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>
/*///////////////////////////////////
 * TYPEDEF: starterFunc
 * PURPOSE: points to a function that
 *          performs work for a thread
/*///////////////////////////////////
typedef void* (*starterFunc)(void* args);

/*///////////////////////////////////
 * STRUCT:  ProcessGroup
 * PURPOSE: Represents a group of child processes
 * IMPORTS: pid     (Array of child pids)
 *          count   (Number of child processes in this group)
/*///////////////////////////////////
typedef struct {
    int* pid;
    int count;
} ProcessGroup;

/*///////////////////////////////////
 * FUNCTION: initProcessGroup
 * EXPORTS:  ProcessGroup* (New ProcessGroup*)
 * IMPORTS:  count  (Number of child processes in this group)
 *
 * PURPOSE: Returns an initialised ProcessGroup*.
 * REMARKS: This function does not initiate child processes.
/*///////////////////////////////////
ProcessGroup* initProcessGroup(int count);

/*///////////////////////////////////
 * FUNCTION: freeProcessGroup
 * EXPORTS:  void
 * IMPORTS:  ptr  (an initialised ProcessGroup*)
 *
 * PURPOSE: frees the data structure
 * REMARKS: 
/*///////////////////////////////////
void freeProcessGroup(ProcessGroup* ptr);

/*///////////////////////////////////
 * FUNCTION: runProcessGroup
 * EXPORTS:  void
 * IMPORTS:  pg     (an initialised ProcessGroup*)
 *           sfunc  (pointer to a function for process group to execute)
 *           argSet (Array of arguments for each process to use in sfunc)
 *
 * PURPOSE: n number of child processes (as specified in pg) will be created,
 *          upon which they will execute the specified sfunc with it's
 *          corresponding argument before terminating.
 * REMARKS: Child pids are stored in pg.
 *          The parent blocks until all child processes have completed.
/*///////////////////////////////////
void runProcessGroup(ProcessGroup* pg, starterFunc sfunc, void* argSet[]);
#endif