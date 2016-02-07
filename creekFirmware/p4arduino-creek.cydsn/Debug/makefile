################################################################################
# Automatically-generated file. Do not edit!
################################################################################

-include ../makefile.init

RM := rm -rf

# All of the sources participating in the build are defined here
-include sources.mk
-include Generated_Source/PSoC4/subdir.mk
-include subdir.mk
-include objects.mk

ifneq ($(MAKECMDGOALS),clean)
ifneq ($(strip $(C_DEPS)),)
-include $(C_DEPS)
endif
endif

-include ../makefile.defs

# Add inputs and outputs from these tool invocations to the build variables 

# All Target
all: pre-build main-build

# Main-build Target
main-build: test1

# Tool invocations
test1: $(OBJS) $(USER_OBJS)
	@echo 'Building target: $@'
	@echo 'Invoking: Cross GCC Linker (PSoC Creator)'
	arm-none-eabi-gcc -T ../Generated_Source/PSoC4/cm0gcc.ld -specs=nano.specs -Xlinker --gc-sections -march=armv6-m -mthumb -o "test1" $(OBJS) $(USER_OBJS) $(LIBS)
	@echo 'Finished building target: $@'
	@echo ' '
	$(MAKE) --no-print-directory post-build

# Other Targets
clean:
	-$(RM) $(OBJS)$(C_DEPS)$(EXECUTABLES) test1
	-@echo ' '

pre-build:
	-"../Generated_Source/PSoC4/prebuild.bat"
	-@echo ' '

post-build:
	-"../Generated_Source/PSoC4/postbuild.bat" "../../Debug/test1"
	-@echo ' '

.PHONY: all clean dependents
.SECONDARY: main-build pre-build post-build

-include ../makefile.targets
