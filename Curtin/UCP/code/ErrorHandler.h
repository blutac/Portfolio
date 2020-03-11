/*///////////////////////////////////////////
 * FILE:    ErrorHandler.h
 * PURPOSE: Contains ErrorHandler specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef ERROR_HANDLER_H
#define ERROR_HANDLER_H

#include <stdio.h>
#include <stdlib.h>

/*
   Notes on Errors:
   > Due to the nature of the error values, Multiple errors can be captured,
     although the structure of the program prevents multiple errors from occuring.
   > Generally, only void functions will return error flags.
   > Each error flag has a corresponding error message.
   > Errors flags are prefixed with one of 2 prefixes
      - INVALID   (for illegal user inputs)
      - FAILURE   (for failed operations)
     
     Usage:
     Whenever a function contains a paramater of <int* err>
     it has the additional precondition of being provided with a valid int pointer.
*/


/*ERROR FLAGS*/
#define OK (0)                         /*No errors encounted*/
#define INVALID_ARGUMENT      (-1)     /*The user given argument is illegal*/
#define INVALID_HEADER        (-2)     /*NULL or otherwise invalid CSV header row*/
#define INVALID_DATATYPE      (-4)     /*NULL or otherwise invalid CSV datatype*/
#define INVALID_COLUMN        (-8)     /*NULL or otherwise invalid CSV header column*/
#define INVALID_COLUMN_COUNT  (-16)    /*Column count differs from header*/
#define INVALID_DATA_ROW      (-32)    /*NULL or otherwise invalid CSV data row*/
#define FAILURE_FILE_OPEN     (-64)    /*The given file could not be opened for access*/
#define FAILURE_FILE_ACCESS   (-128)   /*The given file could not be accessed (ie; read or written to)*/
#define FAILURE_FILE_IMPORT   (-256)   /*The file import operation failed*/
#define FAILURE_FILE_EXPORT   (-512)   /*The file export operation failed*/
#define FAILURE_SORT          (-1024)  /*Insertion sort failed*/


/*ERROR MESSAGES*/
#define INVALID_ARGUMENT_MSG      ("Supplied arguments was invalid!")
#define INVALID_HEADER_MSG        ("The header line of this file has an invalid format!")
#define INVALID_DATATYPE_MSG      ("The attempted column to be sorted by has an unknown datatype!")
#define INVALID_COLUMN_MSG        ("The attempted column to be sorted by has an invalid column header format!")
#define INVALID_COLUMN_COUNT_MSG  ("One or more rows do not match the column count!")
#define INVALID_DATA_ROW_MSG      ("One or more rows contain NULL or otherwise unreadable data!")
#define FAILURE_FILE_OPEN_MSG     ("Failed to open file!")
#define FAILURE_FILE_ACCESS_MSG   ("Failed to read/write to file!")
#define FAILURE_FILE_IMPORT_MSG   ("Fatal error in CSV import operation!")
#define FAILURE_FILE_EXPORT_MSG   ("Fatal error in CSV export operation!")
#define FAILURE_SORT_MSG          ("Fatal error in CSV sorting operation!")




/*///////////////////////////////////
 * FUNCTION: printErr
 * PURPOSE:  Prints the error message
 *           for the corresponding error flag.
 * EXPORTS:  NONE
 * IMPORTS:  err     (Error number)
 *
 * ASSERTIONS:
 * PRE:  err is not NULL.
 * POST: A message is printed to the screen.
///////////////////////////////////*/
void printErr(int err);


#endif
