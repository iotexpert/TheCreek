/*******************************************************************************
* File Name: `$INSTANCE_NAME`_CRC.c  
* Version `$VERSION_MAJOR`.`$VERSION_MINOR`
*
*  Description:
*     The CRC Component consists of a 8, 16, 24, 32, 64-bits CRC with
*     a selectable Polynomial and Seed Values. 
*
*   Note:
*     None
*
*******************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "`@INSTANCE_NAME`.h"

uint8 `@INSTANCE_NAME`_firstTime = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Initializes seed and polynomial registers. Computation of CRC
*  starts on rising edge of input clock. 
*
* Parameters:  
*  void
*
* Return: 
*  void
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void)
{
    /* Writes seed value and ponynom value provided for customizer */
    if (`$INSTANCE_NAME`_firstTime == 0)
    {
        #if (`$INSTANCE_NAME`_CRCSize <= 32)
            `$INSTANCE_NAME`_WritePolynomial(`$INSTANCE_NAME`_DEFAULT_POLYNOM);
            `$INSTANCE_NAME`_WriteSeed(`$INSTANCE_NAME`_DEFAULT_SEED);
        #else
            `$INSTANCE_NAME`_WritePolynomialUpper(`$INSTANCE_NAME`_DEFAULT_POLYNOM_UPPER);
            `$INSTANCE_NAME`_WritePolynomialLower(`$INSTANCE_NAME`_DEFAULT_POLYNOM_LOWER);
            `$INSTANCE_NAME`_WriteSeedUpper(`$INSTANCE_NAME`_DEFAULT_SEED_UPPER);
            `$INSTANCE_NAME`_WriteSeedLower(`$INSTANCE_NAME`_DEFAULT_SEED_LOWER);
        #endif
      
        `$INSTANCE_NAME`_firstTime = 0x01u;
    }
      
    `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
* Summary:
*  Stops CRC computation. 
*
* Parameters:  
*  void
*
* Return: 
*  void
* 
*******************************************************************************/
 void `$INSTANCE_NAME`_Stop(void)
{
    `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Reset
********************************************************************************
* Summary:
*  Sets default seed value.
*
* Parameters:  
*  void
*
* Return: 
*  void
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Reset(void)
{
    /* Writes seed value provided from customizer */
    #if (`$INSTANCE_NAME`_CRCSize <= 32)
        `$INSTANCE_NAME`_WriteSeed(`$INSTANCE_NAME`_DEFAULT_SEED);
    #else
        `$INSTANCE_NAME`_WriteSeedUpper(`$INSTANCE_NAME`_DEFAULT_SEED_UPPER);
        `$INSTANCE_NAME`_WriteSeedLower(`$INSTANCE_NAME`_DEFAULT_SEED_LOWER);
    #endif
}


#if(`$INSTANCE_NAME`_CRCSize <= 32) /* 8-32bit - CRC */
/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadCRC
********************************************************************************
* Summary:
*  Reads CRC value.
*
* Parameters:
*  void
*
* Return: 
*  Returns CRC value.
*
*******************************************************************************/
`$RegSize` `$INSTANCE_NAME`_ReadCRC(void)
{
    `$RegSize` seed;
    
    #if (`$INSTANCE_NAME`_CRCSize <= 8)         /* 8bits - CRC */
        /* Read CRC */
        seed = `$INSTANCE_NAME`_SEED_A__A0_REG ;
    
    #elif (`$INSTANCE_NAME`_CRCSize <= 16)      /* 16bits - CRC */
	    /* Read CRC */
        seed = ((uint16) `$INSTANCE_NAME`_SEED_A__A1_REG) << 8;
	    seed |= `$INSTANCE_NAME`_SEED_A__A0_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 24)      /* 24bits - CRC */
	    /* Read CRC */
        seed = ((uint32) (`$INSTANCE_NAME`_SEED_B__A1_REG)) << 16;
	    seed |= ((uint32) (`$INSTANCE_NAME`_SEED_B__A0_REG)) << 8;
	    seed |= `$INSTANCE_NAME`_SEED_A__A0_REG;       
        
    #else   /* 32bits - CRC */
        /* Read CRC */
	    seed = ((uint32) `$INSTANCE_NAME`_SEED_B__A1_REG) << 24;
	    seed |= ((uint32) `$INSTANCE_NAME`_SEED_A__A1_REG) << 16;
	    seed |= ((uint32) `$INSTANCE_NAME`_SEED_B__A0_REG) << 8;
	    seed |= `$INSTANCE_NAME`_SEED_A__A0_REG;
    #endif
    
    return (seed & `$INSTANCE_NAME`_MASK);
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteSeed
********************************************************************************
* Summary:
*  Writes seed value. 
*
* Parameters:
*  seed: Seed value.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeed(`$RegSize` seed)
{
    #if (`$INSTANCE_NAME`_CRCSize <= 8)         /* 8bits - CRC */
        /* Write Seed */
        `$INSTANCE_NAME`_SEED_A__A0_REG = seed;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 16)      /* 16bits - CRC */
        /* Write Seed */
        `$INSTANCE_NAME`_SEED_A__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_A__A0_REG = LO8(seed);
  
        /* Reset triger */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 24)      /* 24bits - CRC */
        /* Write Seed */
        `$INSTANCE_NAME`_SEED_B__A1_REG = LO8(HI16(seed));
	    `$INSTANCE_NAME`_SEED_B__A0_REG = HI8(seed);
	    `$INSTANCE_NAME`_SEED_A__A0_REG = LO8(seed);
  
        /* Reset triger */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
        
    #else   /* 32bits - CRC */
        /* Write Seed */ 
        `$INSTANCE_NAME`_SEED_B__A1_REG = HI8(HI16(seed));
	    `$INSTANCE_NAME`_SEED_A__A1_REG = LO8(HI16(seed));
	    `$INSTANCE_NAME`_SEED_B__A0_REG = HI8(seed);
	    `$INSTANCE_NAME`_SEED_A__A0_REG = LO8(seed);
  
        /* Reset triger */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
    #endif
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadPolynomial
********************************************************************************
* Summary:
*  Reads CRC polynomial value.
*
* Parameters:
*  void
*
* Return: 
*  Returns CRC polynomial value.
*
*******************************************************************************/
`$RegSize` `$INSTANCE_NAME`_ReadPolynomial(void)
{
	`$RegSize` polynomial;
    
    #if (`$INSTANCE_NAME`_CRCSize <= 8)         /* 8bits - CRC */
        /* Read polynomial */
        polynomial = `$INSTANCE_NAME`_POLYNOM_A__D0_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 16)      /* 16bits - CRC */
        /* Read polynomial */
	    polynomial = ((uint16) `$INSTANCE_NAME`_POLYNOM_A__D1_REG) << 8;
	    polynomial |= (`$INSTANCE_NAME`_POLYNOM_A__D0_REG);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 24)      /* 24bits - CRC */
        /* Read polynomial */
	    polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D1_REG) << 16;
	    polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D0_REG) << 8;
	    polynomial |= `$INSTANCE_NAME`_POLYNOM_A__D0_REG;
        
    #else   /* 32bits - CRC */
        /* Read polynomial */
	    polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D1_REG) << 24;
	    polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_A__D1_REG) << 16;
	    polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D0_REG) << 8;
	    polynomial |= `$INSTANCE_NAME`_POLYNOM_A__D0_REG;        
    #endif
    
    return polynomial;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WritePolynomial
********************************************************************************
* Summary:
*  Writes CRC polynomial value. 
*
* Parameters:
*  polynomial: CRC polynomial.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomial(`$RegSize` polynomial)
{
	#if (`$INSTANCE_NAME`_CRCSize <= 8)         /* 8bits - CRC */
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_A__D0_REG = polynomial;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 16)      /* 16bits - CRC */
        /* Write polynomial */
	    `$INSTANCE_NAME`_POLYNOM_A__D1_REG = HI8(polynomial);
	    `$INSTANCE_NAME`_POLYNOM_A__D0_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 24)      /* 24bits - CRC */
    	/* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_B__D1_REG = LO8(HI16(polynomial));
	    `$INSTANCE_NAME`_POLYNOM_B__D0_REG = HI8(polynomial);
	    `$INSTANCE_NAME`_POLYNOM_A__D0_REG = LO8(polynomial);
        
    #else   /* 32bits - CRC */
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_B__D1_REG = HI8(HI16(polynomial));
	    `$INSTANCE_NAME`_POLYNOM_A__D1_REG = LO8(HI16(polynomial));
    	`$INSTANCE_NAME`_POLYNOM_B__D0_REG = HI8(polynomial);
	    `$INSTANCE_NAME`_POLYNOM_A__D0_REG = LO8(polynomial);
        
    #endif
}


#else  /* 64bit - CRC */
/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadCRCUpper
********************************************************************************
* Summary:
*  Reads upper half of CRC value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  void
*
* Return: 
*  Returns upper half of CRC value. 
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadCRCUpper(void)
{
    uint32 seed;
  
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Read CRC Upper */
        seed = `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Read CRC Upper */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 56)          /* 56bits - CRC */
        /* Read CRC Upper */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG;
        
    #else   /* 64bits - CRC */
        /* Read CRC Upper */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG;
    #endif
  
	return seed;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadCRCLower
********************************************************************************
* Summary:
*  Reads lower half of CRC value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  void
*
* Return: 
*  Returns lower half of CRC value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadCRCLower(void)
{
    uint32 seed;
  
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Read CRC Lower */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Read CRC Lower */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG;
        
    #else   /* 64bits - CRC */
        /* Read CRC Lower */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG;
    #endif
  
	return seed;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteSeedUpper
********************************************************************************
* Summary:
*  Writes upper half of seed value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  seed: Upper half of seed value.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeedUpper(uint32 seed)
{
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = LO8(seed);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG = LO8(seed);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 56)          /* 56bits - CRC */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG = HI8(seed);
        
    #else   /* 64bits - CRC */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG = HI8(HI16(seed));
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG = LO8(seed);
    #endif
   
    /* Reset triger */
    `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
    `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WriteSeedLower
********************************************************************************
* Summary:
*  Writes lower half of seed value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  seed: Lower half of seed value.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeedLower(uint32 seed)
{
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Write Seed Lower */
        `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG = HI8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG = LO8(seed);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Write Seed Lower */
        `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG = HI8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG = LO8(seed);
        
    #else   /* 64bits - CRC */
        /* Write Seed Lower */
        `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG = HI8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG = LO8(seed);
    #endif
   
    /* Reset triger */
    `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
    `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadPolynomialUpper
********************************************************************************
* Summary:
*  Reads upper half of CRC polynomial value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  void
*
* Return: 
*  Returns upper half of CRC polynomial value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadPolynomialUpper(void)
{
    uint32 polynomial;
  
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Read Polynomial Upper */
        polynomial = `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Read Polynomial Upper */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 56)          /* 56bits - CRC */
        /* Read Polynomial Upper */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG;
        
    #else   /* 64bits - CRC */
        /* Read Polynomial Upper */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG;
    #endif
  
  
    return polynomial;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadPolynomialLower
********************************************************************************
* Summary:
*  Reads lower half of CRC polynomial value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  void
*
* Return: 
*  Returns lower half of CRC polynomial value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadPolynomialLower(void)
{
    uint32 polynomial;
   
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Read Polynomial Lower */
        polynomial = ( (uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG) << 24;
        polynomial |= ( (uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG) << 16;
        polynomial |= ( (uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG;
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Read Polynomial Lower */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG;
        
    #else   /* 64bits - CRC */
        /* Read Polynomial Lower */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG;
    #endif
   
    return polynomial;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WritePolynomialUpper
********************************************************************************
* Summary:
*  Writes upper half of CRC polynomial value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  polynomial: Upper half CRC polynomial value.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomialUpper(uint32 polynomial)
{
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 56)          /* 56bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);
        
    #else   /* 64bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG = LO8(polynomial);
    #endif
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_WritePolynomialLower
********************************************************************************
* Summary:
*  Writes lower half of CRC polynomial value. Only generated for 33-64-bit CRC.
*
* Parameters:
*  polynomial: Lower half of CRC polynomial value.
*
* Return: 
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomialLower(uint32 polynomial)
{
    #if (`$INSTANCE_NAME`_CRCSize <= 40)            /* 40bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_CRCSize <= 48)          /* 48bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);
    #else   /* 64bits - CRC */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);
    #endif
}

#endif /* `$INSTANCE_NAME`_CRCSize */


/* [] END OF FILE */
