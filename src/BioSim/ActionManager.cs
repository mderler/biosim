namespace BioSim;

public class ActionManager
{
    private Dictionary<string, SimulationAction> _actions = new Dictionary<string, SimulationAction>();
    private Dictionary<Simulation, SimulationAction> _bindings = new Dictionary<Simulation, SimulationAction>();

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

    }
}
