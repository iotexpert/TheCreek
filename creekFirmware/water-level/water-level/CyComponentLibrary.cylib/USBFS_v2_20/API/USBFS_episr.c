/*******************************************************************************
* File Name: `$INSTANCE_NAME`_episr.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Data endpoint Interrupt Service Routines
*
* Note:
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#include "`$INSTANCE_NAME`.h"


/***************************************
* Custom Declarations
***************************************/
/* `#START CUSTOM_DECLARATIONS` Place your declaration here */

/* `#END` */


/***************************************
* External function references
***************************************/

void `$INSTANCE_NAME`_InitComponent(uint8 device, uint8 mode) `=ReentrantKeil($INSTANCE_NAME . "_InitComponent")`;
void `$INSTANCE_NAME`_ReInitComponent(void) `=ReentrantKeil($INSTANCE_NAME . "_ReInitComponent")`;
#if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u)
    void `$INSTANCE_NAME`_MIDI_OUT_EP_Service(void) `=ReentrantKeil($INSTANCE_NAME . "_MIDI_OUT_EP_Service")`;
#endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/


/***************************************
* External references
***************************************/

extern volatile T_`$INSTANCE_NAME`_EP_CTL_BLOCK `$INSTANCE_NAME`_EP[];
extern volatile uint8 `$INSTANCE_NAME`_device;
#if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u)
    extern volatile uint8 `$INSTANCE_NAME`_midi_out_ep;
    extern volatile uint8 `$INSTANCE_NAME`_midi_in_ep;
    #if `$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE >= 256
        extern volatile uint16 `$INSTANCE_NAME`_midiInPointer;     /* Input endpoint buffer pointer */
    #else
        extern volatile uint8 `$INSTANCE_NAME`_midiInPointer;      /* Input endpoint buffer pointer */
    #endif /* End `$INSTANCE_NAME`_MIDI_IN_BUFF_SIZE >=256 */    
#endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

#if(`$INSTANCE_NAME`_EP1_ISR_REMOVE == 0u)


    /******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_1_ISR
    *******************************************************************************
    *
    * Summary:
    *  Endpoint 1 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    ******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_1_ISR)
    {
        /* `#START EP1_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP1_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP1].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP1].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP1].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                            & ~`$INSTANCE_NAME`_SIE_EP_INT_EP1_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP1)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP1_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP1_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP2_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_2_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 2 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_2_ISR)
    {
        /* `#START EP2_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP2_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP2].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP2].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP2].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP2_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO)
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP2)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP2_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP2_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP3_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_3_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 3 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_3_ISR)
    {
        /* `#START EP3_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP3_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP3].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP3].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP3].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP3_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP3)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP3_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP3_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP4_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_4_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 4 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_4_ISR)
    {
        /* `#START EP4_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP4_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP4].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP4].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP4].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP4_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP4)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP4_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP4_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP5_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_5_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 5 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_5_ISR)
    {
        /* `#START EP5_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP5_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP5].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP5].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP5].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP5_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP5)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP5_END_USER_CODE` Place your code here */

        /* `#END` */
    }
#endif   /* End `$INSTANCE_NAME`_EP5_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP6_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_6_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 6 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_6_ISR)
    {
        /* `#START EP6_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP6_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP6].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP6].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP6].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP6_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP6)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP6_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP6_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP7_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_7_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 7 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_7_ISR)
    {
        /* `#START EP7_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP7_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP7].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP7].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP7].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP7_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP7)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP7_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP7_ISR_REMOVE */


#if(`$INSTANCE_NAME`_EP8_ISR_REMOVE == 0u)

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_EP_8_ISR
    ********************************************************************************
    *
    * Summary:
    *  Endpoint 8 Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_EP_8_ISR)
    {
        /* `#START EP8_USER_CODE` Place your code here */

        /* `#END` */

        CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP8_CR0_PTR); /* Must read the mode reg */
        /* Do not toggle ISOC endpoint */
        if((`$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP8].attrib & `$INSTANCE_NAME`_EP_TYPE_MASK) !=
                                                                                    `$INSTANCE_NAME`_EP_TYPE_ISOC)
        {
            `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP8].epToggle ^= `$INSTANCE_NAME`_EPX_CNT_DATA_TOGGLE;
        }
        `$INSTANCE_NAME`_EP[`$INSTANCE_NAME`_EP8].apiEpState = `$INSTANCE_NAME`_EVENT_PENDING;
        CY_SET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR, CY_GET_REG8(`$INSTANCE_NAME`_SIE_EP_INT_SR_PTR)
                                                                        & ~`$INSTANCE_NAME`_SIE_EP_INT_EP8_MASK);

        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u) && \
            (`$INSTANCE_NAME`_EP_MM == `$INSTANCE_NAME`__EP_DMAAUTO) 
            if(`$INSTANCE_NAME`_midi_out_ep == `$INSTANCE_NAME`_EP8)
            {
                `$INSTANCE_NAME`_MIDI_OUT_EP_Service();
            }   
        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/

        /* `#START EP8_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif   /* End `$INSTANCE_NAME`_EP8_ISR_REMOVE */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_SOF_ISR
********************************************************************************
*
* Summary:
*  Start of Frame Interrupt Service Routine
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_SOF_ISR)
{
    /* `#START SOF_USER_CODE` Place your code here */

    /* `#END` */
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_BUS_RESET_ISR
********************************************************************************
*
* Summary:
*  USB Bus Reset Interrupt Service Routine.  Calls _Start with the same
*  parameters as the last USER call to _Start
*
* Parameters:
*  None.
*
* Return:
*  None.
*
*******************************************************************************/
CY_ISR(`$INSTANCE_NAME`_BUS_RESET_ISR)
{
    /* `#START BUS_RESET_USER_CODE` Place your code here */

    /* `#END` */

    `$INSTANCE_NAME`_ReInitComponent();
}


#if((`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_MANUAL) && (`$INSTANCE_NAME`_ARB_ISR_REMOVE == 0u))


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_ARB_ISR
    ********************************************************************************
    *
    * Summary:
    *  Arbiter Interrupt Service Routine
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Side effect:
    *  Search for EP8 int_status will be much slower than search for EP1 int_status.
    *
    *******************************************************************************/
    CY_ISR(`$INSTANCE_NAME`_ARB_ISR)
    {
        uint8 int_status;
        uint8 ep_status;
        uint8 ep = `$INSTANCE_NAME`_EP1;
        uint8 ptr = 0u;

        /* `#START ARB_BEGIN_USER_CODE` Place your code here */

        /* `#END` */

        int_status = `$INSTANCE_NAME`_ARB_INT_SR_REG;                   /* read Arbiter Status Register */
        `$INSTANCE_NAME`_ARB_INT_SR_REG = int_status;                   /* Clear Serviced Interrupts */

        while(int_status != 0u)
        {
            if(int_status & 1u)  /* If EpX interrupt present */
            {
                ep_status  = `$INSTANCE_NAME`_ARB_EP1_SR_PTR[ptr];       /* read Endpoint Status Register */
                /* If In Buffer Full */
                if(ep_status & `$INSTANCE_NAME`_ARB_EPX_SR_IN_BUF_FULL)
                {
                    if(`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN)
                    {
                        /* Write the Mode register */
                        `$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ptr] = `$INSTANCE_NAME`_EP[ep].epMode;
                    }
                }
                /* If DMA Grant */
                if(ep_status & `$INSTANCE_NAME`_ARB_EPX_SR_DMA_GNT)
                {
                    if(`$INSTANCE_NAME`_EP[ep].addr & `$INSTANCE_NAME`_DIR_IN)
                    {
                        if(`$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ptr] & `$INSTANCE_NAME`_ARB_EPX_CFG_IN_DATA_RDY)
                        {
                            `$INSTANCE_NAME`_ARB_EP1_CFG_PTR[ptr] &= ~`$INSTANCE_NAME`_ARB_EPX_CFG_IN_DATA_RDY;
                        }
                        #if defined(`$INSTANCE_NAME`_ENABLE_MIDI_STREAMING) && \
                                   (`$INSTANCE_NAME`_ENABLE_MIDI_API != 0u)
                            if(ep == `$INSTANCE_NAME`_midi_in_ep)
                            {   /* Clear MIDI input pointer*/
                                `$INSTANCE_NAME`_midiInPointer = 0u;
                            }
                        #endif /* End `$INSTANCE_NAME`_ENABLE_MIDI_STREAMING*/
                    }
                    else
                    {
                        /* (re)arm Out EP only for mode2 */
                        #if(`$INSTANCE_NAME`_EP_MM != `$INSTANCE_NAME`__EP_DMAAUTO)
                            `$INSTANCE_NAME`_EP[ep].apiEpState = `$INSTANCE_NAME`_NO_EVENT_PENDING;
                            /* Write the Mode register */
                            `$INSTANCE_NAME`_SIE_EP1_CR0_PTR[ptr] = `$INSTANCE_NAME`_EP[ep].epMode;
                        #endif /* End `$INSTANCE_NAME`_EP_MM */
                    }
                }

                /* `#START ARB_USER_CODE` Place your code here for handle Buffer Underflow/Overflow */

                /* `#END` */

                `$INSTANCE_NAME`_ARB_EP1_SR_PTR[ptr] = ep_status;       /* Clear Serviced events */
            }
            ptr += `$INSTANCE_NAME`_EPX_CNTX_ADDR_OFFSET;               /* prepare pointer for next EP */
            ep++;
            int_status >>= 1;
        }

        /* `#START ARB_END_USER_CODE` Place your code here */

        /* `#END` */
    }

#endif /* End `$INSTANCE_NAME`_EP_MM */


/* [] END OF FILE */
