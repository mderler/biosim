using Gtk;

public class Program
{
    public static void Main()
    {
        // GuiTest();
    }

    static void GuiTest()
    {
        Application.Init();
        BioSimulator gui = new BioSimulator();
        gui.ImageTest();
        Application.Run();
    }

    static void NeuralTest()
    {


        Dit[] dits = new Dit[10];
        for (int i = 0; i < dits.Length; i++)
        {
            // dits[i] = new Dit();
        }
    }
}