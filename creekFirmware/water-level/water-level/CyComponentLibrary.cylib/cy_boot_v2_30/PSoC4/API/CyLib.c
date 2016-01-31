/*******************************************************************************
* File Name: CyLib.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*
*  Note:
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
*******************************************************************************
* Copyright 2010-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

#include <CyLib.h>
#include <CyLib.h>

/* Convertion table between CySysClkWriteImoFreq() parameter and register's value */
const uint8 cyImoFreqMhz2Reg[49u] = {
     3,  3,  3,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 
	14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 
	27, 28, 29, 30, 31, 32, 33, 34, 35, 
	37, 38, 39, 40, 41, 42, 43, 
	46, 47, 48, 49, 50, 51, 52, 53};

/* Convertion table between register's value and frequency*/
const uint8 cyImoFreqReg2Mhz[54u] = {
     3,  3,  3,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 
	13, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
	25, 25, 26, 27, 28, 29, 30, 31, 32, 
    33, 33, 34, 35, 36, 37, 38, 39, 40, 
    41, 41, 41, 42, 43, 44, 45, 46, 47, 48};

/* CYLIB CLOCK funcs begin*/

/*******************************************************************************
* Function Name: CySysClkImoStart 
********************************************************************************
* Summary:
*   Enables the IMO.
*
* Parameters:
*   None
*
* Return:
*   None
*
********************************************************************************/
void CySysClkImoStart()
{
	/*Sets the IMO clock enable bit*/
	CY_SET_REG32(CY_CLK_IMO_CONFIG_PTR, CY_GET_REG32(CY_CLK_IMO_CONFIG_PTR) | CLK_IMO_CONFIG_ENABLE);
}


/*******************************************************************************
* Function Name: CySysClkImoStop 
********************************************************************************
* Summary:
*   Disables the IMO.
*
* Parameters:
*   None
*
* Return:
*   None
*
********************************************************************************/
void CySysClkImoStop()
{
	CY_SET_REG32(CY_CLK_IMO_CONFIG_PTR, CY_GET_REG32(CY_CLK_IMO_CONFIG_PTR) & ~CLK_IMO_CONFIG_ENABLE);
}


/*******************************************************************************
* Function Name: CySysClkIloStart 
********************************************************************************
* Summary:
*   Enables the ILO.
*
* Parameters:
*   None
*
* Return:
*   None
*
********************************************************************************/
void CySysClkIloStart()
{
	/*CLK_ILO_CONFIG*/
	CY_SET_REG32(CY_CLK_ILO_CONFIG_PTR, CY_GET_REG32(CY_CLK_ILO_CONFIG_PTR) | CLK_ILO_CONFIG_ENABLE);
}

/*******************************************************************************
* Function Name: CySysClkIloStop 
********************************************************************************
* Summary:
*   Disables the ILO.
*
* Parameters:
*   None
*
* Return:
*   None
*
********************************************************************************/
void CySysClkIloStop()
{
	CY_SET_REG32(CY_CLK_ILO_CONFIG_PTR, CY_GET_REG32(CY_CLK_ILO_CONFIG_PTR) & ~CLK_ILO_CONFIG_ENABLE);
}


/*******************************************************************************
* Function Name: CySysClkWriteHfclkDirect 
********************************************************************************
* Summary:
*   Selects the direct source for the HFCLK.
*
* Parameters:
*   clkSelect: One of the available HFCLK direct sources
*    Value		Define					Source
*    0			CY_SYS_CLK_HFCLK_IMO	IMO
*    1			CY_SYS_CLK_HFCLK_EXTCLK	External clock pin
*
* Return:
*   None
*
********************************************************************************/
void CySysClkWriteHfclkDirect (uint8 clkSelect)
{
	/* To select the HFCLK 
	   Bit 17:16 (HFCLK_SEL) must be set to 00 and
	   the Bits 2:0	DIRECT_SEL	must select the required source.*/
	CY_SET_REG32(CY_CLK_SELECT_PTR, (CY_GET_REG32(CY_CLK_SELECT_PTR) & ~CLK_HFCLK_SELECT_SET) 
		| ((clkSelect & CLK_HFLK_SELECT_MASK)));
}

/*******************************************************************************
* Function Name: CySysClkWriteSysclkDiv 
********************************************************************************
* Summary:
*   Selects the pre-scaler divide amount for SYSCLK from HFCLK.
*
* Parameters:
*   divider: Power of 2 prescaler selection
*   
*   Value	Define						Divider
*   0		CY_SYS_CLK_SYSCLK_DIV1		1
*   1		CY_SYS_CLK_SYSCLK_DIV2		2
*   2		CY_SYS_CLK_SYSCLK_DIV4		4
*   3		CY_SYS_CLK_SYSCLK_DIV8		8
*   4		CY_SYS_CLK_SYSCLK_DIV16		16
*   5		CY_SYS_CLK_SYSCLK_DIV32		32
*   6		CY_SYS_CLK_SYSCLK_DIV64		64
*   7		CY_SYS_CLK_SYSCLK_DIV128	128
*
* Return:
*   None
*
********************************************************************************/
void CySysClkWriteSysclkDiv (uint8 divider)
{
	CY_SET_REG32(CY_CLK_SELECT_PTR, CY_GET_REG32(CY_CLK_SELECT_PTR) | 
		((divider & CLK_SYSCLK_SELECT_MASK) << CLK_HFLK_FROM_SYSCLK));
}

/*******************************************************************************
* Function Name: CySysClkWriteHalfSysclkDiv 
********************************************************************************
* Summary:
*   Selects between SYSCLK and SYSCLK/2 for HALFSYSCLK frequency.  
*   Must be set to SYSCLK/2 if the frequency of SYSCLK > 24MHz.
*
* Parameters:
*   selection: Selection of divide for HALFSYSCLK
*      Value	Define						Divider			
*      0		CY_SYS_CLK_HALFSYSCLK_DIV1	1
*      1		CY_SYS_CLK_HALFSYSCLK_DIV2	2
*
* Return:
*   None
*
********************************************************************************/
void CySysClkWriteHalfSysclkDiv (uint8 divider)
{
    CY_SET_REG32(CY_CLK_SELECT_PTR, 
		(CY_GET_REG32(CY_CLK_SELECT_PTR) & ~(CLK_HALFSYSCLK_SELECT_MASK << CLK_HALFSYSCLK_BITS_POS))| 
        ((divider & CLK_HALFSYSCLK_SELECT_MASK) << CLK_HALFSYSCLK_BITS_POS));
}

/*******************************************************************************
* Function Name: CySysClkWriteImoFreq
********************************************************************************
* Summary:
*   Sets the frequency of the IMO.
*
* Parameters:
*   freq: Valid range [3-48].  Frequency for operation of the IMO.
*   Note: Invalid frequency will be ignored.
*
* Return:
*   None
*
********************************************************************************/
void CySysClkWriteImoFreq (uint8 freq)
{
    uint8 current_freq;	
	uint32 inr = CY_GET_REG32(CYREG_CLK_IMO_TRIM2) & CLK_IMO_FREQ_BITS_MASK;
	current_freq = cyImoFreqReg2Mhz[inr];
	
	/*Invalid range is ignored.*/
	CYASSERT(freq < 49);
	if(freq > 48)
	{
		return;
	}
	
	if (current_freq >= freq)
	{
		/*Set the frequency register*/
		CY_SET_REG32((CYREG_CLK_IMO_TRIM2), cyImoFreqMhz2Reg[freq & CLK_IMO_FREQ_BITS_MASK]);
		CyDelayCycles(5);
		/*Set the trim register*/
		CY_SET_REG32((CYREG_CLK_IMO_TRIM1), (CY_GET_REG32(CYREG_SFLASH0_IMO_TRIM21)));
		CyDelayUs(5);
	}
	else
	{
		CY_SET_REG32((CYREG_CLK_IMO_TRIM1), (CY_GET_REG32(CYREG_SFLASH0_IMO_TRIM21)));
		CyDelayUs(5);
		CY_SET_REG32((CYREG_CLK_IMO_TRIM2), cyImoFreqMhz2Reg[freq & CLK_IMO_FREQ_BITS_MASK]);
		CyDelayCycles(5);		
	}
}

/* region WDT functions*/
/*******************************************************************************
* Function Name: CySysWdtLock
********************************************************************************
* Summary:
*   Locks out configuration changes to the Watchdog timer register.
*
* Parameters:
*   None
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtLock()
{
	/*CLK_SELECT register's bits 15:14 are WDT_LOCK */
	CY_SET_REG32(CY_CLK_SELECT_PTR, (CY_GET_REG32(CY_CLK_SELECT_PTR) & ~CLK_CONFIG_WDT_BITS_MASK) 
									| CLK_CONFIG_WDT_BITS_MASK);
}

/*******************************************************************************
* Function Name: CySysWdtUnlock
********************************************************************************
* Summary:
*   Unlocks the Watchdog Timer configuration register.
*
* Parameters:
*   None
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtUnlock()
{
	CY_SET_REG32(CY_CLK_SELECT_PTR, (CY_GET_REG32(CY_CLK_SELECT_PTR) & ~CLK_CONFIG_WDT_BITS_MASK) 
									| CLK_CONFIG_WDT_BIT0);
	CY_SET_REG32(CY_CLK_SELECT_PTR, (CY_GET_REG32(CY_CLK_SELECT_PTR) & ~CLK_CONFIG_WDT_BITS_MASK) 
									| CLK_CONFIG_WDT_BIT1);
}

/*******************************************************************************
* Function Name: CySysGetWdtEnabledMode
********************************************************************************
* Summary:
*   Reads the mode of one of the three WDT counters.
*   Note: A private function to cy_boot.
*
* Parameters:
*   counterNum: Valid range [0-2].  Number of the WDT counter.
*
* Return:
*   uint8
*
********************************************************************************/
uint8 CySysGetWdtEnabledMode(uint8 counterNum)
{
	CYASSERT(counterNum < CY_WDT_COUNTERS_MAX);
	return ((CY_GET_REG32(CY_CLK_WDT_CONTROL_PTR) >> ((8 * counterNum) +1)) & 0x01u);
}

/*******************************************************************************
* Function Name: CySysWdtWriteMode
********************************************************************************
* Summary:
*   Writes the mode of one of the three WDT counters.
*   Note: 
*
* Parameters:
*   counterNum: Valid range [0-2].  Number of the WDT counter.
*   mode:       Mode of operation for the counter
*   Value	Define                   Mode
*   0	CY_SYS_WDT_MODE_NONE	     Free running
*   1	CY_SYS_WDT_MODE_INT		     Interrupt generated on match for counter 0 and 1, 
*                                      and on bit toggle for counter 2
*   2	CY_SYS_WDT_MODE_RESET	     Reset on match (valid for counter 0 and 1 only)
*   3	CY_SYS_WDT_MODE_INT_RESET	 Generate an interrupt.  Generate a reset on 
*                                      the 3rd unhandled interrupt.  
*                                      (valid for counter 0 and 1 only)
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtWriteMode(uint8 counterNum, uint8 mode)
{
	CYASSERT(counterNum < CY_WDT_COUNTERS_MAX);
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);

	if (!CySysGetWdtEnabledMode(counterNum))
	{
		/*
		Watchdog Counter Action on Match (WDT_CTR0=WDT_MATCH0):
		0: Do nothing
		1: Assert WDT_INTx
		2: Assert WDT Reset
		3: Assert WDT_INTx, assert WDT Reset after 3rd unhandled interrupt"
		*/
		CY_SET_REG32(CY_CLK_WDT_CONFIG_PTR, 
		    (CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR) & (~(CLK_SYS_WDT_MODE_MASK) << (counterNum * 8)))
			| (mode & CLK_SYS_WDT_MODE_MASK) << (counterNum * 8));
	}
}

/*******************************************************************************
* Function Name: CySysWdtReadMode
********************************************************************************
* Summary:
*   Reads the mode of one of the three WDT counters.
*
* Parameters:
*	counterNum: Valid range [0-2].  Number of the WDT counter.   
*      
*
* Return:
*   Mode of the counter.  Same enumerated values as mode parameter used in CySysWdtSetMode().
*
********************************************************************************/
uint8 CySysWdtReadMode(uint8 counterNum)
{
	uint32 configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
	return ((configreg_value >> (counterNum * 8)) & 0x3u);
}

/*******************************************************************************
* Function Name: CySysWdtWriteClearOnMatch
********************************************************************************
* Summary:
*   Configures the WDT counter clear on a match setting.  If configured to clear 
*   on match the counter will count from 0 to the MatchValue giving it a 
*   period of (MatchValue + 1).
*
* Parameters:
*   counterNum: Valid range [0-1].  Number of the WDT counter.  Match values 
*     are not supported by counter 2.
*  	enable: 0 to disable, 1 to enable
*      
*
* Return:
*   None.
*
********************************************************************************/
void CySysWdtWriteClearOnMatch(uint8 counterNum, uint8 enable)
{
	uint32 configreg_value = 0;
	CYASSERT(counterNum < 2);
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);

	if (!CySysGetWdtEnabledMode(counterNum))
	{
		configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
		configreg_value |= enable << (counterNum * 8 + 2);
		CY_SET_REG32(CY_CLK_WDT_CONFIG_PTR, configreg_value);
	}
}

/*******************************************************************************
* Function Name: CySysWdtReadClearOnMatch
********************************************************************************
* Summary:
*   Reads the clear on match setting for the specified counter.
*
* Parameters:
*   counterNum: Valid range [0-1].  Number of the WDT counter.  Match values are 
*               not supported by counter 2.
*      
*
* Return:
*   Clear on Match status: 1 if enabled, 0 if disabled
*
********************************************************************************/
uint8 CySysWdtReadClearOnMatch(uint8 counterNum)
{
	uint32 configreg_value;
	CYASSERT(counterNum < 2);
	configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
	return ((configreg_value >> ((counterNum * 8) + 2)) & 0x1u);
}

/*******************************************************************************
* Function Name: CySysWdtEnable
********************************************************************************
* Summary:
*   Enables the specified WDT counters.  All counters specified in the mask are 
*   enabled.
*   Note: Enabling or disabling a WDT requires 3 LF Clock cycles to come into 
*         effect.
* 
* 
* Parameters:
*   counterMask: Mask of all counters to enable
*   	Value	Define	                   Counter
*   	1<<0	CY_SYS_WDT_COUNTER0_ENABLE	0
*   	1<<8	CY_SYS_WDT_COUNTER1_ENABLE	1
*   	1<<16	CY_SYS_WDT_COUNTER2_ENABLE	2
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtEnable(uint32 counterMask)
{
	CY_SET_REG32(CY_CLK_WDT_CONTROL_PTR, (CY_GET_REG32(CY_CLK_WDT_CONTROL_PTR) | counterMask));
}

/*******************************************************************************
* Function Name: CySysWdtDisable
********************************************************************************
* Summary:
*   Disables the specified WDT counters.  All counters specified in the mask are 
*   disabled.
*   Note: Enabling or disabling a WDT requires 3 LF Clock cycles to come into 
*         effect.
* 
*
* Parameters:
*   	counterMask: Mask of all counters to disable
*   	Value	Define	                   Counter
*   	1<<0	CY_SYS_WDT_COUNTER0_ENABLE	0
*   	1<<8	CY_SYS_WDT_COUNTER1_ENABLE	1
*   	1<<16	CY_SYS_WDT_COUNTER2_ENABLE	2
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtDisable(uint32 counterMask)
{
	CY_SET_REG32(CY_CLK_WDT_CONTROL_PTR, (CY_GET_REG32(CY_CLK_WDT_CONTROL_PTR) & ~counterMask));
}

/*******************************************************************************
* Function Name: CySysWdtWriteCascade
********************************************************************************
* Summary:
*   Writes the two WDT cascade values based on the combination of mask values specified.
*
* Parameters:
*   cascadeMask: Mask value used to set or clear both of the cascade values.
*   Value	Define	                Cascade
*   0	    CY_SYS_WDT_CASCADE_NONE	Neither
*   1<<3	CY_SYS_WDT_CASCADE_01	Cascade 01
*   1<<11	CY_SYS_WDT_CASCADE_12	Cascade 12
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtWriteCascade(uint32 cascadeMask)
{
	uint32 configreg_value = 0;
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);

	if (!CySysGetWdtEnabledMode(0) | !CySysGetWdtEnabledMode(1) | !CySysGetWdtEnabledMode(2))
	{
		configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
		configreg_value |= cascadeMask;
		CY_SET_REG32(CY_CLK_WDT_CONFIG_PTR, configreg_value);
	}
}

/*******************************************************************************
* Function Name: CySysWdtReadCascade
********************************************************************************
* Summary:
*   Reads the two WDT cascade values returning a mask of the bits set.
*
* Parameters:
*   None
*      
*
* Return:
*   Mask of cascade values set.
*   Value	Define	                Cascade
*   1<<3	CY_SYS_WDT_CASCADE_01	Cascade 01
*   1<<11	CY_SYS_WDT_CASCADE_12	Cascade 12
*   
********************************************************************************/
uint32 CySysWdtReadCascade()
{
	uint32 configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
	return (configreg_value & (CY_SYS_WDT_CASCADE_01 | CY_SYS_WDT_CASCADE_12));
}

/*******************************************************************************
* Function Name: CySysWdtWriteMatch
********************************************************************************
* Summary:
*   Configures the WDT counter match comparison value.
*
* Parameters:
*   counterNum: Valid range [0-1].  Number of the WDT counter.  
*               Match values are not supported by counter 2.
*   match: 16-bit value to be used to match against the counter.   
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtWriteMatch(uint8 counterNum, uint16 match)
{
    uint32 reg_value = 0;
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);
	CYASSERT(counterNum < 2);
	CYASSERT(!CySysGetWdtEnabledMode(counterNum));
	
	/*Set the comparison value only if not running*/
	if (!CySysGetWdtEnabledMode(counterNum))
	{
		reg_value = CY_GET_REG32(CY_CLK_WDT_MATCH_PTR) & ~(CY_LOWER_16BITS_ONLY << (counterNum * 16));
		CY_SET_REG32(CY_CLK_WDT_MATCH_PTR, (reg_value | match << (counterNum * 16)));
	}
}

/*******************************************************************************
* Function Name: CySysWdtWriteBits2
********************************************************************************
* Summary:
*   Configures which bit in the WDT counter 2 to monitor for a toggle.  When that bit 
*   toggles an interrupt is generated if the mode for counter 2 has interrupts enabled.
*
* Parameters:
*   bit: Valid range [0-31].  Counter 2 bit to monitor for a toggle.
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtWriteBits2(uint8 bits)
{
	uint32 configreg_value = 0;
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);

	if (!CySysGetWdtEnabledMode(2))
	{
		configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
		configreg_value &= ~(CY_CLK_WDT_CONFIG_BITS2_MASK << CY_CLK_WDT_CONFIG_BITS2_POS) ;
		configreg_value |= ((bits & CY_CLK_WDT_CONFIG_BITS2_MASK) << CY_CLK_WDT_CONFIG_BITS2_POS);
		CY_SET_REG32(CY_CLK_WDT_CONFIG_PTR, configreg_value);
	}
}

/*******************************************************************************
* Function Name: CySysWdtReadBits2
********************************************************************************
* Summary:
*   Reads which bit in the WDT counter 2 is monitored for a toggle.
*
* Parameters:
*   None
*
*
* Return:
*   Bit that is monitored (range of 0 to 31)
*
********************************************************************************/
uint8 CySysWdtReadBits2()
{
	uint32 configreg_value = CY_GET_REG32(CY_CLK_WDT_CONFIG_PTR);
	return (uint8)(configreg_value >> CY_CLK_WDT_CONFIG_BITS2_POS);
}

/*******************************************************************************
* Function Name: CySysWdtReadMatch
********************************************************************************
* Summary:
*   Reads the WDT counter match comparison value.
*
* Parameters:
*   counterNum: Valid range [0-1].  Number of the WDT counter.  Match values 
*               are not supported by counter 2.
*      
*
* Return:
*   16-bit match value
*
********************************************************************************/
uint16 CySysWdtReadMatch(uint8 counterNum)
{
	uint32 reg_value;
	CYASSERT(counterNum < 2);
	reg_value = CY_GET_REG32(CY_CLK_WDT_MATCH_PTR);
	return (uint16)(reg_value >> (counterNum * 16));
}


/*******************************************************************************
* Function Name: CySysWdtReadCount
********************************************************************************
* Summary:
*   Reads the current WDT counter value.
*
* Parameters:
*   counterNum: Valid range [0-2].  Number of the WDT counter.
*      
*
* Return:
*   Live counter value.  Counter 0 and 1 are 16 bit counters and counter 2 is 
*   a 32 bit counter.
*
********************************************************************************/
uint32 CySysWdtReadCount(uint8 counterNum)
{
	uint32 configreg_value;
	CYASSERT(counterNum < CY_WDT_COUNTERS_MAX);
	configreg_value = CY_GET_REG32(CY_CLK_WDT_CTRHIGH_PTR);
	if(counterNum < 2)
	{
		configreg_value = CY_GET_REG32(CY_CLK_WDT_CTRLOW_PTR) >> (counterNum * 16);
	}
	return configreg_value;
}

/*******************************************************************************
* Function Name: CySysWdtReadIntr
********************************************************************************
* Summary:
*   Reads a mask containing all the WDT interrupts that are currently set.
*
* Parameters:
*   None
*      
*
* Return:
*   Mask of interrupts set.
*   Value	Define	                Counter
*   1<<2	CY_SYS_WDT_COUNTER0_INT	0
*   1<<10	CY_SYS_WDT_COUNTER1_INT	1
*   1<<18	CY_SYS_WDT_COUNTER2_INT	2
********************************************************************************/
uint32 CySysWdtReadIntr()
{
	uint32 configreg_value = CY_GET_REG32(CY_CLK_WDT_CONTROL_PTR);
	return (configreg_value & (CY_SYS_WDT_COUNTER0_INT | CY_SYS_WDT_COUNTER1_INT | CY_SYS_WDT_COUNTER2_INT));
}

/*******************************************************************************
* Function Name: CySysWdtClearIntr
********************************************************************************
* Summary:
*   Clears all WDT counter interrupts set in the mask.
*
* Parameters:
*   counterMask: Mask of all counters to enable
*   Value	Define	                Counter
*   1<<2	CY_SYS_WDT_COUNTER0_INT	0
*   1<<10	CY_SYS_WDT_COUNTER1_INT	1
*   1<<18	CY_SYS_WDT_COUNTER2_INT	2
*      
*
* Return:
*   None
*   
********************************************************************************/
void CySysWdtClearIntr(uint32 counterMask)
{
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);
	/*set the new WDT control register value*/
	CY_SET_REG32(CY_CLK_WDT_CONTROL_PTR, CY_GET_REG32(CY_CLK_WDT_CONTROL_PTR) |
    (counterMask & ((CY_SYS_WDT_COUNTER0_INT | CY_SYS_WDT_COUNTER1_INT | CY_SYS_WDT_COUNTER2_INT))));
}

/*******************************************************************************
* Function Name: CySysWdtResetCount
********************************************************************************
* Summary:
*   Resets all WDT counters set in the mask.
*
* Parameters:
*   counterMask: Mask of all counters to reset.
*   Value	Define	                   Counter
*   1<<3	CY_SYS_WDT_COUNTER0_RESET	0
*   1<<11	CY_SYS_WDT_COUNTER1_RESET	1
*   1<<19	CY_SYS_WDT_COUNTER2_RESET	2
*      
*
* Return:
*   None
*
********************************************************************************/
void CySysWdtResetCount(uint32 counterMask)
{
	CYASSERT((CY_GET_REG32(CY_CLK_SELECT_PTR) & CLK_CONFIG_WDT_BITS_MASK) == 0);
	/*set the new WDT reset value*/
	CY_SET_REG32(CY_CLK_WDT_CONTROL_PTR, CY_GET_REG32(CY_CLK_WDT_CONTROL_PTR) | 
     (counterMask & ((CY_SYS_WDT_COUNTER0_RESET | CY_SYS_WDT_COUNTER1_RESET | CY_SYS_WDT_COUNTER2_RESET))));
}

/*******************************************************************************
* Function Name: CyDisableInts
********************************************************************************
* Summary:
*  Disables the interrupt enable for each interrupt.
*
*
* Parameters: 
*  None.
*
*
* Return: 
*  32 bit mask of previously enabled interrupts.
*
*******************************************************************************/
uint32 CyDisableInts(void) `=ReentrantKeil("CyDisableInts")`
{
    uint32 intState;

    /* Get the curreent interrutp state. */
    intState = CY_GET_REG32(CYINT_CLEAR);

    /* Disable all of the interrupts. */
    CY_SET_REG32(CYINT_CLEAR, CYINT_CLEAR_DISABLE_ALL);

    return intState;
}

/*******************************************************************************
* Function Name: CyEnableInts
********************************************************************************
* Summary:
*  Enables interrupts to a given state.
*
*
* Parameters:
*   mask, 32 bit mask of interrupts to enable.
*
*
* Return: 
*  void.
*
*******************************************************************************/
void CyEnableInts(uint32 mask) `=ReentrantKeil("CyEnableInts")`
{
    /* Set interrupts as enabled. */
    CY_SET_REG32(CYINT_ENABLE, mask);
}


/*******************************************************************************
* Function Name: CyIntSetSysVector
********************************************************************************
* Summary:
*   Sets the interrupt vector of the specified system interrupt number.  
*   These interrupts are for SysTick, PendSV and others.
*
* Parameters:
*   number: Interrupt number, valid range [0-15].
*
*   address: Pointer to an interrupt service routine.
*
* Return:
*   The old ISR vector at this location.
*
*******************************************************************************/
cyisraddress CyIntSetSysVector(uint8 number, cyisraddress address) CYREENTRANT
{
    cyisraddress oldIsr;
    cyisraddress *ramVectorTable = (cyisraddress *) CYINT_VECT_TABLE;
	
	CYASSERT(number < NUM_INTERRUPTS);
    /* Save old Interrupt service routine. */
    oldIsr = ramVectorTable[number];

    /* Set new Interrupt service routine. */
    ramVectorTable[number] = address;

    return oldIsr;
}

/*******************************************************************************
* Function Name: CyIntGetSysVector
********************************************************************************
* Summary:
*   Gets the interrupt vector of the specified system interrupt number.  
*   These interrupts are for SysTick, PendSV and others.  
*
*
* Parameters:
*   number: The interrupt number, valid range [0-15].
*
*
* Return:
*   Address of the ISR in the interrupt vector table.
*
*******************************************************************************/
cyisraddress CyIntGetSysVector(uint8 number) CYREENTRANT
{
    cyisraddress *ramVectorTable = (cyisraddress *) CYINT_VECT_TABLE;
	
	CYASSERT(number < NUM_INTERRUPTS);
    return ramVectorTable[number];
}

/*******************************************************************************
* Function Name: CyIntSetVector
********************************************************************************
* Summary:
*   Sets the interrupt vector of the specified interrupt number.
*
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number
*
*   address: Pointer to an interrupt service routine
*
* Return:
*   Previous interrupt vector value.
*
*******************************************************************************/
cyisraddress CyIntSetVector(uint8 number, cyisraddress address) `=ReentrantKeil("CyIntSetVector")`
{
    cyisraddress oldIsr;
    cyisraddress *ramVectorTable = (cyisraddress *) CYINT_VECT_TABLE;

	CYASSERT(number < NUM_INTERRUPTS);
    /* Save old Interrupt service routine. */
    oldIsr = ramVectorTable[CYINT_IRQ_BASE + number];

    /* Set new Interrupt service routine. */
    ramVectorTable[CYINT_IRQ_BASE + number] = address;

    return oldIsr;
}

/*******************************************************************************
* Function Name: CyIntGetVector
********************************************************************************
* Summary:
*   Gets the interrupt vector of the specified interrupt number.
*
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number
*
*
* Return:
*   Address of the ISR in the interrupt vector table.
*
*******************************************************************************/
cyisraddress CyIntGetVector(uint8 number) `=ReentrantKeil("CyIntGetVector")`
{
    cyisraddress *ramVectorTable = (cyisraddress *) CYINT_VECT_TABLE;

	CYASSERT(number < NUM_INTERRUPTS);
    return ramVectorTable[CYINT_IRQ_BASE + number];
}

/*******************************************************************************
* Function Name: CyIntSetPriority
********************************************************************************
* Summary:
*   Sets the Priority of the Interrupt.
*
*
* Parameters:
*   priority: Priority of the interrupt. 0 - 3, 0 being the highest.
*
*   number: The number of the interrupt, 0 - 15.
*
* Return:
*   void.
*
*
*******************************************************************************/
void CyIntSetPriority(uint8 number, uint8 priority) `=ReentrantKeil("CyIntSetPriority")`
{
    CYASSERT(priority <= MIN_PRIORITY);
    CYASSERT(number < NUM_INTERRUPTS);
    CYINT_PRIORITY[number/4] = priority << CYINT_PRIORITY_SHIFT(number);
}

/*******************************************************************************
* Function Name: CyIntGetPriority
********************************************************************************
* Summary:
*   Gets the Priority of the Interrupt.
*
*
* Parameters:
*   number: The number of the interrupt, 0 - 15.
*
*
* Return:
*   Priority of the interrupt. 0 - 3, 0 being the highest.
*
*
*******************************************************************************/
uint8 CyIntGetPriority(uint8 number) `=ReentrantKeil("CyIntGetPriority")`
{
    uint8 priority;

    CYASSERT(number < NUM_INTERRUPTS);
    priority = CYINT_PRIORITY[number/4] >> CYINT_PRIORITY_SHIFT(number) ;
    return priority;
}

/*******************************************************************************
* Function Name: CyIntEnable
********************************************************************************
* Summary:
*   Enables the specified interrupt number.
*
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyIntEnable(uint8 number) `=ReentrantKeil("CyIntEnable")`
{
    reg32 * enableReg;

    /* Get a pointer to the Interrupt enable register. */
    enableReg = CYINT_ENABLE;

    /* Enable the interrupt. */
    *enableReg = 1 << (CYINT_ENABLE_RANGE_MASK & number);
}

/*******************************************************************************
* Function Name: CyIntGetState
********************************************************************************
* Summary:
*   Gets the enable state of the specified interrupt number.
*
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number.
*
*
* Return:
*   Enable status: 1 if enabled, 0 if disabled
*
*******************************************************************************/
uint8 CyIntGetState(uint8 number) `=ReentrantKeil("CyIntGetState")`
{
    reg32 * stateReg;

    /* Get a pointer to the Interrupt enable register. */
    stateReg = CYINT_ENABLE;

    /* Get the state of the interrupt. */
    return (*stateReg & (1 << (CYINT_ENABLE_RANGE_MASK & number))) ? 1:0;
}

/*******************************************************************************
* Function Name: CyIntDisable
********************************************************************************
* Summary:
*   Disables the specified interrupt number.
*
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number.
*
*
* Return:
*   void.
*
*
*******************************************************************************/
void CyIntDisable(uint8 number) `=ReentrantKeil("CyIntDisable")`
{
    reg32 * clearReg;

    /* Get a pointer to the Interrupt enable register. */
    clearReg = CYINT_CLEAR;

    /* Enable the interrupt. */
    *clearReg = 1 << (0x0F & number);
}

/*******************************************************************************
* Function Name: CyIntSetPending
********************************************************************************
* Summary:
*   Forces the specified interrupt number to be pending.
*
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyIntSetPending(uint8 number) `=ReentrantKeil("CyIntSetPending")`
{
    reg32 * pendReg;

    /* Get a pointer to the Interrupt set pending register. */
    pendReg = CYINT_SET_PEND;

    /* Pending the interrupt. */
    *pendReg = 1 << (0x0F & number);
}

/*******************************************************************************
* Function Name: CyIntClearPending
********************************************************************************
* Summary:
*   Clears any pending interrupt for the specified interrupt number.
*
* Parameters:
*   number: Valid range [0-15].  Interrupt number.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyIntClearPending(uint8 number) `=ReentrantKeil("CyIntClearPending")`
{
    reg32 * pendReg;

    /* Get a pointer to the Interrupt clear pending register. */
    pendReg = CYINT_CLR_PEND;

    /* Clear the pending interrupt. */
    *pendReg = 1 << (0x0F & number);
}

/*******************************************************************************
* Function Name: CyHalt
********************************************************************************
* Summary:
*  Halts the CPU
*
*
* Parameters:
*   reason: Value to be used during debugging.
*
*
* Return: 
*   void.
*
*
*******************************************************************************/
void CyHalt(uint8 reason)
{
    reason = reason;
#if defined (__ARMCC_VERSION)
    __breakpoint(0x0);
#elif defined(__GNUC__)
    __asm("    bkpt    1");
#elif defined(__C51__)
    CYDEV_HALT_CPU;
#endif
}

/*******************************************************************************
* Function Name: CySoftwareReset
********************************************************************************
* Summary:
*  Forces a software reset of the device.
*
*
* Parameters:
*   None.
*
*
* Return: 
*   void.
*
*
*******************************************************************************/
void CySoftwareReset()
{
    /* Perform a reset by setting CM0_AIRCR register's reset bit*/
	CY_SET_REG32(CY_CM0_AIRCR_PTR, CY_SOFTWARE_RESET);
}

/* CYLIB SYSTEM funcs end */
uint32 cydelay_freq_hz = CYDEV_BCLK__SYSCLK__HZ;
uint32 cydelay_freq_khz = (CYDEV_BCLK__SYSCLK__HZ + 999u) / 1000u;
uint8 cydelay_freq_mhz = (uint8)((CYDEV_BCLK__SYSCLK__HZ + 999999u) / 1000000u);
uint32 cydelay_32k_ms = 32768 * ((CYDEV_BCLK__SYSCLK__HZ + 999u) / 1000u);


/*******************************************************************************
* Function Name: CyDelay
********************************************************************************
* Summary:
*   Blocks for milliseconds.
*   
*
* Parameters:
*   milliseconds: number of milliseconds to delay.
*
* Return:
*   void.
*
*******************************************************************************/
void CyDelay(uint32 milliseconds) CYREENTRANT
{
    while (milliseconds > 32768)
    {
        /* This loop prevents overflow.
         * At 100MHz, milliseconds * delay_freq_khz overflows at about 42 seconds
         */
        CyDelayCycles(cydelay_32k_ms);
        milliseconds -= 32768;
    }

    CyDelayCycles(milliseconds * cydelay_freq_khz);
}


/*******************************************************************************
* Function Name: CyDelayUs
********************************************************************************
* Summary:
*   Blocks for microseconds.
*   
*
* Parameters:
*   microseconds: number of microseconds to delay.
*
* Return:
*   void.
*
*******************************************************************************/
#if defined(__ARMCC_VERSION)
void CyDelayUs(uint16 microseconds) CYREENTRANT
{
    CyDelayCycles((uint32)microseconds * cydelay_freq_mhz);
}
#elif defined(__GNUC__)
void CyDelayUs(uint16 microseconds) CYREENTRANT
{
    CyDelayCycles((uint32)microseconds * cydelay_freq_mhz);
}
#endif


/*******************************************************************************
* Function Name: CyDelayFreq
********************************************************************************
* Summary:
*   Sets clock frequency for CyDelay.
*
* Parameters:
*   freq: Frequency of bus clock in Hertz.
*
* Return:
*   void.
*
*******************************************************************************/
void CyDelayFreq(uint32 freq) CYREENTRANT
{
    if (freq != 0u)
        cydelay_freq_hz = freq;
    else
        cydelay_freq_hz = CYDEV_BCLK__SYSCLK__HZ;
    cydelay_freq_mhz = (uint8)((cydelay_freq_hz + 999999u) / 1000000u);
    cydelay_freq_khz = (cydelay_freq_hz + 999u) / 1000u;
    cydelay_32k_ms = 32768 * cydelay_freq_khz;
}

/* End of File */
