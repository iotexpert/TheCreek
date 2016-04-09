/* ========================================
 *
 * Copyright YOUR COMPANY, THE YEAR
 * All Rights Reserved
 * UNPUBLISHED, LICENSED SOFTWARE.
 *
 * CONFIDENTIAL AND PROPRIETARY INFORMATION
 * WHICH IS THE PROPERTY OF your company.
 *
 * ========================================
*/
#include <project.h>

 typedef  struct DataPacket {
    
    int8 signedByte;
    uint8 unsignedByte;
    int16 signedInt;
    uint16 unsignedInt;
    float floatVariable;
    double doubleVariable;
   
    
} __attribute__((packed)) DataPacket;

DataPacket dp;

int main()
{
    dp.signedByte = -123;
    dp.unsignedByte = 234;
    dp.signedInt = -12345;
    dp.unsignedInt = 54321;
    dp.floatVariable = 1234.567;
    dp.doubleVariable = 7654321.1234;
    
    CyGlobalIntEnable; /* Enable global interrupts. */

    EZI2C_Start();
	EZI2C_EzI2CSetBuffer1(sizeof(dp),0,(uint8 *)&dp);
    /* Place your initialization/startup code here (e.g. MyInst_Start()) */

    for(;;)
    {
        /* Place your application code here. */
    }
}

/* [] END OF FILE */
