using Gtk;

public class Program
{
    public static void Main()
    {
        Application.Init();
        BioSimulatorGUI gui = new BioSimulatorGUI();
        // gui.ImageTest();
        gui.Run();
    }
}