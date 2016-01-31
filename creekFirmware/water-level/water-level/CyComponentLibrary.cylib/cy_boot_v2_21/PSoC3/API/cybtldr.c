/*******************************************************************************
* File Name: cybtldr.c  
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
*  Description:
*   Provides an API for the Bootloader component. The API includes functions
*   for starting boot loading operations, validating the application and
*    jumping to the application.
*
*  Note: 
*   Documentation of the API's in this file is located in the
*   System Reference Guide provided with PSoC Creator.
*
*******************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


#include <cybtldr.h>
#include <device.h>
#include <string.h>

/*******************************************************************************
* Defines
********************************************************************************/
#define FIRST_APP_BYTE (CYDEV_FLS_ROW_SIZE * (*CY_BTLDR_P_LAST_BLDR_ROW + 1))

#define CYBTLDR_VERSION        {`$CY_MINOR_VERSION`, `$CY_MAJOR_VERSION`, 0x01} 

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
#define CYRET_ERR_UNK       0x0F  /* An unknown error occurred */

/* Bootloader command definitions. */
#define CYBTLDR_COMMAND_CHECKSUM    0x31    /* Verify the checksum for the bootloadable project. */
#define CYBTLDR_COMMAND_REPORT_SIZE 0x32    /* Report the programmable portions of flash */
#define CYBTLDR_COMMAND_ERASE       0x34    /* Erase the specified flash row */
#define CYBTLDR_COMMAND_SYNC        0x35    /* Sync the bootloader and host application */
#define CYBTLDR_COMMAND_DATA        0x37    /* Queue up a block of data for programming */
#define CYBTLDR_COMMAND_ENTER       0x38    /* Enter the bootloader */
#define CYBTLDR_COMMAND_PROGRAM     0x39    /* Program the specified row */
#define CYBTLDR_COMMAND_VERIFY      0x3A    /* Compute flash row checksum for verification */
#define CYBTLDR_COMMAND_EXIT        0x3B    /* Exits the bootloader & resets the chip */

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
#define MIN_PKT_SIZE                7

/* Our definition of a row size. */
#if (CYDEV_ECC_ENABLE == 1)
    #define CYBTLDR_FROW_SIZE       CYDEV_FLS_ROW_SIZE
#else
    #define CYBTLDR_FROW_SIZE       (CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE)
#endif

#define SIZEOF_COMMAND_BUFFER       300 /* Maximum number of bytes accepted in a packet plus some */
#if defined (WORKAROUND_OPT_XRES)
    uint8 flashBuffer[CYDEV_FLS_ROW_SIZE + CYDEV_ECC_ROW_SIZE];
#endif

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
* Function Name: CalcPacketChecksum
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
uint16 CalcPacketChecksum(uint8* buffer, uint16 size)
{
#if CYDEV_BOOTLOADER_CHECKSUM == CYDEV_BOOTLOADER_CHECKSUM_CRC
    uint16 CYDATA crc = 0xffff;
    uint16 CYDATA tmp;
    uint8  CYDATA i;
       
    if (size == 0)
    {
        return (~crc);
    }

    do
    {
        for (i = 0, tmp = 0x00ff & *buffer++; i < 8; i++, tmp >>= 1)
        {
            if ((crc & 0x0001) ^ (tmp & 0x0001))
            {
                crc = (crc >> 1) ^ 0x8408;
            }
            else
            {
                crc >>= 1;
            }
        }
    }
    while (--size);

    crc = ~crc;
    tmp = crc;
    crc = (crc << 8) | (tmp >> 8 & 0xFF);
       
    return crc;

#else /* CYDEV_BOOTLOADER_CHECKSUM == CYDEV_BOOTLOADER_CHECKSUM_BASIC */
    uint16 CYDATA sum = 0;
    while (size-- > 0)
    {
        sum += *buffer++;
    }
    
    return (1 + ~sum);
#endif
}


/*******************************************************************************
* Function Name: Calc8BitFlashSum
********************************************************************************
* Summary:
*   This computes the 8 bit sum for the provided number of bytes contained
*   in flash.
*
* Parameters:  
*   start:
*      The starting address to start summing data for
*   size:
*      The number of bytes to read and compute the sum for
*
* Returns:
*   8 bit sum for the provided data
*
*******************************************************************************/
uint8 Calc8BitFlashSum(uint32 start, uint32 size)
{
    uint8 CYDATA sum = 0;
    while (size-- > 0)
    {
        sum += CY_GETCODEDATA(start + size);
    }
    
    return sum;
}


/*******************************************************************************
* Function Name: CyBtldr_Start
********************************************************************************
* Summary:
*   Runs the bootloading algorithm, determining if a bootload is necessary and
*    launching the application if it is not.
*
* Parameters:
*   void:
*   
* Return:
*   This method will never return. It will either load a new application and reset
*    the device or it will jump directly to the existing application.
*
* Remark:
*    If this method determines that the bootloader appliation itself is corrupt,
*    this method will not return, instead it will simply hang the application.
*
*******************************************************************************/
void CyBtldr_Start(void)
{
    /* the bootloader always starts at 0 in flash */
    uint8 CYDATA calcedChecksum = Calc8BitFlashSum(0, *CyBtldr_SizeBytesAccess);
    calcedChecksum -= *CyBtldr_ChecksumAccess; /* we actually included the checksum, so remove it */
    calcedChecksum = 1 + ~calcedChecksum;
    
    if ((calcedChecksum != *CyBtldr_ChecksumAccess) || !(*CyBtldr_SizeBytesAccess))
    {
        CyBtldr_BadBootloader();
    }
    
#if defined (WORKAROUND_OPT_XRES)
    if (CyBtldr_ValidateApp() != CYRET_SUCCESS || (*CY_BTLDR_P_APP_RUN_ADDR) == CYBTLDR_START_BTLDR)
    {
        if (CYRET_SUCCESS != CySetTemp() || CYRET_SUCCESS != CySetFlashEEBuffer(flashBuffer))
        {
            CyBtldr_BadBootloader();
        }
        CyBtldr_SetFlashRunType(0);
#else
    if (CyBtldr_ValidateApp() || (CY_GET_REG8(CYREG_RESET_SR0) & CYBTLDR_START_BTLDR) == CYBTLDR_START_BTLDR)
    {
#endif
        CyBtldr_HostLink(0);
    }

#if CYDEV_BOOTLOADER_WAIT_COMMAND == 1
    CyBtldr_HostLink(CYDEV_BOOTLOADER_WAIT_TIME); /* Timeout is in 10s of miliseconds */
#endif
    CyBtldr_LaunchApplication();
}


/*******************************************************************************
* Function Name: CyBtldr_LaunchApplication
********************************************************************************
* Summary:
*   Jumps the PC to the start address of the user application in flash.
*
* Parameters:  
*   None
*
* Returns:
*   This method will never return if it succesfully goes to the user application.
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
        {
            LaunchApp(*CY_BTLDR_P_APP_ENTRY_POINT); /* we never return from this method */
        }
    }
}


/* Moves the arguement appAddr (RO) into PC, moving execution to the appAddr */
#if defined (__ARMCC_VERSION)
__asm void LaunchApp(uint32 appAddr)
{
    EXTERN CyResetStatus
    LDR R2, =CyResetStatus
    LDR R2, [R2]
    BX  R0;
}
#elif defined(__GNUC__)
__attribute__((noinline)) /* Workaround for GCC toolchain bug with inlining */
void LaunchApp(uint32 appAddr)
{
    __asm volatile(
        "    MOV R2, %0\n"
        "    LDR  R2, [R2]\n"
        "    BX  R0\n"
        : : "r" (CyResetStatus));
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
    uint32 CYDATA idx;
    uint32 CYDATA lastReadIdx = FIRST_APP_BYTE + *CY_BTLDR_P_APP_BYTE_LEN;
    uint8  CYDATA valid = 0; /* Assume bad flash image */
    uint8  CYDATA calcedChecksum = 0;
    
    for (idx = FIRST_APP_BYTE; idx < lastReadIdx; ++idx)
    {
        uint8 CYDATA curByte = CY_GETCODEDATA(idx);
        if (curByte != 0 && curByte != 0xFF)
        {
            valid = 1;
        }

        calcedChecksum += curByte;
    }

    calcedChecksum = 1 + ~calcedChecksum;
    if (calcedChecksum != *CY_BTLDR_P_CHECKSUM || !valid)
    {
        return CYRET_BAD_DATA;
    }
    
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
    uint16 CYDATA numberRead;
    uint16 CYDATA rspSize;
    uint8 CYDATA ackCode;
    uint16 CYDATA pktChecksum;
    uint16 CYDATA pktSize = 0;
    uint8 CYDATA clearedMetaData = 0;
    uint16 CYDATA dataOffset = 0;
    uint8 CYDATA communicationActive = (timeOut == 0);
    uint8 packetBuffer[SIZEOF_COMMAND_BUFFER];
    uint8 dataBuffer[SIZEOF_COMMAND_BUFFER];

    /* Must get temperature and set a temp buffer first. */
    if (CYRET_SUCCESS != CySetTemp())
    {
        CyBtldr_BadBootloader();
    }
    if (timeOut == 0)
    {
        timeOut = 0xFF;
    }

    /* Initialize communications channel. */
    CyBtldrCommStart();

    /* Make sure global interrupts are enabled. */
    CyGlobalIntEnable;

    do
    {
        ackCode = CYRET_SUCCESS;
        if (CyBtldrCommRead(packetBuffer, SIZEOF_COMMAND_BUFFER, &numberRead, timeOut) == CYRET_EMPTY)
        {
            continue;
        }

        if (numberRead < MIN_PKT_SIZE || packetBuffer[CYBTLDR_SOP_ADDR] != SOP)
        {
            ackCode = CYRET_ERR_DATA;
        }
        else
        {
            pktSize = CY_GET_REG16(&packetBuffer[CYBTLDR_SIZE_ADDR]);
            pktChecksum = CY_GET_REG16(&packetBuffer[CYBTLDR_CHK_ADDR(pktSize)]);

            if ((pktSize + MIN_PKT_SIZE) > numberRead)
            {
                ackCode = CYRET_ERR_LENGTH;
            }
            else if (packetBuffer[CYBTLDR_EOP_ADDR(pktSize)] != EOP)
            {
                ackCode = CYRET_ERR_DATA;
            }
            else if (pktChecksum != CalcPacketChecksum(packetBuffer, pktSize + CYBTLDR_DATA_ADDR))
            {
                ackCode = CYRET_ERR_CHECKSUM;
            }
        }

        rspSize = 0;
        if (ackCode == CYRET_SUCCESS)
        {
            uint8 CYDATA btldrData = packetBuffer[CYBTLDR_DATA_ADDR];

            ackCode = CYRET_ERR_DATA;
            switch (packetBuffer[CYBTLDR_CMD_ADDR])
            {
                case CYBTLDR_COMMAND_CHECKSUM:
                    if (communicationActive && pktSize == 0)
                    {
                        rspSize = 1;
                        packetBuffer[CYBTLDR_DATA_ADDR] = (CyBtldr_ValidateApp() == CYRET_SUCCESS);
                        ackCode = CYRET_SUCCESS;
                    }
                    break;

                case CYBTLDR_COMMAND_REPORT_SIZE:
                    if (communicationActive && pktSize == 1)
                    {
                        CYBTLDR_SIZE CYDATA arraySize;

                        if (btldrData < FLASH_NUMBER_SECTORS)
                        {
                            uint16 CYDATA startRow = (btldrData == 0)
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
                        else if (btldrData == FIRST_EE_ARRAYID)
                        {
                            arraySize.FirstRow = 0;
                            #if (defined(__C51__))
                                arraySize.LastRow = CYSWAP_ENDIAN16(CYDEV_EE_SIZE / CYDEV_EEPROM_ROW_SIZE);
                            #else
                                arraySize.LastRow = (CYDEV_EE_SIZE / CYDEV_EEPROM_ROW_SIZE);
                            #endif
                        }

                        rspSize = sizeof(CYBTLDR_SIZE);
                        memcpy(&packetBuffer[CYBTLDR_DATA_ADDR], &arraySize, rspSize);
                        ackCode = CYRET_SUCCESS;
                    }
                    break;

                case CYBTLDR_COMMAND_ERASE:
                    if (communicationActive && pktSize != 3)
                    {
                        break;
                    }
                    dataOffset = CYBTLDR_FROW_SIZE;
                    memset(dataBuffer, 0, CYBTLDR_FROW_SIZE);
                    /* Fall through to write the row */

                case CYBTLDR_COMMAND_PROGRAM:
                    if (communicationActive && pktSize >= 3)
                    {
                        memcpy(&dataBuffer[dataOffset], &packetBuffer[CYBTLDR_DATA_ADDR + 3], pktSize - 3);
                        dataOffset += (pktSize - 3);

                        pktSize = (btldrData > LAST_FLASH_ARRAYID) 
                            ? CYDEV_EEPROM_ROW_SIZE
                            : CYBTLDR_FROW_SIZE;

                        if (dataOffset == pktSize)
                        {
                            if (!clearedMetaData)
                            {
                                uint8 erase[CYBTLDR_FROW_SIZE];
                                memset(erase, 0, CYBTLDR_FROW_SIZE);
                                CyWriteRowFull(CY_BTLDR_MD_ARRAY, CY_BTLDR_MD_ROW, erase, CYBTLDR_FROW_SIZE);
                                clearedMetaData = 1;
                            }

                            ackCode = (CyWriteRowFull(btldrData, CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])), dataBuffer, pktSize))
                                ? CYRET_ERR_ROW
                                : CYRET_SUCCESS;
                        }
                        else
                        {
                            ackCode = CYRET_ERR_LENGTH;
                        }
                        dataOffset = 0;
                    }
                    break;

                case CYBTLDR_COMMAND_SYNC:
                    /* If something failed the host would send this command to reset the bootloader. */
                    dataOffset = 0;
                    continue; /* Don't ack the packet, just get ready to accept the next one */

                case CYBTLDR_COMMAND_DATA:
                    /* We have part of a block of data. */
                    ackCode = CYRET_SUCCESS;
                    memcpy(&dataBuffer[dataOffset], &packetBuffer[CYBTLDR_DATA_ADDR], pktSize);
                    dataOffset += pktSize;
                    break;

                case CYBTLDR_COMMAND_ENTER:
                    if (pktSize == 0)
                    {
                        #if (defined(__C51__))
                            CYBTLDR_ENTER CYDATA BtldrVersion = 
                                {CYSWAP_ENDIAN32(CYDEV_CHIP_JTAG_ID), CYDEV_CHIP_REV_EXPECT, CYBTLDR_VERSION};
                        #else
                            CYBTLDR_ENTER CYDATA BtldrVersion = 
                                {CYDEV_CHIP_JTAG_ID, CYDEV_CHIP_REV_EXPECT, CYBTLDR_VERSION};
                        #endif
                        communicationActive = 1;
                        rspSize = sizeof(CYBTLDR_ENTER);
                        memcpy(&packetBuffer[CYBTLDR_DATA_ADDR], &BtldrVersion, rspSize);
                        ackCode = CYRET_SUCCESS;
                    }
                    break;

                case CYBTLDR_COMMAND_VERIFY:
                    if (communicationActive && pktSize == 3)
                    {
                        /* Read the existing flash data. */
                        uint32 CYDATA rowAddr = ((uint32)btldrData * CYDEV_FLS_SECTOR_SIZE) 
                            + ((uint32)CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])) * CYDEV_FLS_ROW_SIZE);
                        uint8 CYDATA checksum = Calc8BitFlashSum(rowAddr, CYDEV_FLS_ROW_SIZE);

                        #if (CYDEV_ECC_ENABLE == 0)
                            uint16 CYDATA index;
                            /* Save the ECC area. */
                            rowAddr = CYDEV_ECC_BASE + ((uint32)btldrData * CYDEV_ECC_SECTOR_SIZE) 
                                + ((uint32)CY_GET_REG16(&(packetBuffer[CYBTLDR_DATA_ADDR + 1])) * CYDEV_ECC_ROW_SIZE);
                            for (index = 0; index < CYDEV_ECC_ROW_SIZE; index++)
                            {
                                checksum += CY_GET_XTND_REG8((uint8 CYFAR *)(rowAddr + index));
                            }
                        #endif

                        packetBuffer[CYBTLDR_DATA_ADDR] = 1 + ~checksum;
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
    uint16 CYDATA checksum;

    /* Start of the packet. */
    buffer[CYBTLDR_SOP_ADDR] = SOP;
    buffer[CYBTLDR_CMD_ADDR] = status;
    buffer[CYBTLDR_SIZE_ADDR] = LO8(size);
    buffer[CYBTLDR_SIZE_ADDR + 1] = HI8(size);

    /* Compute the checksum. */
    checksum = CalcPacketChecksum(buffer, size + CYBTLDR_DATA_ADDR);

    buffer[CYBTLDR_CHK_ADDR(size)] = LO8(checksum);
    buffer[CYBTLDR_CHK_ADDR(1 + size)] = HI8(checksum);
    buffer[CYBTLDR_EOP_ADDR(size)] = EOP;

    /* Start the packet transmit. */
    return CyBtldrCommWrite(buffer, size + MIN_PKT_SIZE, &size, 150);
}
