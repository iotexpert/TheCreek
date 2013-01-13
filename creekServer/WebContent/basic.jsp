<%@ page contentType="text/html;charset=UTF-8" %>
<%@ page errorPage="error.jsp" %>
<%@ page import="java.sql.*" %>

<html>
<head>
<title>Elkhorn Creek Water Level</title>
</head>
<body>

<h2>Elkhorn Creek Water Level</h2>

<jsp:declaration>

Statement stmt;
Connection con;
String url = "jdbc:mysql://192.168.15.83/creekdata";

</jsp:declaration>

<jsp:scriptlet>

Class.forName("com.mysql.jdbc.Driver");
con = DriverManager.getConnection(url, "creek", "creek"); 

stmt = con.createStatement();

ResultSet rs  = stmt.executeQuery("select feet,inches,ts from creekdata.ddata order by ts desc limit 1");
rs.next();
int ft = rs.getInt("feet");
int in = rs.getInt("inches");
Timestamp ts = rs.getTimestamp("ts");

out.print("The current water level at " + ts + " is " + ft + "'" + in + "\""); 
con.close();

</jsp:scriptlet>


</body>
</html>



<HTML>



