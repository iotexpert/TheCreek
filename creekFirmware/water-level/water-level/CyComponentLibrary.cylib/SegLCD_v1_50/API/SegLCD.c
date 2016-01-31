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
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"
#include <`$INSTANCE_NAME`_Chop_Clock.h>
#include <`$INSTANCE_NAME`_Int_Clock.h>
#include <`$INSTANCE_NAME``[Lcd_Dma]`dma.h>


/* Internal functions */
uint8   `$INSTANCE_NAME`_DmaConfigure(void);
void    `$INSTANCE_NAME`_DmaDispose(void);

/* This section contains look-up tables for 
* different kinds of displays. 
*/
#ifdef `$INSTANCE_NAME`_7SEG

    const uint8 CYCODE `$INSTANCE_NAME`_7SegDigits[] = \
    /* '0'   '1'    '2'    '3'    '4'    '5'    '6'    '7' */
    {0x3fu, 0x06u, 0x5bu, 0x4fu, 0x66u, 0x6du, 0x7du, 0x07u, \
        /* '8'  '9'    'A'    'B'    'C'    'D'    'E'    'F'   null */
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
        0x047fu,0x046fu,0x2200u,0x1100u,0x2100u,0x0a00u,0x0448u,0x2080u,
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
        0x2a80u,0x1462u,0x2209u,0x0039u,0x0880u,0x000fu,0x2203u,0x2008u,
        /*------------------------------------------------------------*/
        /* @       a       b       c       d       e       f       g  */
        /*------------------------------------------------------------*/
        0x0080u,0x0477u,0x150fu,0x0039u,0x110Fu,0x0079u,0x0071u,0x043du,
        /*------------------------------------------------------------*/
        /* h       i       j       k       l       m       n       o  */
        /*------------------------------------------------------------*/
        0x0476u,0x1100u,0x001eu,0x0a70u,0x0038u,0x02b6u,0x08b6u,0x003fu,
        /*------------------------------------------------------------*/
        /* p       q       r       s       t       u       v       w  */
        /*------------------------------------------------------------*/
        0x0473u,0x083fu,0x0C73u,0x046du,0x1101u,0x003eu,0x2230u,0x2836u,
        /*------------------------------------------------------------*/
        /* x       y       z       {       |       }        ~         */
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
        0x88ffu,0x080cu,0x1177u,0x103fu,0x118cu,0x21b3u,0x11fbu,0x4803u,
        /*------------------------------------------------------------*/
        /* 8       9       :       ;       <       =       >       ?  */
        /*------------------------------------------------------------*/
        0x11ffu,0x11bfu,0x4400u,0x8400u,0x2800u,0x1130u,0x8200u,0x5006u,
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
        0xaa00u,0x4A00u,0x8833u,0x4412u,0x2200u,0x4421u,0x8806u,0x0030u,
        /*------------------------------------------------------------*/
        /* @       a       b       c       d       e       f       g  */
        /*------------------------------------------------------------*/
        0x0200u,0x11cfu,0x543fu,0x00f3u,0x443fu,0x01f3u,0x01c3u,0x10fbu,
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
    
        const uint8 CYCODE `$INSTANCE_NAME`_charDotMatrix[][5u] = {
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x3eu, 0x1cu, 0x1cu, 0x08u, 0x08u}, {0x08u, 0x08u, 0x1cu, 0x1cu, 0x3eu},
        {0x02u, 0x06u, 0x08u, 0x10u, 0x20u}, {0x00u, 0x4fu, 0x00u, 0x4fu, 0x00u}, {0x06u, 0x0fu, 0x7fu, 0x01u, 0x7fu},
        {0x48u, 0x56u, 0x55u, 0x35u, 0x09u}, {0x0cu, 0x0cu, 0x0cu, 0x0cu, 0x0cu}, {0x10u, 0x38u, 0x54u, 0x10u, 0x1fu},
        {0x04u, 0x02u, 0x7fu, 0x02u, 0x04u}, {0x10u, 0x20u, 0x7fu, 0x20u, 0x10u}, {0x7fu, 0x3eu, 0x1cu, 0x08u, 0x7fu},
        {0x7fu, 0x08u, 0x1cu, 0x3eu, 0x7fu}, {0x7fu, 0x08u, 0x2au, 0x1cu, 0x08u}, {0x08u, 0x1cu, 0x2au, 0x08u, 0x7fu},
        {0x02u, 0x0eu, 0x3eu, 0x0eu, 0x02u}, {0x20u, 0x38u, 0x3eu, 0x38u, 0x20u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x4fu, 0x00u, 0x00u}, {0x00u, 0x07u, 0x00u, 0x07u, 0x00u}, {0x14u, 0x7fu, 0x14u, 0x7fu, 0x14u},
        {0x24u, 0x2au, 0x7fu, 0x2au, 0x12u}, {0x23u, 0x13u, 0x08u, 0x64u, 0x62u}, {0x36u, 0x49u, 0x55u, 0x22u, 0x50u},
        {0x00u, 0x05u, 0x03u, 0x00u, 0x00u}, {0x00u, 0x1cu, 0x22u, 0x41u, 0x00u}, {0x00u, 0x41u, 0x22u, 0x1cu, 0x00u},
        {0x14u, 0x08u, 0x3eu, 0x08u, 0x14u}, {0x08u, 0x08u, 0x3eu, 0x08u, 0x08u}, {0x00u, 0x50u, 0x30u, 0x00u, 0x00u},
        {0x08u, 0x08u, 0x08u, 0x08u, 0x08u}, {0x00u, 0x60u, 0x60u, 0x00u, 0x00u}, {0x20u, 0x10u, 0x08u, 0x04u, 0x02u},
        {0x3eu, 0x51u, 0x49u, 0x45u, 0x3eu}, {0x00u, 0x42u, 0x7fu, 0x40u, 0x00u}, {0x42u, 0x61u, 0x51u, 0x49u, 0x46u},
        {0x21u, 0x41u, 0x45u, 0x4Bu, 0x31u}, {0x18u, 0x14u, 0x12u, 0x7fu, 0x10u}, {0x27u, 0x45u, 0x45u, 0x45u, 0x39u},
        {0x3cu, 0x4au, 0x49u, 0x49u, 0x30u}, {0x01u, 0x71u, 0x09u, 0x05u, 0x03u}, {0x36u, 0x49u, 0x49u, 0x49u, 0x36u},
        {0x06u, 0x49u, 0x49u, 0x29u, 0x1eu}, {0x00u, 0x36u, 0x36u, 0x00u, 0x00u}, {0x00u, 0x56u, 0x36u, 0x00u, 0x00u},
        {0x08u, 0x14u, 0x22u, 0x41u, 0x00u}, {0x14u, 0x14u, 0x14u, 0x14u, 0x14u}, {0x00u, 0x41u, 0x22u, 0x14u, 0x08u},
        {0x02u, 0x01u, 0x51u, 0x09u, 0x06u}, {0x32u, 0x49u, 0x79u, 0x41u, 0x3eu}, {0x7eu, 0x11u, 0x11u, 0x11u, 0x7eu},
        {0x7fu, 0x49u, 0x49u, 0x49u, 0x36u}, {0x3eu, 0x41u, 0x41u, 0x41u, 0x22u}, {0x7fu, 0x41u, 0x41u, 0x22u, 0x1cu},
        {0x7fu, 0x49u, 0x49u, 0x49u, 0x41u}, {0x7fu, 0x09u, 0x09u, 0x09u, 0x01u}, {0x3eu, 0x41u, 0x49u, 0x49u, 0x3au},
        {0x7fu, 0x08u, 0x08u, 0x08u, 0x7fu}, {0x00u, 0x41u, 0x7fu, 0x41u, 0x00u}, {0x20u, 0x40u, 0x41u, 0x3fu, 0x01u},
        {0x7fu, 0x08u, 0x14u, 0x22u, 0x41u}, {0x7fu, 0x40u, 0x40u, 0x40u, 0x40u}, {0x7fu, 0x02u, 0x0cu, 0x02u, 0x7fu},
        {0x7fu, 0x04u, 0x08u, 0x10u, 0x7fu}, {0x3eu, 0x41u, 0x41u, 0x41u, 0x3eu}, {0x7fu, 0x09u, 0x09u, 0x09u, 0x06u},
        {0x3eu, 0x41u, 0x51u, 0x21u, 0x5eu}, {0x7fu, 0x09u, 0x19u, 0x29u, 0x46u}, {0x46u, 0x49u, 0x49u, 0x49u, 0x31u},
        {0x01u, 0x01u, 0x7fu, 0x01u, 0x01u}, {0x3fu, 0x40u, 0x40u, 0x40u, 0x3fu}, {0x1fu, 0x20u, 0x40u, 0x20u, 0x1fu},
        {0x3fu, 0x40u, 0x38u, 0x40u, 0x3fu}, {0x63u, 0x14u, 0x08u, 0x14u, 0x63u}, {0x07u, 0x08u, 0x70u, 0x08u, 0x07u},
        {0x61u, 0x51u, 0x49u, 0x45u, 0x43u}, {0x00u, 0x7fu, 0x41u, 0x41u, 0x00u}, {0x15u, 0x16u, 0x7cu, 0x16u, 0x15u},
        {0x00u, 0x41u, 0x41u, 0x7fu, 0x00u}, {0x04u, 0x02u, 0x01u, 0x02u, 0x04u}, {0x40u, 0x40u, 0x40u, 0x40u, 0x40u},
        {0x00u, 0x01u, 0x02u, 0x04u, 0x00u}, {0x20u, 0x54u, 0x54u, 0x54u, 0x78u}, {0x7fu, 0x48u, 0x44u, 0x44u, 0x38u},
        {0x38u, 0x44u, 0x44u, 0x44u, 0x40u}, {0x38u, 0x44u, 0x44u, 0x48u, 0x7fu}, {0x38u, 0x54u, 0x54u, 0x54u, 0x18u},
        {0x08u, 0x7eu, 0x09u, 0x01u, 0x02u}, {0x0cu, 0x52u, 0x52u, 0x52u, 0x3eu}, {0x7fu, 0x08u, 0x04u, 0x04u, 0x78u},
        {0x00u, 0x44u, 0x7du, 0x40u, 0x00u}, {0x20u, 0x40u, 0x40u, 0x3du, 0x00u}, {0x7fu, 0x10u, 0x28u, 0x44u, 0x00u},
        {0x00u, 0x41u, 0x7fu, 0x40u, 0x00u}, {0x7cu, 0x04u, 0x18u, 0x04u, 0x78u}, {0x7cu, 0x08u, 0x04u, 0x04u, 0x78u},
        {0x38u, 0x44u, 0x44u, 0x44u, 0x38u}, {0x7cu, 0x14u, 0x14u, 0x14u, 0x08u}, {0x08u, 0x14u, 0x14u, 0x18u, 0x7cu},
        {0x7cu, 0x08u, 0x04u, 0x04u, 0x08u}, {0x48u, 0x54u, 0x54u, 0x54u, 0x20u}, {0x04u, 0x3fu, 0x44u, 0x40u, 0x20u},
        {0x3cu, 0x40u, 0x40u, 0x20u, 0x7cu}, {0x1cu, 0x20u, 0x40u, 0x20u, 0x1cu}, {0x3cu, 0x40u, 0x30u, 0x40u, 0x3cu},
        {0x44u, 0x28u, 0x10u, 0x28u, 0x44u}, {0x0Cu, 0x50u, 0x50u, 0x50u, 0x3Cu}, {0x44u, 0x64u, 0x54u, 0x4cu, 0x44u},
        {0x00u, 0x08u, 0x36u, 0x41u, 0x00u}, {0x00u, 0x00u, 0x7fu, 0x00u, 0x00u}, {0x00u, 0x41u, 0x36u, 0x08u, 0x00u},
        {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x08u, 0x08u, 0x2au, 0x1cu, 0x08u}, {0x44u, 0x44u, 0x5fu, 0x44u, 0x44u},
        {0x22u, 0x14u, 0x08u, 0x14u, 0x22u}, {0x1cu, 0x3eu, 0x3eu, 0x3eu, 0x1cu}, {0x7fu, 0x41u, 0x41u, 0x41u, 0x7fu},
        {0x7fu, 0x5bu, 0x41u, 0x5fu, 0x7fu}, {0x7fu, 0x4du, 0x55u, 0x59u, 0x7fu}, {0x1du, 0x15u, 0x17u, 0x00u, 0x00u},
        {0x15u, 0x15u, 0x1fu, 0x00u, 0x00u}, {0x17u, 0x08u, 0x74u, 0x56u, 0x5du}, {0x17u, 0x08u, 0x24u, 0x32u, 0x79u},
        {0x35u, 0x1fu, 0x28u, 0x34u, 0x7au}, {0x08u, 0x14u, 0x2au, 0x14u, 0x22u}, {0x22u, 0x14u, 0x2au, 0x14u, 0x08u},
        {0x08u, 0x04u, 0x08u, 0x10u, 0x08u}, {0x14u, 0x0au, 0x14u, 0x28u, 0x14u}, {0x2au, 0x55u, 0x2au, 0x55u, 0x2au},
        {0x24u, 0x3bu, 0x2au, 0x7eu, 0x2au}, {0x40u, 0x3fu, 0x15u, 0x15u, 0x7fu}, {0x46u, 0x20u, 0x1fu, 0x20u, 0x46u},
        {0x24u, 0x14u, 0x7fu, 0x18u, 0x24u}, {0x24u, 0x14u, 0x7fu, 0x14u, 0x24u}, {0x44u, 0x6au, 0x79u, 0x6au, 0x44u},
        {0x40u, 0x44u, 0x7fu, 0x44u, 0x40u}, {0x7fu, 0x49u, 0x49u, 0x49u, 0x7fu}, {0x02u, 0x4cu, 0x30u, 0x0cu, 0x02u},
        {0x04u, 0x04u, 0x3cu, 0x04u, 0x44u}, {0x49u, 0x55u, 0x7fu, 0x55u, 0x49u}, {0x3au, 0x45u, 0x45u, 0x45u, 0x39u},
        {0x40u, 0x3eu, 0x10u, 0x10u, 0x1eu}, {0x08u, 0x54u, 0x3eu, 0x15u, 0x08u}, {0x7fu, 0x7fu, 0x7fu, 0x7fu, 0x7fu},
        {0x55u, 0x2au, 0x55u, 0x2au, 0x55u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u}, {0x70u, 0x50u, 0x70u, 0x00u, 0x00u},
        {0x00u, 0x00u, 0x0fu, 0x01u, 0x01u}, {0x40u, 0x40u, 0x70u, 0x00u, 0x00u}, {0x10u, 0x20u, 0x40u, 0x00u, 0x00u},
        {0x00u, 0x18u, 0x18u, 0x00u, 0x00u}, {0x0au, 0x0au, 0x4au, 0x2au, 0x1eu}, {0x04u, 0x44u, 0x34u, 0x14u, 0x0cu},
        {0x20u, 0x10u, 0x78u, 0x04u, 0x00u}, {0x18u, 0x08u, 0x4cu, 0x48u, 0x38u}, {0x48u, 0x48u, 0x78u, 0x48u, 0x48u},
        {0x48u, 0x28u, 0x18u, 0x78u, 0x08u}, {0x08u, 0x7cu, 0x08u, 0x28u, 0x18u}, {0x40u, 0x48u, 0x48u, 0x78u, 0x40u},
        {0x54u, 0x54u, 0x54u, 0x7cu, 0x00u}, {0x18u, 0x00u, 0x58u, 0x40u, 0x38u}, {0x08u, 0x08u, 0x08u, 0x08u, 0x08u},
        {0x01u, 0x41u, 0x3du, 0x09u, 0x07u}, {0x10u, 0x08u, 0x78u, 0x02u, 0x01u}, {0x06u, 0x02u, 0x43u, 0x22u, 0x1eu},
        {0x42u, 0x42u, 0x7eu, 0x42u, 0x42u}, {0x22u, 0x12u, 0x0au, 0x7fu, 0x02u}, {0x42u, 0x3fu, 0x02u, 0x42u, 0x3eu},
        {0x0au, 0x0au, 0x7fu, 0x0au, 0x0au}, {0x08u, 0x46u, 0x42u, 0x22u, 0x1eu}, {0x04u, 0x03u, 0x42u, 0x3eu, 0x02u},
        {0x42u, 0x42u, 0x42u, 0x42u, 0x7eu}, {0x02u, 0x4fu, 0x22u, 0x1fu, 0x02u}, {0x4au, 0x4au, 0x40u, 0x20u, 0x1cu},
        {0x42u, 0x22u, 0x12u, 0x2au, 0x46u}, {0x02u, 0x3fu, 0x42u, 0x4au, 0x46u}, {0x06u, 0x48u, 0x40u, 0x20u, 0x1eu},
        {0x08u, 0x46u, 0x4au, 0x32u, 0x1eu}, {0x0au, 0x4au, 0x3eu, 0x09u, 0x08u}, {0x0eu, 0x00u, 0x4eu, 0x20u, 0x1eu},
        {0x04u, 0x45u, 0x3du, 0x05u, 0x04u}, {0x00u, 0x7fu, 0x08u, 0x10u, 0x00u}, {0x44u, 0x24u, 0x1fu, 0x04u, 0x04u},
        {0x40u, 0x42u, 0x42u, 0x42u, 0x40u}, {0x42u, 0x2au, 0x12u, 0x2au, 0x06u}, {0x22u, 0x12u, 0x7au, 0x16u, 0x22u},
        {0x00u, 0x40u, 0x20u, 0x1fu, 0x00u}, {0x78u, 0x00u, 0x02u, 0x04u, 0x78u}, {0x3fu, 0x44u, 0x44u, 0x44u, 0x44u},
        {0x02u, 0x42u, 0x42u, 0x22u, 0x1eu}, {0x04u, 0x02u, 0x04u, 0x08u, 0x30u}, {0x32u, 0x02u, 0x7fu, 0x02u, 0x32u},
        {0x02u, 0x12u, 0x22u, 0x52u, 0x0eu}, {0x00u, 0x2au, 0x2au, 0x2au, 0x40u}, {0x38u, 0x24u, 0x22u, 0x20u, 0x1eu},
        {0x40u, 0x28u, 0x10u, 0x28u, 0x06u}, {0x0au, 0x3eu, 0x4au, 0x4au, 0x4au}, {0x04u, 0x7fu, 0x04u, 0x14u, 0x0cu},
        {0x40u, 0x42u, 0x42u, 0x7eu, 0x40u}, {0x4au, 0x4au, 0x4au, 0x4au, 0x7eu}, {0x04u, 0x05u, 0x45u, 0x15u, 0x1cu},
        {0x0fu, 0x40u, 0x20u, 0x1fu, 0x00u}, {0x7cu, 0x00u, 0x7eu, 0x40u, 0x30u}, {0x7eu, 0x40u, 0x20u, 0x10u, 0x08u},
        {0x7eu, 0x7eu, 0x7eu, 0x7eu, 0x7eu}, {0x0eu, 0x02u, 0x42u, 0x22u, 0x1eu}, {0x42u, 0x42u, 0x40u, 0x20u, 0x18u},
        {0x02u, 0x04u, 0x01u, 0x02u, 0x00u}, {0x07u, 0x05u, 0x07u, 0x00u, 0x00u}, {0x38u, 0x44u, 0x48u, 0x30u, 0x4cu},
        {0x20u, 0x55u, 0x55u, 0x55u, 0x38u}, {0xfeu, 0x15u, 0x15u, 0x15u, 0x0au}, {0x28u, 0x54u, 0x54u, 0x44u, 0x20u},
        {0xffu, 0x10u, 0x10u, 0x08u, 0x1fu}, {0x38u, 0x44u, 0x4cu, 0x54u, 0x24u}, {0xfcu, 0x12u, 0x11u, 0x11u, 0x0eu},
        {0x0eu, 0xa1u, 0xa1u, 0xa1u, 0x7fu}, {0x20u, 0x40u, 0x3cu, 0x04u, 0x04u}, {0x04u, 0x04u, 0x00u, 0x0eu, 0x00u},
        {0x40u, 0x80u, 0x81u, 0x7fu, 0x00u}, {0x0au, 0x04u, 0x0au, 0x00u, 0x00u}, {0x18u, 0x24u, 0x7eu, 0x24u, 0x10u},
        {0x14u, 0x7fu, 0x54u, 0x40u, 0x40u}, {0x7cu, 0x0au, 0x05u, 0x05u, 0x78u}, {0x38u, 0x45u, 0x45u, 0x45u, 0x38u},
        {0xffu, 0x12u, 0x11u, 0x11u, 0x0eu}, {0x0eu, 0x11u, 0x11u, 0x12u, 0xffu}, {0x3cu, 0x4au, 0x4au, 0x4au, 0x3cu},
        {0x30u, 0x28u, 0x10u, 0x28u, 0x18u}, {0x58u, 0x64u, 0x04u, 0x64u, 0x58u}, {0x3cu, 0x41u, 0x40u, 0x21u, 0x7cu},
        {0x63u, 0x55u, 0x4au, 0x41u, 0x41u}, {0x44u, 0x3cu, 0x04u, 0x7cu, 0x44u}, {0x45u, 0x29u, 0x11u, 0x29u, 0x45u},
        {0x0fu, 0x90u, 0x90u, 0x90u, 0x7fu}, {0x14u, 0x14u, 0x7cu, 0x14u, 0x12u}, {0x44u, 0x3cu, 0x14u, 0x14u, 0x74u},
        {0x7cu, 0x14u, 0x1cu, 0x14u, 0x7cu}, {0x10u, 0x10u, 0x54u, 0x10u, 0x10u}, {0x00u, 0x00u, 0x00u, 0x00u, 0x00u},
        {0xffu, 0xffu, 0xffu, 0xffu, 0xffu}};
    
    #endif /* `$INSTANCE_NAME`_DOT_MATRIX */

#endif /* ALPHANUMERIC */

uint8 `$INSTANCE_NAME`_initVar = 0u;

/* The one and only - Frame Buffer */
static uint8 `$INSTANCE_NAME`_buffer[`$INSTANCE_NAME`_BUFFER_LENGTH];

/* Array of common port TDs */
static uint8 `$INSTANCE_NAME`_lcdTd[`$INSTANCE_NAME`_LCD_TD_SIZE * `$INSTANCE_NAME`_COMM_NUM];
/* DMA channel dedicated for SegLCD commons */
static uint8 `$INSTANCE_NAME`_lcdChannel;

static uint8 `$INSTANCE_NAME`_termOut = (`$INSTANCE_NAME`_Lcd_Dma__TERMOUT0_EN ? TD_TERMOUT0_EN : 0u) | \
                                        (`$INSTANCE_NAME`_Lcd_Dma__TERMOUT1_EN ? TD_TERMOUT1_EN : 0u);


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
    /* LCD Frame Buffer initialization. Need to clear display for normal work */
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

    /* Set LO2 High current mode */
    #if(`$INSTANCE_NAME`_LOW_DRIVE_MODE == `$INSTANCE_NAME`_HI_RANGE)
        `$INSTANCE_NAME`_DRIVER_CONTROL_REG |= `$INSTANCE_NAME`_HI_RANGE_VAL;
    #endif /* $INSTANCE_NAME`_LOW_DRIVE_MODE */
    
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
*  Enables the components. Enables all required clocks and sets initial values
*  for registers and performs component reset.
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
uint8 `$INSTANCE_NAME`_Enable(void)
{
    uint8   status = CYRET_LOCKED;
    uint8   interruptState;
    
    /* Enable the LCD hardware */
    
    interruptState = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_PWR_MGR_REG |= `$INSTANCE_NAME`_LCD_EN;
    `$INSTANCE_NAME`_PWR_MGR_STBY_REG |= `$INSTANCE_NAME`_LCD_STBY_EN;
    
    `$INSTANCE_NAME`_LCDDAC_CONTROL_REG &= ~`$INSTANCE_NAME`_LCDDAC_DIS;
    
    CyExitCriticalSection(interruptState);
    
    status = `$INSTANCE_NAME`_DmaConfigure();
    
    /* Enable required clocks */
    `$INSTANCE_NAME`_CNT_DELAY_REG = `$INSTANCE_NAME`_CNT_DELAY_VAL;
    `$INSTANCE_NAME`_EN_HI_DELAY_REG = `$INSTANCE_NAME`_EN_HI_ACT_VAL;
    `$INSTANCE_NAME``[Int_Clock]`Enable();
    `$INSTANCE_NAME``[Chop_Clock]`Enable();
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_CLK_ENABLE;
    
    interruptState = CyEnterCriticalSection();
    
    `$INSTANCE_NAME`_FRAME_CNT7_PERIOD_REG |= `$INSTANCE_NAME`_CNTR7_ENABLE;

    CyExitCriticalSection(interruptState);
    
    #if(`$INSTANCE_NAME`_DRIVER_POWER_MODE == `$INSTANCE_NAME`_LOW_POWER)
        `$INSTANCE_NAME`_LOW_DRIVE_DELAY_REG = `$INSTANCE_NAME`_LOW_POWER_DELAY_VAL;
    #endif /* `$INSTANCE_NAME`_DRIVER_POWER_MODE */

    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_RESET;
    `$INSTANCE_NAME`_CNT_PERIOD_REG = `$INSTANCE_NAME`_CNT_PERIOD_VAL;
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_POST_RESET;
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_RESET;
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_POST_RESET;
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DmaConfigure
********************************************************************************
*
* Summary:
*  Configures DMA. Allocates required number of TDs and configures the, enables 
*  DMA channels.
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
uint8 `$INSTANCE_NAME`_DmaConfigure(void)
{
    uint8   status = CYRET_LOCKED;
    uint8   i = 0u, j, errorCnt = 0u;
    uint32  srcAddr, dstAddr;
    uint8   comTdCount;
    
    /* Initialization of TDs for the LCD data transfers. */
    srcAddr = `$INSTANCE_NAME`_DMA_ADDRESS_MASK & ((uint32) `$INSTANCE_NAME`_buffer);
    dstAddr = (dstAddr | 0xFFFFFFFFu) & `$INSTANCE_NAME`_ALIASED_AREA_PTR;
            
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
*  Releases allocated TDs desables DMA Channels.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_DmaDispose(void)
{
    uint8 i;

    /* Release all allocated TDs */
    for(i = 0u; i < (`$INSTANCE_NAME`_LCD_TD_SIZE * `$INSTANCE_NAME`_COMM_NUM); i++)
    {
        CyDmaTdFree(`$INSTANCE_NAME`_lcdTd[i]);
    }
    /* Release DMA handle */
    `$INSTANCE_NAME``[Lcd_Dma]`DmaRelease();
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
*  CYRET_SUCCESS - Function completed successfully.
*  CYRET_LOCKED - The object was locked, already in use. Some of TDs or
*                 a channel already i use.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar : used to check initial configuration, modified on 
*  first function call.
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
*  to avoid damage from DC offsets. Does not clear the frame buffer.
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
    uint8 interruptState;
    
    interruptState = CyEnterCriticalSection();
   
    `$INSTANCE_NAME`_LCDDAC_CONTROL_REG |= `$INSTANCE_NAME`_LCDDAC_DIS ;
    /* Disable LCD hardware */
    `$INSTANCE_NAME`_PWR_MGR_REG &= ~`$INSTANCE_NAME`_LCD_EN;
    `$INSTANCE_NAME`_PWR_MGR_STBY_REG &= ~`$INSTANCE_NAME`_LCD_STBY_EN;
    
    CyExitCriticalSection(interruptState);

    /* Release DMA channels and TDs */
    `$INSTANCE_NAME`_DmaDispose();
     
    CyDmaChFree(`$INSTANCE_NAME`_lcdChannel);
    
    /* Set off component enable signal and stop all clocks */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_CLK_ENABLE;
    `$INSTANCE_NAME``[Int_Clock]`Disable();
    `$INSTANCE_NAME``[Chop_Clock]`Disable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_EnableInt
********************************************************************************
*
* Summary:
*  Enables the LCD "every subframe" interrupt . Not required if `$INSTANCE_NAME`
*  _Start called
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
*  Disables the LCD "every subframe" interrupt. Not required if `$INSTANCE_NAME`
*  _Stop called.
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
*  This function sets the bias level for the LCD glass to one of up to 128 
*  values. The actual number of values is limited by the Analog supply voltage 
*  Vdda as the bias voltage can not exceed Vdda. Changing the bias level affects
*  the LCD contrast.
*
* Parameters:
*  biasLevel : bias level for the diplay.
*
* Return:
*  Defines the standard return values used PSoC content CYRET_BAD_PARAM - 
*  Function completed successfully CYRET_SUCCESS - One or more parameters to 
*  the function were invalid.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_SetBias(uint8 biasLevel) `=ReentrantKeil($INSTANCE_NAME . "_SetBias")`
{
    uint8 status = CYRET_BAD_PARAM;
    
    /* Check if parameter is valid and set it in case of success */
    if(biasLevel < 128u)
    {
        `$INSTANCE_NAME`_CONTRAST_CONTROL_REG = biasLevel;
        status = CYRET_SUCCESS;
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
*  invertState: the values can be - 0 (#LCD_NORMAL_STATE) for normal non-
*  inverted display and 1 (#LCD_INVERTED_STATE) for inverted display.
*
* Return:
*  Defines the standard return values used PSoC content CYRET_BAD_PARAM -
*  Function completed successfully, CYRET_SUCCESS - One or more parameters to 
*  the function were invalid.
*
* Theory:
*  This function enables hardware invertor which inverts the data on the 
*  outputs of port data registers. As the inversion occurs on all outputs no 
*  mater is this a common or segment pin the frame buffer data for commons 
*  requires to be changed to prevent DC ofsset in the LCD. So depending on the 
*  function argument commons are initialized in the proper way.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WriteInvertState(uint8 invertState)
{
    uint8 status = CYRET_BAD_PARAM;
    uint16 pixelNumber, j;
    uint8 i;

    /* Return error when invertState is not in the allowed range */
    if(invertState <= `$INSTANCE_NAME`_INVERTED_STATE)
    {
        /* If parameter is valid then procced with driver control register write
        * operation, we don't want to affect other bits then invert bit.
        */
        if(invertState == `$INSTANCE_NAME`_NORMAL_STATE)
        {
            #ifdef `$INSTANCE_NAME`_GANG

                for(i = 0u; i<`$INSTANCE_NAME`_COMM_NUM; i++)
                {
                    pixelNumber = `$INSTANCE_NAME`_gCommons[i];
                    
                    for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
                    {
                        pixelNumber =  (pixelNumber & ~`$INSTANCE_NAME`_ROW_MASK) | (j << `$INSTANCE_NAME`_ROW_SHIFT);
                        /* Cast to viod is used to suppress possible compilation warnings */
                        (void) `$INSTANCE_NAME`_WritePixel(pixelNumber, `$INSTANCE_NAME`_PIXEL_STATE_OFF);
                    }
                }
    
                for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
                {
                    (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_gCommons[i], `$INSTANCE_NAME`_PIXEL_STATE_ON);
                }
                
            #endif /* `$INSTANCE_NAME`_GANG */

            for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
            {
                pixelNumber = `$INSTANCE_NAME`_commons[i];
                
                for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
                {
                    pixelNumber =  (pixelNumber & ~`$INSTANCE_NAME`_ROW_MASK) | (j << `$INSTANCE_NAME`_ROW_SHIFT);
                    (void) `$INSTANCE_NAME`_WritePixel(pixelNumber, `$INSTANCE_NAME`_PIXEL_STATE_OFF);
                }
            }

            /* Reinitialize the commons */
            for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
            {
                (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_commons[i], `$INSTANCE_NAME`_PIXEL_STATE_ON);
            }    
        }
        else /* Initialization of Inverted display */
        {
            #ifdef `$INSTANCE_NAME`_GANG
    
                for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
                {
                    pixelNumber = `$INSTANCE_NAME`_gCommons[i];
                    for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
                    {
                        pixelNumber =  (pixelNumber & ~`$INSTANCE_NAME`_ROW_MASK) | (j << `$INSTANCE_NAME`_ROW_SHIFT);
                        (void) `$INSTANCE_NAME`_WritePixel(pixelNumber, `$INSTANCE_NAME`_PIXEL_STATE_ON);
                    }
                }
        
                for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
                {
                    (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_gCommons[i], `$INSTANCE_NAME`_PIXEL_STATE_OFF);
                }
            #endif /* `$INSTANCE_NAME`_GANG */
    
            for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
            {
                pixelNumber = `$INSTANCE_NAME`_commons[i];
                
                for(j = 0u; j < `$INSTANCE_NAME`_COMM_NUM; j++)
                {
                    pixelNumber =  (pixelNumber & ~`$INSTANCE_NAME`_ROW_MASK) | (j << `$INSTANCE_NAME`_ROW_SHIFT);
                    (void) `$INSTANCE_NAME`_WritePixel(pixelNumber, `$INSTANCE_NAME`_PIXEL_STATE_ON);
                }
            }
    
            /* Reinitialize the commons */
            for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
            {
                (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_commons[i], `$INSTANCE_NAME`_PIXEL_STATE_OFF);
            }
        }

        `$INSTANCE_NAME`_DRIVER_CONTROL_REG = (`$INSTANCE_NAME`_DRIVER_CONTROL_REG & `$INSTANCE_NAME`_STATE_MASK) \
                | (invertState << `$INSTANCE_NAME`_INVERT_SHIFT);
        
        status = CYRET_SUCCESS;
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
*  State of the LCD 0 (#`$INSTANCE_NAME`_NORMAL_STATE) for normal non-inverted 
*  display and 1 (#`$INSTANCE_NAME`_INVERTED_STATE) for inverted display.
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
    
    /* Initialize ganged commons */
    #ifdef `$INSTANCE_NAME`_GANG
        
        for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
        {
            (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_gCommons[i], `$INSTANCE_NAME`_PIXEL_STATE_ON);
        } 
        
    #endif /* `$INSTANCE_NAME`_GANG */

    /* Reinitialize the commons */
    for(i = 0u; i < `$INSTANCE_NAME`_COMM_NUM; i++)
    {
        (void) `$INSTANCE_NAME`_WRITE_PIXEL(`$INSTANCE_NAME`_commons[i], `$INSTANCE_NAME`_PIXEL_STATE_ON);
    }
    
    dispState = (`$INSTANCE_NAME`_DRIVER_CONTROL_REG & ~`$INSTANCE_NAME`_STATE_MASK) >> `$INSTANCE_NAME`_INVERT_SHIFT;
    
    /* If we were in inverted state before the display was cleared, 
    * then we must call WriteInvertState() as there is no separate
    * API to clear inverted display.
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
*  This function sets or clears a pixel based on pixelState
*  in a large frame buffer. The Pixel is addressed by a packed
*  number. User code can also directly write the frame buffer RAM
*  to create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the pixels
*  location in the frame buffer. The lowest three bits in the LSB
*  low nibble are the bit position in the byte, the LSB upper
*  nibble (4 bits) is the byte address in the multiplex row and
*  the MSB low nibble (4 bits) is the multiplex row number.
*  pixelState : 0 for pixel off,1 for pixel on, 2 for pixel invert
*
* Return:
*  Status returns the standardized return value for pass
*  or fail on a range check of the byte address and multiplex row
*  number. No check is performed on bit position.
*
* Reentrant:
*  No.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_WritePixel(uint16 pixelNumber, uint8 pixelState)
{
    uint8 mask, row, port, pin;
    uint8 status = CYRET_BAD_PARAM;

    if(pixelNumber != `$INSTANCE_NAME`_NOT_CON)
    {
        /* Let the User know he is entered wrong parameter */
        if(pixelState > `$INSTANCE_NAME`_PIXEL_STATE_INVERT)
        {
            status = CYRET_BAD_PARAM;
        }    
        else
        {
            if(pixelState == `$INSTANCE_NAME`_PIXEL_STATE_INVERT)
            {
                /* Invert actual pixel state */
                pixelState = !(`$INSTANCE_NAME`_ReadPixel(pixelNumber));
            }
    
            row = `$INSTANCE_NAME`_EXTRACT_ROW(pixelNumber);
            port = `$INSTANCE_NAME`_EXTRACT_PORT(pixelNumber);
            pin = `$INSTANCE_NAME`_EXTRACT_PIN(pixelNumber);
            
            mask = ~(`$INSTANCE_NAME`_PIXEL_STATE_ON << pin);
            
            `$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_MAX_BUFF_ROWS + port] = \
                (`$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_MAX_BUFF_ROWS + port] & mask) | (pixelState << pin);
            
            status = CYRET_SUCCESS;
        }
    }
    
    return(status);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPixel
********************************************************************************
*
* Summary:
*  This function reads a pixels state in a frame
*  buffer. The Pixel is addressed by a packed number. User code can also
*  directly read the frame buffer RAM to create optimized interactions.
*
* Parameters:
*  pixelNumber: is the packed number that points to the
*  pixels location in the frame buffer. The lowest three bits in the LSB
*  low nibble are the bit position in the byte, the LSB upper nibble (4
*  bits) is the byte address in the multiplex row and the MSB low
*  nibble (4 bits) is the multiplex row number.
*
* Return:
*  Pixel State - 0 for pixel off,1 for pixel on.
*
*******************************************************************************/
uint8 `$INSTANCE_NAME`_ReadPixel(uint16 pixelNumber) `=ReentrantKeil($INSTANCE_NAME . "_ReadPixel")`
{
    uint8 pixelState, row, port, pin;
    
    row = `$INSTANCE_NAME`_EXTRACT_ROW(pixelNumber);
    port = `$INSTANCE_NAME`_EXTRACT_PORT(pixelNumber);
    pin = `$INSTANCE_NAME`_EXTRACT_PIN(pixelNumber);

    pixelState = ((`$INSTANCE_NAME`_buffer[row * `$INSTANCE_NAME`_MAX_BUFF_ROWS + port] >> pin) & \
                   `$INSTANCE_NAME`_PIXEL_STATE_ON);
    
    return(pixelState);
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetAwakeMode
********************************************************************************
*
* Summary:
*  When in Low Power mode, set LCD Driver buffers output to hi impendance.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetAwakeMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SetAwakeMode")`
{
    `$INSTANCE_NAME`_DRIVER_CONTROL_REG &= ~`$INSTANCE_NAME`_SLEEP_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SetSleepMode
********************************************************************************
*
* Summary:
*   When in Low Power mode, set LCD Driver buffers output to ground.
*
* Parameters:
*  void
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_SetSleepMode(void) `=ReentrantKeil($INSTANCE_NAME . "_SetSleepMode")`
{
    `$INSTANCE_NAME`_DRIVER_CONTROL_REG |= `$INSTANCE_NAME`_SLEEP_ENABLE;
}


`$writerCFunctions`


/* [] END OF FILE */
