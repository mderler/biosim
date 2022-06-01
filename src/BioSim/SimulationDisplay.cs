using Gtk;

namespace BioSim;

// displays all simulations
class SimulationDisplay : VBox
{
    private VBox _simulationItems;

    // constructor
    public SimulationDisplay(int taskBarHeight)
    {
        NewSimButton button = new NewSimButton(this);

        HBox taskBar = new HBox();
        taskBar.HeightRequest = taskBarHeight;
        taskBar.Add(button);

        PackStart(taskBar, false, false, 0);

        _simulationItems = new VBox();
        PackStart(_simulationItems, true, true, 0);
    }

    // add a Simulation to the list
    public void AddSimulationItem(SimulationItem item)
    {
        this.Remove(_simulationItems);
        _simulationItems.PackStart(item, false, false, 0);
        _simulationItems.PackStart(new Label("asd"), false, false, 0);
        PackStart(_simulationItems, true, true, 0);
    }
}
