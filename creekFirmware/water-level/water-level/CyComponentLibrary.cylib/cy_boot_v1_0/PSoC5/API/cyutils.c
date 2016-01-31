/*******************************************************************************
* FILENAME: cyutils.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*  
*  Description:
*   CyUtils provides functions to handles endian issues for 16 and 32-bit 
*   registers.
* 
*  Note:
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




#include "cytypes.h"

typedef union
{
	uint8 byte[2];
	uint16 value;
} union_reg16;

typedef union
{
	uint8 byte[4];
	uint32 value;
} union_reg32;

/* unsigned short byte locations */
#define LE16_MSB_LOCATION	1
#define LE16_LSB_LOCATION	0	

#define BE16_MSB_LOCATION	0	
#define BE16_LSB_LOCATION	1	

/* uint32 byte locations */
/* NLSB (Next Least Significant Byte) */
/* NMSB (Next Most Significant Byte)  */
#define BE32_LSB_LOCATION 	3
#define BE32_NLSB_LOCATION 	2
#define BE32_NMSB_LOCATION 	1
#define BE32_MSB_LOCATION 	0
   
#define LE32_LSB_LOCATION 	0
#define LE32_NLSB_LOCATION 	1
#define LE32_NMSB_LOCATION 	2
#define LE32_MSB_LOCATION 	3

#if defined(__C51__)

/* Functions for register access of 16 and 32 bit for big endian compiler */
uint16 CyGetSwapReg16(uint16 xdata * addr)
{
	/* create a union reference to the address of the register.*/
	union_reg16 reg;
	
	reg.byte[BE16_MSB_LOCATION] = CY_GET_REG8((uint8 *)addr + LE16_MSB_LOCATION);
	reg.byte[BE16_LSB_LOCATION] = CY_GET_REG8((uint8 *)addr + LE16_LSB_LOCATION);
					 
	return reg.value;
}

void CySetSwapReg16(uint16 xdata * addr, unsigned short value)
{
	/* create a union reference to the address of the register.*/
	union_reg16 reg;
	reg.value = value;
	
	CY_SET_REG8((uint8 *)addr + LE16_LSB_LOCATION, reg.byte[BE16_LSB_LOCATION]);
	/* Address plus 1 */
	CY_SET_REG8((uint8 *)addr + LE16_MSB_LOCATION, reg.byte[BE16_MSB_LOCATION]);

	return;
}

/* 24-bit register access functions */
uint32 CyGetSwapReg24(uint32 xdata * addr)
{
	/* create a union reference to the address of the register.*/
	union_reg32 reg;

    /* MSB of big endian value gets MSB of little endian 24-bit register, not MSB of 32-bit value read */
	reg.byte[BE32_LSB_LOCATION ] = CY_GET_REG8(((uint8 *)addr) + LE32_LSB_LOCATION);
	reg.byte[BE32_NLSB_LOCATION] = CY_GET_REG8(((uint8 *)addr) + LE32_NLSB_LOCATION);
	reg.byte[BE32_NMSB_LOCATION] = CY_GET_REG8(((uint8 *)addr) + LE32_NMSB_LOCATION);
	reg.byte[BE32_MSB_LOCATION ] = 0;
					 
	return reg.value;
}

void CySetSwapReg24(uint32 xdata * addr, unsigned long value)
{
	/* create a union reference to the address of the register.*/
	union_reg32 reg;
	reg.value = value;

	CY_SET_REG8(((uint8 *)addr) + LE32_LSB_LOCATION ,  reg.byte[BE32_LSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_NLSB_LOCATION,  reg.byte[BE32_NLSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_NMSB_LOCATION,  reg.byte[BE32_NMSB_LOCATION]);

	return;
}

/* 32-bit register access functions */
uint32 CyGetSwapReg32(uint32 xdata * addr)
{
	/* create a union reference to the address of the register.*/
	union_reg32 reg;

	reg.byte[BE32_MSB_LOCATION] =  CY_GET_REG8(((uint8 *)addr) + LE32_MSB_LOCATION);
	reg.byte[BE32_NMSB_LOCATION] = CY_GET_REG8(((uint8 *)addr) + LE32_NMSB_LOCATION);
	reg.byte[BE32_NLSB_LOCATION] = CY_GET_REG8(((uint8 *)addr) + LE32_NLSB_LOCATION);
	reg.byte[BE32_LSB_LOCATION] =  CY_GET_REG8(((uint8 *)addr) + LE32_LSB_LOCATION);
					 
	return reg.value;
}

void CySetSwapReg32(uint32 xdata * addr, uint32 value)
{
	/* create a union reference to the address of the register.*/
	union_reg32 reg;
	reg.value = value;

	CY_SET_REG8(((uint8 *)addr) + LE32_LSB_LOCATION, reg.byte[BE32_LSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_NLSB_LOCATION, reg.byte[BE32_NLSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_NMSB_LOCATION, reg.byte[BE32_NMSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_MSB_LOCATION,  reg.byte[BE32_MSB_LOCATION]);

	return;
}

#endif

/* SetReg24 is for little endian compilers.*/
void CySetReg24(uint32 XDATA * addr, uint32 value)
{
	/* create a union reference to the address of the register.*/
	union_reg32 reg;
	reg.value = value;

    /* Writes 3, byte values in succession to the register's location.*/
	CY_SET_REG8(((uint8 *)addr) + LE32_LSB_LOCATION, reg.byte[LE32_LSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_NLSB_LOCATION, reg.byte[LE32_NLSB_LOCATION]);
	CY_SET_REG8(((uint8 *)addr) + LE32_NMSB_LOCATION, reg.byte[LE32_NMSB_LOCATION]);
	
	return;
}
