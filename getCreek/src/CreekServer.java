
import java.io.File;
import java.text.SimpleDateFormat;

public class CreekServer {

    static final String plotsDirectory = "creekPlots";

    public static void main(String[] args) {

        if (args.length == 0) {
            printHelp();
            return;
        }
        else
        if (args[0].equals("MakeChart")) {

            MakeChart mc = new MakeChart();
            mc.run(args);

        }
        else
        if (args[0].equals("ProcessEvents")) {

            ProcessEvents pe = new ProcessEvents();
            pe.run(args);
            createCharts(pe);
            try {pe.createHtml("creekPlots/floods.html"); } catch (Exception e){}
        }
        else
        if (args[0].equals("GetData")) {
            i2cdb i2c = new i2cdb();
            i2c.run(args);
        }
        else
        if (args[0].equals("All")) {
            // get the data
            
            // GetData ... no arguments
            
            // make the current chart
            
            // MakeChart creekPlots/current.png
            
            // ProcessEvents ... no arguments
            
        }
        
        else
            printHelp();

    }

    static void printHelp() {
        System.out.println("CreekServer");
        System.out.println("MakeChart - Create a PNG from the creekdata Database");
        System.out.println("ProcessEvents - Look for Flood Events and Update Charts");
        System.out.println("GetData - read the config.properties file, then request the data from the I2CSlave");

    }

    static void createCharts(ProcessEvents pe) {
        MakeChart mc = new MakeChart();

        try {
            pe.updateEventsFromDB();
        } catch (Exception e) {
        }

        int i;
        for (i = 0; i < pe.events.size(); i++) {
            StartEnd temp = pe.events.get(i);
            String fname = new SimpleDateFormat("yyyy-MM-dd-HH-mm-ss").format(temp.start);
            String path = plotsDirectory + "/" + fname + ".png";

            File f = new File(path);
            if (!f.exists() || temp.end == null) {
                String start = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(temp.start);
                String end;

                if (temp.end != null) {
                    end = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(temp.end);
                    String[] args = {"MakeChart", path, start, end};
                    mc.run(args);
                } else {
                    f.delete();
                    String[] args = {"MakeChart", path, start};
                    mc.run(args);

                }

            }
        }
    }

}
