namespace BioSim;

public class BioSimulator
{
    public Dictionary<string, Simulation> Simulations
    {
        get;
        private set;
    }
    public Dictionary<string, Simulation> RunningSimulations
    {
        get;
        private set;
    }
    private FunctionFactory _factory;
    private Dictionary<string, Command> _commands;

    public bool Running
    {
        get;
        set;
    }
    public BioSimulator()
    {
        Running = true;
        Simulations = new Dictionary<string, Simulation>();
        RunningSimulations = new Dictionary<string, Simulation>();
        _factory = new FunctionFactory();
        _factory.RegisterIOFunction("near to east", InputFunctions.NearToEast);
        _factory.RegisterIOFunction("near to south", InputFunctions.NearToSouth);
        _factory.RegisterIOFunction("move east", OutputFunctions.MoveEast);
        _factory.RegisterIOFunction("move west", OutputFunctions.MoveWest);
        _factory.RegisterIOFunction("move north", OutputFunctions.MoveNorth);
        _factory.RegisterIOFunction("move south", OutputFunctions.MoveSouth);

        _commands = new Dictionary<string, Command>();
        _commands.Add("create", new CreateCommand(this));
        _commands.Add("quit", new QuitCommand(this));
        _commands.Add("run", new RunCommand(this));
        _commands.Add("halt", new HaltCommand(this));
        _commands.Add("delete", new DeleteCommand(this));
        _commands.Add("show", new ShowCommand(this));
        _commands.Add("template", new CreateTemplateCommand(this));
    }

    public void Run()
    {
        while (Running)
        {
            Console.Write("-> ");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            string[] tmp = input.Split(' ');
            string command = tmp[0];
            string[] args = new string[tmp.Length - 1];
            for (int i = 1; i < tmp.Length; i++)
            {
                args[i - 1] = tmp[i];
            }
            if (!_commands.ContainsKey(command))
            {
                Console.Write($"the command does not exist {command}");
            }
            string output = _commands[command].RunCommand(args);

            Console.WriteLine(output);
        }
    }
}
