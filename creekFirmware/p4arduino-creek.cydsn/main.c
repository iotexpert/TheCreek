#include <device.h>

 typedef  struct DataPacket {
    uint16 pressureCounts;
    int16 centiTemp; // temp in degree C / 100
    float depth;
    float temperature;
} __attribute__((packed)) DataPacket;

#define PRESSURE_CHAN (0)
#define TMP036_CHAN (1)

int updateFlag = 0;
int updateCount = 0;

void updateI2CData()
{
    if(updateCount++ > 10000) // update the buffer every 10s
    {
        updateFlag = 1;
        updateCount = 0;
    }
}

int main()
{   
    DataPacket dp;
    DataPacket i2cdp;
    
    float previousTemp;
    float previousDepth;
    
    CyGlobalIntEnable;
	
	ezi2c_Start();
	ezi2c_EzI2CSetBuffer1(sizeof(dp),0,(uint8 *)&i2cdp);
	
	adc_Start();
    adc_StartConvert();
    
    CySysTickStart();
    CySysTickSetCallback(0,updateI2CData);
    
    for(;;)
    {
        if(adc_IsEndConversion(adc_RETURN_STATUS))
        {
            dp.pressureCounts = adc_GetResult16(PRESSURE_CHAN);
            // 408 is the baseline 0 with no pressure = 51.1ohm * 4ma * 2mv/count
            // 3.906311 is the conversion to ft
            // Whole range in counts = (20mA - 4mA)* 51.1 ohm * 2 mv/count = 1635.2 counts
            // Range in Feet = 15PSI / 0.42PSI/Ft = 34.88 Ft
            // Count/Ft = 1635.2 / 34.88 = 46.8807 Counts/Ft 
            
            float depth =( ((float)dp.pressureCounts)-408)/46.8807;
            
            // Apply the USC correction model
            depth = 1.0106 * depth + 1.5589;
            
            dp.depth = ( depth + 7*previousDepth ) / 8.0;  // IIR Filter
            
            if(dp.depth<0.0)
                dp.depth=0.0;
            
            int tempMv = adc_CountsTo_mVolts(TMP036_CHAN,adc_GetResult16(TMP036_CHAN));
            dp.centiTemp = 10*tempMv - 5000; 
            
            dp.temperature = (((float)dp.centiTemp / 100.0) + 7*previousTemp ) / 8  ; // IIR Filter
            
            previousTemp = dp.temperature;
            previousDepth = dp.depth;
           
            if(updateFlag)
            {
                // turn off I2C Interrupt while updating the buffer
                uint8 cs = CyEnterCriticalSection(); 
		        if(ezi2c_EzI2CGetActivity() != ezi2c_EZI2C_STATUS_BUSY)
                {
                    i2cdp.centiTemp = dp.centiTemp;
                    i2cdp.depth = dp.depth;
                    i2cdp.pressureCounts = dp.pressureCounts;
                    i2cdp.temperature = dp.temperature;
                    updateFlag = 0;    
                }
                CyExitCriticalSection(cs); // turn the interrupts back on
            }
        }			
    }
}
