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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include <Cydevice_trm.h>
#include "CYFLASH.H"


#if !defined(CYDEV_FLS_BASE)

#define CYDEV_FLS_BASE          0

#endif

#define ECC_ADDR                0x80u
#define PM_ACT_EEFLASH          ((reg8 *) CYDEV_PM_ACT_CFG0)
#define PM_STBY_EEFLASH         ((reg8 *) CYDEV_PM_STBY_CFG0)
#define PM_FLASH_EE_MASK        0x80u

/* Default values for getting temperature. */
#define TEMP_NUMBER_OF_SAMPLES  0x1u
#define TEMP_TIMER_PERIOD       0xFFFu
#define TEMP_CLK_DIV_SELECT     0x4u
#define NUM_SAMPLES             (1 << (TEMP_NUMBER_OF_SAMPLES))
#define SPC_CLK_PERIOD          120 /* nano sec. */
#define CY_SYS_ns_PER_TICK      1000
#define FRM_EXEC_TIME           (1000) /* In nano seconds. */
#define GET_TEMP_TIME           (1 << (NUM_SAMPLES + 1)) * \
                                (SPC_CLK_PERIOD * TEMP_CLK_DIV_SELECT) * \
                                TEMP_TIMER_PERIOD + \
                                FRM_EXEC_TIME
#define TEMP_MAX_WAIT           ((GET_TEMP_TIME  ) / CY_SYS_ns_PER_TICK) /* In system ticks. */
#define WHILE_TEST(t)           {register uint8 timeOut; for(timeOut = 0xFF; (t) && timeOut; timeOut--) {}}


uint8 dieTemperature[2];

#if (CYDEV_ECC_ENABLE == 0)
    static uint8 * rowBuffer = 0;
#endif

/*******************************************************************************
* Function Name: CySetTemp
********************************************************************************
* Summary:
*   Sends a command to the SPC to read the die temperature. Sets a global value
*   used by the Write functions.
*
*
* Parameters:
*   void:
*
*   
* Return:
*   CYRET_UNKNOWN if there was an SPC error.
*   The first byte is the sign of the temperature (0 = negative, 1 = positive).
*   The second byte is the magnitude.
*
*******************************************************************************/
cystatus CySetTemp(void)
{
    uint8 ticks;
    cystatus status;


    /* Make sure SPC is powerd. */
    CySpcActivePower(1);

    /* Plan for failure. */
    status = CYRET_UNKNOWN;

    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Create the command. */
        if(CySpcCreateCmdGetTemp(TEMP_NUMBER_OF_SAMPLES, TEMP_TIMER_PERIOD, TEMP_CLK_DIV_SELECT) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(0, 0) == CYRET_STARTED)
            {
                ticks = (uint8) TEMP_MAX_WAIT;

                do
                {
                    if(CySpcReadData(dieTemperature, 2) == 2)
                    {
                        status = CYRET_SUCCESS;
                        break;
                    }
                    
                    /* Wait for the reading to come in. */
                    CyDelay(1);
                    ticks--;

                } while(ticks && !(*SPC_STATUS & SPC_SPC_IDLE));
            }
        }

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }
    else
    {
        status = CYRET_LOCKED;
    }

    return status;
}


/*******************************************************************************
* Function Name: CySetFlashEEBuffer
********************************************************************************
* Summary:
*   Sets the user supplied temporary buffer to store SPC data while performing
*   flash and EEPROM commands.
*
*
* Parameters:
*   buffer:
*       Address of block of memory to store temporary memory. The size of the
*       block of memory is SIZEOF_FLASH_ROW + SIZEOF_ECC_ROW.
*   
* Return:
*   CYRET_SUCCESS if successful.
*   CYRET_LOCKED if the SPC is already in use.
*   CYRET_BAD_PARAM if the buffer is 0.
*
*******************************************************************************/
cystatus CySetFlashEEBuffer(uint8 * buffer)
{
    cystatus status;


    /* Make sure SPC is powerd. */
    CySpcActivePower(1);

    if(!buffer)
    {
        status = CYRET_BAD_PARAM;
    }
    /* See if we can get the SPC. */
    else if(CySpcLock() != CYRET_SUCCESS)
    {
        status = CYRET_LOCKED;
    }
    else
    {
		#if (CYDEV_ECC_ENABLE == 0)
            rowBuffer = buffer;
        #endif
        status = CYRET_SUCCESS;

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }

    return status;
}
                      
#if (CYDEV_ECC_ENABLE == 1)

/*******************************************************************************
* Function Name: CyWriteRowData
********************************************************************************
* Summary:
*   Sends a command to the SPC to load and program a row of data in flash.
*
*
* Parameters:
*   rowAddress:
*       rowAddress of flash row to program.
*
*   
* Return:
*   CYRET_SUCCESS if successful.
*   CYRET_LOCKED if the SPC is already in use.
*   CYRET_UNKNOWN if there was an SPC error.
*
*
*******************************************************************************/
cystatus CyWriteRowData(uint8 arrayId, uint16 rowAddress, uint8 * rowData)
{
    uint16 rowSize;
    cystatus status;


    rowSize = (arrayId > LAST_FLASH_ARRAYID) ? SIZEOF_EEPROM_ROW : SIZEOF_FLASH_ROW;

    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Create the command. */
        status = CySpcCreateCmdLoadRow(arrayId);
        if(status == CYRET_SUCCESS)
        {
            /* Write the command. */
            status = CySpcWriteCommand(rowData, rowSize);
            if(status == CYRET_STARTED)
            {
                /* Spin until completion. */
                WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));

                /* See if we were successful. */
                status = CySpcReadStatus;
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS)
        {
            status = CySpcCreateCmdWriteRow(arrayId, rowAddress, dieTemperature[0], dieTemperature[1]);
            if(status == CYRET_SUCCESS)
            {
                /* Write the command. */
                if(CySpcWriteCommand(0, 0) == CYRET_STARTED)
                {
                    /* Spin until completion. */
                    WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));

                    /* See if we were successful. */
                    status = CySpcReadStatus;
                }
            }
        }

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }
    else
    {
        status = CYRET_LOCKED;
    }

    return status;
}

#else
/* CYDEV_ECC_ENABLE == 0 */


/*******************************************************************************
* Function Name: CyWriteRowData
********************************************************************************
* Summary:
*   Sends a command to the SPC to load and program a row of data in flash.
*
*
* Parameters:
*   rowAddress:
*       rowAddress of flash row to program.
*
*   
* Return:
*   CYRET_SUCCESS if successful.
*   CYRET_LOCKED if the SPC is already in use.
*   CYRET_UNKNOWN if there was an SPC error.
*
*
*******************************************************************************/
cystatus CyWriteRowData(uint8 arrayId, uint16 rowAddress, uint8 * rowData)
{
    uint8 Index;
    uint32 Offset;
    uint16 rowSize;
    cystatus status;


    rowSize = (arrayId > LAST_FLASH_ARRAYID) ? SIZEOF_EEPROM_ROW : SIZEOF_FLASH_ROW;

    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        if(rowSize != SIZEOF_EEPROM_ROW)
        {
            /* Save the ECC area. */
            Offset = CYDEV_ECC_BASE + ((uint32) rowAddress * SIZEOF_ECC_ROW);
            for(Index = 0; Index < SIZEOF_ECC_ROW; Index++)
            {
                rowBuffer[SIZEOF_FLASH_ROW + Index] = CY_GET_XTND_REG8(Offset + Index);
            }

        }

        /* Copy the rowdata to the temporary buffer. */
        cymemcpy(rowBuffer, rowData, SIZEOF_FLASH_ROW);

        /* Create the command. */
        status = CySpcCreateCmdLoadRow(arrayId);
        if(status == CYRET_SUCCESS)
        {
            if(rowSize == SIZEOF_FLASH_ROW)
            {
                /* If we are writing to flash add the ecc size. */
                rowSize += SIZEOF_ECC_ROW;
            }

            /* Write the command. */
            status = CySpcWriteCommand(rowBuffer, rowSize);
            if(status == CYRET_STARTED)
            {
                /* Spin until completion. */
                WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));


                /* See if we were successful. */
                status = CySpcReadStatus;
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS)
        {
            status = CySpcCreateCmdWriteRow(arrayId, rowAddress, dieTemperature[0], dieTemperature[1]);
            if(status == CYRET_SUCCESS)
            {
                /* Write the command. */
                status = CySpcWriteCommand(0, 0);
                if(status == CYRET_STARTED)
                {
                    /* Spin until completion. */
                    WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));

                    /* See if we were successful. */
                    status = CySpcReadStatus;
                }
            }
        }

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }
    else
    {
        status = CYRET_LOCKED;
    }

    return status;
}

/* CYDEV_ECC_ENABLE == 0 */
#endif



#if (CYDEV_ECC_ENABLE == 0 && CYDEV_CONFIGURATION_ECC == 0)

/*******************************************************************************
* Function Name: CyWriteRowConfig
********************************************************************************
* Summary:
*   Sends a command to the SPC to load and program a row of config data in flash.
*
*
* Parameters:
*   address:
*       Address of the sector to erase.
*
*   
* Return:
*   CYRET_SUCCESS if successful.
*   CYRET_LOCKED if the SPC is already in use.
*   CYRET_UNKNOWN if there was an SPC error.
*
*
*******************************************************************************/
cystatus CyWriteRowConfig(uint8 arrayId, uint16 rowAddress, uint8 * rowECC)
{
    uint32 Offset;
    uint16 Index;
    cystatus status;


    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Plan for failure. */
        status = CYRET_UNKNOWN;

        status = CYRET_SUCCESS;
        
        /* Read the existing flash data. */
        Offset = CYDEV_FLS_BASE + ((uint32) rowAddress * SIZEOF_FLASH_ROW);
        for(Index = 0; Index < SIZEOF_FLASH_ROW; Index++)
        {
            rowBuffer[Index] = CY_GET_XTND_REG8(Offset + Index);
        }

        /* Copy the rowECC to the temporary buffer. */
        cymemcpy(&rowBuffer[SIZEOF_FLASH_ROW], rowECC, SIZEOF_ECC_ROW);

        /* Create the command. */
        status = CySpcCreateCmdLoadRow(arrayId);
        if(status == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(rowBuffer, SIZEOF_FLASH_ROW + SIZEOF_ECC_ROW) == CYRET_STARTED)
            {
                /* Spin until completion. */
                WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));

                /* See if we were successful. */
                status = CySpcReadStatus;
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS)
        {
            status = CySpcCreateCmdWriteRow(arrayId, rowAddress, dieTemperature[0], dieTemperature[1]);
            if(status == CYRET_SUCCESS)
            {
                /* Write the command. */
                if(CySpcWriteCommand(0, 0) == CYRET_STARTED)
                {
                    /* Spin until completion. */
                    WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));

                    /* See if we were successful. */
                    status = CySpcReadStatus;
                }
            }
        }

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }
    else
    {
        status = CYRET_LOCKED;
    }

    return status;
}

/* (CYDEV_ECC_ENABLE == 1 && CYDEV_CONFIGURATION_ECC == 0) */
#endif

/*******************************************************************************
* Function Name: CyFlashEEActivePower
********************************************************************************
* Summary:
*   Selects the power for active operation mode.
*
*
* Parameters:
*   state:
*       Active power state. 1 is active, 0 inactive.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void CyFlashEEActivePower(uint8 state)
{
    if(state == 0)
    {
        *PM_ACT_EEFLASH &= ~PM_FLASH_EE_MASK;
    }
    else
    {
        *PM_ACT_EEFLASH |= PM_FLASH_EE_MASK;
    }
}

/*******************************************************************************
* Function Name: CyFlashEEStandbyPower
********************************************************************************
* Summary:
*   Selects the power for standby operation modes.
*
*
* Parameters:
*   state:
*       Standby power state. 1 is active, 0 inactive.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void CyFlashEEStandbyPower(uint8 state)
{
    if(state == 0)
    {
        *PM_STBY_EEFLASH &= ~PM_FLASH_EE_MASK;
    }
    else
    {
        *PM_STBY_EEFLASH |= PM_FLASH_EE_MASK;
    }
}

