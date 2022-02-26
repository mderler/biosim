class Simulation
{
    private List<Dit> _population = new List<Dit>();

    public Simulation(int initialPopulation, Tuple<uint, uint> worldSize)
    {
        for (int i = 0; i < initialPopulation; i++)
        {
            _population.Add(new Dit());
        }
    }

    private void UpdateDits()
    {
        NeuralNetworkModel model = new NeuralNetworkModel(1, 2, 1);
        foreach (var item in _population)
        {
            
        }
    }
}