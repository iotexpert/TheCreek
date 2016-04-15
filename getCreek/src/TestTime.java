/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author arh
 */

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.Month;
import java.time.ZoneId;
import java.time.ZoneOffset;
import java.time.format.DateTimeFormatter;
import java.util.Date;
import java.util.Locale;

public class TestTime {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        
//Current Date
        LocalDateTime today = LocalDateTime.now();
        System.out.println("Current DateTime="+today);
        
        System.out.println("Hour="+today.getHour());
        
        LocalDateTime eightHoursAgo = today.minusHours(8);
        System.out.println("Eight Hours Ago ="+eightHoursAgo);
        //System.out.println("Getlong="+today.getLong());
         
        //Current Date using LocalDate and LocalTime
        today = LocalDateTime.of(LocalDate.now(), LocalTime.now());
        System.out.println("Current DateTime="+today);
         
        //Creating LocalDateTime by providing input arguments
        LocalDateTime specificDate = LocalDateTime.of(2014, Month.JANUARY, 1, 10, 10, 30);
        System.out.println("Specific Date="+specificDate);
         
         
        //Try creating date by providing invalid inputs
        //LocalDateTime feb29_2014 = LocalDateTime.of(2014, Month.FEBRUARY, 28, 25,1,1);
        //Exception in thread "main" java.time.DateTimeException: 
        //Invalid value for HourOfDay (valid values 0 - 23): 25
 
         
        //Current date in "Asia/Kolkata", you can get it from ZoneId javadoc
        LocalDateTime todayKolkata = LocalDateTime.now(ZoneId.of("Asia/Kolkata"));
        System.out.println("Current Date in IST="+todayKolkata);
 
        //java.time.zone.ZoneRulesException: Unknown time-zone ID: IST
        //LocalDateTime todayIST = LocalDateTime.now(ZoneId.of("IST"));
         
        //Getting date from the base date i.e 01/01/1970
        LocalDateTime dateFromBase = LocalDateTime.ofEpochSecond(10000, 0, ZoneOffset.UTC);
        System.out.println("10000th second time from 01/01/1970= "+dateFromBase);

    }
    
}
