using System.Text.Json;

namespace BioSim;

// create new sim
public class CreateCommand : Command
{
    // constructor
    public CreateCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 2;
        _bannendWords.Add("all");
    }

    // run command
    protected override string Run(string[] args)
    {
        bool defaultSettings = args[1] == "default";

        if (!defaultSettings && !File.Exists(args[1]))
        {
            return $"this file does not exist: {args[1]}";
        }

        SimulationSettings settings = new SimulationSettings();

        if (!defaultSettings)
        {
            string jsonString = File.ReadAllText(args[1]);
            settings = JsonSerializer.Deserialize<SimulationSettings>(jsonString);
        }

        if (settings == null)
        {
            return "something went wrong reading simulation settings";
        }
        if (settings.version != ProgramVersion.version)
        {
            return "the setting version does not match";
        }

        Simulation sim = new Simulation(settings);

        if (_simulator.Simulations.ContainsKey(args[0]))
        {
            _simulator.Simulations[args[0]] = sim;
        }
        else
        {
            _simulator.Simulations.Add(args[0], sim);
        }

        HelperFunctions.SaveSimulation($"../../saves/{args[0]}.json", sim);

        return "";
    }

}

// quit command
public class QuitCommand : Command
{
    // constructor
    public QuitCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 0;
    }

    // run command
    protected override string Run(string[] args)
    {
        foreach (var item in _simulator.Simulations)
        {
            HelperFunctions.SaveSimulation($"../../saves/{item.Key}.json", item.Value);
        }

        _simulator.Running = false;
        return "";
    }
}

// run simulation
public class RunCommand : Command
{
    // constructor
    public RunCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    // run command
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

// halt simulation
public class HaltCommand : Command
{
    // constructor
    public HaltCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    // run command
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

// delete simulation
public class DeleteCommand : Command
{
    // constructor
    public DeleteCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    // run command
    protected override string Run(string[] args)
    {
        if (args.Contains("all"))
        {
            _simulator.Simulations.Clear();
            foreach (var item in Directory.GetFiles("../../saves/"))
            {
                File.Delete(item);
            }
        }
        if (!_simulator.Simulations.ContainsKey(args[0]))
        {
            return $"this simulation does not exists: {args[0]}";
        }

        _simulator.Simulations.Remove(args[0]);
        File.Delete($"../../saves/{args[0]}");
        return "";
    }
}

// show simulations
public class ShowCommand : Command
{
    // constructor
    public ShowCommand(BioSimulator simulator) : base(simulator) { _minArgs = 0; }

    // run command
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

// create template
public class CreateTemplateCommand : Command
{
    // constructor
    public CreateTemplateCommand(BioSimulator simulator) : base(simulator) { _minArgs = 1; }

    // run command
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

// export simulation save
public class ExportStateCommand : Command
{
    // constructor
    public ExportStateCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 2;
    }

    // run command
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

        HelperFunctions.SaveSimulation(args[1], _simulator.Simulations[args[0]]);

        return "";
    }
}

// import simulation save
public class ImportStateCommand : Command
{
    // constructor
    public ImportStateCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 2;
    }

    // run command
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

// list functions
public class FuncCommand : Command
{
    // constructor
    public FuncCommand(BioSimulator simulator) : base(simulator) { }

    // run command
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

// toggle autosave
public class AutoSaveCommand : Command
{
    // constructor
    public AutoSaveCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 1;
    }

    // run command
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

// create new action
public class ActionCommand : Command
{
    // constructor
    public ActionCommand(BioSimulator simulator) : base(simulator)
    {
        _minArgs = 3;
    }

    // run command
    protected override string Run(string[] args)
    {
        throw new NotImplementedException();
    }
}
