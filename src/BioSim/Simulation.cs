namespace BioSim;

public class Simulation
{
    public Map? SimMap { get; set; }
    public int Generations { get; set; }
    public int Steps { get; set; }
    public InputFunction[]? InputFunctions { get; set; }
    public OutputFunction[]? OutputFunctions { get; set; }
    private List<Dit> _dits = new List<Dit>();

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    public void Setup()
    {
        
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

        List<Dit> ditsToKill = new List<Dit>();
        foreach (var item in _dits)
        {
            if (SimMap.GetSpot(item.position) != Map.CellType.survive)
            {
                ditsToKill.Add(item);
            }
        }

        foreach (var item in ditsToKill)
        {
            _dits.Remove(item);
        }
    }

    public void SaveState()
    {
        
    }

    public void LoadState()
    {

    }
}
