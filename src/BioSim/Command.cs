namespace BioSim;

public abstract class Command
{
    protected BioSimulator _simulator;
    protected uint _minArgs = 0;
    protected List<string> _bannendWords = new List<string>();
    public Command(BioSimulator simulator)
    {
        _simulator = simulator;
    }
    protected abstract string Run(string[] args);

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
