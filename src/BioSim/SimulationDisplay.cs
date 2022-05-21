using Gtk;

namespace BioSim;

class SimulationDisplay : VBox
{
    private VBox _simulationItems;

    public SimulationDisplay(int taskBarHeight)
    {
        NewSimButton button = new NewSimButton(this);

        HBox taskBar = new HBox();
        taskBar.HeightRequest = taskBarHeight;
        taskBar.Add(button);

        PackStart(taskBar, false, false, 0);

        _simulationItems = new VBox();
        PackStart(_simulationItems, false, false, 0);
    }

    public void AddSimulationItem(SimulationItem item)
    {
        _simulationItems.PackStart(item, false, false, 0);
    }
}
