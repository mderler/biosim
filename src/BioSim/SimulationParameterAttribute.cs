namespace BioSim;

public abstract class SimulationParameterAttribute : Attribute
{
    public string Name
    {
        get;
    }

    public SimulationParameterAttribute(string name)
    {
        Name = name;
    }
}

public class NumericalSimulationParameterAttribute : SimulationParameterAttribute
{
    public object? defaultValue = null;
    public object? min;
    public object? max;
    public NumericalSimulationParameterAttribute(string name) : base(name)
    {

    }
}

public class TypeSimulationParameterAttribte : SimulationParameterAttribute
{
    public TypeSimulationParameterAttribte(string name) : base(name)
    {

    }
}
