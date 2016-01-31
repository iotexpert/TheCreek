/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides source code for API of SM/PM Bus Slave component.
*
*******************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include "`$INSTANCE_NAME`_cmd.h"


/* User section for additional include libraries and variable declarations */
/*`#START LIB_INCLUDE_REGION`*/

/*`#END`*/


/**********************************
*      Internal functions
**********************************/
static uint16 `$INSTANCE_NAME`_CrcCalc(void) CYREENTRANT;
static void `$INSTANCE_NAME`_CrcCalcByte(uint8 newByte) CYREENTRANT;

/* This one can't be 'static' as it is used by SM/PM Bus timeout ISR */
void `$INSTANCE_NAME`_ResetBus(void) CYREENTRANT;


/**********************************
*      Global Variables
**********************************/

/* This is "Operating Memory Register Store" (RAM) */
`$INSTANCE_NAME`_REGS `$INSTANCE_NAME`_regs;

/* An array structure that contains data from last Write transaction.
* Or it acts as a placve holder for a data for a Read transaction.
* Currently it has only one element but in future releases it can be extended.
*/
`$INSTANCE_NAME`_TRANSACTION_STRUCT `$INSTANCE_NAME`_transactionData[`$INSTANCE_NAME`_TRANS_QUEUE_SIZE];

/* Defines number of active manual transaction records in Transaction Queue */
volatile uint8 `$INSTANCE_NAME`_trActiveCount = 0u;

/* Global variables is required to store SMB Alert mode and Alert Response
* Address if SMBALERT# pin is enabled.
*/
#if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)
    volatile uint8 `$INSTANCE_NAME`_smbAlertMode;
#endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */
volatile uint8 `$INSTANCE_NAME`_alertResponseAddress = 0u;

/* User section for variable declarations */
/*`#START VAR_DECLARATIONS_REGION`*/

/*`#END`*/


/**********************************
*      Eexternal variables
**********************************/
extern volatile uint8 `$INSTANCE_NAME`_I2C_slStatus;      /* Slave Status byte */
extern volatile uint8 `$INSTANCE_NAME`_I2C_state;         /* Current state of I2C state machine */
extern volatile uint8 `$INSTANCE_NAME`_cmdReceived;       /* Indicates if a command was received in this transaction */


/**********************************
*    Internal variables (static)
**********************************/
static uint16 `$INSTANCE_NAME`_crc;                        /* Store intermediate CRC */
static uint8 `$INSTANCE_NAME`_initVar = 0u;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Initializes SM/PM Bus Component with initial values derived from customizer.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Initialize I2C */
    `$INSTANCE_NAME`_I2C_Init();

    #if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)

        /* When SMBALERT# pin is exposed it is required to deassert it on a startup */
        `$INSTANCE_NAME`_SetSmbAlert(`$INSTANCE_NAME`_SMBALERT_DEASSERT);
        
        `$INSTANCE_NAME`_SetSmbAlertMode(`$INSTANCE_NAME`_SMB_ALERT_MODE_INIT);
        
        /* Also tell slave to recognize additional address - Alert Response Address */
        `$INSTANCE_NAME`_SetAlertResponseAddress(`$INSTANCE_NAME`_ALERT_REPONSE_ADDR);

    #endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */

    /* The ISRs were initialized in `$INSTANCE_NAME`_I2C_Init and now it is
    * required to override I2C ISRs with ISRs that will manage SM/PM Bus
    * transactions.
    */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);
    CyIntSetVector(`$INSTANCE_NAME`_I2C_TMOUT_ISR_NUMBER, `$INSTANCE_NAME`_TIMEOUT_ISR);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables SM/PM Bus Component operation.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    /* Restore defalt parameters */
    (void) `$INSTANCE_NAME`_RestoreDefaultAll();

    /* Enable I2C. This will also enable SM/PM Bus interrupts. */
    `$INSTANCE_NAME`_I2C_Enable();

    `$INSTANCE_NAME`_EnableTimeoutInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the SM/PM Bus component.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified
*  on first function call.
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{

    if(0u == `$INSTANCE_NAME`_initVar)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    /* Enable component */
    `$INSTANCE_NAME`_Enable();
    /* Enable interrupts */
    `$INSTANCE_NAME`_EnableInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables SM/PM Bus Component operation.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void  `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
    /* I2C Stop should do all what is required to stop SM/PM Bus */
    `$INSTANCE_NAME`_I2C_Stop();
    
    `$INSTANCE_NAME`_DisableTimeoutInt();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  This function is implemented as macro in `$INSTANCE_NAME`.h file.
*  Enables I2C interrupt. Interrupts are required for most operations.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Note:
*  This function is implemented as a macro.
*
*******************************************************************************/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  This function is implemented as macro in `$INSTANCE_NAME`.h file.
*  Disables I2C interrupts. Normally this function is not required since the
*  Stop function disables the interrupt. If the I2C interrupt is disabled while
*  the I2C master is still running, it may cause the I2C bus to lock up.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Side Effects:
*  If the I2C interrupt is disabled and the master is addressing the current
*  slave, the bus will be locked until the interrupt is re-enabled.
*
* Note:
*  This function is implemented as a macro.
*
*******************************************************************************/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAddress
********************************************************************************
*
* Summary:
*  This function sets the SM/PM Bus slave address.
*
* Parameters:
*  uint8 address: SM/PM Bus slave address for the primary device. This value
*                 can be any address between 0 and 127 (0x00 to 0x7F). This
*                 address is the 7-bit right-justified slave address and does
*                 not include the R/W bit.
*
* Return:
*  None
*
* Note:
*  This function is implemented as a macro.
*
*******************************************************************************/


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetNextTransaction
********************************************************************************
*
* Summary:
*  This function returns a pointer to the next transaction record in the
*  transaction queue. If the queue is empty, the function returns NULL. Only
*  Manual Reads and Writes will be returned by this function, as the component
*  will handle any Auto transactions on the queue. In the case of Writes, it is
*  the responsibility of the user firmware servicing the Transaction Queue to
*  copy the "payload" to the register store. In the case of Reads, it is the
*  responsibility of user firmware to update the contents of the variable for
*  this command in the register store. For both, the user must call
*  `$INSTANCE_NAME`_CompleteTransaction() to free the transaction record.
*
*  Note: that for Read transactions, the length and payload fields are not used
*  for most transaction types. The exception to this is Process call, where the
*  Word from the write phase will be stored in the payload field.
*
* Parameters:
*  None
*
* Return:
*  Pointer the next transaction record or NULL if there are no active records.
*
*******************************************************************************/
`$INSTANCE_NAME`_TRANSACTION_STRUCT * `$INSTANCE_NAME`_GetNextTransaction(void)
                                `=ReentrantKeil($INSTANCE_NAME . "_GetNextTransaction")`
{
    /* Alocate local variable for return */
    `$INSTANCE_NAME`_TRANSACTION_STRUCT * tr;

    /* Check if there any active records in Transaction Queue */
    if(`$INSTANCE_NAME`_trActiveCount > 0u)
    {
        /* As currently component has a Transaction Queue size = 1 first
        * record of the queue alwas hold active transaction data.
        */
        tr = &`$INSTANCE_NAME`_transactionData[0u];
    }
    else
    {
        /* No active records in Transaction Queue */
        tr = NULL;
    }

    return(tr);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetTransactionCount
********************************************************************************
*
* Summary:
*  Returns the number of transaction records in the transaction queue.
*
* Parameters:
*  None
*
* Return:
*  Number of records in the transaction queue.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetTransactionCount(void) `=ReentrantKeil($INSTANCE_NAME . "_GetTransactionCount")`
{
    return(`$INSTANCE_NAME`_trActiveCount);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CompleteTransaction
********************************************************************************
*
* Summary:
*  Causes the component to complete the currently pending transaction at the
*  head of the queue. The user firmware transaction handler calls this function
*  after processing a transaction. This alerts the component code to copy the
*  register variable associated with the pending Read transaction from the
*  register store to the I2C transfer buffer so that the transfer may complete.
*  It also advances the queue. Must be called for Reads and Writes.
*
* Parameters:
*  None
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_CompleteTransaction(void) `=ReentrantKeil($INSTANCE_NAME . "_CompleteTransaction")`
{
    /* Check for the read or write transaction */
    if(`$INSTANCE_NAME`_I2C_SM_SL_RD_DATA == `$INSTANCE_NAME`_I2C_state)
    {
        /* This will read the data from Registers store to I2C Buffer */
        `$INSTANCE_NAME`_ReadAutoHandler();

        /* Prepare next opeation to read, get data and place in data register */
        if(`$INSTANCE_NAME`_bufferIndex < `$INSTANCE_NAME`_bufferSize)
        {
            `$INSTANCE_NAME`_I2C_DATA_REG =
               `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_bufferIndex]; /* Load first data byte */
            `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;                    /* ACK and transmit */
            `$INSTANCE_NAME`_bufferIndex++;                           /* Advance to data location */

            /* Set READ activity */
            `$INSTANCE_NAME`_I2C_slStatus |= `$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY;
        }
        else    /* Data overflow */
        {
            /* Out of range, send 0xFF */
            `$INSTANCE_NAME`_I2C_DATA_REG = 0xFFu;
            /* ACK and transmit */
            `$INSTANCE_NAME`_I2C_ACK_AND_TRANSMIT;
            /* Set Read with Overflow */
            `$INSTANCE_NAME`_I2C_slStatus  |= (`$INSTANCE_NAME`_I2C_SSTAT_RD_BUSY |
                `$INSTANCE_NAME`_I2C_SSTAT_RD_ERR_OVFL);
        }
    }
    else
    {
        /* Nothing should be done here for a manual write transactions 
        * and for a special read case - `$INSTANCE_NAME`_BOOTLOAD_READ.
        */
    }

    /* Update number of active records */
    if(`$INSTANCE_NAME`_trActiveCount > 0u)
    {
        `$INSTANCE_NAME`_trActiveCount--;
    }

    /* Enable interrupt as manual hadling of a transaction was done */
    `$INSTANCE_NAME`_I2C_EnableInt();
}


#if (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED)
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetAlertResponseAddress
    ********************************************************************************
    *
    * Summary:
    *  This function sets the I2C slave address where the device will respond when
    *  in Alert Response Address Mode.
    *
    * Parameters:
    *  uint8 address: I2C slave address for Alert Response mode. This value can be
    *                 any address between 0 and 127 (0x00 to 0x7F). This address
    *                 is the 7-bit right-justified slave address and does not
    *                 include the R/W bit.
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_alertResponseAddress - stores device's alert response
    *  address.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetAlertResponseAddress(uint8 address)
                                                           `=ReentrantKeil($INSTANCE_NAME . "_SetAlertResponseAddress")`
    {
        `$INSTANCE_NAME`_alertResponseAddress = address;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetSmbAlert
    ********************************************************************************
    *
    * Summary:
    *  Asserts or de-asserts the SMBALERT# smbalert pin. As long as SMBALERT# is
    *  asserted, the component will respond to master READ's to the Alert Response
    *  Address. The response will be the device's primary slave address. Depending
    *  on the mode setting, the component will automatically de-assert SMBALERT#,
    *  call the SMBus_HandleSmbAlertResponse() API, or do nothing.
    *
    * Parameters:
    *  uint8 assert:
    *   `$INSTANCE_NAME`_SMBALERT_ASSERT   - assert a smbalert pin
    *   `$INSTANCE_NAME`_SMBALERT_DEASSERT - deassert a smbalert pin
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetSmbAlert(uint8 assert) `=ReentrantKeil($INSTANCE_NAME . "_SetSmbAlert")`
    {
        /* SMBALERT# is an active low line so to assert it it is required to
        * set this line to "Low".
        */
        if(`$INSTANCE_NAME`_SMBALERT_ASSERT == assert)
        {
            `$INSTANCE_NAME`_SMB_ALERT_CONTROL_REG = `$INSTANCE_NAME`_STATE_ASSERTED;
        }
        else
        {
            `$INSTANCE_NAME`_SMB_ALERT_CONTROL_REG = `$INSTANCE_NAME`_STATE_DEASSERTED;
        }
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetSmbAlertMode
    ********************************************************************************
    *
    * Summary:
    *  This function determines how the component responds to a SMBus master Read at
    *  the Alert Response Address. When SMBALERT# is asserted, the SMBus master may
    *  broadcast a Read to the global Alert Response Address to determine which
    *  SMBus device on the shared bus has asserted SMBALERT#.
    *
    *  In Auto mode, SMBALERT# is automatically deasserted once the bus master
    *  successfully READ's the Alert Response Address.
    *
    *  In Firmware mode, the component will call the API
    *  `$INSTANCE_NAME_HandleSmbAlertResponse() where user code (in a merge section)
    *  is responsible for deasserting SMBALERT#.
    *
    *  In Do Nothing mode, the component will take no action.
    *
    * Parameters:
    *  uint8 alertMode: A byte that defines SMBALERT pin mode. Possible values are:
    *                   `$INSTANCE_NAME`_DO_NOTHING (0x00)
    *                   `$INSTANCE_NAME`_AUTO_MODE (0x01)
    *                   `$INSTANCE_NAME`_FIRMWARE_MODE (0x02)
    *
    * Return:
    *  None
    *
    * Global Variables:
    *  `$INSTANCE_NAME`_smbAlertMode - stores alert response mode.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetSmbAlertMode(uint8 alertMode) `=ReentrantKeil($INSTANCE_NAME . "_SetSmbAlertMode")`
    {
        `$INSTANCE_NAME`_smbAlertMode = alertMode;
    }


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_HandleSmbAlertResponse
    ********************************************************************************
    *
    * Summary:
    *  This API is called by the component when the host responds to the Alert
    *  Response Address and the SMBALERT Mode is set to FIRMWARE_MODE. This function
    *  contains a merge code section where the user inserts code to run after the
    *  Master has responded. For example, the user might update a status register
    *  and de-assert the SMBALERT# pin.
    *
    * Parameters:
    *  None
    *
    * Return:
    *  None
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_HandleSmbAlertResponse(void) `=ReentrantKeil($INSTANCE_NAME . "_HandleSmbAlertResponse")`
    {
        /* Place your code between "start" and "end" comment blocks */
        /*`#START SMBUS_ALERT_REGION`*/

        /*`#END`*/
    }
#endif /* (0u != `$INSTANCE_NAME`_SMB_ALERT_PIN_ENABLED) */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_GetReceiveByteResponse
********************************************************************************
*
* Summary:
*  This function is called by the I2C ISR to determine the response byte when
*  it detects a "Receive Byte" protocol request. This function includes a merge
*  code section where the user may insert their code to override the default
*  return value of this function - which is 0xFF. This function will be called
*  in ISR context. Therefore, user merge code must be fast, non-blocking, and
*  may only call re-entrant functions.
*
* Parameters:
*  None
*
* Return:
*  User-Specified status byte: `$INSTANCE_NAME`_RET_UNDEFINED(0xFF): default
*                                return value.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_GetReceiveByteResponse(void) `=ReentrantKeil($INSTANCE_NAME . "_GetReceiveByteResponse")`
{
    uint8 status = `$INSTANCE_NAME`_RET_UNDEFINED;

    /* Place your code between "start" and "end" comment blocks */
    /*`#START RECEIVE_BYTE_REGION`*/

    /*`#END`*/

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_HandleBusError
********************************************************************************
*
* Summary:
*  This API is called by the component whenever a bus protocol error occurs.
*  Examples of bus errors would be: invalid command, data underflow, and clock
*  stretch violation. This function is only responsible for the aftermath of
*  an error since the component will already handle errors in a deterministic
*  manner. This function is primarily for the purpose of notifying user firmware
*  that an error has occurred. For example, in a PMBus device this would give
*  user firmware an opportunity to set the appropriate error bit in the
*  STATUS_CML register.
*
* Parameters:
*  errorCode - code of occured error
*   Possible values are:
*   `$INSTANCE_NAME`_ERR_READ_FLAG (0x01u)
*   `$INSTANCE_NAME`_ERR_RD_TO_MANY_BYTES (0x02)
*   `$INSTANCE_NAME`_ERR_WR_TO_MANY_BYTES (0x03)
*   `$INSTANCE_NAME`_ERR_UNSUPORTED_CMD (0x04)
*   `$INSTANCE_NAME`_ERR_INVALID_DATA (0x05)
*   `$INSTANCE_NAME`_ERR_TIMEOUT (0x06)
*   `$INSTANCE_NAME`_ERR_WR_TO_FEW_BYTES (0x07u)
*
* Return:
*  None
*
*******************************************************************************/
void `$INSTANCE_NAME`_HandleBusError(uint8 errorCode) `=ReentrantKeil($INSTANCE_NAME . "_HandleBusError")`
{
    /* Place your code between "start" and "end" comment blocks */
    /*`#START BUS_ERROR_REGION`*/

    /*`#END`*/

    /* To suppress "unreferenced variable" warning" */
    if(0u == errorCode)
    {
        errorCode = errorCode;
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StoreUserAll
********************************************************************************
*
* Summary:
*  This function saves the RAM Register Store to the User Register Store in
*  Flash. The CRC field in the Register Store data structure is recalculated and
*  updated prior to the save.
*
* Parameters:
*  flashRegs - a pointer to a location where Register Store (RAM) should be 
*              stored
*
* Return:
*  Status: CYRET_xxx value
*          CYRET_SUCCESS
*          CYRET_MEMORY
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_StoreUserAll(char * flashRegs) `=ReentrantKeil($INSTANCE_NAME . "_StoreUserAll")`
{
    uint8 result = CYRET_SUCCESS;
    uint8 interruptState;
    
    /* Place your variable declarations between "start" and "end" comment blocks */
    /*`#START STORE_USER_ALL_DECL_REGION`*/

    /*`#END`*/
    
    /* Need to disable interrupts to not damage Registers Store that may
    * happen if an "Auto" transaction ocuurs while storing.
    */
    interruptState = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_regs.FLASH_CRC = `$INSTANCE_NAME`_CrcCalc();
    
    /* Implement your metod of storing Operating Memory to Flash in User 
    * section below.
    */
    /* Place your code between "start" and "end" comment blocks */
    /*`#START STORE_USER_ALL_REGION`*/

    /*`#END`*/
    
    CyExitCriticalSection(interruptState);
    
    /* To suppress "unreferenced variable" warning" */
    if(NULL == flashRegs)
    {
        flashRegs = flashRegs;
    }
    
    return(result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreUserAll
********************************************************************************
*
* Summary:
*  This function verifies the CRC field of the User Register Store and then
*  copies the contents of the User Register Store to the RAM Register Store.
*
* Parameters:
*  flashRegs - a pointer to a location where Register Store (Flash) is 
*              stored.
*
* Return:
*  Status: CYRET_xxx value
*          CYRET_SUCCESS  - CRC matches and Operating Memory was updated from
*                           user Register Store (Flash).
*          CYRET_BAD_DATA - in case if CRC doesn't match.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_RestoreUserAll(char * flashRegs) `=ReentrantKeil($INSTANCE_NAME . "_RestoreUserAll")`
{
    uint8 result = CYRET_BAD_DATA;
    uint8 interruptState;
    
    /* Need to disable interrupts to not damage Registers Store that may
    * happen if an "Auto" transaction ocuurs while restoring.
    */
    interruptState = CyEnterCriticalSection();
    
    /* Restore Operating memory from Flash */
    memcpy((char*) &`$INSTANCE_NAME`_regs, flashRegs, `$INSTANCE_NAME`_REGS_SIZE);    

    /* If the CRC matches then indicate success. */
    if (`$INSTANCE_NAME`_regs.FLASH_CRC == `$INSTANCE_NAME`_CrcCalc())
    {
        result = CYRET_SUCCESS;
    }
    
    CyExitCriticalSection(interruptState);

    return(result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreDefaultAll
********************************************************************************
*
* Summary:
*  This function verifies the signature field of the Default Register Store and
*  then copies the contents of the Default Register Store to the RAM Register
*  Store.
*
* Parameters:
*  None
*
* Return:
*  Status: CYRET_xxx value
*          CYRET_SUCCESS
*          CYRET_BAD_DATA
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_RestoreDefaultAll(void) `=ReentrantKeil($INSTANCE_NAME . "_RestoreDefaultAll")`
{
    uint8 result = CYRET_BAD_DATA;

    /* Check signature for validness */
    if(`$INSTANCE_NAME`_SIGNATURE == `$INSTANCE_NAME`_regsDefault.SMBUS_REGS_SIG)
    {
        /* Signature is valid so default register store can be copied into RAM */
        memcpy((char*)&`$INSTANCE_NAME`_regs, (char*)&`$INSTANCE_NAME`_regsDefault, sizeof(`$INSTANCE_NAME`_regs));

        result = CYRET_SUCCESS;
    }

    return(result);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_StoreComponentAll
********************************************************************************
*
* Summary:
*  The user calls this function to update the parameters of other components in
*  the system with the current PMBus settings. Because this action is very
*  application specific, this function consists almost entirely of a merge
*  section. The only component provided firmware is a return value variable
*  (retval) which is initialized to CYRET_SUCCESS and returned at the end of the
*  function. The rest of the function must be user provided.
*
* Parameters:
*  None
*
* Return:
*  Status: CYRET_xxx value
*          CYRET_SUCCESS
*          Or other user determined non-SUCCESS
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_StoreComponentAll(void) `=ReentrantKeil($INSTANCE_NAME . "_StoreComponentAll")`
{
    uint8 status = CYRET_SUCCESS;

    /* Below are refferences to commands data of Operating Memory (RAM).
    * They should be used to implement Component Register Store concept.
    * Each parameter can be individually selected and assigned with 
    * desired value(s).
    
      `$StoreComponentAllVar` 
    */

    /* Place your code between "start" and "end" comment blocks */
    /*`#START STORE_COMPONENT_ALL_REGION`*/

    /*`#END`*/

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_RestoreComponentAll
********************************************************************************
*
* Summary:
*  The user calls this function to update the PMBus Operating Register Store
*  with the current configuration parameters of other components in the system.
*  Because this action is very application specific, this function consists
*  almost entirely of a merge section. The only component provided firmware is
*  a return value variable (retval) which is initialized to CYRET_SUCCESS and
*  returned at the end of the function. The rest of the function must be user
*  provided.
*
* Parameters:
*  None
*
* Return:
*  Status: CYRET_xxx value
*          CYRET_SUCCESS
*          Or other user determined non-SUCCESS
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_RestoreComponentAll(void) `=ReentrantKeil($INSTANCE_NAME . "RestoreComponentAll")`
{
    uint8 status = CYRET_SUCCESS;

    /* Below are refferences to commands data of Operating Memory (RAM).
    * They should be used to implement Component Register Store concept.
    * Each parameter can be individually selected and assigned with 
    * desired value(s).
    
      `$StoreComponentAllVar` 
    */
    
    /* Place your code between "start" and "end" comment blocks */
    /*`#START RESTORE_COMPONENT_ALL_REGION`*/

    /*`#END`*/

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Lin11ToFloat
********************************************************************************
*
* Summary:
*  This function converts the argument "linear11" to floating point and returns
*  it.
*
* Parameters:
*  uint16 linear11: a number in LINEAR11 format
*
* Return:
*  The linear11 parameter converted to floating point
*
*******************************************************************************/
float `$INSTANCE_NAME`_Lin11ToFloat(uint16 linear11) CYREENTRANT
{
    float retval;
    uint16 exponent;
    uint16 mantissa;
    uint8 manSign;
    uint8 expSign;

    /* Disassemble linear11 into 4 parts */
    expSign  = linear11 & 0x8000u ? 1u : 0u;
    exponent = (uint16) ((linear11 & 0x7800u) >> 11u);
    manSign  = (linear11 & 0x0400u) ? 1u : 0u;
    mantissa = (linear11 & 0x03FFu);

    /* Start by tossing mantissa into a float return value */
    retval = (float) mantissa;

    /* If negative exponent, decrease retval */
    if(expSign)
    {
        exponent = (exponent ^ 0x0Fu) + 1u; /* convert (-) exponent to (+)  */
        retval = retval / (1u << exponent);
    }
    /* Else positive exponent, increase retval */
    else
    {
        retval *= (1u << exponent);
    }

    if(manSign)
    {
        retval = -retval;
    }

    return(retval);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_FloatToLin11
********************************************************************************
*
* Summary:
*  This function takes the argument "floatvar" (a floating point number) and
*  converts it to a 16-bit LINEAR11 value (11-bit mantissa + 5-bit exponent),
*  which it returns.
*
* Parameters:
*  float floatvar: a floating point number
*
* Return:
*  floatvar converted to LINEAR11
*
* Theory:
*  linear11 has signed 5-bit exponent (2^15 to 2^-16)  and signed 11-bit
*  mantissa (sXX XXXX XXXX).
*
*  32-bit floating point IEEE numbers 8-bit signed exponent, 23-bit mantissa
*   S EEEEEEEE FFFFFFFFFFFFFFFFFFFFFFF
*   0 1      8 9                    31
*
*  If E==255 and F!=0, then value = "not a number"
*     E==255 and F==0, then value = +/- Infinity, depending on S
*     0<E<255        then value = 2^(E-127) * (1.F) (implicit leading 1)
*     E==0 and F!=0, then value = 2^( -126) * (0.F)
*     E==0 and F==0, then value = +/- 0.
*
*******************************************************************************/
uint16 `$INSTANCE_NAME`_FloatToLin11(float floatvar) CYREENTRANT
{
    int16 mantissa;
    int8 exponent;
    uint16 retval;
    uint32 var32;
    
    #if defined(__GNUC__)
        
        /* This is requred to avoid a GCC warning caused by expression 
        * "var32 = *(uint32*)(&floatvar);". 
        */
        char * pSrc = (char*)(&floatvar);
        char * pDst = (char*)(&var32);

        memcpy(pDst, pSrc, sizeof(float));
    
    #else /* If compiler is not GCC */
    
        /* var32 = floating point number loaded as raw uint32 */
        var32 = *(uint32*)(&floatvar);       /* should auto handle endian issues */

    #endif /* __GNUC__ */
    
    if(var32 == 0u)
    {
        /* 0.0 is a special-case floating point number */
        retval = 0u;
    }
    else
    {
        /* Get top 15 bits of mantissa and restore suppressed leading "1" to make
        * 16 bits, then decrease to 10 bits.
        */
        mantissa = (int16)((var32 & 0x007FFF00) >> 8u);     /* top 15-bits mantissa */
        mantissa >>= 6u;                                    /* 10-bit unsigned */
        mantissa += 0x0200u;                                /* Add suppressed MSbit */

        if (floatvar < 0u)
        {
            mantissa += 0xFC00u;                            /* Add mantissa sign (extended) */
        }

        /* Isolate 8-bit signed exponent */
        exponent = (int8)(var32 >> 23u);                    /* leading mantissa sign also discarded */
        exponent -= 127u;                                   /* Convert to "true" signed exponent 1.11 */

        /* Linear 11 assumes decimal to the far right, so need to decrease exponent
        * by 2^9 (i.e. move decimal 9 places right from 1.x xxxx xxxx to 1x xxxx xxxx.
        */
        exponent -= 9u;

        /* If exponent is < -16, increase it to avoid exceeding 5-bit signed minimum
        * by right-shifting mantissa (possibly throwing away LS bits).
        */
        while (exponent < `$INSTANCE_NAME`_NEG_EXP_MIN)
        {
            ++exponent;
            mantissa >>= 1u;
        }

        /* Assembly linear11 from exponent and mantissa */
        retval  = ((uint16)exponent << 11u) & 0xF800u;  /*  5-bit signed exponent */
        retval += (mantissa <<  0u) & 0x07FF;            /* 11-bit signed mantissa */
    }

    return(retval);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Lin16ToFloat
********************************************************************************
*
* Summary:
*  This function converts the argument "linear16" to floating point and returns
*  it. The argument Linear16 contains the mantissa. The argument inExponent is
*  the 5-bit 2's complement exponent to use in the conversion.
*
* Parameters:
*  uint16 linear16: the 16-bit mantissa of a LINEAR16 number
*  int8 inExponent: the 5-bit exponent of a LINEAR16 number. Packed in the
*                   lower 5 bits. 2's Complement
*
* Return:
*  The parameters converted to floating point.
*
*******************************************************************************/
float `$INSTANCE_NAME`_Lin16ToFloat(uint16 linear16, int8 inExponent) CYREENTRANT
{
    float retval;
    uint16 exponent;
    uint16 mantissa;
    uint8 expSign;

    /* Disassemble linear11 into 4 parts */
    expSign  = inExponent & 0x10u ? 1u : 0u;  /* Get exponent's sign bit  */
    exponent = inExponent & 0x0Fu;          /* Get 4-bit exponent       */
    mantissa = linear16;

    /* Start by tossing mantissa into a float return value */
    retval = (float) mantissa;

    /* If negative exponent, decrease retval */
    if(expSign)
    {
        exponent = (exponent ^ 0x0Fu) + 1u; /* convert (-) exponent to (+)  */
        retval /= (1u << exponent);
    }
    /* if positive exponent, increase retval */
    else
    {
        retval *= (1u << exponent);
    }

   return(retval);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  This function takes the argument "floatvar" (a floating point number) and
*  converts it to a 16-bit LINEAR16 value (16-bit mantissa), which it returns.
*  The argument outExponent is the 5-bit 2's complement exponent to use in the
*  conversion.
*
* Parameters:
*  float floatvar:   a floating point number to be converted to LINEAR16
*  int8 outExponent: user provided 5-bit exponent to use in the conversion.
*
* Return:
*  The parameters converted to LINEAR16
*
*******************************************************************************/
uint16  `$INSTANCE_NAME`_FloatToLin16(float floatvar, int8 outExponent) CYREENTRANT
{
    uint16 mantissa;
    int8 exponent;
    uint8 roundoff;
    uint32 var32;

    #if defined(__GNUC__)
        
        /* This is requred to avoid a GCC warning caused by expression 
        * "var32 = *(uint32*)(&floatvar);". 
        */
        char * pSrc = (char*)(&floatvar);
        char * pDst = (char*)(&var32);

        memcpy(pDst, pSrc, sizeof(float));
    
    #else /* If compiler is not GCC */
    
        /* var32 = floating point number loaded as raw uint32 */
        var32 = *(uint32*)(&floatvar);       /* should auto handle endian issues */

    #endif /* __GNUC__ */

    /* 0.0 is a special-case floating point number */
    if (var32 == 0u)
    {
        mantissa = 0u;
    }
    else
    {
        /* Get top 15 bits of mantissa and restore suppressed leading "1" to make
        * 16 bits, then decrease to 10 bits.
        **/
        mantissa = (int16)((var32 & 0x007FFF00) >> 8u);             /* top 15-bits mantissa */
        mantissa += 0x8000u;                                        /* Add suppressed MSbit */

        /* Isolate 8-bit signed exponent */
        exponent = (int8)(var32 >> 23u);                            /* leading mantissa sign also discarded */
        exponent -= 127u;                                           /* Convert to "true" signed exponent 1.11 */

        /* Linear 16 assumes decimal to the far right, so need to decrease exponent
        *  by 2^15 (i.e. move decimal 15 places right from 1.xxx xxxx xxxx xxxx
        *                                               to  1xxx xxxx xxxx xxxx.
        */
        exponent -=15u;

        /* Adjust mantissa to render caller-specified exponent */
        roundoff = 0u;
        while(exponent > outExponent)
        {
            --exponent;
            mantissa <<= 1u;                                        /* WARNING: may trash value if bit exponent */
        }

        while(exponent < outExponent)
        {
            ++exponent;
            roundoff = mantissa & 0x0001u;
            mantissa >>= 1u;                                        /* WARNING: may lose precision   */
        }

        if(mantissa != 0xFFFFu)
        {
            mantissa += roundoff;
        }
    }
    return(mantissa);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CrcCalc()
********************************************************************************
*
* Summary:
*  Calculate CRC over the Register Store in RAM.
*
* Parameters:
*  None
*
* Return:
*  Copy of updated CRC value (can be used or discarded)
*
* Theory:
*  Using non-zero CRC Seed avoids the all-zeros codeword. Changing or not
*  changing (by simply decrementing) the seed with a new firmware release
*  provides a method to re-initialize (or keep unchanged) the EEPROM when the
*  new firmware is initially run.
*
*******************************************************************************/
static uint16 `$INSTANCE_NAME`_CrcCalc(void) CYREENTRANT
{
    uint8 * pdat;
    uint16 cnt;
    int16 size; 

    pdat = (uint8 *) &`$INSTANCE_NAME`_regs;
    
    /* Calculate size of Register store minus CRC. */
    size = sizeof(`$INSTANCE_NAME`_regs) - 2u;
    
    `$INSTANCE_NAME`_crc = `$INSTANCE_NAME`_CRC_SEED;

    /* Calculate CRC using each byte from register store. Don't use last two bytes
    * from register store as they hold checksum for this register store.
    */
    for(cnt = 0u; cnt < (size - 2u); ++cnt)
    {
        `$INSTANCE_NAME`_CrcCalcByte(*pdat++);
    }

    return (`$INSTANCE_NAME`_crc);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_CrcCalcByte()
********************************************************************************
*
* Summary:
*  Update the CRC based on supplied Byte using CCITT polynomial
*       X^16 + X^12 + X^5 + X^0
*
*  Basic test cases:
*  If CRC = 0, then processing 0x01 yields 0x1021, or X^12+X^5+X^0
*  If CRC = 0, then processing 0x01,0x10,0x21, yields 0 (or no change)
*
* Parameters:
*  uint8 newByte: new 8 bits, MSB = first bit shifted, LSB = final bit shifted
*
* Return:
*  None
*
*  Note:
*   The efficacy of this method is highly dependent on the generator
*   polynomial coefficients.
*
*******************************************************************************/
static void `$INSTANCE_NAME`_CrcCalcByte(uint8 newByte) CYREENTRANT
{
    `$INSTANCE_NAME`_crc = (uint8) (`$INSTANCE_NAME`_crc >> `$INSTANCE_NAME`_CRC_BYTE_SHIFT) |
        (`$INSTANCE_NAME`_crc << `$INSTANCE_NAME`_CRC_BYTE_SHIFT);
    `$INSTANCE_NAME`_crc ^= newByte;
    `$INSTANCE_NAME`_crc ^= (uint8) (`$INSTANCE_NAME`_crc & `$INSTANCE_NAME`_CRC_BYTE_MASK) >> 4u;
    `$INSTANCE_NAME`_crc ^= (`$INSTANCE_NAME`_crc << 12u);
    `$INSTANCE_NAME`_crc ^= (`$INSTANCE_NAME`_crc & `$INSTANCE_NAME`_CRC_BYTE_MASK) << 5u;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ResetBus
********************************************************************************
*
* Summary:
*  Resets SM/PM Bus.
*
* Parameters:
*  None
*
* Return:
*  None
*
* Reentrant:
*  No
*
*******************************************************************************/
void `$INSTANCE_NAME`_ResetBus(void) CYREENTRANT
{
    `$INSTANCE_NAME`_Stop();        /* Reset SM/PM Bus */
    
    /* Inform user that timeout occured */
    `$INSTANCE_NAME`_HandleBusError(`$INSTANCE_NAME`_ERR_TIMEOUT);
    
    /* `#START `$INSTANCE_NAME`_TMOUT_ISR_BEFORE_BUF_RESET` */

    /* `#END` */
    
    `$INSTANCE_NAME`_Enable();      /* Enable component */
    
    /* Reset all variables */
    `$INSTANCE_NAME`_I2C_state = `$INSTANCE_NAME`_I2C_SM_IDLE;         /* Return to IDLE */
    `$INSTANCE_NAME`_cmdReceived = 0u;                                 /* Indicate that there is no command received */
    `$INSTANCE_NAME`_lastReceivedCmd = `$INSTANCE_NAME`_CMD_UNDEFINED; /* Invalidate a stored command code */
    `$INSTANCE_NAME`_I2C_DISABLE_INT_ON_STOP;
    `$INSTANCE_NAME`_trActiveCount = 0u;                               /* Invalideate data in Transaction Queue */
    
    /* Enable interrupts */
    `$INSTANCE_NAME`_EnableInt();
}


/* [] END OF FILE */
