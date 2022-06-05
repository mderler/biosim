using System.Text.Json;

namespace BioSim;

public static class HelperFunctions
{
    public static void SaveSimulation(string path, Simulation simulation)
    {
        JsonSerializerOptions options = new JsonSerializerOptions();
        options.IncludeFields = true;

        string jsonString = JsonSerializer.Serialize<Simulation>(simulation, options);
        File.WriteAllText(path, jsonString);
    }
}
