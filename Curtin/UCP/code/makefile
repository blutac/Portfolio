#######Compiler Options#######
CC = gcc
CFLAGS = -Wall -pedantic -ansi -std=c89 -g
##############################

#######Compiler Targets#######
EXE         = csvSort
EXE_DEPENDS = $(P1_O) $(P2_O) $(P3_O) $(P4_O) $(P5_O) $(P6_O) $(P7_O)
EXE_COMPILE = $(CC) $(CFLAGS) $(EXE_DEPENDS) -o $(EXE)

P0   = Common
#P0_O = $(P0).o
#P0_C = $(P0).c
P0_H = $(P0).h
P0_DEPENDS = $(P0_C) $(P0_H)	$(P1_H) $(P2_H)
#P0_COMPILE = $(CC) $(CFLAGS) -c $(P0_C)

P1   = ErrorHandler
P1_O = $(P1).o
P1_C = $(P1).c
P1_H = $(P1).h
P1_DEPENDS = $(P1_C) $(P1_H)
P1_COMPILE = $(CC) $(CFLAGS) -c $(P1_C)

P2   = Strings
P2_O = $(P2).o
P2_C = $(P2).c
P2_H = $(P2).h
P2_DEPENDS = $(P2_C) $(P2_H)	$(P0_H) $(P1_H)
P2_COMPILE = $(CC) $(CFLAGS) -c $(P2_C)

P3   = LinkedList
P3_O = $(P3).o
P3_C = $(P3).c
P3_H = $(P3).h
P3_DEPENDS = $(P3_C) $(P3_H)    $(P0_H) $(P1_H) $(P2_H)
P3_COMPILE = $(CC) $(CFLAGS) -c $(P3_C)

P4   = FileIO
P4_O = $(P4).o
P4_C = $(P4).c
P4_H = $(P4).h
P4_DEPENDS = $(P4_C) $(P4_H)    $(P0_H) $(P1_H) $(P2_H)
P4_COMPILE = $(CC) $(CFLAGS) -c $(P4_C)

P5   = CSVFile
P5_O = $(P5).o
P5_C = $(P5).c
P5_H = $(P5).h
P5_DEPENDS = $(P5_C) $(P5_H)    $(P0_H) $(P1_H) $(P2_H)   $(P4_H) $(P3_H)
P5_COMPILE = $(CC) $(CFLAGS) -c $(P5_C)

P6   = Sorter
P6_O = $(P6).o
P6_C = $(P6).c
P6_H = $(P6).h
P6_DEPENDS = $(P6_C) $(P6_H)    $(P0_H) $(P1_H) $(P2_H)   $(P3_H) $(P5_H) $(P4_H)
P6_COMPILE = $(CC) $(CFLAGS) -c $(P6_C)

P7   = Launcher
P7_O = $(P7).o
P7_C = $(P7).c
P7_H = $(P7).h
P7_DEPENDS = $(P7_C) $(P7_H)    $(P0_H) $(P1_H) $(P2_H)   $(P3_H) $(P4_H) $(P5_H) $(P6_H)
P7_COMPILE = $(CC) $(CFLAGS) -c $(P7_C)
##############################



#######Compiler Methods########
#Make main program
$(EXE) : $(EXE_DEPENDS)
	$(EXE_COMPILE)

#Make P0 OBJECT
#$(P0_O) : $(P0_DEPENDS)
#	$(P0_COMPILE)

#Make P1 OBJECT
$(P1_O) : $(P1_DEPENDS)
	$(P1_COMPILE)

#Make P2 OBJECT
$(P2_O) : $(P2_DEPENDS)
	$(P2_COMPILE)

#Make P3 OBJECT
$(P3_O) : $(P3_DEPENDS)
	$(P3_COMPILE)

#Make P4 OBJECT
$(P4_O) : $(P4_DEPENDS)
	$(P4_COMPILE)

#Make P5 OBJECT
$(P5_O) : $(P5_DEPENDS)
	$(P5_COMPILE)
	
#Make P6 OBJECT
$(P6_O) : $(P6_DEPENDS)
	$(P6_COMPILE)
	
#Make P7 OBJECT
$(P7_O) : $(P7_DEPENDS)
	$(P7_COMPILE)
###############################


#######Secondary Methods########
clean:
	rm -f $(EXE) $(EXE_DEPENDS)

remk:
	make clean
	make
################################

