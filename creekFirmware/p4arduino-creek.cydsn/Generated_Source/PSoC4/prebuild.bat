@REM This script allows a 3rd party IDE to use CyElfTool to perform
@REM the pre processing that is necessary to extract the bootloader
@REM data from the *.elf file for use when building the bootloadable
@REM application.
@REM NOTE: This script is auto generated. Do not modify.


chdir ..\.\Generated_Source\PSoC4
@IF %errorlevel% NEQ 0 EXIT /b %errorlevel% 
CyElfTool.exe -E "..\..\..\p4bootloader.cydsn\CortexM0\ARM_GCC_473\Debug\p4bootloader.elf" --flash_row_size 128 --flash_array_size 32768 --flash_size 32768
@IF %errorlevel% NEQ 0 EXIT /b %errorlevel% 
