namespace BioSim;

[IncludeAllAsParameters]
public class Simulation
{
    private Map _simMap;
    [ExcludeParameter]
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
    [ExcludeParameter]
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
    [ExcludeParameter]
    public SimulationEnviroment SimEnv { get; private set; }

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    public Simulation() { }

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
        _simMap = simMap;
    }

    public void Setup()
    {
        SimEnv = new SimulationEnviroment(_simMap);
        SimEnv.TryAddRandomDits(InitialPopulation, ModelTemplate);
    }

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

    private void DoGeneration()
    {
        SimEnv.KillAndCreateDits(MinBirthAmount, MaxBirthAmount);
    }

    public void SaveState()
    {

    }

    public void LoadState()
    {

    }
}
