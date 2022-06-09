namespace BioSim;

public class ActionManager
{
    private Dictionary<string, SimulationAction> _actions = new Dictionary<string, SimulationAction>();
    private Dictionary<Simulation, List<SimulationAction>> _bindings = new Dictionary<Simulation, List<SimulationAction>>();

    public void AddAction(string name, SimulationAction action)
    {
        if (_actions.ContainsKey(name))
        {
            _actions[name] = action;
            return;
        }

        _actions.Add(name, action);
    }

    public string RemoveAction(string name)
    {
        if (!_actions.Remove(name))
        {
            return $"action ({name}) not found";
        }

        return "";
    }

    public string CheckAndExecute(Simulation simulation)
    {
        if (!_bindings.ContainsKey(simulation))
        {
            return $"Simulation {simulation} not found";
        }

        foreach (var item in _bindings[simulation])
        {
            item.ExecuteAction();
        }

        return "";
    }

    public string Bind(string action, string simulation)
    {
        return "";
    }
}
