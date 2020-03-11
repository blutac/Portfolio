/*///////////////////////////////////////////
 * FILE:    Sorter.h
 * PURPOSE: Contains Sorter specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef SORTER_H
#define SORTER_H

#include "Common.h"

#include "LinkedList.h"
#include "CSVFile.h"


/*///////////////////////////////////
 * TYPEDEF: comparerFunc
 * PURPOSE: Represents a generic comparer function.
 * IMPORTS: obj1        (Value to compare)
 *          obj2        (Value to compare against)
 *          qualifier   (Additional data for comparing)
///////////////////////////////////*/
typedef int (*comparerFunc)(void* obj1, void* obj2, void* qualifier);


/*///////////////////////////////////
 * FUNCTION: insertionSort
 * PURPOSE:  Sorts a specified list in a specified mannar.
 * EXPORTS:  NONE
 * IMPORTS:  list          (LinkList to sort)
 *           comparerFunc  (Pointer to a comparer function)
 *           dirct         (direction to sort)
 *           err     (Returning Error flag)
 *
 * ASSERTIONS:
 * PRE:  list != NULL, comparerFunc is a relavant 'greaterThan' function,
 *       dirct is either 0 (for accending) or 1 for (deccending).
 * POST: list is sorted.
///////////////////////////////////*/
void insertionSort(LinkedList* list, comparerFunc, int dirct, int* err);


/*///////////////////////////////////
 * FUNCTION: greaterThan_CSVLine
 * PURPOSE:  Tests if one CSVLine is greater than another.
 * EXPORTS:  int     (int representing true(1)/false(0))
 * IMPORTS:  val1    (CSVLine to test if its greater)
 *           val2    (CSVLine to test against)
 *           qualifier (CSVSelection struct containting sorting data)
 *
 * ASSERTIONS:
 * PRE:  val1 and val2 are not NULL and have the same column format.
 *       qualifier != NULL and it's columnIdx is within bounds of val1->columns
 * POST: Returns 1 if val1 > val2, else returns 0 or
 *       -1 if an unknown datatype in encountered.
///////////////////////////////////*/
int greaterThan_CSVLine(void* obj1, void* obj2, void* qualifier);


/*///////////////////////////////////
 * FUNCTION: greaterThan_int
 * PURPOSE:  Tests if one int is greater than another.
 * EXPORTS:  int     (int representing true(1)/false(0))
 * IMPORTS:  val1    (int to test if its greater)
 *           val2    (int to test against)
 *
 * ASSERTIONS:
 * PRE:  val1 and val2 is not NULL.
 * POST: Returns 1 if val1 > val2, else returns 0.
///////////////////////////////////*/
int greaterThan_int(int val1, int val2);


#endif
