namespace BioSim;

public class SimulationSettings
{
    public string version = ProgramVersion.version;
    public int generations = 10;
    public int steps = 10;
    public int initialPopulation = 1000;
    public string[] inputFunctions = new string[0];
    public string[] outputFunctions = new string[0];
    public int minBirthAmount = 1;
    public int maxBirthAmount = 2;
    public int seed = 0;
    public string mapPath = "../../res/testres/firstsim.png";
    public float mutateChance = 0.001f;
    public float mutateStrength = 0.001f;
    public int innerCount = 2;
}
