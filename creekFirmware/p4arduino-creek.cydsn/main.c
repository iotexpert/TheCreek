#include <device.h>

 typedef  struct DataPacket {
    uint16 pressureCounts;
    int16 centiTemp; // temp in degree C / 100
    float depth;
    float temperature;
} __attribute__((packed)) DataPacket;

#define PRESSURE_CHAN (0)
#define TMP036_CHAN (1)

int main()
{   
    DataPacket dp;
    
    float previousTemp;
    float previousDepth;
    
    CyGlobalIntEnable;
	
	ezi2c_Start();
	ezi2c_EzI2CSetBuffer1(sizeof(dp),0,(uint8 *)&dp);
	
	adc_Start();
    adc_StartConvert();
    
    for(;;)
    {
        if(adc_IsEndConversion(adc_RETURN_STATUS))
        {
            uint8 cs = CyEnterCriticalSection(); // turn off I2C Interrupt while updating the buffer
		    dp.pressureCounts = adc_GetResult16(PRESSURE_CHAN);
            //408 is the baseline 0 with no pressure = 51.1ohm * 4ma * 2mv/count
            // 3.906311 is the conversion to ft
            // Whole range in counts = (20mA - 4mA)* 51.1 ohm * 2 mv/count = 1635.2 counts
            // Range in Feet = 15PSI / 0.42PSI/Ft = 34.88 Ft
            // Count/Ft = 1635.2 / 34.88 = 46.8807 Counts/Ft 
            dp.depth = (((((float)dp.pressureCounts)-408)/46.8807) + 7*previousDepth ) / 8.0;  // IIR Filter
            
            int tempMv = adc_CountsTo_mVolts(TMP036_CHAN,adc_GetResult16(TMP036_CHAN));
            dp.centiTemp = 10*tempMv - 5000; 
            
            dp.temperature = (((float)dp.centiTemp / 100.0) + 7*previousTemp ) / 8  ; // IIR Filter
            
            
            previousTemp = dp.temperature;
            previousDepth = dp.depth;
            
            CyExitCriticalSection(cs); // turn the interrupts back on
        }			
    }
}
