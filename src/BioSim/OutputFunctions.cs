
namespace BioSim;

public delegate void OutputFunction(in Dit dit);

public static class OutputFunctions
{
    public static void PrintPositionTest(in Dit dit)
    {
        System.Console.WriteLine(dit.position);
    }
}
