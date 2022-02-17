using Gtk;

public class Program
{
    public static void Main()
    {
        Application.Init();
        BioSimulator gui = new BioSimulator();
        // gui.ImageTest();
        gui.Run();
    }
}