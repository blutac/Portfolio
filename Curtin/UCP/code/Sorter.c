/*///////////////////////////////////////////
 * FILE:    Sorter.c
 * PURPOSE: Contains sorting related functions.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "Sorter.h"



/*///////////////////////////////////
 * FUNCTION: insertionSort
 * REFER TO: Sorter.h
///////////////////////////////////*/
void insertionSort(LinkedList* list, comparerFunc func, int dirct, int* err)
{   
   LinkedListNode* nodeNext   = NULL;     /*next node to sort*/
   LinkedListNode* nodeTarget = NULL;     /*node to sort*/
   LinkedListNode* nodeLHS    = NULL;     /*node Left of target*/
   LinkedListNode* nodeRHS    = NULL;     /*node Right of target*/
   
   if ((list != NULL) && (list->head != NULL)) /*check if list !empty*/
   {
      nodeTarget = list->head->next;
   }
   
   while (nodeTarget != NULL && *err == OK)
   {
      nodeLHS = nodeTarget->prev;         /*Set compare node to current node*/
      nodeRHS = nodeTarget->next;
      
      nodeTarget = linkListExtraction(list, nodeTarget);
      nodeNext = nodeRHS;
      
      /*if nodeLHS == NULL, we can't look any further left*/ /*==0 IS Small-to-Large !=0 IS Large-to-Small*/
      while ((nodeLHS != NULL) && ((*func)(nodeTarget->data, nodeLHS->data, list->metadata) == dirct))
      {
         /*move focus to prev nodes*/
         nodeRHS = nodeLHS;
         nodeLHS = nodeLHS->prev;
      }
      
      /*Extract and Insert target node*/
      if (nodeTarget != NULL)
      {
         if (linkListInsertion(list, nodeLHS, nodeTarget, nodeRHS) == NULL) /*insert current node before compare node*/
         {
            *err += FAILURE_SORT;
         }
      }
      else
      {
         *err += FAILURE_SORT;
      }
      
      nodeTarget = nodeNext;      /*Get next node*/
   }
   
}



/*///////////////////////////////////
 * FUNCTION: greaterThan_CSVLine
 * REFER TO: Sorter.h
///////////////////////////////////*/
int greaterThan_CSVLine(void* obj1, void* obj2, void* qualifier)
{
   int result = FALSE;
   int idxColumn = ((CSVSelection*)qualifier)->idxColumn;
   int datatype  = ((CSVSelection*)qualifier)->datatype;
   
   /*Get data to compare*/
   String val1 = ((CSVLine*)obj1)->columns[idxColumn];
   String val2 = ((CSVLine*)obj2)->columns[idxColumn];
   
   if (val1 == NULL)
   {
      result = FALSE;
   }
   else if (val2 == NULL)
   {
      result = TRUE;
   }
   else
   {
      switch (datatype)    /*Select method of compairing*/
      {
         case INT_CODE:
            result = greaterThan_int( atoi(val1), atoi(val2) );
            break;
         case STRING_CODE:
            result = greaterThan_String(val1, val2);
            break;
         default:
            result = -1;   /*Unknown datatype*/
      }
   }
   
   return result;
}



/*///////////////////////////////////
 * FUNCTION: greaterThan_int
 * REFER TO: Sorter.h
///////////////////////////////////*/
int greaterThan_int(int val1, int val2)
{
   int result = FALSE;
   if (val1 > val2)
   {
      result = TRUE;
   }
   return result;
}




