namespace BioSim;

public class BioSimulator
{
    private List<Simulation> _simulations;
    private readonly FunctionFactory _simulationFactory;

    public BioSimulator()
    {
        _simulations = new List<Simulation>();

        _simulationFactory = new FunctionFactory();
    }

    public void AddSimulation(Simulation simulation)
    {
        _simulations.Add(simulation);
    }
}