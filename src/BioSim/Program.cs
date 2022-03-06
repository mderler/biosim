using Gtk;

namespace BioSim;

public class Program
{
    public static void Main()
    {
        GuiTest();
    }

    static void GuiTest()
    {
        Application.Init();
        BioSimulatorGUI gui = new BioSimulatorGUI();
        // gui.ImageTest();
        Application.Run();
    }

    static void NeuralTest()
    {

    }
}