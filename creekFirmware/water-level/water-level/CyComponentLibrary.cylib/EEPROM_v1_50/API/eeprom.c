/***************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Provides an API for the EEPROM component.
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include <Cydevice_trm.h>
#include <`$INSTANCE_NAME`.H>
#include "CYFLASH.H"


#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)    
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
* Summary: 
*  Enable the EEPROM.
*
* Parameters:  
*  void
*
* Return:
* void
*
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    CyEEPROM_Start();
}

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start       
********************************************************************************
* Summary:
*  Starts EEPROM
*
* Parameters:
*  void.
*
* Return:
*  void
* 
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    /* Enable the EEPROM */
    `$INSTANCE_NAME`_Enable();
}

/*******************************************************************************
* Function Name: CyEEPROM_Stop
********************************************************************************
*
* Summary:
*  Stops and powers down EEPROM.
*   
* Parameters:
*  void
* 
* Return:
*  void  
* 
* Reentrant
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop (void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Disable EEPROM */
    CyEEPROM_Stop();
}
#endif /* (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2) */

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EraseSector
********************************************************************************
* 
* Summary:
*  Erases a block of 64 lines of memory. This function blocks until the
*  operation is complete.
*
* Parameters:
*  sectorNumber. Sector number to erase.
*
* Return:
*  CYRET_SUCCESS if the operation was successful.
*  CYRET_BAD_PARAM if the parameters were invalid.
*  CYRET_LOCKED if the spc is being used.
*  CYRET_TIMEOUT if the operation timed out.
*  CYRET_UNKNOWN if there was an SPC error.
*
* Reentrant
*  Yes
*
********************************************************************************/
cystatus `$INSTANCE_NAME`_EraseSector(uint16 sectorNumber) `=ReentrantKeil($INSTANCE_NAME . "_EraseSector")`
{
    cystatus status;

    /* Make sure SPC is powerd. */
    CySpcActivePower(1);

    if(sectorNumber <= (uint16)(`$INSTANCE_NAME`_EEPROM_SIZE - CYDEV_EEPROM_ROW_SIZE))
    {
        /* See if we can get the SPC. */
        if(CySpcLock() == CYRET_SUCCESS)
        {
            /* Plan for failure. */
            status = CYRET_UNKNOWN;

            /* Create the command to erase a sector. */
            if(CySpcCreateCmdEraseSector(FIRST_EE_ARRAYID, sectorNumber) == CYRET_SUCCESS)
            {
                /* Write the command. */
                if(CySpcWriteCommand(0, 0) == CYRET_STARTED)
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
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }

    return status;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Write
********************************************************************************
* 
* Summary:
*  Writes a line (16 bytes) of data to the EEPROM. The rowNumber must be
*  16 byte aligned. This is a blocking call. It will not return until the
*  function succeeds or fails.
*
* Parameters:
*  rowData. Address of the row of data to write to the EEPROM.
*
*  rowNumber. EEPROM row number to program.
*
* Return:
*  CYRET_SUCCESS if the operation was successful.
*  CYRET_BAD_PARAM if the parameters were invalid.
*  CYRET_LOCKED if the spc is being used.
*  CYRET_TIMEOUT if the operation timed out.
*  CYRET_UNKNOWN if there was an SPC error.
*
* Reentrant
*  Yes
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_Write(uint8 * rowData, uint16 rowNumber) `=ReentrantKeil($INSTANCE_NAME . "_Write")`
{
    cystatus status;

    /* Make sure SPC is powerd. */
    CySpcActivePower(1);

    if(rowNumber < (uint16)(`$INSTANCE_NAME`_EEPROM_SIZE / CYDEV_EEPROM_ROW_SIZE))
    {
        /* Get the die temp. */
        status = CySetTemp();

        /* See if we can get the SPC. */
        if(status == CYRET_SUCCESS && CySpcLock() == CYRET_SUCCESS)
        {
            status = CYRET_UNKNOWN;

            /* Create the command to load the row latch. */
            if(CySpcCreateCmdLoadRow(FIRST_EE_ARRAYID) == CYRET_SUCCESS)
            {
                /* Write the command. */
                if(CySpcWriteCommand(rowData, CYDEV_EEPROM_ROW_SIZE) == CYRET_STARTED)
                {
                    /* Spin until completion. */
                    while(!(*SPC_STATUS & SPC_SPC_IDLE))
                    {
                        //CyDelay(1);
                    }

                    /* See if we were successful. */
                    if(CySpcReadStatus == CYRET_SUCCESS)
                    {
                        status = CYRET_SUCCESS;
                    }
                }
            }

            /* Create the command to erase and program the row. */
            if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(FIRST_EE_ARRAYID, rowNumber, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
            {   
                status = CYRET_UNKNOWN;

                /* Write the command. */
                if(CySpcWriteCommand(0, 0) == CYRET_STARTED)
                {
                    /* Spin until completion. */
                    while(!(*SPC_STATUS & SPC_SPC_IDLE))
                    {
                        //CyDelay(1);
                    }

                    /* See if we were successful. */
                    if(CySpcReadStatus == CYRET_SUCCESS)
                    {
                        status = CYRET_SUCCESS;
                    }
                }
            }
            else
            {
                status = CYRET_UNKNOWN; 
            }
            /* Unlock the SPC so someone else can use it. */
            CySpcUnlock();
        }
        else
        {
            status = CYRET_LOCKED;
        }
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }

    return status;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StartWrite
********************************************************************************
* 
* Summary:
*  Starts the SPC write function. This function does not block, it returns
*  once the command has begun the SPC write function. Once this function has
*  been called the SPC will be locked until MyEE_EEPRomQueryWrite() returns
*  CYRET_SUCCESS. If the caller wants to abandon the write he can call
*  CySpcUnlock() to unlock the SPC.
*  
*
* Parameters:
*  rowData. address of buffer containing a row of data to write to the EEPROM.
*
*  rowNumber. EEPROM row number to program.
*
* Return:
*  CYRET_STARTED if the spc command to write was successfuly started.
*  CYRET_INVALID_STATE if the die temp was not aquired before this function.
*  CYRET_BAD_PARAM if the parameters were invalid.
*  CYRET_LOCKED if the spc is being used.
*  CYRET_UNKNOWN if there was an SPC error.
*
* Reentrant
*  Yes
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_StartWrite(uint8 * rowData, uint16 rowNumber) `=ReentrantKeil($INSTANCE_NAME . "_StartWrite")`
{
    cystatus status;


    if(dieTemperature[1] == 0xFF)
    {
        /* dieTemp can never have this value so it has not been called. */
        status = CYRET_INVALID_STATE;
    }
    else if(rowNumber < (uint16)(`$INSTANCE_NAME`_EEPROM_SIZE / CYDEV_EEPROM_ROW_SIZE))
    {
        /* Get the die temp. */
        status = CySetTemp();

        /* See if we can get the SPC. */
        if(status == CYRET_SUCCESS && CySpcLock() == CYRET_SUCCESS)
        {
            status = CYRET_UNKNOWN;

            /* Create the command to load the row latch. */
            if(CySpcCreateCmdLoadRow(FIRST_EE_ARRAYID) == CYRET_SUCCESS)
            {
                /* Write the command. */
                if(CySpcWriteCommand(rowData, CYDEV_EEPROM_ROW_SIZE) == CYRET_STARTED)
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

            /* Create the command to erase and program the row. */
            if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(FIRST_EE_ARRAYID, rowNumber, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
            {
                status = CYRET_UNKNOWN;

                /* Write the command. */
                if(CySpcWriteCommand(0, 0) == CYRET_STARTED)
                {
                    status = CYRET_STARTED;
                }
            }
            else
            {
                status = CYRET_UNKNOWN; 
            }
        }
        else
        {
            status = CYRET_LOCKED;
        }
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }

    return status;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_QueryWrite
********************************************************************************
* 
* Summary:
*  Checks the state of a write to EEPROM. This function must be called until
*  the return value is not CYRET_STARTED. 
*
* Parameters:
*  void
*
* Return:
*  CYRET_STARTED if the spc command is still processing.
*  CYRET_INVALID_STATE if the die temp was not aquired before this function.
*  CYRET_BAD_PARAM if the parameters were invalid.
*  CYRET_LOCKED if the spc is being used.
*  CYRET_TIMEOUT if the operation timed out.
*  CYRET_UNKNOWN if there was an SPC error.
*
* Reentrant
*  Yes
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_QueryWrite(void) `=ReentrantKeil($INSTANCE_NAME . "_QueryWrite")`
{
    cystatus status;

    if(dieTemperature[1] == 0xFF)
    {
        /* dieTemp can never have this value so it has not been called. */
        status = CYRET_INVALID_STATE;
    }
    else
    {
        /* Spin until completion. */
        if(*SPC_STATUS & SPC_SPC_IDLE)
        {
            /* See if we were successful. */
            if(CySpcReadStatus == CYRET_SUCCESS)
            {
                status = CYRET_SUCCESS;
            }
            else
            {
                status = CYRET_UNKNOWN;
            }
        
            /* Unlock the SPC so someone else can use it. */
            CySpcUnlock();
        }
        else
        {
            status = CYRET_STARTED;
        }
    }

    return status;
}

