namespace BioSim;

// stores all IO Functions for Dits
public class FunctionFactory
{
    private Dictionary<string, InputFunction> _registerdInputFunctions;
    private Dictionary<string, OutputFunction> _registerdOutputFunctions;

    public Dictionary<string, InputFunction> RegisterdInputFunctions => _registerdInputFunctions;
    public Dictionary<string, OutputFunction> RegisterdOutputFunctions => _registerdOutputFunctions;

    // constructor
    public FunctionFactory()
    {
        _registerdInputFunctions = new Dictionary<string, InputFunction>();
        _registerdOutputFunctions = new Dictionary<string, OutputFunction>();
    }

    // Store Function
    public void RegisterIOFunction(string name, InputFunction func)
    {
        _registerdInputFunctions.Add(name, func);
    }

    // Store Function
    public void RegisterIOFunction(string name, OutputFunction func)
    {
        _registerdOutputFunctions.Add(name, func);
    }
}