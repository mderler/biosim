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

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    // constructor
    public Simulation() { SimEnv = new SimulationEnviroment(); }

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
        if (_currentGeneration >= Generations)
        {
            return false;
        }
        if (_currentStep < Steps)
        {
            DoStep();
            _currentStep++;
        }
        if (_currentStep >= Steps)
        {
            DoGeneration();
            if (SimEnv.Dits.Count == 0)
            {
                return false;
            }
            _currentStep = 0;
            _currentGeneration++;
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

    // saves the state of the simulation
    public void SaveState()
    {

    }

    // loads the state of the simulation
    public void LoadState()
    {

    }
}
