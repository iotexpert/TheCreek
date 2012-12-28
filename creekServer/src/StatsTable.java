
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.sql.*;


public class StatsTable {

	/**
	 * @param args
	 */
	
	static Connection con;
	
	static public void openConn()
	{
		
		String url = "jdbc:mysql://192.168.15.80/d1";
		System.out.println(url);
		try {
			Class.forName("com.mysql.jdbc.Driver");
		}
		catch (ClassNotFoundException e) {
			System.err.print("ClassNotFoundException: ");
			System.err.println(e.getMessage());
		}

		try {
			con = DriverManager.getConnection(url, "creek", "creek");
		}
		catch(Exception e)
		{
			System.out.println(e);
		}
	}
	
	static public Float derivative(Timestamp ts,Float currentdepth, Integer min)
	{
		
		try {
			// delta = ts - min
			long l;
			l=ts.getTime();
			l = l - (min*60*1000);
			Timestamp delta = new Timestamp(l);
					
			// select ts,depth where ts<delta limit 1

			String sql = "SELECT ts,depth FROM ddata where ts<=\"" + delta + "\" order by ts desc limit 1 ;";
			Statement stmt = con.createStatement();
			ResultSet rs = stmt.executeQuery(sql);
			//if none return null
			if(!rs.next())
				return null;
			
			Timestamp dts = rs.getTimestamp(1);
			Float depth = rs.getFloat(2); 
			
			//System.out.println(sql);
			//System.out.println("Time 1" + ts + " Time2 "+ delta + " time 3" + dts);
			// derivative = (currentdepth - depth) / (ts - dts)
			float hrs = (float)(ts.getTime() - dts.getTime());
			hrs = (hrs/1000)/60/60;
			
			Float der = (currentdepth - depth) / (hrs);
			//System.out.println(ts.getTime()+","+currentdepth+","+dts.getTime()+","+depth+","+der+" hours = " + hrs);
			
			return der;
			
		}
		catch (SQLException e) {
			System.err.print("SQLException: ");
			System.err.println(e.getMessage());
		}
		
		return null;
		
	}
	
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		System.out.println("Main");
		openConn();
		
		try {
			String sql = "SELECT id,ts,depth FROM ddata order by ts;";
			Statement stmt = con.createStatement();
			ResultSet rs = stmt.executeQuery(sql);
			System.out.println(sql);
		//if none return null
		while(rs.next())
		{	
			// System.out.println("in loop");
			
			Integer id = rs.getInt(1);
			System.out.println("ID =" + id);
			Timestamp dts = rs.getTimestamp(2);
			System.out.println("Timestamp =" + dts);
			//System.out.println("Timestamp " + dts);
			Float depth = rs.getFloat(3); 
			System.out.println( "Depth =" + depth);
			
			Float der15 = derivative(dts,depth,15);
			Float der60 = derivative(dts,depth,60);
			if(der15 == null || der60==null)
				continue;
			sql = "update ddata set 15min=" + der15 + ", 60min=" + der60 + " where id=" + id + ";";
			System.out.println(sql);
			Statement sup = con.createStatement();
			sup.executeUpdate(sql);
			//System.out.println(dts + "," + depth + "," + derivative(dts,depth,60));
		}
		}
		catch (Exception e)
		{
			System.out.println(e);
		}
		

	}

}
