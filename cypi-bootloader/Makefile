#
# Makefile:
#	wiringPi - Wiring Compatable library for the Raspberry Pi
#	https://projects.drogon.net/wiring-pi
#
#	Copyright (c) 2012 Gordon Henderson
#################################################################################
# This file is part of wiringPi:
#	Wiring Compatable library for the Raspberry Pi
#
#    wiringPi is free software: you can redistribute it and/or modify
#    it under the terms of the GNU Lesser General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    wiringPi is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU Lesser General Public License for more details.
#
#    You should have received a copy of the GNU Lesser General Public License
#    along with wiringPi.  If not, see <http://www.gnu.org/licenses/>.
#################################################################################


#DEBUG	= -g -O0
DEBUG	= -g -O2

CC	= gcc	
INCLUDE	= -I/home/pi/cypi-server 
#CFLAGS	= $(DEBUG) -Wall $(INCLUDE) -Winline -pipe
CFLAGS	= 

#LDFLAGS	= -L/usr/local/lib 
#LIBS    = -lpthread -lm 
LDFLAGS	= 
LIBS    = 

# May not need to  alter anything below this line
###############################################################################

SRC	=	cypibl.c bcm2835.c bcm2835_i2cbb.c cybtldr_api.c cybtldr_api2.c cybtldr_command.c cybtldr_parse.c

OBJ	=	$(SRC:.c=.o)

all:		cypibl

cypibl:	$(OBJ)
	@echo [Link]
	@$(CC) -o $@ $(OBJ) $(LDFLAGS) $(LIBS)

.c.o:
	@echo [Compile] $<
	@$(CC) -c $(CFLAGS) $< -o $@

.PHONEY:	clean
clean:
	@echo "[Clean]"
	@rm -f $(OBJ) gpio *~ core tags *.bak

.PHONEY:	tags
tags:	$(SRC)
	@echo [ctags]
	@ctags $(SRC)

.PHONEY:	depend
depend:
	makedepend -Y $(SRC)

# DO NOT DELETE


