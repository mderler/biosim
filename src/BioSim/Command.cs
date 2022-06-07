namespace BioSim;

// base class for commands
public abstract class Command
{
    // the BioSimulator to be used by the command
    protected BioSimulator _simulator;
    // define how many args the user must give
    protected uint _minArgs = 0;
    // list of words the user cant use
    protected List<string> _bannendWords = new List<string>();
    // constructor
    public Command(BioSimulator simulator)
    {
        _simulator = simulator;
    }
    // the run method that a child command must use
    protected abstract string Run(string[] args);

    // wraps the Run method for checks
    public string RunCommand(string[] args)
    {
        if (args.Length < _minArgs)
        {
            return $"{_minArgs - args.Length} argument(s) are missing";
        }
        foreach (var item in args)
        {
            if (string.IsNullOrEmpty(item))
            {
                return "the args must not be empty";
            }
            if (_bannendWords.Contains(item))
            {
                return $"the expression '{item}' is preserved";
            }
        }

        return Run(args);
    }
}
