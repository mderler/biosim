namespace BioSim;

public class Simulation
{
    private Map? _simMap;
    public Map? SimMap 
    {
        get => _simMap;
        set
        {
            _simMap = value;
            _simEnv = new SimulationEnviroment(value);
        }
    }
    public int Generations { get; set; }
    public int Steps { get; set; }
    public int InitialPopulation { get; set; }
    public InputFunction[]? InputFunctions { get; set; }
    public OutputFunction[]? OutputFunctions { get; set; }
    public int MinBirthAmount { get; set; }
    public int MaxBirthAmount { get; set; }
    public Random RandomNumberGenerator { get; set; }
    public Model? ModelTemplate { get; set; }
    private List<Dit> _dits = new List<Dit>();
    private SimulationEnviroment? _simEnv;

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    public Simulation()
    {
        RandomNumberGenerator = new Random();
        MinBirthAmount = 1;
        MaxBirthAmount = 2;
    }

    public void Setup()
    {
        if (SimMap == null || InputFunctions == null || OutputFunctions == null || ModelTemplate == null)
        {
            throw new Exception("Fields of the simulation must not equal null.");
        }

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

        DoStep();
        _currentStep++;

        if (_currentStep >= Steps-1)
        {
            DoGeneration();
            _currentStep = 0;
            _currentGeneration++;
        }

        return true;
    }

    private void DoStep()
    {
        if (SimMap == null || InputFunctions == null || OutputFunctions == null)
        {
            throw new Exception("Fields of the simulation must not equal null.");
        }

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
        if (SimMap == null)
        {
            throw new Exception("SimMap must not be null.");
        }

        List<Dit> bornDits = new List<Dit>();
        foreach (var item in _dits)
        {

        }
    }

    public void SaveState()
    {
        
    }

    public void LoadState()
    {

    }
}
