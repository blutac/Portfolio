///////////////////////
         Compile
///////////////////////
To compile the child process implemented program
(i.e. PART A of the assignment), use the command:

	make forked


To compile the thread implemented program
(i.e. PART B of the assignment), use the command:

	make threaded


To remove the mssv program, use the command:

	make clean


///////////////////////
          Run
///////////////////////
To run the program use:

	./mssv file maxdelay

where;
	file is sudoku grid to read.
	maxdelay is the maximum delay of a child task

NOTE:
mssv must be run in the same directory as the 'mssv.c' file
as it uses it to seed the ftok() function.