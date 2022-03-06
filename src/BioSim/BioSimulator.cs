public class BioSimulator
{
    private List<Simulation> _simulations;
    private readonly FunctionFactory _simulationFactory;

    public BioSimulator()
    {
        _simulations = new List<Simulation>();

        _simulationFactory = new FunctionFactory();
        _simulationFactory.RegisterIOFunction("Input Test", InputFunctions.GetOneTest);
        _simulationFactory.RegisterIOFunction("Output Test", OutputFunctions.PrintPositionTest);
    }

    public void AddSimulation(Simulation simulation)
    {
        _simulations.Add(simulation);
    }
}