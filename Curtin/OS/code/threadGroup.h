/*////////////////////////////////////////////
 * FILE:    threadGroup.h
 * PURPOSE: Provides a data structures and methods
 *          to handle a group of threads as a unit
 *
 * UNIT:    COMP2006
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
/*///////////////////////////////////////////
#ifndef THREADGROUP_H
#define THREADGROUP_H
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
/*///////////////////////////////////
 * TYPEDEF: starterFunc
 * PURPOSE: points to a function that
 *          performs work for a thread
/*///////////////////////////////////
typedef void* (*starterFunc)(void* args);

/*///////////////////////////////////
 * STRUCT:  ThreadGroup
 * PURPOSE: Represents a group of threads
 * IMPORTS: threads (Array of thread ids)
 *          attr    (attributes shared by all threads in this group)
 *          count   (Number of threads in this group)
/*///////////////////////////////////
typedef struct {
    pthread_t* threads;
    pthread_attr_t attr;
    int count;
} ThreadGroup;

/*///////////////////////////////////
 * FUNCTION: initThreadGroup
 * EXPORTS:  ThreadGroup* (New ThreadGroup*)
 * IMPORTS:  count  (Number of threads in this group)
 *
 * PURPOSE: Returns an initialised ThreadGroup*.
 * REMARKS: This function does not initiate threads.
/*///////////////////////////////////
ThreadGroup* initThreadGroup(int count);

/*///////////////////////////////////
 * FUNCTION: freeThreadGroup
 * EXPORTS:  void
 * IMPORTS:  ptr  (an initialised ThreadGroup*)
 *
 * PURPOSE: frees the data structure
 * REMARKS: 
/*///////////////////////////////////
void freeThreadGroup(ThreadGroup* ptr);

/*///////////////////////////////////
 * FUNCTION: runThreadGroup
 * EXPORTS:  void
 * IMPORTS:  tg     (an initialised ThreadGroup*)
 *           sfunc  (pointer to a function for thread group to execute)
 *           argSet (Array of arguments for each thread to use in sfunc)
 *
 * PURPOSE: n number of threads (as specified in tg) will be created,
 *          upon which they will execute the specified sfunc with it's
 *          corresponding argument before joining back with the parent
 * REMARKS: Thread ids are stored in tg.
 *          The parent blocks until all thread have completed.
/*///////////////////////////////////
void runThreadGroup(ThreadGroup* tg, starterFunc sfunc, void* argSet[]);
#endif