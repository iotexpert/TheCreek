/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the Boost Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include <CYLIB.H>

uint8 `$INSTANCE_NAME`_initVar = 0u;
static `$INSTANCE_NAME`_BACKUP_STRUCT `$INSTANCE_NAME`_backup = {`$INSTANCE_NAME`_INIT_VOUT, 
																 `$INSTANCE_NAME`_BOOSTMODE_ACTIVE};


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary: 
*  Inits/Restores default BoostConv configuration provided with customizer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{
    /* Enables mode control from xmode[1:0] instead of control register 1 */
    `$INSTANCE_NAME`_CONTROL_REG1 |= `$INSTANCE_NAME`_PWMEXT_ENABLE;
    
    /* Selects external reference */
    `$INSTANCE_NAME`_CONTROL_REG2 |= `$INSTANCE_NAME`_PRECISION_REF_ENABLE;
	
	if (`$INSTANCE_NAME`_DISABLE_AUTO_BATTERY)
    {
	    `$INSTANCE_NAME`_CONTROL_REG2 |= `$INSTANCE_NAME`_AUTO_BATTERY_DISABLE;
    }
    else
    {
        `$INSTANCE_NAME`_CONTROL_REG2 &= `$INSTANCE_NAME`_AUTO_BATTERY_DISABLE;
    }
    
    /* PSoC3 ES3 or later, PSoC5 ES2 or later */
    #if (CY_PSOC3_ES3 || CY_PSOC5_ES2)
        `$INSTANCE_NAME`_SelExtClk(`$INSTANCE_NAME`_EXTCLK_SRC);     
    #endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */
    
    `$INSTANCE_NAME`_SelFreq(`$INSTANCE_NAME`_FREQUENCY);
    `$INSTANCE_NAME`_SelVoltage(`$INSTANCE_NAME`_INIT_VOUT);
    `$INSTANCE_NAME`_SetMode(`$INSTANCE_NAME`_BOOSTMODE_ACTIVE);                       
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start       
********************************************************************************
*
* Summary: 
*  Starts the `$INSTANCE_NAME` component and puts the boost block into Active 
*  mode. The component is in this state when the chip powers up. This is the
*  preferred method to begin component operation. `$INSTANCE_NAME`_Start()
*  sets the initVar variable, calls the `$INSTANCE_NAME`_Init() function, and
*  then calls the `$INSTANCE_NAME`_Enable() function.
*   
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on
*  first function call.
*
* Side Effect:
*  If the initVar variable is already set, this function: 1 - sets init value
*  of target output voltage (from customizer) and mode (Active mode) or 
*  restores target output voltage and mode saved in `$INSTANCE_NAME`_Stop() 
*  function; 2 - calls the `$INSTANCE_NAME`_Enable() function.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    uint16 timeout;    
    
    for (timeout = `$INSTANCE_NAME`_STARTUP_TIMEOUT;
         timeout && (!(`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_RDY)); timeout--)
    {
        CyDelayUs(1u);
    }

    if (`$INSTANCE_NAME`_RDY == (`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_RDY))
    {
        if (0u == `$INSTANCE_NAME`_initVar)
        {
            `$INSTANCE_NAME`_Init();                   
            `$INSTANCE_NAME`_initVar = 1u;
        }
		else
		{
			`$INSTANCE_NAME`_SelVoltage(`$INSTANCE_NAME`_backup.selVoltage);
            `$INSTANCE_NAME`_SetMode(`$INSTANCE_NAME`_backup.mode);
		}

        `$INSTANCE_NAME`_Enable(); 
   }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary: 
*  Saves boost converter target output voltage and mode. Disables
*  the `$INSTANCE_NAME` component.
*   
* Parameters:
*  None.
*   
* Return:
*  None.
*
* Side Effect:
* Turns off power to the boost converter circuitry. Sets the boost converter
* to Standby mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{    
	`$INSTANCE_NAME`_backup.mode = (`$INSTANCE_NAME`_CONTROL_REG0 & `$INSTANCE_NAME`_MODE_MASK) >> 
							 		`$INSTANCE_NAME`_MODE_SHIFT;                            
	`$INSTANCE_NAME`_backup.selVoltage = (`$INSTANCE_NAME`_CONTROL_REG0 & `$INSTANCE_NAME`_VOLTAGE_MASK) >> 
										  `$INSTANCE_NAME`_VOLTAGE_SHIFT;
	`$INSTANCE_NAME`_SelVoltage(`$INSTANCE_NAME`_VOUT_OFF);
	`$INSTANCE_NAME`_SetMode(`$INSTANCE_NAME`_BOOSTMODE_STANDBY);
    `$INSTANCE_NAME`_Disable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable       
********************************************************************************
*
* Summary: 
*  This function enables the boost only when it is in Active mode. By default
*  this is enabled.
*   
* Parameters:
*  None.
*   
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    `$INSTANCE_NAME`_CONTROL_REG1 |= `$INSTANCE_NAME`_BOOST_ENABLE; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Disable       
********************************************************************************
*
* Summary:
*  This function disables the boost.  
*
* Parameters:
*  None.
*   
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Disable(void) `=ReentrantKeil($INSTANCE_NAME . "_Disable")`
{
    `$INSTANCE_NAME`_CONTROL_REG1 &= ~`$INSTANCE_NAME`_BOOST_ENABLE; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetMode       
********************************************************************************
*
* Summary: 
*  This function sets the mode the Boost is in: Active or Standby.
*
* Parameters:
*  mode: Mode of operation.
*  
* Return:
*  None.
*
* Side Effect:
*  For Standby mode, this function enables automatic thump mode and sets the
*  switching frequency clock source to the 32-kHz external clock. For Active 
*  mode this function disables automatic thump mode.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetMode")`
{                       
    `$INSTANCE_NAME`_CONTROL_REG0 = (`$INSTANCE_NAME`_CONTROL_REG0 & ~`$INSTANCE_NAME`_MODE_MASK) |
                                    (`$INSTANCE_NAME`_MODE_MASK & (mode << `$INSTANCE_NAME`_MODE_SHIFT));      
        
    if(mode == `$INSTANCE_NAME`_BOOSTMODE_STANDBY)
    {       
       `$INSTANCE_NAME`_SelFreq(`$INSTANCE_NAME`__SWITCH_FREQ_32KHZ);
       `$INSTANCE_NAME`_EnableAutoThump();
    }
    else
    {
        `$INSTANCE_NAME`_DisableAutoThump();
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SelVoltage       
********************************************************************************
*
* Summary: 
*  This function selects the target output voltage the boost converter will
*  maintain.
*   
* Parameters:
*  voltage: Desired output voltage.
*   
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SelVoltage(uint8 voltage) `=ReentrantKeil($INSTANCE_NAME . "_SelVoltage")`
{                 
    `$INSTANCE_NAME`_CONTROL_REG0 = (`$INSTANCE_NAME`_CONTROL_REG0 & ~`$INSTANCE_NAME`_VOLTAGE_MASK) |
                                    (`$INSTANCE_NAME`_VOLTAGE_MASK & voltage);     
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SelFreq
********************************************************************************
*
* Summary: 
*  This function sets the frequency to one of the 4 possible values: 
*  100kHz, 400kHz, and 2 MHz (which are all generated internal to the Boost 
*  Converter block with a dedicated oscillator) or 32kHz (which comes from
*  the chips ECO-32kHz or ILO-32kHz).
*
* Parameters:
*  frequency: Desired switching frequency.
*   
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SelFreq(uint8 frequency) `=ReentrantKeil($INSTANCE_NAME . "_SelFreq")`
{    
    `$INSTANCE_NAME`_CONTROL_REG1 = (`$INSTANCE_NAME`_CONTROL_REG1 & ~(`$INSTANCE_NAME`_FREQ_MASK)) |
                                    (`$INSTANCE_NAME`_FREQ_MASK & frequency);     
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableAutoThump       
********************************************************************************
*
* Summary: 
*  This function enables automatic thump mode (only available when Boost is 
*  in Standby mode)
*   
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableAutoThump(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableAutoThump")`
{
    `$INSTANCE_NAME`_CONTROL_REG2 |= `$INSTANCE_NAME`_AUTO_THUMP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableAutoThump       
********************************************************************************
*
* Summary: 
*  This function disables automatic thump mode (only available when Boost is 
*  in Standby mode)  
*
* Parameters:
*  None.
*   
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableAutoThump(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableAutoThump")`
{
    `$INSTANCE_NAME`_CONTROL_REG2 &= ~`$INSTANCE_NAME`_AUTO_THUMP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ManualThump       
********************************************************************************
*
* Summary: 
*  This function forces a single pulse of the boost converter switch 
*  transistors.
*
* Parameters:
*  None.
*   
* Return:
*  None.
*
* Theory:
*  Thump - produces one ~500ns pulse when set. Must be reset to 0 before another
*  pulse can be generated. It should not re-set the bit until after the 500ns 
*  pulse has expired. Used for discrete switch control.
*
* Side Effects:
*  This routine writes a "0" followed by a "1" to the bit 7 "thump" bit in the
*  boost block BOOST_CR0 register.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ManualThump(void) `=ReentrantKeil($INSTANCE_NAME . "_ManualThump")`
{      
    `$INSTANCE_NAME`_CONTROL_REG0 |=  `$INSTANCE_NAME`_MANUAL_THUMP_ENABLE;    
    `$INSTANCE_NAME`_CONTROL_REG0 &= ~`$INSTANCE_NAME`_MANUAL_THUMP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadStatus       
********************************************************************************
*
* Summary: 
*  This function returns the contents of the boost block status register.  
*
* Parameters:
*  None.
*   
* Return:
*  Boost block status register.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadStatus")`
{   
    return(`$INSTANCE_NAME`_STATUS_REG);
}


/* PSoC3 ES3 or later, PSoC5 ES2 or later */
#if (CY_PSOC3_ES3 || CY_PSOC5_ES2)
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SelFreq       
    ********************************************************************************
    *
    * Summary: 
    *  This function sets the source of 32kHz frequency: the chips ECO-32kHz 
    *  or ILO-32kHz.
    *   
    * Parameters:
    *  source: source of 32kHz frequency.
    *   
    * Return:
    *  None.
    *
    *******************************************************************************/
    void  `$INSTANCE_NAME`_SelExtClk(uint8 source) `=ReentrantKeil($INSTANCE_NAME . "_SelExtClk")`
    {               
        `$INSTANCE_NAME`_CONTROL_REG4 = (`$INSTANCE_NAME`_CONTROL_REG4 & ~`$INSTANCE_NAME`_EXTCLK_SRC_MASK) |
                                        (`$INSTANCE_NAME`_EXTCLK_SRC_MASK &
                                        (source << `$INSTANCE_NAME`_EXTCLK_SRC_SHIFT));                                 
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EnableInt       
    ********************************************************************************
    *
    * Summary: 
    *  This function enables the Boost block Output Under-Voltage interrupt 
    *  generation.    
    *
    * Parameters:
    *  None.
    *   
    * Return:
    *  None.
    *
    *******************************************************************************/
    void  `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
    {
        `$INSTANCE_NAME`_CONTROL_REG4 |= `$INSTANCE_NAME`_INT_ENABLE_MASK; 
    }
    
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_DisableInt       
    ********************************************************************************
    *
    * Summary: 
    *  This function disables the Boost block Output Under-Voltage interrupt 
    *  generation.    
    *   
    * Parameters:
    *  None.
    *   
    * Return:
    *  None.
    *
    *******************************************************************************/
    void  `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
    {
        `$INSTANCE_NAME`_CONTROL_REG4 &= ~`$INSTANCE_NAME`_INT_ENABLE_MASK;
    }
    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ReadIntStatus       
    ********************************************************************************
    * 
    * Summary:
    *  This function returns the contents of the boost block interrupt status
    *  register.
    *
    * Parameters:
    *  None.
    *   
    * Return:
    *  Boost interrupt status register.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_ReadIntStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadIntStatus")`
    {
        return (`$INSTANCE_NAME`_STATUS_REG2 & `$INSTANCE_NAME`_INT_ENABLE_MASK);
    }
             
#endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */


/* [] END OF FILE */
