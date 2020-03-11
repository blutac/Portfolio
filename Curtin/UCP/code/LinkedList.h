/*///////////////////////////////////////////
 * FILE:    LinkedList.h
 * PURPOSE: Contains LinkedList specific declarations.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#ifndef LINKEDLIST_H
#define LINKEDLIST_H

#include "Common.h"

/*///////////////////////////////////
 * STRUCT:  LinkedListNode
 * PURPOSE: Represents an element in a linked list.
 * IMPORTS: data  (Pointer to some data)
 *          next  (Pointer to next Node in list)
 *          prev  (Pointer to previous Node in list)
///////////////////////////////////*/
typedef struct Node
{
   void* data;
   struct Node* next;
   struct Node* prev;
} LinkedListNode;


/*///////////////////////////////////
 * STRUCT:  LinkedList
 * PURPOSE: A doubly-linked, double-ended Linked-List abstract data type.
 * IMPORTS: metaData (Pointer to some auxiliary data)
 *          head     (Pointer to first Node in List)
 *          tail     (Pointer to last Node in list)
 *          length   (number of Nodes in list)
 *
 * REMARKS: within this program, metadata is being
 *          used to hold data about what column to sort.
///////////////////////////////////*/
typedef struct
{
   void* metadata;
   LinkedListNode* head;
   LinkedListNode* tail;
   int length;
} LinkedList;


/*///////////////////////////////////
 * FUNCTION: linkListInsertion
 * PURPOSE:  Inserts a LinkedListNode within a LinkedList
 * EXPORTS:  LinkedListNode*  (Pointer to inserted node or NULL on failure)
 * IMPORTS:  list    (LinkList to insert into)
 *           nodeLHS (place to insert after)
 *           nodeNew (node to insert)
 *           nodeRHS (place to insert before)
 *
 * ASSERTIONS:
 * PRE:  nodeLHS and nodeRHS are left and right of each other and nodeNew != NULL.
 * POST: nodeNew with be inserted between nodeLHS and nodeRHS, respectively.
 *
 * REMARKS: if nodeLHS or nodeRHS is NULL, then nodeNew will be inserted at
 *          the start or end of list, respectively. if both nodes are NULL,
 *          then you're effectively inserting into an empty list.
///////////////////////////////////*/
LinkedListNode* linkListInsertion(LinkedList* list,
                                  LinkedListNode* nodeLHS,
                                  LinkedListNode* nodeNew,
                                  LinkedListNode* nodeRHS);


/*///////////////////////////////////
 * FUNCTION: linkListExtraction
 * PURPOSE:  Extracts a LinkedListNode from a LinkedList
 * EXPORTS:  LinkedListNode*  (Pointer to extracted node or NULL on failure)
 * IMPORTS:  list    (LinkList to extract from)
 *           nodeOld (node to extract)
 *
 * ASSERTIONS:
 * PRE:  list contains nodeOld and nodeOld != NULL.
 * POST: nodeOld is extracted from list and list is updated.
///////////////////////////////////*/
LinkedListNode* linkListExtraction(LinkedList* list, LinkedListNode* nodeOld);


/*///////////////////////////////////
 * FUNCTION: getLinkedListNode
 * PURPOSE:  Returns the nth element of a LinkedList.
 * EXPORTS:  LinkedListNode*  (Pointer to found node or NULL on failure)
 * IMPORTS:  list    (LinkList to search in)
 *           index   (index to find)
 *
 * ASSERTIONS:
 * PRE:  index is between 1 and list length and list is not empty.
 * POST: Returns a LinkedListNode* to found node.
 *
 * REMARKS: index is base 1 on the basis that 0 represents the LinkedList itself.
 *          This function is no longer used.
///////////////////////////////////*/
LinkedListNode* getLinkedListNode(LinkedList* list, int index);


/*///////////////////////////////////
 * FUNCTION: newNode
 * PURPOSE:  Returns a LinkedListNode* struct initialised with some data.
 * EXPORTS:  LinkedListNode*  (Pointer to new LinkedListNode)
 * IMPORTS:  data    (Pointer to data to attach)
 *           err     (Returning error flag)
 *
 * ASSERTIONS:
 * PRE:  None.
 * POST: Returns an unlinked LinkedListNode* initialised with data.
///////////////////////////////////*/
LinkedListNode* newNode(void* data);


/*///////////////////////////////////
 * TYPEDEF: freeFunc
 * PURPOSE: Represents a generic free function.
 * IMPORTS: data        (Data to free)
///////////////////////////////////*/
typedef void(*freeFunc)(void* data);


/*///////////////////////////////////
 * FUNCTION: freeLinkedList
 * PURPOSE:  Drills down a specified LinkedList to free all its allocated memory.
 * EXPORTS:  NONE
 * IMPORTS:  obj        (Pointer to LinkedList to free)
 *           freeData   (Function pointer to free node data)
 *           freeMData  (Fucntion pointer to free metadata)
 *
 * ASSERTIONS:
 * PRE:  freeData and freeMData point to functions that are compatible with
 *       the data that they are freeing.
 * POST: All memory allocated associated with the LinkedList is freed.
///////////////////////////////////*/
void freeLinkedList(LinkedList* obj, freeFunc freeData, freeFunc freeMData);


#endif