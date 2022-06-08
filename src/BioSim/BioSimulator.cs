using System.Text.Json;

namespace BioSim;

// Class that controls the Terminal
public class BioSimulator
{
    // get and set loaded Simulations
    public Dictionary<string, Simulation> Simulations
    {
        get;
        private set;
    }

    // registerd commands
    private Dictionary<string, Command> _commands;
    // last auto save
    private DateTime _last;

    // get and set; if false, the programs stops
    public bool Running { get; set; }
    // get and set; if true, the simulations will be saved automaticly every 30min
    public bool AutoSave { get; set; }

    // get ActionManager
    public ActionManager SimActionManager
    {
        get;
    }

    // constructor
    public BioSimulator()
    {
        Running = true;
        AutoSave = true;
        // SimActionManager = new ActionManager();
        _last = DateTime.Now;

        Simulations = new Dictionary<string, Simulation>();

        FunctionFactory.RegisterIOFunction("near_to_east", InputFunctions.NearToEast);
        FunctionFactory.RegisterIOFunction("near_to_south", InputFunctions.NearToSouth);
        FunctionFactory.RegisterIOFunction("move_east", OutputFunctions.MoveEast);
        FunctionFactory.RegisterIOFunction("move_west", OutputFunctions.MoveWest);
        FunctionFactory.RegisterIOFunction("move_north", OutputFunctions.MoveNorth);
        FunctionFactory.RegisterIOFunction("move_south", OutputFunctions.MoveSouth);

        _commands = new Dictionary<string, Command>();
        _commands.Add("create", new CreateCommand(this));
        _commands.Add("quit", new QuitCommand(this));
        _commands.Add("run", new RunCommand(this));
        _commands.Add("halt", new HaltCommand(this));
        _commands.Add("delete", new DeleteCommand(this));
        _commands.Add("show", new ShowCommand(this));
        _commands.Add("template", new CreateTemplateCommand(this));
        _commands.Add("export", new ExportStateCommand(this));
        _commands.Add("import", new ImportStateCommand(this));
        _commands.Add("funcs", new FuncCommand(this));
        _commands.Add("autosave", new AutoSaveCommand(this));

        // load sims

        const string savePath = "../../saves/";

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        var simsPaths = Directory.EnumerateFiles(savePath);
        foreach (var item in simsPaths)
        {
            string jsonString = File.ReadAllText(item);
            Simulation sim = JsonSerializer.Deserialize<Simulation>(jsonString);
            Simulations.Add(new FileInfo(item).Name.Split('.')[0], sim);
        }
    }

    // run the Bio Simulator
    public void Run()
    {
        StringHolder strHolder = new StringHolder();

        Thread inputThread = StartInputThread(strHolder);

        string input;
        bool writing = false;

        while (Running)
        {
            UpdateSimulations();

            if (!writing)
            {
                Console.Write("-> ");
            }

            if (inputThread.IsAlive)
            {
                writing = true;
                continue;
            }

            input = strHolder.str;

            if (string.IsNullOrEmpty(input))
            {
                inputThread = StartInputThread(strHolder);
                continue;
            }

            string[] tmp = input.TrimEnd().Split(' ');
            string command = tmp[0];
            string[] args = new string[tmp.Length - 1];
            for (int i = 1; i < tmp.Length; i++)
            {
                args[i - 1] = tmp[i];
            }

            string output = $"the command does not exist ({command})";
            if (_commands.ContainsKey(command))
            {
                output = _commands[command].RunCommand(args);
            }

            Console.WriteLine(output);
            writing = false;

            if (Running)
            {
                inputThread = StartInputThread(strHolder);
            }
        }
    }

    // set command line input
    private void SetInput(object obj)
    {
        var input = (StringHolder)obj;
        input.str = Console.ReadLine();
    }

    // start and get the Thread that handles the input
    private Thread StartInputThread(StringHolder strHolder)
    {
        Thread thread = new Thread(SetInput);
        thread.Start(strHolder);

        _runningSims.Clear();
        foreach (var item in Simulations)
        {
            if (item.Value.CurrentState == SimulationState.Running)
            {
                _runningSims.Add(item.Value);
            }
        }

        return thread;
    }

    // list of simulations that are running
    private List<Simulation> _runningSims = new List<Simulation>();
    // update the running simulations
    private void UpdateSimulations()
    {
        foreach (var item in _runningSims)
        {
            item.Update();
        }

        if (AutoSave && (DateTime.Now - _last > TimeSpan.FromMinutes(30d)))
        {
            System.Console.WriteLine("asdasdsad");
            foreach (var item in Simulations)
            {
                HelperFunctions.SaveSimulation($"../../saves/{item.Key}.json", item.Value);
            }

            _last = DateTime.Now;
        }

    }

    // class that holds a string
    private class StringHolder
    {
        public string str;
    }
}
