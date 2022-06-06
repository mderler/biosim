namespace BioSim;

public class SimulationAction
{
    private string _actionString;
    public string ActionString
    {
        get => _actionString;
        set
        {
            _actionString = value;
        }
    }

    // private (int gf, int gl, int sf, int sl)[]

    public SimulationAction(string eventString)
    {
        ActionString = eventString;
    }

    public SimulationAction() { }

    public void Execute(Simulation simulation)
    {

    }
}
