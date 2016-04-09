
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.Statement;
import java.sql.Timestamp;


import com.pi4j.io.i2c.I2CBus;
import com.pi4j.io.i2c.I2CDevice;
import com.pi4j.io.i2c.I2CFactory;


public class getCreek {

	/**
	 * @param args
	 */

	static boolean printInfo=false; 
	static boolean insertData=false;
	static boolean repeatRead=false;
	static I2CBus bus;
	static I2CDevice dev;

	public static void main(String[] args) throws Exception {

		for(int i=0;i<args.length;i++)
		{
			switch(args[i].charAt(0))
			{
			case 'p':
				printInfo=true;
				break;
			case 'i':
				insertData=true;
				break;
			case 'r':
				repeatRead=true;
			}
		}

		initI2C();

		if(!repeatRead)
			readI2C();
		else
		{
			while(true)
			{
				readI2C();
				Thread.sleep(5000);
			}
		}

	}
	
	public static void insertString(String insert)
	{
		String url = "jdbc:mysql://192.168.15.83/creekdata";
		Connection con;

		try {
			Class.forName("com.mysql.jdbc.Driver");
		}
		catch (ClassNotFoundException e) {
			System.err.print("ClassNotFoundException: ");
			System.err.println(e.getMessage());
		}

		try {
			con = DriverManager.getConnection(url, "creek", "creek");


			Statement stmt = con.createStatement();
			stmt.executeUpdate(insert);

			con.close();
		}


		catch (Exception e) {
			System.err.print("Exception: ");
			System.err.println(e.getMessage());
		}
	}  

	public static void initI2C() throws IOException
	{
		// get I2C bus instance

		// www.pi4j.com
		// dev.read 
		
		System.out.println("opening i2c");
		I2CBus bus = I2CFactory.getInstance(I2CBus.BUS_1);
		
		dev = bus.getDevice(0x8);

	}
	public static void readI2C() throws IOException
	{


		byte[] buffer;
		buffer = new byte[10];
		//System.out.println("preparing to write");
		//byte b;
		//b=0;
		//dev.write(0,b); // write a 0

		System.out.println("preparing to read");
		// address, buffer, offset, size
		dev.read(0,buffer,0,4);


		Byte b1 = new Byte(buffer[0]);
		int i1;
		i1 = b1 & 0xFF;
		System.out.println("0 = " + i1 + " Hex " + Integer.toHexString(i1));


		Byte b2 = new Byte(buffer[1]);
		int i2;
		i2 = b2 & 0xFF;
		System.out.println("1 = " + i2+ " Hex " + Integer.toHexString(i2));

		Byte b3 =  new Byte(buffer[2]);
		int i3;
		i3 = b3 & 0xFF;
		System.out.println("2 = " + i3+ " Hex " + Integer.toHexString(i3));

		Byte b4 = new Byte(buffer[3]);
		int i4;
		i4 = b4 & 0xFF;
		System.out.println("3 = " + i4+ " Hex " + Integer.toHexString(i4));

		Integer filterval = (i1 * 256) + i2;

		Integer adcval = (i3 * 256) + i4;
		if(printInfo)
		{
			System.out.println("filter val = " + filterval + " adcval =" + adcval);
		}

		Double ad1 = new Double(filterval);

		Double volts = 1.0234 * 2 * ad1 / 65536.0;
		if(printInfo)
			System.out.println("Volts = " + volts);

		// psi
		Double psi = 15 * ((volts - (51.1*0.004)) / (51.1 * (0.02 - 0.004)));
		if(psi<0)
			psi=0.0;

		if(printInfo)
			System.out.println("PSI = " + psi);

		// feet
		Double feet = psi / 0.43 ;
		Integer ft = new Integer(feet.intValue());
		Double inches = (feet-ft)*12;

		if(printInfo) {
			System.out.println("Feet = " + feet);
			System.out.println("Depth = " + ft + "'" + inches + "\"");
		}


		java.util.Date date= new java.util.Date();
		Timestamp ts = new Timestamp(date.getTime());

		String sql = "insert into creekdata.ddata (ts,feet,inches,depth,adc,volts,psi,filter) values (\"" + ts +"\"," + ft +","+ inches + ","
				+ feet +"," + adcval + "," + volts +","+ psi + ","+filterval +  ");";
		if(insertData)
			insertString(sql);
		{
			if(printInfo)
				System.out.println("sql:" + sql);
		}

	}


}
