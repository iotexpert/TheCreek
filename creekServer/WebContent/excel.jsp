<%@ page contentType="text/csv;charset=UTF-8" %>
<%@ page import="java.sql.*" %>
<%

Statement stmt;
Connection con;

Integer id;
Timestamp ts;
Integer adc;
Integer filter;
Integer adcp5;


String url = "jdbc:mysql://192.168.15.83/creekdata";


Class.forName("com.mysql.jdbc.Driver");
con = DriverManager.getConnection(url, "creek", "creek"); 

stmt = con.createStatement();


ResultSet rs  = stmt.executeQuery("select * from creekdata.ddata order by ts desc limit 10000");

out.print("id,time,adc,filter,adcp5\n");


while(rs.next())
{

	id = rs.getInt("id");
	ts = rs.getTimestamp("ts");
	adc = rs.getInt("adc");
	filter = rs.getInt("filter");
	adcp5 = rs.getInt("adcp5");

	out.print(id + "," + ts + "," + adc + "," + filter + "," + adcp5 + "\n");

}

con.close();

%>
