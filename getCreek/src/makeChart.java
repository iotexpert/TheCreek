import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;


import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartUtilities;
import org.jfree.chart.ChartUtilities.*;
import org.jfree.chart.JFreeChart;
import org.jfree.data.jdbc.JDBCXYDataset;
import org.jfree.data.xy.XYDataset;

import java.io.File;

public class makeChart {

	/**
	 * @param args
	 */
	public static void main(String[] args) {

		try {

			XYDataset data = readData(60*8,null);

			// create the chart...
			JFreeChart chart = ChartFactory.createTimeSeriesChart(
					"Elkhorn Creek Water Level", // chart title
					"Timestamp",				// x-axis label
					"Depth - Feet",            // y-axis label
					data,                       // data
					true,                       // include legend
					true,                      // generate tool tips
					false                     // generate URLs
					);

			File creekchart=new File("xxx1.PNG");
			ChartUtilities.saveChartAsPNG(creekchart, chart, 1400, 1000);
			
	            

            /* Convert JFreeChart to JPEG File Using Code below */
            //ChartUtilities.saveChartAsJPEG(creekchart, quality, myColoredChart,width,height);
		}
		catch (Exception e) {
			System.err.println(e.toString());
		}


	}


static XYDataset readData(Integer amount,String sdate) {

	JDBCXYDataset data = null;

	String url = "jdbc:mysql://192.168.15.83/creekdata";
	Connection con;

	try {
		Class.forName("com.mysql.jdbc.Driver");
	}
	catch (ClassNotFoundException e) {
		System.err.print("ClassNotFoundException: ");
		System.err.println(e.getMessage());
	}

	try {
		con = DriverManager.getConnection(url, "creek", "creek");

		data = new JDBCXYDataset(con);

		//			String sql = "SELECT ts,depth,60min FROM ddata where ts>\"" + sdate + "\" order by ts limit " + amount +";";
		amount = 8 * 60;
		String sql = "SELECT ts,filter FROM ddata order by ts desc limit "+ amount + ";" ;

		data.executeQuery(sql);

		con.close();
	}

	catch (SQLException e) {
		System.err.print("SQLException: ");
		System.err.println(e.getMessage());
	}

	catch (Exception e) {
		System.err.print("Exception: ");
		System.err.println(e.getMessage());
	}

	return data;

}

}


