
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.Statement;
import java.sql.Timestamp;
import java.util.*;
import java.io.*;

import com.pi4j.io.i2c.I2CBus;
import com.pi4j.io.i2c.I2CDevice;
import com.pi4j.io.i2c.I2CFactory;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

public class i2cdb {

    static Properties prop;
    static public boolean debug = false;
    static I2CBus bus1;
    static I2CBus bus0;

    public static void main(String[] args) {

        try {
            readProperties();
            getI2CBus();
            i2readVariables();
        } catch (Exception e) {
            System.out.println("Something really bad happened");
            System.out.println(e.getMessage());
        }
    }

    
    static void getI2CBus() throws Exception {

        Exception rval = new Exception();
        
        try {  bus0 = I2CFactory.getInstance(I2CBus.BUS_0); } catch (Exception e) { rval = e;}
        try {  bus1 = I2CFactory.getInstance(I2CBus.BUS_1); }  catch (Exception e) { rval = e;} 
        if(bus0 == null && bus1==null) throw rval;     
    }

    static public void i2readVariables() throws Exception  {

     
        Boolean I2CReadSuccess;
        Integer I2CRetryCount;

        Integer i2vars = new Integer(prop.getProperty("i2vars"));

        Object insertVals[] = new Object[i2vars];
        Object insertNames[] = new Object[i2vars];
   
        for (int i = 0; i < i2vars; i++) {
            Integer i2cbus = new Integer(prop.getProperty("v" + i + "i2cbus"));
            Integer i2caddress = new Integer(prop.getProperty("v" + i + "i2caddress"));
            Integer regaddress = new Integer(prop.getProperty("v" + i + "regaddress"));
            String vtype = prop.getProperty("v" + i + "type");
            String vendian = prop.getProperty("v" + i + "endian");

            Integer nbytes=0;           
            if (vtype.equals("uint8"))  nbytes = 1;
            if (vtype.equals("int8"))   nbytes = 1;
            if (vtype.equals("int16")) nbytes = 2;
            if (vtype.equals("uint16")) nbytes=2;
            if (vtype.equals("float")) nbytes=4;
            if (vtype.equals("double")) nbytes=8;
        
            byte buffer[] = new byte[nbytes];

            I2CDevice dev;
            try {
                if(i2cbus == 0)
                    dev = bus0.getDevice(i2caddress);
                else
                    dev = bus1.getDevice(i2caddress);
            }
            catch (Exception getException) {
                throw getException;
            }
                        
            I2CReadSuccess = false;
            I2CRetryCount = 20; // 20 retrys

            while (I2CReadSuccess == false && I2CRetryCount > 0) {
                try {
                    dev.read(regaddress, buffer, 0, nbytes);
                    I2CReadSuccess = true;

                } catch (Exception e) {
                    try {
                        Thread.sleep(200); //200 ms empirically determined
                    } catch (Exception se) {
                        throw se; // this is really bad probably
                    }
                    I2CRetryCount = I2CRetryCount - 1;
                }
            }

        if (I2CReadSuccess == false) {
            if (debug) System.out.println("Broken Read");
            throw new Exception("Unable to read I2C Buffer");
        }

        ByteBuffer wrapped = ByteBuffer.wrap(buffer);

        if (vendian.equals("little"))
            wrapped.order(ByteOrder.LITTLE_ENDIAN);
        else
            wrapped.order(ByteOrder.BIG_ENDIAN);
        
        if (vtype.equals("uint8"))  insertVals[i] = ((int) buffer[0]) & 0xFF;
        if (vtype.equals("int8"))   insertVals[i] = buffer[0]; 
        if (vtype.equals("int16")) insertVals[i] = wrapped.getShort();
        if (vtype.equals("uint16")) insertVals[i] = wrapped.getShort() & 0xFFFF; // Convert to unsigned
        if (vtype.equals("float")) insertVals[i] = wrapped.getFloat();
        if (vtype.equals("double"))insertVals[i] = wrapped.getDouble();
        insertNames[i] = prop.getProperty("v" + i + "dbname");
    }

    String timestamp = prop.getProperty("timestamp");

    String insertString = "insert into " + prop.getProperty("dbname") + "." + prop.getProperty("dbtable") + "(";

    if (timestamp.equals ("true")) {
            String tsfield = prop.getProperty("timestampfield");
        insertString = insertString + tsfield + ",";
    }
    for (int i = 0; i< i2vars-1; i++) {
        insertString = insertString + insertNames[i] + ",";
    }
    insertString  = insertString + insertNames[i2vars - 1] + ") values (";

    if (timestamp.equals( "true")) {
        java.util.Date date = new java.util.Date();
        Timestamp ts = new Timestamp(date.getTime());
        insertString = insertString + "\"" + ts + "\",";
    }
    
    for (int i = 0; i< i2vars-1; i++) {
        insertString = insertString + insertVals[i] + ",";
    }
    insertString = insertString + insertVals[i2vars-1] + ");";
    System.out.println(insertString);    
    insertStringDatabase(insertString);
}


/// This function read the properties file.    
    static public void readProperties() throws Exception {
        prop = new Properties();
        FileInputStream fis;
        fis = new FileInputStream("config.properties");
        prop.load(fis);
    }

    // Insert the data into the database
    public static void insertStringDatabase(String insert) throws Exception {

        String url = prop.getProperty("dburl");

        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(url, prop.getProperty("dbuser"), prop.getProperty("dbpassword"));

        Statement stmt = con.createStatement();
        stmt.executeUpdate(insert);
        con.close();

    }
 
}
