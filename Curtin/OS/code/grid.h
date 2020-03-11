/*////////////////////////////////////////////
 * FILE:    grid.h
 * PURPOSE: Provides a data structures and methods to handle 
 *          sudoku grid and subgrid initialisation, selection and printing
 *
 * UNIT:    COMP2006
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
/*///////////////////////////////////////////
#ifndef GRID_H
#define GRID_H
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <math.h>
/*///////////////////////////////////
 * ENUM:  Shape
 * PURPOSE: Represents a subgrid type
 * IMPORTS: ROW (Row)
 *          COL (Column)
 *          BOX (3x3 Box for a 9x9 grid)
/*///////////////////////////////////
typedef enum {
    ROW = 0,
    COL = 1,
    BOX = 2
} Shape;

/*///////////////////////////////////
 * STRUCT:  Selection
 * PURPOSE: Specifies a subgrid selection to retrieve
 * IMPORTS: shape (Type of selection)
 *          zone  (Index of shape)
 * REMARKS: zone is base-0 with low values starting:
 *          Left to Right, Top to Bottom
/*///////////////////////////////////
typedef struct {
    Shape shape;
    int zone;   // base 0
} Selection;

/*///////////////////////////////////
 * STRUCT:  TaskSet
 * PURPOSE: Represents a set of tasks (subgrid selections) to process
 * IMPORTS: sel   (Array of Selections to process)
 *          count (Number of elements in sel)
 *          delay (time in seconds to sleep before processing task set)
 *          id    (ID of task)
/*///////////////////////////////////
typedef struct {
    Selection sel[GLEN+1];
    int count;
    int delay;
    int id;
} TaskSet;

/*///////////////////////////////////
 * STRUCT:  Subgrid
 * PURPOSE: Represents a subgrid of a Sudoku grid
 * IMPORTS: items (elements in subgrid)
 *          sel   (location of subgrid)
 *          valid (is the subgrid a valid Sudoku solution)
/*///////////////////////////////////
typedef struct {
    int items[GLEN];
    Selection sel;
    bool valid; // not used
} Subgrid;

/*///////////////////////////////////
 * FUNCTION: loadGrid
 * EXPORTS:  void
 * IMPORTS:  sdk  (Sudoku Grid)
 *           data (Raw String data extracted from file)
 *
 * PURPOSE: Loads grid data from a raw string
 * REMARKS: Elements to extract from string must be deliminated by 1 char
/*///////////////////////////////////
void loadGrid(SdkGrid* sdk, char* data);

/*///////////////////////////////////
 * FUNCTION: getSelection
 * EXPORTS:  Selection (New Selection)
 * IMPORTS:  shape (Specified shape)
 *           zone  (Specified zone)
 *
 * PURPOSE: Returns a Selection object
 * REMARKS: 
/*///////////////////////////////////
Selection getSelection(Shape shape, int zone);

/*///////////////////////////////////
 * FUNCTION: getTaskSet
 * EXPORTS:  TaskSet* (New TaskSet*)
 * IMPORTS:  count  (Number of Selection Objects to process)
 *           id     (id of task)
 *
 * PURPOSE: Constructs a TaskSet Object
 * REMARKS: 
/*///////////////////////////////////
TaskSet* getTaskSet(int count, int delay, int id);

/*///////////////////////////////////
 * FUNCTION: getSubgrid
 * EXPORTS:  Subgrid* (New Subgrid*)
 * IMPORTS:  sdk  (SdkGrid to draw from)
 *           sel  (Subgrid to get)
 *
 * PURPOSE: Returns the specified Subgrid from the specified SdkGrid
 * REMARKS: 
/*///////////////////////////////////
Subgrid* getSubgrid(SdkGrid* sdk, Selection sel);

/*///////////////////////////////////
 * FUNCTION: checkSubgrid
 * EXPORTS:  bool (validity of subgrid)
 * IMPORTS:  subgrid (Subgrid to check)
 *
 * PURPOSE: Returns true on a valid subgrid, else false
 * REMARKS: 
/*///////////////////////////////////
bool checkSubgrid(Subgrid* subgrid);

/*///////////////////////////////////
 * FUNCTION: printSubgrid
 * EXPORTS:  void
 * IMPORTS:  subgrid  (subgrid to print)
 *
 * PURPOSE: Prints the Subgrid Object to stdout
 * REMARKS: Used for debuging
/*///////////////////////////////////
void printSubgrid(Subgrid* subgrid);





/*///////////////////////////////////
 * FUNCTION: printGrid
 * EXPORTS:  void
 * IMPORTS:  sdk  (SdkGrid to print)
 *
 * PURPOSE: Prints the SdkGrid Object to stdout
 * REMARKS: 
/*///////////////////////////////////
void printGrid(SdkGrid* sdk);

/*///////////////////////////////////
 * FUNCTION: genLogString
 * EXPORTS:  char* (New String)
 * IMPORTS:  taskID  (From TaskSet object)
 *           subgrid (array of Subgrid*)
 *           count   (number of elements in array)
 *
 * PURPOSE: Returns a log string that details the specified Subgrids
 * REMARKS: Log string is eventually printed to a log file
/*///////////////////////////////////
char* genLogString(int taskID, Subgrid** subgrid, int count);

/*///////////////////////////////////
 * FUNCTION: itos
 * EXPORTS:  char* (New String)
 * IMPORTS:  n     (int to convert)
 *
 * PURPOSE: Converts n to String
 * REMARKS: 
/*///////////////////////////////////
char* itos(int n);

#endif