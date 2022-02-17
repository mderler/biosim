public class BioSimulation
{
    public Tuple<uint, uint> WorldSize { get; private set; }

    public BioSimulation(uint worldx, uint worldy)
    {
        WorldSize = new Tuple<uint, uint>(worldx, worldy);
    }

    
}