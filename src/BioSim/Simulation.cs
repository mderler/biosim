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
            _simEnv.SimMap = value;
        }
    }
    public int Generations { get; set; }
    public int Steps { get; set; }
    public int InitialPopulation { get; set; }
    public InputFunction[] InputFunctions { get; set; }
    public OutputFunction[] OutputFunctions { get; set; }
    public int MinBirthAmount { get; set; }
    public int MaxBirthAmount { get; set; }
    public Random RandomNumberGenerator { get; set; }
    public Model ModelTemplate { get; set; }
    private List<Dit> _dits = new List<Dit>();
    private SimulationEnviroment _simEnv;

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    public Simulation(Model modelTemplate,
                      InputFunction[] inputFunctions,
                      OutputFunction[] outputFunctions,
                      Map simMap)
    {
        RandomNumberGenerator = new Random();
        MinBirthAmount = 1;
        MaxBirthAmount = 2;
        ModelTemplate = modelTemplate;
        InputFunctions = inputFunctions;
        OutputFunctions = outputFunctions;
        _simMap = simMap;
        _simEnv = new SimulationEnviroment(_simMap);
    }

    public void Setup()
    {
        int actualDitCount = Math.Min(InitialPopulation, SimMap.FreeSpaceCount);
        for (int i = 0; i < actualDitCount; i++)
        {
            (int, int) pos = (
                RandomNumberGenerator.Next(SimMap.Width),
                RandomNumberGenerator.Next(SimMap.Width)
            );
            Model model = ModelTemplate.Copy();
            model.Randomize();

            Dit dit = new Dit(pos, model);
            _dits.Add(dit);
        }
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
            _currentStep = 0;
            _currentGeneration++;
        }

        return true;
    }

    private void DoStep()
    {
        float[] inputs = new float[InputFunctions.Length];
        foreach (var dit in _dits)
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
        _simEnv.KillAndCreateDits(MinBirthAmount, MaxBirthAmount);
    }

    public void SaveState()
    {
        
    }

    public void LoadState()
    {

    }
}
