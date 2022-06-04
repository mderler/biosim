using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioSim;

public class InputFunctionsConverter : JsonConverter<InputFunction[]>
{
    public override InputFunction[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();
        return new InputFunction[0];
    }

    public override void Write(Utf8JsonWriter writer, InputFunction[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            string key = FunctionFactory.RegisterdInputFunctions.FirstOrDefault(x => x.Value == item).Key;
            writer.WriteStringValue(key);
        }

        writer.WriteEndArray();
    }
}

public class OutputFunctionsConverter : JsonConverter<OutputFunction[]>
{
    public override OutputFunction[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, OutputFunction[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            string key = FunctionFactory.RegisterdOutputFunctions.FirstOrDefault(x => x.Value == item).Key;
            writer.WriteStringValue(key);
        }

        writer.WriteEndArray();
    }
}

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
                    if (currentField.GetType() == typeof(float))
                    {
                        currentField.SetValue(settings, reader.GetSingle());
                        break;
                    }
                    currentField.SetValue(settings, reader.GetInt32());
                    break;
                case JsonTokenType.StartArray:
                    bool input = currentField.GetType() == typeof(InputFunction[]);
                    List<string> tmp = new List<string>();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }
                    }
                    break;
            }
        }
        return settings;
    }

    public override void Write(Utf8JsonWriter writer, SimulationSettings value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
