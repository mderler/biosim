namespace BioSim;

public abstract class SimulationAction
{
    protected string[] _args;
    public Simulation Simulation { get; set; }
    protected string _actionString;

    public string Error { get; protected set; }

    // private (int gf, int gl, int sf, int sl)[]

    public SimulationAction(string eventString)
    {
        _actionString = eventString;
    }

    public void ExecuteAction()
    {

    }

    protected virtual void Start() { }
    protected virtual void Execute() { }
    protected virtual void End() { }

    public abstract SimulationAction Copy();
}
