namespace BioSim;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class SimulationParameterAttribute : Attribute
{
    public bool Include
    {
        get;
    }
    public string Name
    {
        get;
    }

    public SimulationParameterAttribute(string name)
    {
        Name = name;
        Include = false;
    }
}

public class ExcludeParameterAttribute : Attribute { }

public class IncludeAllAsParametersAttribute : Attribute { }
