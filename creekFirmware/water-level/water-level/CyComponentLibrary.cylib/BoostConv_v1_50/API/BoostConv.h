/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file provides registers and constants associated with the 
*  Boost component. 
*
* Note:
*  None.
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_BOOST_`$INSTANCE_NAME`_H)
#define CY_BOOST_`$INSTANCE_NAME`_H

#include <cyfitter.h>
#include <cydevice_trm.h>


/***************************************
*   Conditional Compilation Parameters
***************************************/

/* PSoC3 ES3 or later*/
#define `$INSTANCE_NAME`_PSOC3_ES3  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_3A) && \
                                 	(CYDEV_CHIP_REVISION_USED >= CYDEV_CHIP_REVISION_3A_ES3))
                     
/* PSoC5 ES2 or later*/
#define `$INSTANCE_NAME`_PSOC5_ES2  ((CYDEV_CHIP_MEMBER_USED == CYDEV_CHIP_MEMBER_5A) && \
                                  	(CYDEV_CHIP_REVISION_USED > CYDEV_CHIP_REVISION_5A_ES1))



/***************************************
*        Function Prototypes
***************************************/

void  `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void  `$INSTANCE_NAME`_Start(void);
void  `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void  `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;
void  `$INSTANCE_NAME`_Disable(void) `=ReentrantKeil($INSTANCE_NAME . "_Disable")`;
void  `$INSTANCE_NAME`_SetMode(uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_SetMode")`;
void  `$INSTANCE_NAME`_SelVoltage(uint8 voltage) `=ReentrantKeil($INSTANCE_NAME . "_SelVoltage")`;
void  `$INSTANCE_NAME`_SelFreq(uint8 frequency) `=ReentrantKeil($INSTANCE_NAME . "_SelFreq")`;
void  `$INSTANCE_NAME`_EnableAutoThump(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableAutoThump")`;
void  `$INSTANCE_NAME`_DisableAutoThump(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableAutoThump")`;
void  `$INSTANCE_NAME`_ManualThump(void) `=ReentrantKeil($INSTANCE_NAME . "_ManualThump")`;
uint8 `$INSTANCE_NAME`_ReadStatus(void)`=ReentrantKeil($INSTANCE_NAME . "_ReadStatus")`;

/* PSoC3 ES3 or later, PSoC5 ES2 or later */

#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    
    void  `$INSTANCE_NAME`_SelExtClk(uint8 source) `=ReentrantKeil($INSTANCE_NAME . "_SelExtClk")`;
    void  `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`;
    void  `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`;
    uint8 `$INSTANCE_NAME`_ReadIntStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadIntStatus")`;              
     
#endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */


/***************************************
*         API Constants 
***************************************/

/* Constants for SetMode function */

#define `$INSTANCE_NAME`_BOOSTMODE_STANDBY      (0x01u) 
#define `$INSTANCE_NAME`_BOOSTMODE_ACTIVE       (0x03u) 

/* Constants for SelVoltage function */

#define `$INSTANCE_NAME`_VOUT_OFF               (0x00u)
#define `$INSTANCE_NAME`_VOUT_1_8V              (0x03u)
#define `$INSTANCE_NAME`_VOUT_1_9V              (0x04u)
#define `$INSTANCE_NAME`_VOUT_2_0V              (0x05u)
#define `$INSTANCE_NAME`_VOUT_2_1V              (0x06u)
#define `$INSTANCE_NAME`_VOUT_2_2V              (0x07u)
#define `$INSTANCE_NAME`_VOUT_2_3V              (0x08u)
#define `$INSTANCE_NAME`_VOUT_2_4V              (0x09u)
#define `$INSTANCE_NAME`_VOUT_2_5V              (0x0Au)
#define `$INSTANCE_NAME`_VOUT_2_6V              (0x0Bu)
#define `$INSTANCE_NAME`_VOUT_2_7V              (0x0Cu)
#define `$INSTANCE_NAME`_VOUT_2_8V              (0x0Du)
#define `$INSTANCE_NAME`_VOUT_2_9V              (0x0Eu)
#define `$INSTANCE_NAME`_VOUT_3_0V              (0x0Fu)
#define `$INSTANCE_NAME`_VOUT_3_1V              (0x10u)
#define `$INSTANCE_NAME`_VOUT_3_2V              (0x11u)
#define `$INSTANCE_NAME`_VOUT_3_3V              (0x12u)
#define `$INSTANCE_NAME`_VOUT_3_4V              (0x13u)
#define `$INSTANCE_NAME`_VOUT_3_5V              (0x14u)
#define `$INSTANCE_NAME`_VOUT_3_6V              (0x15u)
#define `$INSTANCE_NAME`_VOUT_4_0V              (0x16u)
#define `$INSTANCE_NAME`_VOUT_4_25V             (0x17u)
#define `$INSTANCE_NAME`_VOUT_4_5V              (0x18u)
#define `$INSTANCE_NAME`_VOUT_4_75V             (0x19u)
#define `$INSTANCE_NAME`_VOUT_5_0V              (0x1Au)
#define `$INSTANCE_NAME`_VOUT_5_25V             (0x1Bu)


/***************************************
*    Enumerated Types and Parameters
***************************************/

/* Enumerated Types Boost_Frequency, Used in parameter Frequency */

`#cy_declare_enum Boost_Frequency`

/* PSoC3 ES3 or later, PSoC5 ES2 or later */

#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)

    /* Enumerated Types Boost_ExtClk_Source, Used in parameter ExtClk_Source */
    `#cy_declare_enum Boost_ExtClk_Src`
    
#endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */


/***************************************
*      Initial Parameter Constants 
***************************************/

#define `$INSTANCE_NAME`_INIT_VOUT            (`$OutVoltage`u)
#define `$INSTANCE_NAME`_FREQUENCY            (`$Frequency`u)

/* PSoC3 ES3 or later, PSoC5 ES2 or later */
#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)

    #define `$INSTANCE_NAME`_EXTCLK_SRC       (`$ExtClk_Source`u)  

#endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */


/***************************************
*             Registers
***************************************/

#define `$INSTANCE_NAME`_CONTROL_REG0         (* (reg8*) CYREG_BOOST_CR0)
#define `$INSTANCE_NAME`_CONTROL_REG0_PTR     (  (reg8*) CYREG_BOOST_CR0)
#define `$INSTANCE_NAME`_CONTROL_REG1         (* (reg8*) CYREG_BOOST_CR1)
#define `$INSTANCE_NAME`_CONTROL_REG1_PTR     (  (reg8*) CYREG_BOOST_CR1)
#define `$INSTANCE_NAME`_CONTROL_REG2         (* (reg8*) CYREG_BOOST_CR2)
#define `$INSTANCE_NAME`_CONTROL_REG2_PTR     (  (reg8*) CYREG_BOOST_CR2)
#define `$INSTANCE_NAME`_CONTROL_REG3         (* (reg8*) CYREG_BOOST_CR3)
#define `$INSTANCE_NAME`_CONTROL_REG3_PTR     (  (reg8*) CYREG_BOOST_CR3)
#define `$INSTANCE_NAME`_STATUS_REG           (* (reg8*) CYREG_BOOST_SR)
#define `$INSTANCE_NAME`_STATUS_REG_PTR       (  (reg8*) CYREG_BOOST_SR)

/* PSoC3 ES3 or later, PSoC5 ES2 or later */

#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    
    #define `$INSTANCE_NAME`_CONTROL_REG4         (* (reg8*) CYREG_BOOST_CR4)
    #define `$INSTANCE_NAME`_CONTROL_REG4_PTR     (  (reg8*) CYREG_BOOST_CR4)
    #define `$INSTANCE_NAME`_STATUS_REG2          (* (reg8*) CYREG_BOOST_SR2)
    #define `$INSTANCE_NAME`_STATUS_REG2_PTR      (  (reg8*) CYREG_BOOST_SR2)
     
#endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */


/***************************************
*        Register Constants
***************************************/

/* Boost.CR0 */

#define `$INSTANCE_NAME`_VOLTAGE_SHIFT             (0x00u)
#define `$INSTANCE_NAME`_VOLTAGE_MASK              (0x1Fu << `$INSTANCE_NAME`_VOLTAGE_SHIFT)

#define `$INSTANCE_NAME`_MODE_SHIFT                (0x05u)
#define `$INSTANCE_NAME`_MODE_MASK                 (0x03u << `$INSTANCE_NAME`_MODE_SHIFT)

#define `$INSTANCE_NAME`_MANUAL_THUMP_ENABLE_SHIFT (0x07u)
#define `$INSTANCE_NAME`_MANUAL_THUMP_ENABLE       (0x01u << `$INSTANCE_NAME`_AUTO_THUMP_ENABLE_SHIFT)

/* Boost.CR1 */

#define `$INSTANCE_NAME`_FREQ_SHIFT                (0x00u)
#define `$INSTANCE_NAME`_FREQ_MASK                 (0x03u << `$INSTANCE_NAME`_FREQ_SHIFT)

#define `$INSTANCE_NAME`_PWMEXT_ENABLE_SHIFT       (0x02u)
#define `$INSTANCE_NAME`_PWMEXT_ENABLE             (0x01u << `$INSTANCE_NAME`_PWMEXT_ENABLE_SHIFT)

#define `$INSTANCE_NAME`_BOOST_ENABLE_SHIFT        (0x03u)
#define `$INSTANCE_NAME`_BOOST_ENABLE              (0x01u << `$INSTANCE_NAME`_BOOST_ENABLE_SHIFT)

/* Boost.CR2 */

#define `$INSTANCE_NAME`_AUTO_THUMP_ENABLE_SHIFT   (0x00u)
#define `$INSTANCE_NAME`_AUTO_THUMP_ENABLE         (0x01u << `$INSTANCE_NAME`_AUTO_THUMP_ENABLE_SHIFT)

/* enables external precision 800mv referance */
#define `$INSTANCE_NAME`_PRECISION_REF_ENABLE_SHIFT (0x03u)
#define `$INSTANCE_NAME`_PRECISION_REF_ENABLE       (0x01u << `$INSTANCE_NAME`_PRECISION_REF_ENABLE_SHIFT)
 
/* Boost.SR */

#define `$INSTANCE_NAME`_RDY_SHIFT                 (0x07u)
#define `$INSTANCE_NAME`_RDY                       (0x01u << `$INSTANCE_NAME`_RDY_SHIFT)
#define `$INSTANCE_NAME`_START_SHIFT               (0x06u)
#define `$INSTANCE_NAME`_START                     (0x01u << `$INSTANCE_NAME`_START_SHIFT)
#define `$INSTANCE_NAME`_OV_SHIFT                  (0x04u)
#define `$INSTANCE_NAME`_OV                        (0x01u << `$INSTANCE_NAME`_OV_SHIFT)
#define `$INSTANCE_NAME`_VHI_SHIFT                 (0x03u)
#define `$INSTANCE_NAME`_VHI                       (0x01u << `$INSTANCE_NAME`_VHI_SHIFT)
#define `$INSTANCE_NAME`_VNOM_SHIFT                (0x02u)
#define `$INSTANCE_NAME`_VNOM                      (0x01u << `$INSTANCE_NAME`_VNOM_SHIFT)
#define `$INSTANCE_NAME`_VLO_SHIFT                 (0x01u)
#define `$INSTANCE_NAME`_VLO                       (0x01u << `$INSTANCE_NAME`_VLO_SHIFT)
#define `$INSTANCE_NAME`_UV_SHIFT                  (0x00u)
#define `$INSTANCE_NAME`_UV                        (0x01u << `$INSTANCE_NAME`_UV_SHIFT)

/* PSoC3 ES3 or later, PSoC5 ES2 or later */

#if (`$INSTANCE_NAME`_PSOC3_ES3 || `$INSTANCE_NAME`_PSOC5_ES2)
    
    /* Boost.CR3 */
    
    #define `$INSTANCE_NAME`_INT_ENABLE_SHIFT       (0x00u)
    #define `$INSTANCE_NAME`_INT_ENABLE_MASK        (0x01u << `$INSTANCE_NAME`_INT_ENABLE_SHIFT)
    
    #define `$INSTANCE_NAME`_EXTCLK_SRC_SHIFT       (0x01u) 
    #define `$INSTANCE_NAME`_EXTCLK_SRC_MASK        (0x03u << `$INSTANCE_NAME`_EXTCLK_SRC_SHIFT)    
    
    /* Boost.SR2 */
    
    #define `$INSTANCE_NAME`_INT_SHIFT              (0x00u)
    #define `$INSTANCE_NAME`_INT                    (0x01u << `$INSTANCE_NAME`_INT_SHIFT) 
     
#endif /* PSoC3 ES3 or later, PSoC5 ES2 or later */

/* Timing requirements of Start depends on input voltage and can be in range 100us to 10ms (for 0.5V) */
#define `$INSTANCE_NAME`_STARTUP_TIMEOUT            (10000u) /* Initial startup timeout 10 ms  */

#endif /* CY_BOOST_`$INSTANCE_NAME`_H */


/* [] END OF FILE */
