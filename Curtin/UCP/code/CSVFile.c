/*///////////////////////////////////////////
 * FILE:    CSVFile.c
 * PURPOSE: Contains CSV data import/export functions.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "CSVFile.h"


/*///////////////////////////////////
 * FUNCTION: importCSVFile
 * REFER TO: CSVFile.h
///////////////////////////////////*/
void importCSVFile(FILE* file, LinkedList* list, int* err)
{
   String line = NULL;              /*Read line string*/
   CSVLine* csvLine = NULL;         /*Read line object*/
   LinkedListNode* node = NULL;     /*Read line node*/
   
   if (file != NULL && list != NULL)
   {
      do
      {
         line = getNextLine(file);           /*Get next Line*/
         
         if (line != NULL)
         {
            if (line[0] != '\0')             /*Filter out empty lines*/
            {
               csvLine = newCSVLine(line);   /*Make new CSVLine*/
               
               if (csvLine != NULL)
               {
                  node = newNode(csvLine);   /*Make new LinkedListNode*/
                                             /*Insert new node at end of list*/
                  if (linkListInsertion(list, list->tail, node, NULL) == NULL)
                  {
                     *err += FAILURE_FILE_IMPORT;
                     freeCSVLine(csvLine);
                  }
               }
               else
               {
                  *err += FAILURE_FILE_IMPORT;
                  free(line);
               }
            }
            else
            {
               free(line);
            }
         }
         else
         {
            *err += FAILURE_FILE_IMPORT;
         }
      } while (feof(file) == FALSE && *err == OK);
   }
   else
   {
      *err += FAILURE_FILE_IMPORT;
   }
}


/*///////////////////////////////////
 * FUNCTION: exportCSVFile
 * REFER TO: CSVFile.h
///////////////////////////////////*/
void exportCSVFile(FILE* file, LinkedList* list, int* err)
{
   LinkedListNode* node = NULL;     /*Import node*/
   String csvLine = NULL;           /*Import node data*/
   
   if (list != NULL)
   {
      node = list->head;                   /*Get first node*/
   }
   else
   {
      *err += FAILURE_FILE_EXPORT;
   }
   
   while (node != NULL && list != NULL)
   {                                /*Get csv string from node*/
      csvLine = concatToCSV(
                  ((CSVLine*)node->data)->columns,
                  ((CSVLine*)node->data)->size);

      if (csvLine != NULL)
      {
         fputs(csvLine, file);      /*Write string to file*/
         free(csvLine);             /*Free string*/
         node = node->next;         /*Get next node*/
      }
      else
      {
         *err += FAILURE_FILE_EXPORT;
         free(csvLine);
      }
   }
}


/*///////////////////////////////////
 * FUNCTION: newCSVLine
 * REFER TO: CSVFile.h
///////////////////////////////////*/
CSVLine* newCSVLine(String line)
{
   int tokCt = 0;                   /*get line tokens*/
   String* tokLine = tokenizeString(line, ",", &tokCt);
   CSVLine* csvLine = (CSVLine*)malloc(sizeof(CSVLine));
   
   if ((csvLine != NULL) && (tokLine != NULL))
   {
      csvLine->size = tokCt;        /*Insert token count*/
      csvLine->columns = tokLine;   /*Insert array of tokens*/
   }
   else                             /*Free on failure*/
   {
      free(tokLine);
      free(csvLine);
   }
   
   return csvLine;
}


/*///////////////////////////////////
 * FUNCTION: newCSVSelection
 * REFER TO: CSVFile.h
///////////////////////////////////*/
CSVSelection* newCSVSelection(int colIdx, int datatype)
{
   CSVSelection* csvSel = (CSVSelection*)malloc(sizeof(CSVSelection));
   
   if (csvSel != NULL)
   {
      csvSel->idxColumn = colIdx;
      csvSel->datatype = datatype;
   }
   
   return csvSel;
}


/*///////////////////////////////////
 * FUNCTION: freeCSVLine
 * REFER TO: CSVFile.h
///////////////////////////////////*/
void freeCSVLine(void* obj)
{
   if (obj != NULL)
   {  
      if (((CSVLine*)obj)->columns != NULL)
      {
         free(((CSVLine*)obj)->columns[0]);
         free(((CSVLine*)obj)->columns);
      }
      
      free(obj);
   }
}


/*///////////////////////////////////
 * FUNCTION: freeCSVSelection
 * REFER TO: CSVFile.h
///////////////////////////////////*/
void freeCSVSelection(void* obj)
{
   if (obj != NULL)
   {
      free(((CSVSelection*)obj));
   }
}





