
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
    public static void main(String[] args) throws Exception {

    boolean printInfo=false;
    boolean insertData=false;



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
		    }
	    }


        
        // get I2C bus instance
	byte[] buffer;
	buffer = new byte[10];

    I2CBus bus = I2CFactory.getInstance(I2CBus.BUS_1);
	I2CDevice dev;
	dev = bus.getDevice(0x8);

	dev.read(0,buffer,0,2);
	for(int i =0 ; i<2 ; i++)
	    {
		if(printInfo)
		    System.out.println( i + " = " + buffer[i]);
	    }

	Integer adcval = (buffer[1] << 8) + buffer[0];
	if(printInfo)
	    System.out.println("val = " + adcval);

	Double ad1 = new Double(adcval);

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

	String sql = "insert into creekdata.ddata (ts,feet,inches,depth,adc,volts,psi) values (\"" + ts +"\"," + ft +","+ inches + ","+ feet +"," + adcval + "," + volts +","+ psi + ");";
	if(insertData)
		insertString(sql);
	    {
		if(printInfo)
		    System.out.println("sql:" + sql);
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
}
