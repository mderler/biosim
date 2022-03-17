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
        #region Check fields
            if (SimMap == null || InputFunctions == null || OutputFunctions == null)
            {
                throw new Exception("Fields of the simulation must not equal null.");
            }
        #endregion

        #region Check if finished
            if (_currentGeneration >= Generations)
            {
                return false;
            }
            if (_currentStep >= Steps-1)
            {
                _currentStep = 0;
                _currentGeneration++;
            } else
            {
                _currentStep++;
            }
        #endregion

        #region Update dits
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
        #endregion

        return true;
    }

    public void SaveState()
    {
        
    }

    public void LoadState()
    {

    }
}
