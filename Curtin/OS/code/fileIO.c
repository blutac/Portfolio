/*////////////////////////////////////////////
 * FILE:        fileIO.c
 * REFER TO:    fileIO.h
 *
 * UNIT:        COMP2006
 * AUTHOR:      Jason Gilbert
 * STUDENT ID:  XXXXXXXX
/*///////////////////////////////////////////
#include "fileIO.h"


/*///////////////////////////////////
 * FUNCTION: readString
 * REFER TO: fileIO.h
/*///////////////////////////////////
FILE* getFile(char* path, char* mode)
{
    FILE* f = fopen(path, mode);    // Get file pointer

    if (f == NULL)                  // catch errors
        perror(path);
    else if (ferror(f))
    {
        perror(path);
        fclose(f);
    }

    return f;
}


/*///////////////////////////////////
 * FUNCTION: readString
 * REFER TO: fileIO.h
/*///////////////////////////////////
char* readString(FILE* f, size_t len)
{
    char* data = calloc(len, sizeof(char)); // Allocate
    fread(data, sizeof(char), len, f);      // Read
    data[len - 1] = '\0';                   // Cap off string
    
    return data;
}






