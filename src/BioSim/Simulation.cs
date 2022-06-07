using System.Text.Json.Serialization;

namespace BioSim;

// where it all comes together
// simulate class
[JsonConverter(typeof(SimulationConverter))]
public class Simulation
{
    // current state of the simulation
    public SimulationState CurrentState { get; set; }
    // the map to be used in the simulation
    private Map _simMap;
    // get set the map
    public Map SimMap
    {
        get => _simMap;
        set
        {
            _simMap = value;
            if (SimEnv == null)
            {
                SimEnv = new SimulationEnviroment(_simMap);
                return;
            }
            SimEnv.SimMap = value;
        }
    }

    // random number generator
    private Random _rnd;
    // get set the RNG
    public Random RNG
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

    // template of the Model to be used for every dit
    private SLLModel _modelTemplate;
    // get set model template
    public SLLModel ModelTemplate
    {
        get => _modelTemplate;
        set
        {
            _modelTemplate = value;
            foreach (var item in SimEnv.Dits)
            {
                var conns = item.Model.Connections;
                item.Model = value.Copy();
                item.Model.Connections = conns;
            }
        }
    }
    // the simulation
    public SimulationEnviroment SimEnv { get; private set; }

    // get private set current step
    public int CurrentStep
    {
        get;
        private set;
    }

    // get private set current generation
    public int CurrentGeneration
    {
        get;
        private set;
    }

    // simulation settings
    private SimulationSettings _settings;
    // get set settings
    public SimulationSettings Settings
    {
        get => _settings;
        set
        {
            _settings = value;
            ModelTemplate = new SLLModel(
            _settings.mutateChance, _settings.mutateStrength, _settings.inputFunctions.Length,
            _settings.innerCount, _settings.outputFunctions.Length, _settings.connectionCount, new Random(_settings.seed))
            { InnerCount = _settings.innerCount, ConnectionCount = _settings.connectionCount };

            SimMap = new Map(_settings.mapPath);
            RNG = new Random(_settings.seed);
        }
    }

    // constructor
    public Simulation(SimulationSettings settings)
    {
        CurrentGeneration = 0;
        CurrentStep = 0;
        _settings = settings;
        _modelTemplate = new SLLModel(
            settings.mutateChance, settings.mutateStrength, settings.inputFunctions.Length,
            settings.innerCount, settings.outputFunctions.Length, settings.connectionCount, new Random(settings.seed))
        { InnerCount = settings.innerCount, ConnectionCount = settings.connectionCount };

        SimMap = new Map(settings.mapPath);
        RNG = new Random(settings.seed);

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
