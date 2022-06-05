using System.Text.Json.Serialization;

namespace BioSim;

// where it all comes together
// simulate class
[JsonConverter(typeof(SimulationConverter))]
public class Simulation
{
    public SimulationState CurrentState { get; set; }
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
    public Simulation(SimulationSettings settings)
    {
        CurrentGeneration = 0;
        CurrentStep = 0;
        _settings = settings;
        ModelTemplate = new SLLModel(settings.mutateChance, settings.mutateStrength)
        { InnerCount = settings.innerCount, ConnectionCount = settings.connectionCount };

        _simMap = new Map(settings.mapPath);
        SimEnv = new SimulationEnviroment(_simMap);
        RandomNumberGenerator = new Random(settings.seed);
    }

    // setup the simulation
    public void Setup()
    {
        SimEnv.TryAddRandomDits(_settings.initialPopulation, ModelTemplate);
    }

    // update one iterarion
    public bool Update()
    {
        if (CurrentGeneration >= _settings.generations)
        {
            CurrentState = SimulationState.Finished;
            return false;
        }
        if (CurrentStep < _settings.steps)
        {
            DoStep();
            CurrentStep++;
        }
        if (CurrentStep >= _settings.steps)
        {
            DoGeneration();
            if (SimEnv.Dits.Count == 0)
            {
                CurrentState = SimulationState.Extinct;
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
        float[] inputs = new float[_settings.inputFunctions.Length];
        foreach (var dit in SimEnv.Dits)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = _settings.inputFunctions[i](dit, this);
            }
            bool[] outputs = dit.Model.GetOutput(inputs);
            for (int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i])
                {
                    _settings.outputFunctions[i](in dit, this);
                }
            }
        }
    }

    // do a generation
    private void DoGeneration()
    {
        SimEnv.KillAndCreateDits(_settings.minBirthAmount, _settings.maxBirthAmount);
    }
}
