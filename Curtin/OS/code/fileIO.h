/*////////////////////////////////////////////
 * FILE:    fileIO.h
 * PURPOSE: Contains FileIO specific function.
 * REMARKS: 
 *
 * UNIT:    COMP2006
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
/*///////////////////////////////////////////
#ifndef FILEIO_H
#define FILEIO_H

#include <stdlib.h>
#include <stdio.h>


/*///////////////////////////////////
 * FUNCTION: getFile
 * EXPORTS:  FILE* (New FILE*)
 * IMPORTS:  path  (path of file to open)
 *           mode  (mode to open file in)
 *
 * PURPOSE: Returns a FILE* to the specified file else NULL if failed.
 * REMARKS: Ported from my UCP assignment.
 *          (you can't improve on perfection)
/*///////////////////////////////////
FILE* getFile(char* path, char* mode);


/*///////////////////////////////////
 * FUNCTION: readString
 * EXPORTS:  char* (New String)
 * IMPORTS:  f     (Opened FILE*)
 *           len   (Length of string to get)
 *
 * PURPOSE: Returns string of length len from file.
 * REMARKS: '\0' is appended at end of string,
 *          inclusive of the specified length.
/*///////////////////////////////////
char* readString(FILE* f, size_t len);

#endif







