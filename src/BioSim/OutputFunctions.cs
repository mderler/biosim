// TODO: Test if a class containing the output function with a reference to the
// simulation would be faster than passing a reference to the simulation
// everytime

namespace BioSim;

public delegate void OutputFunction(in Dit dit, Simulation simulation);

public static class OutputFunctions
{
    public static void SetDitPositionToZeroTest(in Dit dit, Simulation simulation)
    {
        dit.position = (0 , 0);
    }
}
