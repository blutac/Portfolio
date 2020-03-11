/*///////////////////////////////////////////
 * FILE:    FileIO.h
 * PURPOSE: Contains FileIO specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef FILEIO_H
#define FILEIO_H

#include "Common.h"

/*Buffer size for function: getNextLine*/
#define CHUNK (8)


/*///////////////////////////////////
 * FUNCTION: getFile
 * PURPOSE:  Opens a specified file in a specified mode
 * EXPORTS:  FILE*   (Pointer to file)
 * IMPORTS:  file    (path of file to open)
 *           mode    (mode to open file in)
 *           err     (Returning Error flag)
 *
 * ASSERTIONS:
 * PRE:  Apart from 'err', the preconditions are the same as the fopen function in <stdio.h>.
 * POST: Returns a FILE pointer to the specified file else NULL if failed.
///////////////////////////////////*/
FILE* getFile(String file, String mode, int* err);


/*///////////////////////////////////
 * FUNCTION: getNextLine
 * PURPOSE:  get the next line within a text file.
 * EXPORTS:  String  (The file line gotten)
 * IMPORTS:  file    (Pointer to the file to read from)
 *
 * ASSERTIONS:
 * PRE:  'file' is a valid FILE* opened for reading and positioned at the start of a line.
 * POST: Returns file line as String.
///////////////////////////////////*/
String getNextLine(FILE* file);


#endif
