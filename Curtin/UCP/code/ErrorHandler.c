/*///////////////////////////////////////////
 * FILE:    ErrorHandler.c
 * PURPOSE: Contains error handling related methods.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "ErrorHandler.h"


/*///////////////////////////////////
 * FUNCTION: printErr
 * REFER TO: ErrorHandler.h
///////////////////////////////////*/
void printErr(int err)
{
   char* msg = NULL;

   switch (err)
   {
      case OK:
         break;
      case INVALID_ARGUMENT:
         msg = INVALID_ARGUMENT_MSG;
         break;
      case INVALID_HEADER:
         msg = INVALID_HEADER_MSG;
         break;
      case INVALID_DATATYPE:
         msg = INVALID_DATATYPE_MSG;
         break;
      case INVALID_COLUMN:
         msg = INVALID_COLUMN_MSG;
         break;
      case INVALID_COLUMN_COUNT:
         msg = INVALID_COLUMN_COUNT_MSG;
         break;
      case INVALID_DATA_ROW:
         msg = INVALID_DATA_ROW_MSG;
         break;
      case FAILURE_FILE_OPEN:
         msg = FAILURE_FILE_OPEN_MSG;
         break;
      case FAILURE_FILE_ACCESS:
         msg = FAILURE_FILE_ACCESS_MSG;
         break;
      case FAILURE_FILE_IMPORT:
         msg = FAILURE_FILE_IMPORT_MSG;
         break;
      case FAILURE_FILE_EXPORT:
         msg = FAILURE_FILE_EXPORT_MSG;
         break;
      case FAILURE_SORT:
         msg = FAILURE_SORT_MSG;
         break;
      default:
         msg = "UNKNOWN ERROR ENCOUNTED!";
   }

   if (err != OK)
   {
      printf("ERROR{%d} %s\n", err, msg);
   }
}


