/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CustChars.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code for the Character LCD custom character
*  API.
*
* Note:
*
********************************************************************************
* Copyright (2008), Cypress Semiconductor Corporation.
********************************************************************************
* This software is owned by Cypress Semiconductor Corporation (Cypress) and is
* protected by and subject to worldwide patent protection (United States and
* foreign), United States copyright laws and international treaty provisions.
* Cypress hereby grants to licensee a personal, non-exclusive, non-transferable
* license to copy, use, modify, create derivative works of, and compile the
* Cypress Source Code and derivative works for the sole purpose of creating
* custom software in support of licensee product to be used only in conjunction
* with a Cypress integrated circuit as specified in the applicable agreement.
* Any reproduction, modification, translation, compilation, or representation of
* this software except as specified above is prohibited without the express
* written permission of Cypress.
*
* Disclaimer: CYPRESS MAKES NO WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, WITH
* REGARD TO THIS MATERIAL, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
* Cypress reserves the right to make changes without further notice to the
* materials described herein. Cypress does not assume any liability arising out
* of the application or use of any product or circuit described herein. Cypress
* does not authorize its products for use as critical components in life-support
* systems where a malfunction or failure may reasonably be expected to result in
* significant injury to the user. The inclusion of Cypress' product in a life-
* support systems application implies that the manufacturer assumes all risk of
* such use and in doing so indemnifies Cypress against all charges. Use may be
* limited by and subject to the applicable Cypress software license agreement.
*******************************************************************************/

#include "cytypes.h"
#include "`$INSTANCE_NAME`.h"

const uint8 `$INSTANCE_NAME`_customFonts[] =
{
    /* Character `$INSTANCE_NAME`_CUSTOM_0   */
 `$CUST_CHAR0`,
    /* Character `$INSTANCE_NAME`_CUSTOM_1   */
 `$CUST_CHAR1`,
    /* Character `$INSTANCE_NAME`_CUSTOM_2   */
 `$CUST_CHAR2`,
    /* Character `$INSTANCE_NAME`_CUSTOM_3   */
 `$CUST_CHAR3`,
    /* Character `$INSTANCE_NAME`_CUSTOM_4   */
 `$CUST_CHAR4`,
    /* Character `$INSTANCE_NAME`_CUSTOM_5   */
 `$CUST_CHAR5`,
    /* Character `$INSTANCE_NAME`_CUSTOM_6   */
 `$CUST_CHAR6`,
    /* Character `$INSTANCE_NAME`_CUSTOM_7   */
 `$CUST_CHAR7`
 };


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_LoadCustomFonts
********************************************************************************
* Summary:
*  Loads 8 custom font characters into the LCD Module for use.  Cannot use
*  characters from two different font sets at once, but font sets can be
*  switched out during runtime.
*
* Parameters:
*  customData: pointer to a constant array of 64 bytes representing 8 custom
*              font characters.
* Return:
*  void
*******************************************************************************/
void `$INSTANCE_NAME`_LoadCustomFonts(const uint8* customData)
{
    uint8 index_u8;

    `$INSTANCE_NAME`_IsReady();
    /* Set starting address in the LCD Module */
    /* Optionally: Read the current address to restore at a later time */
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_CGRAM_0);

    /* Load in the 64 bytes of CustomChar Data */
    for(index_u8 = 0; index_u8 < 64; index_u8++)
    {
        `$INSTANCE_NAME`_WriteData(customData[index_u8]);
    }

    `$INSTANCE_NAME`_IsReady();
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DDRAM_0);
}


/* [] END OF FILE */
