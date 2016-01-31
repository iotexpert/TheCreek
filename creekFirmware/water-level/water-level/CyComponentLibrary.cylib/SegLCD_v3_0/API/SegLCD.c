/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the API source code for the Segment LCD component.
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
#include <`$INSTANCE_NAME`_Int_Clock.h>
#include <`$INSTANCE_NAME``[Lcd_Dma]`dma.h>
#include <`$INSTANCE_NAME``[Frame_Dma]`dma.h>


/* Internal functions */
uint8   `$INSTANCE_NAME`_DmaConfigure(void);
void    `$INSTANCE_NAME`_DmaDispose(void);
void    `$INSTANCE_NAME`_WriteControlReg(uint8 value, uint8 mask);

/* This section contains look-up tables for 
* different kinds of displays. 
*/
#ifdef `$INSTANCE_NAME`_7SEG

    const uint8 CYCODE `$INSTANCE_NAME`_7SegDigits[] = \
    /*    '0'    '1'    '2'    '3'    '4'    '5'    '6'    '7' */
        {0x3fu, 0x06u, 0x5bu, 0x4fu, 0x66u, 0x6du, 0x7du, 0x07u, \
    /*    '8'    '9'    'A'    'B'    'C'    'D'    'E'    'F'   null */
        0x7fu, 0x6fu, 0x77u, 0x7cu, 0x39u, 0x5eu, 0x79u, 0x71u, 0x00u};

#endif /* `$INSTANCE_NAME`_7SEG */

#ifdef ALPHANUMERIC

    #ifdef `$INSTANCE_NAME`_14SEG
    
        const uint16 CYCODE `$INSTANCE_NAME`_14SegChars[] = {
        /*------------------------------------------------------------*/
        /*                           Blank                            */
        /*------------------------------------------------------------*/
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        /*------------------------------------------------------------*/
        /*         !       "       #       $       %       &       '  */
        /*------------------------------------------------------------*/
        0x0000u,0x0006u,0x0120u,0x3fffu,0x156du,0x2ee4u,0x2a8du,0x0200u,
        /*------------------------------------------------------------*/
        /* (       )       *       +       ,       -       .       /  */
        /*------------------------------------------------------------*/
        0x0a00u,0x2080u,0x3fc0u,0x1540u,0x2000u,0x0440u,0x1058u,0x2200u,
        /*------------------------------------------------------------*/
        /* 0       1       2       3       4       5       6       7  */
        /*------------------------------------------------------------*/
        0x223fu,0x0206u,0x045bu,0x040fu,0x0466u,0x0869u,0x047du,0x1201u,
        /*------------------------------------------------------------*/
        /* 8       9       :       ;       <       =       >       ?  */
        /*------------------------------------------------------------*/
        0x047fu,0x046fu,0x1100u,0x2100u,0x0a00u,0x0448u,0x2080u,0x1423u,
        /*------------------------------------------------------------*/
        /* @       A       B       C       D       E       F       G  */
        /*------------------------------------------------------------*/
        0x053bu,0x0477u,0x150fu,0x0039u,0x110Fu,0x0079u,0x0071u,0x043du,
        /*------------------------------------------------------------*/
        /* H       I       J       K       L       M       N       O  */
        /*------------------------------------------------------------*/
        0x0476u,0x1100u,0x001eu,0x0a70u,0x0038u,0x02b6u,0x08b6u,0x003fu,
        /*------------------------------------------------------------*/
        /* P       Q       R       S       T       U       V       W  */
        /*------------------------------------------------------------*/
        0x0473u,0x083fu,0x0C73u,0x046du,0x1101u,0x003eu,0x2230u,0x2836u,
        /*------------------------------------------------------------*/
        /* X       Y       Z       [       \       ]       ^       _  */
        /*------------------------------------------------------------*/
        0x2a80u,0x1462u,0x2209u,0x0039u,0x0880u,0x000fu,0x0003u,0x0008u,
        /*------------------------------------------------------------*/
        /* @       a       b       c       d       e       f       g  */
        /*------------------------------------------------------------*/
        0x053bu,0x0477u,0x150fu,0x0039u,0x110Fu,0x0079u,0x0071u,0x043du,
        /*------------------------------------------------------------*/
        /* h       i       j       k       l       m       n       o  */
        /*------------------------------------------------------------*/
        0x0476u,0x1100u,0x001eu,0x0a70u,0x0038u,0x02b6u,0x08b6u,0x003fu,
        /*------------------------------------------------------------*/
        /* p       q       r       s       t       u       v       w  */
        /*------------------------------------------------------------*/
        0x0473u,0x083fu,0x0C73u,0x046du,0x1101u,0x003eu,0x2230u,0x2836u,
        /*------------------------------------------------------------*/
        /* x       y       z       {       |       }        ~      O  */
        /*------------------------------------------------------------*/
        0x2a80u,0x1280u,0x2209u,0x0e00u,0x1100u,0x20C0u,0x0452u,0x003fu};
    
    #endif /* `$INSTANCE_NAME`_14SEG */

    #ifdef `$INSTANCE_NAME`_16SEG
    
        const uint16 CYCODE `$INSTANCE_NAME`_16SegChars[] = {
        /*------------------------------------------------------------*/
        /*                           Blank                            */
        /*------------------------------------------------------------*/
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,0x0000u,
        /*------------------------------------------------------------*/
        /*         !       "       #       $       %       &       '  */
        /*------------------------------------------------------------*/
        0x0000u,0x000cu,0x0480u,0xffffu,0x55bbu,0xdd99u,0xaa3bu,0x0800u,
        /*------------------------------------------------------------*/
        /* (       )       *       +       ,       -       .       /  */
        /*------------------------------------------------------------*/
        0x2800u,0x8200u,0xff00u,0x5500u,0x8000u,0x1100u,0x4160u,0x8800u,
        /*------------------------------------------------------------*/
        /* 0       1       2       3       4       5       6       7  */
        /*------------------------------------------------------------*/
        0x88ffu,0x000cu,0x1177u,0x103fu,0x118cu,0x21b3u,0x11fbu,0x4803u,
        /*------------------------------------------------------------*/
        /* 8       9       :       ;       <       =       >       ?  */
        /*------------------------------------------------------------*/
        0x11ffu,0x11bfu,0x4400u,0x8400u,0x2800u,0x1130u,0x8200u,0x5087u,
        /*------------------------------------------------------------*/
        /* @       A       B       C       D       E       F       G  */
        /*------------------------------------------------------------*/
        0x14f7u,0x11cfu,0x543fu,0x00f3u,0x443fu,0x01f3u,0x01c3u,0x10fbu,
        /*------------------------------------------------------------*/
        /* H       I       J       K       L       M       N       O  */
        /*------------------------------------------------------------*/
        0x11ccu,0x4400u,0x007eu,0x29c0u,0x00f0u,0x0accu,0x22ccu,0x00ffu,
        /*------------------------------------------------------------*/
        /* P       Q       R       S       T       U       V       W  */
        /*------------------------------------------------------------*/
        0x11c7u,0x20ffu,0x31c7u,0x11bbu,0x4403u,0x00fcu,0x88c0u,0xa0ccu,
        /*------------------------------------------------------------*/
        /* X       Y       Z       [       \       ]       ^       _  */
        /*------------------------------------------------------------*/
        0xaa00u,0x5184u,0x8833u,0x4412u,0x2200u,0x4421u,0x0006u,0x0030u,
        /*------------------------------------------------------------*/
        /* @       a       b       c       d       e       f       g  */
        /*------------------------------------------------------------*/
        0x14f7u,0x11cfu,0x543fu,0x00f3u,0x443fu,0x01f3u,0x01c3u,0x10fbu,
        /*------------------------------------------------------------*/
        /* h       i       j       k       l       m       n       o  */
        /*------------------------------------------------------------*/
        0x11ccu,0x4400u,0x007eu,0x29c0u,0x00f0u,0x0accu,0x22ccu,0x00ffu,
        /*------------------------------------------------------------*/
        /* p       q       r       s       t       u       v       w  */
        /*------------------------------------------------------------*/
        0x11c7u,0x20ffu,0x31c7u,0x11bbu,0x4403u,0x00fcu,0x88c0u,0xa0ccu,
        /*------------------------------------------------------------*/
        /* x       y       z       {       |       }        ~         */
        /*------------------------------------------------------------*/
        0xaa00u,0x4A00u,0x8833u,0x3800u,0x4400u,0x8300u,0x1144u,0x0000u};
    
    #endif /* `$INSTANCE_NAME`_16SEG */

    #ifdef `$INSTANCE_NAME`_DOT_MATRIX
        `$charDotMatrix`
    #endif /* `$INSTANCE_NAME`_DOT_MATRIX */
#endif /* ALPHANUMERIC */

uint8 `$INSTANCE_NAME`_initVar = 0u;

/* Stores DMA  channel number used for Frame signal transaction */
static uint8 `$INSTANCE_NAME`_frameChannel;

/* The one and only - Frame Buffer */
static uint8 `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_BUFFER_LENGTH];

/* Array of common port TDs */
static uint8 `$INSTANCE_NAME`_lcdTd[`$INSTANCE_NAME`_LCD_TD_SIZE * `$INSTANCE_NAME`_COMM_NUM];

/* DMA channel dedicated for SegLCD commons */
static uint8 `$INSTANCE_NAME`_lcdChannel;

static uint8 `$INSTANCE_NAME`_termOut = (`$INSTANCE_NAME`_Lcd_Dma__TERMOUT0_EN ? TD_TERMOUT0_EN : 0u) | \
                                        (`$INSTANCE_NAME`_Lcd_Dma__TERMOUT1_EN ? TD_TERMOUT1_EN : 0u);

static uint8 `$INSTANCE_NAME`_frameTermOut = (`$INSTANCE_NAME`_Frame_Dma__TERMOUT0_EN ? TD_TERMOUT0_EN : 0u) | \
                                             (`$INSTANCE_NAME`_Frame_Dma__TERMOUT1_EN ? TD_TERMOUT1_EN : 0u);


/* Start of customizer generated code */
`$writerCVariables`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Perform initialization of the component. Configures and enables all required
*  hardware blocks, clears frame buffer.   
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void)
{
    /* LCD Timer configuration */
    #if(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_ILO)
        
        /* Select ILO as a wake up source */
        `$INSTANCE_NAME`_TIMER_CONTROL_REG = `$INSTANCE_NAME`_TIMER_CONTROL_REG & ~`$INSTANCE_NAME`_TIMER_CLK_SEL_MASK;
        /* Set period */
        `$INSTANCE_NAME`_TIMER_CONTROL_REG = (`$INSTANCE_NAME`_TIMER_CONTROL_REG & \
                                                ~`$INSTANCE_NAME`_TIMER_PERIOD_MASK) \
                                               | (`$INSTANCE_NAME`_TIMER_PERIOD << `$INSTANCE_NAME`_TIMER_PERIOD_SHIFT);
            
    #elif(`$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_32XTAL)
        /* One PPS must be enabled to generate 8Khz clock for LCD Timer */
        `$INSTANCE_NAME`_TM_WL_GFG_REG |= `$INSTANCE_NAME`_ONE_PPS_EN;
        /* Select 8K tap from OPPS timer as a wake up source */
        `$INSTANCE_NAME`_TIMER_CONTROL_REG = `$INSTANCE_NAME`_TIMER_CONTROL_REG | `$INSTANCE_NAME`_TIMER_32XTAL_SEL;
        /* Set period */
        `$INSTANCE_NAME`_TIMER_CONTROL_REG = (`$INSTANCE_NAME`_TIMER_CONTROL_REG & \
                                                ~`$INSTANCE_NAME`_TIMER_PERIOD_MASK) \
                                               | (`$INSTANCE_NAME`_TIMER_PERIOD << `$INSTANCE_NAME`_TIMER_PERIOD_SHIFT);
        
    #endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__LOW_POWER_ILO */
    
    /* Need to clear display to start normal work */
    `$INSTANCE_NAME`_ClearDisplay();

    /* Select LCD DAC generated voltage as the source for LCD Driver */
    `$INSTANCE_NAME`_LCDDAC_SWITCH_REG0_REG = `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL;
    `$INSTANCE_NAME`_LCDDAC_SWITCH_REG1_REG = `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL;
    `$INSTANCE_NAME`_LCDDAC_SWITCH_REG2_REG = `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL;
    `$INSTANCE_NAME`_LCDDAC_SWITCH_REG3_REG = `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL;
    `$INSTANCE_NAME`_LCDDAC_SWITCH_REG4_REG = `$INSTANCE_NAME`_LCDDAC_VOLTAGE_SEL;

    /* Turn on the LCD DAC and set the bias type */
    `$INSTANCE_NAME`_LCDDAC_CONTROL_REG = (`$INSTANCE_NAME`_LCDDAC_CONTROL_REG & `$INSTANCE_NAME`_BIAS_TYPE_MASK) \
        | `$INSTANCE_NAME`_BIAS_TYPE;

    /* Set the contrast level for LCD with value chosen in the GUI */
    `$INSTANCE_NAME`_SetBias(`$INSTANCE_NAME`_BIAS_VOLTAGE);
    
    /* Set lower bit of HI Drive strength to LCD Driver control register */
    `$INSTANCE_NAME`_DRIVER_CONTROL_REG |= ((`$INSTANCE_NAME`_HIDRIVE_STRENGTH << `$INSTANCE_NAME`_LCDDRV_MODE0_SHIFT) \
                                            & `$INSTANCE_NAME`_LCDDRV_MODE0_MASK);
    
    /* Set two high bits of HI Drive strength */
    `$INSTANCE_NAME`_WriteControlReg((`$INSTANCE_NAME`_HIDRIVE_STRENGTH & `$INSTANCE_NAME`_MODE_MASK), \
                                     `$INSTANCE_NAME`_CNTL_MODE_MASK);
    
    /* ISR initialization */
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);

    /* Set the ISR to point to the RTC_SUT_isr Interrupt. */
    CyIntSetVector(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR);

    /* Set the priority. */
    CyIntSetPriority(`$INSTANCE_NAME`_ISR_NUMBER, `$INSTANCE_NAME`_ISR_PRIORITY);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the power to LCD fixed hardware and enables generation of UDB 
*  signals.
*
* Parameters:
*  None.
*
* Return:
*  Status one of standard status for PSoC3 Component:
*      CYRET_SUCCESS - function completed successfully.
*      CYRET_LOCKED - the object was locked, already in use. Some of TDs or
*                     a channel already in use.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Enable(void)
{
    uint8 status = CYRET_LOCKED;
    uint8 interruptState;
    
    interruptState = CyEnterCriticalSection();
    
    /* Enable the LCD hardware */
    `$INSTANCE_NAME`_PWR_MGR_REG |= `$INSTANCE_NAME`_LCD_EN;
    `$INSTANCE_NAME`_PWR_MGR_STBY_REG |= `$INSTANCE_NAME`_LCD_STBY_EN;
   
    CyExitCriticalSection(interruptState);
    
    /* Enable internal clock */
    `$INSTANCE_NAME``[Int_Clock]`Enable();
    
    status = `$INSTANCE_NAME`_DmaConfigure();
        
    /* Set LCD PWM period */
    `$INSTANCE_NAME`_PWM_PERIOD_REG = `$INSTANCE_NAME`_PWM_PERIOD_VAL;
    `$INSTANCE_NAME`_PWM_DRIVE_REG = `$INSTANCE_NAME`_PWM_DRIVE_VAL;
    `$INSTANCE_NAME`_PWM_LODRIVE_REG = `$INSTANCE_NAME`_PWM_LOWDRIVE_VAL;
        
    `$INSTANCE_NAME`_WriteControlReg(`$INSTANCE_NAME`_CNTL_FRAME_DONE, `$INSTANCE_NAME`_CNTL_FRAME_DONE_MASK);
    
    /* Global UDB enable, will be sent by DMA */
    `$INSTANCE_NAME`_WriteControlReg(`$INSTANCE_NAME`_CNTL_CLK_EN, `$INSTANCE_NAME`_CNTL_CLK_EN_MASK);
    
    /* Global UDB enable, this required to start normal work as 
    * it enables first DMA request. 
    */ 
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CNTL_CLK_EN;
        
    #if(`$INSTANCE_NAME`_POWER_MODE != `$INSTANCE_NAME`__NO_SLEEP)     
    
        /* Enable generation of lp_ack signal from UBD to PM */
        `$INSTANCE_NAME`_LCDDAC_CONTROL_REG |= `$INSTANCE_NAME`_LCDDAC_UDB_LP_EN;
        
        /* Start the LCD timer if in one of Low Power modes */
        `$INSTANCE_NAME`_TIMER_CONTROL_REG |= `$INSTANCE_NAME`_TIMER_EN;
    
    #endif /* `$INSTANCE_NAME`_POWER_MODE == `$INSTANCE_NAME`__NO_SLEEP */

    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DmaConfigure
********************************************************************************
*
* Summary:
*  Configures DMA for the components usage, transfering dasplay data and 
*  generating frame signal. Allocates required number of TDs and configures 
*  them, enables DMA channels.
*
* Parameters:
*  None.
*
* Return:
*  Status one of standard status for PSoC3 Component
*       CYRET_SUCCESS - function completed successfully.
*       CYRET_LOCKED - the object was locked, already in use. Some of TDs or
*                      a channel already in use.
*
* Global variables:
*  `$INSTANCE_NAME`_lcdTd[] – used to store a set of allocated TD IDs that are 
*  used by this component.
*
*  `$INSTANCE_NAME`_lcdChannel – stores a DMA Channel allocated for an LCD 
*  usage.
*  
*  `$INSTANCE_NAME`_termOut – used as a parameter for LCD DMA configuration API 
*  to select proper TermOut signal that was previously calculated.
*
*  `$INSTANCE_NAME`_frameTd[] - used to store a set of allocated TD IDs that 
*  are used by this component for Frame signal generation.
*
*  `$INSTANCE_NAME`_frameTermOut - used as a parameter for Frame DMA 
*  configuration API to select proper TermOut signal that was previously 
*  calculated.
*
*  `$INSTANCE_NAME`_LcdPort_DMA_TD_PROTO_BLOCK[] - used as a parameter for 
*  DMA configuration. 
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_DmaConfigure(void)
{
    uint8   status = CYRET_LOCKED;
    uint8   i = 0u;
    uint8   j;
    uint8   errorCnt = 0u;
    uint32  srcAddr;
    uint32  dstAddr;
    uint8   comTdCount;
    
    /* Take source and destination address */
    srcAddr = `$INSTANCE_NAME`_DMA_ADDRESS_MASK & ((uint32) `$INSTANCE_NAME`_frameTx);
    dstAddr =  `$INSTANCE_NAME`_DMA_ADDRESS_MASK &  ((uint32) `$INSTANCE_NAME`_CONTROL_PTR);
        
    /* Get the DMA channel for the Frame and check it for validness */
    `$INSTANCE_NAME`_frameChannel = `$INSTANCE_NAME``[Frame_Dma]`DmaInitialize(0u, 0u, HI16(srcAddr), HI16(dstAddr));
    
    if(`$INSTANCE_NAME`_frameChannel == DMA_INVALID_CHANNEL)
    {
        errorCnt++;
    }
    
    /* The DMA configuration for Type A and Type B waveforms differs because of 
    *  the their specification. Type A waveforms use only two TDs no matter what 
    *  is the number of commons and Type B waveforms TDs usage is  dependent on 
    *  the number of commons.
    */ 
    #if(`$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_A)
        
        for(i=0; i < 2u; i++)
        {   /* Allocate a TD and check  it if it valid. */
            `$INSTANCE_NAME`_frameTd[i]= CyDmaTdAllocate(); 
            if(`$INSTANCE_NAME`_frameTd[i] == DMA_INVALID_TD)
            {
                errorCnt++;
            }
        }
        /* Set configuration for the first TD */
        CyDmaTdSetConfiguration(`$INSTANCE_NAME`_frameTd[0u], 
                                1u, 
                                `$INSTANCE_NAME`_frameTd[1u], 
                                TD_INC_SRC_ADR | `$INSTANCE_NAME`_frameTermOut);
        CyDmaTdSetAddress(`$INSTANCE_NAME`_frameTd[0u], LO16(srcAddr), LO16(dstAddr));
        
        
        /* Set configuration for the second TD */
        CyDmaTdSetConfiguration(`$INSTANCE_NAME`_frameTd[1u], 
                                1u, 
                                `$INSTANCE_NAME`_frameTd[0u], 
                                TD_INC_SRC_ADR | `$INSTANCE_NAME`_frameTermOut);
        CyDmaTdSetAddress(`$INSTANCE_NAME`_frameTd[1u], LO16(srcAddr + 1u), LO16(dstAddr));

    #else
        for(i = 0u; i < (`$INSTANCE_NAME`_COMM_NUM * 2u); i++)
        {
            `$INSTANCE_NAME`_frameTd[i]= CyDmaTdAllocate(); 
            if(`$INSTANCE_NAME`_frameTd[i] == DMA_INVALID_TD)
            {
                errorCnt++;
            }
        }
        
        for(i = 0u; i < (`$INSTANCE_NAME`_COMM_NUM * 2u); i++)
        {
            if(i != (`$INSTANCE_NAME`_COMM_NUM * 2u) - 1u)
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_frameTd[i],
                                        1u, 
                                        `$INSTANCE_NAME`_frameTd[i+1], 
                                        TD_INC_SRC_ADR | `$INSTANCE_NAME`_frameTermOut);
            }
            else
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_frameTd[i], 
                                        1u, 
                                        `$INSTANCE_NAME`_frameTd[0u], 
                                        TD_INC_SRC_ADR | `$INSTANCE_NAME`_frameTermOut);
            }    
            CyDmaTdSetAddress(`$INSTANCE_NAME`_frameTd[i], LO16(srcAddr) + i , LO16(dstAddr));
        }     
        
    #endif /* `$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_A */
    
    if(errorCnt == 0u)
    {
        CyDmaChSetInitialTd(`$INSTANCE_NAME`_frameChannel, `$INSTANCE_NAME`_frameTd[0u]);
        CyDmaChEnable(`$INSTANCE_NAME`_frameChannel, 1u);
    }
    
    /* Initialization of TDs for the common lines port. 
    */
    srcAddr = `$INSTANCE_NAME`_DMA_ADDRESS_MASK & ((uint32) `$INSTANCE_NAME`_buffer);
    dstAddr = `$INSTANCE_NAME`_DMA_ADDRESS_MASK & `$INSTANCE_NAME`_ALIASED_AREA_PTR;        
    
    /* add comment */
    `$INSTANCE_NAME`_lcdChannel = `$INSTANCE_NAME``[Lcd_Dma]`DmaInitialize(0u, 0u, HI16(srcAddr), HI16(dstAddr));

    if(`$INSTANCE_NAME`_lcdChannel == DMA_INVALID_CHANNEL)
    {
        errorCnt++;
    }
       
    for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
    {
        for(i = 0u; i < `$INSTANCE_NAME`_LCD_TD_SIZE; i++)
        {
            `$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i] = CyDmaTdAllocate();       
            if(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i] == DMA_INVALID_TD)
            {
                errorCnt++;
            }
        }
    }

    comTdCount = (`$INSTANCE_NAME`_COMM_NUM * `$INSTANCE_NAME`_LCD_TD_SIZE);
    
    for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
    {
        for(i = 0u; i < `$INSTANCE_NAME`_LCD_TD_SIZE; i++)
        {
            /* The last TD need to point to TD[0] */
            if((j * `$INSTANCE_NAME`_LCD_TD_SIZE + i) == (comTdCount - 1u))
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                                        `$INSTANCE_NAME`_SegLcdPort_DMA_TD_PROTO_BLOCK[i].length, \
                                        `$INSTANCE_NAME`_lcdTd[0u], \
                                        TD_INC_DST_ADR | TD_INC_SRC_ADR | `$INSTANCE_NAME`_termOut);
            }
            else if(i == (`$INSTANCE_NAME`_LCD_TD_SIZE - 1u))
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                                        `$INSTANCE_NAME`_SegLcdPort_DMA_TD_PROTO_BLOCK[i].length, \
                                        `$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i + 1u], \
                                        TD_INC_DST_ADR | TD_INC_SRC_ADR | `$INSTANCE_NAME`_termOut);
            }
            else
            {
                CyDmaTdSetConfiguration(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                                        `$INSTANCE_NAME`_SegLcdPort_DMA_TD_PROTO_BLOCK[i].length, \
                                        `$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i + 1u], \
                                        TD_INC_DST_ADR | TD_INC_SRC_ADR | TD_AUTO_EXEC_NEXT);
            }
            CyDmaTdSetAddress(`$INSTANCE_NAME`_lcdTd[j * `$INSTANCE_NAME`_LCD_TD_SIZE + i], \
                              LO16(srcAddr) + j * `$INSTANCE_NAME`_ROW_LENGTH + \
                              `$INSTANCE_NAME`_SegLcdPort_DMA_TD_PROTO_BLOCK[i].offset, \
                              LO16(dstAddr) + `$INSTANCE_NAME`_SegLcdPort_DMA_TD_PROTO_BLOCK[i].offset);
        }
    }
        
    if(errorCnt == 0u)
    {
        CyDmaChSetInitialTd(`$INSTANCE_NAME`_lcdChannel, `$INSTANCE_NAME`_lcdTd[0u]);
        CyDmaChEnable(`$INSTANCE_NAME`_lcdChannel, 1u);
    
        status = CYRET_SUCCESS;
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DmaDispose
********************************************************************************
*
* Summary:
*  Releases allocated TDs disables DMA Channels used for component operation.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_lcdTd[] - used to hold a set of allocated TD IDs that are 
*  used by this component which will be released by this function.
* 
*  `$INSTANCE_NAME`_lcdChannel- holds the channel number used by the LCD 
*  component that will be released by this function.
* 
*  `$INSTANCE_NAME`_frameTd[] - used to hold a set of allocated TD IDs that 
*  are used by this component for Frame signal generation, which will be 
*  released by this function.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DmaDispose(void)
{
    uint8 i;

    /* Release LCD DMA handle */
    `$INSTANCE_NAME``[Lcd_Dma]`DmaRelease();
    
    /* Free the channel */
    CyDmaChFree(`$INSTANCE_NAME`_lcdChannel);
    
    /* Release LCD Frame DMA handle */
    `$INSTANCE_NAME``[Frame_Dma]`DmaRelease();

    /* Free the channel */
    CyDmaChFree(`$INSTANCE_NAME`_frameChannel);
    
    #if(`$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_A)
        for(i = 0u; i < 2u; i++)
        {
            CyDmaTdFree(`$INSTANCE_NAME`_frameTd[i]);
        }     
    #else
        for(i = 0u; i < (`$INSTANCE_NAME`_COMM_NUM * 2u); i++)
        {
            CyDmaTdFree(`$INSTANCE_NAME`_frameTd[i]);
        }     
    #endif /* `$INSTANCE_NAME`_WAVEFORM_TYPE == `$INSTANCE_NAME`__TYPE_A */
        


    /* Release all allocated TDs */
    for(i = 0u; i < (`$INSTANCE_NAME`_COMM_NUM * `$INSTANCE_NAME`_LCD_TD_SIZE); i++)
    {
        CyDmaTdFree(`$INSTANCE_NAME`_lcdTd[i]);
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Starts the LCD component, DMA channels, frame buffer and hardware. Does not
*  clear the frame buffer SRAM if previously defined.
*
* Parameters:
*  None.
*
* Return:
*  Status one of standard status for PSoC3 Component
*      CYRET_SUCCESS - function completed successfully.
*      CYRET_LOCKED - the object was locked, already in use. Some of TDs or
*                     a channel already in use.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - is used to indicate initial configuration of 
*  this component. The variable is initialized to zero and set to 1 the 
*  first time `$INSTANCE_NAME`_Start() is called. This allows for component 
*  initialization without re-initialization in all subsequent calls to the 
*  `$INSTANCE_NAME`_Start() routine.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_Start(void)
{
    uint8 status = CYRET_LOCKED;
    
    /* If not Initialized then initialize all required hardware and software */
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_initVar = 1u;
        `$INSTANCE_NAME`_Init();
    }
     
    status = `$INSTANCE_NAME`_Enable();
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disables the LCD component and DMA channels. Automatically blanks the display
*  to avoid damage from DC offsets. Does not clear the frame buffer. Also it 
*  turns off the power for the entire LCD system. 
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void)
{
    uint8   interruptState;
    
    /* Global UDB disable */
    `$INSTANCE_NAME`_WriteControlReg(0u, `$INSTANCE_NAME`_CNTL_CLK_EN_MASK);
    
    #if(`$INSTANCE_NAME`_POWER_MODE != `$INSTANCE_NAME`__NO_SLEEP) 
    
        /* Disable generation of lp_ack signal from UBD to PM */
        `$INSTANCE_NAME`_LCDDAC_CONTROL_REG &= ~`$INSTANCE_NAME`_LCDDAC_UDB_LP_EN;
                
        /* Disable LCD timer */
        `$INSTANCE_NAME`_TIMER_CONTROL_REG &= ~`$INSTANCE_NAME`_TIMER_EN;
    
    #endif /* `$INSTANCE_NAME`_POWER_MODE != `$INSTANCE_NAME`__NO_SLEEP */
    
    /* Clear Continous Drive */
    `$INSTANCE_NAME`_LCDDAC_CONTROL_REG &= ~`$INSTANCE_NAME`_LCDDAC_CONT_DRIVE;
    /* Clear Continous Display Blink bit */
    `$INSTANCE_NAME`_LCDDAC_CONTROL_REG &= ~`$INSTANCE_NAME`_LCDDRV_DISPLAY_BLNK;
    /* Clear Continous Bypass Enable bit */
    `$INSTANCE_NAME`_LCDDAC_CONTROL_REG &= ~`$INSTANCE_NAME`_LCDDRV_BYPASS_EN;
 
    interruptState = CyEnterCriticalSection();
    
    /* Disable LCD fixed function HW in PM */
    `$INSTANCE_NAME`_PWR_MGR_REG &= ~`$INSTANCE_NAME`_LCD_EN;
    `$INSTANCE_NAME`_PWR_MGR_STBY_REG &= ~`$INSTANCE_NAME`_LCD_STBY_EN;
    
    CyExitCriticalSection(interruptState);
          
    /* Release DMA channels and TDs */
    `$INSTANCE_NAME`_DmaDispose();
   
    /* Set off component enable signal and stop all clocks */
    `$INSTANCE_NAME``[Int_Clock]`Disable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables the LCD "every subframe" interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_EnableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_EnableInt")`
{
    CyIntEnable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DisableInt
********************************************************************************
*
* Summary:
*  Disables the LCD "every subframe" interrupt.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DisableInt(void) `=ReentrantKeil($INSTANCE_NAME . "_DisableInt")`
{
    CyIntDisable(`$INSTANCE_NAME`_ISR_NUMBER);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetBias
********************************************************************************
*
* Summary:
*  This function sets the bias level for the LCD glass to one of up to 64 
*  values. The actual number of values is limited by the Analog supply voltage 
*  Vdda as the bias voltage can not exceed Vdda. Changing the bias level affects
*  the LCD contrast.
*
* Parameters:
*  biasLevel : bias level for the diplay.
*
* Return:
*  Status one of standard status for PSoC3 Component:
*      CYRET_SUCCESS - function completed successfully.
*      CYRET_BAD_PARAM - evaluation of biasLevel parameter is failed.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetBias(uint8 biasLevel) `=ReentrantKeil($INSTANCE_NAME . "_SetBias")`
{
    uint8 status;
    
    if(biasLevel < 64u)
    {
        `$INSTANCE_NAME`_CONTRAST_CONTROL_REG = biasLevel;
        status = CYRET_SUCCESS;
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteInvertState
********************************************************************************
*
* Summary:
*  This function inverts the display based on invertState. 
*
* Parameters:
*  invertState: the values can be - 0 (#`$INSTANCE_NAME`_NORMAL_STATE) for 
*               normal noninverted display and 1 
*                (#`$INSTANCE_NAME`_INVERTED_STATE) for inverted display.
*
* Return:
*  Status one of standard status for PSoC3 Component:
*      CYRET_SUCCESS - function completed successfully.
*      CYRET_BAD_PARAM - evaluation of invertState parameter is failed.
*
* Theory:
*  This function enables hardware invertor which inverts the data on the 
*  outputs of port data registers. As the inversion occurs on all outputs no 
*  mater is this a common or segment pin the frame buffer data for commons 
*  requires to be changed to prevent DC ofsset in the LCD. So depending on the 
*  function argument commons are initialized in the proper way.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WriteInvertState(uint8 invertState) `=ReentrantKeil($INSTANCE_NAME . "_WriteInvertState")`
{
    uint8 status;
    uint8 pixelState;
    uint16 pixelNumber;
    uint16 j;
    uint8 i;

    /* If parameter is valid then procced with driver control register write
    * operation, we don't want to affect other bits then invert bit.
    */
    if(invertState <= `$INSTANCE_NAME`_INVERTED_STATE)
    {
        if(invertState == `$INSTANCE_NAME`_NORMAL_STATE)
        {
            pixelState = `$INSTANCE_NAME`_PIXEL_STATE_OFF;
        }
        else
        {
            pixelState = `$INSTANCE_NAME`_PIXEL_STATE_ON;
        }
        
        #ifdef `$INSTANCE_NAME`_GANG

            for(i = 0u; i<`$INSTANCE_NAME`_COMM_NUM; i++)
            {
                pixelNumber = `$INSTANCE_NAME`_gCommons[i];
                   
                for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
                {
                    pixelNumber =  (pixelNumber & ~`$INSTANCE_NAME`_ROW_MASK) | (j << `$INSTANCE_NAME`_ROW_SHIFT);
                    /* Cast to viod is used to suppress possible compilation warnings */
                    `$INSTANCE_NAME`_WRITE_PIXEL(pixelNumber, pixelState);
                }
            }
    
            for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
            {
                `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_gCommons[i],  \
                                            (~pixelState & `$INSTANCE_NAME`_STATE_MASK));
            }
                
        #endif /* `$INSTANCE_NAME`_GANG */

        for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
        {
            pixelNumber = `$INSTANCE_NAME`_commons[i];
                
            for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
            {
                pixelNumber =  (pixelNumber & ~`$INSTANCE_NAME`_ROW_MASK) | (j << `$INSTANCE_NAME`_ROW_SHIFT);
                `$INSTANCE_NAME`_WRITE_PIXEL(pixelNumber, pixelState);
            }
        }

        /* Reinitialize the commons */
        for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
        {
            `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_commons[i], \
                                        (~pixelState & `$INSTANCE_NAME`_STATE_MASK));
        }    
            
        `$INSTANCE_NAME`_DRIVER_CONTROL_REG = (`$INSTANCE_NAME`_DRIVER_CONTROL_REG & `$INSTANCE_NAME`_STATE_MASK) \
            | (invertState << `$INSTANCE_NAME`_INVERT_SHIFT);
        
        status = CYRET_SUCCESS;
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadInvertState
********************************************************************************
*
* Summary:
*  This function returns the current normal or inverted state of the display.
*
* Parameters:
*  None.
*
* Return:
*  State of the LCD - 0(#`$INSTANCE_NAME`_NORMAL_STATE) for normal non-inverted 
*  display and 1(#`$INSTANCE_NAME`_INVERTED_STATE) for inverted display.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadInvertState(void) `=ReentrantKeil($INSTANCE_NAME . "_ReadInvertState")`
{
    /* Get only invert bit of Driver Control Register */
    return((`$INSTANCE_NAME`_DRIVER_CONTROL_REG & `$INSTANCE_NAME`_INVERT_BIT_MASK) >> `$INSTANCE_NAME`_INVERT_SHIFT);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ClearDisplay
********************************************************************************
*
* Summary:
*  This function clears the display RAM of the frame buffer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_buffer[] – an array is modified by this API. Clears the 
*  frame buffer to all zeroes and then reinitialize it with a 1 in the 
*  locations specified by the values from `$INSTANCE_NAME`_commons[].
*
*  `$INSTANCE_NAME`_commons[] – holds the pixel locations for common lines.
*
* Reentrant:
*  No. 
*
*******************************************************************************/
void `$INSTANCE_NAME`_ClearDisplay(void)
{
    uint16 i;
    uint8 dispState;

    /* Clear entire frame buffer to all zeroes */
    for(i = 0u; i < `$INSTANCE_NAME`_BUFFER_LENGTH; i++) 
    {
        `$INSTANCE_NAME`_buffer[i] = 0u;
    }
    
    /* Reinitialize the commons */
    for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
    {
        #ifdef `$INSTANCE_NAME`_GANG
            `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_gCommons[i], `$INSTANCE_NAME`_PIXEL_STATE_ON);
        #endif /* `$INSTANCE_NAME`_GANG */
        
        `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_commons[i], `$INSTANCE_NAME`_PIXEL_STATE_ON);
    }
    
    /* Read invert state. */
    dispState = (`$INSTANCE_NAME`_DRIVER_CONTROL_REG & ~`$INSTANCE_NAME`_STATE_MASK) >> `$INSTANCE_NAME`_INVERT_SHIFT;
    
    /* If we were in inverted state before the display was cleared, then we must
    * call WriteInvertState() as there is no separate API to clear inverted 
    * display.
    */
    if(dispState == `$INSTANCE_NAME`_INVERTED_STATE)
    {
        `$INSTANCE_NAME`_WriteInvertState(dispState);
    }   
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePixel
********************************************************************************
*
* Summary:
*  This function sets or clears a pixel based on pixelState in a frame buffer. 
*  The Pixel is addressed by a packed number. User code can also directly write
*  the frame buffer RAM to create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the pixel's location in the 
*               frame buffer. The lowest three bits in the LSB low nibble are 
*               the bit position in the byte, the LSB upper nibble (4 bits) is 
*               the byte address in the multiplex row and the MSB low nibble 
*               (4 bits) is the multiplex row number. 
*  pixelState: 0 for pixel off,1 for pixel on, 2 for pixel invert.
*
* Return:
*  Status one of standard status for PSoC3 Component:
*      CYRET_SUCCESS - function completed successfully.
*      CYRET_BAD_PARAM - evaluation of pixelNumber parameter is failed.
*
* Global variables:
*  `$INSTANCE_NAME`_buffer[] – an array is modified by this API. This API 
*  may set or clear certain bit in the array defined by the input parameter.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WritePixel(uint16 pixelNumber, uint8 pixelState)
{
    uint8 mask;
    uint8 row;
    uint8 port;
    uint8 pin;
    uint8 status;

    if( (pixelNumber != `$INSTANCE_NAME`_NOT_CON) || !(pixelState > `$INSTANCE_NAME`_PIXEL_STATE_INVERT))
    {
        if(pixelState == `$INSTANCE_NAME`_PIXEL_STATE_INVERT)
        {
            /* Invert actual pixel state */
            pixelState = !(`$INSTANCE_NAME`_ReadPixel(pixelNumber));
        }
        
        /* Extract the pixel information to locate desired pixel in the frame 
        * buffer.
        */
        row = `$INSTANCE_NAME`_EXTRACT_ROW(pixelNumber);
        port = `$INSTANCE_NAME`_EXTRACT_PORT(pixelNumber);
        pin = `$INSTANCE_NAME`_EXTRACT_PIN(pixelNumber);
        
        /* Write new pixel's value to the frame buffer. */
        mask = ~(`$INSTANCE_NAME`_PIXEL_STATE_ON << pin);
        
        /* Write new pixel's value to the frame buffer. */
        `$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_MAX_BUFF_ROWS + port] = \
            (`$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_MAX_BUFF_ROWS + port] & mask) | (pixelState << pin);
            
        status = CYRET_SUCCESS;
    }
    else
    {
        /* Let the User know he is entered wrong parameter. */
        status = CYRET_BAD_PARAM;
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPixel
********************************************************************************
*
* Summary:
*  This function reads a pixels state in a frame buffer. The Pixel is addressed 
*  by a packed number. User code can also directly read the frame buffer RAM to 
*  create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the pixel's location in the 
*               frame buffer. The lowest three bits in the LSB low nibble are 
*               the bit position in the byte, the LSB upper nibble (4 bits) is 
*               the byte address in the multiplex row and the MSB low nibble 
*               (4 bits) is the multiplex row number.
*
* Return:
*  Pixel State:
*       `$INSTANCE_NAME`_PIXEL_STATE_OFF - for pixel off.
*       `$INSTANCE_NAME`_PIXEL_STATE_ON - for pixel on.
*       `$INSTANCE_NAME`_PIXEL_UNKNOWN_STATE - for not assigned pixel.       
*
* Global variables:
*  `$INSTANCE_NAME`_buffer[] – holds the state of every pixel of the LCD glass 
*  which can be read by this function.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadPixel(uint16 pixelNumber) `=ReentrantKeil($INSTANCE_NAME . "_ReadPixel")`
{
    uint8 pixelState;
    uint8 row;
    uint8 port;
    uint8 pin;
    
    if(pixelNumber != `$INSTANCE_NAME`_NOT_CON)
    {
        /* Extract the pixel information to locate desired pixel in the frame 
        * buffer.
        */
        row = `$INSTANCE_NAME`_EXTRACT_ROW(pixelNumber);
        port = `$INSTANCE_NAME`_EXTRACT_PORT(pixelNumber);
        pin = `$INSTANCE_NAME`_EXTRACT_PIN(pixelNumber);
    
        pixelState = ((`$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_MAX_BUFF_ROWS + port] >> pin) & \
                    `$INSTANCE_NAME`_PIXEL_STATE_ON);
    }
    else
    {
        pixelState = `$INSTANCE_NAME`_PIXEL_UNKNOWN_STATE;
    }
    
    return(pixelState);
}

`$writerCFunctions`

/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteControlReg
********************************************************************************
*
* Summary: 
*  This function dedicated for internal component usage only. It used to prepare
*  the values for LCD Control Register wich will be later writen to it by DMA. 
*
* Parameters:
*  value: value to be written to Control Register.
*  mask: mask for the respective value. Mask should contain '0' in bit positions  
*        that dessired to be set. For example for the value 0x03(00000011) mask 
*        will be 0xFC(11111100).
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_frameTx[] - used to store a set of precalculated values 
*  for control register which is generated by this function.
*
* Theory:
*  Non API function. Users should never use this function directly. To save UDB 
*  resources there is only one Control Register used for LCD operation. This 
*  Control Register contains Frame bit which is used to generate Frame signal. 
*  This is a dynamic signal which is transfered by the DMA. So all other 
*  control bist will be owervriten on every DMA transaction. That's why all the 
*  data that need to be remained in the Control Register are written to array 
*  where predefined values of Frame contain.   
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteControlReg(uint8 value, uint8 mask)
{
    uint8 i;
    
    for(i = 0u; i < `$INSTANCE_NAME`_CONTROL_WRITE_COUNT; i++)
    {
        /* Write the value for control register to each element of the array. */
        `$INSTANCE_NAME`_frameTx[i] = (`$INSTANCE_NAME`_frameTx[i] & mask) | value;
    }
}

/* [] END OF FILE */
