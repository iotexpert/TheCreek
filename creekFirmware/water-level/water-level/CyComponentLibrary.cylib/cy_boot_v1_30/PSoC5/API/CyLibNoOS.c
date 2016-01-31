/*******************************************************************************
* File Name: CyLibNoOS.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides standard library support for an Operating System, we have no os
*   so the functions are mostly stubs.
*
*  Note:
*
*   
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if (defined(__GNUC__))


#include <cytypes.h>
#include <errno.h>

/* Add user support code between the following #START and #END tags */
/* `#START _exit`  */
/* `#END`  */

int errno;
char * __env[1] = { 0 }; 
char * * environ = __env; 


/* Defined in the linker descriptor file. */
extern unsigned long _heap;
static uint8 * CyHeap = (uint8 *) &_heap;
register char * stackPointer asm ("sp");


void * _sbrk(int size)
{
    void * memory; 
    uint8 * previous;

    /* Add user function code between the following #START and #END tags */
    /* `#START _sbrk`  */
  	/* `#END`  */

    previous = CyHeap; 

    if((uint32) previous + (uint32) size > (uint32) stackPointer)
    {
         errno = ENOMEM;
         memory = (void *) -1;
    }
    else
    {
        CyHeap += size;
        memory = (void *) previous;
    }

    return memory; 
}

int _wait(int * status)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _wait` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}

void _exit(int status)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _exit` */
  	/* `#END`  */


    /* Don't return. */
    while(42)
        ;
}


int _kill(int pid, int sig)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _kill` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


int _link(char * existing, char * new)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _link` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}

int _unlink(char * name)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _unlink` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


int _symlink(const char *path1, const char *path2)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _symlink` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


typedef int size_t;

int _readlink(const char * path,  char * buffer,  size_t bufferSize)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _readlink` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


int _open(char * file, int flags, int mode)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _open` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


int _close(int fileDescriptor)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _close` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}

int isatty(int file)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _isatty` */
  	/* `#END`  */

    errno = ENOSYS;
    return 0;
}

int _write(int file, char * pointer, int length)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _write` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


int _read(int file, char *ptr, int length)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _read` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


int _lseek(int file, int ptr, int dir)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _lseek` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


/* We don't support this functionality so the struct is only ... */
struct stat
{
	uint8 useless;
};

int _fstat(int fildes, struct stat * buffer)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _fstat` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}

int _stat(const char * file, struct stat *st)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _stat` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}

int _getpid(void)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _getpid` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}

int _fork(void)
{
    /* Add user function code between the following #START and #END tags */
    /* `#START _fork` */
  	/* `#END`  */

    errno = ENOSYS;
    return -1;
}


/* End of __GNUC__. */
#endif
