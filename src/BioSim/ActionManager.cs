namespace BioSim;

public class ActionManager
{
    private Dictionary<string, SimulationAction> _actions = new Dictionary<string, SimulationAction>();
    private Dictionary<Simulation, List<SimulationAction>> _bindings = new Dictionary<Simulation, List<SimulationAction>>();
    private Dictionary<string, Simulation> _actionTypes = new Dictionary<string, Simulation>();

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

    public void CheckAndExecute(Simulation simulation)
    {
        if (!_bindings.ContainsKey(simulation))
        {
            return;
        }

        foreach (var item in _bindings[simulation])
        {
            item.ExecuteAction();
        }
    }
}
