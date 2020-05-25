#include <stdio.h>
#include "cybtldr_api.h"
#include "cybtldr_api2.h"
#include <unistd.h>
#include "bcm2835.h"
#include "bcm2835_i2cbb.h"
#include <limits.h>


#define P4RESET 17

CyBtldr_CommunicationsData cd;

struct bcm2835_i2cbb ibb;

int CYPIOpenConnection(void);
int CYPICloseConnection(void);
int CYPIReadData(unsigned char *, int);
int CYPIWriteData(unsigned char *, int );
int CYPIReset(void);
void PUpdate(unsigned char , unsigned short );

int CYPISetup(void)

{

  printf("Starting Setup\n");  

  //setup reset pin
    if(!bcm2835_init())
      {
	printf("init failed\n");
	exit(0);
      }

    bcm2835_gpio_fsel(P4RESET, BCM2835_GPIO_FSEL_OUTP);
    bcm2835_gpio_clr(P4RESET);

  cd.OpenConnection = &CYPIOpenConnection;

  cd.CloseConnection = &CYPICloseConnection;
  cd.ReadData = &CYPIReadData;
  cd.WriteData = &CYPIWriteData;
  //  cd.MaxTransferSize = 100;
  cd.MaxTransferSize = 20;

  printf("Ending Setup\n");

  return CYRET_SUCCESS;
  

}


int CYPIOpenConnection(void)
{

  printf("Starting connection\n");


 
  //  bcm2835_i2cbb_open(&ibb,0x10,2,3,250,1000000);
   if(bcm2835_i2cbb_open(&ibb,0x08,2,3,300,1e6)) exit(1);


  printf("finished setup\n");
  return CYRET_SUCCESS;

}

int CYPICloseConnection(void)
{

  printf("Closing connection\n");
    bcm2835_close();    

  return CYRET_SUCCESS;
}

int CYPIReadData(unsigned char *inbuff, int size)
{
  int i;

  printf("Reading %d\n",size);


  bcm8235_i2cbb_gets(&ibb,inbuff,size);

   for(i=0;i<size;i++)
     printf("%x ",inbuff[i]);
   printf("\n");


  return CYRET_SUCCESS;


}
int CYPIWriteData(unsigned char *outbuff, int size) 
{
  int i;


  printf("Writing %d bytes\n",size);
  for(i=0;i<size;i++)
    printf("%x ",outbuff[i]);

  printf("\n");

  bcm8235_i2cbb_puts(&ibb,outbuff,size);

  delay(100);
  return CYRET_SUCCESS;  

}

// reset the Raspberry pi
int CYPIReset(void)
{
	printf("Started reset\n");

    printf("start reset\n");

    bcm2835_gpio_set(17);
    printf("write pin\n");
    delay(1000);
    bcm2835_gpio_clr(17);


    printf("done with reset\n");
    delay(1000);


	return CYRET_SUCCESS;
	
}




void PUpdate(unsigned char arrayId, unsigned short rowNum)
{
  printf("Array =%d row = %d\n",arrayId, rowNum);
}


int main(int argc, char **argv)
{

  int rval;

  unsigned char buf[100];
  int j;

  CYPISetup();
  CYPIReset();
   CyBtldr_Action action = PROGRAM;
  rval = CyBtldr_RunAction(action, "p4arduino-creek.cyacd", &cd , &PUpdate);
  


  switch(rval)
    {
    case CYRET_SUCCESS:
      printf("Succeeded\n");
      break;
      /*    case CYRET_ERR_VERIFY:
      printf("CYRET_ERR_VERIFY:Verification of flash failed\n");
      break;*/
    case CYRET_ERR_LENGTH:
      printf("CYRET_ERR_LENGTH:The amount of data is outsdie range\n");
      break;

    case CYRET_ERR_DATA:
      printf("CYRET_ERR_DATA:the data is not of proper form\n");
      break;

    case CYRET_ERR_CMD:
      printf("CYRET_ERR_CMD:Command not recognized\n");
      break;

    case CYRET_ERR_DEVICE:
      printf("CYRET_ERR_DEVICE:The expected device does not match detected device\n");
      break;

    case CYRET_ERR_VERSION:
      printf("CYRET_ERR_VERSION:The bootloader version detected is not supported\n");
      break;

    case CYRET_ERR_CHECKSUM:
      printf("CYRET_ERR_Checksum:The checksum does not match the expected value\n");
      break;


    case CYRET_ERR_ARRAY:
      printf("CYRET_ERR_array:the flash array id is not valid\n");
      break;

    case CYRET_ERR_ROW:
      printf("CYRET_ERR_ROW:The flash row number is not valid\n");
      break;

      /*    case CYRET_ERR_APP:
      printf("CYRET_ERR_APP:The application is not valid\n");
      break;
      */
    case CYRET_ERR_ACTIVE:
      printf("CYRET_ERR_ACTIVE:the application is currently marked as active.\n");
      break;


    case CYRET_ERR_UNK:
      printf("CYRET_ERR_UNK:An unknown error occurred.\n");
      break;


    }
  printf("Rval =%d\n",rval);
  return rval;


}


