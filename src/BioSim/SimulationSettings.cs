struct SimulationSettings
{
    public Tuple<uint, uint> InitialWorldSize { get; set; }
    public uint Population { get; set; }
    public uint Generations { get; set; }
    public uint Steps { get; set; }

    public SimulationSettings(Tuple<uint, uint> size, uint population, uint generations, uint steps)
    {
        InitialWorldSize = size;
        Population = population;
        Generations = generations;
        Steps = steps;
    }
}