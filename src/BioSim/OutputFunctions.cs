// TODO: Test if a class containing the output function with a reference to the
// simulation would be faster than passing a reference to the simulation
// everytime

namespace BioSim;

public delegate void OutputFunction(in Dit dit, Simulation simulation);

public static class OutputFunctions
{
    public static void MoveNorth(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.position;
        position.y--;
        simulation.SimEnv.TryMove(dit, position);
    }

    public static void MoveSouth(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.position;
        position.y++;
        simulation.SimEnv.TryMove(dit, position);
    }

    public static void MoveWest(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.position;
        position.x--;
        simulation.SimEnv.TryMove(dit, position);
    }

    public static void MoveEast(in Dit dit, Simulation simulation)
    {
        (int x, int y) position = dit.position;
        position.x++;
        simulation.SimEnv.TryMove(dit, position);
    }
}
