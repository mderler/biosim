namespace BioSim;

public abstract class Extension
{
    protected Simulation? _simulation;

    public virtual void OnSimulationStep() { }
}