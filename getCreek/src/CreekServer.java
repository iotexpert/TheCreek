
import java.io.File;
import java.sql.Array;
import java.text.SimpleDateFormat;


public class CreekServer {

    static final String plotsDirectory = "creekPlots";
    
    public static void main(String[] args) {
        
        if(args.length == 0)
        {
            printHelp();
            return;
        }
        
        if(args[0].equals("MakeChart"))
        {
            
            MakeChart mc = new MakeChart();
            mc.run(args);
            
        }
       
        if(args[0].equals("ProcessEvents"))
        {
          
            ProcessEvents pe = new ProcessEvents();
            pe.run(args);
            createCharts(pe);
        }
        
    }
    
    static void printHelp()
    {
        System.out.println("CreekServer");
        System.out.println("MakeChart - Create a PNG from the creekdata Database");
        System.out.println("ProcessEvents - Look for Flood Events and Update Charts");
    }
    
    static void createCharts(ProcessEvents pe)
    {
        MakeChart mc = new MakeChart();

         try {
            pe.updateEventsFromDB();
            } catch (Exception e) {}
         
         int i;
        for (i = 0; i < pe.events.size(); i++) {
            StartEnd temp = pe.events.get(i);
            String fname = new SimpleDateFormat("yyyy-MM-dd-HH-mm-ss").format(temp.start);
            String path = plotsDirectory + "/" + fname + ".png";
                       
            File f = new File(path);
            if (!f.exists() || temp.end==null) {
                String start = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(temp.start);
                String end;
                
                if(temp.end != null)
                {
                    end = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(temp.end);
                    String[] args = {"MakeChart",path,start,end};                
                    mc.run(args);
                }
                else
                {
                    f.delete();
                    String[] args = {"MakeChart",path,start};                
                    mc.run(args);
                   
                }
                
            }

        }
    }
    
}
