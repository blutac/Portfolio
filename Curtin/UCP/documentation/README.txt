STUDENT NAME: JASON GILBERT
STUDENT ID:   XXXXXXXX

///// FILE SUBMISSION LIST /////
> DeclarationOfOriginality.pdf = Legal stuff.
> README.txt      = This file.
> makefile        = Compiles program from source files.
> Common.h        = Contains common Directives and declarations.
> CSVFile.c       = Contains CSV data import/export functions.
> CSVFile.h       = Contains CSVFile specific declarations.
> ErrorHandler.c  = Contains error handling related methods.
> ErrorHandler.h  = Contains ErrorHandler specific declarations.
> FileIO.c        = Contains file accessing functions.
> FileIO.h        = Contains FileIO specific declarations.
> Launcher.c      = Contains the main entry point into program.
> Launcher.h      = Contains Launcher specific declarations.
> LinkedList.c    = Contains LinkedList accessors & mutators.
> LinkedList.h    = Contains LinkedList specific declarations.
> Sorter.c        = Contains sorting related functions.
> Sorter.h        = Contains Sorter specific declarations.
> Strings.c       = Contains common String methods.
> Strings.h       = Contains Strings specific declarations.


///// COMPLETION STATEMENT /////
Apart from the points listed below, all other features detailed in the Assignment brief
are functional.

- The program doesn't detect empty fields such as ",," or treat whitespace fields
  as being empty, and as such doesn't put these rows at the end of the file.

///// KNOWN BUGS & DEFECTS /////
- There is an 8 bytes (1 block) memory leak when the program deliberately exits
  after encoutering an invalid row starting with ','
- Multiple commas in succession, e.g: "value1,,,value2" are treated as one comma.
  Which is a side effect of using the strtok() function.
- The function "getUserInput()" can cause invalid writes if the input excceds
  the allocated buffer.
- The program may be a bit too forgiving in what it will let pass as a valid header column.
  allowing solely whitespace names or non whitespace and comma chars after the datatype.
  Although I intend to re-market this as a feature.


///// TEST CERTIFICATE /////
TEST LOCATION: 314-220
TEST COMPUTER SERVICE TAG: 8N56HZ1
TEST DATE: 21/10/2016


///// ASSUMPTIONS /////
1. Column header and datatype validations should only take place on the column
the user wants to sort on. The format of any other column header is irrelevant
to the sorting process and being so unnecessarily strict limits the programs
functionality and versatility, as well as slightly decreasing performance.

2. String comparison is case insensitive.

3. the Deliminator is ','

4. The sorting of strings as integers should be supported. Even though what you effectively end up doing
   is sorting the lines by the order you read them into memory, you can use this fact to reverse a list.
   And I consider this a feature, as well as in the spirit of the c philosophy.




