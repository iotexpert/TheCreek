/***************************************************************************
* File Name: `$INSTANCE_NAME`_Dmac.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the EEPROM component.
*
*
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include <Cydevice.h>
#include <`$INSTANCE_NAME`.H>




/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EraseSector
********************************************************************************
* Summary:
*   Erases a block of 64 lines of memory. This function blocks until the
*   operation is complete.
*
* Parameters:
*   address. Address of sector to erase.
*
* Return:
*   CYRET_SUCCESS if the operation was successful.
*   CYRET_BAD_PARAM if the parameters were invalid.
*   CYRET_LOCKED if the spc is being used.
*   CYRET_TIMEOUT if the operation timed out.
*   CYRET_UNKNOWN if there was an SPC error.
*
********************************************************************************/
cystatus `$INSTANCE_NAME`_EraseSector(uint16 address)
{
    cystatus status;


    /* Make sure SPC is powerd. */
    CySpcActivePower(1);

    if(address <= (uint16)(`$INSTANCE_NAME`_EEPROM_SIZE - CYDEV_EEPROM_ROW_SIZE))
    {
        /* See if we can get the SPC. */
        if(CySpcLock() == CYRET_SUCCESS)
        {
            /* Plan for failure. */
            status = CYRET_UNKNOWN;

            /* Create the command to erase a sector. */
            if(CySpcCreateCmdEraseSector(FIRST_EE_ARRAYID, address) == CYRET_SUCCESS)
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
* Summary:
*   Writes a line (16 bytes) of data to the EEPROM address. The address must be
*   16 byte aligned. This is a blocking call. It will not return until the
*   function succeeds or fails.
*
*
* Parameters:
*   line. Address of the line of data to write to the EEPROM.
*
*   address. 16 byte aligned address in the EEPROM to program.
*
* Return:
*   CYRET_SUCCESS if the operation was successful.
*   CYRET_BAD_PARAM if the parameters were invalid.
*   CYRET_LOCKED if the spc is being used.
*   CYRET_TIMEOUT if the operation timed out.
*   CYRET_UNKNOWN if there was an SPC error.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_Write(uint8 * line, uint16 address)
{
    cystatus status;


    /* Make sure SPC is powerd. */
    CySpcActivePower(1);

    if(address <= (uint16)(`$INSTANCE_NAME`_EEPROM_SIZE - CYDEV_EEPROM_ROW_SIZE))
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
                if(CySpcWriteCommand(line, CYDEV_EEPROM_ROW_SIZE) == CYRET_STARTED)
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
            if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(FIRST_EE_ARRAYID, address, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
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
* Summary:
*   Starts the SPC write function. This function does not block, it returns
*   once the command has begun the SPC write function. Once this function has
*   been called the SPC will be locked until MyEE_EEPRomQueryWrite() returns
*   CYRET_SUCCESS. If the caller wants to abandon the write he can call
*   CySpcUnlock() to unlock the SPC.
*  
*
* Parameters:
*   line. address of buffer containing a line of data to write to the EEPROM.
*
*   address. Address of the line in EEPROM to write.
*
* Return:
*   CYRET_STARTED if the spc command to write was successfuly started.
*   CYRET_INVALID_STATE if the die temp was not aquired before this function.
*   CYRET_BAD_PARAM if the parameters were invalid.
*   CYRET_LOCKED if the spc is being used.
*   CYRET_UNKNOWN if there was an SPC error.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_StartWrite(uint8 * line, uint16 address)
{
    cystatus status;


    if(dieTemperature[1] == 0xFF)
    {
        /* dieTemp can never have this value so it has not been called. */
        status = CYRET_INVALID_STATE;
    }
    else if(address <= (uint16)(`$INSTANCE_NAME`_EEPROM_SIZE - CYDEV_EEPROM_ROW_SIZE))
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
                if(CySpcWriteCommand(line, CYDEV_EEPROM_ROW_SIZE) == CYRET_STARTED)
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
            if(status == CYRET_SUCCESS && CySpcCreateCmdWriteRow(FIRST_EE_ARRAYID, address, dieTemperature[0], dieTemperature[1]) == CYRET_SUCCESS)
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
* Summary:
*   Checks the state of a write to EEPROM. This function must be called until
*   the return value is not CYRET_STARTED. 
*  
*
* Parameters:
*
* Return:
*   CYRET_STARTED if the spc command is still processing.
*   CYRET_INVALID_STATE if the die temp was not aquired before this function.
*   CYRET_BAD_PARAM if the parameters were invalid.
*   CYRET_LOCKED if the spc is being used.
*   CYRET_TIMEOUT if the operation timed out.
*   CYRET_UNKNOWN if there was an SPC error.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_QueryWrite(void)
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


