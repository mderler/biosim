using System.Text.Json.Serialization;

namespace BioSim;

[JsonConverter(typeof(SimulationSettingsConverter))]
public class SimulationSettings
{
    public string version = ProgramVersion.version;
    public string error = "";
    public int generations = 10;
    public int steps = 10;
    public int initialPopulation = 1000;
    [JsonConverter(typeof(InputFunctionsConverter))]
    public InputFunction[] inputFunctions = { InputFunctions.NearToEast, InputFunctions.NearToSouth };
    [JsonConverter(typeof(OutputFunctionsConverter))]
    public OutputFunction[] outputFunctions = { OutputFunctions.MoveEast, OutputFunctions.MoveNorth, OutputFunctions.MoveSouth, OutputFunctions.MoveWest };
    public int minBirthAmount = 1;
    public int maxBirthAmount = 2;
    public int seed = 0;
    public string mapPath = "../../res/testres/firstsim.png";
    public float mutateChance = 0.001f;
    public float mutateStrength = 0.001f;
    public int innerCount = 2;
}
