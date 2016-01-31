/******************************************************************************
* File Name: `$INSTANCE_NAME`_BoostConv.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the API to acquire the die temperature..
*
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#if !defined(`$INSTANCE_NAME`_BOOST_H)
#define `$INSTANCE_NAME`_BOOST_H

void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void);
void  `$INSTANCE_NAME`_SetMode(uint8);
void  `$INSTANCE_NAME`_SelVoltage(uint8);
void  `$INSTANCE_NAME`_SelFreq(uint8);
void  `$INSTANCE_NAME`_EnableAutoThump(void);
void  `$INSTANCE_NAME`_DisableAutoThump(void);
void  `$INSTANCE_NAME`_ManualThump(void);
void  `$INSTANCE_NAME`_Enable(void);
void  `$INSTANCE_NAME`_Disable(void);
uint8 `$INSTANCE_NAME`_ReadStatus(void);

/****************************************************
*-----		Parameter values				    -----
****************************************************/

`#cy_declare_enum SwitchFrequency`

/********************************
 ******     Constants       *****
 *******************************/

#define `$INSTANCE_NAME`_INIT_VOUT             `$OutVoltage`

/* Constants for SetMode function */
#define `$INSTANCE_NAME`_BOOSTMODE_SLEEP        0x00U
#define `$INSTANCE_NAME`_BOOSTMODE_STANDBY      0x01U 
#define `$INSTANCE_NAME`_BOOSTMODE_ACTIVE       0x03U 

/********************************
 ******     Registers       *****
 *******************************/

#define `$INSTANCE_NAME`_CONTROL_REG0         (* (reg8*) CYDEV_BOOST_CR0)
#define `$INSTANCE_NAME`_CONTROL_REG1         (* (reg8*) CYDEV_BOOST_CR1)
#define `$INSTANCE_NAME`_CONTROL_REG2         (* (reg8*) CYDEV_BOOST_CR2)
#define `$INSTANCE_NAME`_CONTROL_REG3         (* (reg8*) CYDEV_BOOST_CR3)
#define `$INSTANCE_NAME`_STATUS_REG           (* (reg8*) CYDEV_BOOST_SR)

 /*************************************
 ******     Register values       *****
 *************************************/

#define `$INSTANCE_NAME`_THUMP_ENABLE         (0x01U << `$INSTANCE_NAME`_THUMP_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_BOOST_ENABLE         (0x01U << `$INSTANCE_NAME`_BOOST_ENABLE_SHIFT)
#define `$INSTANCE_NAME`_PWMEXT_ENABLE        (0x01U << `$INSTANCE_NAME`_PWMEXT_ENABLE_SHIFT)

#define `$INSTANCE_NAME`_THUMP_ENABLE_SHIFT   0x07U
#define `$INSTANCE_NAME`_BOOST_ENABLE_SHIFT   0x03U
#define `$INSTANCE_NAME`_PWMEXT_ENABLE_SHIFT  0x05U

#define `$INSTANCE_NAME`_MODE_SLEEP           (0x00U << `$INSTANCE_NAME`_MODE_SHIFT)
#define `$INSTANCE_NAME`_MODE_ACTIVE          (0x03U << `$INSTANCE_NAME`_MODE_SHIFT) 
#define `$INSTANCE_NAME`_MODE_STANDBY         (0x01U << `$INSTANCE_NAME`_MODE_SHIFT) 
 
#define `$INSTANCE_NAME`_MODE_SHIFT           0x05U

#define `$INSTANCE_NAME`_SWITCH_FREQ_100KHZ   0x02U
#define `$INSTANCE_NAME`_SWITCH_FREQ_400KHZ   0x01U
#define `$INSTANCE_NAME`_SWITCH_FREQ_2MHZ     0x00U
#define `$INSTANCE_NAME`_SWITCH_FREQ_32KHZ    0x03U

#define `$INSTANCE_NAME`_VOUT_OFF             0x00U
#define `$INSTANCE_NAME`_VOUT_1_6V            0x01U
#define `$INSTANCE_NAME`_VOUT_1_7V            0x02U
#define `$INSTANCE_NAME`_VOUT_1_8V            0x03U
#define `$INSTANCE_NAME`_VOUT_1_9V            0x04U
#define `$INSTANCE_NAME`_VOUT_2_0V            0x05U
#define `$INSTANCE_NAME`_VOUT_2_1V            0x06U
#define `$INSTANCE_NAME`_VOUT_2_2V            0x07U
#define `$INSTANCE_NAME`_VOUT_2_3V            0x08U
#define `$INSTANCE_NAME`_VOUT_2_4V            0x09U
#define `$INSTANCE_NAME`_VOUT_2_5V            0x0AU
#define `$INSTANCE_NAME`_VOUT_2_6V            0x0BU
#define `$INSTANCE_NAME`_VOUT_2_7V            0x0CU
#define `$INSTANCE_NAME`_VOUT_2_8V            0x0DU
#define `$INSTANCE_NAME`_VOUT_2_9V            0x0EU
#define `$INSTANCE_NAME`_VOUT_3_0V            0x0FU
#define `$INSTANCE_NAME`_VOUT_3_1V            0x10U
#define `$INSTANCE_NAME`_VOUT_3_2V            0x11U
#define `$INSTANCE_NAME`_VOUT_3_3V            0x12U
#define `$INSTANCE_NAME`_VOUT_3_4V            0x12U
#define `$INSTANCE_NAME`_VOUT_3_5V            0x14U
#define `$INSTANCE_NAME`_VOUT_3_6V            0x15U
#define `$INSTANCE_NAME`_VOUT_4_0V            0x18U
#define `$INSTANCE_NAME`_VOUT_4_25V           0x19U
#define `$INSTANCE_NAME`_VOUT_4_75V           0x1AU
#define `$INSTANCE_NAME`_VOUT_5_0V            0x1BU

#define `$INSTANCE_NAME`_MODE_MASK            (0x11U << `$INSTANCE_NAME`_MODE_SHIFT)
#define `$INSTANCE_NAME`_VOLTAGE_MASK         0x1FU
#define `$INSTANCE_NAME`_FREQ_MASK            0x03U

/* Boost.CR2 */
/* bit marked Reserved in RDL - enables precision 800mv referance */
#define `$INSTANCE_NAME`_PRECISION_REF_ENABLE (0x01U << `$INSTANCE_NAME`_PRECISION_REF_ENABLE_SHIFT)

#define `$INSTANCE_NAME`_PRECISION_REF_ENABLE_SHIFT   0x03U
 
/* Boost.SR */
#define `$INSTANCE_NAME`_RDY                  (0x01U << `$INSTANCE_NAME`_RDY_SHIFT)
#define `$INSTANCE_NAME`_START                (0x01U << `$INSTANCE_NAME`_START_SHIFT)
#define `$INSTANCE_NAME`_NO_BAT               (0x01U << `$INSTANCE_NAME`_NO_BAT_SHIFT)
#define `$INSTANCE_NAME`_OV                   (0x01U << `$INSTANCE_NAME`_OV_SHIFT)
#define `$INSTANCE_NAME`_VHI                  (0x01U << `$INSTANCE_NAME`_VHI_SHIFT)
#define `$INSTANCE_NAME`_VNOM                 (0x01U << `$INSTANCE_NAME`_VNOM_SHIFT)
#define `$INSTANCE_NAME`_VLO                  (0x01U << `$INSTANCE_NAME`_VLO_SHIFT)
#define `$INSTANCE_NAME`_UV                   (0x01U << `$INSTANCE_NAME`_UV_SHIFT)

#define `$INSTANCE_NAME`_RDY_SHIFT            0x07U
#define `$INSTANCE_NAME`_START_SHIFT          0x06U
#define `$INSTANCE_NAME`_NO_BAT_SHIFT         0x05U
#define `$INSTANCE_NAME`_OV_SHIFT             0x04U
#define `$INSTANCE_NAME`_VHI_SHIFT            0x03U
#define `$INSTANCE_NAME`_VNOM_SHIFT           0x02U
#define `$INSTANCE_NAME`_VLO_SHIFT            0x01U
#define `$INSTANCE_NAME`_UV_SHIFT             0x00U

#endif /* `$INSTANCE_NAME`_BOOST_H */
/* [] END OF FILE */
