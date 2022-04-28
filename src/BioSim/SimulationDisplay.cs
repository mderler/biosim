using Gtk;

namespace BioSim;

class SimulationDisplay : VBox
{
    public SimulationDisplay()
    {
        Button addButton = new Button("Add");

        ButtonBox buttonBox = new ButtonBox(Orientation.Horizontal);
        buttonBox.Add(addButton);

        VBox mainBox = new VBox();
        mainBox.Add(buttonBox);

        VBox simulationItems = new VBox();
    }

    private void AddButtonClicked(object? obj, EventArgs e)
    {
        NewSimButton newSimButton = new NewSimButton();
    }
}
