using Gtk;

namespace BioSim;

public class Program
{
    // entry point
    public static void Main()
    {
        BioSimulator simulator = new BioSimulator();
        simulator.Run();
    }
}
