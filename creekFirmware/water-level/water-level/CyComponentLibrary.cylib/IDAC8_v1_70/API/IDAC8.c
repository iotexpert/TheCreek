/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file provides the source code to the API for the 8-bit Current 
*    DAC (IDAC8) User Module.
*
*   Note:
*     
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"   

#if (CY_PSOC5_ES1)
#include <CyLib.h>
#endif /* CY_PSOC5_ES1 */

uint8 `$INSTANCE_NAME`_initVar = 0u;


static `$INSTANCE_NAME`_backupStruct  `$INSTANCE_NAME`_backup;

/* Variable to decide whether or not to restore control register in Enable()
   API. This valid only for PSoC5 ES1 */
#if (CY_PSOC5_ES1)
uint8 `$INSTANCE_NAME`_restoreReg = 0u;
uint8 `$INSTANCE_NAME`_intrStatus = 0u;
#endif /* CY_PSOC5_ES1 */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
* Summary:
*  Initialize to the schematic state.
* 
* Parameters:  
*  void:  
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects: 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    `$INSTANCE_NAME`_CR0 = (`$INSTANCE_NAME`_MODE_I | `$INSTANCE_NAME`_DEFAULT_RANGE )  ;    

    /* Set default data source */
    if(`$INSTANCE_NAME`_DEFAULT_DATA_SRC != 0u )    
    {
        `$INSTANCE_NAME`_CR1 = (`$INSTANCE_NAME`_DEFAULT_CNTL | `$INSTANCE_NAME`_DACBUS_ENABLE | 
                                `$INSTANCE_NAME`_DEFAULT_POLARITY) ;   
    }
    else
    {
        `$INSTANCE_NAME`_CR1 = (`$INSTANCE_NAME`_DEFAULT_CNTL | `$INSTANCE_NAME`_DACBUS_DISABLE | 
                                `$INSTANCE_NAME`_DEFAULT_POLARITY) ;   
    } 
    
    /* Set default strobe mode */
    if(`$INSTANCE_NAME`_DEFAULT_STRB != 0u)
    {
        `$INSTANCE_NAME`_Strobe |= `$INSTANCE_NAME`_STRB_EN ;
    }
    
    /* Set default speed */
    `$INSTANCE_NAME`_SetSpeed(`$INSTANCE_NAME`_DEFAULT_SPEED); 
    
    /* Set proper DAC trim */
    `$INSTANCE_NAME`_DacTrim();   
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
* Summary:
*     Enable the IDAC8
* 
* Parameters:  
*  void:  
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects: 
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    `$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
    `$INSTANCE_NAME`_STBY_PWRMGR |= `$INSTANCE_NAME`_STBY_PWR_EN;

    /* This is to restore the value of register CR0 which is saved 
      in prior to the modification in stop() API */
    #if (CY_PSOC5_ES1)
    if(`$INSTANCE_NAME`_restoreReg == 1u)
    {
        `$INSTANCE_NAME`_CR0 = `$INSTANCE_NAME`_backup.DACCR0Reg;

        /* Clear the flag */
        `$INSTANCE_NAME`_restoreReg = 0u;
    }
    #endif /* CY_PSOC5_ES1 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Set power level then turn on IDAC8.
*
* Parameters:  
*  power:   Sets power level between off (0) and (3) high power
*
* Return: 
*  (void) 
*
* Global variables:
*  `$INSTANCE_NAME`_initVar: Is modified when this function is called for 
*   the first time. Is used to ensure that initialization happens only once.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) 
{
    /* Hardware initiazation only needs to occur the first time */
    if ( `$INSTANCE_NAME`_initVar == 0u)  
    {     
        `$INSTANCE_NAME`_Init();
       
        `$INSTANCE_NAME`_initVar = 1;
    }  
   
    /* Enable power to DAC */
    `$INSTANCE_NAME`_Enable();

    /* Set default value */
    `$INSTANCE_NAME`_SetValue(`$INSTANCE_NAME`_DEFAULT_DATA);

}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Powers down IDAC8 to lowest power state.
*
* Parameters:  
*   (void)
*
* Return: 
*  (void)
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
   /* Disble power to DAC */
   `$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
   `$INSTANCE_NAME`_STBY_PWRMGR &= ~`$INSTANCE_NAME`_STBY_PWR_EN;
   
   #if (CY_PSOC5_ES1)
   
        /* Set the global variable  */
        `$INSTANCE_NAME`_restoreReg = 1u;

        /* Save the control register and then Clear it. */
        `$INSTANCE_NAME`_backup.DACCR0Reg = `$INSTANCE_NAME`_CR0;
        `$INSTANCE_NAME`_CR0 = (`$INSTANCE_NAME`_MODE_I | `$INSTANCE_NAME`_RANGE_3 | `$INSTANCE_NAME`_HS_HIGHSPEED);
    #endif /* CY_PSOC5_ES1 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSpeed
********************************************************************************
* Summary:
*  Set DAC speed
*
* Parameters:  
*  power:   Sets speed value
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetSpeed(uint8 speed) `=ReentrantKeil($INSTANCE_NAME . "_SetSpeed")`
{
   /* Clear power mask then write in new value */
   `$INSTANCE_NAME`_CR0 &= ~`$INSTANCE_NAME`_HS_MASK ;    
   `$INSTANCE_NAME`_CR0 |=  ( speed & `$INSTANCE_NAME`_HS_MASK) ;    
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetPolarity
********************************************************************************
* Summary:
*  Sets IDAC to Sink or Source current
*
* Parameters:  
*  Polarity: Sets the IDAC to Sink or Source
*   0x00 - Source
*   0x04 - Sink
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetPolarity(uint8 polarity) `=ReentrantKeil($INSTANCE_NAME . "_SetPolarity")`
{
    `$INSTANCE_NAME`_CR1 &= ~`$INSTANCE_NAME`_IDIR_MASK;                /* clear polarity bit */
    `$INSTANCE_NAME`_CR1 |= (polarity & `$INSTANCE_NAME`_IDIR_MASK);    /* set new value */
    
    `$INSTANCE_NAME`_DacTrim();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetRange
********************************************************************************
* Summary:
*  Set current range
*
* Parameters:  
*  Range:  Sets on of four valid ranges.
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetRange(uint8 range) `=ReentrantKeil($INSTANCE_NAME . "_SetRange")`
{
   `$INSTANCE_NAME`_CR0 &= ~`$INSTANCE_NAME`_RANGE_MASK ;       /* Clear existing mode */
   `$INSTANCE_NAME`_CR0 |= ( range & `$INSTANCE_NAME`_RANGE_MASK );  /*  Set Range  */
   `$INSTANCE_NAME`_DacTrim();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetValue
********************************************************************************
* Summary:
*  Set DAC value
*
* Parameters:  
*  value:  Sets DAC value between 0 and 255.
*
* Return: 
*  (void) 
*
* Theory: 
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetValue(uint8 value) `=ReentrantKeil($INSTANCE_NAME . "_SetValue")`
{

   #if (CY_PSOC5_ES1)
       `$INSTANCE_NAME`_intrStatus = CyEnterCriticalSection();
   #endif /* CY_PSOC5_ES1 */

   `$INSTANCE_NAME`_Data = value;         /*  Set Value  */
   
   /* PSOC3 silicons earlier to ES3 require a double write */
   #if (CY_PSOC3_ES2 ||CY_PSOC5_ES1 )
       `$INSTANCE_NAME`_Data = value;
   #endif /* CY_PSOC3_ES2 ||CY_PSOC5_ES1  */
   
   #if (CY_PSOC5_ES1)
       CyExitCriticalSection(`$INSTANCE_NAME`_intrStatus);
   #endif /* CY_PSOC5_ES1 */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DacTrim
********************************************************************************
* Summary:
*  Set the trim value for the given range.
*
* Parameters:  
*  None
*
* Return: 
*  (void) 
*
* Theory: 
*  Trim values for the IDAC blocks are stored in the "Customer Table" area in 
*  Row 1 of the Hidden Flash.  There are 8 bytes of trim data for each 
*  IDAC block.
*  The values are:
*       I Gain offset, min range, Sourcing
*       I Gain offset, min range, Sinking
*       I Gain offset, med range, Sourcing
*       I Gain offset, med range, Sinking
*       I Gain offset, max range, Sourcing
*       I Gain offset, max range, Sinking
*       V Gain offset, 1V range
*       V Gain offset, 4V range
*
* Side Effects:
*
*******************************************************************************/
void `$INSTANCE_NAME`_DacTrim(void) `=ReentrantKeil($INSTANCE_NAME . "_DacTrim")`
{
    uint8 mode;

    mode = ((`$INSTANCE_NAME`_CR0 & `$INSTANCE_NAME`_RANGE_MASK) >> 1);
    
    if((`$INSTANCE_NAME`_IDIR_MASK & `$INSTANCE_NAME`_CR1) == `$INSTANCE_NAME`_IDIR_SINK)
    {
        mode++;
    }

    `$INSTANCE_NAME`_TR = CY_GET_XTND_REG8((uint8 *)(`$INSTANCE_NAME`_DAC_TRIM_BASE + mode));
}


/* [] END OF FILE */
