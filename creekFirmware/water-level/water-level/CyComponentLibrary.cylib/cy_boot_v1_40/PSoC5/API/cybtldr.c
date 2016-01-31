/*******************************************************************************
* File Name: cybtldr.c  
* Version 1.20
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
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include <device.h>
#include <string.h>
/*******************************************************************************
* External References
********************************************************************************/
extern void LaunchApp(uint32 addr);

/*******************************************************************************
* Defines
********************************************************************************/
#define CHECKSUM_INVERTER 0x100
#define FIRST_APP_BYTE (CYDEV_FLS_ROW_SIZE * (*CY_BTLDR_P_LAST_BLDR_ROW + 1))

/* CyBtldr_Checksum and CyBtldr_SizeBytes are forcefully set in code. We then post process
 * the hex file from the linker and inject their values then. When the hex file is loaded
 * onto the device these two variables should have valid values. Because the compiler can
 * do optimizations remove the constant accesses, these should not be accessed directly.
 * Insted, the variables CyBtldr_ChecksumAccess & CyBtldr_SizeBytesAccess should be used
 * to get the proper values at runtime.
*/
#if (defined(__C51__))
const uint8 CYCODE CyBtldr_Checksum = 0;
const uint32 CYCODE CyBtldr_SizeBytes = 0xFF;
#else
const uint8 CYCODE __attribute__((section (".bootloader"))) CyBtldr_Checksum = 0;
const uint32 CYCODE __attribute__((section (".bootloader"))) CyBtldr_SizeBytes = 0xFF;
#endif
uint8* CyBtldr_ChecksumAccess = (uint8*)(&CyBtldr_Checksum);
uint32* CyBtldr_SizeBytesAccess = (uint32*)(&CyBtldr_SizeBytes);

/* Defined below. */
void CyBtLdrHostLink(uint16 TimeOut);

void CYBtldr_BadBootloader(void)
{
	for (;;)
	{
		/* the bootloader is invalid so all we can do is hang the MCU */
	}
}

void CyBtldr_Start(void)
{
	cystatus status;
	uint32 idx;
	uint8 calcedChecksum = 0;
	uint8 XDATA *resetSR0;
	resetSR0 = (uint8 XDATA*)CYDEV_RESET_SR0;
	
	if (*CyBtldr_SizeBytesAccess == 0)
	{
		CYBtldr_BadBootloader();
	}
		
	/* the bootloader always starts at 0 in flash */
	for (idx = 0; idx < *CyBtldr_SizeBytesAccess; ++idx)
	{
		calcedChecksum += CY_GETCODEDATA(idx);
	}
	calcedChecksum -= *CyBtldr_ChecksumAccess; /* we actually included the checksum, so remove it */
	calcedChecksum = CHECKSUM_INVERTER - calcedChecksum;
	
	if (calcedChecksum != *CyBtldr_ChecksumAccess)
	{
		CYBtldr_BadBootloader();
	}
	
	status = CyBtldr_ValidateApp();
	if (status != CYRET_SUCCESS || (*resetSR0 & 0x40) == 0x40)
	{
		CyBtLdrHostLink(0);
	}
	else
	{
#if CYDEV_BOOTLOADER_WAIT_COMMAND == 1
		CyBtLdrHostLink(CYDEV_BOOTLOADER_WAIT_TIME);
#endif
		CyBtldr_LaunchApplication();
	}
}

void CyBtldr_LaunchApplication(void)
{
	uint8 XDATA *resetSR0;
	uint8 XDATA *resetCR2;
	resetSR0 = (uint8 XDATA*)CYDEV_RESET_SR0;
	resetCR2 = (uint8 XDATA*)CYDEV_RESET_CR2;
	*resetSR0 |= 0x80; /* set high order gpsw_s bit to indicate we want to jump to application */
	*resetCR2 |= 0x01; /* set swr bit to cause a software reset */
}

void CyBtldr_CheckLaunch(void)
{
	uint8 XDATA *resetSR0;
	resetSR0 = (uint8 XDATA*)CYDEV_RESET_SR0;
	if ((*resetSR0 & 0x80) == 0x80)
	{
		uint32 CYCODE *appAddr;
		/* indicates that we have told ourselves to jump to the application
		 * since we have already told ourselves to jump, we do not do any
		 * expensive verification of the application. We just check to make
		 * sure that the value at CY_APP_ADDR_ADDRESS is something other than
		 * 0
		*/
		*resetSR0 &= 0x3F; // clear high order gpsw_s bit, next reset goes to bootloader
		appAddr = (uint32 CYCODE*)CY_BTLDR_MD_APP_ENTRY_POINT_ADDR;
		if (*appAddr)
			LaunchApp(*appAddr); // we never return from this method
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
*   application area.
*
* Parameters:  
*   None
*
* Returns:  CYRET_SUCCESS or CYRET_BAD_DATA
*
*******************************************************************************/
cystatus CyBtldr_ValidateApp()
{
    uint16 CYDATA lastReadIdx;
	uint8  CYDATA allZeros;
	uint8  CYDATA allFs;
	uint16 CYDATA idx;
	uint8  CYDATA calcedChecksum = 0;
	
	/* We don't include the meta data */
	CYAPPADDRESS firstByte = FIRST_APP_BYTE;
	CYAPPADDRESS validDataLen = *CY_BTLDR_P_APP_BYTE_LEN;
	lastReadIdx = firstByte + validDataLen;
	
	/* Assume they are bad */
	allZeros = allFs = 1; 
	
	for (idx = FIRST_APP_BYTE; idx < lastReadIdx; ++idx)
	{
		uint8 CYDATA curByte = CY_GETCODEDATA(idx);
		if (allZeros == 1 && curByte != 0)
			allZeros = 0;
		if (allFs == 1 && curByte != 0xFF)
			allFs = 0;	

		calcedChecksum += curByte;
	}

	calcedChecksum = CHECKSUM_INVERTER - calcedChecksum;
	if (calcedChecksum != *CY_BTLDR_P_CHECKSUM || allZeros != 0 || allFs != 0)
		return CYRET_BAD_DATA;
	
	return CYRET_SUCCESS;
}



/*******************************************************************************
*
*  CyBtLdrHostLink defines, variables and functions.
*
*******************************************************************************/
#if defined(__C51__)
#define LSB16                               1
#define MSB16                               0
#else
#define LSB16                               0
#define MSB16                               1
#endif

/* Packet sizes. */
#define MIN_PKT_SIZE                        6
#define MAX_PKT_SIZE                        BTLDR_MAX_PACKET_SIZE
#define MAX_DATA_SIZE                       (MAX_PKT_SIZE - MIN_PKT_SIZE)

#define CYBTLDR_SIZEOF_ROW_CHKSUM           1

/* Packet framing constants. */
#define SOP                                 0x01
#define EOP                                 0x17

/* Packet and function acknowledment. */
#define ACK                                 CYRET_SUCCESS    /* Standard successful packet acknowledgement. */
#define NAK                                 0x15    /* Standard NOT successful packet acknowledgement. */

#define ERROR_PACKET_CHECKSUM               0x05
#define ERROR_FLASH_ARRAY                   0x02
#define ERROR_FLASH_PROTECTION              0x06

/* Bootloader command definitions. */
#define CYBTLDR_COMMAND_ENTER     			0x38
#define CYBTLDR_COMMAND_EXIT      			0x3B
#define CYBTLDR_COMMAND_SYNC				0x35
#define CYBTLDR_COMMAND_DATA				0x37
#define CYBTLDR_COMMAND_PROGRAM				0x39
#define CYBTLDR_COMMAND_READ				0x36
#define CYBTLDR_COMMAND_ERASE				0x34
#define CYBTLDR_COMMAND_PROTECT				0x33
#define CYBTLDR_COMMAND_VERIFY				0x3A
#define CYBTLDR_COMMAND_UPDATE_CERTIFICATE	0x3C
#define CYBTLDR_COMMAND_TEST_CERTIFICATE	0x31
#define CYBTLDR_COMMAND_REPORT_SIZE			0x32
#define NOT_VALID_COMMAND(x)                ((x < CYBTLDR_COMMAND_TEST_CERTIFICATE) || (x > CYBTLDR_COMMAND_UPDATE_CERTIFICATE))

/* Our definition of a row size. */
#if (CYDEV_ECC_ENABLE == 1)
#define CYBTLDR_FROW_SIZE                   CYDEV_FLS_ROW_SIZE
#else
#define CYBTLDR_FROW_SIZE                   (CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE)
#endif

typedef struct _CYBTLDR_ENTER
{
    unsigned long SiliconId;
    unsigned char Revision;
    unsigned char BootLoaderVersion[3];

} CYBTLDR_ENTER;

typedef struct _CYBTLDR_ROW
{
    /* FLASH or EEPROM array id. */
    unsigned char ArrayID;

    /* Row number of the array. */
    unsigned short RowNumber;

} CYBTLDR_ROW;

typedef struct _CYBTLDR_SIZE
{
    unsigned short FirstRowNumber;
    unsigned short LastRowNumber;

} CYBTLDR_SIZE;

#define CYBTLDR_VERSION						{0x00, 0x00, 0x01} 


/* Link layer functions. */
int WritePacket(uint8 Command, uint8 * Data, uint16 Size);
cystatus CyBtLdrWriteRowData(uint8 arrayId, uint16 rowNumber, uint8 * rowData, uint16 rowSize);

#define SIZEOF_COMMAND_BUFFER               0x200
uint16 CommandDataSize;
uint8 CommandDataBuffer[SIZEOF_COMMAND_BUFFER];

/* Erase row for eraseing. */
uint8 EraseRow[CYBTLDR_FROW_SIZE];

/* Version of this chip and boot loader build. */
#if (defined(__C51__))
CYBTLDR_ENTER   BtLdrVersion = {CYSWAP_ENDIAN32(CYDEV_CHIP_JTAG_ID), CYDEV_CHIP_REV_EXPECT, CYBTLDR_VERSION};
#else
CYBTLDR_ENTER   BtLdrVersion = {CYDEV_CHIP_JTAG_ID, CYDEV_CHIP_REV_EXPECT, CYBTLDR_VERSION};
#endif

#define CYSWAP_ENDIAN16(x) ((uint16)((x << 8) | (x >> 8)))

/* Memory descriptions for flash and eeprom, Must be in little endian. */
#if (defined(__C51__))
CYBTLDR_SIZE BtLdrEESize = {0, CYSWAP_ENDIAN16((CYDEV_EE_SIZE / CYDEV_EEPROM_ROW_SIZE))};
CYBTLDR_SIZE BtLdrFlashSize = {0, CYSWAP_ENDIAN16(((CYDEV_FLS_SIZE / CYDEV_FLS_ROW_SIZE) - 1))};
#else
CYBTLDR_SIZE BtLdrEESize = {0, (CYDEV_EE_SIZE / CYDEV_EEPROM_ROW_SIZE)};
CYBTLDR_SIZE BtLdrFlashSize = {0, ((CYDEV_FLS_SIZE / CYDEV_FLS_ROW_SIZE) - 1)};
#endif
uint8 rowBuffer[CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE];


/*******************************************************************************
**
**
**
**
**
*******************************************************************************/
void CyBtLdrHostLink(uint16 TimeOut)
{
    uint8 AckNak = NAK;
    uint16 PktSize;
    uint8 PktCommand;
    uint16 XDATA Index;
    uint16 NumberRead;
    uint16 CheckSum;
    uint8 TData[4];
    uint32 Offset;
	uint8 LinkWaitState;

    /* Must be set at run time. */
#if (defined(__C51__))
	BtLdrFlashSize.FirstRowNumber = CYSWAP_ENDIAN16((*CyBtldr_SizeBytesAccess / CYDEV_FLS_ROW_SIZE));
#else
    BtLdrFlashSize.FirstRowNumber = (*CyBtldr_SizeBytesAccess / CYDEV_FLS_ROW_SIZE);
#endif

    /* Must get temperature and set a temp buffer first. */
    if(CYRET_SUCCESS != CySetTemp() || CYRET_SUCCESS != CySetFlashEEBuffer(rowBuffer))
    {
        /* Handle Error! */
    }

    /* Initialize communications channel. */
    CyBtldrCommStart();

    /* Make sure global interrupts are enabled. */
    CYGlobalIntEnable;

    /* Start with nothing. */
    CommandDataSize = 0;

    /* Setup the row for erasing. */    
    for(Index = 0;Index < CYBTLDR_FROW_SIZE; Index++)
    {
        EraseRow[Index] = 0xFF;
    }

	/* Do we wait for a command to come it? */
	LinkWaitState = (TimeOut == 0) ? 1:0;

    while(LinkWaitState || TimeOut--)
    {
        /* Keep track of the checksum. */
        ReadCheckSum = 0;

        /* Get packet header. */
        if(CyBtldrCommRead(TData, 1, &NumberRead, 10) || NumberRead != 1 || *TData != SOP)
        {
            continue;
        }

        /* Get Command. */
        if(CyBtldrCommRead(&PktCommand, 1, &NumberRead, 5) || NumberRead != 1)
        {
            continue;
        }

        /* Get data Size. */
        if(CyBtldrCommRead((uint8 *) &Index, 2, &NumberRead, 5) || NumberRead != 2)
        {
            continue;
        }

        /* Convert from little endian to either endian. */
        PktSize  = CY_GET_REG16(&Index);

        /* is the payload a valid size? */
        if(PktSize > (uint16) MAX_DATA_SIZE)
        {
            continue;
        }

        if((PktSize + CommandDataSize) > (uint16) SIZEOF_COMMAND_BUFFER)
        {
            continue;
        }

        /* Save this data for the next command. */
        if(PktSize && (CyBtldrCommRead(&CommandDataBuffer[CommandDataSize], PktSize, &NumberRead, 5) || NumberRead != PktSize))
        {
            continue;
        }

        /* Save the current check sum. */
        CheckSum = ReadCheckSum;
        CheckSum = 1 + ~CheckSum;

        /* Did we recieve a well formed packet? */
        if(CyBtldrCommRead(TData, 3, &NumberRead, 3) || NumberRead != 3 || EOP != TData[2])
        {
            continue;
        }

        /* Someone is trying to talk to us. */
		LinkWaitState = 1;

        /* Is the packet good but the data in the packet bad? */        
        if((TData[0] != ((uint8 *) &CheckSum )[LSB16]) ||  /* LO byte of checksum. */
           (TData[1] != ((uint8 *) &CheckSum )[MSB16]))    /* HI byte of checksum. */
        {
            /* BAD PACKET! Do some clean up before next packet. */

            /* Clean out the recieve buffer. */
            CyBtldrCommReset();

            /* Send NAK. */
            AckNak = ERROR_PACKET_CHECKSUM;
        }
        else if(NOT_VALID_COMMAND(PktCommand))
        {
            AckNak = CYRET_BAD_PARAM;

            /* Data associated with this function should be cleared. */
            CommandDataSize = 0;
        }
        else if(PktCommand == CYBTLDR_COMMAND_DATA)
        {
            /* We have part of a block of data. */
            AckNak = ACK;

            /* We have good data keep it. */
            CommandDataSize += PktSize;
        }
        else if(PktCommand == CYBTLDR_COMMAND_PROGRAM)
        {
            /* @#*&%@#, People design without understanding. */
            TData[0] = CommandDataBuffer[CommandDataSize];
            
            /* Convert from little endian to either endian. */
            Index = (uint16) (CommandDataBuffer[CommandDataSize + 1] | CommandDataBuffer[CommandDataSize + 2] << 8);

            PktSize -= 3;

            /* Move data down three bytes. */            
            cymemmove(&CommandDataBuffer[CommandDataSize], &CommandDataBuffer[3 + CommandDataSize], PktSize);

            /* We have good data keep it. */
            CommandDataSize += PktSize;
            
            /* Erase and Program the row. */
            if(CyBtLdrWriteRowData(TData[0], Index, CommandDataBuffer, CommandDataSize))
            {
                AckNak = ERROR_FLASH_ARRAY;
            }
            else
            {
                AckNak = ACK;
            }

            /* Data associated with this function should be cleared. */
            CommandDataSize = 0;
        }
        else if(PktCommand == CYBTLDR_COMMAND_ERASE)
        {
            PktSize = (CommandDataBuffer[0] > LAST_FLASH_ARRAYID) ? CYDEV_EEPROM_ROW_SIZE:CYBTLDR_FROW_SIZE;

            Index = (uint16) (CommandDataBuffer[1] | CommandDataBuffer[2] << 8);

            /* We have a row number to errase. */
            if(CyBtLdrWriteRowData(CommandDataBuffer[0], Index, EraseRow, PktSize))
            {
                AckNak = ERROR_FLASH_ARRAY;
            }
            else
            {
                AckNak = ACK;
            }

            /* Data associated with this function should be cleared. */
            CommandDataSize = 0;
        }
        else if(PktCommand == CYBTLDR_COMMAND_VERIFY)
        {
            /* Get Row number. */
            Index = (uint16) (CommandDataBuffer[1] | CommandDataBuffer[2] << 8);

            /* Read the existing flash data. */
            Offset = CYDEV_FLASH_BASE + ((uint32) Index * CYDEV_FLS_ROW_SIZE);
            for(TData[0] = 0, Index = 0; Index < CYDEV_FLS_ROW_SIZE; Index++)
            {
                TData[0] += CY_GET_XTND_REG8((uint8 CYFAR *) Offset + Index);
            }

#if (CYDEV_ECC_ENABLE == 0)

            /* Get Row number. */
            Index = (uint16) (CommandDataBuffer[1] | CommandDataBuffer[2] << 8);

            /* Save the ECC area. */
            Offset = CYDEV_ECC_BASE + ((uint32) Index * CYDEV_ECC_ROW_SIZE);
            for(Index = 0; Index < CYDEV_ECC_ROW_SIZE; Index++)
            {
                TData[0] +=  CY_GET_XTND_REG8((uint8 CYFAR *) Offset + Index);
            }

/* CYDEV_ECC_ENABLE == 0 */
#endif

            TData[0] = 1 + ~TData[0];

            WritePacket(ACK, (uint8 *) TData, CYBTLDR_SIZEOF_ROW_CHKSUM);

            /* The packet is the respoonse. */
            continue;
        }
        else if(PktCommand == CYBTLDR_COMMAND_SYNC)
        {
            AckNak = CYRET_SUCCESS;
            /* This command starts all transfers over. */
            /* If something failed the host would send */
            /* this command to reset the bootloader.   */
            CommandDataSize = 0;

            /* The packet is the respoonse. */
            continue;
        }
        else if(PktCommand == CYBTLDR_COMMAND_PROTECT)
        {
            AckNak = CYRET_BAD_PARAM;
        }
        else if(PktCommand == CYBTLDR_COMMAND_READ)
        {
            AckNak = CYRET_BAD_PARAM;
        }
        else if(PktCommand == CYBTLDR_COMMAND_TEST_CERTIFICATE)
        {
            TData[0] = *CY_BTLDR_P_CHECKSUM;
            WritePacket(ACK, TData, 1);
            
            /* The packet is the respoonse. */
            continue;
        }
        else if(PktCommand == CYBTLDR_COMMAND_REPORT_SIZE)
        {
            /* If we fail it will be because of a bad parameter. */
            AckNak = CYRET_BAD_PARAM;
            
            if(PktSize == 1)
            {
                if(*CommandDataBuffer == 0)
                {
                    WritePacket(ACK, (uint8 *) &BtLdrFlashSize, sizeof(CYBTLDR_SIZE));
                    continue;
                }
                else if (*CommandDataBuffer < FLASH_NUMBER_SECTORS)
                {				
                    #if (defined(__C51__))
                    CYBTLDR_SIZE arraySize = {0, CYSWAP_ENDIAN16(((CYDEV_FLS_SIZE / CYDEV_FLS_ROW_SIZE) - 1))};
                    #else
                    CYBTLDR_SIZE arraySize = {0, ((CYDEV_FLS_SIZE / CYDEV_FLS_ROW_SIZE) - 1)};
                    #endif
                    WritePacket(ACK, (uint8 *) &arraySize, sizeof(CYBTLDR_SIZE));
                    continue;
                }
                else if(*CommandDataBuffer == FIRST_EE_ARRAYID)
                {
                    WritePacket(ACK, (uint8 *) &BtLdrEESize, sizeof(CYBTLDR_SIZE));
                    continue;
                }
            }
        }
        else if(PktCommand == CYBTLDR_COMMAND_ENTER)
        {
            WritePacket(ACK, (uint8 *) &BtLdrVersion, sizeof(CYBTLDR_ENTER));
        
            /* This command starts all transfers over. */
            /* If something failed the host would send */
            /* this command to reset the bootloader.   */
            CommandDataSize = 0;

            /* The packet is the respoonse. */
            continue;
        }
        else if(PktCommand == CYBTLDR_COMMAND_EXIT)
        {
			uint8 XDATA *resetSR0;
			uint8 XDATA *resetCR2;
			resetSR0 = (uint8 XDATA*)CYDEV_RESET_SR0;
			resetCR2 = (uint8 XDATA*)CYDEV_RESET_CR2;
			*resetSR0 &= 0x3F; /* clear two MSB gpsw_s bits */
			*resetCR2 |= 0x01; /* set swr bit to cause a software reset */;
        }

        /* ?CK the packet and function. */
        WritePacket(AckNak, 0, 0);
    }
}


/*******************************************************************************
**
**                                                   
**                                                   
**
**
*******************************************************************************/
int WritePacket(uint8 Command, uint8 * Data, uint16 Size)
{
    uint8 Index;
    uint16 Count;
    uint16 CheckSum;
    static unsigned char TxPacket[MAX_PKT_SIZE];

    /* Keep track of number of bytes to transmit. */
    Index = 0;

    /* Start of the packet. */
    TxPacket[Index++] = SOP;

    /* Set the command. */
    TxPacket[Index++] = Command;

    CheckSum = SOP + Command;

    /* Set the size of the packet. */
    CheckSum += TxPacket[Index++] = ((uint8 *) &Size)[LSB16];
    CheckSum += TxPacket[Index++] = ((uint8 *) &Size)[MSB16];

    /* Copy the data. */
    while(Index < (4 + Size))
    {
        CheckSum += *Data;
        TxPacket[Index++] = *Data++;
    }

    /* Check Sum. Always little endian. */
    CheckSum = 1 + ~CheckSum;
    TxPacket[Index++] = ((uint8 *) &CheckSum)[LSB16];
    TxPacket[Index++] = ((uint8 *) &CheckSum)[MSB16];

    /* End of the packet. */
    TxPacket[Index++] = EOP;

    /* Start the packet transmit. */
    return (CyBtldrCommWrite(TxPacket, (uint16) Index, &Count, 150) || Count != Index) ? CYRET_UNKNOWN:CYRET_SUCCESS;
}


#define WHILE_TEST(t)           {register uint16 timeOut; for(timeOut = 0xFFFF; (t) && timeOut; timeOut--) {}}
/*******************************************************************************
* Function Name: CyBtLdrWriteRowData
********************************************************************************
* Summary:
*   Sends a command to the SPC to load and program a row of data in flash.
*
*
* Parameters:
*
*       arrayId: FLASH or EEPROM array id.
*
*       rowData: pointer to a row of data to write.
*
*       rowNumber: Zero based number of the row.
*
*       rowSize: Size of the row.
*   
* Return:
*   CYRET_SUCCESS if successful.
*   CYRET_LOCKED if the SPC is already in use.
*   CYRET_UNKNOWN if there was an SPC error.
*
*
*******************************************************************************/
cystatus CyBtLdrWriteRowData(uint8 arrayId, uint16 rowNumber, uint8 * rowData, uint16 rowSize)
{
    cystatus status;


    /* See if we can get the SPC. */
    if(CySpcLock() == CYRET_SUCCESS)
    {
        /* Create the command. */
        status = CySpcCreateCmdLoadRow(arrayId);
        if(status == CYRET_SUCCESS)
        {
            /* Write the command. */
            status = CySpcWriteCommand(rowData, rowSize);
            if(status == CYRET_STARTED)
            {
                /* Spin until completion. */
                WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));
                //while(!(*SPC_STATUS & SPC_SPC_IDLE));


                /* See if we were successful. */
                status = CySpcReadStatus;
            }
        }

        /* Create the command. */
        if(status == CYRET_SUCCESS)
        {
            status = CySpcCreateCmdWriteRow(arrayId, rowNumber, dieTemperature[0], dieTemperature[1]);
            if(status == CYRET_SUCCESS)
            {
                /* Write the command. */
                status = CySpcWriteCommand(0, 0);
                if(status == CYRET_STARTED)
                {
                    /* Spin until completion. */
                    WHILE_TEST(!(*SPC_STATUS & SPC_SPC_IDLE));

                    /* See if we were successful. */
                    status = CySpcReadStatus;
                }
            }
        }

        /* Unlock the SPC so someone else can use it. */
        CySpcUnlock();
    }
    else
    {
        status = CYRET_LOCKED;
    }

    return status;
}

