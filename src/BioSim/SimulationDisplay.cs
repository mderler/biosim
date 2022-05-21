using Gtk;

namespace BioSim;

class SimulationDisplay : VBox
{
    public SimulationDisplay(int taskBarHeight)
    {
        Button addButton = new Button("Add");

        HBox taskBar = new HBox();
        taskBar.HeightRequest = taskBarHeight;
        taskBar.Add(addButton);

        PackStart(taskBar, false, false, 0);

        VBox simulationItems = new VBox();
    }

    private void AddButtonClicked(object? obj, EventArgs e)
    {
        NewSimButton newSimButton = new NewSimButton();
    }
}
