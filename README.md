-----------
creekServer
-----------

The creek server code has been obsoleted... and has been completely replace by Java that generates static
html content.

-------------
creekFirmware
-------------
The Creator project of the firmware.


----------
getCreek
---------
The program that runs on the rasperry PI.
reads the i2c
figures out what the adc value means
writes the data into the database on the RPI

to build
ant build

to install - (which basically copies the directory into the right place
ant install


To make the program run on the RPI you need to have 
crontab -l
0,2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,36,38,40,42,44,46,48,50,52,54,56,58 * * * * /home/pi/getCreek/runi2c
0,15,30,45 * * * * /home/pi/getCreek/processEvents

All of these files have the database user and pw in them... which is totally hosed and needs to be fixed.

The RPi has a directory called ~pi/getCreek/creekPlots which is built by the cron jobs
It is then copied to /var/lib/tomca7/webapps/creekServer