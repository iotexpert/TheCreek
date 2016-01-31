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


#include <CYDEVICE.H>
#include <CYLIB.H>



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

    /* Disable all of the interrutps. */
    *NVIC_CLR_EN = 0;

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








