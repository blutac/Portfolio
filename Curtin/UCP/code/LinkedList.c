/*///////////////////////////////////////////
 * FILE:    LinkedList.c
 * PURPOSE: Contains LinkedList accessors & mutators.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "LinkedList.h"



/*///////////////////////////////////
 * FUNCTION: linkListInsertion
 * REFER TO: LinkedList.h
///////////////////////////////////*/
LinkedListNode* linkListInsertion(LinkedList* list,
                                  LinkedListNode* nodeLHS,
                                  LinkedListNode* nodeNew,
                                  LinkedListNode* nodeRHS)
{
   LinkedListNode* result = nodeNew;
   
   if (nodeNew != NULL && list != NULL)   /*Validations*/
   {
      if (nodeLHS != NULL)                /*Update LHS*/
      {
         nodeLHS->next = nodeNew;   /*Connect LHS to nodeNew*/
      }
      else
      {
         list->head = nodeNew;      /*Connect head to nodeNew*/
      }

      nodeNew->prev = nodeLHS;      /*Connect nodeNew to LHS*/
      nodeNew->next = nodeRHS;      /*Connect nodeNew to RHS*/

      if (nodeRHS != NULL)                /*Update RHS*/
      {
         nodeRHS->prev = nodeNew;   /*Connect RHS to nodeNew*/
      }
      else
      {
         list->tail = nodeNew;      /*Connect tail to nodeNew*/
      }

      list->length++;               /*Update list length*/
   }
   else
   {
      result = NULL;
   }
   
   return result;      /*return target note on success*/
}



/*///////////////////////////////////
 * FUNCTION: linkListExtraction
 * REFER TO: LinkedList.h
///////////////////////////////////*/
LinkedListNode* linkListExtraction(LinkedList* list, LinkedListNode* nodeOld)
{
   LinkedListNode* result = nodeOld;
   LinkedListNode* nodeLHS = NULL;
   LinkedListNode* nodeRHS = NULL;
   
   if (nodeOld != NULL && list != NULL)   /*Validations*/
   {
      nodeLHS = nodeOld->prev;
      nodeRHS = nodeOld->next;
   
      if (nodeLHS != NULL)                /*Update LHS*/
      {
         nodeLHS->next = nodeRHS;      /*Connect LHS to RHS*/
      }
      else
      {
         list->head = nodeRHS;         /*Connect head to RHS*/
      }

      nodeOld->prev = NULL;            /*Disconnect nodeOld*/
      nodeOld->next = NULL;

      if (nodeRHS != NULL)                /*Update RHS*/
      {
         nodeRHS->prev = nodeLHS;      /*Connect RHS to LHS*/
      }
      else
      {
         list->tail = nodeLHS;         /*Connect tail to LHS*/
      }
      
      list->length--;                  /*Update list length*/
   }
   else
   {
      result = NULL;
   }
   
   return result;      /*return target note on success*/
}



/*///////////////////////////////////
 * FUNCTION: getLinkedListNode
 * REFER TO: LinkedList.h
///////////////////////////////////*/
LinkedListNode* getLinkedListNode(LinkedList* list, int index)
{
   LinkedListNode* node = NULL;
   int i = 1;
   
   if ((list != NULL) && (index >= 1) && (index <= list->length))
   {
      node = list->head;
      while ((node != NULL) && (i < index))
      {
         i++;
         node = node->next;
      }
      
      if (i != index) /*shoulden't ever be reached as indexes < length aren't allowed*/
      {
         node = NULL;
      }
   }
   
   return node;
}


/*///////////////////////////////////
 * FUNCTION: newNode
 * REFER TO: LinkedList.h
///////////////////////////////////*/
LinkedListNode* newNode(void* data)
{
   LinkedListNode* node = (LinkedListNode*)malloc(sizeof(LinkedListNode));
   
   if (node != NULL)
   {
      node->data = data;
      node->next = NULL;
      node->prev = NULL;
   }
   
   return node;
}


/*///////////////////////////////////
 * FUNCTION: freeLinkedList
 * REFER TO: LinkedList.h
///////////////////////////////////*/
void freeLinkedList(LinkedList* obj, freeFunc freeData, freeFunc freeMData)
{
   LinkedListNode* nodeNext = NULL;
   LinkedListNode* nodeThis = NULL;
   
   if (obj != NULL)
   {
      nodeNext = obj->head;                  /*Get first node*/
      while (nodeNext != NULL)
      {
         nodeThis = nodeNext;                /*Get this node*/
         nodeNext = nodeNext->next;          /*Get next node*/

         (*freeData)(nodeThis->data);        /*Free this data*/
         free(nodeThis);                     /*Free this node*/
      }
      
      (*freeMData)(obj->metadata);           /*Free metadata*/
   }
}


