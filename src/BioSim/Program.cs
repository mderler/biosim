using Gtk;

namespace BioSim;

public class Program
{
    public static void Main()
    {
        Application.Init();
        BioSimulator gui = new BioSimulator();
        // gui.ImageTest();
        Application.Run();
    }
}
