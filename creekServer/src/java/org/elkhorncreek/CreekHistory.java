/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 *
 * @author arh
 */
package org.elkhorncreek;

import java.io.IOException;
import java.io.InputStream;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.servlet.jsp.JspWriter;

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
    
    public Timestamp startTime;
    
    public static final Integer timeIncrementMinutes = 15;
    private static final Integer timeIncrementMs = 15 * 60 * 1000 ;
    private static final Integer numHours = 3;
    private static final Integer maxCount = (numHours * 60 / timeIncrementMinutes) + 1;
    
    public JspWriter debugOut=null;

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
            InputStream input = classLoader.getResourceAsStream("config.properties");
            prop = new Properties();
            prop.load(input);

            dburl = prop.getProperty("dburl");
            dbuser = prop.getProperty("dbuser");
            dbpassword = prop.getProperty("dbpassword");

        } catch (Exception ex) {
            Logger.getLogger(CreekHistory.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
}
