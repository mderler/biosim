namespace BioSim;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class SimulationParameterAttribute : Attribute
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

public class ExcludeParameterAttribute : Attribute { }

public class IncludeAllAsParametersAttribute : Attribute { }
