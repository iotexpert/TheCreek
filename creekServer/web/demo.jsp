<html>
<head><title>Roll the dice</title></head>
<body>
  <%
    int die1 = (int)(Math.random() * 6 + 0.5);
    int die2 = (int)(Math.random() * 6 + 0.5);
    %>
    <h2> Die1 = <%= die1 %></h2>
    <h2> Die2 = <%= die2 %></h2>
    
  <a href="<%= request.getRequestURI() %>"><h3>Try Again</h3></a>
</body>
</html>