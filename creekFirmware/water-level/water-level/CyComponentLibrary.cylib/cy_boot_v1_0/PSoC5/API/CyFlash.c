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
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include <CYDEVICE.H>
#include "CYFLASH.H"



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

uint8 dieTemperature[2];

static uint8 * rowBuffer = 0;

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


    if(buffer)
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
        rowBuffer = buffer;
        status = CYRET_SUCCESS;

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }

    return status;
}

#if !defined(ECC_FOR_CONFIG)

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
    uint8 rowSize;
    cystatus status;


    if(arrayId > LAST_FLASH_ARRAYID)
    {
        rowSize = SIZEOF_EEPROM_ROW;
    }
    else
    {
        rowSize = SIZEOF_FLASH_ROW;
    }

    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Plan for failure. */
        status = CYRET_UNKNOWN;

        /* Create the command. */
        if(CySpcCreateCmdLoadRow(arrayId) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(rowData, rowSize) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    status = CYRET_SUCCESS;
                }
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(arrayId, rowAddress, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(0, 0) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    status = CYRET_SUCCESS;
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

/* ECC_FOR_CONFIG */
#else

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
    uint8 rowSize;
    cystatus status;


    if(arrayId > LAST_FLASH_ARRAYID)
    {
        rowSize = SIZEOF_EEPROM_ROW;
    }
    else
    {
        rowSize = SIZEOF_FLASH_ROW;
    }

    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        if(rowSize == SIZEOF_EEPROM_ROW)
        {
            status = CYRET_SUCCESS;
        }
        else if(CySpcCreateCmdReadMultiByte(arrayId, ECC_ADDR, rowAddress >> 3, SIZEOF_ECC_ROW) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(0, 0) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    if(CySpcReadData(&rowBuffer[SIZEOF_FLASH_ROW], SIZEOF_ECC_ROW) == SIZEOF_ECC_ROW)
                    {
                        cymemcpy(rowBuffer, rowData, SIZEOF_FLASH_ROW);
                        status = CYRET_SUCCESS;
                    }
                }
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS && CySpcCreateCmdLoadRow(arrayId) == CYRET_SUCCESS)
        {
            if(rowSize == SIZEOF_FLASH_ROW)
            {
                /* If we are writing to flash add the ecc size. */
                rowSize += SIZEOF_ECC_ROW;
            }

            /* Write the command. */
            if(CySpcWriteCommand(rowData, rowSize) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    status = CYRET_SUCCESS;
                }
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(arrayId, rowAddress, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(0, 0) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    status = CYRET_SUCCESS;
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
    cystatus status;


    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Plan for failure. */
        status = CYRET_UNKNOWN;

        /* Read the existing flash data. */
        if(CySpcCreateCmdReadMultiByte(arrayId, 0, rowAddress, SIZEOF_FLASH_ROW) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(0, 0) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    if(CySpcReadData(rowBuffer, SIZEOF_FLASH_ROW) == SIZEOF_FLASH_ROW)
                    {
                        cymemcpy(&rowBuffer[SIZEOF_FLASH_ROW], rowECC, SIZEOF_ECC_ROW);
                        status = CYRET_SUCCESS;
                    }
                }
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS && CySpcCreateCmdLoadRow(arrayId) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(rowBuffer, SIZEOF_FLASH_ROW + SIZEOF_ECC_ROW) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    status = CYRET_SUCCESS;
                }
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(arrayId, rowAddress, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
        {
            /* Write the command. */
            if(CySpcWriteCommand(0, 0) == CYRET_SUCCESS)
            {
                /* Spin until completion. */
                while(!(*SPC_STATUS & SPC_SPC_IDLE))
                {
                    CyDelay(1);
                }

                /* See if we were successful. */
                if(CySpcReadStatus == CYRET_SUCCESS)
                {
                    status = CYRET_SUCCESS;
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

/* ECC_FOR_CONFIG */
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






