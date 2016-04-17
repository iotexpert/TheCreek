
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartUtilities;
import org.jfree.chart.axis.NumberAxis;
import org.jfree.chart.axis.NumberTickUnit;
import org.jfree.chart.plot.XYPlot;
import org.jfree.chart.JFreeChart;
import org.jfree.data.jdbc.JDBCXYDataset;
import java.io.File;
import java.sql.Timestamp;
import java.time.Duration;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.time.temporal.ChronoUnit;

public class MakeChart {

    static final long runTimeDefault = 8; // hours
    static final double maxDepthChart = 23.0; // feet
    static final String url = "jdbc:mysql://iotexpertpi.local/creekdata";
    static final String sqlUser = "creek";
    static final String sqlPassword = "creek";
    static final double tickMarks = 1.0; // feet

    
    
    public static void run(String[] args)  {
        LocalDateTime ldt = null;
        LocalDateTime ldtEnd = null;
        Long runTime = null;
        
        if (args.length == 1 || args.length > 4) {
            System.out.println("MakeChart -help");
            System.out.println("MakeChart filename chart of the last 8 hours");
            System.out.println("MakeChart filename [hours] chart of last \"hours\"");
            System.out.println("MakeChart filename [date] chart of date+8 hours");
            System.out.println("MakeChart filename [date] [hours] chart of date+hours");
            System.out.println("Date can be yyyy-MM-dd hh:mm:ss or yyyy-MM-dd hh:mm or yyyy-MM-dd");
            return;
        } else if (args.length == 2) {
            System.out.println("MakeChart " + args[1]);

        } else if (args.length == 3) {
            System.out.println("MakeChart " + args[1] + " " + args[2]);

        } else if (args.length == 4) {
            System.out.println("MakeChart " + args[1] + " " + args[2] + " " + args[3]);
        }
       
        
        // if there is just argument try to do that date/time + 8 hours
        // if that doesnt work then try do do the current time - that # of hours
        if (args.length == 3) {
            boolean success;
            try {
                runTime = new Long(args[2]);
                ldt = LocalDateTime.now().minusHours(runTime);
                success = true;
            } catch (Exception e) {
                runTime= null;
                success = false;
            }

            if (success == false) // if it isnt an integer.. then try dates
            {
                try {
                    ldt = convertStringDateTime(args[2]);
                } catch (Exception e) {
                    System.out.println(e);
                    return;
                }
            }
        }

        if (args.length == 4) {
            try {
                ldt = convertStringDateTime(args[2]);
            } catch (Exception e) {
                System.out.println("arg[2] must be date");
                return;
            }

            boolean twoDate=false;
            
            try {
                ldtEnd = convertStringDateTime(args[3]);
                twoDate = true;
            } catch (Exception e) {}
            
            if (twoDate == false) {
                try {
                    runTime = new Long(args[3]);
                } catch (Exception e) {
                    System.out.println("arg[3] must be Integer Hours or Date");
                    return;
                }
            }
            
            
        }

        JDBCXYDataset data = null;
        
         
        // If there is no arguments then do the last "runTimeDefult" hours
        if(ldt !=null && ldtEnd !=null)
        {
            data = createDataset(ldt,ldtEnd);
            runTime =  ChronoUnit.HOURS.between(ldt, ldtEnd);

        }
        else
        if(ldt == null && runTime == null)
        {
            ldt = LocalDateTime.now().minusHours(runTimeDefault);
            runTime = runTimeDefault;
            data = createDataset(ldt);
        }
        else
        if (runTime == null && ldt !=null)
        {
             data = createDataset(ldt);
             runTime = 0L;
        }
        else
        if(runTime !=null && ldt == null)
        {
            ldt = LocalDateTime.now().minusHours(runTime);
            data = createDataset(ldt);
        }
        else
        if(runTime !=null && ldt != null)
        {
            data = createDataset(ldt,runTime);
        }
        
        JFreeChart chart = ChartFactory.createTimeSeriesChart(
                "Elkhorn Creek Water Level ", // chart title
                "Start = "+ldt+ "           Duration=" + runTime + " Hours", // x-axis label
                "Depth - Feet", // y-axis label
                data, // data
                true, // include legend
                true, // generate tool tips
                false // generate URLs
        );

        XYPlot plot = (XYPlot) chart.getPlot();
        NumberAxis axis = (NumberAxis) plot.getRangeAxis();
        axis.setRange(0.0, maxDepthChart);
        axis.setTickUnit(new NumberTickUnit(tickMarks));

        try {ChartUtilities.saveChartAsPNG(new File(args[1]), chart, 1000, 700); } catch (Exception e) {System.out.println(e);}
    }

    static LocalDateTime convertStringDateTime(String in) throws Exception {
        LocalDateTime ldt;
        try {
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
            ldt = LocalDateTime.parse(in, formatter);
            return ldt;
        } catch (Exception e) {
        }

        try {
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm");
            ldt = LocalDateTime.parse(in, formatter);
            return ldt;
        } catch (Exception e) {
        }

        try {
            LocalDate aLD = LocalDate.parse(in);
            ldt = aLD.atStartOfDay();
            return ldt;
        } catch (Exception e) {
        }
        throw new Exception("Date Conversion Failed for " + in);
    }

    static JDBCXYDataset createDataset(LocalDateTime ts, long hours) {

        LocalDateTime dt = ts.plusHours(hours);
        Timestamp start = Timestamp.valueOf(ts);
        Timestamp end = Timestamp.valueOf(dt);
        String sql = "select created_at,depth from creekdata.creekdata where created_at between '" + start + "' and '" + end + "' order by created_at";
        //System.out.println("date = "+sql);
        try {
            Connection conn = DriverManager.getConnection(url, sqlUser, sqlPassword);
            JDBCXYDataset jds = new JDBCXYDataset(conn);
            jds.executeQuery(sql);
            return jds;
        } catch (SQLException ex) {
            ex.printStackTrace(System.err);
        }
        return null;
    }
    
    static JDBCXYDataset createDataset(LocalDateTime ts) {
        
        Timestamp start = Timestamp.valueOf(ts);
        String sql = "select created_at,depth from creekdata.creekdata where created_at >'" + start + "' order by created_at";
        //System.out.println("date = "+sql);
        try {
            Connection conn = DriverManager.getConnection(url, sqlUser, sqlPassword);
            JDBCXYDataset jds = new JDBCXYDataset(conn);
            jds.executeQuery(sql);
            return jds;
        } catch (SQLException ex) {
            ex.printStackTrace(System.err);
        }
        return null;
    }
    
    static JDBCXYDataset createDataset(LocalDateTime ts, LocalDateTime tsend) {
        
        Timestamp start = Timestamp.valueOf(ts);
        String sql = "select created_at,depth from creekdata.creekdata where created_at >'" + start + "' and created_at<='"+tsend+"' order by created_at";
        //System.out.println("date = "+sql);
        try {
            Connection conn = DriverManager.getConnection(url, sqlUser, sqlPassword);
            JDBCXYDataset jds = new JDBCXYDataset(conn);
            jds.executeQuery(sql);
            return jds;
        } catch (SQLException ex) {
            ex.printStackTrace(System.err);
        }
        return null;
    }
}
