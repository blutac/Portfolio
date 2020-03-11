/*////////////////////////////////////////////
 * FILE:    resources.h
 * PURPOSE: Provides all shared data resources and the methods to
 *          initialise, free and safely write to them locally or shared.
 * REMARKS: resources.c and mssv.c are the only files that directly
 *          access the globals defined in this file.
 *
 * UNIT:    COMP2006
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
/*///////////////////////////////////////////
#ifndef RESOURCES_H
#define RESOURCES_H

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stdbool.h>
#include <semaphore.h>
#include <pthread.h>
#include <sys/shm.h>
//#include <sys/types.h>
//#include <sys/ipc.h>
#define SEED ("mssv.c") // Used to seed ftok()

/*///////////////////////////////////
 * TYPEDEF: Grid
 * PURPOSE: Represents the grid datatype
 * REMARKS: Grid is initialised in the SdkGrid object
/*///////////////////////////////////
typedef int Grid[GLEN][GLEN];

/*///////////////////////////////////
 * STRUCT:  SdkGrid
 * PURPOSE: Represents Buffer1
 * IMPORTS: sid  (shared memory id)
 *          grid (sudoku grid)
/*///////////////////////////////////
typedef struct {
    int sid;
    Grid grid;
} SdkGrid;

/*///////////////////////////////////
 * STRUCT:  TaskLog
 * PURPOSE: Represents Buffer2
 * IMPORTS: sid (shared memory id)
 *          log (contains the sum of valid subgrids per TaskSet)
/*///////////////////////////////////
typedef struct {
    int sid;
    int log[TASK_COUNT];
} TaskLog;

/*///////////////////////////////////
 * STRUCT:  TaskCtr
 * PURPOSE: Represents Counter
 * IMPORTS: sid   (shared memory id)
 *          count (total count of all valid subgrids)
 *          sem   (semaphore lock)
 *          mut   (mutex lock)
/*///////////////////////////////////
typedef struct {
    int sid;
    int count;
    sem_t sem;
    pthread_mutex_t mut;
} TaskCtr;

/*///////////////////////////////////
 * STRUCT:  LogFile
 * PURPOSE: Represents the Log file
 * IMPORTS: sid  (shared memory id)
 *          dir  (directory of logfile)
 *          file (pointer to logfile)
 *          sem  (semaphore lock)
 *          mut  (mutex lock)
/*///////////////////////////////////
typedef struct {
    int sid;
    char dir[8]; //"logfile\0"
    FILE* file;
    sem_t sem;
    pthread_mutex_t mut;
} LogFile;

// Globals /////////////
SdkGrid* Buffer1 = NULL;
TaskLog* Buffer2 = NULL;
TaskCtr* Counter = NULL;
LogFile* logfile = NULL;
////////////////////////

/*///////////////////////////////////
 * FUNCTIONS: initLocal_<Resource>
 * EXPORTS:  <Resource>*
 * IMPORTS:  void
 *
 * PURPOSE: Initialises <Resource> in local memory
 * REMARKS: 
/*///////////////////////////////////
SdkGrid* initLocal_SdkGrid();
TaskLog* initLocal_TaskLog();
TaskCtr* initLocal_TaskCtr();
LogFile* initLocal_LogFile();

/*///////////////////////////////////
 * FUNCTIONS: initShared_<Resource>
 * EXPORTS:  void
 * IMPORTS:  ptr  (<Resource>)
 *
 * PURPOSE: Initialises <Resource> in shared memory
 * REMARKS: 
/*///////////////////////////////////
void initShared_SdkGrid(SdkGrid** ptr);
void initShared_TaskLog(TaskLog** ptr);
void initShared_TaskCtr(TaskCtr** ptr);
void initShared_LogFile(LogFile** ptr);

/*///////////////////////////////////
 * FUNCTION: initShared_Resources
 * EXPORTS:  void
 * IMPORTS:  void
 *
 * PURPOSE: Initialises all global resources in shared memory
 * REMARKS: 
/*///////////////////////////////////
void initShared_Resources();

/*///////////////////////////////////
 * FUNCTION: initLocal_Resources
 * EXPORTS:  void
 * IMPORTS:  void
 *
 * PURPOSE: Initialises all global resources in local memory
 * REMARKS: 
/*///////////////////////////////////
void initLocal_Resources();

/*///////////////////////////////////
 * FUNCTION: freeShared_Resources
 * EXPORTS:  void
 * IMPORTS:  void
 *
 * PURPOSE: Frees all global resources in shared memory
 * REMARKS: 
/*///////////////////////////////////
void freeShared_Resources();

/*///////////////////////////////////
 * FUNCTION: freeLocal_Resources
 * EXPORTS:  void
 * IMPORTS:  void
 *
 * PURPOSE: Frees all global resources in local memory
 * REMARKS: 
/*///////////////////////////////////
void freeLocal_Resources();


/*///////////////////////////////////
 * FUNCTION: freeShared_Object
 * EXPORTS:  void
 * IMPORTS:  ptr  (pointer to object)
 *           sid  (shared memory id of object)
 *
 * PURPOSE: Frees an object in shared memory
 * REMARKS: 
/*///////////////////////////////////
void freeShared_Object(void* ptr, int sid);


/*///////////////////////////////////
 * FUNCTION: writeLocal_TaskCtr
 * EXPORTS:  void
 * IMPORTS:  ptr  (resource in shared/local memory to write to)
 *           data (data to write)
 *
 * PURPOSE: Safely writes data to the specified shared resource
 * REMARKS: 
/*///////////////////////////////////
void writeLocal_TaskCtr(TaskCtr* ptr, int data);

/*///////////////////////////////////
 * FUNCTION: writeShared_TaskCtr
 * EXPORTS:  void
 * IMPORTS:  ptr  (resource in shared/local memory to write to)
 *           data (data to write)
 *
 * PURPOSE: Safely writes data to the specified shared resource
 * REMARKS: 
/*///////////////////////////////////
void writeShared_TaskCtr(TaskCtr* ptr, int data);

/*///////////////////////////////////
 * FUNCTION: writeLocal_LogFile
 * EXPORTS:  void
 * IMPORTS:  ptr  (resource in shared/local memory to write to)
 *           data (data to write)
 *
 * PURPOSE: Safely writes data to the specified shared resource
 * REMARKS: 
/*///////////////////////////////////
void writeLocal_LogFile(LogFile* ptr, char* data);



/*///////////////////////////////////
 * FUNCTION: writeShared_LogFile
 * EXPORTS:  void
 * IMPORTS:  ptr  (resource in shared/local memory to write to)
 *           data (data to write)
 *
 * PURPOSE: Safely writes data to the specified shared resource
 * REMARKS: 
/*///////////////////////////////////
void writeShared_LogFile(LogFile* ptr, char* data);

/*///////////////////////////////////
 * FUNCTION: printTaskSummary
 * EXPORTS:  void
 * IMPORTS:  logger  (valid subgrids per TaskSet)
 *           counter (total valid subgrids)
 *
 * PURPOSE: Prints sudoku validation results to stdout.
 * REMARKS: 
/*///////////////////////////////////
void printTaskSummary(TaskLog* logger, TaskCtr* counter);


#endif