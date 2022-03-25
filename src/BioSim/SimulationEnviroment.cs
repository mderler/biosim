namespace BioSim;

class SimulationEnviroment
{
    public Map SimMap { get; set; }

    public SimulationEnviroment(Map simMap)
    {
        SimMap = simMap;
    }

    public void TryMove(in Dit dit, (int x, int y) newPosition)
    {

    }
}
