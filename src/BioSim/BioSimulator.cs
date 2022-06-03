using System.Text.Json;

namespace BioSim;

public class BioSimulator
{
    public Dictionary<string, Simulation> Simulations
    {
        get;
        private set;
    }

    private Dictionary<string, Simulation> _runningSimulations;
    public Dictionary<string, Simulation> RunningSimulations
    {
        get
        {

            return _runningSimulations;

        }
    }
    public FunctionFactory IOFactory
    {
        get;
        private set;
    }
    private Dictionary<string, Command> _commands;

    private bool _running;
    public bool Running
    {
        get
        {
            return _running;
        }
        set
        {
            _running = value;
        }
    }

    public BioSimulator()
    {
        _running = true;
        Simulations = new Dictionary<string, Simulation>();
        _runningSimulations = new Dictionary<string, Simulation>();
        IOFactory = new FunctionFactory();
        IOFactory.RegisterIOFunction("near to east", InputFunctions.NearToEast);
        IOFactory.RegisterIOFunction("near to south", InputFunctions.NearToSouth);
        IOFactory.RegisterIOFunction("move east", OutputFunctions.MoveEast);
        IOFactory.RegisterIOFunction("move west", OutputFunctions.MoveWest);
        IOFactory.RegisterIOFunction("move north", OutputFunctions.MoveNorth);
        IOFactory.RegisterIOFunction("move south", OutputFunctions.MoveSouth);

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

        // load sims
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.IncludeFields = true;

        /*
        var simsPaths = Directory.EnumerateFiles("../../saves/");
        foreach (var item in simsPaths)
        {
            Simulation sim = JsonSerializer.Deserialize<Simulation>(item, options);
            Simulations.Add(new FileInfo(item).Name, sim);
        }
        */


    }

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

            inputThread = StartInputThread(strHolder);
        }
    }

    private void GetInput(object obj)
    {
        var input = (StringHolder)obj;
        input.str = Console.ReadLine();
    }

    private Thread StartInputThread(StringHolder strHolder)
    {
        Thread thread = new Thread(GetInput);
        thread.Start(strHolder);

        return thread;
    }

    private void UpdateSimulations()
    {
        var sims = RunningSimulations;
        foreach (var item in sims)
        {
            bool running = item.Value.Update();
            if (!running)
            {
                sims.Remove(item.Key);
            }
        }
    }

    private class StringHolder
    {
        public string str;
    }
}
