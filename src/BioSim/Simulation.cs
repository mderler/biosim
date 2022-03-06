namespace BioSim;

public class Simulation
{
    private List<Dit> _dits = new List<Dit>();

    private int currentStep = 0;

    private SimulationSettings _settings;

    public Simulation(SimulationSettings settings)
    {
        _settings = settings;
    }

    public void DoStep()
    {
        
    }
}