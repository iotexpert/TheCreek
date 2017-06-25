/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 *
 * @author arh
 */

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintStream;
import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.time.LocalDateTime;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

public class CreekHistory {

    private Properties prop;
  
    private String dburl;
    private String dbuser;
    private String dbpassword;

    public int count=-1;
    
    public Double depth[];
    public Double temperature[];
   
    public Double depthDelta[];
    public Double temperatureDelta[];
    
    public Timestamp startTime=null;
    
    public static final Integer timeIncrementMinutes = 15;
    private static final Integer timeIncrementMs = 15 * 60 * 1000 ;
    private static final Integer numHours = 3;
    private static final Integer maxCount = (numHours * 60 / timeIncrementMinutes) + 1;
    
    //public JspWriter debugOut=null;

    public PrintStream debugOut;
    
    public CreekHistory()
    {
        readProperties();
        depth = new Double[maxCount];
        temperature = new Double[maxCount];
        depthDelta = new Double[maxCount];
        temperatureDelta = new Double[maxCount];

    }

    public void loadData() throws Exception
    {
        
        LocalDateTime searchWindow = LocalDateTime.now().minusHours(numHours+1);
        Timestamp ldt = Timestamp.valueOf(searchWindow);
        
        
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");

        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));

        String query;
   
        query = "select created_at,depth,temperature from creekdata.creekdata where created_at>'" + ldt + "' order by created_at desc";
        if(debugOut != null)
            debugOut.println(query + "<br>");
        
        Statement st = con.createStatement();
        long timeOffset = 0;
        
        try {
            ResultSet rs = st.executeQuery(query);
            
            Timestamp currentTime = null;
            Timestamp previousTime = null;
            
            Timestamp targetTime = new Timestamp(0);
            
            double previousDepth = 0.0;
            double previousTemperature = 0;
                    
            while(rs.next()) {
                
                Timestamp rowTime = rs.getTimestamp("created_at");
                double rowDepth = rs.getDouble("depth");
                double rowTemperature = cToF(rs.getDouble("temperature"));
 
                if(startTime == null)
                    startTime = rowTime;
                
                if(count == -1)
                {
                    count = 0;
                    depth[count] = rowDepth;
                    temperature[count] = rowTemperature;
                    
                    depthDelta[count] = depth[count] - depth[0];
                    temperatureDelta[count] = temperature[count] - temperature[0];
                    
                    targetTime = new Timestamp(rowTime.getTime()-timeIncrementMs);
                    
                    previousDepth = rowDepth;
                    previousTemperature = rowTemperature;
                    previousTime = rowTime;
                    
                    if(debugOut != null)
                    {
                        debugOut.println("Firstcount <br>");
                        debugOut.println("Count = "+count + " TargetTime = "+targetTime + " RowTime = "+rowTime 
                                + " previousRow ="+previousTime + " Depth =" + depth[count]+"<br>");
                    }
                    count = count + 1;       
                    continue;
                }
                
                if(rowTime.getTime() < targetTime.getTime())
                {
                    depth[count] = interpolate(previousTime,previousDepth,rowTime,rowDepth,targetTime);
                    temperature[count] = interpolate(previousTime,previousTemperature,rowTime,rowTemperature,targetTime);
                    depthDelta[count] = depth[0] - depth[count];
                    temperatureDelta[count] =  temperature[0] - temperature[count];
                    
                    if(debugOut != null) 
                        debugOut.println("Count = "+count + " TargetTime = "+targetTime + " RowTime = "+rowTime 
                                + " previousRow ="+previousTime + " Depth =" + depth[count]+" Temp="+temperature[count]+"<br><br><br>");
                    
                    count = count + 1;
                    targetTime = new Timestamp(targetTime.getTime()-timeIncrementMs);
                
                }
                
                if(count == maxCount)
                    break;

                previousDepth = rowDepth;
                previousTemperature = rowTemperature;
                previousTime = rowTime;
                    
            }
            st.close();
            con.close();    

        } catch (Exception e) {
            System.out.println(e);
        }
    }
    
    private double interpolate(Timestamp startTime, Double startVal, Timestamp endTime, Double endVal, Timestamp targetTime)
    {
        long range =  startTime.getTime() - endTime.getTime();
        double fraction = ((double)(targetTime.getTime() - endTime.getTime() ))/((double)range);
     
        // not a very good thing
        if(fraction>1.0)
            return endVal;
        
        
        double result = (startVal * (fraction) + endVal * (1-fraction));     
       
        if(debugOut != null)
        {
            try {
            debugOut.println("StartTime ="+startTime + " startVal="+startVal+
                    " endTime="+endTime+" endVal="+endVal+" targetTime="+targetTime+" Fraction="+fraction+"<br>");
            } catch (Exception e) {}
        }
       
        return result;
    
    }
    
    private double cToF(double c)
    {
        return 1.8*c+32;
    }

    private final void readProperties() {

        try {

            ClassLoader classLoader = Thread.currentThread().getContextClassLoader();
            InputStream input = new FileInputStream("config.properties");
            prop = new Properties();
            prop.load(input);

            dburl = prop.getProperty("dburl");
            dbuser = prop.getProperty("dbuser");
            dbpassword = prop.getProperty("dbpassword");

        } catch (Exception ex) {
            Logger.getLogger(CreekHistory.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public void createHtml(String fileName) throws Exception {
        PrintWriter writer = new PrintWriter(fileName, "UTF-8");

        writer.println("<html><head>"
                + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">"
                + "<title>Elkhorn Creek Depth</title>"
                + "</head>"
                + "<body>"
                + "<h1>Elkhorn Creek Water Level</h1>");

        writer.println("Time = " + startTime + "<br>");

        writer.println("<table border=1><tr><th>Minutes Ago</th><th>Time</th><th>Depth</th>"
                + "<th>Depth Delta</th><th>Temp</th><th>Temp Delta</th></tr>");
        int i;
        for (i = 0; i < count; i++) {
            writer.println("<tr>");
            writer.println("<td>" + (i * timeIncrementMinutes) + "</td>");
            Timestamp ts = new Timestamp(startTime.getTime() - i * timeIncrementMinutes * 60 * 1000);
            writer.println("<td>" + ts + "</td>");
            writer.println("<td>" + String.format("%.1f", depth[i]) + "</td>");
            writer.println("<td>" + String.format("%.1f", depthDelta[i]) + "</td>");
            writer.println("<td>" + String.format("%.1f", temperature[i]) + "</td>");
            writer.println("<td>" + String.format("%.1f", temperatureDelta[i]) + "</td>");
            writer.println("<tr>");
        }
        writer.println("</table>");



	writer.println("<br> <br> <img src=\"current.png\" />    <br> <a href=\"floods.html\">Flood History</a>");
        


	writer.println("<table border=1><tr><th>Reference</th><th>Depth</th><tr>");
	writer.println("<br>");
	writer.println("<tr><td>Bank</td><td>7.6</td></tr>");
	writer.println("<tr><td>Tall Grass Edge</td><td>10.7</td></tr>");
	writer.println("<tr><td>River Cut Across</td><td>11.0</td></tr>");
        writer.println("<tr><td>Bird House</td><td>12.7</td></tr>");
	writer.println("<tr><td>Garden Oak</td><td>16.2</td></tr>");
        writer.println("<tr><td>Hawkins Driveway</td><td>16.6</td></tr>");
	writer.println("<tr><td>Barn Pad</td><td>16.9</td>");
	writer.println("<tr><td>Garden Oak</td><td>16.2</td></tr>");
	writer.println("<tr><td>Hawkins Driveway</td><td>16.6</td></tr>");
	writer.println("<tr><td>Barn Pad</td><td>16.9</td></tr>");
	writer.println("<tr><td>Barn Floor</td><td>18.0</td></tr>");
	writer.println("<tr><td>House Floor</td><td>23.0</td></tr>");
	writer.println("</table><br><br><br>");
	writer.println("<a href=\"https://iotexpert.com/category/solutions/elkhorn-creek/\">Visit iotexpert.com</a> for all source code used to build this site<br><br>");
	writer.println("</body></html>");
       writer.close();
    }
}
