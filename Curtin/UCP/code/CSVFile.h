/*///////////////////////////////////////////
 * FILE:    CSVFile.h
 * PURPOSE: Contains CSVFile specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef CSVFILE_H
#define CSVFILE_H

#include "Common.h"

#include "FileIO.h"
#include "LinkedList.h"

/*CSV datatypes*/
#define UNKNOWN (-1)
#define INT_CODE (1)
#define INT_NAME ("INTEGER")
#define STRING_CODE (2)
#define STRING_NAME ("STRING")


/*///////////////////////////////////
 * STRUCT:  CSVSelection
 * PURPOSE: Contains data on the CSV column to sort on.
 * IMPORTS: idxColumn  (0 based column index to sort on)
 *          datatype   (datatype of column)
///////////////////////////////////*/
typedef struct
{
   int idxColumn;
   int datatype;
} CSVSelection;


/*///////////////////////////////////
 * STRUCT:  CSVLine
 * PURPOSE: Represents a row in a CSV file.
 * IMPORTS: size     (number of columns in row)
 *          columns  (array of columns)
///////////////////////////////////*/
typedef struct
{
   int size;
   String* columns;
} CSVLine;


/*///////////////////////////////////
 * FUNCTION: importCSVFile
 * PURPOSE:  Import CSV data to LinkList format.
 * EXPORTS:  NONE
 * IMPORTS:  file    (File to import data from)
 *           list    (LinkedList to export data to)
 *           err     (Returning Error flag)
 *
 * ASSERTIONS:
 * PRE:  file is open for reading and point to a file with a valid CSV format.
 *       list != NULL and empty.
 * POST: list is populated with data from file.
///////////////////////////////////*/
void importCSVFile(FILE* file, LinkedList* list, int* err);


/*///////////////////////////////////
 * FUNCTION: exportCSVFile
 * PURPOSE:  Export CSV data to file.
 * EXPORTS:  NONE
 * IMPORTS:  file    (File to export data to)
 *           list    (LinkedList to import data from)
 *           err     (Returning Error flag)
 *
 * ASSERTIONS:
 * PRE:  file is open for witing. list != NULL and contains data.
 * POST: file is populated with CSV lines from list.
///////////////////////////////////*/
void exportCSVFile(FILE* file, LinkedList* list, int* err);


/*///////////////////////////////////
 * FUNCTION: newCSVLine
 * PURPOSE:  Returns a CSVLine* struct initialised with a CSV string.
 * EXPORTS:  CSVLine* (Pointer to new CSVLine)
 * IMPORTS:  line     (CSV String)
 *
 * ASSERTIONS:
 * PRE:  line has a valid CSV format.
 * POST: Returns new initialized CSVLine pointer.
///////////////////////////////////*/
CSVLine* newCSVLine(String line);


/*///////////////////////////////////
 * FUNCTION: newCSVSelection
 * PURPOSE:  Returns a CSVSelection* struct initialised with two ints.
 * EXPORTS:  CSVSelection  (Pointer to new CSVSelection)
 * IMPORTS:  colIdx     (0-based column index to sort on)
 *           datatype   (an int representing a datatype (see CSVFile.h))
 *           err        (Returning error flag)
 *
 * ASSERTIONS:
 * PRE:  None.
 * POST: Returns a CSVSelection* struct initialised with the import values.
///////////////////////////////////*/
CSVSelection* newCSVSelection(int colIdx, int datatype);


/*///////////////////////////////////
 * FUNCTION: freeCSVLine
 * PURPOSE:  Frees any memory allocated for specified CSVLine pointer.
 * EXPORTS:  NONE
 * IMPORTS:  obj     (A CSVLine pointer)
 *
 * ASSERTIONS:
 * PRE:  'obj' is of type 'CSVLine*'.
 * POST: Any memory allocated under 'obj' is freed.
 *
 * REMARKS: 'obj' is of type 'void*' to be compatible with
 *          the freeFunc function typedef. (see LinkedList.h)
///////////////////////////////////*/
void freeCSVLine(void* obj);


/*///////////////////////////////////
 * FUNCTION: freeCSVSelection
 * PURPOSE:  Frees any memory allocated for specified CSVSelection pointer.
 * EXPORTS:  NONE
 * IMPORTS:  obj     (A CSVSelection pointer)
 *
 * ASSERTIONS:
 * PRE:  'obj' is of type 'CSVSelection*'.
 * POST: Any memory allocated under 'obj' is freed.
 *
 * REMARKS: 'obj' is of type 'void*' to be compatible with
 *          the freeFunc function typedef. (see LinkedList.h)
///////////////////////////////////*/
void freeCSVSelection(void* obj);

#endif
