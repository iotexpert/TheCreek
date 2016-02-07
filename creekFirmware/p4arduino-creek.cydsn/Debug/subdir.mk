################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../main.c 

OBJS += \
./main.o 

C_DEPS += \
./main.d 


# Each subdirectory must supply rules for building sources it contributes
%.o: ../%.c
	@echo 'Building file: $<'
	@echo 'Invoking: Cross GCC Compiler (PSoC Creator)'
	arm-none-eabi-gcc -DDEBUG -I"C:\Users\arh\Desktop\CreekFirmware\p4arduino-creek.cydsn" -I"C:\Users\arh\Desktop\CreekFirmware\p4arduino-creek.cydsn\Generated_Source\PSoC4" -O2 -ffunction-sections -g -Wall -c -fmessage-length=0 -mcpu=cortex-m0 -mthumb -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -o "$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '


