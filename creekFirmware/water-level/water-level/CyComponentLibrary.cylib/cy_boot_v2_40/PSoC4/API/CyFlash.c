/*******************************************************************************
* File Name: CyFlash.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the FLASH/EEPROM.
*
*  Note:
*   This code is endian agnostic.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
*******************************************************************************
* Copyright 2010-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include <CyFlash.h>

/*******************************************************************************
* Function Name: CySysFlashWriteRow
********************************************************************************
*
* Summary: Writes a row of Flash.
* 
* Parameters: 
*   rowNum: Row number.  Each row is 128 bytes so a 32KB flash memory will have 
*    a valid range of [0-511].
*   rowData: Array of bytes to write.
*
* Return Value: Status
*		Value			Description 
*		CYRET_SUCCESS	Successful
*		CYRET_LOCKED	Flash writing already in use
*		CYRET_CANCELED	Command not accepted
*		Other non-zero	Failure
* 
*******************************************************************************/
cystatus CySysFlashWriteRow(uint16 rowNum, uint8 *rowData)
{
    cystatus ret_value = CYRET_SUCCESS;
	uint32 ptr = 0;
    uint32 cpu_value = 0;
    uint8 i = 0;
    uint8 localData[CYDEV_FLS_ROW_SIZE + SRAM_ROM_DATA];
	
	CYASSERT(rowNum <= FLASH_NUMBER_ROWS - 1);
	CYASSERT(rowData);
    /*First load the data. 
      Then invoke Write row command.
    */
	ptr = (uint32) localData + SRAM_ROM_KEY1;
    CY_SET_REG32(CYREG_CPUSS_SYSARG, (uint32)localData + SRAM_ROM_KEY1);
    CY_SET_REG32(ptr, SROM_LOAD_CMD | (rowNum & 0x100) << 16);
	ptr = (uint32) localData + SRAM_ROM_PARAM2;
	CY_SET_REG32(ptr, CYDEV_FLS_ROW_SIZE - 1);
	
    for(i = 0; i < CYDEV_FLS_ROW_SIZE; i++)
    {
		localData [SRAM_ROM_DATA + i] = rowData[i];
    }
    
    /*Perform sysreq load data, opcode 0x80000004h*/
    CY_SET_REG32(CYREG_CPUSS_SYSREQ, LOAD_BYTE_OPCODE | CPUSS_REQ_START);
	
	/*wait till the CPUSS_REQ_START bit goes low*/
	while((CY_GET_REG32(CYREG_CPUSS_SYSREQ) & CPUSS_REQ_START) == CPUSS_REQ_START)
	{
		;
	}
    /*Check if the value is written to the SRAM_BASE*/
    cpu_value = CY_GET_REG32(CYREG_CPUSS_SYSARG);
    if (cpu_value == SROM_CMD_RETURN_SUCC) 
    {
        ret_value = CYRET_SUCCESS;
    }
    else if((cpu_value & SROM_CMD_RETURN_MASK) == SROM_CMD_RETURN_MASK)
	{
		/*Error value, cant proceed */
		ret_value = CYRET_CANCELED;
	}
	else
	{
        /*Wait till the request is completed.*/
        while((cpu_value & SROM_CMD_RETURN_MASK) != SROM_CMD_RETURN_SUCC)
        {
            cpu_value = CY_GET_REG32(CYREG_CPUSS_SYSARG);
        }
    }
    
    if(ret_value == CYRET_SUCCESS)
    {
		ptr = (uint32) localData + SRAM_ROM_KEY1;
        CY_SET_REG32(CYREG_CPUSS_SYSARG, ptr);
   
        /*Write row command to SROM*/
		CY_SET_REG32(ptr, rowNum << 16 | (SROM_KEY2_WRITE << 8 ) | SROM_KEY1);

        /*Perform sysreq for Write row, opcode 0x80000005*/
        CY_SET_REG32(CYREG_CPUSS_SYSREQ , WRITE_ROW_OPCODE | CPUSS_REQ_START);

		/*wait till the CPUSS_REQ_START bit goes low*/
		while((CY_GET_REG32(CYREG_CPUSS_SYSREQ) & CPUSS_REQ_START) == CPUSS_REQ_START)
		{
			;
		}

        cpu_value = CY_GET_REG32(CYREG_CPUSS_SYSARG);
        if (cpu_value != SROM_CMD_RETURN_SUCC) 
        {
            ret_value = cpu_value;
        }
    }
	return ret_value;
}


/* End of File */
