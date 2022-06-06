using System.Text.Json;

namespace BioSim;

public static class HelperFunctions
{
    public static void SaveSimulation(string path, Simulation simulation)
    {
        string jsonString = JsonSerializer.Serialize<Simulation>(simulation);
        File.WriteAllText(path, jsonString);
    }
}
