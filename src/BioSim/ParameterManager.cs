using Gtk;

namespace BioSim;

public class ParameterManager
{
    private ParameterHolder _parameterHolder;
    private Dictionary<string, Entry> _entries;

    public ParameterManager()
    {
        _parameterHolder = new ParameterHolder();
        _entries = new Dictionary<string, Entry>();
        ReflectParameters();
    }

    public VBox GetGUIObjects()
    {
        var dict = _parameterHolder.Parameters;
        VBox main = new VBox();

        foreach (var key in dict.Keys)
        {
            HBox tmpHBox = new HBox();

            Label tmpLabel = new Label(key + ":");
            Entry tmpEntry = new Entry();

            _entries.Add(key, tmpEntry);

            tmpHBox.PackStart(tmpLabel, false, false, 4);
            tmpHBox.PackEnd(tmpEntry, false, false, 4);

            main.PackStart(tmpHBox, false, false, 2);
        }

        return main;
    }

    public void ParseValues()
    {
        var parameters = _parameterHolder.Parameters;
        foreach (var item in _entries)
        {
            string txt = item.Value.Text;
            var val = item.Value.Text;
            parameters[item.Key].fieldObjs.ForEach((fieldObj) => fieldObj.field.SetValue(val, fieldObj.obj));
            parameters[item.Key].propObjs.ForEach((propObj) => propObj.prop.SetValue(val, propObj.obj));
        }
    }


    private void ReflectParameters()
    {
        var paraFields = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         from field in type.GetFields()
                         where field.IsDefined(typeof(SimulationParameterAttribute), false) |
                             (type.IsDefined(typeof(IncludeAllAsParametersAttribute), false) &
                             !field.IsDefined(typeof(ExcludeParameterAttribute), false))
                         select field;

        var paraProps = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.GetTypes()
                        from prop in type.GetProperties()
                        where prop.IsDefined(typeof(SimulationParameterAttribute), false) |
                            (type.IsDefined(typeof(IncludeAllAsParametersAttribute), false) &
                            !prop.IsDefined(typeof(ExcludeParameterAttribute), false))
                        select prop;

        foreach (var item in paraFields)
        {
            System.Console.WriteLine(item);
            var attr = item.GetCustomAttributes(typeof(SimulationParameterAttribute), false);
            if (attr.Length == 0)
            {
                _parameterHolder.AddParameter(GetName(item.Name), item, null);
                continue;
            }

            var att = attr[0] as SimulationParameterAttribute;
            _parameterHolder.AddParameter(att.Name, item, null);
        }

        foreach (var item in paraProps)
        {
            System.Console.WriteLine(item);
            var attr = item.GetCustomAttributes(typeof(SimulationParameterAttribute), false);
            if (attr.Length == 0)
            {
                _parameterHolder.AddParameter(GetName(item.Name), item, null);
                continue;
            }

            var att = attr[0] as SimulationParameterAttribute;
            _parameterHolder.AddParameter(att.Name, item, null);
        }
    }

    private string GetName(string name)
    {
        string n = "";
        n += char.ToUpper(name[0]);

        for (int i = 1; i < name.Length; i++)
        {
            if (name[i] == char.ToUpper(name[i]))
            {
                n += " ";
            }

            n += name[i];
        }

        return n;
    }
}
