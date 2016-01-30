
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.Statement;
import java.sql.Timestamp;
import java.util.*;
import java.io.*;

import com.pi4j.io.i2c.I2CBus;
import com.pi4j.io.i2c.I2CDevice;
import com.pi4j.io.i2c.I2CFactory;

public class i2cdb {
	
	static Properties prop;
	static public boolean debug=true;
	static I2CBus bus1;
	static I2CBus bus0;

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		readProperties();
		getI2CBus();
		i2readVariables();

	}
	
	static void getI2CBus()
	{

		try {
			bus0 = I2CFactory.getInstance(I2CBus.BUS_0);
			bus1 = I2CFactory.getInstance(I2CBus.BUS_1);

		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	
	}
	
	static public void i2readVariables()
	{
		
		if(debug)
			System.out.println("Start Reading");

		Integer i2vars = new Integer(prop.getProperty("i2vars"));

		
		Object insertVals[] = new Object[i2vars];
		Object insertNames[] = new Object[i2vars];
		
		for(int i=0;i<i2vars;i++)
		{
			if(debug)
				System.out.println("Count = " + i );
			
			Integer i2cbus = new Integer(prop.getProperty("v"+i+"i2cbus"));
			Integer i2caddress = new Integer(prop.getProperty("v"+i+"i2caddress")); 
			Integer regaddress = new Integer(prop.getProperty("v"+i+"regaddress"));
			Integer nbytes = new Integer(prop.getProperty("v"+i+"nbytes"));

			Byte[] rawvals = readi2c(i2cbus,i2caddress,regaddress,nbytes);
			
			String vtype = prop.getProperty("v"+i+"type");
			
			String vendian = prop.getProperty("v"+i+"endian"); 

			String vsign  = prop.getProperty("v"+i+"sign"); 
			
			if(vtype.equals("int"))
			{
			    insertVals[i] = convertInteger(rawvals,vendian,vsign, nbytes); 
			}
			/*
			if(vtype.equals("float"))
			{
				insertVals[i] = convertFloat(rawvals,vendian,nbytes);
			}
			*/
			
			insertNames[i] = prop.getProperty("v"+i+"dbname");
		}
		
		String timestamp = prop.getProperty("timestamp");
		
				
		
		String insertString = "insert into " + prop.getProperty("dbname") + "." + prop.getProperty("dbtable") + "(";
		if(timestamp.equals("true"))
		{
			String tsfield = prop.getProperty("timestampfield");
			insertString = insertString + tsfield + ",";
		}
		for(int i=0;i<i2vars-1;i++)
		{
			insertString = insertString + insertNames[i] + ",";
		}
		insertString = insertString + insertNames[i2vars-1] + ") values (";
		
		if(timestamp.equals("true"))
		{
			java.util.Date date= new java.util.Date();
			Timestamp ts = new Timestamp(date.getTime());
			insertString = insertString + "\"" + ts + "\",";
		}
		
		for(int i=0;i<i2vars-1;i++)
		{
			insertString = insertString + insertVals[i] + ",";
		}
		insertString = insertString + insertVals[i2vars-1] + ");";
		
		if(debug)
			System.out.println(insertString);
		
		insertStringDatabase(insertString);
	}
	
    static public Integer convertInteger(Byte []b, String vendian, String vsign,Integer nbytes)
	{
		Integer rval = 0;
		System.out.println("Running conversion endian="+vendian +" sign = "+vsign +" bytes="+nbytes);

		/*
		for(int i=0;i<nbytes;i++)
		{
			if(vendian.equals("big"))
			{
			
				Double t1 = Math.pow(256,nbytes-i-1);			
				rval = rval + (rawvals[i] * t1.intValue());
			}
			else
			{
				Double t1 = Math.pow(256, i);
				rval = rval + (rawvals[i] * t1.intValue());
			}
			
		}
		*/


		if(nbytes == 2 && vsign.equals("yes") && vendian.equals("big"))
		{
		    rval = (   ((b[0] ) << 8) | ((b[1] & 0xFF) << 0));
		}

		if(nbytes == 2 && vsign.equals("yes") && !vendian.equals("big"))
		{
		    rval = (   ((b[1] ) << 8) | ((b[0] & 0xFF) << 0));
		}

		    if(nbytes == 2 && (!vsign.equals("yes")) && vendian.equals("big"))
		{
		    rval = (   ((b[0] & 0xFF) << 8) | ((b[1] & 0xFF) << 0));

		}

	    if(nbytes == 2 && (!vsign.equals("yes")) && !vendian.equals("big"))
		{
		    rval = (   ((b[1] & 0xFF) << 8) | ((b[0] & 0xFF) << 0));
		}

	   if(debug)
		       {
			   System.out.println("Rval =" + rval);
		       }

		
		return rval;
	}
	
	static public Byte[]readi2c(int i2cbus, int i2caddress, int regaddress, int numbytes)
	{
		if(debug)
			System.out.println("Reading i2c");
		
		I2CDevice dev;
		
		byte buffer[] = new byte[numbytes];
		
		
		Byte [] rval;
		rval = new Byte[numbytes];
		
		try {
	
	
		
		if(debug)
		{
			System.out.println("Reading bus=" + i2cbus + " i2caddress="+i2caddress + " reg="+regaddress + " bytes="+numbytes);
		}
		
		
		if(debug)
		{
			System.out.println("got device");
		}
		
		
		if(i2cbus == 0)
			dev = bus0.getDevice(i2caddress);

		else
			dev = bus1.getDevice(i2caddress);

		
		dev.read(regaddress,buffer,0,numbytes);
		
		if(debug)
		{
			System.out.println("completed read");
		}
		

		
	
		
		}
		catch(Exception e)
		{
			
			System.out.println("Exception");
			e.printStackTrace();
		}
		
		for(int i=0;i<numbytes;i++)
		{
		    rval[i]=new Byte(buffer[i]);

		    /*
		     System.out.println("Val = " + i + " = " + buffer[i] + " +128 " + buffer[i]+128+ " 2comp = " + ( ~(buffer[i] & 0xFF) + 1));

		    //rval[i]=buffer[i] + 128;
			if(debug)
			    {
				System.out.println("Rval["+i+"]="+rval[i].toHexString(rval[i]));
				//	System.out.println("Buffer["+i+"]="+buffer[i].toHexString(buffer[i]));
			    }
		    */
		}
		
		
		return rval;
		
	}
	
	static public void readProperties()
	{
		prop = new Properties();
		FileInputStream fis;
		try {
			fis = new FileInputStream("config.properties");
			prop.load(fis);
			//prop.list(System.out);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			System.out.println("Exception");
			e.printStackTrace();
		}
		
	}
	
	public static void insertStringDatabase(String insert)
	{
		
		String url = prop.getProperty("dburl");
		
		Connection con;

		try {
			Class.forName("com.mysql.jdbc.Driver");
		}
		catch (ClassNotFoundException e) {
			System.err.print("ClassNotFoundException: ");
			System.err.println(e.getMessage());
		}

		try {
			con = DriverManager.getConnection(url, prop.getProperty("dbuser"), prop.getProperty("dbpassword"));


			Statement stmt = con.createStatement();
			stmt.executeUpdate(insert);

			con.close();
		}


		catch (Exception e) {
			System.err.print("Exception: ");
			System.err.println(e.getMessage());
		}
	}  


}
