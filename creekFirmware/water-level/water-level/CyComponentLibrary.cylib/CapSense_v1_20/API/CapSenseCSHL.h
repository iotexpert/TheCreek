/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CSHL.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides constants and parameter values for the High Level API
*  of CapSense Component.
*
* Note:
*********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(CY_CAPSENSE_CSHL_`$INSTANCE_NAME`_H) /* End CY_CAPSENSE_CSHL_`$INSTANCE_NAME`_H */
#define CY_CAPSENSE_CSHL_`$INSTANCE_NAME`_H

#include "`$INSTANCE_NAME`.h"


/***************************************
*       Types definition
***************************************/     

/********** Widget struct *************/
typedef struct  _`$INSTANCE_NAME`_Widget
{
    uint8 Type;                /* Type of widget element */
    uint8 RawOffset;        /* Offset in SlotResult array */
    uint8 ScanSlotCount;    /* Number of Slot elements */
    uint8 FingerThreshold;    /* FingerThreshold */
    uint8 NoiseThreshold;    /* NoiseThreshold */
    uint8 Debounce;            /* Debounce */
    uint8 Hysteresis;        /* Hysteresis */
    uint16 Filters;            /* Filters */
    void *AdvancedSettings;    /* Filters data and addtional settings */
}  `$INSTANCE_NAME`_Widget;

/****** Buttons Settings struct *******/
typedef struct _`$INSTANCE_NAME`_BtnSettings
{
    uint16 Raw1Median;
    uint16 Raw2Median;
    uint16 Raw1Averaging;
    uint16 Raw2Averaging;
    uint16 RawIIR;
    uint16 RawJitter;
}  `$INSTANCE_NAME`_BtnSettings;

/******* Slider Settings struct *******/
typedef struct _`$INSTANCE_NAME`_SlSettings
{
    uint32 Resolution;
    uint8 *DiplexTable;
    uint16 *Raw1Median;
    uint16 *Raw2Median;
    uint16 *Raw1Averaging;
    uint16 *Raw2Averaging;
    uint16 *RawIIR;
    uint16 *RawJitter;
    uint8 FirstTime;
    uint16 Pos1Median;
    uint16 Pos2Median;
    uint16 Pos1Averaging;
    uint16 Pos2Averaging;
    uint16 PosIIR;
    uint16 PosJitter;
}  `$INSTANCE_NAME`_SlSettings;

/****** TouchPad Settings struct ******/
typedef struct _`$INSTANCE_NAME`_TPSettings
{
    uint32 Resolution;
    uint16 *Raw1Median;
    uint16 *Raw2Median;
    uint16 *Raw1Averaging;
    uint16 *Raw2Averaging;
    uint16 *RawIIR;
    uint16 *RawJitter;
    uint8 FirstTime;
    uint16 *Pos1Median;
    uint16 *Pos2Median;
    uint16 *Pos1Averaging;
    uint16 *Pos2Averaging;
    uint16 *PosIIR;
    uint16 *PosJitter;
    uint16 *Position;
}  `$INSTANCE_NAME`_TPSettings;

/*** Matrix Buttons Settings struct ***/
typedef struct _`$INSTANCE_NAME`_MBSettings
{
    uint16 *Raw1Median;
    uint16 *Raw2Median;
    uint16 *Raw1Averaging;
    uint16 *Raw2Averaging;
    uint16 *RawIIR;
    uint16 *RawJitter;
}  `$INSTANCE_NAME`_MBSettings;


/***************************************
*        Function Prototypes 
***************************************/
`$writerCHLFunctionPrototypesLeft``$writerCHLFunctionPrototypesRight`
`$writerCHLFunctionPrototypes`
/***************************************
*           API Constants
***************************************/

/* Widgets Type */
#define `$INSTANCE_NAME`_CSHL_TYPE_BUTTON                0
#define `$INSTANCE_NAME`_CSHL_TYPE_LINEAR_SLIDER        1
#define `$INSTANCE_NAME`_CSHL_TYPE_RADIAL_SLIDER        2
#define `$INSTANCE_NAME`_CSHL_TYPE_TOUCHPAD                3
#define `$INSTANCE_NAME`_CSHL_TYPE_MATRIX_BUTTONS        4
#define `$INSTANCE_NAME`_CSHL_TYPE_PROXIMITY            5
#define `$INSTANCE_NAME`_CSHL_NO_WIDGET                    0xFFu

/* Mask for RAW and POS filters */
#define `$INSTANCE_NAME`_CSHL_RAW_MEDIAN_FILTER            0x01u
#define `$INSTANCE_NAME`_CSHL_RAW_AVERAGING_FILTER        0x02u
#define `$INSTANCE_NAME`_CSHL_RAW_IIR_FILTER_0            0x04u
#define `$INSTANCE_NAME`_CSHL_RAW_IIR_FILTER_1            0x08u
#define `$INSTANCE_NAME`_CSHL_RAW_JITTER_FILTER            0x10u
#define `$INSTANCE_NAME`_CSHL_POS_MEDIAN_FILTER            0x20u
#define `$INSTANCE_NAME`_CSHL_POS_AVERAGING_FILTER        0x40u
#define `$INSTANCE_NAME`_CSHL_POS_IIR_FILTER_0            0x80u
#define `$INSTANCE_NAME`_CSHL_POS_IIR_FILTER_1            0x100u
#define `$INSTANCE_NAME`_CSHL_POS_JITTER_FILTER            0x200u

/* Types of IIR filters */
#define `$INSTANCE_NAME`_CSHL_IIR_FILTER_0                0x00u
#define `$INSTANCE_NAME`_CSHL_IIR_FILTER_1                0x01u

/* Defines is slot active */
#define `$INSTANCE_NAME`_CSHL_SLOT_ACTIVE                0x01u

/* Defines diplex type of Slider */
#define `$INSTANCE_NAME`_CSHL_IS_DIPLEX                    0x80u

/* Defines max fingers on TouchPad  */
#define  `$INSTANCE_NAME`_CSHL_MAX_FINGERS                 1

/* Radial Slider defines */
#define `$INSTANCE_NAME`_CSHL_ROTARY_SLIDER_A360         (360)
#define `$INSTANCE_NAME`_CSHL_ROTARY_SLIDER_A270         (270)
#define `$INSTANCE_NAME`_CSHL_ROTARY_SLIDER_A180         (180)
#define `$INSTANCE_NAME`_CSHL_ROTARY_SLIDER_A90           (90)

#endif /* End CY_CAPSENSE_CSHL_`$INSTANCE_NAME`_H */ 


/* [] END OF FILE */
