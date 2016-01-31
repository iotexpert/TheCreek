/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This header file contains registers and constants associated with the 
*  Character LCD component.
*
* Note:
* 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(`$INSTANCE_NAME`_H) /* Character LCD Header File */
#define `$INSTANCE_NAME`_H

#include "cytypes.h"
#include "cyfitter.h"
#include "device.h"

/***************************************
*        Function Prototypes
***************************************/

void `$INSTANCE_NAME`_Start();
void `$INSTANCE_NAME`_Stop();
void `$INSTANCE_NAME`_WriteControl(uint8 cByte);
void `$INSTANCE_NAME`_WriteData(uint8 dByte);
void `$INSTANCE_NAME`_PrintString(char8 *);
void `$INSTANCE_NAME`_Position(uint8 row, uint8 column);
void `$INSTANCE_NAME`_PutChar(char8 character);
void `$INSTANCE_NAME`_IsReady();
void `$INSTANCE_NAME`_WrDatNib(uint8 nibble);
void `$INSTANCE_NAME`_WrCntrlNib(uint8 nibble);
void `$INSTANCE_NAME`_DelayUS(uint8 blocks);

`$Bargraph_Prototypes_API_GEN``$CustomChar_Prototypes_API_GEN``$Conversion_Routine_Prototypes_API_GEN`
/* Clear Macro */
#define `$INSTANCE_NAME`_ClearDisplay() `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CLEAR_DISPLAY)

/* Off Macro */
#define `$INSTANCE_NAME`_DisplayOff() `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_CURSOR_OFF)

/* On Macro */
#define `$INSTANCE_NAME`_DisplayOn() `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DISPLAY_ON_CURSOR_OFF)


/***************************************
*           API Constants        
***************************************/

/* Full Byte Commands Sent as Two Nibbles */
#define `$INSTANCE_NAME`_DISPLAY_8_BIT_INIT      0x03u
#define `$INSTANCE_NAME`_DISPLAY_4_BIT_INIT      0x02u
#define `$INSTANCE_NAME`_DISPLAY_CURSOR_OFF      0x08u
#define `$INSTANCE_NAME`_CLEAR_DISPLAY           0x01u
#define `$INSTANCE_NAME`_CURSOR_AUTO_INCR_ON     0x06u
#define `$INSTANCE_NAME`_DISPLAY_CURSOR_ON       0x0Eu
#define `$INSTANCE_NAME`_DISPLAY_2_LINES_5x10    0x2Cu
#define `$INSTANCE_NAME`_DISPLAY_ON_CURSOR_OFF   0x0Cu

#define `$INSTANCE_NAME`_RESET_CURSOR_POSITION   0x03u
#define `$INSTANCE_NAME`_CURSOR_WINK             0x0Du
#define `$INSTANCE_NAME`_CURSOR_BLINK            0x0Fu
#define `$INSTANCE_NAME`_CURSOR_SH_LEFT          0x10u
#define `$INSTANCE_NAME`_CURSOR_SH_RIGHT         0x14u
#define `$INSTANCE_NAME`_CURSOR_HOME             0x02u
#define `$INSTANCE_NAME`_CURSOR_LEFT             0x04u
#define `$INSTANCE_NAME`_CURSOR_RIGHT            0x06u
/* Point to Character Generator Ram 0 */             
#define `$INSTANCE_NAME`_CGRAM_0                 0x40u
/* Point to Display Data Ram 0 */
#define `$INSTANCE_NAME`_DDRAM_0                 0x80u

/* LCD Characteristics */
#define `$INSTANCE_NAME`_CHARACTER_WIDTH         0x05u
#define `$INSTANCE_NAME`_CHARACTER_HEIGHT        0x08u

/* Masks and Offset */
#define `$INSTANCE_NAME`_DATA_MASK               0x0Fu
#define `$INSTANCE_NAME`_NIBBLE_SHIFT            0x04u

/* LCD Module Address Constants */
#define `$INSTANCE_NAME`_ROW_0_START             0x80u
#define `$INSTANCE_NAME`_ROW_1_START             0xC0u
#define `$INSTANCE_NAME`_ROW_2_START             0x94u
#define `$INSTANCE_NAME`_ROW_3_START             0xD4u

/* Bargraph and Custom Character Constants */
#define `$INSTANCE_NAME`_NONE   0x00u   /* No Custom Fonts      */
#define `$INSTANCE_NAME`_HBG    0x01u   /* Horizontal Bar Graph */
#define `$INSTANCE_NAME`_VBG    0x02u   /* Vertical Bar Graph   */
#define `$INSTANCE_NAME`_UD     0x03u   /* User Defined Fonts   */

`$CustomCharDefines_API_GEN`


/***************************************
*             Registers        
***************************************/

/* Port Register Definitions */
#define `$INSTANCE_NAME`_PORT_DR        (*(reg8 *) `$INSTANCE_NAME`_LCDPort__DR)   /* Data Output Register */
#define `$INSTANCE_NAME`_PORT_PS        (*(reg8 *) `$INSTANCE_NAME`_LCDPort__PS)   /* Pin State Register */
#define `$INSTANCE_NAME`_PORT_DM_0      (*(reg8 *) `$INSTANCE_NAME`_LCDPort__DM0)  /* Port Drive Mode 0 */
#define `$INSTANCE_NAME`_PORT_DM_1      (*(reg8 *) `$INSTANCE_NAME`_LCDPort__DM1)  /* Port Drive Mode 1 */
#define `$INSTANCE_NAME`_PORT_DM_2      (*(reg8 *) `$INSTANCE_NAME`_LCDPort__DM2)  /* Port Drive Mode 2 */


/***************************************
*       Register Constants        
***************************************/

/* First Pin on Port */
#define `$INSTANCE_NAME`_PORT_SHIFT              0x00u
#define `$INSTANCE_NAME`_PORT_MASK               0x7Fu

/**Drive Mode register values for High Z */
#define `$INSTANCE_NAME`_HIGH_Z_DM_0             0xFFu
#define `$INSTANCE_NAME`_HIGH_Z_DM_1             0x00u
#define `$INSTANCE_NAME`_HIGH_Z_DM_2             0x00u

/* Drive Mode register values for Strong */
#define `$INSTANCE_NAME`_STRONG_DM_0             0x00u
#define `$INSTANCE_NAME`_STRONG_DM_1             0xFFu
#define `$INSTANCE_NAME`_STRONG_DM_2             0xFFu

/* Pin Masks */
#define `$INSTANCE_NAME`_RS                      0x20u
#define `$INSTANCE_NAME`_RW                      0x40u
#define `$INSTANCE_NAME`_E                       0x10u
#define `$INSTANCE_NAME`_READY_BIT               0x08u

#endif /* End Character LCD Header File */


/* [] END OF FILE */