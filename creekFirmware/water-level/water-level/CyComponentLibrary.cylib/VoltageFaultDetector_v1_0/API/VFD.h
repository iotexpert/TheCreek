/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Contains the function prototypes, constants and register definition of the 
*  Voltage Fault Detector Component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_`$INSTANCE_NAME`_H)
    #define CY_`$INSTANCE_NAME`_H
#endif

#include "cytypes.h"
#include "cyfitter.h"
#include "CyLib.h"

#define `$INSTANCE_NAME`_OV    (1u)
#define `$INSTANCE_NAME`_UV    (2u)

#define `$INSTANCE_NAME`_OV_UV      (0u)
#define `$INSTANCE_NAME`_OV_ONLY    (1u)
#define `$INSTANCE_NAME`_UV_ONLY    (2u)
#define `$INSTANCE_NAME`_GF_LENGTH      (`$GfLength`u) 
#define `$INSTANCE_NAME`_CompareType    (`$CompareType`u)

#define `$INSTANCE_NAME`_External_Reference    (`$ExternalRef`u)

#if(!`$INSTANCE_NAME`_External_Reference)

    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)

        #include "`$INSTANCE_NAME`_Comp_OV.h"
        #include "`$INSTANCE_NAME`_Comp_UV.h"
        #include "`$INSTANCE_NAME`_VDAC_OV.h"
        #include "`$INSTANCE_NAME`_VDAC_UV.h"
        #include "`$INSTANCE_NAME`_DAC_OV_dma.h"
        #include "`$INSTANCE_NAME`_DAC_UV_dma.h"
        #include "`$INSTANCE_NAME`_OV_RD_dma.h"
        #include "`$INSTANCE_NAME`_OV_WR_dma.h"
        #include "`$INSTANCE_NAME`_UV_RD_dma.h"
        #include "`$INSTANCE_NAME`_UV_WR_dma.h"
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */ 

    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
        
        #include "`$INSTANCE_NAME`_Comp_OV.h"
        #include "`$INSTANCE_NAME`_VDAC_OV.h"
        #include "`$INSTANCE_NAME`_DAC_OV_dma.h"
        #include "`$INSTANCE_NAME`_OV_RD_dma.h"
        #include "`$INSTANCE_NAME`_OV_WR_dma.h"
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */

    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
        
        #include "`$INSTANCE_NAME`_Comp_UV.h"
        #include "`$INSTANCE_NAME`_VDAC_UV.h"
        #include "`$INSTANCE_NAME`_DAC_UV_dma.h"
        #include "`$INSTANCE_NAME`_UV_RD_dma.h"
        #include "`$INSTANCE_NAME`_UV_WR_dma.h"
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
#else
    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)

        #include "`$INSTANCE_NAME`_Comp_OV.h"
        #include "`$INSTANCE_NAME`_Comp_UV.h"
        #include "`$INSTANCE_NAME`_OV_RD_dma.h"
        #include "`$INSTANCE_NAME`_OV_WR_dma.h"
        #include "`$INSTANCE_NAME`_UV_RD_dma.h"
        #include "`$INSTANCE_NAME`_UV_WR_dma.h"
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */ 

    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
        
        #include "`$INSTANCE_NAME`_Comp_OV.h"
        #include "`$INSTANCE_NAME`_OV_RD_dma.h"
        #include "`$INSTANCE_NAME`_OV_WR_dma.h"
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */

    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
        
        #include "`$INSTANCE_NAME`_Comp_UV.h"
        #include "`$INSTANCE_NAME`_UV_RD_dma.h"
        #include "`$INSTANCE_NAME`_UV_WR_dma.h"
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */

#endif /* !`$INSTANCE_NAME`_External_Reference */

/* Check to see if required defines such as CY_PSOC5A are available */
/* They are defined starting with cy_boot v3.0 */
#if !defined (CY_PSOC5A)
    #error Component `$CY_COMPONENT_NAME` requires cy_boot v3.0 or later
#endif /* (CY_PSOC5A) */


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define `$INSTANCE_NAME`_Amount_of_Voltages    (`$NumVoltages`u)
#define `$INSTANCE_NAME`_Dac_Range             (`$DacRange`u)

#define `$INSTANCE_NAME`_SW_ENABLE          (1u) 
#define `$INSTANCE_NAME`_STATUS_MASK        (`$StatusMask`)
#define `$INSTANCE_NAME`_SHIFT_VAL_8        (0x08u)
#define `$INSTANCE_NAME`_SHIFT_VAL_16       (0x10u)
#define `$INSTANCE_NAME`_SHIFT_VAL_24       (0x18u)
#define `$INSTANCE_NAME`_CNT_EN             (0x20u)
#define `$INSTANCE_NAME`_DAC_RANGE_1V       (0u)
#define `$INSTANCE_NAME`_DAC_RANGE_4V       (1u)

#define `$INSTANCE_NAME`_DAC_RANGE_1V_VAL    (1024u)
#define `$INSTANCE_NAME`_DAC_RANGE_4V_VAL    (4096u)

#if(`$INSTANCE_NAME`_External_Reference == 0u)
    
    #if(`$INSTANCE_NAME`_Dac_Range == `$INSTANCE_NAME`_DAC_RANGE_1V)
        
        #define `$INSTANCE_NAME`_DAC_VOL_DIVIDER           (4u)
    
    #else
    
        #define `$INSTANCE_NAME`_DAC_VOL_DIVIDER           (16u)
    
    #endif /* `$INSTANCE_NAME`_Dac_Range == `$INSTANCE_NAME`_DAC_RANGE_1V_VAL */
    
#endif /* `$INSTANCE_NAME`_External_Reference == 0u */


/***************************************
*        Function Prototypes
***************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`;
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`;
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`;
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`;

#if((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)|| \
    (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY))
    void `$INSTANCE_NAME`_SetOVFaultThreshold(uint8 voltageNum,uint16 ovFaultThreshold) `=ReentrantKeil($INSTANCE_NAME . "_SetOVFaultThreshold")`;
    uint16 `$INSTANCE_NAME`_GetOVFaultThreshold(uint8 voltageNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultThreshold")`;
    void `$INSTANCE_NAME`_SetOVGlitchFilterLength(uint8 filterLength) `=ReentrantKeil($INSTANCE_NAME . "_SetOVGlitchFilterLength")`;
    uint8 `$INSTANCE_NAME`_GetOVGlitchFilterLength(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVGlitchFilterLength")`;
    void `$INSTANCE_NAME`_SetOVDac(uint8 voltageNum, uint8 dacValue) `=ReentrantKeil($INSTANCE_NAME . "_SetOVDac")`;
    uint8 `$INSTANCE_NAME`_GetOVDac(uint8 voltageNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOVDac")`;
    void `$INSTANCE_NAME`_SetOVDacDirect(uint8 dacValue) `=ReentrantKeil($INSTANCE_NAME . "_SetOVDacDirect")`;
    uint32 `$INSTANCE_NAME`_GetOVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultStatus")`;
    uint8 `$INSTANCE_NAME`_GetOVDacDirect(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVDacDirect")`;
#endif /* ((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)|| \
           (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)) */

#if((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)|| \
    (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY))

    void `$INSTANCE_NAME`_SetUVFaultThreshold(uint8 voltageNum,uint16 uvFaultThreshold) `=ReentrantKeil($INSTANCE_NAME . "_SetUVFaultThreshold")`;
    uint16 `$INSTANCE_NAME`_GetUVFaultThreshold(uint8 voltageNum) `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultThreshold")`;
    void `$INSTANCE_NAME`_SetUVGlitchFilterLength(uint8 filterLength) `=ReentrantKeil($INSTANCE_NAME . "_SetUVGlitchFilterLength")`;
    uint8 `$INSTANCE_NAME`_GetUVGlitchFilterLength(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVGlitchFilterLength")`;
    void `$INSTANCE_NAME`_SetUVDac(uint8 voltageNum, uint8 dacValue) `=ReentrantKeil($INSTANCE_NAME . "_SetUVDac")`;
    uint8 `$INSTANCE_NAME`_GetUVDac(uint8 voltageNum) `=ReentrantKeil($INSTANCE_NAME . "_GetUVDac")`;
    void `$INSTANCE_NAME`_SetUVDacDirect(uint8 dacValue) `=ReentrantKeil($INSTANCE_NAME . "_SetUVDacDirect")`;
    uint8 `$INSTANCE_NAME`_GetUVDacDirect(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVDacDirect")`;
#endif /* ((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)|| \
           (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)) */

uint32 `$INSTANCE_NAME`_GetUVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultStatus")`;
void `$INSTANCE_NAME`_Pause(void) `=ReentrantKeil($INSTANCE_NAME . "_Pause")`;
void `$INSTANCE_NAME`_Resume(void) `=ReentrantKeil($INSTANCE_NAME . "_Resume")`;
void `$INSTANCE_NAME`_ComparatorCal(uint8 compType) `=ReentrantKeil($INSTANCE_NAME . "_ComparatorCal")`;


/***************************************
*             Registers
***************************************/

#if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)

    #define `$INSTANCE_NAME`_OV_THRESHOLD_ADDR    (`$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
    #define `$INSTANCE_NAME`_UV_THRESHOLD_ADDR    (`$INSTANCE_NAME`_VDAC_UV_viDAC8__D)
    #define `$INSTANCE_NAME`_OV_GF_ADDR           (`$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
    #define `$INSTANCE_NAME`_UV_GF_ADDR           (`$INSTANCE_NAME`_bVFD_VFDp_u0__A1_REG)
    
#endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */

#if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)

    #define `$INSTANCE_NAME`_OV_THRESHOLD_ADDR    `$INSTANCE_NAME`_VDAC_OV_viDAC8__D
    #define `$INSTANCE_NAME`_OV_GF_ADDR           `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG
#endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */

#if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)

    #define `$INSTANCE_NAME`_UV_THRESHOLD_ADDR    `$INSTANCE_NAME`_VDAC_UV_viDAC8__D
    #define `$INSTANCE_NAME`_UV_GF_ADDR           `$INSTANCE_NAME`_bVFD_VFDp_u0__A1_REG
#endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */

/* Generic DMA Configuration parameters */
#define `$INSTANCE_NAME`_BYTES_PER_BURST      (1u)
#define `$INSTANCE_NAME`_REQUEST_PER_BURST    (1u)
#define `$INSTANCE_NAME`_SRC_BASE             (CYDEV_PERIPH_BASE)
#define `$INSTANCE_NAME`_DST_BASE             (CYDEV_SRAM_BASE)

#define `$INSTANCE_NAME`_VS_CNT_AUX_CONTROL_REG       (*(reg8 *) `$INSTANCE_NAME`_bVFD_VSCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_VS_CNT_AUX_CONTROL_PTR       ( (reg8 *) `$INSTANCE_NAME`_bVFD_VSCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_CYCLE_CNT_AUX_CONTROL_REG    (*(reg8 *)`$INSTANCE_NAME`_bVFD_CycleCounter__CONTROL_AUX_CTL_REG)
#define `$INSTANCE_NAME`_CYCLE_CNT_AUX_CONTROL_PTR    ( (reg8 *)`$INSTANCE_NAME`_bVFD_CycleCounter__CONTROL_AUX_CTL_REG)

#if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
 
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A1_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A1_REG)

    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D1_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D1_REG)

    #define `$INSTANCE_NAME`_OV_VDAC_DATA_REG    (* (reg8 *) `$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
    #define `$INSTANCE_NAME`_OV_VDAC_DATA_PTR    (  (reg8 *) `$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
    #define `$INSTANCE_NAME`_UV_VDAC_DATA_REG    (* (reg8 *) `$INSTANCE_NAME`_VDAC_UV_viDAC8__D)
    #define `$INSTANCE_NAME`_UV_VDAC_DATA_PTR    (  (reg8 *) `$INSTANCE_NAME`_VDAC_UV_viDAC8__D)
#endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */

#if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
 
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
   
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D0_REG)
   
    #define `$INSTANCE_NAME`_OV_VDAC_DATA_REG    (* (reg8 *) `$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
    #define `$INSTANCE_NAME`_OV_VDAC_DATA_PTR    (  (reg8 *) `$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
#endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */

#if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
 
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A0_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A1_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__A1_REG)

    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D0_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_REG    (* (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D1_REG)
    #define `$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR    (  (reg8 *) `$INSTANCE_NAME`_bVFD_VFDp_u0__D1_REG)

    #define `$INSTANCE_NAME`_OV_VDAC_DATA_REG    (* (reg8 *) `$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
    #define `$INSTANCE_NAME`_OV_VDAC_DATA_PTR    (  (reg8 *) `$INSTANCE_NAME`_VDAC_OV_viDAC8__D)
    #define `$INSTANCE_NAME`_UV_VDAC_DATA_REG    (* (reg8 *) `$INSTANCE_NAME`_VDAC_UV_viDAC8__D)
    #define `$INSTANCE_NAME`_UV_VDAC_DATA_PTR    (  (reg8 *) `$INSTANCE_NAME`_VDAC_UV_viDAC8__D)
#endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */


#define `$INSTANCE_NAME`_CONTROL_REG         (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$CtrlModeReplacementString`_CtrlReg__CONTROL_REG)
#define `$INSTANCE_NAME`_CONTROL_PTR         (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$CtrlModeReplacementString`_CtrlReg__CONTROL_REG)

#if(`$INSTANCE_NAME`_Amount_of_Voltages <= 8u)
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    
#endif /* `$INSTANCE_NAME`_Amount_of_Voltages <= 8u */

#if((`$INSTANCE_NAME`_Amount_of_Voltages > 8u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 16u))
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS16_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS16_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg16__STATUS_REG)
    
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS16_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS16_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg16__STATUS_REG)
#endif /* `$INSTANCE_NAME`_Amount_of_Voltages <= 16u */

#if((`$INSTANCE_NAME`_Amount_of_Voltages > 16u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 24u))
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS16_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS16_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS24_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg24__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS24_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg24__STATUS_REG)
    
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS16_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS16_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS24_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg24__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS24_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg24__STATUS_REG)
#endif /* `$INSTANCE_NAME`_Amount_of_Voltages <= 24u */

#if((`$INSTANCE_NAME`_Amount_of_Voltages > 24u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 32u))
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS16_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS16_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS24_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg24__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS24_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg24__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS32_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg32__STATUS_REG)
    #define `$INSTANCE_NAME`_PG_STS32_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_PGStsReg32__STATUS_REG)
    
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_REG     (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS8_STATUS_PTR     (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg8__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS16_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS16_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg16__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS24_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg24__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS24_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg24__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS32_STATUS_REG    (* (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg32__STATUS_REG)
    #define `$INSTANCE_NAME`_OV_STS32_STATUS_PTR    (  (reg8 *)`$INSTANCE_NAME`_bVFD_`$StsReplacementString`_OV_OVStsReg32__STATUS_REG)    
#endif /* `$INSTANCE_NAME`_Amount_of_Voltages <= 32u */


//[] END OF FILE
