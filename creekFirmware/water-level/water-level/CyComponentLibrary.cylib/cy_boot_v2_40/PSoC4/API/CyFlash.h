/******************************************************************************
* File Name: CyFlash.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the FLASH/EEPROM.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2010-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#if !defined(__CYFLASH_H__)
#define __CYFLASH_H__

#include <device.h>

/*******************************************************************************
*     Function Prototypes
*******************************************************************************/
cystatus CySysFlashWriteRow(uint16 rowNum, uint8 *rowData);

/*******************************************************************************
*     Flash defines 
*******************************************************************************/
#define SRAM_SPCCMD		        0x200000FFu                  /*SPC command address */
#define SRAM_ROM_KEY1		    0x0000u          	         /*SROM Key1 address*/
#define SRAM_ROM_KEY2		    (SRAM_ROM_KEY1 + 0x01u)      /*SROM key2 address*/
#define SRAM_ROM_PARAM		    (SRAM_ROM_KEY1 + 0x02u)      /*SROM parameter address*/
#define SRAM_ROM_PARAM1	        (SRAM_ROM_KEY1 + 0x03u)      /*SROM parameter one address*/
#define SRAM_ROM_PARAM2	        (SRAM_ROM_KEY1 + 0x04u)      /*SROM parameter 2 address*/
#define SRAM_ROM_PARAM3	        (SRAM_ROM_KEY1 + 0x05u)      /*SROM parameter 3 address*/
#define SRAM_ROM_PARAM4	        (SRAM_ROM_KEY1 + 0x06u)      /*SROM parameter 4 address*/
#define SRAM_ROM_PARAM5	        (SRAM_ROM_KEY1 + 0x07u)      /*SROM parameter 5 address*/
#define SRAM_ROM_DATA		    (SRAM_ROM_KEY1 + 0x08u)      /*SROM data address*/
#define SRAM_ROM_RETVAL	        (SRAM_ROM_KEY1 + 0x03u)      /*SROM return value address*/

#define SROM_CMD_RETURN_MASK	0xF0000000u                  /*SROM command return value mask*/
#define SROM_CMD_RETURN_SUCC	0xA0000000u                  /*SROM command success */
#define SROM_KEY1				0xB6u                        /*SROM key for FLASH command*/
#define SROM_KEY2_LOAD			0xD7u                        /*SROM key for FLASH load command*/
#define SROM_KEY2_WRITE			0xD8u                        /*SROM key for FLASH write command*/

#define SROM_LOAD_CMD			(SROM_KEY2_LOAD << 8 | SROM_KEY1)        /*SROM key for FLASH commands*/

#define MMIO1_BASE				0x40100000u                               /*SROM key for FLASH commands*/
#define CPUSS_SYSREQ			(MMIO1_BASE + 0x0004u)                    /*SROM key for FLASH commands*/
#define CPUSS_SYSARG			(MMIO1_BASE + 0x0008u)                    /*SROM key for FLASH commands*/

#define LOAD_BYTE_OPCODE		0x4u                                      /*Load byte SROM opcode*/
#define WRITE_ROW_OPCODE		0x5u                                      /*Row write SROM opcode*/
#define CPUSS_REQ_START			(0x01u<<31)                              /*Request CPU to execute the special code*/

#define FLASH_NUMBER_ROWS       (CYDEV_FLASH_SIZE / CYDEV_FLS_ROW_SIZE)

/* __CYFLASH_H__ */
#endif
