/*///////////////////////////////////////////
 * FILE:    Launcher.c
 * PURPOSE: Contains the main entry point into program,
 *          as well as methods for validating & reading
 *          the supplied arguments.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "Launcher.h"


/*///////////////////////////////////
 * FUNCTION: main
///////////////////////////////////*/
int main(int i, String args[])
{
   int err = OK;
   
   Params pars;
   FILE* fileRead;
   FILE* fileWrite;
   
   int direction = 0;
   CSVSelection* select;
   LinkedListNode* header;
   
   LinkedList list;
   list.metadata = NULL;
   list.head = NULL;
   list.tail = NULL;
   list.length = 0;
   
   /*Get and validate arguments*/
   pars = getArgs(i, args, &err);
   
   
   /*///// Import data from file ///////////////////////////////////*/
   if (err == OK)
   {
      fileRead = getFile(pars.fileInput, "r", &err);  /*Open input file for reading*/
      if (err == OK)
      {
         importCSVFile(fileRead, &list, &err);        /*Import data from file*/
         fclose(fileRead);                            /*Close file*/
      }
   }/*//////////////////////////////////////////////////////////////*/
   
   
   /*///// Validations /////////////////////////////////////////////*/
   if (err == OK) /*Get column to sort on*/
   {
      header = linkListExtraction(&list, list.head);           /*Extract header*/
      select = getCSVSelection((CSVLine*)header->data, &err);  /*Validate header and select column to sort on*/
      list.metadata = select;                                  /*Set selection*/
      
      if (err == OK)
      {
         validateCSVList(&list, ((CSVLine*)header->data)->size, &err); /*Validate List*/
      }
      
      if (err != OK)
      {
         linkListInsertion(&list, NULL, header, list.head);    /*Re-insert to be freed later*/
      }
   }/*//////////////////////////////////////////////////////////////*/
   
   
   /*///// Sort data ///////////////////////////////////////////////*/
   if (err == OK)
   {
      direction = getUserInput("Input 1 to sort accending, 2 to sort deccending:", 1, 2) -1;
      insertionSort(&list, &greaterThan_CSVLine, direction, &err);   /*Sort List*/
      linkListInsertion(&list, NULL, header, list.head);             /*Re-insert header at list head*/
   }/*//////////////////////////////////////////////////////////////*/
   
   
   /*///// Export data to file /////////////////////////////////////*/
   if (err == OK)
   {
      fileWrite = getFile(pars.fileOutput, "w", &err);   /*Open output file for writing*/
      if (err == OK)
      {
         exportCSVFile(fileWrite, &list, &err);          /*Export data to file*/
         fclose(fileWrite);                              /*Close file*/
      }
   }/*//////////////////////////////////////////////////////////////*/
   
   
   /*Free resources*/
   freeLinkedList(&list, &freeCSVLine, &freeCSVSelection);
   
   /*Error reporting*/
   printErr(err);
   return err;
}


/*///////////////////////////////////
 * FUNCTION: validateCSVList
 * REFER TO: Launcher.h
///////////////////////////////////*/
CSVSelection* getCSVSelection(CSVLine* header, int* err)
{
   CSVSelection* csvSel = NULL;
   int colIndex = 0;
   int datatype = 0;
   
   int colCt = 0;
   String colStr = NULL;
   String copyStr = NULL;
   String* attrbts = NULL;
   String typeName = NULL;
   
   if (header != NULL && header->size >= 1)
   {
      /*Select a column index*/
      colIndex = getUserInput("Input a column index (starting at 1) to sort on:", 1, header->size) - 1;
      colStr   = header->columns[colIndex];
      
      if (colStr != NULL)
      {
         /*Copy header column at selected index*/
         copyStr = (String)calloc(strlen(colStr) + 1, sizeof(char));
         strncpy(copyStr, colStr, strlen(colStr));
         
         if (copyStr != NULL)
         {
            attrbts = tokenizeString(copyStr, "()", &colCt);   /*Split into column name and datatype*/
            
            if (attrbts != NULL)
            {
               if (attrbts[1] != NULL)
               {
                  typeName = toUpperCase(attrbts[1]);          /*Standardized string*/

                  if (strcmp(INT_NAME, typeName) == 0)
                  {
                     datatype = INT_CODE;
                  }
                  else if (strcmp(STRING_NAME, typeName) == 0)
                  {
                     datatype = STRING_CODE;
                  }
                  else
                  {
                     *err += INVALID_DATATYPE;
                  }
               }
               else
               {
                  *err += INVALID_DATATYPE;
               }
            }
            else
            {
               *err += INVALID_COLUMN;
            }
         }
         else
         {
            *err += INVALID_COLUMN;
         }
      }
      else
      {
         *err += INVALID_COLUMN;
      }
   }
   else
   {
      *err += INVALID_HEADER;
   }
   
   if (*err == OK)
   {
      csvSel = newCSVSelection(colIndex, datatype);
   }
   
   free(typeName);
   free(attrbts);
   free(copyStr);
   
   return csvSel;
}


/*///////////////////////////////////
 * FUNCTION: validateCSVList
 * REFER TO: Launcher.h
///////////////////////////////////*/
void validateCSVList(LinkedList* list, int colCt, int* err)
{
   LinkedListNode* node = NULL;
   CSVLine* data = NULL;
   
   if (list != NULL)
   {
      node = list->head;
   }
   
   while (node != NULL && *err == OK)
   {
      data = (CSVLine*)node->data;
      
      if (data != NULL)             /*Check for NULL data*/
      {
         if (data->size == colCt)   /*Check column count*/
         {
            node = node->next;
         }
         else
         {
            *err += INVALID_COLUMN_COUNT;
         }
      }
      else
      {
         *err += INVALID_DATA_ROW;
      }
   }
}


/*///////////////////////////////////
 * FUNCTION: getUserInput
 * REFER TO: Launcher.h
///////////////////////////////////*/
int getUserInput(String prompt, int lBound, int uBound)
{
   int num = 0;
   String input = (String)calloc(4, sizeof(char));
   
   printf("> %s\n", prompt);
   scanf("%s", input);
   num = atoi(input);
   
   /*Since atoi returns 0 on error,
   0 should not be included within the bounds of accepted input*/
   
   while ((num < lBound) || (num > uBound))
   {
      printf("> Invalid! Input must be between %d and %d\n", lBound, uBound);
      scanf("%s", input);
      num = atoi(input);
   }
   
   free(input);
   return num;
}


/*///////////////////////////////////
 * FUNCTION: getArgs
 * REFER TO: Launcher.h
///////////////////////////////////*/
Params getArgs(int i, String args[], int* err)
{
   int j = 0;
   Params pars;
   pars.fileInput = NULL;
   pars.fileOutput = NULL;

   if (i == 5)
   {
      for (j = 1; j <= 3; j+=2)
      {
         if (strcmp("i", args[j]) == 0)
         {
            pars.fileInput = args[j+1];
         }
         else if (strcmp("o", args[j]) == 0)
         {
            pars.fileOutput = args[j+1];
         }
         else
         {
            *err += INVALID_ARGUMENT;
         }
      }
   }
   
   if ((pars.fileInput == NULL ) || (pars.fileOutput == NULL))
   {
      *err += INVALID_ARGUMENT;
   }
   
   return pars;
}

