/*///////////////////////////////////////////
 * FILE:    Strings.c
 * PURPOSE: Contains common String methods.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "Strings.h"


/*///////////////////////////////////
 * FUNCTION: tokenizeString
 * REFER TO: Strings.h
///////////////////////////////////*/
String* tokenizeString(String line, String delim, int* segCt)
{
   String* array = NULL;
   String  token = NULL;
   
   if (line != NULL)
   {
      array = (String*)malloc(sizeof(String));
      token = strtok(line, delim);           /*Get first token*/
      *segCt = 0;                            /*Set segCt*/
      
      while (token != NULL && array != NULL)
      {
         (*segCt)++;                         /*Increase memory*/
         array = (String*)realloc(array, (sizeof(String) * (*segCt + 1)));

         if (array != NULL)
         {
            array[(*segCt) - 1] = token;     /*Add token*/
            array[(*segCt)]     = '\0';      /*Add null string at end*/
            token = strtok(NULL, delim);     /*Get next token*/
         }
      }
   }
   
   return array;
}



/*///////////////////////////////////
 * FUNCTION: concatToCSV
 * REFER TO: Strings.h
///////////////////////////////////*/
String concatToCSV(String line[], int segCt)
{
   String csvLine = NULL;
   int csvLen = 1;                              /*+1 for '\0'*/
   int i = 0;
   
   /*Calculate array memory*/
   for (i = 0; i < segCt; i++)
   {
      csvLen += strlen(line[i]) + 1;            /*+1 for ',' or '\n'*/
   }                                            /*Allocate memory for export string*/
   csvLine = (String)calloc(csvLen, sizeof(char));
   
   if (csvLine != NULL)                         /*Add strings to export string*/
   {
      strncat(csvLine, line[0], strlen(line[0]));     /*Add first string*/
      
      for (i = 1; i < segCt; i++)
      {
         strncat(csvLine, ",", 1);                    /*Add delim*/
         strncat(csvLine, line[i], strlen(line[i]));  /*Add next string*/
      }
      strncat(csvLine, "\n\0", 2);                    /*Add new line*/
   }
   
   return csvLine;
}



/*///////////////////////////////////
 * FUNCTION: toUpperCase
 * REFER TO: Strings.h
///////////////////////////////////*/
String toUpperCase(String value)
{
   int i = 0;
   int len = strlen(value);                     /*Allocate memory for export string*/
   String str = (String)calloc(len+1, sizeof(char));
   
   if (str != NULL)
   {
      for (i = 0; i < len; i++)
      {
         if (value[i] >= 'a' && value[i] <= 'z')
         {
            str[i] = value[i] - CASE_OFFSET;    /*Convert to uppercase before insert*/
         }
         else
         {
            str[i] = value[i];                  /*Insert char*/
         }
      }
   }
   
   return str;
}



/*///////////////////////////////////
 * FUNCTION: greaterThan_String
 * REFER TO: Strings.h
///////////////////////////////////*/
int greaterThan_String(String val1, String val2)
{
   int result = FALSE;
   String val01 = toUpperCase(val1);    /*Convert to uppercase*/
   String val02 = toUpperCase(val2);    /*to standarize Strings*/
   
   if (strcmp(val01, val02) > 0)
   {
      result = TRUE;
   }
   
   free(val01);
   free(val02);
   return result;
}

