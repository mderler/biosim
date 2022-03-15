namespace BioSim;

// TODO: Change to reference type, mostly because for the model class
public class SimulationSettings
{
    public Map? map;
    public uint steps;
    public uint generations;
    public uint initialPopulation;
    public InputFunction[] inputFunctions;
    public OutputFunction[] outputFunctions;
    public int innerNeurons;
    public int connections;
    public int mutateChance;
    public int mutateStrength;

    public SimulationSettings()
    {
        inputFunctions = new InputFunction[0];
        outputFunctions = new OutputFunction[0];   
    }
}
