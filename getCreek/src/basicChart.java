import java.sql.*;

public class basicChart {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		System.out.println("starting");
		getCurrentLevel();

	}
	

	static public void getCurrentLevel()
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
		System.out.println("Got Conection");
		
		Statement stmt = con.createStatement();
		ResultSet rs  = stmt.executeQuery("select feet,inches,ts from creekdata.ddata order by ts desc limit 1");
		System.out.println("Executed Query");
		rs.next();

		int ft = rs.getInt("feet");
		int in = rs.getInt("inches");
		Timestamp ts = rs.getTimestamp("ts");
		
		System.out.println("Current level at " + ts + " is " + ft + "'" + in + "\"");
		
		con.close();
	}


	catch (Exception e) {
		System.err.print("Exception: ");
		System.err.println(e.getMessage());
	}
	}

}
