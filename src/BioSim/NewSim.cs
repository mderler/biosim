using Gtk;

namespace BioSim;

class NewSimButton : Button
{
    private SimulationDisplay _display;

    public NewSimButton(SimulationDisplay display) : base("New Simulation")
    {
        Clicked += OnButtenClicked;
        _display = display;
    }

    private void OnButtenClicked(object sender, EventArgs e)
    {
        new NewSimWindow(_display);
    }
}


class NewSimWindow : Window
{
    private SimulationDisplay _display;
    public readonly Button createButton;
    public Simulation Simulation { get; private set; }

    public NewSimWindow(SimulationDisplay display) : base("Create new Simulation")
    {
        _display = display;

        VBox outer = new VBox();
        Add(outer);

        ParameterManager manager = new ParameterManager();
        outer.Add(manager.GetGUIObjects());

        // Simulation sim = new Simulation();

        HBox buttonBox = new HBox();
        outer.PackStart(buttonBox, false, false, 4);

        createButton = new Button("Create");
        createButton.Clicked += CreateButtonClicked;
        buttonBox.PackStart(createButton, true, true, 2);

        Button cancelButten = new Button("Cancel");
        cancelButten.Clicked += (object sender, EventArgs e) => Destroy();
        buttonBox.PackStart(cancelButten, true, true, 2);

        ShowAll();
    }

    private void CreateButtonClicked(object obj, EventArgs e)
    {
        Destroy();
    }
}
