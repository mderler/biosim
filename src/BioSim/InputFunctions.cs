// TODO: Test if a class containing the input function with a reference to the
// simulation would be faster than passing a reference to the simulation
// everytime

namespace BioSim;

public delegate float InputFunction(Dit dit, Simulation simulation);

public static class InputFunctions
{
    public static float GetOneTest(Dit dit, Simulation simulation)
    {
        return 1f;
    }
}