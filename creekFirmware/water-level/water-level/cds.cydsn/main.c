
#include <device.h>
#include <stdio.h>

/*AMUX selections*/
#define Select_Single 0 
#define Select_Reference 1

/*IIR Filter parameters*/
#define IIR_FILTER_STEP 16
#define IIR_SHIFT 4

 /*variables for project*/
 int32 iVout1, iVout2, iVcds, iVcds_curr, iVcds_acc;
 char buff[20];

void main()
{

   	float fVcds;
   	uint8 iLoop;
   	uint16 i2buf;
   
   /*Initialization*/
   	CYGlobalIntEnable;
   	adc_Start();
   	amux_Start();
	lcd_Start();
	i2c_Start();
  	i2c_SetBuffer1(2,0,&i2buf);
	lcd_ClearDisplay();
	
	for(;;)
    {
		iVcds_acc = 0;
		for(iLoop =0; iLoop < IIR_FILTER_STEP; iLoop++)
		{
			/*Get the first sample Vout1*/
			amux_Select(0);
			adc_StartConvert();
			adc_IsEndConversion(adc_WAIT_FOR_RESULT);
			iVout1 = adc_GetResult32();  
			adc_StopConvert();
			
			/*Get the second sample Vout2*/
			amux_Select(1);
			adc_StartConvert();
			adc_IsEndConversion(adc_WAIT_FOR_RESULT);
			iVout2 = adc_GetResult32();
			adc_StopConvert();
			
			/*perform CDS*/
			iVcds = iVout1 - iVout2; 
			
			/* IIR Filter*/
			iVcds_curr = iVcds;
			iVcds_acc += (iVcds_curr - iVcds_acc) >> 4;
		}

		// put the adc value into the i2c buffer
		i2buf=iVout1;
		// display the i2c buffer
		lcd_Position(1,0);
		lcd_PrintString("A:");
		lcd_PrintInt16(i2buf);
	
		fVcds = adc_CountsTo_Volts(iVout1);

		sprintf(buff,"V:%1.3f ",(float)fVcds);
		lcd_Position(0,0);
		lcd_PrintString(buff);
	
		/*
		// calculate the voltage
		float nm = i2buf;
		float dn = 65536;
		float calc = (float)(nm/dn)  ;
		float mv = 2* 1.024 * calc ;
		sprintf(buff,"c:%1.3f ", mv);
		lcd_PrintString(buff);
		*/
		
		// calculate psi - range of current loop (4-->20ma) 
		// range of sensor 15 psi
		// resistor is 51.1 ohms
		
		float psi = (fVcds-(0.004*51.1)) / (0.020*51.1 - 0.004*51.1) * 15;
			
		sprintf(buff,"P:%2.2f ",psi);
		lcd_PrintString(buff);
	
		uint8 ft = (int)(psi / 0.43);
		uint8 inches = (int)(12*((psi / 0.43) - (float)ft)); 
		lcd_Position(1,8);
		lcd_PrintString("D:");
		lcd_PrintNumber(ft);
		lcd_PrintString("'");
		lcd_PrintNumber(inches);
		lcd_PrintString("\"   ");
		
		// Toggle the LED every time there is a read
		uint8 i2status = i2c_GetActivity();
		if(i2status & i2c_STATUS_READ1)
		{
				if(led_ReadDataReg())
					led_Write(0);
				else
					led_Write(1);
		}
		
    }
}