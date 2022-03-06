
namespace BioSim;

public delegate void OutputFunction(ref Dit dit);

public static class OutputFunctions
{
    public static void PrintPositionTest(ref Dit dit)
    {
        System.Console.WriteLine(dit.position);
    }
}
