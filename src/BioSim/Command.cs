namespace BioSim;

// base class for commands
public abstract class Command
{
    // the BioSimulator to be used by the command
    protected BioSimulator _simulator;
    protected ArgsChecker _checker;

    public Command(BioSimulator simulator)
    {
        _simulator = simulator;
        _checker = new ArgsChecker();
    }
    // the run method that a child command must use
    protected abstract string Run(string[] args);

    // wraps the Run method for checks
    public string RunCommand(string[] args)
    {
        string error;
        if (!_checker.Check(args, out error))
        {
            return error;
        }
        return Run(args);
    }
}
