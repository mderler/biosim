using System.Text.Json.Serialization;

namespace BioSim;

// all the settings rquired to run a simulation
[JsonConverter(typeof(SimulationSettingsConverter))]
public class SimulationSettings
{
    public string version = ProgramVersion.version;
    public string error = "";
    public int generations = 10;
    public int steps = 10;
    public int initialPopulation = 1000;
    public InputFunction[] inputFunctions = { InputFunctions.NearToEast, InputFunctions.NearToSouth };
    public OutputFunction[] outputFunctions = { OutputFunctions.MoveEast, OutputFunctions.MoveNorth, OutputFunctions.MoveSouth, OutputFunctions.MoveWest };
    public int minBirthAmount = 1;
    public int maxBirthAmount = 2;
    public int seed = 0;
    public string mapPath = "../../res/testres/firstsim.png";
    public float mutateChance = 0.001f;
    public float mutateStrength = 0.001f;
    public int innerCount = 2;
    public int connectionCount = 10;
}
