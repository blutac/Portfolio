/*////////////////////////////////////////////
 * FILE:        resources.c
 * REFER TO:    resources.h
 *
 * UNIT:        COMP2006
 * AUTHOR:      Jason Gilbert
 * STUDENT ID:  XXXXXXXX
/*///////////////////////////////////////////
#include "resources.h"

/*///////////////////////////////////
 * FUNCTION: initLocal_SdkGrid
 * REFER TO: resources.h
/*///////////////////////////////////
SdkGrid* initLocal_SdkGrid()
{
    // Initialise local memory //
    SdkGrid* ptr = malloc(sizeof(SdkGrid));	
    // Initialise data //
    ptr->sid = -1; // No sid
    return ptr;
}

/*///////////////////////////////////
 * FUNCTION: initLocal_TaskLog
 * REFER TO: resources.h
/*///////////////////////////////////
TaskLog* initLocal_TaskLog()
{
    // Initialise local memory //
    TaskLog* ptr = malloc(sizeof(TaskLog));
    // Initialise data //
    ptr->sid = -1;  // No sid
    for(int i = 0; i < TASK_COUNT; i++)
        ptr->log[i] = 0;
    return ptr;
}

/*///////////////////////////////////
 * FUNCTION: initLocal_TaskCtr
 * REFER TO: resources.h
/*///////////////////////////////////
TaskCtr* initLocal_TaskCtr()
{
    // Initialise local memory //
    TaskCtr* ptr = malloc(sizeof(TaskCtr));
    // Initialise data //
    ptr->sid = -1;  // No sid
    ptr->count = 0;
    pthread_mutex_init(&(ptr->mut), NULL);  // initialise mutex
    return ptr;
}

/*///////////////////////////////////
 * FUNCTION: initLocal_LogFile
 * REFER TO: resources.h
/*///////////////////////////////////
LogFile* initLocal_LogFile()
{
    // Initialise local memory //
    LogFile* ptr = malloc(sizeof(LogFile));

    // Initialise data //
    ptr->sid = -1;  // No sid
    sprintf(ptr->dir, "%s", "logfile");
    ptr->file = getFile(ptr->dir, "w");     // initialise logfile name
    pthread_mutex_init(&(ptr->mut), NULL);  // initialise mutex
    return ptr;
}

/*///////////////////////////////////
 * FUNCTION: initShared_SdkGrid
 * REFER TO: resources.h
/*///////////////////////////////////
void initShared_SdkGrid(SdkGrid** ptr)
{
    // Initialise shared memory //
    key_t key = ftok(SEED, 'A');
    int sid = shmget(key, sizeof(SdkGrid), 0666 | IPC_CREAT);
    (*ptr) = shmat(sid, (void*)0, 0);
    
    // Initialise data //
    (*ptr)->sid = sid;
}

/*///////////////////////////////////
 * FUNCTION: initShared_TaskLog
 * REFER TO: resources.h
/*///////////////////////////////////
void initShared_TaskLog(TaskLog** ptr)
{
    // Initialise shared memory //
    key_t key = ftok(SEED, 'B');
    int sid = shmget(key, sizeof(TaskLog), 0666 | IPC_CREAT);
    (*ptr) = shmat(sid, (void*)0, 0);
    
    // Initialise data //
    (*ptr)->sid = sid;
    for(int i = 0; i < TASK_COUNT; i++)
        (*ptr)->log[i] = 0;
}

/*///////////////////////////////////
 * FUNCTION: initShared_TaskCtr
 * REFER TO: resources.h
/*///////////////////////////////////
void initShared_TaskCtr(TaskCtr** ptr)
{
    // Initialise shared memory //
    key_t key = ftok(SEED, 'C');
    int sid = shmget(key, sizeof(TaskCtr), 0666 | IPC_CREAT);
    (*ptr) = shmat(sid, (void*)0, 0);
    
    // Initialise data //
    (*ptr)->sid = sid;
    (*ptr)->count = 0;
    sem_init(&((*ptr)->sem), 1, 1); // Initialise semaphore
}

/*///////////////////////////////////
 * FUNCTION: initShared_LogFile
 * REFER TO: resources.h
/*///////////////////////////////////
void initShared_LogFile(LogFile** ptr)
{
    // Initialise shared memory //
    key_t key = ftok(SEED, 'D');
    int sid = shmget(key, sizeof(LogFile), 0666 | IPC_CREAT);
    (*ptr) = shmat(sid, (void*)0, 0);
    
    // Initialise data //
    (*ptr)->sid = sid;
    sprintf((*ptr)->dir, "%s", "logfile");
    (*ptr)->file = getFile((*ptr)->dir, "w");
    sem_init(&((*ptr)->sem), 1, 1);  // Initialise semaphore
}


/*///////////////////////////////////
 * FUNCTION: initShared_Resources
 * REFER TO: resources.h
/*///////////////////////////////////
void initShared_Resources()
{
    // Initialise all globals in shared memory //
    initShared_SdkGrid(&Buffer1);
    initShared_TaskLog(&Buffer2);
    initShared_TaskCtr(&Counter);
    initShared_LogFile(&logfile);
}

/*///////////////////////////////////
 * FUNCTION: initLocal_Resources
 * REFER TO: resources.h
/*///////////////////////////////////
void initLocal_Resources()
{
    // Initialise all globals in local memory //
    Buffer1 = initLocal_SdkGrid();
    Buffer2 = initLocal_TaskLog();
    Counter = initLocal_TaskCtr();
    logfile = initLocal_LogFile();
}

/*///////////////////////////////////
 * FUNCTION: freeShared_Resources
 * REFER TO: resources.h
/*///////////////////////////////////
void freeShared_Resources()
{
    // free misc. objects in globals //
    fclose(logfile->file);
    sem_destroy(&(Counter->sem));
    sem_destroy(&(logfile->sem));
	
    // free all globals in shared memory //
    freeShared_Object(Buffer1, Buffer1->sid);
    freeShared_Object(Buffer2, Buffer2->sid);
    freeShared_Object(Counter, Counter->sid);
    freeShared_Object(logfile, logfile->sid);
}

/*///////////////////////////////////
 * FUNCTION: freeLocal_Resources
 * REFER TO: resources.h
/*///////////////////////////////////
void freeLocal_Resources()
{
    // free misc. objects in globals //
    fclose(logfile->file);
    pthread_mutex_destroy(&(Counter->mut));
    pthread_mutex_destroy(&(logfile->mut));
	
    // free all globals in local memory //
    free(Buffer1);
    free(Buffer2);
    free(Counter);
    free(logfile);
}








/*///////////////////////////////////
 * FUNCTION: freeShared_Object
 * REFER TO: resources.h
/*///////////////////////////////////
void freeShared_Object(void* ptr, int sid)
{
    shmdt(ptr);                  // disconnect from shared memory
    shmctl(sid, IPC_RMID, NULL); // destroy shared memory
}

/*///////////////////////////////////
 * FUNCTION: writeLocal_TaskCtr
 * REFER TO: resources.h
/*///////////////////////////////////
void writeLocal_TaskCtr(TaskCtr* ptr, int data)
{
    pthread_mutex_lock(&(ptr->mut));
    ptr->count += data;
    pthread_mutex_unlock(&(ptr->mut));
}

/*///////////////////////////////////
 * FUNCTION: writeShared_TaskCtr
 * REFER TO: resources.h
/*///////////////////////////////////
void writeShared_TaskCtr(TaskCtr* ptr, int data)
{
    sem_wait(&(ptr->sem));
    ptr->count += data;
    sem_post(&(ptr->sem));
}

/*///////////////////////////////////
 * FUNCTION: writeLocal_LogFile
 * REFER TO: resources.h
/*///////////////////////////////////
void writeLocal_LogFile(LogFile* ptr, char* data)
{
    pthread_mutex_lock(&(ptr->mut));
    fputs(data, ptr->file);
    pthread_mutex_unlock(&(ptr->mut));
}

/*///////////////////////////////////
 * FUNCTION: writeShared_LogFile
 * REFER TO: resources.h
/*///////////////////////////////////
void writeShared_LogFile(LogFile* ptr, char* data)
{
    sem_wait(&(ptr->sem));
    fputs(data, ptr->file);
    sem_post(&(ptr->sem));
}
















/*///////////////////////////////////
 * FUNCTION: printTaskSummary
 * REFER TO: resources.h
/*///////////////////////////////////
void printTaskSummary(TaskLog* logger, TaskCtr* counter)
{
    printf("\n");
    printf("============================================================\n");
    for (int i = 0; i < GLEN; i++)
    {
        printf("Validation result from process ID-%d: row is ", i+1);
        if (logger->log[i] == 1)
            printf("valid\n");
        else
            printf("invalid\n");
    }
    
    printf("Validation result from process ID-10: %d of 9 columns are valid\n", logger->log[TASK_COL]);
    printf("Validation result from process ID-11: %d of 9 sub-grids are valid\n", logger->log[TASK_BOX]);
    printf("There are %d valid sub-grids, and thus the solution is ", counter->count);
    
    if (counter->count == 27)
        printf("valid\n");
    else
        printf("invalid\n");

    printf("============================================================\n");
}
