namespace BioSim;

abstract class SimulationParameter
{
    public string Name
    {
        get;
    }

    public SimulationParameter(string name)
    {
        Name = name;
    }

    public abstract bool CheckInput(string input);
}

class IntergerParameter : SimulationParameter
{
    private int? _defaultValue;
    private int _min;
    private int _max;
    public IntergerParameter(string name, int? defaultValue = null, int min = int.MinValue, int max = int.MaxValue) : base(name)
    {
        _defaultValue = defaultValue;
        _min = min;
        _max = max;
    }

    public override bool CheckInput(string input)
    {
        throw new NotImplementedException();
    }
}

class FloatingParameter : SimulationParameter
{
    private float? _defaultValue;
    private float _min;
    private float _max;
    public FloatingParameter(string name, float? defaultValue = null, float min = float.MinValue, float max = float.MaxValue) : base(name)
    {
        _defaultValue = defaultValue;
        _min = min;
        _max = max;
    }

    public override bool CheckInput(string input)
    {
        throw new NotImplementedException();
    }
}
