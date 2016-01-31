/*******************************************************************************
* File Name: `$INSTANCE_NAME`_DieTemp.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Provides the API to acquire the die temperature.
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include <CYDEVICE_TRM.H>
#include <`$INSTANCE_NAME`.H>


#define `$INSTANCE_NAME`_NUMBER_OF_SAMPLES  0x1u
#define `$INSTANCE_NAME`_TIMER_PERIOD       0xFFFu
#define `$INSTANCE_NAME`_CLK_DIV_SELECT     0x4u

#define SPC_CLK_PERIOD                      120 /* nano sec. */

#define NANO_TO_MILLI_FACTOR                1000000
#define FRM_EXEC_TIME                       (1000) /* In nano seconds. */
#define GET_TEMPR_TIME                      (1 << (`$INSTANCE_NAME`_NUMBER_OF_SAMPLES + 1)) * \
                                            (SPC_CLK_PERIOD * `$INSTANCE_NAME`_CLK_DIV_SELECT) * \
                                            `$INSTANCE_NAME`_TIMER_PERIOD + \
                                            FRM_EXEC_TIME
#define `$INSTANCE_NAME`_MAX_WAIT           ((GET_TEMPR_TIME + NANO_TO_MILLI_FACTOR - 1) / NANO_TO_MILLI_FACTOR) /* In milli seconds. */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* 
* Summary:
*  Starts the SPC command to get the die temperature. If this function is
*  called successfuly. The SPC will be locked and `$INSTANCE_NAME`_QueryDieTemp
*  will have to be successfuly called to unlock it. CySpcUnlock() can also
*  be called if the caller decides not to finish the temperature reading.
*
* Parameters:
*  void.
*
* Return:
*  CYRET_STARTED if the spc command was started sucessfuly.
*  CYRET_UNKNOWN if the spc command failed.
*  CYRET_LOCKED if the spc was busy.
*
* Reentrant:
*  Yes.
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    cystatus status;
    
    /* Plan for failure. */
    status = CYRET_UNKNOWN;
    
    /* Power up the SPC. */
    CySpcActivePower(1);
    CySpcStandbyPower(1);

    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Create the command. */
        if(CySpcCreateCmdGetTemp(`$INSTANCE_NAME`_NUMBER_OF_SAMPLES, `$INSTANCE_NAME`_TIMER_PERIOD, `$INSTANCE_NAME`_CLK_DIV_SELECT) == CYRET_SUCCESS)
        {
            /* Write the command. */
            status = CySpcWriteCommand(0, 0);
        }
    }
    else
    {
        status = CYRET_LOCKED;
    }
    return status;
}


/*******************************************************************************
* FUNCTION NAME:   `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Stops the temperature reading
*
* Parameters:  
*  void
*
* Return: 
*  void
*
* Reentrant:
*  Yes
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* Inactive SPC power state. */
    CySpcActivePower(0);
    CySpcStandbyPower(0);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Query
********************************************************************************
*
* Summary:
*  Querys the SPC to see if the termperature command is finished.
*   
* Parameters:
*  temperature: Address to store the two byte temperature value.
* 
* Return:
*  CYRET_SUCCESS if the temperature command completed sucessfully.
*  CYRET_UNKNOWN if the there was an spc failure.
*  CYRET_STARTED if the temperature command has not completed.
*  CYRET_TIMEOUT if waited to long before reading data.
*
* Reentrant:
*  Yes
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_Query(int16 * temperature) `=ReentrantKeil($INSTANCE_NAME . "_Query")`
{
    uint8 temp[2];
    cystatus status;

    /* Are we still waiting? */
    if(!(*SPC_STATUS & SPC_SPC_IDLE))
    {
        /* See if an error occured. */
        if(CySpcReadStatus == CYRET_SUCCESS)
        {
            /* See if any data is ready. */
            if(CySpcReadData(temp, 2) == 2)
            {
                /* Set magnitude */
                if(temp[0])
                    *temperature = (int16) ((uint16) temp[1]);
                else
                    *temperature = (int16) - ((uint16) temp[1]);
                
                status = CYRET_SUCCESS;
            }
            else
            {
                status = CYRET_UNKNOWN;
            }
        }
        else
        {
            /* Need to wait longer. */
            status = CYRET_STARTED;
        }
    }
    else
    {
        /* We probly waited too long. */
        status = CYRET_TIMEOUT;
    }

    if(status != CYRET_STARTED)
    {
        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }

    return status;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetTemp
********************************************************************************
*
* Summary:
*  Sets up the command to get the temperature and blocks until finished. After
*  `$INSTANCE_NAME`_MAX_WAIT ticks the function will return even if the
*  SPC has not finished.
*   
* Parameters:
*  temperature: Address to store the two byte temperature value.
*
* Return:
*  CYRET_SUCCESS if the command was completed sucessfuly.
*  Status code from `$INSTANCE_NAME`_Start or `$INSTANCE_NAME`_Query
*
* Reentrant:
*  Yes
*
*******************************************************************************/
cystatus `$INSTANCE_NAME`_GetTempT(int16 * temperature) `=ReentrantKeil($INSTANCE_NAME . "_GetTempT")`
{
    uint16 ms;
    cystatus status;

    /* Start the temp reading */
    status = `$INSTANCE_NAME`_Start();

    if(status == CYRET_STARTED)
    {
        for (ms=`$INSTANCE_NAME`_MAX_WAIT; ms>0; ms--)
        {
            status = `$INSTANCE_NAME`_Query(temperature);
            if(status != CYRET_STARTED)
            {
                break;
            }
            CyDelay(1);
        }
    }

    return status;
}
cystatus `$INSTANCE_NAME`_GetTemp(int16 * temperature) `=ReentrantKeil($INSTANCE_NAME . "_GetTemp")`
{ 
    cystatus status;
    uint8 count = 2;
    while (count!=0)
    {
        status = `$INSTANCE_NAME`_GetTempT(temperature);
        if (status != CYRET_SUCCESS) 
        {
            break;
        }
        count--;
    }
    return status;
}


/* [] END OF FILE */
