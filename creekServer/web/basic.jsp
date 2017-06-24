<%@page import="java.sql.Timestamp"%>
<%@ page import="org.elkhorncreek.CreekHistory" %>

<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Elkhorn Creek Depth</title>
    </head>
    <body>
        <h1>Elkhorn Creek Water Level</h1>
        <%
            
            CreekHistory creekHistory = new CreekHistory();
            //creekHistory.debugOut = out;
                       
            creekHistory.loadData();
            
            out.println("Time = "+ creekHistory.startTime + "<br>");
            
            out.println("<table border=1><tr><th>Minutes Ago</th><th>Time</th><th>Depth</th>" +
                    "<th>Depth Delta</th><th>Temp</th><th>Temp Delta</th></tr>");
            int i;
            for(i=0;i<creekHistory.count;i++)
            {
                out.println("<tr>");
                out.println("<td>"+ (i*creekHistory.timeIncrementMinutes) + "</td>");
                Timestamp ts = new Timestamp(creekHistory.startTime.getTime() - i*creekHistory.timeIncrementMinutes*60*1000);
                out.println("<td>" + ts + "</td>");
                out.println("<td>" + String.format( "%.1f", creekHistory.depth[i]) + "</td>");
                out.println("<td>" + String.format( "%.1f", creekHistory.depthDelta[i]) + "</td>");
                out.println("<td>" + String.format( "%.1f", creekHistory.temperature[i]) +"</td>");
                out.println("<td>" + String.format( "%.1f", creekHistory.temperatureDelta[i]) + "</td>");
                out.println("<tr>");
            }
            out.println("</table>");
        %>
        <br>
        <br>
        <img src="creekPlots/current.png" />
        <br>
        <a href="creekPlots/floods.html">Flood History</a>
	<br>
        <table border=1><tr><th>Reference</th><th>Depth</th><tr>
	<tr><td>Bank</td><td>7.6</td></tr>
	<tr><td>Tall grass Edge</td><td>10.7</td></tr>
	<tr><td>River Cut Across</td><td>11.0</td></tr>
	<tr><td>Bird House</td><td>12.7</td></tr>
	<tr><td>Garden Oak</td><td>16.2</td></tr>
	<tr><td>Hawkins Driveway</td><td>16.6</td></tr>
	<tr><td>Barn Pad</td><td>16.9</td></tr>
	<tr><td>Barn Floor</td><td>18.0</td></tr>
	<tr><td>House Floor</td><td>23.0</td></tr>
	</table>
    </body>
</html>
