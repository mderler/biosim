namespace BioSim;

public abstract class Command
{
    protected BioSimulator _simulator;
    protected uint _minArgs = 0;
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
        }

        return Run(args);
    }
}
