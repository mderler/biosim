namespace BioSim;

public class FunctionFactory
{
    private List<(string name, InputFunction func)> _registerdInputFunctions;
    private List<(string name, OutputFunction func)> _registerdOutputFunctions;

    public (string name, InputFunction func)[] RegisterdInputFunctions => _registerdInputFunctions.ToArray();
    public (string name, OutputFunction func)[] RegisterdOutputFunctions => _registerdOutputFunctions.ToArray();

    public FunctionFactory()
    {
        _registerdInputFunctions = new List<(string name, InputFunction func)>();
        _registerdOutputFunctions = new List<(string name, OutputFunction func)>();
    }

    public void RegisterIOFunction(string name, InputFunction func)
    {
        _registerdInputFunctions.Add((name, func));
    }

    public void RegisterIOFunction(string name, OutputFunction func)
    {
        _registerdOutputFunctions.Add((name, func));
    }
}