

package org.elkhorncreek;

import java.io.IOException;
import java.io.OutputStream;
import java.io.OutputStreamWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartUtilities;
import org.jfree.chart.JFreeChart;
import org.jfree.chart.plot.CategoryPlot;
import org.jfree.chart.plot.PlotOrientation;
import org.jfree.data.category.DefaultCategoryDataset;


import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;


import org.jfree.chart.ChartFrame;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.JFreeChart;
import org.jfree.chart.plot.PlotOrientation;
import org.jfree.data.category.CategoryDataset;
import org.jfree.data.category.DefaultCategoryDataset;
import org.jfree.data.general.DefaultPieDataset;
import org.jfree.data.jdbc.JDBCCategoryDataset;
import org.jfree.data.jdbc.JDBCXYDataset;
import org.jfree.data.xy.DefaultXYDataset;
import org.jfree.data.xy.XYDataset;
import org.jfree.chart.*;


public class CreateChart extends HttpServlet {

	/**
	 * Creates a new demo.
	 */


	public CreateChart() {
		// nothing required
	}

	/**
	 * Processes a GET request.
	 *
	 * @param request  the request.
	 * @param response  the response.
	 *
	 * @throws ServletException if there is a servlet related problem.
	 * @throws IOException if there is an I/O problem.
	 */
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		OutputStream out = response.getOutputStream();

		Integer amount;

		String dpoint = request.getParameter("dpoints");


		if(dpoint == null)
		{
			amount = new Integer(7 * 24 * 12);
		}
		else
			amount = new Integer(dpoint);


		String sdate;

		String sdatep = request.getParameter("sdate");


		if(sdatep == null)
		{
			sdate = new String("2011-11-28");
		}
		else
			sdate = sdatep;



		String csv = request.getParameter("csv");




		try {

			XYDataset data = readData(amount,sdate);
		//	XYDataset datatwo = readDataSecond(amount,sdate);

			if(csv == null)
			{

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
				
	//			CategoryPlot plot=chart.getCategoryPlot();
	//			plot.setDataset(1,datatwo);
	//			plot.mapDatasetToRangeAxes(1,1);

				response.setContentType("image/png");
				ChartUtilities.writeChartAsPNG(out, chart, 1400, 1000);
			}

			else
			{
				response.setContentType("text/csv");
				int i = data.getItemCount(0);
				OutputStreamWriter ow = new OutputStreamWriter(out);
				ow.write("Timestamp,ft\n");
				for(int j=0;j<i;j++)
				{
					ow.write(data.getX(0,j) + "," + data.getY(0,j) + "\n");
				}
			}

		}
		catch (Exception e) {
			System.err.println(e.toString());
		}
		finally {


			out.close();
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
			String sql = "SELECT ts,round(depth,2) FROM ddata order by ts desc limit "+ amount + ";" ;

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
	
	static XYDataset readDataSecond(Integer amount,String sdate) {

		JDBCXYDataset data = null;

		String url = "jdbc:mysql://192.168.15.80/d1";
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

			String sql = "SELECT ts,60min FROM ddata where ts>\"" + sdate + "\" order by ts limit " + amount +";";

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
