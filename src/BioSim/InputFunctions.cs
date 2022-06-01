namespace BioSim;

public delegate float InputFunction(Dit dit, Simulation simulation);

// static class to define all input functions
public static class InputFunctions
{
    // return a high value whet a Dit is near to South
    public static float NearToSouth(Dit dit, Simulation simulation)
    {
        return (float)dit.position.y / simulation.SimMap.Height;
    }

    // return a high value when a Dit is near to East
    public static float NearToEast(Dit dit, Simulation simulation)
    {
        return (float)dit.position.x / simulation.SimMap.Width;
    }
}