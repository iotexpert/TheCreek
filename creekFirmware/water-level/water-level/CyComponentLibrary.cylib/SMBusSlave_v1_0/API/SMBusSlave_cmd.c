/*******************************************************************************
* File Name: `$INSTANCE_NAME`_cmd.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides source code related to SM/PM bus command handling.
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`_cmd.h"


volatile uint8 `$INSTANCE_NAME`_lastReceivedCmd = `$INSTANCE_NAME`_CMD_UNDEFINED;  /* Stores last received CMD code */
volatile uint8 `$INSTANCE_NAME`_cmdProperties;                                     /* Command properties */
volatile uint8 `$INSTANCE_NAME`_cmdPage = 0u;                             /* Current page value, equals 0 by default */
volatile uint8  * `$INSTANCE_NAME`_cmdDataPtr;                                     /* Pointer to command data */

/* Holds a command code and properties extrated due last
* receiving of QUERY command.
*/
uint8 `$INSTANCE_NAME`_queryCmd = `$INSTANCE_NAME`_CMD_UNDEFINED;
uint8 `$INSTANCE_NAME`_queryData = 0u;


/**********************************
*      Eexternal variables
**********************************/

#if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))
    /* Writes to this buffer */
    extern uint8 `$INSTANCE_NAME`_btldrReadBuf[`$INSTANCE_NAME`_MAX_BUFFER_SIZE];
    /* Reads from this buffer */
    extern uint8 `$INSTANCE_NAME`_btldrWriteBuf[`$INSTANCE_NAME`_MAX_BUFFER_SIZE];
    
    extern uint8 `$INSTANCE_NAME`_btldrStatus;
    
    extern uint8 `$INSTANCE_NAME`_btldrWrBufByteCount;
    extern uint8 `$INSTANCE_NAME`_btldrRdBufByteCount;
    
#endif /* if defined(CYDEV_BOOTLOADER_IO_COMP) && \
            ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */


/**********************************
*      Generated Code
**********************************/
`$CmdTableEntry`

/**********************************
*     End Of Generated Code
**********************************/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CheckCommand
********************************************************************************
*
* Summary:
*  Looks for a command in the array of supported commands.
*
* Parameters:
*  uint8 command: received command.
*
* Return:
*  Status of command checking procees:
*    `$INSTANCE_NAME`_COMMAND_INVALID (0x00) - the command wasn't found.
*    `$INSTANCE_NAME`_COMMAND_VALID (0x01)   - the command was found.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_CheckCommand(uint8 command) CYREENTRANT
{
    uint8 i;
    uint8 result = `$INSTANCE_NAME`_COMMAND_INVALID;

    /* `$INSTANCE_NAME`_NUM_COMMANDS is a total number of enabled PM Bus commands
    * (when component opertes as PM Bus Slave) and dened custom command.
    */
    for(i = 0u; i < `$INSTANCE_NAME`_NUM_COMMANDS; i++)
    {
        /* Look for a command in the array of supported commands */
        if(command == `$INSTANCE_NAME`_commands[i].command)
        {
            /* Set write pointer to the start of intermediate buffer */
            `$INSTANCE_NAME`_bufferIndex = 0u;                        /* Index shoul always be 0 on atart of a write */
            `$INSTANCE_NAME`_lastReceivedCmd = command;                              /* Store the command */
            `$INSTANCE_NAME`_bufferSize = `$INSTANCE_NAME`_commands[i].dataLength;   /* Store buffer size */
            `$INSTANCE_NAME`_cmdProperties = `$INSTANCE_NAME`_commands[i].cmdProp;
            `$INSTANCE_NAME`_cmdDataPtr = `$INSTANCE_NAME`_commands[i].dataPtr;      /* Store a ptr to the command
                                                                                     * data for a quick access to it
                                                                                     */
            /* Indicate a succesful result */
            result = `$INSTANCE_NAME`_COMMAND_VALID;

            break; /* Command found - no need to continue */
        }
    }

    if(`$INSTANCE_NAME`_COMMAND_INVALID == result)
    {
        /* Reset I2C buffer to avoid erorneus read/writes from/to it */
        `$INSTANCE_NAME`_bufferSize = 0u;
        `$INSTANCE_NAME`_bufferIndex = 0xFFu;
    }

    return(result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SearchCommand
********************************************************************************
*
* Summary:
*  Searching for a command in the array of supported commands. And returns an
*  index of the command in the `$INSTANCE_NAME`_commands[i] array.
*
* Parameters:
*  uint8 command: received command.
*
* Return:
*  Index of the command in array or `$INSTANCE_NAME`_CMD_UNDEFINED if command
*  wasn't found.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SearchCommand(uint8 command) CYREENTRANT
{
    uint8 i;
    uint8 result = `$INSTANCE_NAME`_CMD_UNDEFINED;

    /* `$INSTANCE_NAME`_NUM_COMMANDS is a total number of enabled SM/PM Bus
    * commands (when component opertes as PM Bus Slave) and dened custom command.
    */
    for(i = 0u; i < `$INSTANCE_NAME`_NUM_COMMANDS; i++)
    {
        /* Look for a command in the array of supported commands */
        if(command == `$INSTANCE_NAME`_commands[i].command)
        {
            /* Indicate a succesful result */
            result = i;
            
            break; /* Command found - no need to continue */
        }
    }
    
    /* Need to reset buffer's index as it was affected by 
    * a "Write" part of QUERY command.
    */
    `$INSTANCE_NAME`_bufferIndex = 0u;

    return(result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadAutoHandler
********************************************************************************
*
* Summary:
*  This is a handler for commands that have Read configuration set to "Auto" or
*  "Manual". it simply copied data from Operating Memory (RAM) into I2C buffer.
*  For "Auto" transaction it is called right before  an addres  acknowledge due
*  ReStart. For "Manual" it is in the same phase but after
*  `$INSTANCE_NAME`_ReadManualHandler.
*
* Parameters:
*  None
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ReadAutoHandler(void) CYREENTRANT
{
    uint8 i = 0u;
    uint8 page_offset;

    uint8 tmpProps;
    uint8 tmpCmd;

    if(0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_PAGE_INDEXED))
    {
        page_offset = `$INSTANCE_NAME`_bufferSize * `$INSTANCE_NAME`_cmdPage;
    }
    else
    {
        page_offset = 0u;
    }

    /* If a page-indexed command that marked as "Auto" and with page set to
    * ALL_PAGES was received, and error should be reported as this is invalid case.
    * Same should be done if current page is greater then max number of pages
    * entered in the customizer.
    */
    if(((0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_PAGE_INDEXED)) &&
        (0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_READ_AUTO)) &&
            (`$INSTANCE_NAME`_CMD_ALL_PAGES == `$INSTANCE_NAME`_cmdPage))||
                (`$INSTANCE_NAME`_MAX_PAGES < `$INSTANCE_NAME`_cmdPage))
    {
        /* Protocol error occured */
        `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_INVALID_DATA);
    }
    /* Check if reac config. is not set to "None" */
    else if(0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_READ_MASK))
    {
        /* This switch handlers Write-Auto part of command. Commands that only
        * marked as "Auto" should be present in the switch.
        */
        switch(`$INSTANCE_NAME`_lastReceivedCmd)
        {
            /**********************************
            *      Generated Code
            **********************************/
            `$ReadHandlerCases`

            /**********************************
            *     End Of Generated Code
            **********************************/

            default:
                break;
        }
    }
    else
    {
        /* Reset I2C buffer to avoid erorneus read/writes from/to it */
        `$INSTANCE_NAME`_bufferSize = 0u;
        `$INSTANCE_NAME`_bufferIndex = 0xFFu;
    }
    
    /* to avoid unreferenced variable warning */
    i = i;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadManualHandler
********************************************************************************
*
* Summary:
*  Fills transaction queue with all user required data for a "Manual" handling
*  of SM/PM Bus transaction.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ReadManualHandler(void) CYREENTRANT
{
    uint8 i;
    uint8 * payloadPtr;  /* A pointer to copy payload without each time access to transaction struct */

    if(`$INSTANCE_NAME`_trActiveCount < `$INSTANCE_NAME`_TRANS_QUEUE_SIZE)
    {
        /* Copy all the data from the last transaction into the trasaction struct */
        `$INSTANCE_NAME`_transactionData[0u].read = 1u;                            /* This is a Write transaction */
        `$INSTANCE_NAME`_transactionData[0u].commandCode = `$INSTANCE_NAME`_lastReceivedCmd;
        `$INSTANCE_NAME`_transactionData[0u].page = `$INSTANCE_NAME`_cmdPage;               /* Fill page information */
        `$INSTANCE_NAME`_transactionData[0u].length = `$INSTANCE_NAME`_bufferSize;

        /* Using templorary ptr will save some flash, as a code consuming access to
        * the struct is avoided.
        */
        payloadPtr = &`$INSTANCE_NAME`_transactionData[0u].payload[0u];

        /* Copy all the data into a trasaction structure */
        for(i = 0; i < `$INSTANCE_NAME`_bufferSize; i++)
        {
            payloadPtr[i] = `$INSTANCE_NAME`_buffer[i];
        }

        /* Increment Transaction Queue records count */
        `$INSTANCE_NAME`_trActiveCount++;
    }
    else
    {
        /* Should never get gere because of the blocking nature
        * of both Read and Write transactions.
        */
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteHandler
********************************************************************************
*
* Summary:
*  Handles data processing for a "Write" part of SM/PM Bus transaction.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteHandler(void) CYREENTRANT
{
    uint8 i;
    uint8 * payloadPtr;  /* A pointer to copy payload without each time access to transaction struct */
    uint8 page_offset;

    #if defined(CYDEV_BOOTLOADER_IO_COMP) && ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                                          (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface))
        uint8 byteCount;

    #endif /* if defined(CYDEV_BOOTLOADER_IO_COMP) && \
            ((CYDEV_BOOTLOADER_IO_COMP == CyBtldr_`$INSTANCE_NAME`) || \
                (CYDEV_BOOTLOADER_IO_COMP == CyBtldr_Custom_Interface)) */

    if(0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_PAGE_INDEXED))
    {
        page_offset = `$INSTANCE_NAME`_bufferSize * `$INSTANCE_NAME`_cmdPage;
    }
    else
    {
        page_offset = 0u;
    }

    /* If a page-indexed command that marked as "Auto" and with page set to
    * ALL_PAGES was received, and error should be reported as this is invalid case.
    * Same should be done if current page is greater then max number of pages
    * entered in the customizer. For nonpage-indexed commands page number is 
    * always zero.
    */
    if(((0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_PAGE_INDEXED)) &&
        (0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_WRITE_AUTO)) &&
            (`$INSTANCE_NAME`_CMD_ALL_PAGES == `$INSTANCE_NAME`_cmdPage)) ||
                (`$INSTANCE_NAME`_MAX_PAGES < `$INSTANCE_NAME`_cmdPage))
    {
        /* Protocol error occured */
        `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_INVALID_DATA);
    }
        /* This is a check for a munual command handling property. If it passsed then
    *  fill the transaction queue for user.
    */
    else if(0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_WRITE_MANUAL))
    {
        if(`$INSTANCE_NAME`_trActiveCount < `$INSTANCE_NAME`_TRANS_QUEUE_SIZE)
        {
            /* Copy all the data from the last transaction into the trasaction struct */
            `$INSTANCE_NAME`_transactionData[0u].read = 0u;                          /* This is a Write transaction */
            `$INSTANCE_NAME`_transactionData[0u].commandCode = `$INSTANCE_NAME`_lastReceivedCmd;
            `$INSTANCE_NAME`_transactionData[0u].page = `$INSTANCE_NAME`_cmdPage;
            `$INSTANCE_NAME`_transactionData[0u].length = `$INSTANCE_NAME`_bufferSize;

            /* Using templorary ptr will save some flash, as a code consuming access to
            * the struct is avoided.
            */
            payloadPtr = &`$INSTANCE_NAME`_transactionData[0u].payload[0u];

            /* Copy all the data into a trasaction structure */
            for(i = 0u; i < `$INSTANCE_NAME`_bufferSize; i++)
            {
                payloadPtr[i] = `$INSTANCE_NAME`_buffer[i];
            }

            /* Increment Transaction Queue records count */
            `$INSTANCE_NAME`_trActiveCount++;
        }
        else
        {
            /* Should never get here */
        }

        /* Disable interrupt and wait for `$INSTANCE_NAME`_CompleteTransaction() */
        `$INSTANCE_NAME`_I2C_DisableInt();
    }
    else if(0u != (`$INSTANCE_NAME`_cmdProperties & `$INSTANCE_NAME`_CMD_PROP_WRITE_MASK))
    {
        /* This switch handlers Write-Auto part of command. Commands that only
        * marked as "Auto" should be present in the switch.
        */
        switch(`$INSTANCE_NAME`_lastReceivedCmd)
        {
            /**********************************
            *      Generated Code
            **********************************/
            `$WriteHandlerCases`
            /**********************************
            *     End Of Generated Code
            **********************************/

            default:
                break;
        }
    }
    else
    {
        /* Data should be ignored */
    }
}


/* [] END OF FILE */
