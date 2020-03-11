/*////////////////////////////////////////////
 * FILE:        grid.c
 * REFER TO:    grid.h
 *
 * UNIT:        COMP2006
 * AUTHOR:      Jason Gilbert
 * STUDENT ID:  XXXXXXXX
/*///////////////////////////////////////////
#include "grid.h"


/*///////////////////////////////////
 * FUNCTION: loadGrid
 * REFER TO: grid.h
/*///////////////////////////////////
void loadGrid(SdkGrid* sdk, char* data)
{
    int i = 0;
    for (int y = 0; y < GLEN; y++)
    {
        for(int x = 0; x < GLEN; x++)
        {
            sdk->grid[x][y] = atoi(&data[i]); // Store data as int
            i += 2; // get next digit (skips the whitespace delimiter)
        }
    }
}


/*///////////////////////////////////
 * FUNCTION: getSelection
 * REFER TO: grid.h
/*///////////////////////////////////
Selection getSelection(Shape shape, int zone)
{
    Selection sel;// = malloc(sizeof(Selection));
    sel.shape = shape;
    sel.zone = zone;
    
    return sel;
}


/*///////////////////////////////////
 * FUNCTION: getTaskSet
 * REFER TO: grid.h
/*///////////////////////////////////
TaskSet* getTaskSet(int count, int delay, int id)
{
    TaskSet* selSet = malloc(sizeof(TaskSet));
    selSet->count = count;
    selSet->delay = delay;
    selSet->id = id;
    
    return selSet;
}













/*///////////////////////////////////
 * FUNCTION: getSubgrid
 * REFER TO: grid.h
/*///////////////////////////////////
Subgrid* getSubgrid(SdkGrid* sdk, Selection sel)
{
    Subgrid* subgrid = malloc(sizeof(Subgrid));
    subgrid->sel = sel; // Record location of this subgrid
    
    switch (sel.shape)
    {
        case ROW: // illiterate over specified ROW
            for (int i = 0; i < GLEN; i++)
                subgrid->items[i] = sdk->grid[i][sel.zone];
        break;
        
        case COL: // illiterate over specified COLUMN
            for (int i = 0; i < GLEN; i++)
                subgrid->items[i] = sdk->grid[sel.zone][i];
        break;
        
        case BOX: // illiterate over specified BOX
        {
            int i = 0;
            int xStart = (sel.zone % SLEN) * SLEN;
            int yStart = (sel.zone / SLEN) * SLEN;
            
            for (int y = yStart; y < yStart + SLEN; y++)
            {
                for (int x = xStart; x < xStart + SLEN; x++)
                {
                    subgrid->items[i] = sdk->grid[x][y];
                    i++;
                }
            }
        }
        break;
    }
    return subgrid;
}

/*///////////////////////////////////
 * FUNCTION: checkSubgrid
 * REFER TO: grid.h
/*///////////////////////////////////
bool checkSubgrid(Subgrid* subgrid)
{
    bool valid = true;
    int* map = calloc(GLEN, sizeof(int));
    int j, i = 0;
    
    // Checks by (1 to 1) mapping each number in the subgrid to an array index
    while (valid && i < GLEN)
    {
        j = subgrid->items[i];  // get first item
        
        if (j < 1 || j > GLEN)
            valid = false;      // out of range
        else if (map[j - 1] == subgrid->items[i])
            valid = false;      // already mapped
        else
            map[j - 1] = subgrid->items[i];  // map to array
        
        i++;
    }
	
    free(map);
    return valid;
}

/*///////////////////////////////////
 * FUNCTION: printSubgrid
 * REFER TO: grid.h
/*///////////////////////////////////
void printSubgrid(Subgrid* subgrid)
{
    char delim = ' ';
    if (subgrid->sel.shape == COL)
        delim = '\n';
    
    for (int i = 0; i < GLEN; i++)
    {
        // (Shape is BOX) && (element is located on the edge)
        if ((subgrid->sel.shape == BOX) && (((i+1) % SLEN) == 0))
            delim = '\n';
        else if (subgrid->sel.shape != COL)
            delim = ' ';
        
        printf("%d%c", subgrid->items[i], delim);
    }
    printf("\n");
}

/*///////////////////////////////////
 * FUNCTION: printGrid
 * REFER TO: grid.h
/*///////////////////////////////////
void printGrid(SdkGrid* sdk)
{
    for (int y = 0; y < GLEN; y++)
    {
        printf("\t");
        for(int x = 0; x < GLEN; x++)
        {
            printf("%d ", sdk->grid[x][y]);
        }
        printf("\n");
    }
}

/*///////////////////////////////////
 * FUNCTION: itos
 * REFER TO: grid.h
/*///////////////////////////////////
char* itos(int n)
{
    int len = (int)log10(n) + 1;
    if (len < 0) len++; // add space for '-' char
    char* str = calloc(len + 1, sizeof(char));
    sprintf(str, "%d", n);
    
    return str;
}















/*///////////////////////////////////
 * FUNCTION: genLogString
 * REFER TO: grid.h
/*///////////////////////////////////
char* genLogString(int taskID, Subgrid** subgrid, int count)
{
    char* string = calloc(2000, sizeof(char));

    // Add TaskID to string //
    char* a = itos(taskID); // convert taskID to string
    strncat(string, "process ID-", strlen("process ID-") + 1);
    strncat(string, a, strlen(a) + 1);
    strncat(string, ": ", 3);
    free(a);
    
    // Add shape to string //
    switch (subgrid[0]->sel.shape)
    {
        case ROW:
            strncat(string, "row ", 5);
        break;
        case COL:
            strncat(string, "columns ", 9);
        break;
        case BOX:
            strncat(string, "sub-grids ", 11);
        break;
    }
    
    // Add zones to string //
    for (int i = 0; i < count; i++)
    {
        // append zones (in base 1)//
        char* z = itos(subgrid[i]->sel.zone + 1);
        strncat(string, z, strlen(z) + 1);
        free(z);
        
        if (i < count - 1)              // if not end
            strncat(string, ", ", 3);   // append delim
    }

    // End String //
    strncat(string, " are invalid\n", 14);
    
    return string;
}