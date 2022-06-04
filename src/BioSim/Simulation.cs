using System.Text.Json.Serialization;

namespace BioSim;

// where it all comes together
// simulate class
public class Simulation
{
    private Map _simMap;
    public Map SimMap
    {
        get => _simMap;
        set
        {
            _simMap = value;
            SimEnv.SimMap = value;
        }
    }
    public int Generations { get; set; }
    public int Steps { get; set; }
    public int InitialPopulation { get; set; }
    public InputFunction[] InputFunctions { get; set; }
    public OutputFunction[] OutputFunctions { get; set; }
    public int MinBirthAmount { get; set; }
    public int MaxBirthAmount { get; set; }
    private Random _rnd;
    public Random RandomNumberGenerator
    {
        get
        {
            if (_rnd == null)
            {
                _rnd = new Random();
            }

            return _rnd;
        }
        set
        {
            _rnd = value;
            SimEnv.RandomNumberGenerator = value;
            ModelTemplate.RNG = value;
        }
    }
    public Model ModelTemplate { get; set; }
    public SimulationEnviroment SimEnv { get; private set; }

    public int CurrentStep
    {
        get;
        private set;
    }
    public int CurrentGeneration
    {
        get;
        private set;
    }

    private SimulationSettings _settings;
    public SimulationSettings Settings
    {
        get => _settings;
        set
        {
            _settings = value;
        }
    }

    // constructor
    public Simulation()
    {
        SimEnv = new SimulationEnviroment();
        CurrentStep = 0;
        CurrentGeneration = 0;
    }

    // constructor
    public Simulation(Model modelTemplate,
                      InputFunction[] inputFunctions,
                      OutputFunction[] outputFunctions,
                      Map simMap,
                      int minBirthAmount = 1,
                      int maxBrithAmount = 2,
                      int initialPopulation = 0,
                      int steps = 0,
                      int generations = 0)
    {
        MinBirthAmount = minBirthAmount;
        MaxBirthAmount = maxBrithAmount;
        ModelTemplate = modelTemplate;
        InputFunctions = inputFunctions;
        OutputFunctions = outputFunctions;
        Generations = generations;
        Steps = steps;
        _simMap = simMap;
        CurrentStep = 0;
        CurrentGeneration = 0;
    }

    // setup the simulation
    public void Setup()
    {
        SimEnv = new SimulationEnviroment(_simMap);
        SimEnv.TryAddRandomDits(InitialPopulation, ModelTemplate);
    }

    // update one iterarion
    public bool Update()
    {
        if (CurrentGeneration >= Generations)
        {
            return false;
        }
        if (CurrentStep < Steps)
        {
            DoStep();
            CurrentStep++;
        }
        if (CurrentStep >= Steps)
        {
            DoGeneration();
            if (SimEnv.Dits.Count == 0)
            {
                return false;
            }
            CurrentStep = 0;
            CurrentGeneration++;
        }

        return true;
    }

    // do one step
    private void DoStep()
    {
        float[] inputs = new float[InputFunctions.Length];
        foreach (var dit in SimEnv.Dits)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = InputFunctions[i](dit, this);
            }
            bool[] outputs = dit.model.GetOutput(inputs);
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i])
                {
                    OutputFunctions[i](in dit, this);
                }
            }
        }
    }

    // do a generation
    private void DoGeneration()
    {
        SimEnv.KillAndCreateDits(MinBirthAmount, MaxBirthAmount);
    }
}
