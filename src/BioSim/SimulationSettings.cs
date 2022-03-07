namespace BioSim;

public struct SimulationSettings
{
    public Map map;
    public uint steps;
    public uint generations;
    public uint initialPopulation;
    public InputFunction[] inputFunctions;
    public OutputFunction[] outputFunctions;
    public int innerNeurons;
    public int connections;
    public int mutateChance;
}
