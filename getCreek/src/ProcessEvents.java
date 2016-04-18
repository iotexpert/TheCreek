
import java.io.FileInputStream;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Properties;

class StartEnd {
    int id = 0;
    Timestamp start;
    Timestamp end;
};


public class ProcessEvents {

    static Properties prop;

    static double upperThreshold = 2.5;
    static double lowerThreshold = 0.75;
    
    static public ArrayList<StartEnd> events;
    
    
    public static void main(String[] args)
    {
        run(args);
    }
    

    public static void run(String[] args) {
        // TODO code application logic here

        LocalDateTime ldt = LocalDateTime.now();
        System.out.println("Starting Time " + ldt);
        StartEnd pair;

        try {
            readProperties();
            
            
        } catch (Exception e) {
            System.out.println(e);
            return;
        }
        
       try {
           String ut = prop.getProperty("floodUpperThreshold");
           
           if(ut != null)
               upperThreshold = Double.parseDouble(ut);
           
       }
       catch (Exception e) {}
       
       try {
           String lt = prop.getProperty("floodLowerThreshold");
   
           if(lt != null)
               lowerThreshold = Double.parseDouble(lt);
       }
       catch (Exception e) {}

        

        try {
            while (true) {
                boolean newEvent;
                newEvent = false;
                pair = getLastEvent();

                // if there are no events in the table (pair.start == null) 
                // or the last event is Complete pair.end != null
                // then for sure you have a new event
                if (pair.start == null || pair.end != null) {
                    newEvent = true;
                    if (pair.start == null)
                        pair.start = findStart(null);
                    else
                        pair.start = findStart(pair.end);
                    
                    pair.end = null;
                    if (pair.start == null) 
                        break; // there are no more events
                }

                if (pair.end == null)
                    pair.end = findEnd(pair.start);
               
                System.out.println("Start = " + pair.start + " End=" + pair.end);

                if (newEvent) {
                    System.out.println("Inserting new event");
                    newEventInsert(pair);
                } else
                    eventUpdate(pair);

                if (pair.end == null)  break;
            }
            
            calcAndInsertMaxDepth();

        } catch (Exception e) {
            return;
        }

        System.out.println("Ending Time = " + LocalDateTime.now());
        System.out.println("Elapsed time =");

    }

    static void newEventInsert(StartEnd pair) throws Exception {
        //System.out.println("Inserting new event" + pair.start + "  " + pair.end);
        String url = prop.getProperty("dburl");
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(url, prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String insert;
        if(pair.end == null)
            insert = "insert into creekdata.floodevents (start,end) values ('" + pair.start + "',null)";
        else
            insert = "insert into creekdata.floodevents (start,end) values ('" + pair.start + "','" + pair.end + "')";
        
        System.out.println(insert);
        Statement stmt = con.createStatement();
        stmt.executeUpdate(insert);
        con.close();

    }

    static void eventUpdate(StartEnd pair) throws Exception {
        if (pair.end == null) {
            return;
        }

        //System.out.println("Updating event");
        String url = prop.getProperty("dburl");
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(url, prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String insert = "update creekdata.floodevents set end='" + pair.end + "' where id=" + pair.id;
        //System.out.println(insert);
        Statement stmt = con.createStatement();
        stmt.executeUpdate(insert);
        con.close();
    }

    /// This function read the properties file.    
    static public void readProperties() throws Exception {
        prop = new Properties();
        FileInputStream fis;
        fis = new FileInputStream("config.properties");
        prop.load(fis);
    }

    static Timestamp findStart(Timestamp ts) throws Exception {
        Timestamp rval = null;
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");

        //System.out.println("looking for start = " + ts);
        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));

        String query;
        if (ts == null) {
            query = "select created_at,depth from creekdata.creekdata where depth>" + upperThreshold + "limit 1";
        } else {
            query = "select created_at,depth from creekdata.creekdata where depth>" + upperThreshold + " and created_at > '" + ts + "' limit 1";
        }
        //System.out.println(query);
        Statement st = con.createStatement();

        try {
            ResultSet rs = st.executeQuery(query);

            if (!rs.next()) {
                //System.out.println("No Events");
                return rval;
            }

            rval = rs.getTimestamp("created_at");

            //System.out.println("Found new start" + rval);
            st.close();
            con.close();

            return rval;

        } catch (Exception e) {
            System.out.println(e);
        }
        return rval;

    }

    static Timestamp findEnd(Timestamp ts) throws Exception {

        Timestamp rval = null;
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));

        String query;
        query = "select created_at,depth from creekdata.creekdata where depth<" + lowerThreshold + " and created_at > '" + ts + "' limit 1";
        //System.out.println("Looking for end");
        //System.out.println(query);
        Statement st = con.createStatement();

        try {
            ResultSet rs = st.executeQuery(query);

            if (!rs.next()) {
                //System.out.println("No Events");
                return rval;
            }

            rval = rs.getTimestamp("created_at");
            st.close();
            con.close();

            return rval;

        } catch (Exception e) {
            System.out.println(e);
        }
        return rval;

    }

    static StartEnd getLastEvent() throws Exception {

        StartEnd rval = new StartEnd();
        rval.start = null;
        rval.end = null;

        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String query = "select id,start,end from creekdata.floodevents order by start desc limit 1";
        Statement st = con.createStatement();

        try {
            ResultSet rs = st.executeQuery(query);

            if (!rs.next()) {
                // System.out.println("Nothing in the table");
                return rval;
            }

            rval.id = rs.getInt("id");
            rval.start = rs.getTimestamp("start");
            rval.end = rs.getTimestamp("end");
            st.close();
            con.close();

            return rval;

        } catch (Exception e) {
            System.out.println(e);
        }
        return null;
    }

    static void calcAndInsertMaxDepth() throws Exception {

        StartEnd temp;
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String query = "select id,start,end,max from creekdata.floodevents where max is null or end is null";
        Statement st = con.createStatement();

        try {
            ResultSet rs = st.executeQuery(query);

            while (rs.next()) {

                temp = new StartEnd();
                temp.id = rs.getInt("id");
                temp.start = rs.getTimestamp("start");
                temp.end = rs.getTimestamp("end");
                double maxDepth = getMaxDepth(temp.start, temp.end);
                depthUpdate(temp.id, maxDepth);
                System.out.println("Start ="+temp.start + " End="+temp.end + " Max = " + maxDepth);
               
            }
            st.close();
            con.close();

            return;

        } catch (Exception e) {
            System.out.println(e);
        }
        return;
    }

    static double getMaxDepth(Timestamp start, Timestamp end) throws Exception {

        //String url = prop.getProperty("dburl")+"?"
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String query;
        if(end != null)
        {
            query = "select created_at,max(depth) from creekdata.creekdata where created_at>'" + start + "' and created_at<'" + end + "'";
        } else {
            query = "select created_at,max(depth) from creekdata.creekdata where created_at>'" + start + "'";
        }
        Statement st = con.createStatement();

        try {
            st.setQueryTimeout(100);

            ResultSet rs = st.executeQuery(query);
            rs.next();
            float rval = rs.getFloat("max(depth)");
            st.close();
            con.close();
            return rval;

        } catch (Exception e) {
            System.out.println("Max depth query failed");
            System.out.println(e);
        }
        return 0.0; // probably a bad thing
    }

    static void depthUpdate(int id, double maxDepth) throws Exception {
        //System.out.println("Updating event");
        String url = prop.getProperty("dburl");
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(url, prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String insert = "update creekdata.floodevents set max='" + maxDepth + "' where id=" + id;
        Statement stmt = con.createStatement();
        stmt.executeUpdate(insert);
        con.close();
    }

    static public void updateEventsFromDB() throws Exception
    {
        events = new ArrayList<StartEnd>();
        
        Connection con;
        Class.forName("com.mysql.jdbc.Driver");
        con = DriverManager.getConnection(prop.getProperty("dburl"), prop.getProperty("dbuser"), prop.getProperty("dbpassword"));
        String query = "select id,start,end from creekdata.floodevents order by start desc";
        Statement st = con.createStatement();

        try {
            ResultSet rs = st.executeQuery(query);

            while(rs.next())
            {
                StartEnd temp = new StartEnd();
                
                temp.id = rs.getInt("id");
                temp.start = rs.getTimestamp("start");
                temp.end = rs.getTimestamp("end");
                
                events.add(temp);
            }
            st.close();
            con.close();

        } catch (Exception e) {
            System.out.println(e);
        }
        
        //System.out.println("#Events = " + events.size());
        
        return;
    }
    
}
