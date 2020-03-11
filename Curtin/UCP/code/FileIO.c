/*///////////////////////////////////////////
 * FILE:    FileIO.c
 * PURPOSE: Contains file accessing functions.
 * REMARKS: 
 *
 * UNIT:    COMP1000
 * AUTHOR:  Jason Gilbert
 * STUDENT ID: XXXXXXXX
///////////////////////////////////////////*/
#include "FileIO.h"


/*///////////////////////////////////
 * FUNCTION: getFile
 * REFER TO: FileIO.h
///////////////////////////////////*/
FILE* getFile(String path, String mode, int* err)
{
   FILE* file = fopen(path, mode);     /*Get file pointer*/
   
   if (file == NULL)                   /*Test for errors*/
   {
      perror(path); 
      *err += FAILURE_FILE_OPEN;
   }
   else if (ferror(file))
   {
      perror(path);
      fclose(file);
      *err += FAILURE_FILE_ACCESS;
   }

   return file;
}


/*///////////////////////////////////
 * FUNCTION: getNextLine
 * REFER TO: FileIO.h
///////////////////////////////////*/
String getNextLine(FILE* file)
{
   String line = NULL;  /*Export string*/
   int len = 0;         /*Export string length*/
   int i   = 0;         /*Current element*/
   int c   = 0;         /*Current character*/
   
   line = (String)malloc(sizeof(char));
   
   /*loop till end of line/file or error is reached*/
   while (c != '\n' && c != EOF && line != NULL)
   {
      if (i >= (len-2))                /*Increase line size*/
      {
         len += CHUNK;
         line = (String)realloc(line, sizeof(char) * len);
      }
      
      if (line != NULL)
      {
         c = fgetc(file);              /*Get next char*/
         
         /*add next char if not end*/
         if (c != '\n' && c != EOF)
         {
            line[i] = (char)c;         /*Add char*/
            i++;
         }
      }
   }
   
   if (line != NULL)
   {
      line[i] = '\0';   /*Cap string*/
   }
   return line;
}
















