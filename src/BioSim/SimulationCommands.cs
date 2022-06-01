using System.Text.Json;

namespace BioSim;

public class CreateCommand : Command
{
    public CreateCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        if (!File.Exists(args[0]))
        {
            return $"this file does not exist: {args[0]}";
        }
        return "";
    }
}

public class QuitCommand : Command
{
    public QuitCommand(BioSimulator simulator) : base(simulator) { _minArgs = 0; }

    protected override string Run(string[] args)
    {
        _simulator.Running = false;
        return "";
    }
}

public class RunCommand : Command
{
    public RunCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        if (!_simulator.Simulations.ContainsKey(args[0]))
        {
            return $"this simulation does not exists: {args[0]}";
        }
        _simulator.RunningSimulations.Add(args[0], _simulator.Simulations[args[0]]);
        return "";
    }
}

public class HaltCommand : Command
{
    public HaltCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        if (!_simulator.RunningSimulations.ContainsKey(args[0]))
        {
            return $"this simulation does not exists: {args[0]}";
        }
        _simulator.RunningSimulations.Remove(args[0]);
        return "";
    }
}

public class DeleteCommand : Command
{
    public DeleteCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        if (!_simulator.Simulations.ContainsKey(args[0]))
        {
            return $"this simulation does not exists: {args[0]}";
        }
        if (_simulator.RunningSimulations.ContainsKey(args[0]))
        {
            _simulator.RunningSimulations.Remove(args[0]);
        }
        if (args.Contains("save"))
        {
            // save
        }
        _simulator.Simulations.Remove(args[0]);
        return "";
    }
}

public class ShowCommand : Command
{
    public ShowCommand(BioSimulator simulator) : base(simulator) { _minArgs = 0; }

    protected override string Run(string[] args)
    {
        string output = "";
        foreach (var item in _simulator.Simulations)
        {
            output += "---------\n";
            output += item.Key + "\n";
            output += $"step: {3} generation: {0}\n";
        }
        return output;
    }
}

public class CreateTemplateCommand : Command
{
    public CreateTemplateCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        char[] sarr = { '/', '\\' };

        int index = args[0].LastIndexOfAny(sarr);
        string path = args[0].Remove(index);
        if (!Directory.Exists(path))
        {
            return $"the path does not exist {path}";
        }
        if (Directory.Exists(args[0]))
        {
            return $"{args[0]} is a directory not a file";
        }
        File.Copy(Resource.CombinePath("template.json"), args[0]);
        return "";
    }
}

public class SaveStateCommand : Command
{
    public SaveStateCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 2;
    }

    protected override string Run(string[] args)
    {
        if (_simulator.Simulations.ContainsKey(args[0]))
        {
            return $"simulation {args[0]} not found";
        }
        char[] sarr = { '/', '\\' };

        int index = args[1].LastIndexOfAny(sarr);
        string path = args[1].Remove(index);

        if (!Directory.Exists(path))
        {
            return $"the path does not exist {path}";
        }
        if (Directory.Exists(args[1]))
        {
            return $"{args[1]} is a directory, not a file";
        }

        // JsonSerializer.Serialize(args[1], _simulator.Simulations[args[1]], typeof(Simulation));
        return "";
    }
}
