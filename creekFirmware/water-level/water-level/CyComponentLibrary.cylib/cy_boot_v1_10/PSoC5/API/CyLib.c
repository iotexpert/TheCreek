/*******************************************************************************
* File Name: CyLib.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
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



#include <CYDEVICE.H>
#include <CYLIB.H>








#if defined(CYLIB_SYSTEM_CLOCKS)


#define FASTCLK_PLL_CONTROL0   ((reg8 *) CYDEV_FASTCLK_PLL_CFG0)
#define PLL_CONTROL_ENABLE     (1 << 0) /* Enables the PLL. */

#define FASTCLK_PLL_STATUS     ((reg8 *) CYDEV_FASTCLK_PLL_SR)
#define PLL_STATUS_LOCK  	   (1 << 0) /* Lock Status. */

#define PM_IMO_ACT_REG         ((reg8 *) CYDEV_PM_ACT_CFG0)
#define IMO_PM_ENABLE          (1 << 4) /* Enable IMO clock source. */

#define FASTCLK_XMHZ_CONTROL   ((reg8 *) CYDEV_FASTCLK_XMHZ_CSR)
#define XMHZ_CONTROL_ENABLE    (1 << 0) /* Enables the 4 - 33 MHz crystal oscillator circuit when set high. */

#define SLOWCLK_X32_CONTROL    ((reg8 *) CYDEV_SLOWCLK_X32_CR)
#define X32_CONTROL_ANA_STAT   (1 << 5) /* Indicates oscillator status, using internal analog measurement. */
#define X32_CONTROL_DIG_STAT   (1 << 4) /* Indicates oscillator status, using test against a reference clock. The ILO's CLK33K must be enabled for this function to operate. */
#define X32_CONTROL_LPM        (1 << 1) /* Power setting for 32K crystal oscillator. This setting only takes effect in sleep modes. During active modes, the oscillator always runs in high power mode. */
#define X32_CONTROL_X32EN      (1 << 0) /* 32K Crystal Oscillator Enable. */

#define SLOWCLK_ILO_CONTROL0   ((reg8 *) CYDEV_SLOWCLK_ILO_CR0)
#define ILO_CONTROL_PD_MODE    (1 << 4) /* Power down mode for ILO. */


/*******************************************************************************
* Function Name: CyPLL_OUT_Start
********************************************************************************
* Summary:
*   Enables PLL, and waits for it to become stable.
*
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyPLL_OUT_Start(void)
{
    *FASTCLK_PLL_CONTROL0 |= XMHZ_CONTROL_ENABLE;

    while(!(*FASTCLK_PLL_STATUS & PLL_STATUS_LOCK))
        ;
}


/*******************************************************************************
* Function Name: CyPLL_OUT_Stop
********************************************************************************
* Summary:
*   Disables the PLL.
*
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyPLL_OUT_Stop(void)
{
    /* Disable the the oscillator. */
    *FASTCLK_PLL_CONTROL0 &= ~XMHZ_CONTROL_ENABLE;
}


/*******************************************************************************
* Function Name: CyIMO_Start
********************************************************************************
* Summary:
*   Enables the Internal Main oscillator by setting the enable bit in the
*   'Active Power Mode Configuration Register' corresponding to this clock.
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyIMO_Start(void)
{
    /* Set the bit to enable the clock. */
    *PM_IMO_ACT_REG |= IMO_PM_ENABLE;
}


/*******************************************************************************
* Function Name: CyIMO_Stop
********************************************************************************
* Summary:
*   Disables the Internal Main oscillator by clearing the enable bit in the
*   'Active Power Mode Configuration Register' corresponding to this clock.
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyIMO_Stop(void)
{
    /* Clear the bit to disable the clock. */
    *PM_IMO_ACT_REG &= ~IMO_PM_ENABLE;
}


/*******************************************************************************
* Function Name: CyXTAL_Start
********************************************************************************
* Summary:
*   Enables the 4 - 33 MHz crystal oscillator.
*
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyXTAL_Start(void)
{
    *FASTCLK_XMHZ_CONTROL |= XMHZ_CONTROL_ENABLE;
}


/*******************************************************************************
* Function Name: CyXTAL_Stop
********************************************************************************
* Summary:
*   Disables the 4 - 33 MHz crystal oscillator.
*
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyXTAL_Stop(void)
{
    /* Disable the the oscillator. */
    *FASTCLK_XMHZ_CONTROL &= ~XMHZ_CONTROL_ENABLE;
}


/*******************************************************************************
* Function Name: CyXTAL_32KHZ_Start
********************************************************************************
* Summary:
*   Enables the xternal 32k oscillator, and waits for it to become stable.
*
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyXTAL_32KHZ_Start(void)
{
    /* Enable the the oscillator. */
    *SLOWCLK_X32_CONTROL |= X32_CONTROL_X32EN;

    /* Wait for the oscillator to stablize. */
    while(!(*SLOWCLK_X32_CONTROL & X32_CONTROL_ANA_STAT))
        ;
}


/*******************************************************************************
* Function Name: CyXTAL_32KHZ_Stop
********************************************************************************
* Summary:
*   Disables the xternal 32k oscillator.
*
*
* Parameters:  
*   void.
*
*
* Return:
*   void.
*
*******************************************************************************/
void CyXTAL_32KHZ_Stop(void)
{
    /* Disable the the oscillator. */
    *SLOWCLK_X32_CONTROL &= ~X32_CONTROL_X32EN;
}

/*******************************************************************************
* Function Name: CyXTAL_32KHZ_SetPowerMode
********************************************************************************
* Summary:
*   Alows the caller to set the power mode for the xternal 32k oscillator
*   This setting only takes effect in sleep modes. During active modes,
*   the oscillator always runs in high power mode.
*
* Parameters:  
*   mode, 0 = High Power Mode, 1 = Low Power Mode (Only applies in chip sleep mode).
*
*
* Return:
*   Prevous power mode state.
*
*******************************************************************************/
uint8 CyXTAL_32KHZ_SetPowerMode(uint8 mode)
{
    uint8 state;


    /* Get current state. */
    state = *SLOWCLK_X32_CONTROL;

    /* Set the the oscillator power mode. */
    if(mode)
    {
        *SLOWCLK_X32_CONTROL = state | X32_CONTROL_LPM;
    }
    else
    {
        *SLOWCLK_X32_CONTROL = state & ~X32_CONTROL_LPM;
    }

    /* Return the old mode. */
    return state & X32_CONTROL_LPM;    
}


/*******************************************************************************
* Function Name: CyILO_SetPowerMode
********************************************************************************
* Summary:
*   Alows the caller to set the power mode for the ILO clock
*   
*
* Parameters:  
*   mode, 0 = Faster start-up, internal bias left on,
*         1 = Slower start-up, internal bias off.
*
*
* Return:
*   Prevous power mode state.
*
*******************************************************************************/
uint8 CyILO_SetPowerMode(uint8 mode)
{
    uint8 state;


    /* Get current state. */
    state = *SLOWCLK_ILO_CONTROL0;

    /* Set the the oscillator power mode. */
    if(mode)
    {
        *SLOWCLK_ILO_CONTROL0 = state | ILO_CONTROL_PD_MODE;
    }
    else
    {
        *SLOWCLK_ILO_CONTROL0 = state & ~ILO_CONTROL_PD_MODE;
    }

    /* Return the old mode. */
    return state & ILO_CONTROL_PD_MODE;    
}

/* CYLIB_SYSTEM_CLOCKS */
#endif


#if defined(CYLIB_POWER_MANAGEMENT)

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
* Function Name: CyWait
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
	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_STANDBY;

	/* Recommended readback. */
	*PM_MODE_CSR;

	/* Two recommended NOPs to get into the mode. */
	CY_NOP;
	CY_NOP;

	/* Sweet Dreams. */
}

/*******************************************************************************
* Function Name: CyIdle
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
	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_IDLE;

	/* Recommended readback and two recommended NOPs to get into the mode. */
	*PM_MODE_CSR;
	CY_NOP;
	CY_NOP;

	/* Sweet Dreams. */

	/* We should come out into limited active mode. */

	/* `#START CyIdle` Place limited active mode code here. */

    /* `#END` */

	/* Wait until the system is ready to be reactivated. */
	while(!(*PM_INT_SR & PM_INT_SR_LIM_ACT))
		;

	/* Return to active mode. */
	*PM_MODE_CSR |= PM_MODE_REACTIVATE;
}

/*******************************************************************************
* Function Name: CySleep
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
	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_SLEEP;

	/* Recommended readback. */
	*PM_MODE_CSR;

	/* Two recommended NOPs to get into the mode. */
	CY_NOP;
	CY_NOP;

	/* Sweet Dreams. */
}


/*******************************************************************************
* Function Name: CyHibernate
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
	/* Put device in a different mode. */
	*PM_MODE_CSR = PM_MODE_HIBERNATE;

	/* Recommended readback. */
	*PM_MODE_CSR;

	/* Two recommended NOPs to get into the mode. */
	CY_NOP;
	CY_NOP;

	/* Sweet Dreams. */
}


/* CYLIB_POWER_MANAGEMENT */
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
* Function Name: cystrcat
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
char * cystrcat(char * s1, char * s2)
{
    char c;
    char * ss1 = s1;


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
char * cystrcpy(char * s1, char * s2)
{
    char c;
    char * ss1 = s1;


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
char * cystrncpy(char * s1, char * s2, uint32 n)
{
    char ch;
    char * ss1 = s1;
    char * ss2 = s2;


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
uint32 cystrlen(const char * s)
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
int32 cystrcmp(char * s1, char * s2)
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
int32 cystrncmp(const char * s1, const char * s2, uint32 n)
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
char * cystrncat(char * s1, const char * s2, uint32 n)
{
    char * ss1 = s1;
    char temp;


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
    
        //if(!(*ss1++ = *s2++))
        temp = *ss1 = *s2;
        ss1++;
        s2++;

        if(!temp)
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
char * cystrchr(const char * s, char c)
{
    char * ss = (char *) s;


    CYASSERT((s));
    CYASSERT((c));

    while(*ss != 0 && *ss != (char) c)
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
char * cystrrchr(const char * s, char c)
{
    char * ss;


    CYASSERT((s));
    CYASSERT((c));

    for(ss = 0; *s != 0; s++)
    {
        if(*s == (char) c)
        {
            ss = (char *) s;
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
char * cystrstr(const char * s1, const char * s2)
{
    char * ss = 0;
    uint32 n = cystrlen(s2);


    CYASSERT((s1));
    CYASSERT((s2));

    while(*s1)
    {
        if(cymemcmp(s1, s2, n) == 0)
        {
            ss = (char *) s1;
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
int32 cystrtol(const char * s, char * * e, int8 b)
{
    char digit;
    int32 some;
    int32 cutlim;
    int32 negitive;
    uint32 value;
    uint32 cutoff;
    const char * ss;


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
        *e = (char *) (some ? (char *) ss - 1 : s);
    }

    return value;
}






/* CYLIB_STRING */
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
    static uint8 GotCount = 0;


#define CYDELAY_OVERHEAD            255
#define CYDELAY_LOOP_CYCLES         96


    CYASSERT((milliSeconds));
    if(GotCount == 0)
    {
        /* Get the bus clock. */
        cycles = BCLK__BUS_CLK__HZ;
        
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

// Redo when in cydevice.h
#define NVIC_SET_EN         ((reg32 *) 0xE000E100)
#define NVIC_CLR_EN         ((reg32 *) 0xE000E180)

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
* Return: 32 bit mask, of previously enabled interrupts.
*    
*
*******************************************************************************/
uint32 CyDisableInts(void)
{
    uint32 intState;


    /* Get the curreent interrutp state. */
    intState = *NVIC_CLR_EN;

    /* Disable all of the interrupts. */
    *NVIC_CLR_EN = 0xFFFFFFFF;

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
*   intState, 32 bit mask, of interrupts to enable.
*
*
* Return: Void.
*   
*   
*******************************************************************************/
void CyEnableInts(uint32 intState)
{
    /* Set interrupts as enabled. */
    *NVIC_SET_EN = intState;
}

/*******************************************************************************
* Function Name: CYDEV_FATAL_ABORT
********************************************************************************
* Summary:
*  Halts the CPU 
*
*
* Parameters:
*   reason, string containing a message explaining the reason for the halt
*
*
* Return: Void.
*   
*   
*******************************************************************************/
void CYDEV_FATAL_ABORT(unsigned char reason)
{
    reason = reason;
    CYDEV_HALT_CPU;
}

#define RESET_CR2         ((reg8 *) CYDEV_RESET_CR2)

/*******************************************************************************
* Function Name: CySoftwareReset
********************************************************************************
* Summary:
*  Performs a software reset
*
*
* Parameters:
*  none 
*
*
* Return: Void.
*   
*   
*******************************************************************************/
void CySoftwareReset()
{
    /* Perform a reset by writing the software reset bit in reset control register 2 */	
    *RESET_CR2 = 0x1;
}

#define CACHE_CC_CTL         ((reg16 *) CYDEV_CACHE_CC_CTL)

/*******************************************************************************
* Function Name: CyFlushCache
********************************************************************************
* Summary:
*  Performs a cache flush by invalidating the cache contents
*
*
* Parameters:
*  none 
*
*
* Return: Void.
*   
*   
*******************************************************************************/
void CyFlushCache()
{	
      /* read the register, set the flush bit, write back */
	uint16 ctrlreg = *CACHE_CC_CTL;
	ctrlreg |= 0x4; // set bit 2
	*CACHE_CC_CTL = ctrlreg;
	/* cache is invalidated on next cycle */

      /* Execute a few NOPs to fill the ARM CM3 pipeline with instructions
       so that instructions about to execute won't depend on data in the cache,
       and will maintain data integrity. */
	CY_NOP;
	CY_NOP;
	CY_NOP;
	CY_NOP;
}



