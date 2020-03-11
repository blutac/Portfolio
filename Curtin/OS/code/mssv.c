/*////////////////////////////////////////////
 * FILE:        mssv.c
 * REFER TO:    mssv.h
 *
 * UNIT:        COMP2006
 * AUTHOR:      Jason Gilbert
 * STUDENT ID:  XXXXXXXX
/*///////////////////////////////////////////
#include "mssv.h"
#include "fileIO.c"
#include "resources.c"
#include "grid.c"
#include "threadGroup.c"
#include "processGroup.c"

/*///////////////////////////////////
 * FUNCTION: childTask
 * REFER TO: mssv.h
/*///////////////////////////////////
void* childTask(void* args)
{
    printf("\t<Child[%d] sleep[%d]>\n", ((TaskSet*)args)->id,
                                        ((TaskSet*)args)->delay);
    sleep(((TaskSet*)args)->delay);
    
    //// Initialisation /////////////////
    TaskSet* tSet = args;
    int items = tSet->count;
    int id = tSet->id;
    int xCount = 0;
    
    Subgrid** sgrid = calloc(items, sizeof(Subgrid*));   // All assigned subgrids
    Subgrid** sgrid_x = calloc(items, sizeof(Subgrid*)); // invalid subgrids
    /////////////////////////////////////

    //// Processing Taskset /////////////
    for (int i = 0; i < items; i++)
    {
        sgrid[i] = getSubgrid(Buffer1, tSet->sel[i]);
        
        if (!checkSubgrid(sgrid[i])) // Store invalid subgrids
        {
            sgrid_x[xCount] = sgrid[i];
            xCount++;
        }
    }
    /////////////////////////////////////

    //// Log invalid subgrids ///////////
    if (xCount != 0)
    {
        char* str = genLogString(id, sgrid_x, xCount);
        #ifdef THREADED
            writeLocal_LogFile(logfile, str);
        #else
            writeShared_LogFile(logfile, str);
        #endif
        free(str);
    }
    /////////////////////////////////////

    //// Log valid subgrid count ////////
    #ifdef THREADED
        writeLocal_TaskCtr(Counter, items - xCount);
    #else
        writeShared_TaskCtr(Counter, items - xCount);
    #endif    
    Buffer2->log[id - 1] = (items - xCount);
    /////////////////////////////////////
    
    //// Clean-up ///////////////////////
    for (int i = 0; i < items; i++)
        free(sgrid[i]);
    free(sgrid);
    free(sgrid_x);
    #ifndef THREADED
        free(tSet);
        shmdt(Buffer1); // Disconnect child from shared mem
        shmdt(Buffer2);
        shmdt(Counter);
        shmdt(logfile);
    #endif
    /////////////////////////////////////
    
    return NULL;
}

/*///////////////////////////////////
 * FUNCTION: validateArgs
 * REFER TO: mssv.h
/*///////////////////////////////////
void validateArgs(int argc, char* argv[])
{
    if (argc != 3)
    {
        printf("Usage: .\\mssv file delay\n");
        exit(1);
    }
    if (atoi(argv[2]) < 1)
    {
        printf("delay must be greater than 0\n");
        exit(1);
    }
}




/*///////////////////////////////////
 * FUNCTION: main
 * REFER TO: mssv.h
/*///////////////////////////////////
int main(int argc, char* argv[])
{
    validateArgs(argc, argv);

    printf("============================================================\n");
    /////////////////////////////////////
    #ifdef THREADED
        printf("> Initialising local resources....");
        initLocal_Resources();
    #else
        printf("> Initialising shared resources....");
        initShared_Resources();
    #endif
    printf("OK!\n");
    /////////////////////////////////////

    /////////////////////////////////////
    printf("> Loading grid data from file: '%s'...\n", argv[1]);
    FILE* f = getFile(argv[1], "r");
    char* str = readString(f, FILE_SIZE);
    loadGrid(Buffer1, str);
    printGrid(Buffer1);
    fclose(f);
    free(str);
    /////////////////////////////////////
    
    /////////////////////////////////////
    printf("> Generating task sets... ");
    int max = atoi(argv[2]);
    srand((int)&max);

    TaskSet** tSet = calloc(TASK_COUNT, sizeof(TaskSet*));
    tSet[TASK_COL]  = getTaskSet(GLEN, RAND(max), TASK_COL + 1);
    tSet[TASK_BOX] = getTaskSet(GLEN, RAND(max), TASK_BOX + 1);
    
    for (int i = 0; i < GLEN; i++)
    {
        tSet[i] = getTaskSet(1, RAND(max), i + 1);
        tSet[i]->sel[0]  = getSelection(ROW, i);
        tSet[TASK_COL]->sel[i]  = getSelection(COL, i);
        tSet[TASK_BOX]->sel[i] = getSelection(BOX, i);
    }
    printf("OK!\n");
    /////////////////////////////////////

    /////////////////////////////////////
    printf("> Launching Child tasks... \n");
    printf("============================================================\n");
    #ifdef THREADED
        ThreadGroup* tg = initThreadGroup(TASK_COUNT);
        runThreadGroup(tg, &childTask, (void*)tSet);
        freeThreadGroup(tg);
    #else
        ProcessGroup* pg = initProcessGroup(TASK_COUNT);
        runProcessGroup(pg, &childTask, (void*)tSet);
        freeProcessGroup(pg);
    #endif
    /////////////////////////////////////

    printTaskSummary(Buffer2, Counter);
    
    /////////////////////////////////////
    printf("> Freeing Resources... ");
    #ifdef THREADED
        freeLocal_Resources();
    #else
        freeShared_Resources();
    #endif
    
    for (int i = 0; i < TASK_COUNT; i++)
        free(tSet[i]);
    free(tSet);
    printf("OK!\n");
    /////////////////////////////////////
    
    printf("> Finished!\n");
    return 0;
}
















