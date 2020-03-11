/*////////////////////////////////////////////
 * FILE:    mssv.h
 * PURPOSE: Contains main entry point into program
 * REMARKS: 
 *
 * UNIT:    COMP2006
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
/*///////////////////////////////////////////
#ifndef MSSV_H
#define MSSV_H

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stdbool.h>
#include <pthread.h>
#include <time.h>

#define GLEN (9)  // Sudoku Grid Length
#define SLEN (3)  // Sudoku Subgrid Length (should be square root of GLEN)
#define FILE_SIZE (GLEN * GLEN * 2) // Grid elements + whitespace chars

#define TASK_COUNT (11)             // Number of child tasks to create
#define TASK_COL (TASK_COUNT - 2)   // Second last task will process columns
#define TASK_BOX (TASK_COUNT - 1)   // Last task will process Subgrid boxes
#define RAND(max) ((rand() % max) + 1) // For delay

/*///////////////////////////////////
 * FUNCTION: main
 * EXPORTS:  int   (exit code)
 * IMPORTS:  argc  (arg count)
 *           argv  (arg array)
 *
 * PURPOSE: Validates a specified Sudoku grid
 * REMARKS: 
/*///////////////////////////////////
int main(int argc, char* argv[]);


/*///////////////////////////////////
 * FUNCTION: childTask
 * EXPORTS:  void* (NULL)
 * IMPORTS:  args  (information to process, in this case:
 *                  a TaskSet struct)
 *
 * PURPOSE: Performs the Sudoku validation on all subgrids specified
 *          by the imported TaskSet.
 * REMARKS: this function is used by both child processes and threads.
/*///////////////////////////////////
void* childTask(void* args);

/*///////////////////////////////////
 * FUNCTION: validateArgs
 * EXPORTS:  void
 * IMPORTS:  argc  (arg count)
 *           argv  (arg array)
 *
 * PURPOSE: Validates command line arguments
 * REMARKS: exits program if invalid
/*///////////////////////////////////
void validateArgs(int argc, char* argv[]);

#endif




