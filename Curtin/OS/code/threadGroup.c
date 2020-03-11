/*////////////////////////////////////////////
 * FILE:        threadGroup.c
 * REFER TO:    threadGroup.h
 *
 * UNIT:        COMP2006
 * AUTHOR:      Jason Gilbert
 * STUDENT ID:  XXXXXXXX
/*///////////////////////////////////////////
#include "threadGroup.h"


/*///////////////////////////////////
 * FUNCTION: initThreadGroup
 * REFER TO: threadGroup.h
/*///////////////////////////////////
ThreadGroup* initThreadGroup(int count)
{
    ThreadGroup* tg = malloc(sizeof(ThreadGroup));
    tg->threads = calloc(count, sizeof(pthread_t));
    tg->count   = count;
    pthread_attr_init(&tg->attr);   // default initialisation of attr
    
    return tg;
}

/*///////////////////////////////////
 * FUNCTION: freeThreadGroup
 * REFER TO: threadGroup.h
/*///////////////////////////////////
void freeThreadGroup(ThreadGroup* ptr)
{
    free(ptr->threads);
    free(ptr);
}

/*///////////////////////////////////
 * FUNCTION: runThreadGroup
 * REFER TO: threadGroup.h
/*///////////////////////////////////
void runThreadGroup(ThreadGroup* tg, starterFunc sfunc, void* argSet[])
{
    // Create & Launch threads //
    for (int i = 0; i < tg->count; i++)
    {
        pthread_create(&tg->threads[i], &tg->attr, sfunc, argSet[i]);
    }
    
    // Wait for completion of all threads //
    fflush(stdout);
    printf("> Parent waiting...\n");
    for (int j = 0; j < tg->count; j++)
    {
        pthread_join(tg->threads[j], NULL); // Thread complete
        printf("\t<thread[%d] complete!>\n", j);
    }
    printf("> Parent ready!\n");
}


