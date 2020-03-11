/*///////////////////////////////////////////
 * FILE:    Strings.h
 * PURPOSE: Contains Strings specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef STRINGS_H
#define STRINGS_H

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "Common.h"

/*Constant for uppercase conversions*/
#define CASE_OFFSET 32

/*///////////////////////////////////
 * TYPEDEF: String
 * PURPOSE: Represents the String datatype.
///////////////////////////////////*/
typedef char* String;


/*///////////////////////////////////
 * FUNCTION: tokenizeString
 * PURPOSE:  tokenizes a string.
 * EXPORTS:  String* (Array of tokens)
 * IMPORTS:  line    (String to tokenize)
 *           delim   (char array of delimiters to use)
 *           segCt   (Returning Array token length)
 *
 * ASSERTIONS:
 * PRE:  line != NULL, delim != empty, segCt is a valid pointer.
 * POST: line is tokenized and returned in array form.
///////////////////////////////////*/
String* tokenizeString(String line, String delim, int* segCt);


/*///////////////////////////////////
 * FUNCTION: concatToCSV
 * PURPOSE:  Concatinates an array of strings to one CSV string.
 * EXPORTS:  String  (CSV string)
 * IMPORTS:  line    (Array of strings to concatinate)
 *           segCt   (Array length of line)
 *
 * ASSERTIONS:
 * PRE:  line != NULL and segCt = array length of line.
 * POST: line is concatinated and returned in CSV format.
///////////////////////////////////*/
String concatToCSV(String line[], int segCt);


/*///////////////////////////////////
 * FUNCTION: toUpperCase
 * PURPOSE:  Converts import string to uppercase.
 * EXPORTS:  String  (Uppercase string copy)
 * IMPORTS:  value   (String to make Uppercase from)
 *
 * ASSERTIONS:
 * PRE:  value != NULL.
 * POST: Returns a copy of 'value' converted to uppercase.
///////////////////////////////////*/
String toUpperCase(String value);


/*///////////////////////////////////
 * FUNCTION: greaterThan_String
 * PURPOSE:  Tests if one String is greater than another.
 * EXPORTS:  int     (int representing true(1)/false(0))
 * IMPORTS:  val1    (String to test if its greater)
 *           val2    (String to test against)
 *
 * ASSERTIONS:
 * PRE:  val1 and val2 is not NULL.
 * POST: Returns 1 if val1 > val2, else returns 0.
 *
 * REMARKS: Comparison is case insensitive.
///////////////////////////////////*/
int greaterThan_String(String val1, String val2);

#endif
