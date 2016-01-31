/*******************************************************************************
* File Name: CyLib.c  
* Version `$CY_VERSION_MAJOR`.`$CY_VERSION_MINOR`
*
*  Description:
*
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#pragma O2
#include <CYDEVICE.H>
#include <CYLIB.H>
#include <intrins.h>









#if defined(CYLIB_POWER_MANAGMENT)

#define CYPM_WAKE_IO            1
#define CYPM_WAKE_RTC           2
#define CYPM_WAKE_SLEEP         4
#define CYPM_WAKE_I2C           8
#define CYPM_WAKE_CMP           16

#define ANAIF_CFG_CMP0_CR       ((reg8 *) CYDEV_ANAIF_CFG_CMP0_CR)

#define CMP_PD_OVERRIDE_DISABLE 0 /* Don't override power down. */
#define CMP_PD_OVERRIDE_ENABLE  1 /* Override power down. */ 


#define PM_INT_SR				((reg8 *) CYDEV_PM_INT_SR)

#define PWRSYS_CR1              ((ret8 *) CYDEV_PWRSYS_CR1)

#define I2CREG_EN               (1 << 2) /* When set, enables the I2C regulator. */

#define PM_MODE_CSR             ((reg8 *) CYDEV_PM_MODE_CSR)

#define PM_MODE_REACTIVATE		(1 << 4)

#define PM_INT_SR_LIM_ACT		(1 << 3)


#define PM_MODE_ACTIVE          0x0 /* Active mode. */
#define PM_MODE_STANDBY         0x1 /* Standby mode. */
#define PM_MODE_IDLE            0x2 /* Idle mode. */
#define PM_MODE_SLEEP           0x3 /* Sleep mode. */
#define PM_MODE_HIBERNATE       0x4 /* Hibernate mode. */
#define PM_MODE_HIBTIMERS       0x5 /* Hibernate+timewheels. */

#define PM_CLKDIST_BCFG0 		((reg8 *) CYDEV_CLKDIST_BCFG0)
#define PM_CLKDIST_MSTR0 		((reg8 *) CYDEV_CLKDIST_MSTR0)
#define PM_CLKDIST_MSTR1 		((reg8 *) CYDEV_CLKDIST_MSTR1)

#define MASTER_CLK_SRC_IMO		0
#define MASTER_CLK_DIV			32
#define BUS_CLK_DIV				32



/*******************************************************************************
* Function Name: 
********************************************************************************
* Summary:
* 	Stops the CPU, but the part remains active. Exits wait state on
*   receipt of a non-masked interrupt.
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyWait(void)
{
	uint8 powerMode;


	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_STANDBY;

	/* Recommended readback. */
	powerMode = *PM_MODE_CSR;

	/* Two recommended NOPs to get into the mode. */
	_nop_();
	_nop_();

	/* Sweet Dreams. */

	/* `#START CyWait` Place wakeup code here. */

    /* `#END` */
    
}

/*******************************************************************************
* Function Name: 
********************************************************************************
* Summary:
* 	Puts the part into idle mode. Exits idle state on receipt of a non-masked
*   interrupt.
*
* Parameters:
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyIdle(void)
{
	uint8 powerMode;
    uint8 ClockSource;
    uint8 BusClkDivider;
    uint8 CpuClkDivider;


    /* Before entering idle mode, firmware is responsible for limiting the frequency of clk_cpu and clk_bus, either at the
     * source or in the clock dividers. It must also disable the other clock roots and all other subsystems in the active template.
     * This ensures that the system does not consume more current than the external cap can supply. Refer to Current
     * Consumption on page 517 for more details on current requirements in limited active mode. */

    /* Save the bus clock divider. */
    BusClkDivider = *PM_CLKDIST_BCFG0;

    /* Set divider for the bus clock. */
//    *PM_CLKDIST_BCFG0 = BUS_CLK_DIV;

    /* Save the cpu clock divider. */
    CpuClkDivider = *PM_CLKDIST_MSTR0;

    /* Set divider for the cpu clock. */
//    *PM_CLKDIST_MSTR0 = MASTER_CLK_DIV;

    /* Save old clock source. */
    ClockSource = *PM_CLKDIST_MSTR1;

    /* The user must change the clock source to the IMO before entering Idle to ensure proper shutdown and startup. */
    *PM_CLKDIST_MSTR1 = MASTER_CLK_SRC_IMO;

	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_IDLE;

	/* Recommended readback and two recommended NOPs to get into the mode. */
	powerMode = *PM_MODE_CSR;
	_nop_();
	_nop_();

	/* Sweet Dreams. */

	/* We come out into limited active mode. */
	while(*PM_INT_SR & PM_INT_SR_LIM_ACT)
		;

	*PM_MODE_CSR |= PM_MODE_REACTIVATE;

    /* Restore the bus clock divider. */
//    *PM_CLKDIST_BCFG0 = BusClkDivider;

    /* Restore the cpu clock divider. */
//    *PM_CLKDIST_MSTR0 = CpuClkDivider;

	/* Wait for the PLL to lock. */

	/* Restore old clock source. */
    *PM_CLKDIST_MSTR1 = ClockSource;

	/* Need to wait for clocks to sync up. */



	/* `#START CyIdle` Place wakeup code here. */

    /* `#END` */
}

/*******************************************************************************
* Function Name: 
********************************************************************************
* Summary:
* 	Puts the part into sleep mode. Exits sleep state on receipt of a non-masked
* 	interrupt.
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CySleep(void)
{
	uint8 powerMode;


	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_SLEEP;

	/* Recommended readback. */
	powerMode = *PM_MODE_CSR;

	/* Two recommended NOPs to get into the mode. */
	_nop_();
	_nop_();

	/* Sweet Dreams. */

	/* `#START CySleep` Place wakeup code here. */

    /* `#END` */
}


/*******************************************************************************
* Function Name: 
********************************************************************************
* Summary:
*	Puts the part into hibernate mode. Exits hibernate state on receipt of a
* 	nonmasked port (I/O) interrupt (the other sources cannot wake from
*	hibernate). 
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyHibernate(void)
{
	uint8 powerMode;


	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_HIBERNATE;

	/* Recommended readback. */
	powerMode = *PM_MODE_CSR;

	/* Two recommended NOPs to get into the mode. */
	_nop_();
	_nop_();

	/* Sweet Dreams. */

	/* `#START CyHibernate` Place wakeup code here. */

    /* `#END` */
}



/* CYLIB_POWER_MANAGMENT */
#endif



#if defined(CYLIB_DEBUG)

/*******************************************************************************
* Function Name: CyAssert
********************************************************************************
* Summary:
*   Defined by the user.
*   
*
*
* Parameters:
*   arg. value that failed the assert.
*
*
* Return:
*   
*   
*
* Theory:
*   The user should decide what happens in an assert.  
*   
*
*
*******************************************************************************/
void CyAssert(uint32 arg)
{
    arg = 1;
}

/* CYLIB_DEBUG */
#endif


#if defined(CYLIB_STRING)

/*******************************************************************************
* Function Name: cymemset
********************************************************************************
* Summary:
*   sets n bytes at s to c
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
void * cymemset(void * s, int32 c, int32 n)
{
    void * ss = s;


    CYASSERT((s));
    CYASSERT((n));

    while(n != 0)
    {
        n--;

        *((int8 *) s) = (int8) c;

        s = ((int8 *) s) + 1;
    }

    return ss;
}

/*******************************************************************************
* Function Name: cymemcpy
********************************************************************************
* Summary:
*   MISRA C complient version of standard library memcpy.  
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
void * cymemcpy(void * s1, const void * s2, int32 n)
{
    int8 * ss1 = (int8 *) s1;
    int8 * ss2 = (int8 *) s2;


    CYASSERT((s1));
    CYASSERT((s2));
    CYASSERT((n));

    if(n & 1)
    {
        *ss1 = *ss2;
        ss1 += 1;
        ss2 += 1;
    }
    
    n >>= 1;

    while(n != 0)
    {
        n--;
    
        *((int16 *) ss1) = *((int16 *) ss2);
    
        ss1 += 2;
        ss2 += 2;
    }

    return s1;
}

/*******************************************************************************
* Function Name: cymemmove
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
void * cymemmove(void * s1, void * s2, int32 n)
{
    int8 * ss1 = (int8 *) s1;
    int8 * ss2 = (int8 *) s2;


    CYASSERT((s1));
    CYASSERT((s2));
    CYASSERT((n));

    if(s1 <= s2)
    {
        if(n & 1)
        {
            *ss1 = *ss2;
            ss1 += 1;
            ss2 += 1;
        }
    
        n >>= 1;

        while(n != 0)
        {
            n--;
            *((int16 *) ss1) = *((int16 *) ss2);
        
            ss1 += 2;
            ss2 += 2;
        }
    }
    else
    {
        ss1 += n;
        ss2 += n;

        if(n & 1)
        {
            *ss1 = *ss2;
            ss1 -= 1;
            ss2 -= 1;
        }
    
        n >>= 1;

        while(n != 0)
        {
            n--;
            *((int16 *) ss1) = *((int16 *) ss2);
        
            ss1 -= 2;
            ss2 -= 2;
        }
    }

    return s1;
}

/*******************************************************************************
* Function Name: cymemcmp
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int cymemcmp(const void * s1, const void * s2, uint32 n)
{
    int8 c = 0;
    const uint8 * ss1 = s1;
    const uint8 * ss2 = s2;


    CYASSERT((s1));
    CYASSERT((s2));
    CYASSERT((n));

    while(n != 0)
    {
        n--;

        if(*ss1 != *ss2)
        {
            c = *ss1 - *ss2;
            break;
        }
        else
        {
            ss1++;
            ss2++;
        }
    }
    
    return c;
}

/*******************************************************************************
* Function Name: cymemchr
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
void * cymemchr(const void * s, int c, uint32 n)
{
    uint8 * ss = (unsigned char*) s;


    CYASSERT((s));
    CYASSERT((c));
    CYASSERT((n));

    while(n != 0)
    {
        n--;
        if(*ss != (unsigned char) c)
        {
            ss++;
        }
        else
        {
            return ss;
        }
    }
    return 0;
}

/*******************************************************************************
* Function Name: cy
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrcat(int8 * s1, int8 * s2)
{
    int8 c;
    int8 * ss1 = s1;


    CYASSERT((s1));
    CYASSERT((s2));

    /* find end of s1 */
    while(*ss1)
    {
        ss1++;
    }

    /* Copy s2 to end of s1 */
    do
    {
        c = *ss1++ = *s2++;
        
    } while(c != 0);

    return s1;

}

/*******************************************************************************
* Function Name: cystrcpy
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrcpy(int8 * s1, int8 * s2)
{
    int8 c;
    int8 * ss1 = s1;


    CYASSERT((s1));
    CYASSERT((s2));

    /* Copy src over dst until we get to a NULL */
    do
    {
        c = *ss1++ = *s2++;
        
    } while(c != 0);


    return s1;
}

/*******************************************************************************
* Function Name: cystrncpy
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrncpy(int8 * s1, int8 * s2, uint32 n)
{
    int8 ch;
    int8 * ss1 = s1;
    int8 * ss2 = s2;


    CYASSERT((s1));
    CYASSERT((s2));
    CYASSERT((n));

    while(n != 0)
    {
        n--;
    
        ch = *ss2++;
        *ss1++ = ch;

        if(ch == 0)
        {
            while(n != 0)
            {
                n--;
                *ss1++ = 0;
            }

            break;
        }
    }

    return s1;
}

/*******************************************************************************
* Function Name: cystrlen
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
uint32 cystrlen(const int8 * s)
{
    uint32 l = (uint32) -1;


    CYASSERT((s));

    do
    {
        l++;

    } while(*s++ != 0);

    return l;
}

/*******************************************************************************
* Function Name: cystrcmp
********************************************************************************
* Summary:
*   Compares strings s1 and s2.   
*
*
* Parameters:
*   
*
*
* Return:
*   returns -1 if s1 < s2  
*   returns  0 if s1 = s2
*   returns +1 if s1 > s2
*   
*   
*
*******************************************************************************/
int32 cystrcmp(int8 * s1, int8 * s2)
{
    int32 c;


    CYASSERT((s1));
    CYASSERT((s2));

    do
    {
        c = *((uint8 *)s1) - *((uint8 *)s2);

        s1++;
        s2++;

    } while(c == 0 && *s2 != 0);

    if(c < 0)
    {
        c = -1;
    }
    else if(c > 0)
    {
        c = 1;
    }

    return c;
}

/*******************************************************************************
* Function Name: cystrncmp
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int32 cystrncmp(const int8 * s1, const int8 * s2, uint32 n)
{
    int32 c;

    CYASSERT((s1));
    CYASSERT((s2));
    CYASSERT((n));


    do
    {
        c = *((uint8 *)s1) - *((uint8 *)s2);
        s1++;
        s2++;
        n--;

    } while(n != 0 && c == 0 && *s2 != 0);

    if(c < 0)
    {
        c = -1;
    }
    else if(c > 0)
    {
        c = 1;
    }

    return c;
}

/*******************************************************************************
* Function Name: cystrncat
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrncat(int8 * s1, const int8 * s2, uint32 n)
{
    int8 * ss1 = s1;


    CYASSERT((s1));
    CYASSERT((s2));
    CYASSERT((n));

    while(*ss1 != 0)
    {
        ss1++;
    }

    while(n != 0)
    {
        n--;
    
        if(!(*ss1++ = *s2++))
        {
            break;
        }
    }
    
    *ss1 = 0;

    return s1;
}

/*******************************************************************************
* Function Name: cystrchr
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrchr(const int8 * s, int8 c)
{
    int8 * ss = (int8 *) s;


    CYASSERT((s));
    CYASSERT((c));

    while(*ss != 0 && *ss != (int8) c)
    {
        s++;
    }

    if(*ss != c)
    {
        ss = 0;
    }

    return ss;
}

/*******************************************************************************
* Function Name: cystrrchr
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrrchr(const int8 * s, int8 c)
{
    int8 * ss;


    CYASSERT((s));
    CYASSERT((c));

    for(ss = 0; *s != 0; s++)
    {
        if(*s == (char) c)
        {
            ss = (int8 *) s;
        }
        
    }

    return ss;
}

/*******************************************************************************
* Function Name: cystrstr
********************************************************************************
* Summary:
*   
*
*
* Parameters:
*   
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int8 * cystrstr(const int8 * s1, const int8 * s2)
{
    int8 * ss = 0;
    uint32 n = cystrlen(s2);


    CYASSERT((s1));
    CYASSERT((s2));

    while(*s1)
    {
        if(cymemcmp(s1, s2, n) == 0)
        {
            ss = (int8 *) s1;
            break;
        }

        s1++;
    }

    return ss;
}

/* This way we do not depend on the div instruction. */
#define MAX_B16_VAL         ((uint32) ULONG_MAX / (uint32) 16)
#define MAX_B16_DIGIT       ((uint32) ULONG_MAX % (uint32) 16)
#define MAX_B10_VAL         ((uint32) ULONG_MAX / (uint32) 10)
#define MAX_B10_DIGIT       ((uint32) ULONG_MAX % (uint32) 10)
#define MAX_B8_VAL          ((uint32) ULONG_MAX / (uint32) 8)
#define MAX_B8_DIGIT        ((uint32) ULONG_MAX % (uint32) 8)

/*******************************************************************************
* Function Name: cystrstr
********************************************************************************
* Summary:
*   Reads a string and generates an integer.  
*
*
* Parameters:
*   s, String to convert to a in32 value.   
*
*   e, Pointer to a pointer to store the first invalid character. Can be
*      NULL if not needed.
*
*   b, Base of the string. We only support 16, 10 and 8.
*
*
* Return:
*   
*   
*
* Theory:
*   
*
*
*******************************************************************************/
int32 cystrtol(const int8 * s, int8 * * e, int8 b)
{
    int8 digit;
    int32 some;
    int32 cutlim;
    int32 negitive;
    uint32 value;
    uint32 cutoff;
    const int8 * ss;


    /* Initialize. */
    ss = s;
    value = 0;
    some = 0;

    /* Pass spaces. */
    while(isspace(*ss));
    {
        ss++;
    } 

    /* Look for a sign. */
    if(*ss == '-')
    {
        ss++;
        negitive = 1;
    }
    else if(*ss == '+')
    {
        ss++;
        negitive = 0;
    }
    
    /* Figure out the base. */
    if((b == 0 || b == 16) && ss[0] == '0' && ( ss[1] | 0x20) == 'x')
    {
        ss += 2;
        b = 16;

        cutoff = MAX_B16_VAL;
        cutlim = MAX_B16_DIGIT;
    }
    else if(b == 0)
    {
        b = (*ss == '0') ? 8 : 10;
        if(b == 8)
        {
            cutoff = MAX_B8_VAL;
            cutlim = MAX_B8_DIGIT;
        }
        else
        {
            cutoff = MAX_B10_VAL;
            cutlim = MAX_B10_DIGIT;
        }
    }
    else if(b == 8)
    {
        cutoff = MAX_B8_VAL;
        cutlim = MAX_B8_DIGIT;
    }
    else
    {
        CYBREAK((0));
    }

    /* Calculate the value. */
    for(;; ss++)
    {
        digit = *ss;
        
        if(isdigit(digit))
        {
            digit -= '0';
        }
        else if(isalpha(digit))
        {
            digit = (digit | 0x20) - 'A' + 10;
        }    
        else
        {
            break;
        }
        
        if((int32) digit >= b)
        {
            break;
        }

        if(some < 0 || value > cutoff || (value == cutoff && (int32) digit > cutlim))
        {
            some = -1;
        }
        else
        {
            some = 1;
            value *= b;
            value += digit;
        }
    }

    /* Did we get more than an int32? */
    if(some < 0)
    {
        value = ULONG_MAX;
    }
    else if(negitive)
    {
        value = -value;
    }

    /* If we were given a end pointer set it to the first invalid character. */
    if(e != NULL)
    {
        *e = (int8 *) (some ? (int8 *) ss - 1 : s);
    }

    return value;
}

/* CYLIB_STRING */
#endif


#if defined(CYLIB_CACHE)

extern void cyconfigcpy(unsigned short size, const void far *src, void far *dest) small;

/*******************************************************************************
* Function Name: CyCacheLoadLockedLine
********************************************************************************
* Summary:
*   Loads code into the cache SRAM and locks it. A line of code must be 64 bytes
*   long and 64 byte aligned.
*
*
* Parameters:
*   line:
*       The line number in cache to load.
*
*
*   address:
*       Pointes to 64 bytes of code to load.
*       
*
* Return:
*   CYRET_SUCCESS or CYRET_BAD_PARAM.
*
*
*******************************************************************************/
cystatus CyCacheLoadLockedLine(uint8 line, void * address)
{
    uint8 far * cache;
    cystatus status;


    cache = CACHE_ADRR_SPACE_CACHESRAM + line * CACHE_LINE_SIZEOF;

    if(!(((uint8) address) & 0x3F) || line > 7)
    {
        CACHE_ENTER

        /* Fill the cache sram. */
        cyconfigcpy(CACHE_LINE_SIZEOF, address, cache);

        /* Set the address and row bits as valid and locked. */
        CACHE_TAG[line] = (0xFF << 24) | ((uint16) address) | 3;

        status = CYRET_SUCCESS;

        CACHE_EXIT 
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }

    return status;
} 

/*******************************************************************************
* Function Name: CyCacheUnlockLines
********************************************************************************
* Summary:
*   Unlocks a line in cache.
*
*
* Parameters:
*   lines:
*       The line number to unlock.
*
*
* Return:
*   void.
*
*
*******************************************************************************/
cystatus CyCacheUnlockLine(uint8 line)
{
    cystatus status;


    if(line > 7)
    {
        /* Unlock and invalidate the whole line. */
        CACHE_TAG[line] &= ~CC_TAG_LOCK;
        
        status = CYRET_SUCCESS;
    }
    else
    {
        status = CYRET_BAD_PARAM;
    }

    return status;
}

/* CYLIB_CACHE */
#endif

/*******************************************************************************
* Function Name: CyEnableDigitalArray
********************************************************************************
* Summary:
*   Uses the PM system to enable the UDB array within the device
*
*
* Parameters:
*   none
*
* Return:
*   void.
*
*
*******************************************************************************/
void CyEnableDigitalArray()
{
    CY_SET_REG8(CYDEV_PM_ACT_CFG0, CY_GET_REG8(CYDEV_PM_ACT_CFG0) | 0x40);
    CY_SET_REG8(CYDEV_PM_STBY_CFG0, CY_GET_REG8(CYDEV_PM_STBY_CFG0) | 0x40);
    CY_SET_REG8(CYDEV_PM_AVAIL_CR2, CY_GET_REG8(CYDEV_PM_AVAIL_CR2) | 0x10);
}

/*******************************************************************************
* Function Name: CyDisableDigitalArray
********************************************************************************
* Summary:
*   Uses the PM system to disable the UDB array within the device
*
*
* Parameters:
*   none
*
* Return:
*   void.
*
*
*******************************************************************************/
void CyDisableDigitalArray()
{
    CY_SET_REG8(CYDEV_PM_AVAIL_CR2, CY_GET_REG8(CYDEV_PM_AVAIL_CR2) & 0xEF);
    CY_SET_REG8(CYDEV_PM_STBY_CFG0, CY_GET_REG8(CYDEV_PM_STBY_CFG0) & 0xBF);
    CY_SET_REG8(CYDEV_PM_ACT_CFG0, CY_GET_REG8(CYDEV_PM_ACT_CFG0) & 0xBF);
}

/*******************************************************************************
* Function Name: CyDelay
********************************************************************************
* Summary:
*   Blocks for miliseconds.
*
*
*       T E M P O R A R Y  until we get a clock and interrupt.
*
*
* Parameters:
*   milliSeconds: number of miliseconds to delay.
*
*
* Return:
*   void.
*
*
*******************************************************************************/
void CyDelay(uint32 milliSeconds)
{
    uint32 number;
    static uint32 cycles; 
    static uint8 data GotCount = 0;


#define CYDELAY_OVERHEAD            255
#define CYDELAY_LOOP_CYCLES         96


    CYASSERT((milliSeconds));
    if(GotCount == 0)
    {
        /* Get the bus clock. */
        cycles = BCLK__BUS_CLK__HZ;

        /* Get 8051 bus clock. */
        cycles /= 1 + CY_GET_XTND_REG8(CYDEV_SFR_USER_CPUCLK_DIV);
        
        /* Get Cycles per millisecond. */
        cycles /= 1000;
    }

    /* Get Total wait in cycles. */
    number = cycles * milliSeconds;

    /* Remove function overhead. */
    number -= CYDELAY_OVERHEAD;

    while(number > CYDELAY_LOOP_CYCLES)
    {
        number -= CYDELAY_LOOP_CYCLES;
    }
}


#define INTC_SET_EN         ((reg32 *) CYDEV_INTC_SET_EN0)
#define INTC_CLR_EN         ((reg32 *) CYDEV_INTC_CLR_EN0)


/*******************************************************************************
* Function Name: CyDisableInts
********************************************************************************
* Summary:
*  Disables all interrupts.  
*   
*
*
* Parameters: None.
*   
*
*
* Return: 32 bit mask of previously enabled interrupts.
*    
*
*******************************************************************************/
uint32 CyDisableInts(void)
{
    uint32 intState;


    /* Get the curreent interrutp state. */
    intState = *INTC_CLR_EN;

    /* Disable all of the interrutps. */
    *INTC_CLR_EN = 0;

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
*   intState, 32 bit mask of interrupts to enable.
*
*
* Return: Void.
*   
*   
*******************************************************************************/
void CyEnableInts(uint32 intState)
{
    /* Set interrupts as enabled. */
    *INTC_SET_EN = intState;
}









































