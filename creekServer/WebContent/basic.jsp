<%@ page contentType="text/html;charset=UTF-8" %>
<%@ page errorPage="error.jsp" %>
<%@ page import="java.sql.*" %>

<html>
<head>
<title>Elkhorn Creek Water Level</title>
</head>
<body>

<h2>Elkhorn Creek Water Level</h2>

<%

Statement stmt;
Connection con;
int i;

Integer filter[] = new Integer[200];
Timestamp ts[] = new Timestamp[200];
Double depth[] = new Double[200];

Integer[] inches = new Integer[200];

Double feet[] = new Double[200];

String url = "jdbc:mysql://192.168.15.83/creekdata";


Class.forName("com.mysql.jdbc.Driver");
con = DriverManager.getConnection(url, "creek", "creek"); 

stmt = con.createStatement();



ResultSet rs  = stmt.executeQuery("select filter,ts from creekdata.ddata order by ts desc limit 181");


rs.next(); 

out.print("<table   border=\"1\">");
out.print("<tr align=\"center\"><th>Minutes</th><th>Time</th><th>Depth<br/>Feet</td><th>Delta<br/>Feet</th><th>Depth<br/>Inches</td><th>Delta<br/>Inches</th><th>Counts</th></tr>");
for(i=0;i<181;i++)
{
	filter[i] = rs.getInt("filter");
	ts[i] = rs.getTimestamp("ts");
	depth[i] = ((filter[i].doubleValue())-408.8)/3.906311;
	feet[i] = depth[i]/12;

	if((i % 15)==0)
	{
		out.print("<tr align=\"center\"><td> " + i + "</td><td> " 
		+ String.format("%tR",ts[i]) + "</td><td>" 
		+ String.format("%.1f%n",feet[i]) + "</td><td>" 
		+ String.format("%.1f%n",(feet[0]-feet[i])) + "</td><td>"
		+ String.format("%.1f%n",depth[i]) + "</td><td>" 
		+ String.format("%.1f%n",(depth[0]-depth[i])) + "</td><td>"
		+ filter[i] + "</td></tr>"
		);
	}

	rs.next();

}
out.print("</table>");



con.close();

%>

<img src="creeklevel.png" />
</body>
</html>



<HTML>



