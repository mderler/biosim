namespace BioSim;

// stores all IO Functions for Dits
public class FunctionFactory
{
    private List<(string name, InputFunction func)> _registerdInputFunctions;
    private List<(string name, OutputFunction func)> _registerdOutputFunctions;

    public (string name, InputFunction func)[] RegisterdInputFunctions => _registerdInputFunctions.ToArray();
    public (string name, OutputFunction func)[] RegisterdOutputFunctions => _registerdOutputFunctions.ToArray();

    // constructor
    public FunctionFactory()
    {
        _registerdInputFunctions = new List<(string name, InputFunction func)>();
        _registerdOutputFunctions = new List<(string name, OutputFunction func)>();
    }

    // Store Function
    public void RegisterIOFunction(string name, InputFunction func)
    {
        _registerdInputFunctions.Add((name, func));
    }

    // Store Function
    public void RegisterIOFunction(string name, OutputFunction func)
    {
        _registerdOutputFunctions.Add((name, func));
    }
}