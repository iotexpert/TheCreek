<%@ page contentType="text/csv;charset=UTF-8" %>
<%@ page import="java.sql.*" %>
<%

Statement stmt;
Connection con;

Integer id;
Timestamp ts;
Integer filter;
Integer temp;


String url = "jdbc:mysql://192.168.15.83/creekdata";


Class.forName("com.mysql.jdbc.Driver");
con = DriverManager.getConnection(url, "creek", "creek"); 

stmt = con.createStatement();


ResultSet rs  = stmt.executeQuery("select id,ts,filter,temp from creekdata.ddata order by ts desc limit 10000");

out.print("id,time,filter,temp\n");


while(rs.next())
{

	id = rs.getInt("id");
	ts = rs.getTimestamp("ts");
	filter = rs.getInt("filter");
	temp = rs.getInt("temp");



	out.print(id + "," + ts + "," + filter + "," + temp + "\n");

}

con.close();

%>
