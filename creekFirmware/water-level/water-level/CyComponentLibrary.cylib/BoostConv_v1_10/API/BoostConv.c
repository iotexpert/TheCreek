/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* This file provides the source code to the API for the Boost Component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_initFlag = 0;

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start       
********************************************************************************
* Summary: Starts the Boost component and puts the Boost into Active Mode. 
*   By default the component is started.
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    uint16 timeout;    
    for (timeout = 1024; timeout && (!(`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_RDY)); timeout--) 
    {;}
    if (`$INSTANCE_NAME`_STATUS_REG & `$INSTANCE_NAME`_RDY)
    {
        if (0 == `$INSTANCE_NAME`_initFlag)
        {
            `$INSTANCE_NAME`_CONTROL_REG1 |= `$INSTANCE_NAME`_PWMEXT_ENABLE; 
            `$INSTANCE_NAME`_CONTROL_REG2 |= `$INSTANCE_NAME`_PRECISION_REF_ENABLE; 
            `$INSTANCE_NAME`_SelFreq(`$INSTANCE_NAME`_FREQUENCY);
            `$INSTANCE_NAME`_SelVoltage(`$INSTANCE_NAME`_INIT_VOUT);
            `$INSTANCE_NAME`_SetMode(`$INSTANCE_NAME`_BOOSTMODE_ACTIVE);        
            
            `$INSTANCE_NAME`_initFlag = 1;
        }    

        `$INSTANCE_NAME`_Enable(); 
   }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop       
********************************************************************************
* Summary: Disables the Boost component and disables interrupts. 
*   Turns off power to the Boost logic.
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    `$INSTANCE_NAME`_SelVoltage(`$INSTANCE_NAME`_VOUT_OFF);
    `$INSTANCE_NAME`_Disable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetMode       
********************************************************************************
* Summary: This function sets the mode the Boost is in: Active, Standby or Sleep.
*   For standby mode, automatic thump mode is enabled.
*   For Active and Sleep modes automatic thump mode is disabled.   
*
* Parameters:
*   Mode: Mode of operation
*
*   
* Return:
*  void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetMode(uint8 Mode)
{
    uint8 tempRegValue;
    
    tempRegValue = `$INSTANCE_NAME`_CONTROL_REG0;
    tempRegValue = (tempRegValue & ~`$INSTANCE_NAME`_MODE_MASK) | (`$INSTANCE_NAME`_MODE_MASK & (Mode << `$INSTANCE_NAME`_MODE_SHIFT));      
    `$INSTANCE_NAME`_CONTROL_REG0 = tempRegValue;    

    if(Mode == `$INSTANCE_NAME`_BOOSTMODE_STANDBY)
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
* Summary:This function selects the voltage the Boost will output which is
*    a 5 bit representation of all desired output voltages, as defined the 
*    Leopard Register Boost.CR0
*   
*
* Parameters:
*   Voltage:
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_SelVoltage(uint8 Voltage)
{
    uint8 tempReg;
    
    tempReg = `$INSTANCE_NAME`_CONTROL_REG0;    
    tempReg = ( (tempReg & ~`$INSTANCE_NAME`_VOLTAGE_MASK) | (`$INSTANCE_NAME`_VOLTAGE_MASK & Voltage));
    `$INSTANCE_NAME`_CONTROL_REG0 = tempReg;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SelFreq       
********************************************************************************
* Summary: This function sets the frequency to one of the 4 possible values: 
*   100kHz, 400kHz, and 2 MHz (which are all generated internal to the Boost 
*   Converter block with a dedicated oscillator) or 32kHz (which comes from
*   the chips ECO-32kHz).
*   
*
* Parameters:
*   Frequency:
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_SelFreq(uint8 Frequency)
{
    uint8 tempReg;
   
    tempReg = `$INSTANCE_NAME`_CONTROL_REG1;
    tempReg = (tempReg & ~(`$INSTANCE_NAME`_FREQ_MASK)) | (`$INSTANCE_NAME`_FREQ_MASK & Frequency); 
    `$INSTANCE_NAME`_CONTROL_REG1 = tempReg;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableAutoThump       
********************************************************************************
* Summary:This function enables automatic thump mode 
*    (only available when Boost is in Standby mode)
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableAutoThump(void)
{
    `$INSTANCE_NAME`_CONTROL_REG2 |= `$INSTANCE_NAME`_AUTO_THUMP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableAutoThump       
********************************************************************************
* Summary:This function disables automatic thump mode
*   (only available when Boost is in Standby mode)
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableAutoThump(void)
{
    `$INSTANCE_NAME`_CONTROL_REG2 &= ~`$INSTANCE_NAME`_AUTO_THUMP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ManualThump       
********************************************************************************
* Summary:This function forces a single pulse of the boost converter switch 
*    transistors.  This function can be called when the system periodically 
*    comes out of sleep mode to counter the affects of voltage drift during 
*    sleep.
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_ManualThump(void)
{
    uint8 i;
   
    `$INSTANCE_NAME`_CONTROL_REG0 |=  `$INSTANCE_NAME`_MANUAL_THUMP_ENABLE;
    for(i = 0; i < 10; i++) /* Some delay */ ;
    `$INSTANCE_NAME`_CONTROL_REG0 &= ~`$INSTANCE_NAME`_MANUAL_THUMP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable       
********************************************************************************
* Summary: This function enables the boost only when it is in Active mode.
* By default this is enabled.
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void)
{
    `$INSTANCE_NAME`_CONTROL_REG1 |= `$INSTANCE_NAME`_BOOST_ENABLE; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Disable       
********************************************************************************
* Summary:This function disables the boost.
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
void `$INSTANCE_NAME`_Disable(void)
{
    `$INSTANCE_NAME`_CONTROL_REG1 &= ~`$INSTANCE_NAME`_BOOST_ENABLE; 
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadStatus       
********************************************************************************
* Summary: This function returns the contents of the boost block status register.
*   
*
* Parameters:
*   void.
*
*   
* Return:
*   void.
*
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadStatus(void)
{   
    return(`$INSTANCE_NAME`_STATUS_REG);
}


/* [] END OF FILE */

