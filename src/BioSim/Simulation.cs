namespace BioSim;

public class Simulation
{
    private List<Dit> _dits = new List<Dit>();

    private int _currentStep = 0;
    private int _currentGeneration = 0;

    private SimulationSettings _settings;

    public Simulation(SimulationSettings settings)
    {
        _settings = settings;

        for (int i = 0; i < settings.initialPopulation; i++)
        {
            Model model = new Model(
                settings.inputFunctions.Length,
                settings.innerNeurons,
                settings.outputFunctions.Length,
                settings.connections,
                settings.mutateChance);
            model.Randomize();
            Random rnd = new Random();
            int x = rnd.Next(settings.map.Width);
            int y = rnd.Next(settings.map.Height);
            _dits.Add(new Dit((x, y), model));
        }
    }

    public bool Update()
    {
        // update dits
        float[] inputs = new float[_settings.inputFunctions.Length];
        foreach (var dit in _dits)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = _settings.inputFunctions[i](dit);
            }
            bool[] outputs = dit.model.GetOutput(inputs);
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i])
                    _settings.outputFunctions[i](in dit);
            }
        }

        // check if simulation is finished
        if (_currentGeneration >= _settings.generations)
            return true;
        if (_currentStep >= _settings.steps)
        {
            _currentStep = 0;
            _currentGeneration++;
        } else
            _currentStep++;
        return false;
    }

    public void SaveState()
    {
        
    }

    public void LoadState()
    {

    }
}