namespace BioSim;

// stores all IO Functions for Dits
public static class FunctionFactory
{
    private static Dictionary<string, InputFunction> _registerdInputFunctions = new Dictionary<string, InputFunction>();
    private static Dictionary<string, OutputFunction> _registerdOutputFunctions = new Dictionary<string, OutputFunction>();

    public static Dictionary<string, InputFunction> RegisterdInputFunctions => _registerdInputFunctions;
    public static Dictionary<string, OutputFunction> RegisterdOutputFunctions => _registerdOutputFunctions;

    // Store Function
    public static void RegisterIOFunction(string name, InputFunction func)
    {
        _registerdInputFunctions.Add(name, func);
    }

    // Store Function
    public static void RegisterIOFunction(string name, OutputFunction func)
    {
        _registerdOutputFunctions.Add(name, func);
    }
}