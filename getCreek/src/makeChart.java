
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
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class makeChart {

    static final int runTimeDefault = 8; // hours
    static final double maxDepthChart = 23.0; // feet
    static final String url = "jdbc:mysql://iotexpertpi.local/creekdata";
    static final String sqlUser = "creek";
    static final String sqlPassword = "creek";
    static final String chartFileName = "creekdepth.png"; // file name of chart
    static final double tickMarks = 1.0; // feet

    public static void main(String[] args) throws Exception {
        LocalDateTime ldt = null;
        Integer runTime = null;
        
        if (args.length == 1 && args[0].equals("-help")) {
            System.out.println("makeChart -help");
            System.out.println("makeChart chart of the last 8 hours");
            System.out.println("makeChart [hours] chart of last \"hours\"");
            System.out.println("makeChart [date] chart of date+8 hours");
            System.out.println("makeChart [date] [hours] chart of date+hours");
            System.out.println("Date can be yyyy-MM-dd hh:mm:ss or yyyy-MM-dd hh:mm or yyyy-MM-dd");
            return;
        }

        // if there is 1 argument try to do that date/time + 8 hours
        // if that doesnt work then try do do the current time - that # of hours
        if (args.length == 1) {
            boolean success;
            try {
                runTime = new Integer(args[0]);
                ldt = LocalDateTime.now().minusHours(runTime);
                success = true;
            } catch (Exception e) {
                success = false;
            }

            if (success == false) // if it isnt an integer.. then try dates
            {
                try {
                    ldt = convertStringDateTime(args[0]);
                } catch (Exception e) {
                    System.out.println(e);
                    return;
                }
            }
        }

        if (args.length == 2) {
            try {
                ldt = convertStringDateTime(args[0]);
            } catch (Exception e) {
                System.out.println("arg[0] must be date");
                return;
            }

            try {
                runTime = new Integer(args[1]);
            } catch (Exception e) {
                System.out.println("arg[1] must be Integer Hours");
                return;
            }
        }

        // If there is no arguments then do the last 8 hours
        if (runTime == null)
            runTime = runTimeDefault;
        if (ldt == null) 
            ldt = LocalDateTime.now().minusHours(runTime);
        
        JDBCXYDataset data = createDatasetHours(ldt, runTime);
        JFreeChart chart = ChartFactory.createTimeSeriesChart(
                "Elkhorn Creek Water Level", // chart title
                "Timestamp", // x-axis label
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

        ChartUtilities.saveChartAsPNG(new File(chartFileName), chart, 1000, 700);
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

    static JDBCXYDataset createDatasetHours(LocalDateTime ts, long hours) {

        LocalDateTime dt = ts.plusHours(hours);
        Timestamp start = Timestamp.valueOf(ts);
        Timestamp end = Timestamp.valueOf(dt);
        String sql = "select created_at,depth from creekdata.creekdata where created_at between '" + start + "' and '" + end + "' order by created_at";

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
