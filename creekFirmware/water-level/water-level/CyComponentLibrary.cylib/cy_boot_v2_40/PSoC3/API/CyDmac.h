/******************************************************************************
* File Name: CyDmac.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides the function definitions for the DMA Controller.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#if !defined(__CYDMAC_H__)
#define __CYDMAC_H__



#include <CYTYPES.H>
#include <CYFITTER.H>
#include <CYDEVICE_TRM.H>
#include <CYLIB.H>


/* Invalid Channel ID. */
#define DMA_INVALID_CHANNEL             0xFFu

/* Invalid TD. */
#define DMA_INVALID_TD                  0xFFu

/* End of chain TD. */
#define DMA_END_CHAIN_TD                0xFFu

#if CY_PSOC3_ES3
/* Disable TD. */
#define DMA_DISABLE_TD                  0xFEu
#endif

typedef struct _dmac_ch
{
    volatile uint8 basic_cfg[4];
    volatile uint8 action[4];
    volatile uint8 basic_status[4];
    volatile uint8 reserved[4];

} dmac_ch;

typedef struct _dmac_cfgmem
{
    volatile uint8 CFG0[4];
    volatile uint8 CFG1[4];

} dmac_cfgmem;

typedef struct _dmac_tdmem
{
    volatile uint8  TD0[4];
    volatile uint8  TD1[4];

} dmac_tdmem;

typedef struct _dmac_tdmem2
{
    volatile uint16 xfercnt;
    volatile uint8 next_td_ptr;
    volatile uint8 flags;
    volatile uint16 src_adr;
    volatile uint16 dst_adr;
} dmac_tdmem2;


#define DMAC_TD_SIZE                    0x08u
#define TD_SWAP_EN                      0x80u
#define TD_SWAP_SIZE4                   0x40u
#define TD_AUTO_EXEC_NEXT               0x20u
#define TD_TERMIN_EN                    0x10u
#define TD_TERMOUT1_EN                  0x08u
#define TD_TERMOUT0_EN                  0x04u
#define TD_INC_DST_ADR                  0x02u
#define TD_INC_SRC_ADR                  0x01u

#define DMAC_CFG                        ((reg32 *) CYREG_PHUB_CFG)
#define DMAC_ERR                        ((reg32 *) CYREG_PHUB_ERR)  
#define DMAC_ERR_ADR                    ((reg32 *) CYREG_PHUB_ERR_ADR)


#if (defined(__C51__))


#define DMAC_CH                         ((dmac_ch xdata *) CYDEV_PHUB_CH0_BASE)
#define DMAC_CFGMEM                     ((dmac_cfgmem xdata *) CYDEV_PHUB_CFGMEM0_BASE)  
#define DMAC_TDMEM                      ((dmac_tdmem xdata *) CYDEV_PHUB_TDMEM0_BASE)

#else

#define DMAC_CH                         ((dmac_ch *) CYDEV_PHUB_CH0_BASE)
#define DMAC_CFGMEM                     ((dmac_cfgmem *) CYDEV_PHUB_CFGMEM0_BASE)  
#define DMAC_TDMEM                      ((dmac_tdmem *) CYDEV_PHUB_TDMEM0_BASE)

#endif


/* TODO: Belong in device.h or sommething else TBD. */
#define NUMBEROF_TDS                    128 /* 32, 64, 128, 256 */
#define NUMBEROF_CHANNELS               CYDEV_DMA_CHANNELS_AVAILABLE /* 64 */


/* Action register bits. */
#define CPU_REQ                         (1 << 0)
#define CPU_TERM_TD                     (1 << 1)
#define CPU_TERM_CHAIN                  (1 << 2)

/* Basic Status register bits. */
#define STATUS_CHAIN_ACTIVE             (1 << 0)
#define STATUS_TD_ACTIVE                (1 << 1)

/* DMA controller register error bits. */
#define DMAC_BUS_TIMEOUT                (1 << 1)
#define DMAC_UNPOP_ACC                  (1 << 2)
#define DMAC_PERIPH_ERR                 (1 << 3)

/* Round robin bits. */
#define ROUND_ROBIN_ENABLE              (1 << 4)

/* DMA Controller functions. */
void CyDmacConfigure(void);
uint8 CyDmacError(void) `=ReentrantKeil("CyDmacError")`;
void CyDmacClearError(uint8 Error) `=ReentrantKeil("CyDmacClearError")`;
uint32 CyDmacErrorAddress(void) `=ReentrantKeil("CyDmacErrorAddress")`;

/* Channel specific functions. */
uint8 CyDmaChAlloc(void) `=ReentrantKeil("CyDmaChAlloc")`;
cystatus CyDmaChFree(uint8 chHandle) `=ReentrantKeil("CyDmaChFree")`;
cystatus CyDmaChEnable(uint8 chHandle, uint8 preserveTds) `=ReentrantKeil("CyDmaChEnable")`;
cystatus CyDmaChDisable(uint8 chHandle) `=ReentrantKeil("CyDmaChDisable")`;
cystatus CyDmaClearPendingDrq(uint8 chHandle) `=ReentrantKeil("CyDmaClearPendingDrq")`;
cystatus CyDmaChPriority(uint8 chHandle, uint8 priority) `=ReentrantKeil("CyDmaChPriority")`;
cystatus CyDmaChSetExtendedAddress(uint8 chHandle, uint16 source, uint16 destination) `=ReentrantKeil("CyDmaChSetExtendedAddress")`;
cystatus CyDmaChSetInitialTd(uint8 chHandle, uint8 startTd) `=ReentrantKeil("CyDmaChSetInitialTd")`;
cystatus CyDmaChSetRequest(uint8 chHandle, uint8 request) `=ReentrantKeil("CyDmaChSetRequest")`;
cystatus CyDmaChGetRequest(uint8 chHandle) `=ReentrantKeil("CyDmaChGetRequest")`;
cystatus CyDmaChStatus(uint8 chHandle, uint8 * currentTd, uint8 * state) `=ReentrantKeil("CyDmaChStatus")`;
cystatus CyDmaChSetConfiguration(uint8 chHandle, uint8 burstCount, uint8 requestPerBurst, uint8 tdDone0, uint8 tdDone1, uint8 tdStop) `=ReentrantKeil("CyDmaChSetConfiguration")`;
cystatus CyDmaChRoundRobin(uint8 chHandle, uint8 enableRR) `=ReentrantKeil("CyDmaChRoundRobin")`;

/* Transfer Descriptor functions. */
uint8 CyDmaTdAllocate(void) `=ReentrantKeil("CyDmaTdAllocate")`;
void CyDmaTdFree(uint8 tdHandle) `=ReentrantKeil("CyDmaTdFree")`;
uint8 CyDmaTdFreeCount(void) `=ReentrantKeil("CyDmaTdFreeCount")`;
cystatus CyDmaTdSetConfiguration(uint8 tdHandle, uint16 transferCount, uint8 nextTd, uint8 configuration) `=ReentrantKeil("CyDmaTdSetConfiguration")`;
cystatus CyDmaTdGetConfiguration(uint8 tdHandle, uint16 * transferCount, uint8 * nextTd, uint8 * configuration) `=ReentrantKeil("CyDmaTdGetConfiguration")`;
cystatus CyDmaTdSetAddress(uint8 tdHandle, uint16 source, uint16 destination) `=ReentrantKeil("CyDmaTdSetAddress")`;
cystatus CyDmaTdGetAddress(uint8 tdHandle, uint16 * source, uint16 * destination) `=ReentrantKeil("CyDmaTdGetAddress")`;

/* __CYDMAC_H__ */
#endif
