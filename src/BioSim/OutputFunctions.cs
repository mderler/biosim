
namespace BioSim;

public delegate void OutputFunction(in Dit dit, Simulation simulation);

// static class to define all output Functions
public static class OutputFunctions
{
    // move dit to north
    public static void MoveNorth(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.Position;
        position.y--;
        simulation.SimEnv.TryMove(dit, position);
    }

    // move dit to south
    public static void MoveSouth(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.Position;
        position.y++;
        simulation.SimEnv.TryMove(dit, position);
    }

    // move dit to west
    public static void MoveWest(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.Position;
        position.x--;
        simulation.SimEnv.TryMove(dit, position);
    }

    // move dit to east
    public static void MoveEast(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.Position;
        position.x++;
        simulation.SimEnv.TryMove(dit, position);
    }
}
