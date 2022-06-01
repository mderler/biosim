using System.Text.Json;

namespace BioSim;

public class CreateCommand : Command
{
    public CreateCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        if (!File.Exists(args[1]))
        {
            return $"this file does not exist: {args[1]}";
        }
        string jsonString = File.ReadAllText(args[1]);

        Simulation sim = new Simulation();
        SimulationSettings settings = JsonSerializer.Deserialize<SimulationSettings>(jsonString);
        if (settings == null)
        {
            return "something went wrong reading simulation settings";
        }
        if (settings.version != ProgramVersion.version)
        {
            return "the setting version does not match";
        }

        Map map = new Map(settings.mapPath);
        List<InputFunction> inputFunctions = new List<InputFunction>();
        List<OutputFunction> outputFunctions = new List<OutputFunction>();

        foreach (var item in settings.inputFunctions)
        {
            if (!_simulator.IOFactory.RegisterdInputFunctions.ContainsKey(item))
            {
                return $"input function ({item}) does not exist";
            }

            inputFunctions.Add(_simulator.IOFactory.RegisterdInputFunctions[item]);
        }

        foreach (var item in settings.outputFunctions)
        {
            if (!_simulator.IOFactory.RegisterdOutputFunctions.ContainsKey(item))
            {
                return $"output function ({item}) does not exist";
            }

            outputFunctions.Add(_simulator.IOFactory.RegisterdOutputFunctions[item]);
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
            Simulation sim = _simulator.Simulations[args[0]];
            string jsonString = JsonSerializer.Serialize<Simulation>(sim);
            File.WriteAllText($"../../saves/{args[0]}.json", jsonString);
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
        File.Copy(JsonSerializer.Serialize<SimulationSettings>(new SimulationSettings()), args[0]);
        return "";
    }
}

public class ExportStateCommand : Command
{
    public ExportStateCommand(BioSimulator simulator) : base(simulator)
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

        string jsonString = JsonSerializer.Serialize(_simulator.Simulations[args[1]]);
        File.WriteAllText(args[1], jsonString);

        return "";
    }
}

public class ImportStateCommand : Command
{
    public ImportStateCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 2;
    }

    protected override string Run(string[] args)
    {
        if (!File.Exists(args[1]))
        {
            return $"the file does not exist {args[1]}";
        }

        string jsonString = File.ReadAllText(args[1]);
        Simulation sim = JsonSerializer.Deserialize<Simulation>(jsonString);

        if (sim == null)
        {
            return $"something went wrong importing the simulation";
        }

        _simulator.Simulations.Add(args[0], sim);

        return "";
    }
}
