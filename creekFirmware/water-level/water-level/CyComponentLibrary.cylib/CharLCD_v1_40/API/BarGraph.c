/*******************************************************************************
* File Name: `$INSTANCE_NAME`_BarGraph.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the custom character data and API for creating and using
*  bar graphs with the Character LCD component.
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

uint8 `$INSTANCE_NAME`_bargraphType = `$CustomCharacterSet`;
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
*
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
        /* Delay between each write */
        `$INSTANCE_NAME`_WriteData(customData[index_u8]);
    }

    `$INSTANCE_NAME`_IsReady();
    `$INSTANCE_NAME`_WriteControl(`$INSTANCE_NAME`_DDRAM_0);
}

/* Bargraph Functions */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_DrawHorizontalBG
********************************************************************************
* Summary:
*  Draws the horizontal bargraph.
*
* Parameters:
*  row:             The row in which the bar graph starts.
*  column:          The column in which the bar graph starts.
*  maxCharacters:   The max length of the graph in whole characters.
*  value:           The current length or height of the graph in pixels.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_DrawHorizontalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value)
{
    /* 8-bit Counter */
    uint8 count8 = 0;
    /* Number of full characters to draw */
    uint8 fullChars = value /  `$INSTANCE_NAME`_CHARACTER_WIDTH;
    /* Number of remaining pixels to draw */
    uint8 remainingPixels = value % `$INSTANCE_NAME`_CHARACTER_WIDTH;

    /* Ensure that the maximum character limit is followed. */
    if(fullChars >= maxCharacters)
        fullChars = maxCharacters;

    /* Put Cursor at start position */
    `$INSTANCE_NAME`_Position(row, column);

    /* Write full characters */
    for(count8 = 0; count8 < fullChars; count8++)
    {
        `$INSTANCE_NAME`_WriteData(`$INSTANCE_NAME`_CUSTOM_5);
    }

    if(fullChars < maxCharacters)
    {
        /* Write remaining pixels */
        `$INSTANCE_NAME`_WriteData(remainingPixels);

        if(fullChars < maxCharacters - 1)
        {
            /* Fill with whitespace to end of bar graph */
            for(count8 = 0; count8 < maxCharacters - fullChars - 1; count8++)
            {
                `$INSTANCE_NAME`_WriteData(`$INSTANCE_NAME`_CUSTOM_0);
            }
        }
    }
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_DrawVerticalBG
********************************************************************************
* Summary:
*  Draws the vertical bargraph.
*
* Parameters:
*  row:             The row in which the bar graph starts.
*  column:          The column in which the bar graph starts.
*  maxCharacters:   The max height of the graph in whole characters.
*  value:           The current length or height of the graph in pixels.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_DrawVerticalBG(uint8 row, uint8 column, uint8 maxCharacters, uint8 value)
{
    /* 8-bit Counter */
    uint8 count8 = 0;
    /* Current Row Tracker */
    int8 currentRow = row;
    /* Row number error.  Don't write remaining pixels.*/

    /* Number of full characters to draw */
    uint8 fullChars = value / `$INSTANCE_NAME`_CHARACTER_HEIGHT;
        /* Number of remaining pixels to draw */
    uint8 remainingPixels = value % `$INSTANCE_NAME`_CHARACTER_HEIGHT;

    /* Put Cursor at start position */
    `$INSTANCE_NAME`_Position(row, column);

    /* Make sure the bar graph fits inside the space allotted */
    if(fullChars >= maxCharacters)
    {
        fullChars = maxCharacters;
    }

    /*  Write full characters */
    while(count8 < fullChars)
    {
        `$INSTANCE_NAME`_WriteData(`$INSTANCE_NAME`_CUSTOM_7);
        count8++;
        /* Each pass through, move one row higher */
        if(row - count8 >=0)
            `$INSTANCE_NAME`_Position(row-count8, column);
        else
            break;
    }

    if(row - count8 >= 0 && fullChars < maxCharacters)
    {
        /* Write remaining pixels */
        if(remainingPixels == 0)
        {
            `$INSTANCE_NAME`_WriteData(' ');
        }
        else
        {
            `$INSTANCE_NAME`_WriteData(remainingPixels - 1);
        }

        currentRow = row - count8 - 1;

        if(currentRow >= 0)
        {
            /* Move up one row and fill with whitespace till top of bar graph */
            for(count8 = 0; count8 < maxCharacters - fullChars - 1; count8++)
            {
                `$INSTANCE_NAME`_Position(currentRow, column);
                `$INSTANCE_NAME`_WriteData(' ');
                currentRow --;
            }
        }
    }
}


/* [] END OF FILE */
