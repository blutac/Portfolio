#######Compiler Options########
CC = gcc
CFLAGS = -Wall -pedantic -ansi -std=c99 -g -lm -pthread
TFLAG = -DTHREADED
###############################

#######Compiler Targets########
EXE         = mssv
EXE_ROOTFILE= mssv.c
DEPENDENCIES = mssv.c mssv.h fileIO.c fileIO.h grid.c grid.h resources.c resources.h processGroup.c processGroup.h threadGroup.c threadGroup.h
COMPILE_STRING = $(CC) $(EXE_ROOTFILE) $(CFLAGS) -o $(EXE)
###############################

#########Run Parameters########
DEBUGGER = valgrind
DBFLAGS = --leak-check=full
TEST_FILE = ./data
DELAY = 5
###############################

#######Compiler Methods########
#Default Program (Forked)
$(EXE) : $(DEPENDENCIES)
	$(COMPILE_STRING)

#Forked Program
forked :
	$(COMPILE_STRING)

#Threaded Program
threaded :
	$(COMPILE_STRING) $(TFLAG)
###############################


#######Secondary Methods########
clean:
	rm -f $(EXE)

remake_f:
	make clean
	make forked

remake_t:
	make clean
	make threaded

run:
	./$(EXE) $(TEST_FILE) $(DELAY)

dbrun:
	$(DEBUGGER) ./$(EXE) $(TEST_FILE) $(DELAY)

dbfrun:
	$(DEBUGGER) $(DBFLAGS) ./$(EXE) $(TEST_FILE) $(DELAY)

remake_f_run:
	make remake_f
	make run

remake_f_dbrun:
	make remake_f
	make dbrun

remake_f_dbfrun:
	make remake_f
	make dbfrun

remake_t_run:
	make remake_t
	make run

remake_t_dbrun:
	make remake_t
	make dbrun

remake_t_dbfrun:
	make remake_t
	make dbfrun
################################