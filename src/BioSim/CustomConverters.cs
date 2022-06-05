using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioSim;

public class SimulationSettingsConverter : JsonConverter<SimulationSettings>
{
    public override SimulationSettings Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var fields = typeof(SimulationSettings).GetFields();
        FieldInfo currentField = null;

        SimulationSettings settings = new SimulationSettings();
        while (reader.Read())
        {
            var token = reader.TokenType;
            if (token == JsonTokenType.PropertyName)
            {
                string name = reader.GetString();

                if (name == nameof(settings.error))
                {
                    continue;
                }

                currentField = fields.FirstOrDefault((x) => x.Name == name, null);

                if (currentField == null)
                {
                    throw new Exception("Property name not found");
                }
                continue;
            }

            switch (token)
            {
                case JsonTokenType.String: currentField.SetValue(settings, reader.GetString()); break;
                case JsonTokenType.Number:
                    if (currentField.FieldType == typeof(float))
                    {
                        currentField.SetValue(settings, reader.GetSingle());
                        break;
                    }
                    currentField.SetValue(settings, reader.GetInt32());
                    break;
                case JsonTokenType.StartArray:
                    bool inputFunction = currentField.FieldType == typeof(InputFunction[]);
                    List<string> tmp = new List<string>();

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }
                        tmp.Add(reader.GetString());
                    }

                    if (inputFunction)
                    {
                        InputFunction[] inputFunctions = new InputFunction[tmp.Count];
                        for (int i = 0; i < tmp.Count; i++)
                        {
                            if (FunctionFactory.RegisterdInputFunctions.ContainsKey(tmp[i]))
                            {
                                inputFunctions[i] = FunctionFactory.RegisterdInputFunctions[tmp[i]];
                                continue;
                            }

                            settings.error += $"input function {tmp[i]} not found;";
                        }
                        break;
                    }
                    OutputFunction[] outputFunctions = new OutputFunction[tmp.Count];
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        if (FunctionFactory.RegisterdOutputFunctions.ContainsKey(tmp[i]))
                        {
                            outputFunctions[i] = FunctionFactory.RegisterdOutputFunctions[tmp[i]];
                            continue;
                        }

                        settings.error += $"output function {tmp[i]} not found;";
                    }

                    break;
            }
        }
        return settings;
    }

    public override void Write(Utf8JsonWriter writer, SimulationSettings value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var fields = typeof(SimulationSettings).GetFields();
        foreach (var field in fields)
        {
            if (field.Name == nameof(value.error))
            {
                continue;
            }

            writer.WritePropertyName(field.Name);
            if (field.FieldType == typeof(InputFunction[]))
            {
                writer.WriteStartArray();
                var funcs = (InputFunction[])field.GetValue(value);
                foreach (var func in funcs)
                {
                    string name = FunctionFactory.RegisterdInputFunctions.FirstOrDefault((x) => x.Value == func).Key;
                    writer.WriteStringValue(name);
                }
                writer.WriteEndArray();
                continue;
            }
            if (field.FieldType == typeof(OutputFunction[]))
            {
                writer.WriteStartArray();
                var funcs = (OutputFunction[])field.GetValue(value);
                foreach (var func in funcs)
                {
                    string name = FunctionFactory.RegisterdOutputFunctions.FirstOrDefault((x) => x.Value == func).Key;
                    writer.WriteStringValue(name);
                }
                writer.WriteEndArray();
                continue;
            }

            if (field.FieldType == typeof(float))
            {
                writer.WriteNumberValue((float)field.GetValue(value));
                continue;
            }
            if (field.FieldType == typeof(int))
            {
                writer.WriteNumberValue((int)field.GetValue(value));
                continue;
            }
            if (field.FieldType == typeof(string))
            {
                writer.WriteStringValue((string)field.GetValue(value));
                continue;
            }
            throw new Exception($"type not clear: {field.FieldType}");
        }

        writer.WriteEndObject();
    }
}

public class SimulationConverter : JsonConverter<Simulation>
{
    public override Simulation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Simulation simulation;
        SimulationSettings settings = new SimulationSettings();

        var props = typeof(Simulation).GetProperties();
        List<Dit> dits = new List<Dit>();

        int currentStep = 0;
        int CurrentGeneration = 0;

        while (reader.Read())
        {
            var token = reader.TokenType;
            if (token == JsonTokenType.PropertyName)
            {
                string name = reader.GetString();

                reader.Read();
                var tokenNow = reader.TokenType;

                if (name == nameof(simulation.Settings))
                {
                    settings = JsonSerializer.Deserialize<SimulationSettings>(ref reader);
                    reader.Skip();
                    continue;
                }
                if (name == "dits")
                {
                    dits = JsonSerializer.Deserialize<List<Dit>>(ref reader);
                    reader.Skip();
                    continue;
                }
                if (name == nameof(simulation.CurrentStep))
                {
                    currentStep = reader.GetInt32();
                    continue;
                }
                if (name == nameof(simulation.CurrentGeneration))
                {
                    CurrentGeneration = reader.GetInt32();
                    continue;
                }
            }

        }
        simulation = new Simulation(settings);

        return simulation;
    }

    public override void Write(Utf8JsonWriter writer, Simulation value, JsonSerializerOptions options)
    {
        string[] excluded = {
            nameof(value.ModelTemplate),
            nameof(value.RandomNumberGenerator),
            nameof(value.SimMap),
            nameof(value.SimEnv)
        };

        var properties = typeof(Simulation).GetProperties();

        writer.WriteStartObject();

        foreach (var prop in properties)
        {
            if (excluded.Contains(prop.Name))
            {
                continue;
            }

            writer.WritePropertyName(prop.Name);

            if (prop.PropertyType == typeof(SimulationSettings))
            {
                string jsonStringSettings = JsonSerializer.Serialize<SimulationSettings>((SimulationSettings)prop.GetValue(value));
                writer.WriteRawValue(jsonStringSettings);
                continue;
            }

            writer.WriteNumberValue((int)prop.GetValue(value));
        }

        writer.WritePropertyName("dits");

        string jsonStringDits = JsonSerializer.Serialize<List<Dit>>(value.SimEnv.Dits);
        writer.WriteRawValue(jsonStringDits);

        writer.WriteEndObject();
    }
}
