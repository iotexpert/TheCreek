<%@ page contentType="text/html" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>

<% 

   int numpoints = 0;
   int scale=0;
   String s1 = request.getParameter("numpoints");
   String startDay;
   if(s1 == null)
   {
	s1= new String("365");
	scale = 24*12;
	numpoints = 365;
    }
    else
    {
	Integer x1 = new Integer(s1);
	numpoints = x1;
    }

    String s2 = request.getParameter("chart");

    String dayChecked = new String("");
    String weekChecked = new String("");
    String monthChecked = new String("");

    	 if(s2 != null)
	 {
		if(s2.equals("day"))
		{
		       scale = 24 * 12;
		       dayChecked = new String("CHECKED");
		}


	       if(s2.equals("week"))
	       	 {
		 	scale = 24 * 7 * 12;
			weekChecked = new String("CHECKED");
		 }

		 if(s2.equals("month"))
	 	 {
		 	scale = 24 * 7 * 4 * 12;
			monthChecked = new String("CHECKED");
	 	}
	}
	else
		{ dayChecked = new String("CHECKED");
		}


	 s2 = request.getParameter("startday");
	 
	 if(s2 == null)
	 {
		startDay = new String("2011-11-1");
	 }
	else
		startDay = s2;

	 if(s1 != null)
	 {
		Integer i1 = new Integer(s1);
		numpoints = i1;

	 }

	 int points= scale * numpoints;
%>




<HTML>




  <HEADER>
    <TITLE>Elkhorn Creek Water Level</TITLE>
  </HEADER>
  <BODY>
    <H2>Elkhorn Creek Water Level</H2>
    <a href="/creek/todo.html">Todo list</a><br>
    <P>
    <FORM ACTION="chart.jsp" METHOD=POST>
	<br>Start Day<input type="text" name="startday" value="<%= startDay %>">
	<br> <input type="text" name="numpoints" value=<%= s1 %>>
        <INPUT TYPE="radio" NAME="chart" VALUE="day" <%= dayChecked %> > Days
        <INPUT TYPE="radio" NAME="chart" VALUE="week" <%= weekChecked %>> Weeks
        <INPUT TYPE="radio" NAME="chart" VALUE="month" <%= monthChecked %> > Months<br>
        <INPUT TYPE="submit" VALUE="Generate Chart">
    </FORM>
<a href="servlet/CreateChart?sdate=<%=startDay%>&dpoints=<%=points%>&csv">CSV Download</a><br>
      <IMG src="servlet/CreateChart?sdate=<%=startDay%>&dpoints=<%=points%>" height="800" width="1000"/>
  </BODY>

</HTML>
