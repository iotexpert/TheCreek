/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*     The CAN Component provide functionalty of Control Area Network. 
*     Two types of mailbox configurations available "Full" and "Basic". 
*     Interrupt handlers funcitons generate accordingly to CyDesigner Wizard 
*     inputs. 
*
*   Note:
*     CAN configuration puts as constant to ROM and could change only direclty 
*     registers write by user.
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "`$INSTANCE_NAME`.h"

/* `#START CAN_C_CODE_DEFINITION` */
		
/* `#END` */

/* Iniital values of CAN RX and TX registers */
const CANRXcfg `$INSTANCE_NAME`_RXConfigStruct[] = 
{
	{ 0, 0x`$Rx0_Cmd`, 0x`$Rx0_Amr`, 0x`$Rx0_Acr` },
	{ 1, 0x`$Rx1_Cmd`, 0x`$Rx1_Amr`, 0x`$Rx1_Acr` },
	{ 2, 0x`$Rx2_Cmd`, 0x`$Rx2_Amr`, 0x`$Rx2_Acr` },
	{ 3, 0x`$Rx3_Cmd`, 0x`$Rx3_Amr`, 0x`$Rx3_Acr` },
	{ 4, 0x`$Rx4_Cmd`, 0x`$Rx4_Amr`, 0x`$Rx4_Acr` },
	{ 5, 0x`$Rx5_Cmd`, 0x`$Rx5_Amr`, 0x`$Rx5_Acr` },
	{ 6, 0x`$Rx6_Cmd`, 0x`$Rx6_Amr`, 0x`$Rx6_Acr` },
	{ 7, 0x`$Rx7_Cmd`, 0x`$Rx7_Amr`, 0x`$Rx7_Acr` },
	{ 8, 0x`$Rx8_Cmd`, 0x`$Rx8_Amr`, 0x`$Rx8_Acr` },
	{ 9, 0x`$Rx9_Cmd`, 0x`$Rx9_Amr`, 0x`$Rx9_Acr` },
	{ 10, 0x`$Rx10_Cmd`, 0x`$Rx10_Amr`, 0x`$Rx10_Acr` },
	{ 11, 0x`$Rx11_Cmd`, 0x`$Rx11_Amr`, 0x`$Rx11_Acr` },
	{ 12, 0x`$Rx12_Cmd`, 0x`$Rx12_Amr`, 0x`$Rx12_Acr` },
	{ 13, 0x`$Rx13_Cmd`, 0x`$Rx13_Amr`, 0x`$Rx13_Acr` },
	{ 14, 0x`$Rx14_Cmd`, 0x`$Rx14_Amr`, 0x`$Rx14_Acr` },
	{ 15, 0x`$Rx15_Cmd`, 0x`$Rx15_Amr`, 0x`$Rx15_Acr` }
};

const CANTXcfg `$INSTANCE_NAME`_TXConfigStruct[] = 
{
	{ 0, 0x`$Tx0_Cmd`, 0x`$Tx0_Id` },
	{ 1, 0x`$Tx1_Cmd`, 0x`$Tx1_Id` },
	{ 2, 0x`$Tx2_Cmd`, 0x`$Tx2_Id` },
	{ 3, 0x`$Tx3_Cmd`, 0x`$Tx3_Id` },
	{ 4, 0x`$Tx4_Cmd`, 0x`$Tx4_Id` },
	{ 5, 0x`$Tx5_Cmd`, 0x`$Tx5_Id` },
	{ 6, 0x`$Tx6_Cmd`, 0x`$Tx6_Id` },
	{ 7, 0x`$Tx7_Cmd`, 0x`$Tx7_Id` }
};


/*-----------------------------------------------------------------------------
 * FUNCTION NAME:  uint8 `$INSTANCE_NAME`_Init(void)
 *-----------------------------------------------------------------------------
 * Summary: 
 *  This funciton configurates CAN component. The parameters are passed from 
 *  wizard output. This function should call once in code. Reconfiguration on 
 *  the fly only possible through directly registers write.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication whether the configuration has been accepted or rejected.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_Init(void)
{
	uint8 i;
	uint8 result = FAIL;
	uint8 localresult = CYRET_SUCCESS;
		
	/* Disable power to CAN */
	`$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;	
		
	/* For all we know the interrupt is active. */
    `$INSTANCE_NAME`_isr_Disable();

    /* Set the ISR to point to the CAN_SUT_isr Interrupt. */
    `$INSTANCE_NAME`_isr_SetVector(`$INSTANCE_NAME`_ISR);

    /* Set the priority. */
    `$INSTANCE_NAME`_isr_SetPriority(`$INSTANCE_NAME`_isr_INTC_PRIOR_NUMBER);	
		
	/* Enable power to CAN */
	`$INSTANCE_NAME`_PWRMGR |= `$INSTANCE_NAME`_ACT_PWR_EN;
	
	if (`$INSTANCE_NAME`_SetPreScaler(`$INSTANCE_NAME`_BITRATE) == CYRET_SUCCESS)
	{
		if (`$INSTANCE_NAME`_SetArbiter(`$INSTANCE_NAME`_ARBITER) == CYRET_SUCCESS)
		{		
			if (`$INSTANCE_NAME`_SetTsegSample(`$INSTANCE_NAME`_CFG_TSEG1, `$INSTANCE_NAME`_CFG_TSEG2, `$INSTANCE_NAME`_CFG_SJW, `$INSTANCE_NAME`_SAMPLING_MODE) == CYRET_SUCCESS)
			{
				if (`$INSTANCE_NAME`_SetRestartType(`$INSTANCE_NAME`_RESET_TYPE) == CYRET_SUCCESS)
				{
					if (`$INSTANCE_NAME`_SetEdgeMode(`$INSTANCE_NAME`_SYNC_EDGE) == CYRET_SUCCESS)
					{
						/* Initialize TX maiboxes */
						for(i = 0; i < `$INSTANCE_NAME`_NUMBER_OF_TX_MAILBOXES; i++)
						{
							if (`$INSTANCE_NAME`_TxBufConfig( &`$INSTANCE_NAME`_TXConfigStruct[i]) != CYRET_SUCCESS)
							{
								localresult = FAIL;
								break;
							}		
						}
						
						if (localresult == CYRET_SUCCESS)
						{ 
							/* Initialize RX mailboxes */
							for(i = 0; i < `$INSTANCE_NAME`_NUMBER_OF_RX_MAILBOXES; i++)
							{
								if (`$INSTANCE_NAME`_RxBufConfig( &`$INSTANCE_NAME`_RXConfigStruct[i]) != CYRET_SUCCESS)
								{
									localresult = FAIL;
									break;
								}				
							}
							
							if (localresult == CYRET_SUCCESS)
							{					
								/* Write IRQ Mask */
								if (`$INSTANCE_NAME`_WriteIrqMask(`$INSTANCE_NAME`_INIT_INTERRUPT_MASK) == CYRET_SUCCESS)
								{		
									/* Active mode always */
									if (`$INSTANCE_NAME`_SetOpMode(`$INSTANCE_NAME`_ACTIVE_MODE) == CYRET_SUCCESS)
									{
										result = CYRET_SUCCESS;
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	/* Enable isr */
    `$INSTANCE_NAME`_isr_Enable();		
	
  	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_Start(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets CAN Component into Run mode. Starts the Rate Counter if 
 *  polling mailboxes available.
 *
 * Parameters: 
 *   None
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_Start(void)
{
	uint8 result = FAIL;
		
	`$INSTANCE_NAME`_CMD.byte[0] |= `$INSTANCE_NAME`_MODE_MASK;
	
	/* Verify that bit is set */
	if (`$INSTANCE_NAME`_CMD.byte[0] & `$INSTANCE_NAME`_MODE_MASK)
	{
		result = CYRET_SUCCESS;
	}
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_Stop(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets CAN Component into Stop mode.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_Stop(void)
{
	uint8 result = FAIL;
	
	`$INSTANCE_NAME`_CMD.byte[0] &= ~`$INSTANCE_NAME`_MODE_MASK;
	
	/* Verify that bit is cleared */
	if (!(`$INSTANCE_NAME`_CMD.byte[0] & `$INSTANCE_NAME`_MODE_MASK))
	{
		result = CYRET_SUCCESS;
	}
			    
	/* Disable interrupt */
    `$INSTANCE_NAME`_isr_Disable();		
	
	/* Disable power to CAN */
	`$INSTANCE_NAME`_PWRMGR &= ~`$INSTANCE_NAME`_ACT_PWR_EN;
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`GlobalIntEnable(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function enables Global Interrupts from CAN Core.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GlobalIntEnable(void)
{
	uint8 result = FAIL;
	
	`$INSTANCE_NAME`_INT_EN.byte[0] |= `$INSTANCE_NAME`_GLOBAL_INT_MASK;
	
	/* Verify that bit is set */
	if (`$INSTANCE_NAME`_INT_EN.byte[0] & `$INSTANCE_NAME`_GLOBAL_INT_MASK)
	{
		result = CYRET_SUCCESS;
	}

	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`GlobalIntDisable(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function disables Global Interrupts from CAN Core.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GlobalIntDisable(void)
{
	uint8 result = FAIL;

	`$INSTANCE_NAME`_INT_EN.byte[0] &= ~`$INSTANCE_NAME`_GLOBAL_INT_MASK;
	
	/* Verify that bit is cleared */
	if (!(`$INSTANCE_NAME`_INT_EN.byte[0] & `$INSTANCE_NAME`_GLOBAL_INT_MASK))
	{
		result = CYRET_SUCCESS;
	}
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SetPreScaler(uint8 bitrate)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets PreScaler for generation the time quantum which defines 
 *  the time quanta. Value between 0x0 and 0x7FFF are valid.
 *
 * Parameters: 
 *  (uint8) bitrate: PreScaler value.
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SetPreScaler(uint16 bitrate)
{
	uint8 result = OUT_OF_RANGE;
		
	if (bitrate <= `$INSTANCE_NAME`_BITRATE_MASK)
	{	
		result = FAIL;
				
		/* Set prescaler */
		CY_SET_REG16((reg16 *) &`$INSTANCE_NAME`_CFG.byte[2], bitrate);
				
		/* Verify that prescaler is set */
		if(CY_GET_REG16((reg16 *) &`$INSTANCE_NAME`_CFG.byte[2]) == bitrate)
		{
			result = CYRET_SUCCESS;
		}
	}
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SetArbiter(uint8 arbiter)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets arbitaration type for transmit mailboxes. Types of arbiters 
 *  are Round Robin and Fixed priority. Value 0 and 1 are valid.
 *
 * Parameters: 
 *  (uint8) arbiter: Arbitaration type for transmit mailboxes
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SetArbiter(uint8 arbiter)
{
	uint8 result = FAIL;

	if (arbiter)
	{		
		`$INSTANCE_NAME`_CFG.byte[1] |= `$INSTANCE_NAME`_ARBITRATION_MASK;
		
		/* Verify that bit is set */
		if (`$INSTANCE_NAME`_CFG.byte[1] & `$INSTANCE_NAME`_ARBITRATION_MASK)
		{
			result = CYRET_SUCCESS;
		}
	}
	else
	{
		`$INSTANCE_NAME`_CFG.byte[1] &= ~`$INSTANCE_NAME`_ARBITRATION_MASK;
		
		/* Verify that bit is cleared */
		if (!(`$INSTANCE_NAME`_CFG.byte[1] & `$INSTANCE_NAME`_ARBITRATION_MASK))
		{
			result = CYRET_SUCCESS;
		}
	}
				
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SetTsegSample(uint8 cfg_tseg1, uint8 cfg_tseg2, uint8 sjw, uint8 sm)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function configures: Time segment 1, Time segment 2, Sampling Mode 
 *  and Synchronization Jump Width. 
 *
 * Parameters: 
 *  (uint8) cfg_tseg1: Time segment 1, value between 0x2 and 0xF are valid;
 *  (uint8) cfg_tseg2: Time segment 2, value between 0x1 and 0x7 are valid;
 *  (uint8) sjw: Synchronization Jump Width, value between 0x0 and 0x3 are valid;
 *  (uint8) sm: Sampling Mode, one or three sampling points are used ;
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SetTsegSample(uint8 cfg_tseg1, uint8 cfg_tseg2, uint8 sjw, uint8 sm)
{
	uint8 result = OUT_OF_RANGE;
	uint8 cfg_temp;
	
	if ((cfg_tseg1 >= `$INSTANCE_NAME`_CFG_TSEG1_LOWER_LIMIT) && (cfg_tseg1 <= `$INSTANCE_NAME`_CFG_TSEG1_UPPER_LIMIT))
	{
		if (((cfg_tseg2 >= `$INSTANCE_NAME`_CFG_TSEG2_LOWER_LIMIT) && (cfg_tseg2 <= `$INSTANCE_NAME`_CFG_TSEG2_UPPER_LIMIT)) ||
		   ((sm == `$INSTANCE_NAME`_ONE_SAMPLE_POINT) && (cfg_tseg2 == `$INSTANCE_NAME`_CFG_TSEG2_EXCEPTION)))
		{
			if((sjw <= `$INSTANCE_NAME`_CFG_SJW_LOWER_LIMIT) && (sjw <= cfg_tseg1) && (sjw <= cfg_tseg2))
			{
				result = FAIL;
								
				cfg_temp = `$INSTANCE_NAME`_CFG.byte[1];
 				cfg_temp &= ~`$INSTANCE_NAME`_CFG_TSEG1_MASK;
				cfg_temp |= cfg_tseg1;
					
				/* write register byte 1 */
				`$INSTANCE_NAME`_CFG.byte[1] = cfg_temp;
				
				/* Verify 2nd byte of `$INSTANCE_NAME`_CFG register */
				if (`$INSTANCE_NAME`_CFG.byte[1] == cfg_temp)
				{	
					/* read and clean bits */
					cfg_temp = `$INSTANCE_NAME`_CFG.byte[0];
 					cfg_temp &= ~(`$INSTANCE_NAME`_CFG_TSEG2_MASK | `$INSTANCE_NAME`_CFG_SJW_MASK | `$INSTANCE_NAME`_SAMPLE_MODE_MASK);
					
					cfg_temp = 0;
					/* Set appropriate bits */
					if (sm)
					{
						cfg_temp |= `$INSTANCE_NAME`_SAMPLE_MODE_MASK;
					}
					cfg_temp |= ((cfg_tseg2 << `$INSTANCE_NAME`_CFG_TSEG2_SHIFT) | (sjw << `$INSTANCE_NAME`_CFG_SJW_SHIFT));
						
					/* write register byte 0 */
					`$INSTANCE_NAME`_CFG.byte[0] = cfg_temp;				
					
					/* Verify 1st byte of `$INSTANCE_NAME`_CFG register */
					if (`$INSTANCE_NAME`_CFG.byte[0] == cfg_temp)
					{
						result = CYRET_SUCCESS;
					}					
				}
					
			}
			
		}	
	}
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SetRestartType(uint8 reset)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets Reset type. Types of Reset are Automatic and Manual.
 *  Manual Reset is recommended setting. Value 0 and 1 are valid.
 *
 * Parameters: 
 *  (uint8) reset: Reset Type.
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SetRestartType(uint8 reset)
{	
	uint8 result = FAIL;
	
	if (reset)
	{
		`$INSTANCE_NAME`_CFG.byte[0] |= `$INSTANCE_NAME`_RESET_MASK;
		
		/* Verify that bit is set */
		if (`$INSTANCE_NAME`_CFG.byte[0] & `$INSTANCE_NAME`_RESET_MASK)	
		{
			result = CYRET_SUCCESS;
		}
	}
	else
	{
		`$INSTANCE_NAME`_CFG.byte[0] &= ~`$INSTANCE_NAME`_RESET_MASK;
		
		/* Verify that bit is cleared */
		if (!(`$INSTANCE_NAME`_CFG.byte[0] & `$INSTANCE_NAME`_RESET_MASK))
		{
			result = CYRET_SUCCESS;
		}
	}
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SetEdgeMode(uint8 edge)
 *-----------------------------------------------------------------------------
 * Summary: 
 *  This function sets Edge Mode. Modes are 'R' to 'D'(Reccessive to Dominant) 
 *  and Both edges are used. Value 0 and 1 are valid.
 *  
 * Parameters: 
 *  (uint8) edge: Edge Mode.
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SetEdgeMode(uint8 edge)
{
	uint8 result = FAIL;
	
	if (edge)
	{
		`$INSTANCE_NAME`_CFG.byte[0] |= `$INSTANCE_NAME`_EDGE_MODE_MASK;
		
		/* Verify that bit is set */
		if (`$INSTANCE_NAME`_CFG.byte[0] & `$INSTANCE_NAME`_EDGE_MODE_MASK)	
		{
			result = CYRET_SUCCESS;
		}
	}
	else
	{
		`$INSTANCE_NAME`_CFG.byte[0] &= ~`$INSTANCE_NAME`_EDGE_MODE_MASK;
		
		/* Verify that bit is cleared */
		if (!(`$INSTANCE_NAME`_CFG.byte[0] & `$INSTANCE_NAME`_EDGE_MODE_MASK))
		{
			result = CYRET_SUCCESS;
		}
	}
	
	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_SetOpMode(uint8 opmode)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets Operation Mode. Operations Modes are Active of Listen 
 *  Only. Value 0 and 1 are valid.
 *
 * Parameters: 
 *  (uint8) opmode: Operation Mode value.
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_SetOpMode(uint8 opmode)
{
	uint8 result = FAIL;
	
	if(opmode)
	{
		`$INSTANCE_NAME`_CMD.byte[0] |= `$INSTANCE_NAME`_OPMODE_MASK;
		
		/* Verify that bit is set */
		if (`$INSTANCE_NAME`_CMD.byte[0] & `$INSTANCE_NAME`_OPMODE_MASK)	
		{
			result = CYRET_SUCCESS;
		}
	}
	else
	{
		`$INSTANCE_NAME`_CMD.byte[0] &= ~`$INSTANCE_NAME`_OPMODE_MASK;
		
		/* Verify that bit is cleared */
		if (!(`$INSTANCE_NAME`_CMD.byte[0] & `$INSTANCE_NAME`_OPMODE_MASK))
		{
			result = CYRET_SUCCESS;
		}
	}
	
	return result;
	
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_RXRegisterInit(uint32 *reg, uint32 configuration)
 *-----------------------------------------------------------------------------
 * Summary: 
 *  This function writes only receive CAN registers. 
 *
 * Parameters: 
 *  (uint32 *) reg: Pointer to CAN receive register.
 *  (uint32) configuration:  Value that will be written in register. 
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_RXRegisterInit(uint32 *reg, uint32 configuration)
{	
	uint8 result = OUT_OF_RANGE;
	uint32 address = (uint32) reg;
	
	if ((address >= `$INSTANCE_NAME`_RX_FIRST_REG) && (address <= `$INSTANCE_NAME`_RX_LAST_REG))
	{
		result = FAIL; 
		
		if (address % `$INSTANCE_NAME`_RX_CMD_REG_WIDTH)
		{
			/* All registers except RX CMD*/
			
			/* Write definined CAN receive register */
			CY_SET_REG32((reg32 *) reg, configuration);
		
			/* Verify register */
			if( CY_GET_REG32((reg32 *) reg) == configuration)
			{
				result = CYRET_SUCCESS;
			}
			
		}
		else
		{
			/* RX CMD registers */
			configuration |= `$INSTANCE_NAME`_RX_WPN_SET;
			
			/* Write definined CAN receive register */
			CY_SET_REG32((reg32 *) reg, configuration);
		
			/* Verify register */
			if( (CY_GET_REG32((reg32 *) reg) & `$INSTANCE_NAME`_RX_READ_BACK_MASK) ==
				(configuration & `$INSTANCE_NAME`_RX_READ_BACK_MASK) )
			{
				result = CYRET_SUCCESS;
			}			
		}	
	}
	
	return result;
}

 /*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_WriteIrqMask(uint16 request)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function sets to enable/disable particular interrupt sources. Interrupt 
 *  Mask directly write to CAN Interrupt Enable register.
 *
 * Parameters: 
 *  (uint8) request: Interrupt enable/disable request. 1 bit per interrupt source.
 *
 * Return:
 *  (uint8) Indication whether register is written and verified.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *   None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_WriteIrqMask(uint16 request)
{
  	uint8 result = FAIL;
	
	/* Write byte 0 and 1 of `$INSTANCE_NAME`_INT_EN register */
	CY_SET_REG16((reg16 *)&`$INSTANCE_NAME`_INT_EN, request);
	
	/* Verify `$INSTANCE_NAME`_INT_EN register */
	if (CY_GET_REG16((reg16 *)&`$INSTANCE_NAME`_INT_EN) == request)	
	{
		result = CYRET_SUCCESS;
	}

	return result;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetTXErrorflag(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function returns the bit that indicates if the number of TX errors 
 *  exceeds 0x60.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication whether the number of TX errors exceeds 0x60.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GetTXErrorFlag(void)
{
  	/* Get the state of the transmit error flag */
	return (`$INSTANCE_NAME`_ERR_SR.byte[2] & `$INSTANCE_NAME`_TX_ERROR_FLAG_MASK) ? 1:0;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_GetRXErrorflag(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function returns the bit that indicates if the number of RX errors 
 *  exceeds 0x60. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Indication whether the number of TX errors exceeds 0x60.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GetRXErrorFlag(void)
{
  	/* Get the state of the receive error flag */
	return (`$INSTANCE_NAME`_ERR_SR.byte[2] & `$INSTANCE_NAME`_RX_ERROR_FLAG_MASK) ? 1:0;
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_TXErrorCount(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function returns the number of Transamit Errors.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Number of Transamit Errors.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GetTXErrorCount(void)
{	
	/* Get the state of the transmit error count */
	return (`$INSTANCE_NAME`_ERR_SR.byte[0]);  /* bits 7-0 */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_RXErrorCount(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function returns the number of Receive Errors. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Number of Receive Errors.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GetRXErrorCount(void)
{
	/* Get the state of the receive error count */
	return (`$INSTANCE_NAME`_ERR_SR.byte[1]);  /* bits 15-8 */
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_RXErrorstat(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function returns error status of CAN Component.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  (uint8) Error status.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *   None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_GetRXErrorStat(void)
{
	/* Get the error state of the receiver */
	return (`$INSTANCE_NAME`_ERR_SR.byte[2] & `$INSTANCE_NAME`_ERROR_STATE_MASK);
}

#if (`$INSTANCE_NAME`_ARB_LOST)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_ArbLost(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Arbitration Lost Interrupt. Clears 
 *  Arbitration Lost interrupt flag. Only generated if Arbitration Lost 
 *  Interrupt enable in wizard. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_ArbLostIsr(void)
{
	/* `#START ARBITRATION_LOST_ISR` */
		
	/* `#END` */
  
  	/* Clear Arbitration Lost flag */
	`$INSTANCE_NAME`_INT_SR.byte[0] |= `$INSTANCE_NAME`_ARBITRATION_LOST_MASK;
}
#endif 

#if (`$INSTANCE_NAME`_OVERLOAD)
 /*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_OvrLdErrror(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Overload Error Interrupt. Clears Overload 
 *  Error interrupt flag. Only generated if Overload Error Interrupt enable 
 *  in wizard. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_OvrLdErrorIsr(void)
{
	/* `#START OVER_LOAD_ERROR_ISR` */
		
	/* `#END` */
  
  	/* Clear Overload Error flag */
	`$INSTANCE_NAME`_INT_SR.byte[0] |= `$INSTANCE_NAME`_OVERLOAD_ERROR_MASK;
}
#endif

#if (`$INSTANCE_NAME`_BIT_ERR)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_BitError(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Bit Error Interrupt. Clears Bit Error 
 *  interrupt flag. Only generated if Bit Error Interrupt enable in wizard. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_BitErrorIsr(void)
{
	/* `#START BIT_ERROR_ISR` */
		
	/* `#END` */
 
   	/* Clear Bit Error flag */
	`$INSTANCE_NAME`_INT_SR.byte[0] |= `$INSTANCE_NAME`_BIT_ERROR_MASK;
}
#endif

#if (`$INSTANCE_NAME`_STUFF_ERR)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_BitStuffError(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Bit Stuff Error Interrupt. Clears Bit Stuff 
 *  Error interrupt flag. Only generated if Bit Stuff Error Interrupt enable
 *  in wizard. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_BitStuffErrorIsr(void)
{
	/* `#START BIT_STUFF_ERROR_ISR` */
		
	/* `#END` */
 
   	/* Clear Stuff Error flag */
	`$INSTANCE_NAME`_INT_SR.byte[0] |= `$INSTANCE_NAME`_STUFF_ERROR_MASK;
}
#endif

#if (`$INSTANCE_NAME`_ACK_ERR)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_AckErrorIsr(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Acknowledge Error Interrupt. Clears 
 *  Acknowledge Error interrupt flag. Only generated if Acknowledge Error 
 *  Interrupt enable in wizard.
 *
 * Parameters: 
 *  None 
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_AckErrorIsr(void)
{
	/* `#START ACKNOWLEDGE_ERROR_ISR` */
		
	/* `#END` */
 
   	/* Clear Acknoledge Error flag */
	`$INSTANCE_NAME`_INT_SR.byte[0] |= `$INSTANCE_NAME`_ACK_ERROR_MASK;
}
#endif

#if (`$INSTANCE_NAME`_FORM_ERR)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_MsgError(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Form Error Interrupt. Clears Form Error 
 *  interrupt flag. Only generated if Form Error Interrupt enable in wizard.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_MsgErrorIsr(void)
{
	/* `#START MESSAGE_ERROR_ISR` */
		
	/* `#END` */
 
   	/* Clear Form Error flag */
	`$INSTANCE_NAME`_INT_SR.byte[0] |= `$INSTANCE_NAME`_FORM_ERROR_MASK;
}
#endif

#if (`$INSTANCE_NAME`_CRC_ERR)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_CrcError(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to CRC Error Interrupt. Clears CRC Error 
 *  interrupt flag. Only generated if CRC Error Interrupt enable in wizard.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_CrcErrorIsr(void)
{
	/* `#START CRC_ERROR_ISR` */
		
	/* `#END` */
 
   	/* Clear CRC Error flag */
	`$INSTANCE_NAME`_INT_SR.byte[1] |= `$INSTANCE_NAME`_CRC_ERROR_MASK;
}
#endif


#if (`$INSTANCE_NAME`_BUS_OFF)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_BusOff(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Buss Off Interrupt. Places CAN Component 
 *  to Stop mode. Only generated if Buss Off Interrupt enable in wizard. 
 *  Recommended setting to enable this interrupt.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  Stop operations of CAN Component 
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_BusOffIsr(void)
{
	/* `#START BUSS_OFF_ISR` */
		
	/* `#END` */
 
	/* Clear Bus Off flag */
	`$INSTANCE_NAME`_INT_SR.byte[1] |= `$INSTANCE_NAME`_BUS_OFF_MASK;
	//`$INSTANCE_NAME`_Stop();
}
#endif

#if (`$INSTANCE_NAME`_RX_MSG_LOST)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_MsgLost(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Message Lost Interrupt. Clears Message Lost 
 *  interrupt flag. Only generated if Message Lost Interrupt enable in wizard. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_MsgLostIsr(void)
{
	/* `#START MESSAGE_LOST_ISR` */
		
	/* `#END` */
 
   	/* Clear Receive Message Lost flag */
	`$INSTANCE_NAME`_INT_SR.byte[1] |= `$INSTANCE_NAME`_RX_MSG_LOST_MASK;
}
#endif

#if (`$INSTANCE_NAME`_TX_MSG)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_MsgTXIsr(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Transmit Message Interrupt. Clears Transmit 
 *  Message interrupt flag. Only generated if Transmit Message Interrupt enable 
 *  in wizard. 
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_MsgTXIsr(void)
{
	/* `#START MESSAGE_TRANSMITTED_ISR` */
		
	/* `#END` */
 
   	/* Clear Transtmit Message flag */
	`$INSTANCE_NAME`_INT_SR.byte[1] |= `$INSTANCE_NAME`_TX_MSG_MASK;
}
#endif

#if (`$INSTANCE_NAME`_RX_MSG)
/*-----------------------------------------------------------------------------
 * FUNCTION NAME: void `$INSTANCE_NAME`_MsgRXIsr(void)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function is entry point to Receive Message Interrupt. Clears Receive 
 *  Message interrupt flag and call appropriate handlers for Basic and Full 
 *  interrupt based mailboxes. Only generated if Receive Message Interrupt 
 *  enable in wizard. Recommended setting to enable this interrupt.
 *
 * Parameters: 
 *  None
 *
 * Return:
 *  None
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *  None
 *
 *---------------------------------------------------------------------------*/
void `$INSTANCE_NAME`_MsgRXIsr(void)
{
	uint8 i;
  	uint16 shift = 1;
		
	/* `#START MESSAGE_RECEIVE_ISR` */
		
	/* `#END` */
	
  	for (i = 0; i < `$INSTANCE_NAME`_NUMBER_OF_RX_MAILBOXES; i++)
  	{
		if (CY_GET_REG16((reg16 *) &`$INSTANCE_NAME`_BUF_SR.byte[0]) & shift)
		{
			if (`$INSTANCE_NAME`_RX[i].rxcmd.byte[0] & `$INSTANCE_NAME`_RX_INT_ENABLE_MASK)
			{
				if(`$INSTANCE_NAME`_RX_MAILBOX_TYPE & shift)
				{
					/* RX Full mailboxes handler */
					switch(i)
					{
`$APIRateGenerationParamIRQ`
						default:
							break;
					}
					
				}
				else
				{
					/* RX Basic mailbox handler */
					`$INSTANCE_NAME`_ReceiveMsg(i);
				}
			}
		}		
		shift <<= 1;
 	}
	
	/* Clear Receive Message flag */
	`$INSTANCE_NAME`_INT_SR.byte[1] |= `$INSTANCE_NAME`_RX_MSG_MASK;
}
#endif

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_RxBufConfig(CANRXcfg *rxconfig)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function configures all recieve registers for particular mailbox.
 *  
 * Parameters: 
 *  (CANRXcfg *) rxconfig: Pointer to structure that contain all required values to 
 *  configure all recieve registers for particular mailbox.
 *
 * Return:
 *  (uint8) Indication if particular configuration has been accepted 
 *  or rejected.
 *
 * Theory:
 *  See summary
 *
 * Side Effects:
 *   None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_RxBufConfig(CANRXcfg *rxconfig)
{
	uint8 result = FAIL;
		
	/* Write RX CMD Register */
	CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxcmd, (rxconfig->rxcmd | `$INSTANCE_NAME`_RX_WPN_SET));
	if ( (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxcmd) & `$INSTANCE_NAME`_RX_READ_BACK_MASK) == 
		(rxconfig->rxcmd & `$INSTANCE_NAME`_RX_WPN_CLEAR) )
	{
		/* Write RX AMR Register */
		CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxamr, rxconfig->rxamr);
		if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxamr) == rxconfig->rxamr)
		{
			/* Write RX ACR Register */
			CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxacr, rxconfig->rxacr);
			if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxacr) == rxconfig->rxacr)
			{
				/* Write RX AMRD Register */
				CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxamrd, 0xFFFFFFFF);
				if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxamrd) == 0xFFFFFFFF)
				{
					/* Write RX ACRD Register */
					CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxacrd, 0x00000000);
					if (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_RX[rxconfig->rxmailbox].rxacrd) == 0x00000000)
					{
							result = CYRET_SUCCESS;
					}
				}
			}
		}
	}
	
	return result;	
}

/*-----------------------------------------------------------------------------
 * FUNCTION NAME: uint8 `$INSTANCE_NAME`_TxBufConfig(CANTXcfg *txconfig)
 *-----------------------------------------------------------------------------
 * Summary:
 *  This function configures all transmit registers for particular mailbox. Mailbox
 *  number contians CANTXcfg structure.
 *
 * Parameters: 
 *  (CANTXcfg *) txconfig: Pointer to structure that contain all required values to 
 *  configure all transmit registers for particular mailbox.  
 *
 * Return:
 *  (uint8) Indication if particular configuration has been accepted 
 *  or rejected.
 *
 * Theory:
 *   See summary
 *
 * Side Effects:
 *   None
 *
 *---------------------------------------------------------------------------*/
uint8 `$INSTANCE_NAME`_TxBufConfig(CANTXcfg *txconfig)
{
	uint8 result = FAIL;
	
	/* Write TX CMD Register */
	CY_SET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[txconfig->txmailbox].txcmd, (txconfig->txcmd | `$INSTANCE_NAME`_TX_WPN_SET) );
	if ( (CY_GET_REG32((reg32 *) &`$INSTANCE_NAME`_TX[txconfig->txmailbox].txcmd) & `$INSTANCE_NAME`_TX_READ_BACK_MASK ) == \
		(txconfig->txcmd & `$INSTANCE_NAME`_TX_WPN_CLEAR) )
	{
		/* Write TX ID Register */
		CY_SET_REG32( (reg32 *) &`$INSTANCE_NAME`_TX[txconfig->txmailbox].txid, txconfig->txid);	
		if (CY_GET_REG32( (reg32 *) &`$INSTANCE_NAME`_TX[txconfig->txmailbox].txid) == txconfig->txid)
		{
			result = CYRET_SUCCESS;
		}
	}
	
	return result;
}

///* [] END OF FILE */
//