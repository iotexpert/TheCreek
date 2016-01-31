/*******************************************************************************
* FILENAME: cytypes.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*  CyTypes provides register access macros and approved types for use in 
*  firmware.
* 
*  Note:
*  Due to endiannesses of the hardware and some compilers, the register
*  access macros for big endian compilers use some library calls to arrange 
*  data the correct way.
*   
*  Register Access macros and functions perform their operations on an 
*  input of type pointer to void.  The arguments passed to it should be 
*  pointers to the type associated with the register size. 
*  (i.e. a "uint8 *" shouldn't be passed to obtain a 16-bit register value)
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(CYTYPES_H)
#define CYTYPES_H




/* Acceptable types from MISRA-C specifying signedness and size.*/
typedef unsigned char           char8;

typedef unsigned char           uint8;
typedef unsigned short          uint16;
typedef unsigned long           uint32;

typedef signed char             int8;
typedef signed short            int16;
typedef signed long             int32;


/* Address of an ISR. */
typedef void (* cyisraddress)(void); 


/* Get 8 bits of a 16 bit value. */
#define LO8(x)                  ((uint8)  ((x) & 0xFFu))
#define HI8(x)                  ((uint8)  ((x) >> 8))

/* Get 16 bits of a 32 bit value. */
#define LO16(x)                 ((uint16) ((x) & 0xFFFFu))
#define HI16(x)                 ((uint16) ((uint32)(x) >> 16))


/*
*   Defines the standard return values used PSoC content. A function is
*   not limited to these return values but can use them when returning standard
*   error values.
*
*   Return values can be overloaded if documented in the function header.
*
*   On the 8051 a function can use a larger return type but still use the defined
*   return codes.
*
*   0 is successful, all other values indicate some form of failure.
*
*   1 - 0x7F, Standard defined values.
*   0x80 - ... , User or content defined values.
*/

/* Completed successfully. */
#define CYRET_SUCCESS                   0x00u

/* Failed, the exact cause is unknown. */
#define CYRET_UNKNOWN                   ((cystatus) -1)

/* One or more parameters to the function were invalid. */
#define CYRET_BAD_PARAM                 0x01u

/* One or more parameters to the function were invalid. */
#define CYRET_INVALID_OBJECT            0x02u

/* There was a memory problem. */
#define CYRET_MEMORY                    0x03u

/* The object was locked, already in use. */
#define CYRET_LOCKED                    0x04u

/* No more objects are available. */
#define CYRET_EMPTY                     0x05u

/* The CRC, Checksum, FEC or packet format was bad. */
#define CYRET_BAD_DATA                  0x06u

/* The operation was started correctly, but may not be completed. */
#define CYRET_STARTED                   0x07u

/* The operation has finished. */
#define CYRET_FINISHED                  0x08u

/* The operation was canceled. */
#define CYRET_CANCELED                  0x09u

/* The operation timed out. */
#define CYRET_TIMEOUT                   0x10u

/* The operation was not setup or in a proper state. */
#define CYRET_INVALID_STATE             0x11u







/* Include for the KEIL C51 compiler
*  KEIL for the 8051 is a Big Endian compiler This causes problems as the
*  on chip registers are little endian.  Byte swapping for two and four byte
*  registers is implemented in the functions below.  This will require
*  conditional compilation of function prototypes in code.
*/
#if (defined(__C51__))


/* To allow code to be 8051-ARM agnostic. */
#define XDATA xdata

/* Keil wants Interrupt functions to include 'interrupt 0' */
#define CY_ISR(FuncName)  void FuncName (void) interrupt 0

/* Keil wants Interrupt function prototypes to not include 'interrupt 0' */
#define CY_ISR_PROTO(FuncName) void FuncName (void)

/* 8051 naturarly returns an 8 bit value. */
typedef unsigned char cystatus;


/* Prototypes for handling little endian registers with big endian compiler. */
extern uint16 CyGetSwapReg16(uint16 xdata * addr);
extern void CySetSwapReg16(uint16 xdata * addr, uint16 value);
extern uint32 CyGetSwapReg24(uint32 xdata * addr);
extern void CySetSwapReg24(uint32 xdata * addr, uint32 value);
extern uint32 CyGetSwapReg32(uint32 xdata * addr);
extern void CySetSwapReg32(uint32 xdata * addr, uint32 value);

/* Access macros for 8, 16, 24 and 32-bit registers, IN THE FIRST 64K OF XDATA */
#define CY_GET_REG8(addr)           (*((volatile uint8 xdata *)(addr)))
#define CY_SET_REG8(addr, value)    (*((volatile uint8 xdata *)(addr))  = (uint8)(value))

#define CY_GET_REG16(addr)          CyGetSwapReg16(addr)
#define CY_SET_REG16(addr, value)   CySetSwapReg16(addr, value)

#define CY_GET_REG24(addr)          CyGetSwapReg24(addr)
#define CY_SET_REG24(addr, value)   CySetSwapReg24(addr,value)

#define CY_GET_REG32(addr)          CyGetSwapReg32(addr)
#define CY_SET_REG32(addr, value)   CySetSwapReg32(addr, value)



/* Prototypes for absolute memory address functions (cymem.a51) */
/* These functions also swap endian */
extern uint8 cyread8(const void far *addr);
extern void cywrite8(void far *addr, uint8 value);
extern uint16 cyread16(const void far *addr);
extern void cywrite16(void far *addr, uint16 value);
extern uint32 cyread24(const void far *addr);
extern void cywrite24(void far *addr, uint32 value);
extern uint32 cyread32(const void far *addr);
extern void cywrite32(void far *addr, uint32 value);

/* Access macros for 8, 16, 24 and 32-bit registers, ABOVE THE FIRST 64K OF XDATA */
#define CY_GET_XTND_REG8(addr)           cyread8(addr)
#define CY_SET_XTND_REG8(addr, value)    cywrite8(addr,value)

#define CY_GET_XTND_REG16(addr)          cyread16(addr)
#define CY_SET_XTND_REG16(addr, value)   cywrite16(addr,value)

#define CY_GET_XTND_REG24(addr)          cyread24(addr)
#define CY_SET_XTND_REG24(addr, value)   cywrite24(addr,value)

#define CY_GET_XTND_REG32(addr)          cyread32(addr)
#define CY_SET_XTND_REG32(addr, value)   cywrite32(addr,value)



/* End of Keil C51. */
#endif




/* Include for the GNUC ARM compiler
*  GNUC for the ARM is a Little Endian compiler.
*/
#if defined(__GNUC__)


/* To allow code to be 8051-ARM agnostic. */
#define XDATA 

/* ARM wants Interrupt functions to not include 'interrupt 0' */
#define CY_ISR(FuncName)  void FuncName (void)

/* ARM wants Interrupt functions prototypes to not include 'interrupt 0' */
#define CY_ISR_PROTO(FuncName) void FuncName (void)

/* ARM naturarly returns an 32 bit value. */
typedef unsigned long cystatus;



/* Prototype for function to set a 24-bit register */
extern void     CySetReg24(uint32 *, uint32);

/* 8, 16, 24 and 32-bit register access macros */
#define CY_GET_REG8(addr)           	(*((volatile uint8 *)(addr)))
#define CY_SET_REG8(addr, value)    	(*((volatile uint8 *)(addr))  = (uint8)(value))

#define CY_GET_REG16(addr)          	(*((volatile uint16 *)(addr)))
#define CY_SET_REG16(addr, value)   	(*((volatile uint16 *)(addr)) = (uint16)(value))

#define CY_GET_REG24(addr)          	(*((uint32 *)(addr)) & 0x00FFFFFF)
#define CY_SET_REG24(addr, value)   	CySetReg24((uint32 *)addr, value)

#define CY_GET_REG32(addr)          	(*((volatile uint32 *)(addr)))
#define CY_SET_REG32(addr, value)   	(*((volatile uint32 *)(addr)) = (uint32)(value))


/* To allow code to be 8051-ARM agnostic. */
#define CY_GET_XTND_REG8(addr)			(*((volatile uint8 *)(addr)))			
#define CY_SET_XTND_REG8(addr, value)   (*((volatile uint8 *)(addr))  = (uint8)(value))

#define CY_GET_XTND_REG16(addr)         (*((volatile uint16 *)(addr)))
#define CY_SET_XTND_REG16(addr, value)  (*((volatile uint16 *)(addr)) = (uint16)(value))

#define CY_GET_XTND_REG24(addr)         (*((uint32 *)(addr)) & 0x00FFFFFF)
#define CY_SET_XTND_REG24(addr, value)  CySetReg24((uint32 *)addr, value)

#define CY_GET_XTND_REG32(addr)         (*((volatile uint32 *)(addr)))
#define CY_SET_XTND_REG32(addr, value)  (*((volatile uint32 *)(addr)) = (uint32)(value))





/* End of ARM GNUC Compiler. */
#endif



/* Acceptable types from MISRA-C for PSoC hardware registers. */
typedef unsigned char XDATA     reg8;
typedef unsigned short XDATA    reg16;
typedef unsigned long XDATA     reg32;








#endif


