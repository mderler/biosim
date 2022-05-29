using System.Reflection;

namespace BioSim;

public class ParameterHolder
{
    public Dictionary<string, (List<(FieldInfo field, object obj)> fieldObjs, List<(PropertyInfo prop, object obj)> propObjs, Type type)> Parameters
    {
        get;
        private set;
    }

    public ParameterHolder()
    {
        Parameters = new Dictionary<string, (List<(FieldInfo, object)>, List<(PropertyInfo, object)>, Type)>();
    }

    public void AddParameter(string key, FieldInfo field, object obj)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new Exception("Key must not be null or empty");
        }

        if (!Parameters.ContainsKey(key))
        {
            Parameters.Add(key, (new List<(FieldInfo, object)>(), new List<(PropertyInfo, object)>(), field.GetType()));
        }

        if (field.GetType() != Parameters[key].type)
        {
            throw new Exception("The fields or properties must have the same types");
        }

        Parameters[key].fieldObjs.Add((field, obj));
    }

    public void AddParameter(string key, PropertyInfo property, object obj)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new Exception("Key must not be null or empty");
        }

        if (!Parameters.ContainsKey(key))
        {
            Parameters.Add(key, (new List<(FieldInfo, object)>(), new List<(PropertyInfo, object)>(), property.GetType()));
        }

        if (property.GetType() != Parameters[key].type)
        {
            throw new Exception("The fields or properties must have the same types");
        }

        Parameters[key].propObjs.Add((property, obj));
    }
}
