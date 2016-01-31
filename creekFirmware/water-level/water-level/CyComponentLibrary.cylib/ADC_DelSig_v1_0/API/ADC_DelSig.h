/*******************************************************************************
* File Name: `$INSTANCE_NAME`.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*    This file contains the function prototypes and constants used in
*    the Delta-Sigma ADC user module.
*
*   Note:
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include "cytypes.h"
#include "cyfitter.h"
#include "cydevice.h"  /* This required until DSM power register control is moved. */

#if !defined(CY_ADC_DELSIG_`$INSTANCE_NAME`_H) 
#define CY_ADC_DELSIG_`$INSTANCE_NAME`_H 


/***************************************
* Conditional Compilation Parameters
***************************************/

/* PSoC5 ES1 or later */
#define `$INSTANCE_NAME`_PSOC5_ES1  ((CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_PANTHER)    && \
                                    (CYDEV_CHIP_REV_EXPECT >= CYDEV_CHIP_REV_PANTHER_ES1))
/* PSoC3 ES3 or later */
#define `$INSTANCE_NAME`_PSOC3_ES3  ((CYDEV_CHIP_DIE_EXPECT == CYDEV_CHIP_DIE_LEOPARD)    && \
                                    (CYDEV_CHIP_REV_EXPECT > CYDEV_CHIP_REV_LEOPARD_ES2))
                                    
                                    
/***************************************
*        Function Prototypes 
***************************************/

void    `$INSTANCE_NAME`_Start(void);
void    `$INSTANCE_NAME`_Stop(void);
void    `$INSTANCE_NAME`_SetPower(uint8 power);
void    `$INSTANCE_NAME`_SetBufferGain(uint8 gain);
void    `$INSTANCE_NAME`_StartConvert(void);
void    `$INSTANCE_NAME`_StopConvert(void);
uint8   `$INSTANCE_NAME`_IsEndConversion(uint8 retMode);
int8    `$INSTANCE_NAME`_GetResult8(void);
int16   `$INSTANCE_NAME`_GetResult16(void);
int32   `$INSTANCE_NAME`_GetResult32(void);
void    `$INSTANCE_NAME`_InitRegisters(void);
CY_ISR_PROTO( `$INSTANCE_NAME`_ISR );

/* These functions may not be supported in the released version */
void    `$INSTANCE_NAME`_SetBufferChop(uint8 chopen, uint8 chopFreq);  



/**************************************
*           Parameter Defaults        
**************************************/

/* Default config values from user parameters */

#define `$INSTANCE_NAME`_DEFAULT_POWER        `$ADC_Power`           /* Default power setting */
#define `$INSTANCE_NAME`_DEFAULT_RESOLUTION   `$ADC_Resolution`      /* ADC resolution selected with parameters. */
#define `$INSTANCE_NAME`_DEFAULT_RANGE        `$ADC_Input_Range`     /* ADC Input Range selection. */
#define `$INSTANCE_NAME`_DEFAULT_SRATE        `$Sample_Rate`         /* Selected sample rate */
#define `$INSTANCE_NAME`_DEFAULT_REFERENCE    `$ADC_Reference`       /* Default reference */
#define `$INSTANCE_NAME`_DEFAULT_BUF_GAIN     `$Input_Buffer_Gain`   /* Default buffer gain. */
#define `$INSTANCE_NAME`_DEFAULT_STROBE       `$Start_of_Conversion` /* Start of conversion method */
#define `$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK `$ADC_Clock`           /* Default clock selection */
#define `$INSTANCE_NAME`_DEFAULT_CONV_MODE    `$Conversion_Mode`     /* Default conversion method */

/* Priority of the ADC_IRQ interrupt. */
#define `$INSTANCE_NAME`_IRQ_INTC_PRIOR_NUMBER      `$INSTANCE_NAME`_IRQ__INTC_PRIOR_NUM


/*     Reference setting constants        */
#define `$INSTANCE_NAME`_REF_1V024      0x00u 
#define `$INSTANCE_NAME`_REF_P03        0x00u 
#define `$INSTANCE_NAME`_REF_P32        0x00u 
#define `$INSTANCE_NAME`_REF_ETC        0x00u 

/**************************************
*           API Constants        
**************************************/

/* Constants for SetPower(), "power" paramter  */
#define `$INSTANCE_NAME`_LOWPOWER              0x00u 
#define `$INSTANCE_NAME`_MEDPOWER              0x01u 
#define `$INSTANCE_NAME`_HIGHPOWER             0x02u 
#define `$INSTANCE_NAME`_HIGHPOWER_1_5         0x03u  /* Reserved */
#define `$INSTANCE_NAME`_HIGHPOWER_2_0         0x04u  /* Reserved */
#define `$INSTANCE_NAME`_HIGHPOWER_2_5         0x07u  /* Reserved */
#define `$INSTANCE_NAME`_POWER_MASK            0x07u 

/*  Constants for SetRef(), "ref" parameter */
#define `$INSTANCE_NAME`_INT_REF               0x00u 
#define `$INSTANCE_NAME`_INT_REF_BYPASS_P03    0x01u 
#define `$INSTANCE_NAME`_INT_REF_BYPASS_P32    0x02u 
#define `$INSTANCE_NAME`_EXT_REF_P03           0x03u 
#define `$INSTANCE_NAME`_EXT_REF_P32           0x04u 

/*  Constants for SetBufferGain() "gain" paramter  */
#define `$INSTANCE_NAME`_BUF_GAIN_1X           0x00u 
#define `$INSTANCE_NAME`_BUF_GAIN_2X           0x01u 
#define `$INSTANCE_NAME`_BUF_GAIN_4X           0x02u 
#define `$INSTANCE_NAME`_BUF_GAIN_8X           0x03u 
#define `$INSTANCE_NAME`_BUF_GAIN_MASK         0x03u 

/*   Constants for IsEndConversion() "retMode" parameter  */
#define `$INSTANCE_NAME`_RETURN_STATUS         0x01u 
#define `$INSTANCE_NAME`_WAIT_FOR_RESULT       0x00u 


/**************************************
*      Unsupported API Constants        
**************************************/

/*   Constants for SetBuffer() "bufMode" parameter  */   
#define `$INSTANCE_NAME`_BUF_DIFF_MODE         0x00u  /* Differential mode */
#define `$INSTANCE_NAME`_BUF_BYPASS_POS        0x04u  /* Bypass and power down positive channel */
#define `$INSTANCE_NAME`_BUF_BYPASS_NEG        0x08u  /* Bypass and power down negative channel */
#define `$INSTANCE_NAME`_BUF_RAIL_TO_RAIL      0x10u  /* Enables Rail-to-rail mode */
#define `$INSTANCE_NAME`_BUF_FILTER_EN         0x20u  /* Output filter enable */
#define `$INSTANCE_NAME`_BUF_LOW_PWR           0x40u  /* Enable  Low power mode */

/*   Constants for SetInputRange() "inputMode" parameter  */
#define `$INSTANCE_NAME`_INMODE_VSSA_TO_VREF        0x00u 
#define `$INSTANCE_NAME`_INMODE_VSSA_TO_2VREF       0x01u 
#define `$INSTANCE_NAME`_INMODE_VSSA_TO_VDDA        0x02u 
#define `$INSTANCE_NAME`_INMODE_VNEG_PM_VREF_DIFF   0x03u 
#define `$INSTANCE_NAME`_INMODE_VNEG_PM_2VREF_DIFF  0x04u 

/*   Constants for SetBufferChop() "chopen" parameter  */
#define `$INSTANCE_NAME`_BUF_CHOP_ENABLE       0x01u 
#define `$INSTANCE_NAME`_BUF_CHOP_DISABLE      0x00u 

/*   Constants for SetBufferChop() "chopFreq" parameter  */
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS2     0x00u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS4     0x01u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS8     0x02u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS16    0x03u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS32    0x04u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS64    0x05u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS128   0x06u 
#define `$INSTANCE_NAME`_BUF_CHOP_FREQ_FS256   0x07u 



/* Constants for custom reference mode settings */
#define `$INSTANCE_NAME`_REF_INTREF            0x80u 
#define `$INSTANCE_NAME`_REF_REFMODE_MASK      0x07u 
#define `$INSTANCE_NAME`_INTREF_MASK           0x07u 

#define `$INSTANCE_NAME`_MIN_RES           8 
#define `$INSTANCE_NAME`_MAX_RES           20 



/**************************************
*       Register Constants        
**************************************/


/******************************************/
/* DSM.CR0 Control Register 0 definitions */
/******************************************/

/* Bit Field  DSM_NONOV                   */
#define `$INSTANCE_NAME`_DSM_NONOV_MASK    0x0Cu
#define `$INSTANCE_NAME`_DSM_NONOV_LOW     0x00u
#define `$INSTANCE_NAME`_DSM_NONOV_MED     0x04u
#define `$INSTANCE_NAME`_DSM_NONOV_HIGH    0x08u
#define `$INSTANCE_NAME`_DSM_NONOV_VHIGH   0x0Cu

/* Bit Field  DSM_QLEV                   */
#define `$INSTANCE_NAME`_DSM_QLEV_MASK     0x03u
#define `$INSTANCE_NAME`_DSM_QLEV_2        0x00u
#define `$INSTANCE_NAME`_DSM_QLEV_3        0x01u
#define `$INSTANCE_NAME`_DSM_QLEV_9        0x02u
#define `$INSTANCE_NAME`_DSM_QLEV_9x       0x03u


/******************************************/
/* DSM.CR1 Control Register 1 definitions */
/******************************************/
#define `$INSTANCE_NAME`_DSM_ODET_TH_MASK  0x1Fu
#define `$INSTANCE_NAME`_DSM_ODEN          0x20u
#define `$INSTANCE_NAME`_DSM_DPMODE        0x40u

/******************************************/
/* DSM.CR2 Control Register 2 definitions */
/******************************************/
/* Bit Field  DSM_FCHOP                   */
#define `$INSTANCE_NAME`_DSM_FCHOP_MASK     0x07u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV2     0x00u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV4     0x01u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV8     0x02u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV16    0x03u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV32    0x04u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV64    0x05u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV128   0x06u
#define `$INSTANCE_NAME`_DSM_FCHOP_DIV256   0x07u

/* Bit Field  DSM_MOD_CHOP_EN                */
#define `$INSTANCE_NAME`_DSM_MOD_CHOP_EN    0x08u

/* Bit Field  DSM_MX_RESET                   */
#define `$INSTANCE_NAME`_DSM_MX_RESET       0x80u

/* Bit Field  DSM_RESET1_EN                  */
#define `$INSTANCE_NAME`_DSM_RESET1_EN      0x10u

/* Bit Field  DSM_RESET2_EN                  */
#define `$INSTANCE_NAME`_DSM_RESET2_EN      0x20u

/* Bit Field  DSM_RESET3_EN                  */
#define `$INSTANCE_NAME`_DSM_RESET3_EN      0x40u

/******************************************/
/* DSM.CR3 Control Register 3 definitions */
/******************************************/
/* Bit Field  DSM_SELECT_MOD_BIT          */
#define `$INSTANCE_NAME`_DSM_MODBITIN_MASK   0x0Fu
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT0   0x00u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT1   0x01u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT2   0x02u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT3   0x03u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT4   0x04u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT5   0x05u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT6   0x06u
#define `$INSTANCE_NAME`_DSM_MODBITIN_LUT7   0x07u
#define `$INSTANCE_NAME`_DSM_MODBITIN_UDB    0x08u

/* Bit Field  DSM_MODBIT_EN                 */
#define `$INSTANCE_NAME`_DSM_MODBIT_EN      0x10u

/* Bit Field  DSM_MX_DOUT                   */
#define `$INSTANCE_NAME`_DSM_MX_DOUT_8BIT   0x00u
#define `$INSTANCE_NAME`_DSM_MX_DOUT_2SCOMP 0x20u

/* Bit Field  DSM_MODBIT  TBD               */
#define `$INSTANCE_NAME`_DSM_MODBIT         0x40u

/* Bit Field  DSM_SIGN                      */
#define `$INSTANCE_NAME`_DSM_SIGN_NINV      0x00u
#define `$INSTANCE_NAME`_DSM_SIGN_INV       0x80u


/******************************************/
/* DSM.CR4 Control Register 4 definitions */
/******************************************/
/* Bit Field  DSM_SELECT_FCAP_EN            */
#define `$INSTANCE_NAME`_DSM_FCAP1_EN       0x80u
#define `$INSTANCE_NAME`_DSM_FCAP1_DIS      0x00u

/* Bit Field  DSM_SELECT_FCAP1             */
#define `$INSTANCE_NAME`_DSM_FCAP1_MASK     0x7Fu
#define `$INSTANCE_NAME`_DSM_FCAP1_MIN      0x00u
#define `$INSTANCE_NAME`_DSM_FCAP1_MAX      0x7Fu
#define `$INSTANCE_NAME`_DSM_FCAP1_100FF    0x01u
#define `$INSTANCE_NAME`_DSM_FCAP1_200FF    0x02u
#define `$INSTANCE_NAME`_DSM_FCAP1_400FF    0x04u
#define `$INSTANCE_NAME`_DSM_FCAP1_800FF    0x08u
#define `$INSTANCE_NAME`_DSM_FCAP1_1600FF   0x10u
#define `$INSTANCE_NAME`_DSM_FCAP1_3200FF   0x20u
#define `$INSTANCE_NAME`_DSM_FCAP1_6400FF   0x40u

/******************************************/
/* DSM.CR5 Control Register 5 definitions */
/******************************************/
/* Bit Field  DSM_SELECT_IPCAP_EN            */
#define `$INSTANCE_NAME`_DSM_IPCAP1_EN      0x80u
#define `$INSTANCE_NAME`_DSM_IPCAP1_DIS     0x00u

/* Bit Field  DSM_SELECT_IPCAP1             */
#define `$INSTANCE_NAME`_DSM_IPCAP1_MASK    0x7Fu
#define `$INSTANCE_NAME`_DSM_IPCAP1_MIN     0x00u
#define `$INSTANCE_NAME`_DSM_IPCAP1_MAX     0x7Fu
#define `$INSTANCE_NAME`_DSM_IPCAP1_100FF   0x01u
#define `$INSTANCE_NAME`_DSM_IPCAP1_200FF   0x02u
#define `$INSTANCE_NAME`_DSM_IPCAP1_400FF   0x04u
#define `$INSTANCE_NAME`_DSM_IPCAP1_800FF   0x08u
#define `$INSTANCE_NAME`_DSM_IPCAP1_1600FF  0x10u
#define `$INSTANCE_NAME`_DSM_IPCAP1_3200FF  0x20u
#define `$INSTANCE_NAME`_DSM_IPCAP1_6400FF  0x40u


/******************************************/
/* DSM.CR6 Control Register 6 definitions */
/******************************************/
/* Bit Field  DSM_SELECT_DACCAP_EN            */
#define `$INSTANCE_NAME`_DSM_DACCAP_EN      0x40u
#define `$INSTANCE_NAME`_DSM_DACCAP_DIS     0x00u

/* Bit Field  DSM_SELECT_DACCAP             */
#define `$INSTANCE_NAME`_DSM_DACCAP_MASK    0x3Fu
#define `$INSTANCE_NAME`_DSM_DACCAP_MIN     0x00u
#define `$INSTANCE_NAME`_DSM_DACCAP_MAX     0x3Fu
#define `$INSTANCE_NAME`_DSM_DACCAP_96FF    0x01u
#define `$INSTANCE_NAME`_DSM_DACCAP_192FF   0x02u
#define `$INSTANCE_NAME`_DSM_DACCAP_400FF   0x04u
#define `$INSTANCE_NAME`_DSM_DACCAP_800FF   0x08u
#define `$INSTANCE_NAME`_DSM_DACCAP_1600FF  0x10u
#define `$INSTANCE_NAME`_DSM_DACCAP_3200FF  0x20u


/******************************************/
/* DSM.CR7 Control Register 7 definitions */
/******************************************/
/* Bit Field  DSM_SELECT_RESCAP_EN            */
#define `$INSTANCE_NAME`_DSM_RESCAP_EN      0x08u
#define `$INSTANCE_NAME`_DSM_RESCAP_DIS     0x00u

/* Bit Field  DSM_SELECT_RESCAP             */
#define `$INSTANCE_NAME`_DSM_RESCAP_MASK    0x07u
#define `$INSTANCE_NAME`_DSM_RESCAP_MIN     0x00u
#define `$INSTANCE_NAME`_DSM_RESCAP_MAX     0x07u
#define `$INSTANCE_NAME`_DSM_RESCAP_12FF    0x00u
#define `$INSTANCE_NAME`_DSM_RESCAP_24FF    0x01u
#define `$INSTANCE_NAME`_DSM_RESCAP_36FF    0x02u
#define `$INSTANCE_NAME`_DSM_RESCAP_48FF    0x03u
#define `$INSTANCE_NAME`_DSM_RESCAP_60FF    0x04u
#define `$INSTANCE_NAME`_DSM_RESCAP_72FF    0x05u
#define `$INSTANCE_NAME`_DSM_RESCAP_84FF    0x06u
#define `$INSTANCE_NAME`_DSM_RESCAP_96FF    0x07u

#define `$INSTANCE_NAME`_DSM_FCAP2_DIS      0x00u
#define `$INSTANCE_NAME`_DSM_FCAP2_EN       0x80u

#define `$INSTANCE_NAME`_DSM_FCAP3_DIS      0x00u
#define `$INSTANCE_NAME`_DSM_FCAP3_EN       0x40u

#define `$INSTANCE_NAME`_DSM_IPCAP1OFFSET   0x20u
#define `$INSTANCE_NAME`_DSM_FPCAP1OFFSET   0x10u


/******************************************/
/* DSM.CR8 Control Register 8 definitions */
/******************************************/
#define `$INSTANCE_NAME`_DSM_IPCAP2_EN      0x80u

#define `$INSTANCE_NAME`_DSM_IPCAP2_MASK    0x70u
#define `$INSTANCE_NAME`_DSM_IPCAP2_0_FF    0x00u
#define `$INSTANCE_NAME`_DSM_IPCAP2_50_FF   0x10u
#define `$INSTANCE_NAME`_DSM_IPCAP2_100_FF  0x20u
#define `$INSTANCE_NAME`_DSM_IPCAP2_150_FF  0x30u
#define `$INSTANCE_NAME`_DSM_IPCAP2_200_FF  0x40u
#define `$INSTANCE_NAME`_DSM_IPCAP2_250_FF  0x50u
#define `$INSTANCE_NAME`_DSM_IPCAP2_300_FF  0x60u
#define `$INSTANCE_NAME`_DSM_IPCAP2_350_FF  0x70u

#define `$INSTANCE_NAME`_DSM_FCAP2_MASK     0x0Fu
#define `$INSTANCE_NAME`_DSM_FCAP2_0_FF     0x00u
#define `$INSTANCE_NAME`_DSM_FCAP2_50_FF    0x01u
#define `$INSTANCE_NAME`_DSM_FCAP2_100_FF   0x02u
#define `$INSTANCE_NAME`_DSM_FCAP2_150_FF   0x03u
#define `$INSTANCE_NAME`_DSM_FCAP2_200_FF   0x04u
#define `$INSTANCE_NAME`_DSM_FCAP2_250_FF   0x05u
#define `$INSTANCE_NAME`_DSM_FCAP2_300_FF   0x06u
#define `$INSTANCE_NAME`_DSM_FCAP2_350_FF   0x07u
#define `$INSTANCE_NAME`_DSM_FCAP2_400_FF   0x08u
#define `$INSTANCE_NAME`_DSM_FCAP2_450_FF   0x09u
#define `$INSTANCE_NAME`_DSM_FCAP2_500_FF   0x0Au
#define `$INSTANCE_NAME`_DSM_FCAP2_550_FF   0x0Bu
#define `$INSTANCE_NAME`_DSM_FCAP2_600_FF   0x0Cu
#define `$INSTANCE_NAME`_DSM_FCAP2_650_FF   0x0Du
#define `$INSTANCE_NAME`_DSM_FCAP2_700_FF   0x0Eu
#define `$INSTANCE_NAME`_DSM_FCAP2_750_FF   0x0Fu

/******************************************/
/* DSM.CR9 Control Register 9 definitions */
/******************************************/
#define `$INSTANCE_NAME`_DSM_IPCAP3_EN      0x80u

#define `$INSTANCE_NAME`_DSM_IPCAP3_MASK    0x70u
#define `$INSTANCE_NAME`_DSM_IPCAP3_0_FF    0x00u
#define `$INSTANCE_NAME`_DSM_IPCAP3_50_FF   0x10u
#define `$INSTANCE_NAME`_DSM_IPCAP3_100_FF  0x20u
#define `$INSTANCE_NAME`_DSM_IPCAP3_150_FF  0x30u
#define `$INSTANCE_NAME`_DSM_IPCAP3_200_FF  0x40u
#define `$INSTANCE_NAME`_DSM_IPCAP3_250_FF  0x50u
#define `$INSTANCE_NAME`_DSM_IPCAP3_300_FF  0x60u
#define `$INSTANCE_NAME`_DSM_IPCAP3_350_FF  0x70u

#define `$INSTANCE_NAME`_DSM_FCAP3_MASK     0x0Fu
#define `$INSTANCE_NAME`_DSM_FCAP3_0_FF     0x00u
#define `$INSTANCE_NAME`_DSM_FCAP3_50_FF    0x01u
#define `$INSTANCE_NAME`_DSM_FCAP3_100_FF   0x02u
#define `$INSTANCE_NAME`_DSM_FCAP3_150_FF   0x03u
#define `$INSTANCE_NAME`_DSM_FCAP3_200_FF   0x04u
#define `$INSTANCE_NAME`_DSM_FCAP3_250_FF   0x05u
#define `$INSTANCE_NAME`_DSM_FCAP3_300_FF   0x06u
#define `$INSTANCE_NAME`_DSM_FCAP3_350_FF   0x07u
#define `$INSTANCE_NAME`_DSM_FCAP3_400_FF   0x08u
#define `$INSTANCE_NAME`_DSM_FCAP3_450_FF   0x09u
#define `$INSTANCE_NAME`_DSM_FCAP3_500_FF   0x0Au
#define `$INSTANCE_NAME`_DSM_FCAP3_550_FF   0x0Bu
#define `$INSTANCE_NAME`_DSM_FCAP3_600_FF   0x0Cu
#define `$INSTANCE_NAME`_DSM_FCAP3_650_FF   0x0Du
#define `$INSTANCE_NAME`_DSM_FCAP3_700_FF   0x0Eu
#define `$INSTANCE_NAME`_DSM_FCAP3_750_FF   0x0Fu


/********************************************/
/* DSM.CR10 Control Register 10 definitions */
/********************************************/
#define `$INSTANCE_NAME`_DSM_SUMCAP1_EN      0x80u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_EN      0x08u

#define `$INSTANCE_NAME`_DSM_SUMCAP1_MASK    0x70u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_0_FF    0x00u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_50_FF   0x10u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_100_FF  0x20u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_150_FF  0x30u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_200_FF  0x40u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_250_FF  0x50u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_300_FF  0x60u
#define `$INSTANCE_NAME`_DSM_SUMCAP1_350_FF  0x70u

#define `$INSTANCE_NAME`_DSM_SUMCAP2_MASK    0x07u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_0_FF    0x00u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_50_FF   0x01u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_100_FF  0x02u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_150_FF  0x03u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_200_FF  0x04u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_250_FF  0x05u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_300_FF  0x06u
#define `$INSTANCE_NAME`_DSM_SUMCAP2_350_FF  0x07u

/********************************************/
/* DSM.CR11 Control Register 11 definitions */
/********************************************/
#define `$INSTANCE_NAME`_DSM_SUMCAP3_EN       0x08u

#define `$INSTANCE_NAME`_DSM_SUMCAP3_MASK     0x70u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_0_FF     0x00u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_50_FF    0x10u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_100_FF   0x20u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_150_FF   0x30u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_200_FF   0x40u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_250_FF   0x50u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_300_FF   0x60u
#define `$INSTANCE_NAME`_DSM_SUMCAP3_350_FF   0x70u

#define `$INSTANCE_NAME`_DSM_SUMCAPFB_MASK    0x07u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_0_FF    0x00u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_50_FF   0x01u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_100_FF  0x02u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_150_FF  0x03u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_200_FF  0x04u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_250_FF  0x05u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_300_FF  0x06u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_350_FF  0x07u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_400_FF  0x08u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_450_FF  0x09u
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_500_FF  0x0Au
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_550_FF  0x0Bu
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_600_FF  0x0Cu
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_650_FF  0x0Du
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_700_FF  0x0Eu
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_750_FF  0x0Fu

/********************************************/
/* DSM.CR12 Control Register 12 definitions */
/********************************************/
#define `$INSTANCE_NAME`_DSM_SUMCAPFB_EN      0x40u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_EN      0x20u

#define `$INSTANCE_NAME`_DSM_SUMCAPIN_MASK    0x1Fu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_0_FF    0x00u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_50_FF   0x01u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_100_FF  0x02u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_150_FF  0x03u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_200_FF  0x04u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_250_FF  0x05u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_300_FF  0x06u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_350_FF  0x07u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_400_FF  0x08u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_450_FF  0x09u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_500_FF  0x0Au
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_550_FF  0x0Bu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_600_FF  0x0Cu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_650_FF  0x0Du
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_700_FF  0x0Eu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_750_FF  0x0Fu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_800_FF  0x10u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_850_FF  0x11u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_900_FF  0x12u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_950_FF  0x13u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1000_FF 0x14u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1050_FF 0x15u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1100_FF 0x16u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1150_FF 0x17u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1200_FF 0x18u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1250_FF 0x19u
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1300_FF 0x1Au
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1350_FF 0x1Bu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1400_FF 0x1Cu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1450_FF 0x1Du
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1500_FF 0x1Eu
#define `$INSTANCE_NAME`_DSM_SUMCAPIN_1550_FF 0x15u


/********************************************/
/* DSM.CR13 Control Register 13 definitions */
/********************************************/
#define `$INSTANCE_NAME`_DSM_CR13_RSVD        0xFFu

/********************************************/
/* DSM.CR14 Control Register 14 definitions */
/********************************************/

/* Bit Field  DSM_POWER1                    */
#define `$INSTANCE_NAME`_DSM_POWER1_MASK      0x07u

#define `$INSTANCE_NAME`_DSM_POWER1_0         0x00u
#define `$INSTANCE_NAME`_DSM_POWER1_1         0x01u
#define `$INSTANCE_NAME`_DSM_POWER1_2         0x02u
#define `$INSTANCE_NAME`_DSM_POWER1_3         0x03u
#define `$INSTANCE_NAME`_DSM_POWER1_4         0x04u
#define `$INSTANCE_NAME`_DSM_POWER1_5         0x05u
#define `$INSTANCE_NAME`_DSM_POWER1_6         0x06u
#define `$INSTANCE_NAME`_DSM_POWER1_7         0x07u
  
#define `$INSTANCE_NAME`_DSM_POWER1_44UA      0x00u
#define `$INSTANCE_NAME`_DSM_POWER1_123UA     0x01u
#define `$INSTANCE_NAME`_DSM_POWER1_492UA     0x02u
#define `$INSTANCE_NAME`_DSM_POWER1_750UA     0x03u
#define `$INSTANCE_NAME`_DSM_POWER1_1MA       0x04u

/* Bit Field  DSM_OPAMP1_BW                 */
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_MASK   0xF0u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_0      0x00u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_1      0x10u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_2      0x20u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_3      0x30u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_4      0x40u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_5      0x50u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_6      0x60u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_7      0x70u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_8      0x80u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_9      0x90u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_A      0xA0u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_B      0xB0u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_C      0xC0u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_D      0xD0u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_E      0xE0u
#define `$INSTANCE_NAME`_DSM_OPAMP1_BW_F      0xF0u

/********************************************/
/* DSM.CR15 Control Register 15 definitions */
/********************************************/

/* Bit Field  DSM_POWER2_3                  */
#define `$INSTANCE_NAME`_DSM_POWER2_3_MASK   0x07u

#define `$INSTANCE_NAME`_DSM_POWER2_3_LOW    0x00u
#define `$INSTANCE_NAME`_DSM_POWER2_3_MED    0x01u
#define `$INSTANCE_NAME`_DSM_POWER2_3_HIGH   0x02u
#define `$INSTANCE_NAME`_DSM_POWER2_3_IP5X   0x03u
#define `$INSTANCE_NAME`_DSM_POWER2_3_2X     0x04u
#define `$INSTANCE_NAME`_DSM_POWER2_3_HIGH_5 0x05u
#define `$INSTANCE_NAME`_DSM_POWER2_3_HIGH_6 0x06u
#define `$INSTANCE_NAME`_DSM_POWER2_3_HIGH_7 0x07u

/* Bit Field  DSM_POWER_COMP                */
#define `$INSTANCE_NAME`_DSM_POWER_COMP_MASK 0x30u

#define `$INSTANCE_NAME`_DSM_POWER_VERYLOW   0x00u
#define `$INSTANCE_NAME`_DSM_POWER_NORMAL    0x10u
#define `$INSTANCE_NAME`_DSM_POWER_6MHZ      0x20u
#define `$INSTANCE_NAME`_DSM_POWER_12MHZ     0x30u

/********************************************/
/* DSM.CR16 Control Register 16 definitions */
/********************************************/

/* Bit Field  DSM_POWER_SUM                 */
#define `$INSTANCE_NAME`_DSM_POWER_SUM_MASK   0x70u

#define `$INSTANCE_NAME`_DSM_POWER_SUM_LOW    0x00u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_MED    0x10u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_HIGH   0x20u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_IP5X   0x30u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_2X     0x40u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_HIGH_5 0x50u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_HIGH_6 0x60u
#define `$INSTANCE_NAME`_DSM_POWER_SUM_HIGH_7 0x70u

#define `$INSTANCE_NAME`_DSM_CP_ENABLE         0x08u
#define `$INSTANCE_NAME`_DSM_CP_PWRCTL_MASK    0x07u
#define `$INSTANCE_NAME`_DSM_CP_PWRCTL_DEFAULT 0x0Au

/********************************************/
/* DSM.CR17 Control Register 17 definitions */
/********************************************/

/* Bit Field  DSM_EN_BUF                    */
#define `$INSTANCE_NAME`_DSM_EN_BUF_VREF        0x01u
#define `$INSTANCE_NAME`_DSM_EN_BUF_VCM         0x02u

#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_MASK 0x0Cu
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_0    0x00u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_1    0x04u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_2    0x08u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_3    0x0Cu

#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VCM_MASK  0x30u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VCM_0     0x00u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VCM_1     0x10u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VCM_2     0x20u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VCM_3     0x30u

#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_INN_MASK  0xC0u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_INN_0     0x00u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_INN_1     0x40u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_INN_2     0x80u
#define `$INSTANCE_NAME`_DSM_PWR_CTRL_VREF_INN_3     0xC0u

/*********************************************/
/* DSM.REF0 Reference Register 0 definitions */
/*********************************************/

#define `$INSTANCE_NAME`_DSM_REFMUX_MASK     0x07u
#define `$INSTANCE_NAME`_DSM_REFMUX_NONE     0x00u
#define `$INSTANCE_NAME`_DSM_REFMUX_UVCM     0x01u
#define `$INSTANCE_NAME`_DSM_REFMUX_VDA_4    0x02u
#define `$INSTANCE_NAME`_DSM_REFMUX_VDAC0    0x03u
#define `$INSTANCE_NAME`_DSM_REFMUX_1_024V   0x04u
#define `$INSTANCE_NAME`_DSM_REFMUX_1_20V    0x05u

#define `$INSTANCE_NAME`_DSM_EN_BUF_VREF_INN 0x08u
#define `$INSTANCE_NAME`_DSM_VREF_RES_DIV_EN 0x10u
#define `$INSTANCE_NAME`_DSM_VCM_RES_DIV_EN  0x20u
#define `$INSTANCE_NAME`_DSM_VCMSEL_MASK     0xC0u
#define `$INSTANCE_NAME`_DSM_VCMSEL_NOSEL    0x00u
#define `$INSTANCE_NAME`_DSM_VCMSEL_0_8V     0x40u
#define `$INSTANCE_NAME`_DSM_VCMSEL_0_7V     0x80u
#define `$INSTANCE_NAME`_DSM_VCMSEL_VPWRA_2  0xC0u

/*********************************************/
/* DSM.REF1 Reference Register 1 definitions */
/*********************************************/
#define `$INSTANCE_NAME`_DSM_REF1_MASK     0xFFu

/*********************************************/
/* DSM.REF2 Reference Register 2 definitions */
/*********************************************/
#define `$INSTANCE_NAME`_DSM_REF2_MASK     0xFFu
#define `$INSTANCE_NAME`_DSM_REF2_S0_EN    0x01u
#define `$INSTANCE_NAME`_DSM_REF2_S1_EN    0x02u
#define `$INSTANCE_NAME`_DSM_REF2_S2_EN    0x04u
#define `$INSTANCE_NAME`_DSM_REF2_S3_EN    0x08u
#define `$INSTANCE_NAME`_DSM_REF2_S4_EN    0x10u
#define `$INSTANCE_NAME`_DSM_REF2_S5_EN    0x20u
#define `$INSTANCE_NAME`_DSM_REF2_S6_EN    0x40u
#define `$INSTANCE_NAME`_DSM_REF2_S7_EN    0x80u

/*********************************************/
/* DSM.REF3 Reference Register 3 definitions */
/*********************************************/
#define `$INSTANCE_NAME`_DSM_REF3_MASK     0xFFu
#define `$INSTANCE_NAME`_DSM_REF2_S8_EN    0x01u
#define `$INSTANCE_NAME`_DSM_REF2_S9_EN    0x02u
#define `$INSTANCE_NAME`_DSM_REF2_S10_EN   0x04u
#define `$INSTANCE_NAME`_DSM_REF2_S11_EN   0x08u
#define `$INSTANCE_NAME`_DSM_REF2_S12_EN   0x10u
#define `$INSTANCE_NAME`_DSM_REF2_S13_EN   0x20u


/************************************************/
/* DSM.DEM0 Dynamic Element Matching Register 0 */
/************************************************/
#define `$INSTANCE_NAME`_DSM_EN_SCRAMBLER0 0x01u
#define `$INSTANCE_NAME`_DSM_EN_SCRAMBLER1 0x02u
#define `$INSTANCE_NAME`_DSM_EN_DEM        0x04u
#define `$INSTANCE_NAME`_DSM_ADC_TEST_EN   0x08u
#define `$INSTANCE_NAME`_DSM_DEMTEST_SRC   0x10u

/************************************************/
/* DSM.DEM1 Dynamic Element Matching Register 1 */
/************************************************/
#define `$INSTANCE_NAME`_DSM_DEM1_MASK     0xFF


/***********************************************/
/* DSM.BUF0 Buffer Register 0                  */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_ENABLE_P      0x01u
#define `$INSTANCE_NAME`_DSM_BYPASS_P      0x02u
#define `$INSTANCE_NAME`_DSM_RAIL_RAIL_EN  0x04u

#define `$INSTANCE_NAME`_DSM_BUFSEL        0x08u
#define `$INSTANCE_NAME`_DSM_BUFSEL_A      0x00u
#define `$INSTANCE_NAME`_DSM_BUFSEL_B      0x08u


/***********************************************/
/* DSM.BUF1 Buffer Register 1                  */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_ENABLE_N      0x01u
#define `$INSTANCE_NAME`_DSM_BYPASS_N      0x02u
#define `$INSTANCE_NAME`_DSM_GAIN_MASK     0x0Cu

/***********************************************/
/* DSM.BUF2 Buffer Register 2                  */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_LOWPOWER_EN   0x01u
#define `$INSTANCE_NAME`_DSM_ADD_EXTRA_RC  0x02u

/***********************************************/
/* DSM.BUF3 Buffer Register 3                  */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_BUF_CHOP_EN     0x08u

#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_MASK  0x07u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS2   0x00u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS4   0x01u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS8   0x02u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS16  0x03u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS32  0x04u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS64  0x05u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS128 0x06u
#define `$INSTANCE_NAME`_DSM_BUF_FCHOP_FS256 0x07u

/***********************************************/
/* DSM.MISC Delta Sigma Misc Register          */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_SEL_ICLK_CP   0x01u

/************************************************/
/* DSM.CLK Delta Sigma Clock selection Register */
/************************************************/
#define `$INSTANCE_NAME`_DSM_CLK_MX_CLK_MSK  0x07u
#define `$INSTANCE_NAME`_DSM_CLK_CLK_EN      0x08u
#define `$INSTANCE_NAME`_DSM_CLK_BYPASS_SYNC 0x10u


/***********************************************/
/* DSM.OUT0 Output Register 0                  */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_DOUT0           0xFFu


/***********************************************/
/* DSM.OUT1 Output Register 1                  */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_DOUT2SCOMP_MASK 0x0Fu
#define `$INSTANCE_NAME`_DSM_OVDFLAG         0x10u
#define `$INSTANCE_NAME`_DSM_OVDCAUSE        0x20u


/***********************************************/
/*          Decimator                          */
/***********************************************/


/***********************************************/
/* DEC.CR Control Register 0                   */
/***********************************************/
#define `$INSTANCE_NAME`_DEC_SAT_EN          0x80u /* Enable post process offset correction */
#define `$INSTANCE_NAME`_DEC_FIR_EN          0x40u /* Post process FIR enable  */
#define `$INSTANCE_NAME`_DEC_OCOR_EN         0x20u /* Offest correction enable */
#define `$INSTANCE_NAME`_DEC_GCOR_EN         0x10u /* Enable gain correction feature */

#define `$INSTANCE_NAME`_MODE_MASK           0x0Cu /* Conversion Mode */
#define `$INSTANCE_NAME`_MODE_SINGLE_SAMPLE  0x00u
#define `$INSTANCE_NAME`_MODE_FAST_FILTER    0x04u
#define `$INSTANCE_NAME`_MODE_CONTINUOUS     0x08u
#define `$INSTANCE_NAME`_MODE_FAST_FIR       0x0Cu

#define `$INSTANCE_NAME`_DEC_XSTART_EN       0x02u /* Enables external start signal */
#define `$INSTANCE_NAME`_DEC_START_CONV      0x01u /* A write to this bit starts a conversion */


/***********************************************/
/* DEC.SR Status and Control Register 0        */
/***********************************************/
#define `$INSTANCE_NAME`_DEC_CORECLK_DISABLE 0x20u /* Controls gating of the Core clock */
#define `$INSTANCE_NAME`_DEC_INTR_PULSE      0x10u /* Controls interrupt mode */ 
#define `$INSTANCE_NAME`_DEC_OUT_ALIGN       0x08u /* Controls 8-bit shift of SAMP registers */
#define `$INSTANCE_NAME`_DEC_INTR_CLEAR      0x04u /* A write to this bit clears bit0 */
#define `$INSTANCE_NAME`_DEC_INTR_MASK       0x02u /* Controls the generation of the conversion comp. INTR */
#define `$INSTANCE_NAME`_DEC_CONV_DONE       0x01u /* Bit set if conversion has completed  */

/***********************************************/
/* DEC.SHIFT1 Decimator Input Shift Register 1 */
/***********************************************/
#define `$INSTANCE_NAME`_DEC_SHIFT1_MASK     0x1Fu /* Input shift1 control */

/***********************************************/
/* DEC.SHIFT2 Decimator Input Shift Register 2 */
/***********************************************/
#define `$INSTANCE_NAME`_DEC_SHIFT2_MASK     0x0Fu /* Input shift2 control */

/****************************************************************/
/* DEC.DR2 Decimator Decimation Rate of FIR Filter Low Register */
/****************************************************************/
#define `$INSTANCE_NAME`_DEC_DR2_MASK        0xFFu 

/******************************************************************/
/* DEC.DR2H Decimator Decimation Rate of FIR Filter High Register */
/******************************************************************/
#define `$INSTANCE_NAME`_DEC_DR2H_MASK       0x03u 

/***********************************************/
/* DEC.COHR Decimator Coherency Register       */
/***********************************************/
#define `$INSTANCE_NAME`_DEC_GCOR_KEY_MASK   0x30u 
#define `$INSTANCE_NAME`_DEC_GCOR_KEY_OFF    0x00u 
#define `$INSTANCE_NAME`_DEC_GCOR_KEY_LOW    0x10u 
#define `$INSTANCE_NAME`_DEC_GCOR_KEY_MID    0x20u 
#define `$INSTANCE_NAME`_DEC_GCOR_KEY_HIGH   0x30u 

#define `$INSTANCE_NAME`_DEC_OCOR_KEY_MASK   0x0Cu
#define `$INSTANCE_NAME`_DEC_OCOR_KEY_OFF    0x00u 
#define `$INSTANCE_NAME`_DEC_OCOR_KEY_LOW    0x04u 
#define `$INSTANCE_NAME`_DEC_OCOR_KEY_MID    0x08u 
#define `$INSTANCE_NAME`_DEC_OCOR_KEY_HIGH   0x0Cu 

#define `$INSTANCE_NAME`_DEC_SAMP_KEY_MASK   0x03u 
#define `$INSTANCE_NAME`_DEC_SAMP_KEY_OFF    0x00u 
#define `$INSTANCE_NAME`_DEC_SAMP_KEY_LOW    0x01u 
#define `$INSTANCE_NAME`_DEC_SAMP_KEY_MID    0x02u 
#define `$INSTANCE_NAME`_DEC_SAMP_KEY_HIGH   0x03u 

/* PM_ACT_CFG (Active Power Mode CFG Register)     */ 
#define `$INSTANCE_NAME`_ACT_PWR_DEC_EN   `$INSTANCE_NAME`_DEC__PM_ACT_MSK /* Power enable mask */
#define `$INSTANCE_NAME`_ACT_PWR_DSM_EN   0x01u /* Power enable mask */

#if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
#define `$INSTANCE_NAME`_ACT_PWR_CLK_EN   `$INSTANCE_NAME`_theACLK__PM_ACT_MSK /* Power enable mask */
#endif

/***********************************************/
/* DSM.SW3 DSM Analog Routing Register 3       */
/***********************************************/
#define `$INSTANCE_NAME`_DSM_VP_VSSA       0x04u 
#define `$INSTANCE_NAME`_DSM_VN_AMX        0x10u 
#define `$INSTANCE_NAME`_DSM_VN_VREF       0x20u 
#define `$INSTANCE_NAME`_DSM_VN_VSSA       0x40u 
#define `$INSTANCE_NAME`_DSM_VN_REF_MASK   0x60u 


/***********************************************/
/* ANIF.PUMP.CR1 Pump Configuration Register 1 */
/***********************************************/
#define `$INSTANCE_NAME`_PUMP_CR1  (* (reg8 *) CYDEV_ANAIF_CFG_PUMP_CR1)

#define `$INSTANCE_NAME`_PUMP_CR1_CLKSEL  0x04
#define `$INSTANCE_NAME`_PUMP_CR1_FORCE   0x02
#define `$INSTANCE_NAME`_PUMP_CR1_AUTO    0x01


/***************************************
*              Registers        
***************************************/


/* Decimator (DEC) Registers */
#define `$INSTANCE_NAME`_DEC_CR     (* (reg8 *) `$INSTANCE_NAME`_DEC__CR )
#define `$INSTANCE_NAME`_DEC_SR     (* (reg8 *) `$INSTANCE_NAME`_DEC__SR )
#define `$INSTANCE_NAME`_DEC_SHIFT1 (* (reg8 *) `$INSTANCE_NAME`_DEC__SHIFT1 )
#define `$INSTANCE_NAME`_DEC_SHIFT2 (* (reg8 *) `$INSTANCE_NAME`_DEC__SHIFT2 )

#define `$INSTANCE_NAME`_DEC_DR2    (* (reg8 *) `$INSTANCE_NAME`_DEC__DR2 )
#define `$INSTANCE_NAME`_DEC_DR2H   (* (reg8 *) `$INSTANCE_NAME`_DEC__DR2H )
#define `$INSTANCE_NAME`_DEC_DR1    (* (reg8 *) `$INSTANCE_NAME`_DEC__DR1 )
#define `$INSTANCE_NAME`_DEC_OCOR   (* (reg8 *) `$INSTANCE_NAME`_DEC__OCOR )
#define `$INSTANCE_NAME`_DEC_OCORM  (* (reg8 *) `$INSTANCE_NAME`_DEC__OCORM )
#define `$INSTANCE_NAME`_DEC_OCORH  (* (reg8 *) `$INSTANCE_NAME`_DEC__OCORH )
#define `$INSTANCE_NAME`_DEC_GVAL   (* (reg8 *) `$INSTANCE_NAME`_DEC__GVAL )
#define `$INSTANCE_NAME`_DEC_GCOR   (* (reg8 *) `$INSTANCE_NAME`_DEC__GCOR )
#define `$INSTANCE_NAME`_DEC_GCORH  (* (reg8 *) `$INSTANCE_NAME`_DEC__GCORH )
#define `$INSTANCE_NAME`_DEC_SAMP   (* (reg8 *) `$INSTANCE_NAME`_DEC__OUTSAMP )
#define `$INSTANCE_NAME`_DEC_SAMPM  (* (reg8 *) `$INSTANCE_NAME`_DEC__OUTSAMPM )
#define `$INSTANCE_NAME`_DEC_SAMPH  (* (reg8 *) `$INSTANCE_NAME`_DEC__OUTSAMPH )
#define `$INSTANCE_NAME`_DEC_COHER  (* (reg8 *) `$INSTANCE_NAME`_DEC__COHER )
#define `$INSTANCE_NAME`_PWRMGR_DEC (* (reg8 *) `$INSTANCE_NAME`_DEC__PM_ACT_CFG )     /* Decimator Power manager Reg */

/* Delta Sigma Modulator (DSM) Registers */  
#define `$INSTANCE_NAME`_DSM_CR0    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR0 )
#define `$INSTANCE_NAME`_DSM_CR1    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR1 )
#define `$INSTANCE_NAME`_DSM_CR2    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR2 )
#define `$INSTANCE_NAME`_DSM_CR3    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR3 )
#define `$INSTANCE_NAME`_DSM_CR4    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR4 )
#define `$INSTANCE_NAME`_DSM_CR5    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR5 )
#define `$INSTANCE_NAME`_DSM_CR6    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR6 )
#define `$INSTANCE_NAME`_DSM_CR7    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR7 )
#define `$INSTANCE_NAME`_DSM_CR8    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR8 )
#define `$INSTANCE_NAME`_DSM_CR9    (* (reg8 *) `$INSTANCE_NAME`_DSM__CR9 )
#define `$INSTANCE_NAME`_DSM_CR10   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR10 )
#define `$INSTANCE_NAME`_DSM_CR11   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR11 )
#define `$INSTANCE_NAME`_DSM_CR12   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR12 )
#define `$INSTANCE_NAME`_DSM_CR13   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR13 )
#define `$INSTANCE_NAME`_DSM_CR14   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR14 )
#define `$INSTANCE_NAME`_DSM_CR15   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR15 )
#define `$INSTANCE_NAME`_DSM_CR16   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR16 )
#define `$INSTANCE_NAME`_DSM_CR17   (* (reg8 *) `$INSTANCE_NAME`_DSM__CR17 )
#define `$INSTANCE_NAME`_DSM_REF0   (* (reg8 *) `$INSTANCE_NAME`_DSM__REF0 )
#define `$INSTANCE_NAME`_DSM_REF1   (* (reg8 *) `$INSTANCE_NAME`_DSM__REF1 )
#define `$INSTANCE_NAME`_DSM_REF2   (* (reg8 *) `$INSTANCE_NAME`_DSM__REF2 )
#define `$INSTANCE_NAME`_DSM_REF3   (* (reg8 *) `$INSTANCE_NAME`_DSM__REF3 )
#define `$INSTANCE_NAME`_DSM_DEM0   (* (reg8 *) `$INSTANCE_NAME`_DSM__DEM0 )
#define `$INSTANCE_NAME`_DSM_DEM1   (* (reg8 *) `$INSTANCE_NAME`_DSM__DEM1 )
#define `$INSTANCE_NAME`_DSM_MISC   (* (reg8 *) `$INSTANCE_NAME`_DSM__MISC )
#define `$INSTANCE_NAME`_DSM_CLK    (* (reg8 *) `$INSTANCE_NAME`_DSM__CLK )
#define `$INSTANCE_NAME`_DSM_TST0   (* (reg8 *) `$INSTANCE_NAME`_DSM__TST0 )
#define `$INSTANCE_NAME`_DSM_TST1   (* (reg8 *) `$INSTANCE_NAME`_DSM__TST1 )
#define `$INSTANCE_NAME`_DSM_BUF0   (* (reg8 *) `$INSTANCE_NAME`_DSM__BUF0 )
#define `$INSTANCE_NAME`_DSM_BUF1   (* (reg8 *) `$INSTANCE_NAME`_DSM__BUF1 )
#define `$INSTANCE_NAME`_DSM_BUF2   (* (reg8 *) `$INSTANCE_NAME`_DSM__BUF2 )
#define `$INSTANCE_NAME`_DSM_BUF3   (* (reg8 *) `$INSTANCE_NAME`_DSM__BUF3 )
#define `$INSTANCE_NAME`_DSM_OUT0   (* (reg8 *) `$INSTANCE_NAME`_DSM__OUT0 )
#define `$INSTANCE_NAME`_DSM_OUT1   (* (reg8 *) `$INSTANCE_NAME`_DSM__OUT1 )
#define `$INSTANCE_NAME`_DSM_SW0    (* (reg8 *) `$INSTANCE_NAME`_DSM__SW0 )
#define `$INSTANCE_NAME`_DSM_SW2    (* (reg8 *) `$INSTANCE_NAME`_DSM__SW2 )
#define `$INSTANCE_NAME`_DSM_SW3    (* (reg8 *) `$INSTANCE_NAME`_DSM__SW3 )
#define `$INSTANCE_NAME`_DSM_SW4    (* (reg8 *) `$INSTANCE_NAME`_DSM__SW4 )
#define `$INSTANCE_NAME`_DSM_SW6    (* (reg8 *) `$INSTANCE_NAME`_DSM__SW6 )


//:TODO This definition will be changed when the register location moves
#define `$INSTANCE_NAME`_PWRMGR_DSM (* (reg8 *) CYDEV_PM_AVAIL_SR4 )  /* Modulator Power manager */



/* This is only valid if there is an internal clock */
#if(`$INSTANCE_NAME`_DEFAULT_INTERNAL_CLK)
#define `$INSTANCE_NAME`_PWRMGR_CLK (* (reg8 *) `$INSTANCE_NAME`_theACLK__PM_ACT_CFG )  /* Clock Power manager Reg */
#endif



/* Default register settings for the following configuration */
/* Sample Rate: `$Sample_Rate` Samples per Second */
/* Conversion Mode: `$Conversion_Mode` */
/* Input Buffer Gain: `$Input_Buffer_Gain`   */
/* ADC Reference: `$ADC_Reference` */
/* ADC Input Range: `$ADC_Input_Range`  */
/* ADC Resolution: `$ADC_Resolution` bits */
/* ADC Clock: `$DFLT_CLK_FREQ` Hz */

#define `$INSTANCE_NAME`_DFLT_DEC_CR       `$DFLT_DEC_CR`
#define `$INSTANCE_NAME`_DFLT_DEC_SR       `$DFLT_DEC_SR`
#define `$INSTANCE_NAME`_DFLT_DEC_SHIFT1   `$DFLT_DEC_SHIFT1`
#define `$INSTANCE_NAME`_DFLT_DEC_SHIFT2   `$DFLT_DEC_SHIFT2`
#define `$INSTANCE_NAME`_DFLT_DEC_DR2      `$DFLT_DEC_DR2`
#define `$INSTANCE_NAME`_DFLT_DEC_DR2H     `$DFLT_DEC_DR2H`
#define `$INSTANCE_NAME`_DFLT_DEC_DR1      `$DFLT_DEC_DR1`
#define `$INSTANCE_NAME`_DFLT_DEC_OCOR     `$DFLT_DEC_OCOR`
#define `$INSTANCE_NAME`_DFLT_DEC_OCORM    `$DFLT_DEC_OCORM`
#define `$INSTANCE_NAME`_DFLT_DEC_OCORH    `$DFLT_DEC_OCORH`
#define `$INSTANCE_NAME`_DFLT_DEC_GVAL     `$DFLT_DEC_GVAL`
#define `$INSTANCE_NAME`_DFLT_DEC_GCOR     `$DFLT_DEC_GCOR`
#define `$INSTANCE_NAME`_DFLT_DEC_GCORH    `$DFLT_DEC_GCORH`
#define `$INSTANCE_NAME`_DFLT_DEC_COHER    `$DFLT_DEC_COHER`

#define `$INSTANCE_NAME`_DFLT_DSM_CR0      `$DFLT_DSM_CR0`
#define `$INSTANCE_NAME`_DFLT_DSM_CR1      `$DFLT_DSM_CR1`
#define `$INSTANCE_NAME`_DFLT_DSM_CR2      `$DFLT_DSM_CR2`
#define `$INSTANCE_NAME`_DFLT_DSM_CR3      `$DFLT_DSM_CR3`
#define `$INSTANCE_NAME`_DFLT_DSM_CR4      `$DFLT_DSM_CR4`
#define `$INSTANCE_NAME`_DFLT_DSM_CR5      `$DFLT_DSM_CR5`
#define `$INSTANCE_NAME`_DFLT_DSM_CR6      `$DFLT_DSM_CR6`
#define `$INSTANCE_NAME`_DFLT_DSM_CR7      `$DFLT_DSM_CR7`
#define `$INSTANCE_NAME`_DFLT_DSM_CR8      `$DFLT_DSM_CR8`
#define `$INSTANCE_NAME`_DFLT_DSM_CR9      `$DFLT_DSM_CR9`
#define `$INSTANCE_NAME`_DFLT_DSM_CR10     `$DFLT_DSM_CR10`
#define `$INSTANCE_NAME`_DFLT_DSM_CR11     `$DFLT_DSM_CR11`
#define `$INSTANCE_NAME`_DFLT_DSM_CR12     `$DFLT_DSM_CR12`
#define `$INSTANCE_NAME`_DFLT_DSM_CR13     `$DFLT_DSM_CR13`
#define `$INSTANCE_NAME`_DFLT_DSM_CR14     `$DFLT_DSM_CR14`
#define `$INSTANCE_NAME`_DFLT_DSM_CR15     `$DFLT_DSM_CR15`
#define `$INSTANCE_NAME`_DFLT_DSM_CR16     `$DFLT_DSM_CR16`
#define `$INSTANCE_NAME`_DFLT_DSM_CR17     `$DFLT_DSM_CR17`
#define `$INSTANCE_NAME`_DFLT_DSM_REF0     `$DFLT_DSM_REF0`
#define `$INSTANCE_NAME`_DFLT_DSM_REF1     `$DFLT_DSM_REF1`
#define `$INSTANCE_NAME`_DFLT_DSM_REF2     `$DFLT_DSM_REF2`
#define `$INSTANCE_NAME`_DFLT_DSM_REF3     `$DFLT_DSM_REF3`

#define `$INSTANCE_NAME`_DFLT_DSM_DEM0     `$DFLT_DSM_DEM0`
#define `$INSTANCE_NAME`_DFLT_DSM_DEM1     `$DFLT_DSM_DEM1`
#define `$INSTANCE_NAME`_DFLT_DSM_MISC     `$DFLT_DSM_MISC`
#define `$INSTANCE_NAME`_DFLT_DSM_CLK      `$DFLT_DSM_CLK`

#define `$INSTANCE_NAME`_DFLT_DSM_BUF0     `$DFLT_DSM_BUF0`
#define `$INSTANCE_NAME`_DFLT_DSM_BUF1     `$DFLT_DSM_BUF1`
#define `$INSTANCE_NAME`_DFLT_DSM_BUF2     `$DFLT_DSM_BUF2`
#define `$INSTANCE_NAME`_DFLT_DSM_BUF3     `$DFLT_DSM_BUF3`
#define `$INSTANCE_NAME`_DFLT_DSM_OUT0     `$DFLT_DSM_OUT0`
#define `$INSTANCE_NAME`_DFLT_DSM_OUT1     `$DFLT_DSM_OUT1`

#define `$INSTANCE_NAME`_DFLT_DSM_SW3      `$DFLT_DSM_SW3`
#define `$INSTANCE_NAME`_CLOCKS_PER_SAMPLE `$DFLT_CLOCKS_PER_SAMPLE`
#define `$INSTANCE_NAME`_DFLT_CLOCK_FREQ   `$DFLT_CLK_FREQ`


#endif /* CY_ADC_DELSIG_`$INSTANCE_NAME`_H */
/* [] END OF FILE */


