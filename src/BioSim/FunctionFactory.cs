namespace BioSim;

// stores all IO Functions for Dits
public static class FunctionFactory
{
    // registerd input functions
    private static Dictionary<string, InputFunction> _registerdInputFunctions = new Dictionary<string, InputFunction>();
    // registerd output functions
    private static Dictionary<string, OutputFunction> _registerdOutputFunctions = new Dictionary<string, OutputFunction>();

    // get input functions
    public static Dictionary<string, InputFunction> RegisterdInputFunctions => _registerdInputFunctions;
    // set input functions
    public static Dictionary<string, OutputFunction> RegisterdOutputFunctions => _registerdOutputFunctions;

    // Store Function
    public static void RegisterIOFunction(string name, InputFunction func)
    {
        if (_registerdInputFunctions.ContainsKey(name))
        {
            _registerdInputFunctions[name] = func;
            return;
        }
        _registerdInputFunctions.Add(name, func);
    }

    // Store Function
    public static void RegisterIOFunction(string name, OutputFunction func)
    {
        if (_registerdOutputFunctions.ContainsKey(name))
        {
            _registerdOutputFunctions[name] = func;
            return;
        }
        _registerdOutputFunctions.Add(name, func);
    }
}