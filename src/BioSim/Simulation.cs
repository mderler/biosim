class Simulation
{
    private List<Dit> _dits = new List<Dit>();
    private SimulationData _data;
    public Simulation(SimulationData data)
    {
        _data = data;
        for (int i = 0; i < _data.population; i++)
        {
            //_dits.Add(new Dit());
        }
    }

    private void MakeStep()
    {
        NeuralNetworkModel model = new NeuralNetworkModel(1, 2, 1);
        foreach (var dit in _dits)
        {
            
        }
    }

    private void MakeGeneration()
    {
        Random rnd = new Random();
        for (int i = 0; i < _dits.Count; i++)
        {
            
        }
    }
}

struct SimulationData
{
    public int population;
    public (int x, int y) worldSize;
    public int steps;
    public string genomeReadSetting;

    public SimulationData(int population, (int x, int y) worldSize, int steps, string genomeReadSetting)
    {
        this.population = population;
        this.worldSize = worldSize;
        this.steps = steps;
        this.genomeReadSetting = genomeReadSetting;
    }
}
