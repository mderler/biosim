namespace BioSim;

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
    private Random? _rnd;
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
            ModelTemplate.RandomNumberGenerator = value;
        }
    }
    public Model ModelTemplate { get; set; }
    public SimulationEnviroment SimEnv { get; private set; }

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    public Simulation(Model modelTemplate,
                      InputFunction[] inputFunctions,
                      OutputFunction[] outputFunctions,
                      Map simMap)
    {
        MinBirthAmount = 1;
        MaxBirthAmount = 2;
        ModelTemplate = modelTemplate;
        InputFunctions = inputFunctions;
        OutputFunctions = outputFunctions;
        _simMap = simMap;
        SimEnv = new SimulationEnviroment(_simMap);
    }

    public void Setup()
    {
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
