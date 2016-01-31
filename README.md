-----------
creekServer
-----------
jsp that displays the output of the creek database

to build it use 'ant compile' or 'ant'
to deploy it type 'and install'
the username and password is hardcoded in the build file... and needs to be replaced with the real one.  You can find them at /etc/tomcat6/tomcat-users.xml

All of the jsp files have the database user and pw in them... which is totally hosed and needs to be fixed.

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
* * * * * /home/pi/getCreek/printData

All of these files have the database user and pw in them... which is totally hosed and needs to be fixed.