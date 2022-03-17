using Gtk;

namespace BioSim;

public class Program
{
    public static void Main()
    {
        Application.Init();
        BioSimulatorGUI gui = new BioSimulatorGUI();
        // gui.ImageTest();
        Application.Run();
    }
}
