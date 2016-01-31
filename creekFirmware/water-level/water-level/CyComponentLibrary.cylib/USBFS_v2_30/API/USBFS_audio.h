/*******************************************************************************
* File Name: `$INSTANCE_NAME`_audio.h
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  Header File for the USFS component. Contains prototypes and constant values.
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

#if !defined(`$INSTANCE_NAME`_audio_H)
#define `$INSTANCE_NAME`_audio_H

#include "cytypes.h"


/***************************************
*  Constants for $INSTANCE_NAME` API.
***************************************/

/* Audio Class-Specific Request Codes (AUDIO Table A-9) */
#define `$INSTANCE_NAME`_REQUEST_CODE_UNDEFINED     (0x00u)
#define `$INSTANCE_NAME`_SET_CUR                    (0x01u)
#define `$INSTANCE_NAME`_GET_CUR                    (0x81u)
#define `$INSTANCE_NAME`_SET_MIN                    (0x02u)
#define `$INSTANCE_NAME`_GET_MIN                    (0x82u)
#define `$INSTANCE_NAME`_SET_MAX                    (0x03u)
#define `$INSTANCE_NAME`_GET_MAX                    (0x83u)
#define `$INSTANCE_NAME`_SET_RES                    (0x04u)
#define `$INSTANCE_NAME`_GET_RES                    (0x84u)
#define `$INSTANCE_NAME`_SET_MEM                    (0x05u)
#define `$INSTANCE_NAME`_GET_MEM                    (0x85u)
#define `$INSTANCE_NAME`_GET_STAT                   (0xFFu)

/* Endpoint Control Selectors (AUDIO Table A-19) */
#define `$INSTANCE_NAME`_EP_CONTROL_UNDEFINED       (0x00u)
#define `$INSTANCE_NAME`_SAMPLING_FREQ_CONTROL      (0x01u)
#define `$INSTANCE_NAME`_PITCH_CONTROL              (0x02u)

/* Feature Unit Control Selectors (AUDIO Table A-11) */
#define `$INSTANCE_NAME`_FU_CONTROL_UNDEFINED       (0x00u)
#define `$INSTANCE_NAME`_MUTE_CONTROL               (0x01u)
#define `$INSTANCE_NAME`_VOLUME_CONTROL             (0x02u)
#define `$INSTANCE_NAME`_BASS_CONTROL               (0x03u)
#define `$INSTANCE_NAME`_MID_CONTROL                (0x04u)
#define `$INSTANCE_NAME`_TREBLE_CONTROL             (0x05u)
#define `$INSTANCE_NAME`_GRAPHIC_EQUALIZER_CONTROL  (0x06u)
#define `$INSTANCE_NAME`_AUTOMATIC_GAIN_CONTROL     (0x07u)
#define `$INSTANCE_NAME`_DELAY_CONTROL              (0x08u)
#define `$INSTANCE_NAME`_BASS_BOOST_CONTROL         (0x09u)
#define `$INSTANCE_NAME`_LOUDNESS_CONTROL           (0x0Au)

#define `$INSTANCE_NAME`_SAMPLE_FREQ_LEN            (3u)
#define `$INSTANCE_NAME`_VOLUME_LEN                 (2u)

#endif /* End `$INSTANCE_NAME`_audio_H */


/* [] END OF FILE */
