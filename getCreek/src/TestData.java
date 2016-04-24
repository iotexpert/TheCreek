
import java.io.FileInputStream;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.Statement;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.Properties;
import java.util.concurrent.TimeUnit;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author arh
 */
public class TestData {

    static Properties prop;
    static boolean countup = true;
    static double currentDepth = 0.0;
    static double currentTemp = 20.0;
    static double depthIncrement = 0.1;
    static double depthMax = 23.0;

      
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        if((args.length != 1) || (!(args[0].equals("insertHours") || args[0].equals("insertDelay"))))
        {
            System.out.println("insertHours - insert 10 hours of data");
            System.out.println("insertDelay - insert a datapoint 1/minute");
            return;
        }
        
                    
        LocalDateTime ldt = LocalDateTime.now();
        System.out.println("Starting Time =" + ldt);
        StartEnd pair;

        try {
            readProperties();
            if(args[0].equals("insertHours"))
                    insertHoursData(10);
            if(args[0].equals("insertDelay"))
                    delayInsert();

        } catch (Exception e) {
            System.out.println(e);
            return;
        }

    }
    
    static void delayInsert() throws Exception
    {
        while(true)
        {
            LocalDateTime now = LocalDateTime.now();
 
            System.out.println("Time = "+ now + " Depth="+currentDepth);
            newEventInsert(Timestamp.valueOf(now), currentDepth, currentTemp);
            nextDepthPoint();
            TimeUnit.MINUTES.sleep(1);
            //TimeUnit.SECONDS.sleep(5);
        }
        
    }
    
    static void nextDepthPoint() {
        if (countup) {
            currentDepth += depthIncrement;
        } else {
            currentDepth -= depthIncrement;
        }

        if (currentDepth > depthMax) {
            countup = false;
        }
        if (currentDepth <= 0.1) {
            countup = true;
        }

    }

    static void insertHoursData(int hours) {
        try {

            System.out.println("InsertData");

            LocalDateTime now = LocalDateTime.now();
            LocalDateTime currentLoop = now.minusHours(hours);

            System.out.println("Time = " + currentLoop);

            while (currentLoop.isBefore(now)) {

                newEventInsert(Timestamp.valueOf(currentLoop), currentDepth, currentTemp);
                //System.out.println("Time = " + currentLoop + " Depth=" + currentDepth);
                nextDepthPoint();
                currentLoop = currentLoop.plusMinutes(1);

            }
        } catch (Exception e) {
        }
    }
    
    /// This function read the properties file.    
    static void readProperties() throws Exception {
        prop = new Properties();
        FileInputStream fis;
        fis = new FileInputStream("config.properties");
        prop.load(fis);
    }
    
    static void newEventInsert(Timestamp time,double depth, double temperature) throws Exception {
        //System.out.println("Inserting new event" + pair.start + "  " + pair.end);
        String url = prop.getProperty("dburl");
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(url, prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String insert;
        
        insert = "insert into creekdata.creekdata (created_at,depth,temperature) values ('" + time + "'," + depth + ","+ temperature + ")";
        
        System.out.println(insert);
        Statement stmt = con.createStatement();
        stmt.executeUpdate(insert);
        con.close();

    }
    
}
