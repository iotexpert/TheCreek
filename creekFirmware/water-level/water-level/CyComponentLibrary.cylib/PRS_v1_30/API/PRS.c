/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides the source code to the API for the PRS Component
*
* Note:
*  None
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "`$INSTANCE_NAME`.h"

uint8 `$INSTANCE_NAME`_firstTime = 0;


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
* Summary:
*  Initializes seed and polynomial registers. Computation of PRS
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
        #if (`$INSTANCE_NAME`_PRS_SIZE <= 32)
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
*  Stops PRS computation.
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


#if (`$INSTANCE_NAME`_RUN_MODE > 0)
/*******************************************************************************
* FUNCTION NAME: `$INSTANCE_NAME`_Step
********************************************************************************
* 
* Summary:
*  Increments the PRS by one when API single step mode is used.
*
* Parameters:
*  void
* 
* Return:
*  void
* 
*******************************************************************************/
void `$INSTANCE_NAME`_Step(void)
{
    `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RISING_EDGE;
    
    /* TODO
        need to immplement delay
    */
    
    `$INSTANCE_NAME`_CONTROL &= ~ `$INSTANCE_NAME`_CTRL_RISING_EDGE;
    
    /* Need two clock pulse because timemultiplexing implemented*/
    #if (`$INSTANCE_NAME`_PRS_SIZE > 8)
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RISING_EDGE;
    
        /* TODO
            need to immplement delay
        */
    
        `$INSTANCE_NAME`_CONTROL &= ~ `$INSTANCE_NAME`_CTRL_RISING_EDGE;
    #endif
}

#endif


#if(`$INSTANCE_NAME`_PRS_SIZE <= 32) /* 8-32bit - PRS */
/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Read
********************************************************************************
* Summary:
*  Reads PRS value.
*
* Parameters:
*  void
*
* Return:
*  Returns PRS value.
*
*******************************************************************************/
`$RegSize` `$INSTANCE_NAME`_Read(void)
{
    `$RegSize` seed;
    
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 8)         /* 8bits - PRS */
        /* Read PRS */
        seed = `$INSTANCE_NAME`_SEED_A__A0_REG ;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 16)      /* 16bits - PRS */
        /* Read PRS */
        seed = ((uint16) `$INSTANCE_NAME`_SEED_A__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_A__A0_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 24)      /* 24bits - PRS */
        /* Read PRS */
        seed = ((uint32) (`$INSTANCE_NAME`_SEED_B__A1_REG)) << 16;
        seed |= ((uint32) (`$INSTANCE_NAME`_SEED_B__A0_REG)) << 8;
        seed |= `$INSTANCE_NAME`_SEED_A__A0_REG;       
        
    #else   /* 32bits - PRS */
        /* Read PRS */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_B__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_A__A1_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_A__A0_REG;
    #endif
    
    return seed;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteSeed
********************************************************************************
* Summary:
*  Writes seed value.
*
* Parameters:
*  seed:  Seed value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeed(`$RegSize` seed)
{
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 8)         /* 8bits - PRS */
        /* Write Seed */
        `$INSTANCE_NAME`_SEED_A__A0_REG = seed;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 16)      /* 16bits - PRS */
        /* Write Seed */
        `$INSTANCE_NAME`_SEED_A__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_A__A0_REG = LO8(seed);
        
        /* Reset triger */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 24)      /* 24bits - PRS */
        /* Write Seed */
        `$INSTANCE_NAME`_SEED_B__A1_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_B__A0_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_A__A0_REG = LO8(seed);
        
        /* Reset triger */
        `$INSTANCE_NAME`_CONTROL |= `$INSTANCE_NAME`_CTRL_RESET_DFF;
        `$INSTANCE_NAME`_CONTROL &= ~`$INSTANCE_NAME`_CTRL_RESET_DFF;
        
    #else   /* 32bits - PRS */
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
* Function Name: `$INSTANCE_NAME`_ReadPolynomial
********************************************************************************
* Summary:
*  Reads PRS polynomial value.
*
* Parameters:
*  void
*
* Return:
*  Returns PRS polynomial value.
*
*******************************************************************************/
`$RegSize` `$INSTANCE_NAME`_ReadPolynomial(void)
{
    `$RegSize` polynomial;
    
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 8)         /* 8bits - PRS */
        /* Read polynomial */
        polynomial = `$INSTANCE_NAME`_POLYNOM_A__D0_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 16)      /* 16bits - PRS */
        /* Read polynomial */
        polynomial = ((uint16) `$INSTANCE_NAME`_POLYNOM_A__D1_REG) << 8;
        polynomial |= (`$INSTANCE_NAME`_POLYNOM_A__D0_REG);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 24)      /* 24bits - PRS */
        /* Read polynomial */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D1_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_A__D0_REG;
        
    #else   /* 32bits - PRS */
        /* Read polynomial */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D1_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_A__D1_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_A__D0_REG;        
    #endif
    
    return polynomial;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePolynomial
********************************************************************************
* Summary:
*  Writes PRS polynomial value. 
*
* Parameters:
*  polynomial:  PRS polynomial.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomial(`$RegSize` polynomial)
{
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 8)         /* 8bits - PRS */
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_A__D0_REG = polynomial;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 16)      /* 16bits - PRS */
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_A__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_A__D0_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 24)      /* 24bits - PRS */
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_B__D1_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_A__D0_REG = LO8(polynomial);
        
    #else   /* 32bits - PRS */
        /* Write polynomial */
        `$INSTANCE_NAME`_POLYNOM_B__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_A__D1_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_A__D0_REG = LO8(polynomial);
        
    #endif
}


#else  /* 64bit - PRS */
/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadUpper
********************************************************************************
* Summary:
*  Reads upper half of PRS value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  void
*
* Return:
*  Returns upper half of PRS value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadUpper(void)
{
    uint32 seed;
  
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Read PRS Upper */
        seed = `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Read PRS Upper */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 56)          /* 56bits - PRS */
        /* Read PRS Upper */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG;
        
    #else    /* 64bits - PRS */
        /* Read PRS Upper */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_UPPER_A__A1_REG;
    #endif
    
    return seed;
}


/*******************************************************************************
*  Function Name: `$INSTANCE_NAME`_ReadLower
********************************************************************************
* Summary:
*  Reads lower half of PRS value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  void
*
* Return:
*  Returns lower half of PRS value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadLower(void)
{
    uint32 seed;
    
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Read PRS Lower */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Read PRS Lower */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG;
        
    #else    /* 64bits - PRS */
        /* Read PRS Lower */
        seed = ((uint32) `$INSTANCE_NAME`_SEED_LOWER_D__A0_REG) << 24;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG) << 16;
        seed |= ((uint32) `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG) << 8;
        seed |= `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG;
    #endif
    
    return seed;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WriteSeedUpper
********************************************************************************
* Summary:
*  Writes upper half of seed value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  seed:  Upper half of seed value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeedUpper(uint32 seed)
{
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = LO8(seed);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG = LO8(seed);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 56)          /* 56bits - PRS */
        /* Write Seed Upper */
        `$INSTANCE_NAME`_SEED_UPPER_D__A1_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_UPPER_C__A1_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_UPPER_B__A1_REG = HI8(seed);
        
    #else    /* 64bits - PRS */
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
* Function Name: `$INSTANCE_NAME`_WriteSeedLower
********************************************************************************
* Summary:
*  Writes lower half of seed value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  seed:  Lower half of seed value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WriteSeedLower(uint32 seed)
{
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Write Seed Lower */
        `$INSTANCE_NAME`_SEED_LOWER_B__A1_REG = HI8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG = LO8(seed);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Write Seed Lower */
        `$INSTANCE_NAME`_SEED_LOWER_A__A1_REG = HI8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_C__A0_REG = LO8(HI16(seed));
        `$INSTANCE_NAME`_SEED_LOWER_B__A0_REG = HI8(seed);
        `$INSTANCE_NAME`_SEED_LOWER_A__A0_REG = LO8(seed);
        
    #else    /* 64bits - PRS */
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
* Function Name: `$INSTANCE_NAME`_ReadPolynomialUpper
********************************************************************************
* Summary:
*  Reads upper half of PRS polynomial value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  void
*
* Return:
*  Returns upper half of PRS polynomial value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadPolynomialUpper(void)
{
    uint32 polynomial;
    
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Read Polynomial Upper */
        polynomial = `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Read Polynomial Upper */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 56)          /* 56bits - PRS */
        /* Read Polynomial Upper */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG;
        
    #else    /* 64bits - PRS */
        /* Read Polynomial Upper */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG;
    #endif
    
    return polynomial;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ReadPolynomialLower
********************************************************************************
* Summary:
*  Reads lower half of PRS polynomial value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  void
*
* Return:
*  Returns lower half of PRS polynomial value.
*
*******************************************************************************/
uint32 `$INSTANCE_NAME`_ReadPolynomialLower(void)
{
    uint32 polynomial;
    
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Read Polynomial Lower */
        polynomial = ( (uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG) << 24;
        polynomial |= ( (uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG) << 16;
        polynomial |= ( (uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG;
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Read Polynomial Lower */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG;
        
    #else    /* 64bits - PRS */
        /* Read Polynomial Lower */
        polynomial = ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG) << 24;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG) << 16;
        polynomial |= ((uint32) `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG) << 8;
        polynomial |= `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG;
    #endif
    
    return polynomial;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePolynomialUpper
********************************************************************************
* Summary:
*  Writes upper half of PRS polynomial value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  polynomial:  Upper half PRS polynomial value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomialUpper(uint32 polynomial)
{
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 56)          /* 56bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG = LO8(polynomial);
        
    #else   /* 64bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_UPPER_D__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_UPPER_C__D1_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_UPPER_B__D1_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_UPPER_A__D1_REG = LO8(polynomial);
    #endif
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_WritePolynomialLower
********************************************************************************
* Summary:
*  Writes lower half of PRS polynomial value. Only generated for 33-64-bit PRS.
*
* Parameters:
*  polynomial:  Lower half of PRS polynomial value.
*
* Return:
*  void
*
*******************************************************************************/
void `$INSTANCE_NAME`_WritePolynomialLower(uint32 polynomial)
{
    #if (`$INSTANCE_NAME`_PRS_SIZE <= 40)            /* 40bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);
        
    #elif (`$INSTANCE_NAME`_PRS_SIZE <= 48)          /* 48bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D1_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);
        
    #else    /* 64bits - PRS */
        /* Write Polynomial Lower */
        `$INSTANCE_NAME`_POLYNOM_LOWER_D__D0_REG = HI8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_C__D0_REG = LO8(HI16(polynomial));
        `$INSTANCE_NAME`_POLYNOM_LOWER_B__D0_REG = HI8(polynomial);
        `$INSTANCE_NAME`_POLYNOM_LOWER_A__D0_REG = LO8(polynomial);
    #endif
}

#endif /* `$INSTANCE_NAME`_PRS_SIZE */

/* [] END OF FILE */

