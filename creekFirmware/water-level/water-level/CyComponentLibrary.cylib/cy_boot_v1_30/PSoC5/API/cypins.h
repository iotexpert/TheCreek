/*******************************************************************************
* File Name: cypins.h  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   This file contains the function prototypes and constants used for port/pin in
*   access and control.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/



#include "cytypes.h"
#include "cyfitter.h"

#if !defined(CY_CYPINS_H) 
#define CY_COMP_CYPINS_H 


/**************************************
*        API Parameter Constants 
**************************************/

/*  SetPinDriveMode */
#define PIN_DM_ALG_HIZ      PC_DRIVE_MODE_0
#define PIN_DM_DIG_HIZ      PC_DRIVE_MODE_1  
#define PIN_DM_RES_UP       PC_DRIVE_MODE_2
#define PIN_DM_RES_DWN      PC_DRIVE_MODE_3
#define PIN_DM_OD_LO        PC_DRIVE_MODE_4
#define PIN_DM_OD_HI        PC_DRIVE_MODE_5
#define PIN_DM_STRONG       PC_DRIVE_MODE_6
#define PIN_DM_RES_UPDWN    PC_DRIVE_MODE_7 


/**************************************
*       Register Constants        
**************************************/

/* IO.PRT.PC (Port Pin Configuration Register */
#define PC_DATAOUT          (0x01u)

#define PC_DRIVE_MODE_SHIFT (0x01u)
#define PC_DRIVE_MODE_MASK  (0x07u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_0     (0x00u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_1     (0x01u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_2     (0x02u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_3     (0x03u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_4     (0x04u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_5     (0x05u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_6     (0x06u << PC_DRIVE_MODE_SHIFT)
#define PC_DRIVE_MODE_7     (0x07u << PC_DRIVE_MODE_SHIFT)

#define PC_PIN_STATE        (0x10u)
#define PC_BIDIR_EN         (0x20u)
#define PC_SLEW             (0x40u)
#define PC_BYPASS           (0x80u)


/**************************************
*       Pin API Macros       
**************************************/

/*******************************************************************************
* Macro Name: CyPins_ReadPin
********************************************************************************
* Summary:
*  This macro returns the current state read from a pin.
*
* Parameters:
*   pinPC: address of a Pin Configuration register.
*   #defines for each pin on a chip are provided in the cydevice_trm.h file
*   in the form: 
*       CYREG_PRTx_PCy
*    
*   where x is a port number 0 - 15 and y is a pin number 0 - 7
*
* Return:  
*   uint8 pin state:
*       0 - if the pin state is 0 (low)
*       non 0 - if the pin state is 1 (high)
*
*******************************************************************************/
#define CyPins_ReadPin(pinPC)    ( *(reg8 *)pinPC & PC_PIN_STATE )


/*******************************************************************************
* Macro Name: CyPins_SetPin 
********************************************************************************
* Summary:
*  This macro sets the state of the specified pin to 1
*
* Parameters:
*   pinPC: address of a Pin Configuration register.
*   #defines for each pin on a chip are provided in the cydevice_trm.h file
*   in the form: 
*       CYREG_PRTx_PCy
*    
*   where x is a port number 0 - 15 and y is a pin number 0 - 7
*
* Return:  
*   None   
*
*******************************************************************************/
#define CyPins_SetPin(pinPC)     ( *(reg8 *)pinPC |= PC_DATAOUT)


/*******************************************************************************
* Macro Name: CyPins_ClearPin
********************************************************************************
* Summary:
*  This macro sets the state of the specified pin to 0
*
* Parameters:
*   pinPC: address of a Pin Configuration register.
*   #defines for each pin on a chip are provided in the cydevice_trm.h file
*   in the form: 
*       CYREG_PRTx_PCy
*    
*   where x is a port number 0 - 15 and y is a pin number 0 - 7
*
* Return:  
*   None   
*
*******************************************************************************/
#define CyPins_ClearPin(pinPC)   ( *(reg8 *)pinPC &= ~PC_DATAOUT )


/*******************************************************************************
* Macro Name: CyPins_SetPinDriveMode
********************************************************************************
* Summary:
*  This macro sets drive mode for the specified pin
*
* Parameters:
*   pinPC: address of a Pin Configuration register.
*   #defines for each pin on a chip are provided in the cydevice_trm.h file
*   in the form: 
*       CYREG_PRTx_PCy
*    
*   where x is a port number 0 - 15 and y is a pin number 0 - 7
*
*   mode:  pin drive mode, 0 - 7
*
* Return:  
*   None   
*
*******************************************************************************/
#define CyPins_SetPinDriveMode(pinPC, mode)     ( *(reg8 *)pinPC = (*(reg8 *)pinPC & ~PC_DRIVE_MODE_MASK) | (mode & PC_DRIVE_MODE_MASK) )

/*******************************************************************************
* Macro Name: CyPins_ReadPinDriveMode
********************************************************************************
* Summary:
*  This macro reads the drive mode setting for the specified pin
*
* Parameters:
*   pinPC: address of a Pin Configuration register.
*   #defines for each pin on a chip are provided in the cydevice_trm.h file
*   in the form: 
*       CYREG_PRTx_PCy
*    
*   where x is a port number 0 - 15 and y is a pin number 0 - 7
*
*
* Return:  
*   mode:  uint8 - pin drive mode, 0 - 7
*
*******************************************************************************/
#define CyPins_ReadPinDriveMode(pinPC)      (*(reg8 *)pinPC & PC_DRIVE_MODE_MASK)


#endif /* CY_COMP_CYPINS_H */
/* [] END OF FILE */
