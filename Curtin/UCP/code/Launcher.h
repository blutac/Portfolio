/*///////////////////////////////////////////
 * FILE:    Launcher.h
 * PURPOSE: Contains Launcher specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef LAUNCHER_H
#define LAUNCHER_H

#include "Common.h"

#include "FileIO.h"
#include "LinkedList.h"
#include "CSVFile.h"
#include "Sorter.h"

/*///////////////////////////////////
 * STRUCT:  Param
 * PURPOSE: Stores the user specfied arguments.
 * IMPORTS: fileInput   (File input argument)
 *          fileOutput  (File output argument)
///////////////////////////////////*/
typedef struct
{
   String fileInput;
   String fileOutput;
} Params;


/*///////////////////////////////////
 * FUNCTION: getCSVSelection
 * PURPOSE:  Validates the specified CSV header and
 *           prompts the user for a column to sort on.
 * EXPORTS:  CSVSelection* (New CSVSelection pointer)
 * IMPORTS:  header  (CSVLine pointer containing CSV header data)
 *           err     (Returning error flag)
 *
 * ASSERTIONS:
 * PRE:  'header' is pointing to a CSVLine struct containing
 *       a valid CSV header string.
 * POST: Returns a pointer to a CSVSelection struct containing
 *       information on a valid CSV column. Else returns NULL.
 *
 * REMARKS: Only the datatype of the selected column to sort on
 *          will be validated.
///////////////////////////////////*/
CSVSelection* getCSVSelection(CSVLine* header, int* err);


/*///////////////////////////////////
 * FUNCTION: validateCSVList
 * PURPOSE:  Checks the specified list for NULL items and
 *           unexcpected column counts.
 * EXPORTS:  NONE
 * IMPORTS:  list    (LinkedList to check)
 *           colCt   (Expected column count for each item)
 *           err     (Returning error flag)
 *
 * ASSERTIONS:
 * PRE:  A valid column count is given
 * POST: Returns any errors through error flag
///////////////////////////////////*/
void validateCSVList(LinkedList* list, int colCt, int* err);


/*///////////////////////////////////
 * FUNCTION: getUserInput
 * PURPOSE:  Receives user input.
 * EXPORTS:  int     (the user input)
 * IMPORTS:  prompt  (prompt to print to screen)
 *           lBound  (lowest allowed input)
 *           uBound  (highest allowed input)
 *
 * ASSERTIONS:
 * PRE:  lBound is < uBound and are valid ints.
 * POST: an int between lBound and uBound is returned.
///////////////////////////////////*/
int getUserInput(String prompt, int lBound, int uBound);


/*///////////////////////////////////
 * FUNCTION: getArgs
 * PURPOSE:  validates command line arguments and returns
 *           them within a struct if found to be valid.
 * EXPORTS:  Params  (Struct containing arguments)
 * IMPORTS:  i       (Number of arguments)
 *           args[]  (Array of arguments)
 *           err     (Returning error flag)
 *
 * ASSERTIONS:
 * PRE:  i = array length of args[] and args[] != NULL.
 * POST: returns legal args in a Params struct.
///////////////////////////////////*/
Params getArgs(int i, String args[], int* err);


#endif
