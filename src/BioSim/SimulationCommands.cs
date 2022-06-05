using System.Text.Json;

namespace BioSim;

public class CreateCommand : Command
{
    public CreateCommand(BioSimulator simulator) : base(simulator) { _minArgs = 2; }

    protected override string Run(string[] args)
    {
        if (!File.Exists(args[1]))
        {
            return $"this file does not exist: {args[1]}";
        }
        string jsonString = File.ReadAllText(args[1]);

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.Converters.Add(new SimulationSettingsConverter());
        SimulationSettings settings = JsonSerializer.Deserialize<SimulationSettings>(jsonString);
        if (settings == null)
        {
            return "something went wrong reading simulation settings";
        }
        if (settings.version != ProgramVersion.version)
        {
            return "the setting version does not match";
        }

        SLLModel model = new SLLModel(settings.mutateChance, settings.mutateStrength);
        model.InnerCount = settings.innerCount;

        Simulation sim = new Simulation(settings);

        sim.Setup();

        _simulator.Simulations.Add(args[0], sim);

        return "";
    }

}

public class QuitCommand : Command
{
    public QuitCommand(BioSimulator simulator) : base(simulator) { _minArgs = 0; }

    protected override string Run(string[] args)
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.IncludeFields = true;

        foreach (var item in _simulator.Simulations)
        {
            string jsonString = JsonSerializer.Serialize(item.Value, options);
            File.WriteAllText($"../../saves/{item.Key}.json", jsonString);
        }

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
        _simulator.Simulations[args[0]].CurrentState = SimulationState.Running;

        return "";
    }
}

public class HaltCommand : Command
{
    public HaltCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    protected override string Run(string[] args)
    {
        if (!_simulator.Simulations.ContainsKey(args[0]))
        {
            return $"this simulation does not exists: {args[0]}";
        }
        _simulator.Simulations[args[0]].CurrentState = SimulationState.Halted;
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
            string state = "";
            switch (item.Value.CurrentState)
            {
                case SimulationState.Running: state = "running"; break;
                case SimulationState.Halted: state = "halted"; break;
                case SimulationState.Finished: state = "finished"; break;
                case SimulationState.Extinct: state = "extinct"; break;
            }
            output += "---------\n";
            output += item.Key + "\n";
            output += $"step: {item.Value.CurrentStep}/{item.Value.Settings.steps}; ";
            output += $"generation: {item.Value.CurrentGeneration}/{item.Value.Settings.generations}\n";
            output += state;
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

        SimulationSettings settings = new SimulationSettings();
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.IncludeFields = true;
        options.WriteIndented = true;

        File.WriteAllText(args[0], JsonSerializer.Serialize<SimulationSettings>(settings, options));
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
        if (!_simulator.Simulations.ContainsKey(args[0]))
        {
            return $"simulation ({args[0]}) not found";
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

        JsonSerializerOptions options = new JsonSerializerOptions();
        options.WriteIndented = true;

        string jsonString = JsonSerializer.Serialize<Simulation>(_simulator.Simulations[args[0]], options);
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

public class FuncCommand : Command
{
    public FuncCommand(BioSimulator simulator) : base(simulator) { }

    protected override string Run(string[] args)
    {
        string output = "";

        output += "input functions:\n";
        foreach (var item in FunctionFactory.RegisterdInputFunctions.Keys)
        {
            output += $"{item}\n";
        }

        output += "\noutput functions:\n";
        foreach (var item in FunctionFactory.RegisterdOutputFunctions.Keys)
        {
            output += $"{item}\n";
        }

        return output;
    }
}

public class AutoSaveCommand : Command
{
    public AutoSaveCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 1;
    }

    protected override string Run(string[] args)
    {
        string output = "";

        output = "the argument must ether be 'on' or 'off'";
        if (args[0] == "on")
        {
            _simulator.AutoSave = true;
            output = "autosave is now on";
        }
        if (args[0] == "off")
        {
            _simulator.AutoSave = false;
            output = "autosave is now off";
        }

        return output;
    }
}
