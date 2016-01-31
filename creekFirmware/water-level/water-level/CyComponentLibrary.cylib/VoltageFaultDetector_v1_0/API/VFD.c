/*******************************************************************************
* File Name: `$INSTANCE_NAME`.c
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Contains the function prototypes, constants and register definition
*  of the Voltage Fault Detector Component.
*
* Note:
*  None
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

#include "CyLib.h"
#include "`$INSTANCE_NAME`.h"
#include "math.h"

uint8 `$INSTANCE_NAME`_OVGFValArr[`$INSTANCE_NAME`_Amount_of_Voltages]  = {0u};
uint8 `$INSTANCE_NAME`_UVGFValArr[`$INSTANCE_NAME`_Amount_of_Voltages]  = {0u};
uint8 `$INSTANCE_NAME`_dacOVChan = 0u;
uint8 `$INSTANCE_NAME`_dacUVChan = 0u;
uint8 `$INSTANCE_NAME`_ovRDChan  = 0u;
uint8 `$INSTANCE_NAME`_uvRDChan  = 0u;
uint8 `$INSTANCE_NAME`_ovWRChan  = 0u;
uint8 `$INSTANCE_NAME`_uvWRChan  = 0u;
uint8 `$INSTANCE_NAME`_initVar   = 0u;
uint32 `$INSTANCE_NAME`_ovStatus = 0u;

`$NominalVoltage`
`$UVFaultThreshold`
`$OVFaultThreshold`
`$VoltageScale`


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Init
********************************************************************************
*
* Summary:
*  Inits/Restores default VFD configuration provided with customizer.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Init(void) `=ReentrantKeil($INSTANCE_NAME . "_Init")`
{   
    uint8 i;
    
    #if(!`$INSTANCE_NAME`_External_Reference)   
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
    
            uint8 dacOVTD = 0u;
            uint8 dacUVTD = 0u;
            uint8 ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 ovWRTD  = 0u;
            uint8 uvWRTD  = 0u;
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
        
            uint8 dacOVTD = 0u;
            uint8 ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 ovWRTD  = 0u;
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */ 
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
        
            uint8 dacUVTD = 0u;
            uint8 uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 uvWRTD  = 0u;
            
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */ 
        
    #else
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
    
            uint8 ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 ovWRTD  = 0u;
            uint8 uvWRTD  = 0u;
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
    
            uint8 ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 ovWRTD  = 0u;
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
    
            uint8 uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages]  = 0u;
            uint8 uvWRTD  = 0u;
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
    
    #endif /* !`$INSTANCE_NAME`_External_Reference */
  
    
    #if(!`$INSTANCE_NAME`_External_Reference)   
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
            
            CY_SET_REG8(`$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH); 
            CY_SET_REG8(`$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH);             
            
            /* Init DMA, 1 byte bursts, each burst requires a request */
            `$INSTANCE_NAME`_dacOVChan = `$INSTANCE_NAME`_DAC_OV_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST, `$INSTANCE_NAME`_REQUEST_PER_BURST, \
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_dacUVChan = `$INSTANCE_NAME`_DAC_UV_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST, `$INSTANCE_NAME`_REQUEST_PER_BURST, \
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_ovRDChan = `$INSTANCE_NAME`_OV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvRDChan = `$INSTANCE_NAME`_UV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_ovWRChan = `$INSTANCE_NAME`_OV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST, `$INSTANCE_NAME`_REQUEST_PER_BURST, \
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvWRChan = `$INSTANCE_NAME`_UV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST, `$INSTANCE_NAME`_REQUEST_PER_BURST, \
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));                                 
            dacOVTD = CyDmaTdAllocate();
            dacUVTD = CyDmaTdAllocate();  
            ovWRTD  = CyDmaTdAllocate();
            uvWRTD  = CyDmaTdAllocate();
            
            /* Configure this Td as follows:
            *  - The TD is looping on itself
            *  - Increment the destination address, but not the source address
            */
            CyDmaTdSetConfiguration(dacOVTD, `$INSTANCE_NAME`_Amount_of_Voltages, dacOVTD, TD_INC_SRC_ADR);
            CyDmaTdSetConfiguration(dacUVTD, `$INSTANCE_NAME`_Amount_of_Voltages, dacUVTD, TD_INC_SRC_ADR);
            CyDmaTdSetConfiguration(ovWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, ovWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_OV_WR__TD_TERMOUT_EN);
            CyDmaTdSetConfiguration(uvWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, uvWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_UV_WR__TD_TERMOUT_EN);
            
            /* From the memory to VDAC_OV */
            CyDmaTdSetAddress(dacOVTD, LO16((uint32)`$INSTANCE_NAME`_OVFaultThreshold), LO16((uint32)`$INSTANCE_NAME`_OV_THRESHOLD_ADDR));
            /* From the memory to VDAC_UV */
            CyDmaTdSetAddress(dacUVTD, LO16((uint32)`$INSTANCE_NAME`_UVFaultThreshold), LO16((uint32)`$INSTANCE_NAME`_UV_THRESHOLD_ADDR));
            /* From the memory to OV Glitch Filter */
            CyDmaTdSetAddress(ovWRTD, (LO16((uint32)`$INSTANCE_NAME`_OVGFValArr)), LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR));
            /* From the memory to UV Glitch Filter */
            CyDmaTdSetAddress(uvWRTD, (LO16((uint32)`$INSTANCE_NAME`_UVGFValArr)), LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR));
            
            /* Allocate required number of TDs for UV and OV DMA read */
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                ovRDTD[i]  = CyDmaTdAllocate();
                uvRDTD[i]  = CyDmaTdAllocate();
            }
            
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                
                if((i + 1) != `$INSTANCE_NAME`_Amount_of_Voltages)
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[i+1], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);                                                                                       
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[i+1], `$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                else
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[0u], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);                                                                                       
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[0u],`$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                
                /* From the OV Glitch Filter to memory */
                CyDmaTdSetAddress(ovRDTD[i], LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_OVGFValArr + i)));
                /* From the UV Glitch Filter to memory */
                CyDmaTdSetAddress(uvRDTD[i], LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_UVGFValArr + i)));
            }
            
            /* Associate the TDs with the channels */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_dacOVChan, dacOVTD);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_dacUVChan, dacUVTD);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovRDChan, ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvRDChan, uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovWRChan, ovWRTD);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvWRChan, uvWRTD);   
            
        
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
            
            CY_SET_REG8(`$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH);
            CY_SET_REG8(`$INSTANCE_NAME`_OV_VDAC_DATA_PTR,`$INSTANCE_NAME`_OVFaultThreshold[0]); 
           
                    
            /* Init DMA, 1 byte bursts, each burst requires a request */
            `$INSTANCE_NAME`_dacOVChan = `$INSTANCE_NAME`_DAC_OV_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST,
                                                                               `$INSTANCE_NAME`_REQUEST_PER_BURST,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_ovRDChan = `$INSTANCE_NAME`_OV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_ovWRChan = `$INSTANCE_NAME`_OV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST,
                                                                             `$INSTANCE_NAME`_REQUEST_PER_BURST,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            
            dacOVTD = CyDmaTdAllocate();
            ovWRTD  = CyDmaTdAllocate();
             
            /* Configure this Td as follows:
            *  - The TD is looping on itself
            *  - Increment the destination address, but not the source address
            */
            CyDmaTdSetConfiguration(dacOVTD, `$INSTANCE_NAME`_Amount_of_Voltages, dacOVTD, TD_INC_SRC_ADR);
            CyDmaTdSetConfiguration(ovWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, ovWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_OV_WR__TD_TERMOUT_EN);
            
                  
            /* From the memory to VDAC_OV */
            CyDmaTdSetAddress(dacOVTD, LO16((uint32)`$INSTANCE_NAME`_OVFaultThreshold), \
                                       LO16((uint32)`$INSTANCE_NAME`_OV_THRESHOLD_ADDR));
            /* From the memory to OV Glitch Filter */
            CyDmaTdSetAddress(ovWRTD, (LO16((uint32)`$INSTANCE_NAME`_OVGFValArr)), \
                                      LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR));
            
            
            /* Allocate required number of TDs for UV and OV DMA read */
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                ovRDTD[i]  = CyDmaTdAllocate();
            }
            
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                
                if((i + 1) != `$INSTANCE_NAME`_Amount_of_Voltages)
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[i+1], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);
                }
                else
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[0u], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);
                }
                
                /* From the OV Glitch Filter to memory */
                CyDmaTdSetAddress(ovRDTD[i], LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_OVGFValArr + i)));
            }
            
            /* Associate the TDs with the channels */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_dacOVChan, dacOVTD);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovRDChan, ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovWRChan, ovWRTD);
        
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
                         
           CY_SET_REG8(`$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH);
            CY_SET_REG8(`$INSTANCE_NAME`_UV_VDAC_DATA_PTR,`$INSTANCE_NAME`_UVFaultThreshold[0]); 
           
                    
            /* Init DMA, 1 byte bursts, each burst requires a request */
            `$INSTANCE_NAME`_dacUVChan = `$INSTANCE_NAME`_DAC_UV_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST,
                                                                               `$INSTANCE_NAME`_REQUEST_PER_BURST,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvRDChan = `$INSTANCE_NAME`_UV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvWRChan = `$INSTANCE_NAME`_UV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST,
                                                                             `$INSTANCE_NAME`_REQUEST_PER_BURST,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            
            dacUVTD = CyDmaTdAllocate();
            uvWRTD  = CyDmaTdAllocate();
             
            /* Configure this Td as follows:
            *  - The TD is looping on itself
            *  - Increment the destination address, but not the source address
            */
            CyDmaTdSetConfiguration(dacUVTD, `$INSTANCE_NAME`_Amount_of_Voltages, dacUVTD, TD_INC_SRC_ADR);
            CyDmaTdSetConfiguration(uvWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, uvWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_UV_WR__TD_TERMOUT_EN);
            
                  
            /* From the memory to VDAC_OV */
            CyDmaTdSetAddress(dacUVTD, LO16((uint32)`$INSTANCE_NAME`_UVFaultThreshold), \
                                       LO16((uint32)`$INSTANCE_NAME`_UV_THRESHOLD_ADDR));
            /* From the memory to OV Glitch Filter */
            CyDmaTdSetAddress(uvWRTD, (LO16((uint32)`$INSTANCE_NAME`_UVGFValArr)), \
                                      LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR));
            
            
            /* Allocate required number of TDs for UV and OV DMA read */
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                uvRDTD[i]  = CyDmaTdAllocate();
            }
            
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                
                if((i + 1) != `$INSTANCE_NAME`_Amount_of_Voltages)
                {
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[i+1], `$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                else
                {
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[0u], `$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                
                /* From the OV Glitch Filter to memory */
                CyDmaTdSetAddress(uvRDTD[i], LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_UVGFValArr + i)));
            }
            
            /* Associate the TDs with the channels */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_dacUVChan, dacUVTD);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvRDChan, uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvWRChan, uvWRTD);
        
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
    
    #else
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
            
            CY_SET_REG8(`$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH); 
            CY_SET_REG8(`$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH);             
            
            /* Init DMA, 1 byte bursts, each burst requires a request */
            `$INSTANCE_NAME`_ovRDChan = `$INSTANCE_NAME`_OV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvRDChan = `$INSTANCE_NAME`_UV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_ovWRChan = `$INSTANCE_NAME`_OV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST, `$INSTANCE_NAME`_REQUEST_PER_BURST, \
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvWRChan = `$INSTANCE_NAME`_UV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST, `$INSTANCE_NAME`_REQUEST_PER_BURST, \
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));                                 
            ovWRTD  = CyDmaTdAllocate();
            uvWRTD  = CyDmaTdAllocate();
            
            /* Configure this Td as follows:
            *  - The TD is looping on itself
            *  - Increment the destination address, but not the source address
            */
            CyDmaTdSetConfiguration(ovWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, ovWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_OV_WR__TD_TERMOUT_EN);
            CyDmaTdSetConfiguration(uvWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, uvWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_UV_WR__TD_TERMOUT_EN);
            
            CyDmaTdSetAddress(ovWRTD, (LO16((uint32)`$INSTANCE_NAME`_OVGFValArr)), LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR));
            /* From the memory to UV Glitch Filter */
            CyDmaTdSetAddress(uvWRTD, (LO16((uint32)`$INSTANCE_NAME`_UVGFValArr)), LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR));
            
            /* Allocate required number of TDs for UV and OV DMA read */
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                ovRDTD[i]  = CyDmaTdAllocate();
                uvRDTD[i]  = CyDmaTdAllocate();
            }
            
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                
                if((i + 1) != `$INSTANCE_NAME`_Amount_of_Voltages)
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[i+1], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);                                                                                       
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[i+1], `$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                else
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[0u], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);                                                                                       
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[0u],`$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                
                /* From the OV Glitch Filter to memory */
                CyDmaTdSetAddress(ovRDTD[i], LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_OVGFValArr + i)));
                /* From the UV Glitch Filter to memory */
                CyDmaTdSetAddress(uvRDTD[i], LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_UVGFValArr + i)));
            }
            
            /* Associate the TDs with the channels */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovRDChan, ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvRDChan, uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovWRChan, ovWRTD);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvWRChan, uvWRTD);   
            
        
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
            
            CY_SET_REG8(`$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH);
           
                    
            /* Init DMA, 1 byte bursts, each burst requires a request */
            `$INSTANCE_NAME`_ovRDChan = `$INSTANCE_NAME`_OV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_ovWRChan = `$INSTANCE_NAME`_OV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST,
                                                                             `$INSTANCE_NAME`_REQUEST_PER_BURST,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            ovWRTD  = CyDmaTdAllocate();
             
            /* Configure this Td as follows:
            *  - The TD is looping on itself
            *  - Increment the destination address, but not the source address
            */
            CyDmaTdSetConfiguration(ovWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, ovWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_OV_WR__TD_TERMOUT_EN);
            
            /* From the memory to OV Glitch Filter */
            CyDmaTdSetAddress(ovWRTD, (LO16((uint32)`$INSTANCE_NAME`_OVGFValArr)), \
                                      LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR));
            
            
            /* Allocate required number of TDs for UV and OV DMA read */
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                ovRDTD[i]  = CyDmaTdAllocate();
            }
            
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                
                if((i + 1) != `$INSTANCE_NAME`_Amount_of_Voltages)
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[i+1], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);
                }
                else
                {
                    CyDmaTdSetConfiguration(ovRDTD[i],  1u, ovRDTD[0u], `$INSTANCE_NAME`_OV_RD__TD_TERMOUT_EN);
                }
                
                /* From the OV Glitch Filter to memory */
                CyDmaTdSetAddress(ovRDTD[i], LO16((uint32)`$INSTANCE_NAME`_OV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_OVGFValArr + i)));
            }
            
            /* Associate the TDs with the channels */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovRDChan, ovRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_ovWRChan, ovWRTD);
        
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
                         
            CY_SET_REG8(`$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR, `$INSTANCE_NAME`_GF_LENGTH);
                   
            /* Init DMA, 1 byte bursts, each burst requires a request */
            `$INSTANCE_NAME`_uvRDChan = `$INSTANCE_NAME`_UV_RD_DmaInitialize(0, 0,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            `$INSTANCE_NAME`_uvWRChan = `$INSTANCE_NAME`_UV_WR_DmaInitialize(`$INSTANCE_NAME`_BYTES_PER_BURST,
                                                                             `$INSTANCE_NAME`_REQUEST_PER_BURST,
                                             HI16(`$INSTANCE_NAME`_SRC_BASE), HI16(`$INSTANCE_NAME`_DST_BASE));
            uvWRTD  = CyDmaTdAllocate();
             
            /* Configure this Td as follows:
            *  - The TD is looping on itself
            *  - Increment the destination address, but not the source address
            */
            CyDmaTdSetConfiguration(uvWRTD,  `$INSTANCE_NAME`_Amount_of_Voltages, uvWRTD, TD_INC_SRC_ADR | `$INSTANCE_NAME`_UV_WR__TD_TERMOUT_EN);
            
                  
            /* From the memory to OV Glitch Filter */
            CyDmaTdSetAddress(uvWRTD, (LO16((uint32)`$INSTANCE_NAME`_UVGFValArr)), \
                                      LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR));
            
            
            /* Allocate required number of TDs for UV and OV DMA read */
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                uvRDTD[i]  = CyDmaTdAllocate();
            }
            
            for(i = 0; i < `$INSTANCE_NAME`_Amount_of_Voltages; i++)
            {
                
                if((i + 1) != `$INSTANCE_NAME`_Amount_of_Voltages)
                {
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[i+1], `$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                else
                {
                    CyDmaTdSetConfiguration(uvRDTD[i],  1u, uvRDTD[0u], `$INSTANCE_NAME`_UV_RD__TD_TERMOUT_EN);
                }
                
                /* From the OV Glitch Filter to memory */
                CyDmaTdSetAddress(uvRDTD[i], LO16((uint32)`$INSTANCE_NAME`_UV_GF_ADDR), LO16((uint32)(`$INSTANCE_NAME`_UVGFValArr + i)));
            }
            
            /* Associate the TDs with the channels */
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvRDChan, uvRDTD[`$INSTANCE_NAME`_Amount_of_Voltages - 1]);
            CyDmaChSetInitialTd(`$INSTANCE_NAME`_uvWRChan, uvWRTD);
        
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
    
    #endif /* !`$INSTANCE_NAME`_External_Reference */
  
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Enable
********************************************************************************
*
* Summary:
*  Enables the DMA and also provides the software enable to the FSM.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  None.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Enable(void) `=ReentrantKeil($INSTANCE_NAME . "_Enable")`
{
    
    #if(!`$INSTANCE_NAME`_External_Reference)
    
        /* Start internal components an enable DMA channels */
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
            
            `$INSTANCE_NAME`_VDAC_OV_Start();
            `$INSTANCE_NAME`_VDAC_UV_Start();
            `$INSTANCE_NAME`_Comp_OV_Start();
            `$INSTANCE_NAME`_Comp_UV_Start();
            
            CyDmaChEnable(`$INSTANCE_NAME`_dacOVChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_dacUVChan, 1u);     
            CyDmaChEnable(`$INSTANCE_NAME`_ovRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_uvRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_ovWRChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_uvWRChan, 1u); 
            
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
      
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
            
            `$INSTANCE_NAME`_VDAC_OV_Start();
            `$INSTANCE_NAME`_Comp_OV_Start();
            
            CyDmaChEnable(`$INSTANCE_NAME`_dacOVChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_ovRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_ovWRChan, 1u); 
                      
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
        
            `$INSTANCE_NAME`_VDAC_UV_Start();
            `$INSTANCE_NAME`_Comp_UV_Start();
            
            CyDmaChEnable(`$INSTANCE_NAME`_dacUVChan, 1u);     
            CyDmaChEnable(`$INSTANCE_NAME`_uvRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_uvWRChan, 1u); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
    
    #else
            /* Start internal components an enable DMA channels */
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
            
            `$INSTANCE_NAME`_Comp_OV_Start();
            `$INSTANCE_NAME`_Comp_UV_Start();
            
            CyDmaChEnable(`$INSTANCE_NAME`_ovRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_uvRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_ovWRChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_uvWRChan, 1u); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
      
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
            
            `$INSTANCE_NAME`_Comp_OV_Start();
            
            CyDmaChEnable(`$INSTANCE_NAME`_ovRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_ovWRChan, 1u); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
        
            `$INSTANCE_NAME`_Comp_UV_Start();
            
            CyDmaChEnable(`$INSTANCE_NAME`_uvRDChan, 1u); 
            CyDmaChEnable(`$INSTANCE_NAME`_uvWRChan, 1u); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
    
    #endif /* !`$INSTANCE_NAME`_External_Reference */
    
    CY_SET_REG8(`$INSTANCE_NAME`_VS_CNT_AUX_CONTROL_PTR, `$INSTANCE_NAME`_CNT_EN);       
    CY_SET_REG8(`$INSTANCE_NAME`_CYCLE_CNT_AUX_CONTROL_PTR, `$INSTANCE_NAME`_CNT_EN); 
    
    /* Software enable */
    `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_SW_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Start
********************************************************************************
*
* Summary:
*  Initialize and Enable the VFD component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Global variables:
*  `$INSTANCE_NAME`_initVar - used to check initial configuration, modified on
*  first function call.
*
* Theory:
*  Enables the DMA channels and provides the software enable for the FSM.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Start(void) `=ReentrantKeil($INSTANCE_NAME . "_Start")`
{
    if(`$INSTANCE_NAME`_initVar == 0u)
    {
        `$INSTANCE_NAME`_Init();
        `$INSTANCE_NAME`_initVar = 1u;
    }

    `$INSTANCE_NAME`_Enable();
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Stop
********************************************************************************
*
* Summary:
*  Disable the VFD component.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Theory:
*  Disable the DMA channels and disable the software enable for FSM.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Stop(void) `=ReentrantKeil($INSTANCE_NAME . "_Stop")`
{
      /* Disable FSM */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_SW_ENABLE;
    
    #if(!`$INSTANCE_NAME`_External_Reference)
    
        /* Enable DMA channels */
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
        
            #if(!CY_PSOC5A)
            
                `$INSTANCE_NAME`_VDAC_OV_Stop();
                `$INSTANCE_NAME`_VDAC_UV_Stop();
                `$INSTANCE_NAME`_Comp_OV_Stop();
                `$INSTANCE_NAME`_Comp_UV_Stop();
            #endif /* !CY_PSOC5A */
            
            CyDmaChDisable(`$INSTANCE_NAME`_dacOVChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_dacUVChan);     
            CyDmaChDisable(`$INSTANCE_NAME`_ovRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_uvRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_ovWRChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_uvWRChan); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
      
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
        
            #if(!CY_PSOC5A)
                
                `$INSTANCE_NAME`_VDAC_OV_Stop();
                `$INSTANCE_NAME`_Comp_OV_Stop();
            #endif /* !CY_PSOC5A */                
            
            CyDmaChDisable(`$INSTANCE_NAME`_dacOVChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_ovRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_ovWRChan); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
            
            #if(!CY_PSOC5A)
            
                `$INSTANCE_NAME`_VDAC_UV_Stop();
                `$INSTANCE_NAME`_Comp_UV_Stop();
            #endif /* !CY_PSOC5A */
            
            CyDmaChDisable(`$INSTANCE_NAME`_dacUVChan);     
            CyDmaChDisable(`$INSTANCE_NAME`_uvRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_uvWRChan); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
    #else
        /* Enable DMA channels */
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
        
            #if(!CY_PSOC5A)
            
                `$INSTANCE_NAME`_Comp_OV_Stop();
                `$INSTANCE_NAME`_Comp_UV_Stop();
            #endif /* !CY_PSOC5A */
            
            CyDmaChDisable(`$INSTANCE_NAME`_ovRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_uvRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_ovWRChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_uvWRChan); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
      
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
        
            #if(!CY_PSOC5A)
                
                `$INSTANCE_NAME`_Comp_OV_Stop();
            #endif /* !CY_PSOC5A */                
            
            CyDmaChDisable(`$INSTANCE_NAME`_ovRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_ovWRChan); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY */
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
            
            #if(!CY_PSOC5A)
            
                `$INSTANCE_NAME`_Comp_UV_Stop();
            #endif /* !CY_PSOC5A */
            
            CyDmaChDisable(`$INSTANCE_NAME`_uvRDChan); 
            CyDmaChDisable(`$INSTANCE_NAME`_uvWRChan); 
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY */
        
    #endif /* !`$INSTANCE_NAME`_External_Reference */
}


#if((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV) || \
    (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY))

    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetOVFaultStatus
    ********************************************************************************
    *
    * Summary:
    *  Returns over voltage fault status of each voltage input.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  None.
    *
    * Side Effects:
    *  Calling this API clears the fault condition source sticky bits. If the 
    *  condition still persists then the bit will be set again after the next scan.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint32 `$INSTANCE_NAME`_GetOVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultStatus")`
    {
        uint32 ovStatus = 0u;
        
            #if(`$INSTANCE_NAME`_Amount_of_Voltages <= 8u)
                ovStatus = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS8_STATUS_PTR);  
                    
            #elif((`$INSTANCE_NAME`_Amount_of_Voltages > 8u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 16u))
                
                uint8 ovStatus8 = 0u;
                uint8 ovStatus16 = 0u;
                
                ovStatus8  = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS8_STATUS_PTR);  
                ovStatus16 = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS16_STATUS_PTR);  
                ovStatus   = (uint32)((((uint16)ovStatus16) << `$INSTANCE_NAME`_SHIFT_VAL_8) | (uint16)ovStatus8);
                    
            #elif((`$INSTANCE_NAME`_Amount_of_Voltages > 16u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 24u))
                
                uint8 ovStatus8 = 0u;
                uint8 ovStatus16 = 0u;
                uint8 ovStatus24 = 0u;
                
                ovStatus8  = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS8_STATUS_PTR);
                ovStatus16 = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS16_STATUS_PTR);
                ovStatus24 = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS24_STATUS_PTR);
                ovStatus   = (uint32)((((uint32)ovStatus24) << `$INSTANCE_NAME`_SHIFT_VAL_16) | 
                    ((((uint32)ovStatus16) << `$INSTANCE_NAME`_SHIFT_VAL_8) | (uint32)ovStatus8));
                    
            #elif((`$INSTANCE_NAME`_Amount_of_Voltages > 24u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 32u))
                
                uint8 ovStatus8 = 0u;
                uint8 ovStatus16 = 0u;
                uint8 ovStatus24 = 0u;
                uint8 ovStatus32 = 0u;
                
                ovStatus8  = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS8_STATUS_PTR);  
                ovStatus16 = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS16_STATUS_PTR);  
                ovStatus24 = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS24_STATUS_PTR);  
                ovStatus32 = CY_GET_REG8(`$INSTANCE_NAME`_OV_STS32_STATUS_PTR);  
                
                ovStatus = (uint32) ((((uint32)ovStatus32) << `$INSTANCE_NAME`_SHIFT_VAL_24) | 
                    (((uint32)ovStatus24) << `$INSTANCE_NAME`_SHIFT_VAL_16) | 
                        ((((uint32)ovStatus16) << `$INSTANCE_NAME`_SHIFT_VAL_8) | (uint32)ovStatus8));
            #endif /* `$INSTANCE_NAME`_Amount_of_Voltages <= 8u */ 
        

        
        return (ovStatus);
    }
    

    #if(`$INSTANCE_NAME`_External_Reference == 0u)
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_SetOVFaultThreshold
        ********************************************************************************
        *
        * Summary:
        *  Sets the over voltage fault threshold for the specified voltage input. The 
        *  ovFaultThreshold parameter should be stored as is in SRAM for retrieval by the
        *  GetOVFaultThreshold() API. The calculated VDAC value gets written to an SRAM 
        *  buffer for use by the DMA controller that drives the OV DAC. This API does not
        *  apply when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32.
        *
        *  uint16 ovFaultThreshold
        *   Specifies the over voltage fault threshold in mV. Valid range: 1..65,535.
        *
        * Return:
        *  None.
        *  
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_SetOVFaultThreshold(uint8 voltageNum,uint16 ovFaultThreshold) \
                                                              `= ReentrantKeil($INSTANCE_NAME . "_SetOVFaultThreshold")`
        {
            uint16 tmpThreshold = 0u;
            uint32 u32Threshold;
                       
            u32Threshold = (uint32)ovFaultThreshold * 
                (uint32)`$INSTANCE_NAME`_VoltageScale[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum];
           
            tmpThreshold = ((u32Threshold / 1000) / `$INSTANCE_NAME`_DAC_VOL_DIVIDER);
            
            if((((uint16)(u32Threshold / 1000)) % `$INSTANCE_NAME`_DAC_VOL_DIVIDER) > (`$INSTANCE_NAME`_DAC_VOL_DIVIDER / 2u))
            {
                tmpThreshold++;
            }
            
            `$INSTANCE_NAME`_OVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum] = (uint8)(tmpThreshold);
        }

        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_GetOVFaultThreshold
        ********************************************************************************
        *
        * Summary:
        *  Disable the VFD component.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32.
        *
        * Return:
        *  uint16 ovFaultThreshold
        *   The over voltage fault threshold in mV. Valid range: 1..65,535,
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        uint16 `$INSTANCE_NAME`_GetOVFaultThreshold(uint8 voltageNum) \
                                                               `=ReentrantKeil($INSTANCE_NAME . "_GetOVFaultThreshold")`
        {   
            uint32 u32Threshold;
            
			u32Threshold = 
				((uint32)`$INSTANCE_NAME`_OVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum] * 
                (uint32)`$INSTANCE_NAME`_DAC_VOL_DIVIDER * (uint32)1000u) /
				((uint16)`$INSTANCE_NAME`_VoltageScale[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum]);
				
			return ((uint16)(u32Threshold));
        }
            
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_SetOVDac
        ********************************************************************************
        *
        * Summary:
        *  The dacValue gets written to an SRAM buffer for use by the DMA controller 
        *  that drives updates to the OV DAC for the specified voltage input. This API 
        *  does not apply when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32
        *
        * Return:
        *  uint8 dacValue
        *   Specifies the value to be written to the OV VDAC. Valid range: 1..255
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_SetOVDac(uint8 voltageNum, uint8 dacValue) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_SetOVDac")`
        {
            `$INSTANCE_NAME`_OVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum] = dacValue;
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_GetOVDac
        ********************************************************************************
        *
        * Summary:
        *  Returns the dacValue currently being used by the DMA controller that drives 
        *  updates to the OV DAC for the specified voltage input. The value is extracted
        *  from an SRAM buffer – not from the VDAC directly. This API does not apply 
        *  when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32
        *
        * Return:
        *  uint8 dacValue
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        uint8 `$INSTANCE_NAME`_GetOVDac(uint8 voltageNum) `=ReentrantKeil($INSTANCE_NAME . "_GetOVDac")`
        {
            return(`$INSTANCE_NAME`_OVFaultThreshold[voltageNum - 1u]);
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_SetOVDacDirect
        ********************************************************************************
        *
        * Summary:
        *  Allows manual control of the OV VDAC. The dacValue is written directly to 
        *  the VDAC component. Useful for OV VDAC calibration. This API does not apply 
        *  when ExternalRef=true.
        *
        * Parameters:
        *  uint8 dacValue
        *   Valid range: 1..255
        *
        * Return:
        *  None.
        *
        * Side Effects:
        *  Calling this API may cause the comparator to trigger a fault condition. To 
        *  prevent this, call the VFD_Pause() API prior to calling this API.
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_SetOVDacDirect(uint8 dacValue) `=ReentrantKeil($INSTANCE_NAME . "_SetOVDacDirect")`
        {
            CY_SET_REG8(`$INSTANCE_NAME`_OV_VDAC_DATA_PTR, dacValue);
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_GetOVDacDirect
        ********************************************************************************
        *
        * Summary:
        *  Returns current OV VDAC. The returned dacValue is read directly from the VDAC
        *  component. Useful for OV VDAC calibration. Note: if this API is called while 
        *  the component is running, it isn’t possible to know which voltage input the 
        *  OV VDAC value is associated with. This API does not apply when ExternalRef =
        *  true.
        *
        * Parameters:
        *  None.
        *
        * Return:
        *  uint8 dacValue
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/   
        uint8 `$INSTANCE_NAME`_GetOVDacDirect(void) `=ReentrantKeil($INSTANCE_NAME . "_GetOVDacDirect")`
        {
            return(CY_GET_REG8(`$INSTANCE_NAME`_OV_VDAC_DATA_PTR));
        }
    
    #endif /* `$INSTANCE_NAME`_External_Reference == 0u */
    
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetOVGlitchFilterLength
    ********************************************************************************
    *
    * Summary:
    *  The filterLength value gets written to an SRAM buffer for use by the DMA 
    *  controller that drives updates to the OV Glitch Filter.
    *
    * Parameters:
    *  uint8 filterLength
    *   Absolute time units depend on the input clock frequency. Valid range: 1..255
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetOVGlitchFilterLength(uint8 filterLength) \
                                                           `=ReentrantKeil($INSTANCE_NAME . "_SetOVGlitchFilterLength")`
    {
        CY_SET_REG8(`$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR, filterLength);
    }

    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetOVGlitchFilterLength
    ********************************************************************************
    *
    * Summary:
    *  Returns the filterLength value currently being used by the DMA controller. 
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  uint8 filterLength
    *  Absolute time units depend on the input clock frequency. Valid range: 1..255.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetOVGlitchFilterLength(void) \
                                                           `=ReentrantKeil($INSTANCE_NAME . "_SetOVGlitchFilterLength")`
    {
        return(CY_GET_REG8(`$INSTANCE_NAME`_OV_GLITCH_FILTER_LENGTH_PTR));
    }


#endif /* ((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV) || \
        *  (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)) 
        */


    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetUVFaultStatus
    ********************************************************************************
    *
    * Summary:
    *  Returns under voltage fault status of each voltage input.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  uint32 uvFaultStatus
    *  b0: UV fault condition on Voltage Input 1 (1==Present, 0==Not Present)
    *  ...
    *  b31: UV fault condition on Voltage Input 32 (1==Present, 0==Not Present)
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint32 `$INSTANCE_NAME`_GetUVFaultStatus(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultStatus")`
    {
        uint32 uvStatus;
        uint32 pgStatus;
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
            uint32 ovTmpStatus;
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
        
        #if((`$INSTANCE_NAME`_Amount_of_Voltages > 8u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 16u))
            uint8 pgStatus8;
            uint8 pgStatus16;
        #elif((`$INSTANCE_NAME`_Amount_of_Voltages > 16u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 24u))
            uint8 pgStatus8;
            uint8 pgStatus16;
            uint8 pgStatus24;
        #elif((`$INSTANCE_NAME`_Amount_of_Voltages > 16u) && (`$INSTANCE_NAME`_Amount_of_Voltages <= 32u))
            uint8 pgStatus8;
            uint8 pgStatus16;
            uint8 pgStatus24;
            uint8 pgStatus32;
        #endif /* `$INSTANCE_NAME`_Amount_of_Voltages <= 16u */
        
        #if(`$INSTANCE_NAME`_Amount_of_Voltages <= 8u)
        
            pgStatus = `$INSTANCE_NAME`_PG_STS8_STATUS_REG;
                
        #elif(`$INSTANCE_NAME`_Amount_of_Voltages <= 16u)
            
            pgStatus8  = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS8_STATUS_PTR);  
            pgStatus16 = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS16_STATUS_PTR);  
            pgStatus   = (uint32)((((uint16)pgStatus16) << `$INSTANCE_NAME`_SHIFT_VAL_8) | (uint16)pgStatus8);
                
        #elif(`$INSTANCE_NAME`_Amount_of_Voltages <= 24u)
            
            pgStatus8  = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS8_STATUS_PTR);  
            pgStatus16 = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS16_STATUS_PTR);  
            pgStatus24 = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS24_STATUS_PTR);  
            pgStatus   = (uint32)((((uint32)pgStatus24) << `$INSTANCE_NAME`_SHIFT_VAL_16) |
                         ((((uint32)pgStatus16) << `$INSTANCE_NAME`_SHIFT_VAL_8) | (uint32)pgStatus8));
                
        #elif(`$INSTANCE_NAME`_Amount_of_Voltages <= 32u)
            
            pgStatus8  = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS8_STATUS_PTR);  
            pgStatus16 = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS16_STATUS_PTR);  
            pgStatus24 = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS24_STATUS_PTR);  
            pgStatus32 = CY_GET_REG8(`$INSTANCE_NAME`_PG_STS32_STATUS_PTR);  
            
            pgStatus = (uint32)((((uint32)pgStatus32) << `$INSTANCE_NAME`_SHIFT_VAL_24) | 
                (((uint32)pgStatus24) << `$INSTANCE_NAME`_SHIFT_VAL_16) | ((((uint32)pgStatus16) 
                    << `$INSTANCE_NAME`_SHIFT_VAL_8) | pgStatus8));
        #endif
        
        #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
            ovTmpStatus = `$INSTANCE_NAME`_GetOVFaultStatus();
            uvStatus    = (~ovTmpStatus & ~pgStatus) & `$INSTANCE_NAME`_STATUS_MASK;
        #else
            uvStatus = ~pgStatus & `$INSTANCE_NAME`_STATUS_MASK;          
        #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV */
        
        return (uvStatus);
    }
    

#if((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV) || \
    (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY))

    #if(`$INSTANCE_NAME`_External_Reference == 0u)
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_SetUVFaultThreshold
        ********************************************************************************
        *
        * Summary:
        *  Sets the under voltage fault threshold for the specified voltage input. The 
        *  uvFaultThreshold parameter should be stored as is in SRAM for retrieval by 
        *  the GetUVFaultThreshold() API. The calculated VDAC value gets written to an 
        *  SRAM buffer for use by the DMA controller that drives the UV DAC. This API 
        *  does not apply when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32.
        *
        *  uint16 uvFaultThreshold
        *   Specifies the under voltage fault threshold in mV. Valid range: 1..65,535.
        *
        * Return:
        *  None.
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_SetUVFaultThreshold(uint8 voltageNum,uint16 uvFaultThreshold) \
                                                              `= ReentrantKeil($INSTANCE_NAME . "_SetUVFaultThreshold")`
        {
            uint16 tmpThreshold = 0u;
            uint32 u32Threshold;
                       
            u32Threshold = (uint32)uvFaultThreshold * 
                (uint32)`$INSTANCE_NAME`_VoltageScale[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum];
           
            tmpThreshold = ((u32Threshold / 1000) / `$INSTANCE_NAME`_DAC_VOL_DIVIDER);
            
            if((((uint16)(u32Threshold / 1000)) % `$INSTANCE_NAME`_DAC_VOL_DIVIDER) > (`$INSTANCE_NAME`_DAC_VOL_DIVIDER / 2u))
            {
                tmpThreshold++;
            }
            
            `$INSTANCE_NAME`_UVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum] = (uint8)(tmpThreshold);
        }

        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_GetUVFaultThreshold
        ********************************************************************************
        *
        * Summary:
        *  Returns the under voltage fault threshold for the specified voltage input 
        *  that was stored in SRAM by the SetUVFaultThreshold() API. This API does not 
        *  apply when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32.
        *
        * Return:
        *  uint16 uvFaultThreshold
        *   Specifies the under voltage fault threshold in mV. Valid range: 1..65,535.
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        uint16 `$INSTANCE_NAME`_GetUVFaultThreshold(uint8 voltageNum) \
                                                               `=ReentrantKeil($INSTANCE_NAME . "_GetUVFaultThreshold")`
        {
            uint32 u32Threshold;
            
			u32Threshold = 
				((uint32)`$INSTANCE_NAME`_UVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum] * 
                (uint32)`$INSTANCE_NAME`_DAC_VOL_DIVIDER * (uint32)1000u) /
				((uint16)`$INSTANCE_NAME`_VoltageScale[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum]);
				
			return ((uint16)(u32Threshold));
        }
            
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_SetUVDac
        ********************************************************************************
        *
        * Summary:
        *  The dacValue gets written to an SRAM buffer for use by the DMA controller 
        *  that drives updates to the UV DAC for the specified voltage input. This API 
        *  does not apply when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32.
        *
        *  uint8 dacValue
        *   Specifies the value to be written to the UV VDAC. Valid range: 1..255.
        *
        * Return:
        *  None.
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_SetUVDac(uint8 voltageNum, uint8 dacValue) \
                                                                `=ReentrantKeil($INSTANCE_NAME . "_SetUVDac")`
        {
            `$INSTANCE_NAME`_UVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum] = dacValue;
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_GetUVDac
        ********************************************************************************
        *
        * Summary:
        *  Returns the dacValue currently being used by the DMA controller that drives 
        *  updates to the UV DAC for the specified voltage input. The value is extracted
        *  from an SRAM buffer – not from the VDAC directly. This API does not apply 
        *  when ExternalRef=true.
        *
        * Parameters:
        *  uint8 voltageNum  
        *   Specifies the voltage input number. Valid range: 1..32.
        *
        * Return:
        *  uint8 dacValue
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        uint8 `$INSTANCE_NAME`_GetUVDac(uint8 voltageNum) `=ReentrantKeil($INSTANCE_NAME . "_GetUVDac")`
        {
            return(`$INSTANCE_NAME`_UVFaultThreshold[`$INSTANCE_NAME`_Amount_of_Voltages - voltageNum]);
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_SetUVDacDirect
        ********************************************************************************
        *
        * Summary:
        *  Allows manual control of the UV VDAC value. The dacValue is written directly 
        *  to the VDAC component. Useful for UV VDAC calibration. This API does not 
        *  apply when ExternalRef=true.
        *
        * Parameters:
        *  uint8 dacValue
        *   Valid range: 1..255.
        *
        * Return:
        *  None.
        *
        * Side Effects:
        *  Calling this API may cause the comparator to trigger a fault condition. To 
        *  prevent this, call the VFD_Pause() API prior to calling this API.
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/
        void `$INSTANCE_NAME`_SetUVDacDirect(uint8 dacValue) `=ReentrantKeil($INSTANCE_NAME . "_SetUVDacDirect")`
        {
            CY_SET_REG8(`$INSTANCE_NAME`_UV_VDAC_DATA_PTR, dacValue);
        }
        
        
        /*******************************************************************************
        * Function Name: `$INSTANCE_NAME`_GetUVDacDirect
        ********************************************************************************
        *
        * Summary:
        *  Returns current UV VDAC. The returned dacValue is read directly from the VDAC
        *  component. Useful for UV VDAC calibration. Note: if this API is called while 
        *  the component is running, it isn’t possible to know which voltage input the 
        *  UV VDAC value is associated with. This API does not apply when 
        *  ExternalRef=true.
        *
        * Parameters:
        *  uint8 dacValue
        *
        * Return:
        *  None.
        *
        * Reentrant:
        *  No.
        *
        *******************************************************************************/   
        uint8 `$INSTANCE_NAME`_GetUVDacDirect(void) `=ReentrantKeil($INSTANCE_NAME . "_GetUVDacDirect")`
        {
            return(CY_GET_REG8(`$INSTANCE_NAME`_UV_VDAC_DATA_PTR));
        }
    
    #endif /* `$INSTANCE_NAME`_External_Reference == 0u */
    
        
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_SetUVGlitchFilterLength
    ********************************************************************************
    *
    * Summary:
    *  The filterLength value gets written to an SRAM buffer for use by the DMA 
    *  controller that drives updates to the UV Glitch Filter.
    *
    * Parameters:
    *  uint8 filterLength
    *   Absolute time units depend on the input clock frequency. Valid range: 
    *   1..255.
    *
    * Return:
    *  None.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    void `$INSTANCE_NAME`_SetUVGlitchFilterLength(uint8 filterLength) \
                                                           `=ReentrantKeil($INSTANCE_NAME . "_SetUVGlitchFilterLength")`
    {
        CY_SET_REG8(`$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR, filterLength);
    }

    
    /*******************************************************************************
    * Function Name: `$INSTANCE_NAME`_GetUVGlitchFilterLength
    ********************************************************************************
    *
    * Summary:
    *  Returns the filterLength value currently being used by the DMA controller 
    *  that drives updates to the UV Glitch Filter. The value is extracted from an 
    *  SRAM buffer – not from the Glitch Filter directly.
    *
    * Parameters:
    *  None.
    *
    * Return:
    *  uint8 filterLength
    *   Absolute time units depend on the input clock frequency. Valid range: 
    *   1..255.
    *
    * Reentrant:
    *  No.
    *
    *******************************************************************************/
    uint8 `$INSTANCE_NAME`_GetUVGlitchFilterLength(void) \
                                                           `=ReentrantKeil($INSTANCE_NAME . "_GetUVGlitchFilterLength")`
    {
        return(CY_GET_REG8(`$INSTANCE_NAME`_UV_GLITCH_FILTER_LENGTH_PTR));
    }

#endif /* ((`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV) || \
        *  (`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)) 
        */


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Pause
********************************************************************************
*
* Summary:
*  Disables the clock to the comparator controller state machine. Note that 
*  calling this API does not stop the DMA controller if it is in process of 
*  executing transactions. DMA takes around 20 BUS_CLK cycles to complete 
*  assuming no other resource is using the DMA controller at the same time. 
*  Therefore, if the purpose of calling this API is specifically to change VDAC 
*  settings (for calibration purposes for example), sufficient time should be
*  allowed to let the DMA controller run to completion before attempting to 
*  access the VDACs directly.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  Stops the fault detection state machine. Does not stop the DMA controller 
*  immediately.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Pause(void) `=ReentrantKeil($INSTANCE_NAME . "_Pause")`
{
    /* Disable FSM */
    `$INSTANCE_NAME`_CONTROL_REG &= ~`$INSTANCE_NAME`_SW_ENABLE;
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_Resume
********************************************************************************
*
* Summary:
*  Enables the clock to the comparator controller state machine.
*
* Parameters:
*  None.
*
* Return:
*  None.
*
* Side Effects:
*  Restarts the fault detection logic.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_Resume(void) `=ReentrantKeil($INSTANCE_NAME . "_Resume")`
{
    if(`$INSTANCE_NAME`_initVar != 0u)
    {    
        /* Enable FSM */
        `$INSTANCE_NAME`_CONTROL_REG |= `$INSTANCE_NAME`_SW_ENABLE;
    }
    else
    {
        /* Doesn't take any effect if `$INSTANCE_NAME`_Start() hasn't been called at least once */
    }
}


/*******************************************************************************
* Function Name: `$INSTANCE_NAME`_ComparatorCal
********************************************************************************
*
* Summary:
*  Runs a calibration routine that measures the selected comparator’s offset 
*  voltage by shorting its inputs together and corrects for it by writing to the
*  CMP block’s trim register.
*
* Parameters:
*  enum compType
*    Valid values: OV, UV.
*
* Return:
*  None.
*
* Side Effects:
*  Calling this API may cause the comparator to trigger a fault condition. To 
*  prevent this, call the VFD_Pause() API prior to calling this API.
*
* Reentrant:
*  No.
*
*******************************************************************************/
void `$INSTANCE_NAME`_ComparatorCal(uint8 compType) `=ReentrantKeil($INSTANCE_NAME . "_ComparatorCal")`
{    
    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV)
        if(`$INSTANCE_NAME`_OV == compType)
        {
            (void) `$INSTANCE_NAME`_Comp_OV_ZeroCal();
        }
        else if(`$INSTANCE_NAME`_UV == compType)
        {
            (void) `$INSTANCE_NAME`_Comp_UV_ZeroCal();
        }
        else
        {
            /* Do nothing. Invalid input parameter */
        }
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_UV) */
    
    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY)
        if(`$INSTANCE_NAME`_OV == compType)
        {
            (void) `$INSTANCE_NAME`_Comp_OV_ZeroCal();
        }
        else
        {
            /* Do nothing. Invalid input parameter */
        }
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_OV_ONLY) */
    
    #if(`$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY)
        if(`$INSTANCE_NAME`_UV == compType)
        {
            (void) `$INSTANCE_NAME`_Comp_UV_ZeroCal();
        }
        else
        {
            /* Do nothing. Invalid input parameter */
        }
    #endif /* `$INSTANCE_NAME`_CompareType == `$INSTANCE_NAME`_UV_ONLY) */
}


/* [] END OF FILE */
