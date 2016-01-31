/*******************************************************************************
* File Name: cybtldr.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the Bootloader component. The API includes functions
*   for starting boot loading operations, validating the application and
*	jumping to the application.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include <cybtldr.h>
#include <cybtldr_common.h>
#include <device.h>
#include <string.h>
/*******************************************************************************
* External References
********************************************************************************/
extern void LaunchApp(uint32 addr);

/*******************************************************************************
* Defines
********************************************************************************/
#define FIRST_APP_BYTE (CYDEV_FLS_ROW_SIZE * (*CY_BTLDR_P_LAST_BLDR_ROW + 1))

#define CYBTLDR_VERSION		{`$CY_MINOR_VERSION`, `$CY_MAJOR_VERSION`, 0x01} 

/* Packet sizes. */
#define MIN_PKT_SIZE        7
#define MAX_PKT_SIZE        BTLDR_MAX_PACKET_SIZE

/* Packet framing constants. */
#define SOP                 0x01    /* Start of Packet */
#define EOP                 0x17    /* End of Packet */

/* Bootloader command responces */
#define CYRET_ERR_LENGTH    0x03  /* The amount of data available is outside the expected range */
#define CYRET_ERR_DATA      0x04  /* The data is not of the proper form */
#define CYRET_ERR_CMD       0x05  /* The command is not recognized */
#define CYRET_ERR_DEVICE    0x06  /* The expected device does not match the detected device */
#define CYRET_ERR_VERSION   0x07  /* The bootloader version detected is not supported */
#define CYRET_ERR_CHECKSUM  0x08  /* The checksum does not match the expected value */
#define CYRET_ERR_ARRAY     0x09  /* The flash array is not valid */
#define CYRET_ERR_ROW       0x0A  /* The flash row is not valid */
#define CYRET_ERR_UNK       0x0F  /* An unknown error occured */

/* Bootloader command definitions. */
#define CYBTLDR_COMMAND_REPORT_SIZE	0x32    /* Report the programmable portions of flash */
//#define CYBTLDR_COMMAND_PROTECT	0x33    /* Protect the flash row */
#define CYBTLDR_COMMAND_ERASE		0x34    /* Erase the specified flash row */
#define CYBTLDR_COMMAND_SYNC		0x35    /* Sync the bootloader and host application */
#define CYBTLDR_COMMAND_READ		0x36    /* Read a row of flash data */
#define CYBTLDR_COMMAND_DATA		0x37    /* Queue up a block of data for programming */
#define CYBTLDR_COMMAND_ENTER     	0x38    /* Enter the bootloader */
#define CYBTLDR_COMMAND_PROGRAM		0x39    /* Program the specified row */
#define CYBTLDR_COMMAND_VERIFY		0x3A    /* Compute flash row checksum for verification */
#define CYBTLDR_COMMAND_EXIT      	0x3B    /* Exits the bootloader & resets the chip */

/* Bootloader packet format definitions */
/*******************************************************************************
* [1-byte] [1-byte ] [2-byte] [n-byte] [ 2-byte ] [1-byte]
* [ SOP  ] [Command] [ Size ] [ Data ] [Checksum] [ EOP  ]
*******************************************************************************/
#define CYBTLDR_SOP_ADDR            0x00         /* Start of packet offset from beginning */
#define CYBTLDR_CMD_ADDR            0x01         /* Command offset from beginning */
#define CYBTLDR_SIZE_ADDR           0x02         /* Packet size offset from beginning */
#define CYBTLDR_DATA_ADDR           0x04         /* Packet data offset from beginning */
#define CYBTLDR_CHK_ADDR(x)         (0x04 + (x)) /* Packet checksum offset from end */
#define CYBTLDR_EOP_ADDR(x)         (0x06 + (x)) /* End of packet offset from end */

/* Our definition of a row size. */
#if (CYDEV_ECC_ENABLE == 1)
#define CYBTLDR_FROW_SIZE           CYDEV_FLS_ROW_SIZE
#else
#define CYBTLDR_FROW_SIZE           (CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE)
#endif

#define SIZEOF_COMMAND_BUFFER       300 /* Maximum number of bytes accepted in a packet */
uint16 dataOffset;
uint8 CYXDATA packetBuffer[SIZEOF_COMMAND_BUFFER];
uint8 CYXDATA dataBuffer[SIZEOF_COMMAND_BUFFER];
uint8 flashBuffer[CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE];

/* Erase row for eraseing. */
uint8 CYXDATA EraseRow[CYBTLDR_FROW_SIZE];

typedef struct _CYBTLDR_ENTER
{
    uint32 SiliconId;
    uint8 Revision;
    uint8 BootLoaderVersion[3];

} CYBTLDR_ENTER;

typedef struct _CYBTLDR_SIZE
{
    uint16 FirstRow;
    uint16 LastRow;

} CYBTLDR_SIZE;

/* CyBtldr_Checksum and CyBtldr_SizeBytes are forcefully set in code. We then post process
 * the hex file from the linker and inject their values then. When the hex file is loaded
 * onto the device these two variables should have valid values. Because the compiler can
 * do optimizations remove the constant accesses, these should not be accessed directly.
 * Insted, the variables CyBtldr_ChecksumAccess & CyBtldr_SizeBytesAccess should be used
 * to get the proper values at runtime.
*/
#if (defined(__C51__))
const uint8 CYCODE CyBtldr_Checksum = 0;
const uint32 CYCODE CyBtldr_SizeBytes = 0xFFFFFFFF;
#else
const uint8 CYCODE __attribute__((section (".bootloader"))) CyBtldr_Checksum = 0;
const uint32 CYCODE __attribute__((section (".bootloader"))) CyBtldr_SizeBytes = 0xFFFFFFFF;
#endif
uint8* CyBtldr_ChecksumAccess = (uint8*)(&CyBtldr_Checksum);
uint32* CyBtldr_SizeBytesAccess = (uint32*)(&CyBtldr_SizeBytes);

/* Function prototypes */
void CyBtldr_HostLink(uint8 TimeOut);
int WritePacket(uint8 command, uint8 CYXDATA* buffer, uint16 size);

/*******************************************************************************
* Function Name: CyBtldr_BadBootloader
********************************************************************************
* Summary:
*   This is an infinite loop that should be run iff there is an error with the
*   bootloader.
*
* Parameters:  
*   None
*
* Returns:
*   None
*
*******************************************************************************/
void CyBtldr_BadBootloader(void)
{
    while (1); /* the bootloader is invalid so all we can do is hang the MCU */
}


/*******************************************************************************
* Function Name: Calc16BitChecksum
********************************************************************************
* Summary:
*   This computes the 16 bit checksum for the provided number of bytes contained
*   in the provided buffer
*
* Parameters:  
*   buffer:
*      The buffer containing the data to compute the checksum for
*   size:
*      The number of bytes in buffer to compute the checksum for
*
* Returns:
*   16 bit checksum for the provided data
*
*******************************************************************************/
uint16 Calc16BitChecksum(uint8* buffer, uint16 size)
{
	uint16 index = 0;
	uint16 checksum = 0;
	for (; index < size; index++)
		checksum += buffer[index];
		
	checksum = 1 + ~checksum;
	return checksum;
}


/*******************************************************************************
* Function Name: CyBtldr_Start
********************************************************************************
* Summary:
*   This routine will validate the bootloader then if successful attempt to
*   read data from the host application for the specified time before launching
*   the bootloadable application.
*
* Parameters:  
*   None
*
* Returns:
*   None
*
*******************************************************************************/
void CyBtldr_Start(void)
{
	cystatus status;
	uint32 idx;
	uint8 calcedChecksum = 0;
	
	if (!(*CyBtldr_SizeBytesAccess))
	{
		CyBtldr_BadBootloader();
	}
		
	/* the bootloader always starts at 0 in flash */
	for (idx = 0; idx < *CyBtldr_SizeBytesAccess; ++idx)
	{
		calcedChecksum += CY_GETCODEDATA(idx);
	}
	calcedChecksum -= *CyBtldr_ChecksumAccess; /* we actually included the checksum, so remove it */
	calcedChecksum = 1 + ~calcedChecksum;
	
	if (calcedChecksum != *CyBtldr_ChecksumAccess)
	{
		CyBtldr_BadBootloader();
	}
	
	status = CyBtldr_ValidateApp();
#if defined (WORKAROUND_OPT_XRES)
	if (status != CYRET_SUCCESS || (*CY_BTLDR_P_APP_RUN_ADDR) == CYBTLDR_START_BTLDR)
	{
		if (CYRET_SUCCESS != CySetTemp() || CYRET_SUCCESS != CySetFlashEEBuffer(flashBuffer))
		{
			CyBtldr_BadBootloader();
		}
		CyBtldr_SetFlashRunType(0);
#else
	if (status != CYRET_SUCCESS || (CY_GET_REG8(CYREG_RESET_SR0) & CYBTLDR_START_BTLDR) == CYBTLDR_START_BTLDR)
	{
#endif
		CyBtldr_HostLink(0);
	}
	else
	{
#if CYDEV_BOOTLOADER_WAIT_COMMAND == 1
		CyBtldr_HostLink(CYDEV_BOOTLOADER_WAIT_TIME); /* Timeout is in 10s of miliseconds */
#endif
		CyBtldr_LaunchApplication();
	}
}

/*******************************************************************************
* Function Name: CyBtldr_LaunchApplication
********************************************************************************
* Summary:
*   This routine sets a couple bits in the reset register and then performs
*   a soft reset to cause the bootloadable application to run.
*
* Parameters:  
*   None
*
* Returns:
*   None
*
*******************************************************************************/
void CyBtldr_LaunchApplication(void)
{
#if defined (WORKAROUND_OPT_XRES)
	CyBtldr_SetFlashRunType(CYBTLDR_START_APP);
#else
    (*(uint8 CYXDATA*)CYREG_RESET_SR0) |= CYBTLDR_START_APP; /* set bit to indicate we want to start application */
#endif
	(*(uint8 CYXDATA*)CYREG_RESET_CR2) |= 0x01;              /* set bit to cause a software reset */
}

/*******************************************************************************
* Function Name: CyBtldr_CheckLaunch
********************************************************************************
* Summary:
*   This routine checks to see if the bootloader or the bootloadable application
*   should be run.  If the application is to be run, it will start executing.
*   If the bootloader is to be run, ti will return so the bootloader can 
*   continue starting up.
*
* Parameters:  
*   None
*
* Returns:
*   None
*
*******************************************************************************/
void CyBtldr_CheckLaunch(void)
{
#if defined (WORKAROUND_OPT_XRES)
	if ((*CY_BTLDR_P_APP_RUN_ADDR) == CYBTLDR_START_APP)
	{
	    if (CYRET_SUCCESS != CySetTemp() || CYRET_SUCCESS != CySetFlashEEBuffer(flashBuffer))
		{
			CyBtldr_BadBootloader();
		}
		CyBtldr_SetFlashRunType(0);
#else
	if ((*(uint8 XDATA*)CYREG_RESET_SR0 & CYBTLDR_START_APP) == CYBTLDR_START_APP)
	{
		*(uint8 XDATA*)CYREG_RESET_SR0 &= 0x3F; /* clear high order gpsw_s bit, next reset goes to bootloader */
#endif
		/* indicates that we have told ourselves to jump to the application since we
		 * have already told ourselves to jump, we do not do any expensive verification
		 * of the application. We just check to make sure that the value at 
         * CY_APP_ADDR_ADDRESS is something other than 0
		 */
        if (*CY_BTLDR_P_APP_ENTRY_POINT)
			LaunchApp(*CY_BTLDR_P_APP_ENTRY_POINT); /* we never return from this method */
	}
}

/* Moves the arguement appAddr (RO) into PC, moving execution to the appAddr */
#if defined (__ARMCC_VERSION)
__asm void LaunchApp(uint32 appAddr)
{
	BX R0;
}
#elif defined(__GNUC__)
__attribute__((noinline)) /* Workaround for GCC toolchain bug with inlining */
void LaunchApp(uint32 appAddr)
{
	__asm("    BX R0");
}
#endif

/*******************************************************************************
* Function Name: CyBtldr_ValidateApp
********************************************************************************
* Summary:
*   This routine computes the checksum, zero check, 0xFF check of the
*   application area to determine whether a valid application is loaded.
*
* Parameters:  
*   None
*
* Returns:
*   CYRET_SUCCESS  - if successful
*   CYRET_BAD_DATA - if the bootloadable is corrupt
*
*******************************************************************************/
cystatus CyBtldr_ValidateApp()
{
	uint16 CYDATA lastReadIdx;
	uint16 CYDATA idx;
	uint8  CYDATA valid = 0; /* Assume bad flash image */
	uint8  CYDATA calcedChecksum = 0;
	
	/* We don't include the meta data */
	lastReadIdx = FIRST_APP_BYTE + *CY_BTLDR_P_APP_BYTE_LEN;
	
	for (idx = FIRST_APP_BYTE; idx < lastReadIdx; ++idx)
	{
		uint8 CYDATA curByte = CY_GETCODEDATA(idx);
		if (!valid && (curByte != 0 && curByte != 0xFF))
			valid = 1;

		calcedChecksum += curByte;
	}

	calcedChecksum = 1 + ~calcedChecksum;
	if (calcedChecksum != *CY_BTLDR_P_CHECKSUM || !valid)
		return CYRET_BAD_DATA;
	
	return CYRET_SUCCESS;
}


/*******************************************************************************
* Function Name: CyBtldr_HostLink
********************************************************************************
* Summary:
*   Causes the bootloader to attempt to read data being transmitted by the
*   host application.  If data is sent from the host, this establishes the
*   communication interface to process all requests.
*
*
* Parameters:
*   timeOut:
*       The amount of time to listen for data before giving up. Timeout is 
*       measured in 10s of ms.  Use 0 for infinite wait.
*
*   
* Return:
*   None
*
*******************************************************************************/
void CyBtldr_HostLink(uint8 timeOut)
{
    uint16 index;
    uint16 numberRead;
    uint16 checksum;
    uint16 rspSize;
    uint8 ackCode;
    uint16 pktChecksum;
    uint16 pktSize = 0;
    uint8 pktCommand = 0;
	uint8 communicationActive = (timeOut == 0) ? 1 : 0;

    /* Must get temperature and set a temp buffer first. */
    if (CYRET_SUCCESS != CySetTemp() || CYRET_SUCCESS != CySetFlashEEBuffer(flashBuffer))
    {
        CyBtldr_BadBootloader();
    }
    if (timeOut == 0)
        timeOut = 0xFF;

    /* Initialize communications channel. */
    CyBtldrCommStart();

    /* Make sure global interrupts are enabled. */
    CYGlobalIntEnable;

    /* Setup the row for erasing. */    
    for (index = 0; index < CYBTLDR_FROW_SIZE; index++)
    {
        EraseRow[index] = 0xFF;
    }

    /* Start with nothing. */
    dataOffset = 0;

    do
    {
        ackCode = CYRET_SUCCESS;
        if (CyBtldrCommRead(packetBuffer, SIZEOF_COMMAND_BUFFER, &numberRead, timeOut) == CYRET_EMPTY)
        {
            continue;
        }
        if (numberRead < MIN_PKT_SIZE || numberRead > MAX_PKT_SIZE || packetBuffer[CYBTLDR_SOP_ADDR] != SOP)
        {
            ackCode = CYRET_ERR_DATA;
        }
        communicationActive = 1;

        if (ackCode == CYRET_SUCCESS)
        {
            pktCommand  = packetBuffer[CYBTLDR_CMD_ADDR];
            pktSize     = CY_GET_REG16(&packetBuffer[CYBTLDR_SIZE_ADDR]);
            pktChecksum = CY_GET_REG16(&packetBuffer[CYBTLDR_CHK_ADDR(pktSize)]);

            if ((pktSize + MIN_PKT_SIZE) > numberRead)
            {
                ackCode = CYRET_ERR_LENGTH;
            }
            else if (packetBuffer[CYBTLDR_EOP_ADDR(pktSize)] != EOP)
            {
                ackCode = CYRET_ERR_DATA;
            }
            else
            {
                checksum = Calc16BitChecksum(packetBuffer, pktSize + CYBTLDR_DATA_ADDR);
                if (checksum != pktChecksum)
                {
                    ackCode = CYRET_ERR_CHECKSUM;
                }
            }
        }

        rspSize = 0;
        if (ackCode == CYRET_SUCCESS)
        {
            ackCode = CYRET_ERR_DATA;
            switch (pktCommand)
            {
                case CYBTLDR_COMMAND_REPORT_SIZE:
                    if (pktSize == 1)
                    {
                        CYBTLDR_SIZE arraySize;

                        if (packetBuffer[CYBTLDR_DATA_ADDR] < FLASH_NUMBER_SECTORS)
                        {
                            uint16 startRow = (packetBuffer[CYBTLDR_DATA_ADDR] == 0)
                                ? (*CyBtldr_SizeBytesAccess / CYDEV_FLS_ROW_SIZE)
                                : 0;

                            #if (defined(__C51__))
					        arraySize.FirstRow = CYSWAP_ENDIAN16(startRow);
					        arraySize.LastRow = CYSWAP_ENDIAN16(FLASH_NUMBER_ROWS - 1);
                            #else
					        arraySize.FirstRow = startRow;
					        arraySize.LastRow = (FLASH_NUMBER_ROWS - 1);
                            #endif
                        }
                        #if (CYDEV_ECC_ENABLE == 0)
                        else if(packetBuffer[CYBTLDR_DATA_ADDR] == FIRST_EE_ARRAYID)
                        {
					        arraySize.FirstRow = 0;
                            #if (defined(__C51__))
					        arraySize.LastRow = CYSWAP_ENDIAN16(CYDEV_EE_SIZE / CYDEV_EEPROM_ROW_SIZE);
                            #else
					        arraySize.LastRow = (CYDEV_EE_SIZE / CYDEV_EEPROM_ROW_SIZE);
                            #endif
                        }
                        #endif

                        rspSize = sizeof(CYBTLDR_SIZE);
                        cymemcpy(&packetBuffer[CYBTLDR_DATA_ADDR], &arraySize, rspSize);
                        ackCode = CYRET_SUCCESS;
                    }
                    break;

                case CYBTLDR_COMMAND_ERASE:
                    if (pktSize == 3)
                    {
                        pktSize = (packetBuffer[CYBTLDR_DATA_ADDR] > LAST_FLASH_ARRAYID) 
                            ? CYDEV_EEPROM_ROW_SIZE
                            : CYBTLDR_FROW_SIZE;

                        /* We have a row number to errase. */
                        if (CyWriteRowFull(packetBuffer[CYBTLDR_DATA_ADDR], CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])), EraseRow, pktSize))
                        {
                            ackCode = CYRET_ERR_ROW;
                        }
                        else
                        {
                            ackCode = CYRET_SUCCESS;
                        }
                    }
                    break;

                case CYBTLDR_COMMAND_SYNC:
                    /* If something failed the host would send this command to reset the bootloader. */
                    dataOffset = 0;
                    continue; /* Don't ack the packet, just get ready to accept the next one */

                case CYBTLDR_COMMAND_READ:
                    /* Not currently supported */
                    ackCode = CYRET_ERR_CMD;
                    break;

                case CYBTLDR_COMMAND_DATA:
                    /* We have part of a block of data. */
                    ackCode = CYRET_SUCCESS;
                    cymemcpy(&dataBuffer[dataOffset], &packetBuffer[CYBTLDR_DATA_ADDR], pktSize);
                    dataOffset += pktSize;
                    break;

                case CYBTLDR_COMMAND_ENTER:
                    if (pktSize == 0)
                    {
                        #if (defined(__C51__))
                        CYBTLDR_ENTER BtldrVersion = {CYSWAP_ENDIAN32(CYDEV_CHIP_JTAG_ID), CYDEV_CHIP_REV_EXPECT, CYBTLDR_VERSION};
                        #else
                        CYBTLDR_ENTER BtldrVersion = {CYDEV_CHIP_JTAG_ID, CYDEV_CHIP_REV_EXPECT, CYBTLDR_VERSION};
                        #endif
                        rspSize = sizeof(CYBTLDR_ENTER);
                        cymemcpy(&packetBuffer[CYBTLDR_DATA_ADDR], &BtldrVersion, rspSize);
                        ackCode = CYRET_SUCCESS;
                    }
                    break;

                case CYBTLDR_COMMAND_PROGRAM:
                    if (pktSize >= 3)
                    {
                        cymemcpy(&dataBuffer[dataOffset], &packetBuffer[CYBTLDR_DATA_ADDR + 3], pktSize - 3);
                        dataOffset += (pktSize - 3);

                        pktSize = (packetBuffer[CYBTLDR_DATA_ADDR] > LAST_FLASH_ARRAYID) 
                            ? CYDEV_EEPROM_ROW_SIZE
                            : CYBTLDR_FROW_SIZE;

                        if (dataOffset == pktSize)
                        {
                            if (CyWriteRowFull(packetBuffer[CYBTLDR_DATA_ADDR], CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])), dataBuffer, pktSize))
                            {
                                ackCode = CYRET_ERR_ROW;
                            }
                            else
                            {
                                ackCode = CYRET_SUCCESS;
                            }
                        }
                        else
                        {
                            ackCode = CYRET_ERR_LENGTH;
                        }
                        dataOffset = 0;
                    }
                    break;

                case CYBTLDR_COMMAND_VERIFY:
                    if (pktSize == 3)
                    {
                        /* Read the existing flash data. */
                        uint32 rowAddr = ((uint32)packetBuffer[CYBTLDR_DATA_ADDR] * CYDEV_FLS_SECTOR_SIZE) 
                            + ((uint32)CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])) * CYDEV_FLS_ROW_SIZE);
                        for (packetBuffer[CYBTLDR_DATA_ADDR] = 0, index = 0; index < CYDEV_FLS_ROW_SIZE; index++)
                        {
                            packetBuffer[CYBTLDR_DATA_ADDR] += CY_GETCODEDATA(rowAddr + index);
                        }

                        #if (CYDEV_ECC_ENABLE == 0)
                        /* Save the ECC area. */
                        rowAddr = CYDEV_ECC_BASE + ((uint32)packetBuffer[CYBTLDR_DATA_ADDR] * EEPROM_SIZEOF_SECTOR) 
                            + ((uint32)CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])) * CYDEV_ECC_ROW_SIZE);
                        for(index = 0; index < CYDEV_ECC_ROW_SIZE; index++)
                        {
                            packetBuffer[CYBTLDR_DATA_ADDR] += CY_GET_XTND_REG8((uint8 CYFAR *)(rowAddr + index));
                        }
                        #endif

                        packetBuffer[CYBTLDR_DATA_ADDR] = 1 + ~packetBuffer[CYBTLDR_DATA_ADDR];
                        ackCode = CYRET_SUCCESS;
                        rspSize = 1;
                    }
                    break;

                case CYBTLDR_COMMAND_EXIT:
                    #if defined (WORKAROUND_OPT_XRES)
	                CyBtldr_SetFlashRunType(0);
                    #else
                    (*(uint8 XDATA*)CYREG_RESET_SR0) &= 0x3F;   /* clear two MSB gpsw_s bits */
                    #endif
	                *((uint8 CYXDATA*)CYREG_RESET_CR2) |= 0x01; /* set swr bit to cause a software reset */;
                    /* Software reset triggered, will never get here */
                    break;

                default:
                    ackCode = CYRET_ERR_CMD;
                    break;
            }
        }

        /* ?CK the packet and function. */
        WritePacket(ackCode, packetBuffer, rspSize);
    }
    while (communicationActive);
}


/*******************************************************************************
* Function Name: WritePacket
********************************************************************************
* Summary:
*   Creates a bootloader responce packet and transmits it back to the bootloader
*   host application over the already established communications protocol.
*
*
* Parameters:
*   status:
*       The status code to pass back as the second byte of the packet
*   buffer:
*       The buffer containing the data portion of the packet
*   size:
*       The number of bytes contained within the buffer to pass back
*
*   
* Return:
*   CYRET_SUCCESS if successful.
*   CYRET_UNKNOWN if there was an error tranmitting the packet.
*
*******************************************************************************/
int WritePacket(uint8 status, uint8 CYXDATA* buffer, uint16 size)
{
    uint16 sentCount;
    uint16 checksum;
    uint16 count = size + CYBTLDR_DATA_ADDR;

    /* Start of the packet. */
    buffer[CYBTLDR_SOP_ADDR] = SOP;
    buffer[CYBTLDR_CMD_ADDR] = status;
    buffer[CYBTLDR_SIZE_ADDR] = LO8(size);
    buffer[CYBTLDR_SIZE_ADDR + 1] = HI8(size);

    /* Compute the checksum. */
    checksum = Calc16BitChecksum(buffer, count);

    buffer[count++] = LO8(checksum);
    buffer[count++] = HI8(checksum);
    buffer[count++] = EOP;

    /* Start the packet transmit. */
    return (CyBtldrCommWrite(buffer, count, &sentCount, 150) || count != sentCount) 
        ? CYRET_UNKNOWN
        : CYRET_SUCCESS;
}
