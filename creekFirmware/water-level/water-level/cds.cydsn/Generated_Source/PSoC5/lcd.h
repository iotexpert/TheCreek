/*******************************************************************************
* File Name: lcd.h
* Version 1.70
*
* Description:
*  This header file contains registers and constants associated with the
*  Character LCD component.
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(CY_CHARLCD_lcd_H)
#define CY_CHARLCD_lcd_H

#include "cytypes.h"
#include "cyfitter.h"


/***************************************
*   Conditional Compilation Parameters
***************************************/

#define lcd_CONVERSION_ROUTINES     (1u)
#define lcd_CUSTOM_CHAR_SET         (0u)

/* Custom character set types */
#define lcd_NONE                     (0u)    /* No Custom Fonts      */
#define lcd_HORIZONTAL_BG            (1u)    /* Horizontal Bar Graph */
#define lcd_VERTICAL_BG              (2u)    /* Vertical Bar Graph   */
#define lcd_USER_DEFINED             (3u)    /* User Defined Fonts   */


/***************************************
*     Data Struct Definitions
***************************************/

/* Sleep Mode API Support */
typedef struct _lcd_backupStruct
{
    uint8 enableState;
} lcd_BACKUP_STRUCT;


/***************************************
*        Function Prototypes
***************************************/

void lcd_Start(void) ;
void lcd_Stop(void) ;
void lcd_WriteControl(uint8 cByte) ;
void lcd_WriteData(uint8 dByte) ;
void lcd_PrintString(char8 *string) ;
void lcd_Position(uint8 row, uint8 column) ;
void lcd_PutChar(char8 character) ;
void lcd_IsReady(void) ;
void lcd_WrDatNib(uint8 nibble) ;
void lcd_WrCntrlNib(uint8 nibble) ;
void lcd_Sleep(void) ;
void lcd_Wakeup(void) ;

#if((lcd_CUSTOM_CHAR_SET == lcd_VERTICAL_BG) || \
                (lcd_CUSTOM_CHAR_SET == lcd_HORIZONTAL_BG))

    void  lcd_LoadCustomFonts(const uint8 * customData)
                        ;

    void  lcd_DrawHorizontalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value)
                         ;

    void lcd_DrawVerticalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value)
                        ;

#endif /* ((lcd_CUSTOM_CHAR_SET == lcd_VERTICAL_BG) */

#if(lcd_CUSTOM_CHAR_SET == lcd_USER_DEFINED)

    void lcd_LoadCustomFonts(const uint8 * customData)
                            ;

#endif /* ((lcd_CUSTOM_CHAR_SET == lcd_USER_DEFINED) */

#if(lcd_CONVERSION_ROUTINES == 1u)

    /* ASCII Conversion Routines */
    void lcd_PrintHexUint8(uint8 value) ;
    void lcd_PrintHexUint16(uint16 value) ;
    void lcd_PrintDecUint16(uint16 value) ;

    #define lcd_PrintNumber(x)       lcd_PrintDecUint16(x)
    #define lcd_PrintInt8(x)         lcd_PrintHexUint8(x)
    #define lcd_PrintInt16(x)        lcd_PrintHexUint16(x)

#endif /* lcd_CONVERSION_ROUTINES == 1u */

/* Clear Macro */
#define lcd_ClearDisplay() lcd_WriteControl(lcd_CLEAR_DISPLAY)

/* Off Macro */
#define lcd_DisplayOff() lcd_WriteControl(lcd_DISPLAY_CURSOR_OFF)

/* On Macro */
#define lcd_DisplayOn() lcd_WriteControl(lcd_DISPLAY_ON_CURSOR_OFF)


/***************************************
*           API Constants
***************************************/

/* Full Byte Commands Sent as Two Nibbles */
#define lcd_DISPLAY_8_BIT_INIT       (0x03u)
#define lcd_DISPLAY_4_BIT_INIT       (0x02u)
#define lcd_DISPLAY_CURSOR_OFF       (0x08u)
#define lcd_CLEAR_DISPLAY            (0x01u)
#define lcd_CURSOR_AUTO_INCR_ON      (0x06u)
#define lcd_DISPLAY_CURSOR_ON        (0x0Eu)
#define lcd_DISPLAY_2_LINES_5x10     (0x2Cu)
#define lcd_DISPLAY_ON_CURSOR_OFF    (0x0Cu)

#define lcd_RESET_CURSOR_POSITION    (0x03u)
#define lcd_CURSOR_WINK              (0x0Du)
#define lcd_CURSOR_BLINK             (0x0Fu)
#define lcd_CURSOR_SH_LEFT           (0x10u)
#define lcd_CURSOR_SH_RIGHT          (0x14u)
#define lcd_CURSOR_HOME              (0x02u)
#define lcd_CURSOR_LEFT              (0x04u)
#define lcd_CURSOR_RIGHT             (0x06u)

/* Point to Character Generator Ram 0 */
#define lcd_CGRAM_0                  (0x40u)

/* Point to Display Data Ram 0 */
#define lcd_DDRAM_0                  (0x80u)

/* LCD Characteristics */
#define lcd_CHARACTER_WIDTH          (0x05u)
#define lcd_CHARACTER_HEIGHT         (0x08u)

#if(lcd_CONVERSION_ROUTINES == 1u)
    #define lcd_NUMBER_OF_REMAINDERS (0x05u)
    #define lcd_TEN                  (0x0Au)
#endif /* lcd_CONVERSION_ROUTINES == 1u */

/* Nibble Offset and Mask */
#define lcd_NIBBLE_SHIFT             (0x04u)
#define lcd_NIBBLE_MASK              (0x0Fu)

/* LCD Module Address Constants */
#define lcd_ROW_0_START              (0x80u)
#define lcd_ROW_1_START              (0xC0u)
#define lcd_ROW_2_START              (0x94u)
#define lcd_ROW_3_START              (0xD4u)

/* Custom Character References */
#define lcd_CUSTOM_0                 (0x00u)
#define lcd_CUSTOM_1                 (0x01u)
#define lcd_CUSTOM_2                 (0x02u)
#define lcd_CUSTOM_3                 (0x03u)
#define lcd_CUSTOM_4                 (0x04u)
#define lcd_CUSTOM_5                 (0x05u)
#define lcd_CUSTOM_6                 (0x06u)
#define lcd_CUSTOM_7                 (0x07u)

/* Other constants */
#define lcd_BYTE_UPPER_NIBBLE_SHIFT   (0x04u)
#define lcd_BYTE_LOWER_NIBBLE_MASK    (0x0Fu)
#define lcd_U16_UPPER_BYTE_SHIFT      (0x08u)
#define lcd_U16_LOWER_BYTE_MASK       (0xFFu)
#define lcd_CUSTOM_CHAR_SET_LEN       (0x40u)


/***************************************
*             Registers
***************************************/

/* Port Register Definitions */
#define lcd_PORT_DR_REG              (*(reg8 *) lcd_LCDPort__DR)   /* Data Output Register */
#define lcd_PORT_DR_PTR              ( (reg8 *) lcd_LCDPort__DR)
#define lcd_PORT_PS_REG              (*(reg8 *) lcd_LCDPort__PS)   /* Pin State Register */
#define lcd_PORT_PS_PTR              ( (reg8 *) lcd_LCDPort__PS)
#define lcd_PORT_DM0_REG             (*(reg8 *) lcd_LCDPort__DM0)  /* Port Drive Mode 0 */
#define lcd_PORT_DM0_PTR             ( (reg8 *) lcd_LCDPort__DM0)
#define lcd_PORT_DM1_REG             (*(reg8 *) lcd_LCDPort__DM1)  /* Port Drive Mode 1 */
#define lcd_PORT_DM1_PTR             ( (reg8 *) lcd_LCDPort__DM1)
#define lcd_PORT_DM2_REG             (*(reg8 *) lcd_LCDPort__DM2)  /* Port Drive Mode 2 */
#define lcd_PORT_DM2_PTR             ( (reg8 *) lcd_LCDPort__DM2)

/* These names are obsolete and will be removed in future revisions */
#define lcd_PORT_DR                  lcd_PORT_DR_REG
#define lcd_PORT_PS                  lcd_PORT_PS_REG
#define lcd_PORT_DM0                 lcd_PORT_DM0_REG
#define lcd_PORT_DM1                 lcd_PORT_DM1_REG
#define lcd_PORT_DM2                 lcd_PORT_DM2_REG


/***************************************
*       Register Constants
***************************************/

/* SHIFT must be 1 or 0 */
#define lcd_PORT_SHIFT               lcd_LCDPort__SHIFT
#define lcd_PORT_MASK                lcd_LCDPort__MASK

/* Drive Mode register values for High Z */
#define lcd_HIGH_Z_DM0               (0xFFu)
#define lcd_HIGH_Z_DM1               (0x00u)
#define lcd_HIGH_Z_DM2               (0x00u)

/* Drive Mode register values for High Z Analog */
#define lcd_HIGH_Z_A_DM0             (0x00u)
#define lcd_HIGH_Z_A_DM1             (0x00u)
#define lcd_HIGH_Z_A_DM2             (0x00u)

/* Drive Mode register values for Strong */
#define lcd_STRONG_DM0               (0x00u)
#define lcd_STRONG_DM1               (0xFFu)
#define lcd_STRONG_DM2               (0xFFu)

/* Pin Masks */
#define lcd_RS                       (0x20u << lcd_LCDPort__SHIFT)
#define lcd_RW                       (0x40u << lcd_LCDPort__SHIFT)
#define lcd_E                        (0x10u << lcd_LCDPort__SHIFT)
#define lcd_READY_BIT                (0x08u << lcd_LCDPort__SHIFT)
#define lcd_DATA_MASK                (0x0Fu << lcd_LCDPort__SHIFT)

#endif /* CY_CHARLCD_lcd_H */


/* [] END OF FILE */
