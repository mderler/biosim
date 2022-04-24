using Gtk;

namespace BioSim;

class SimulationItem : VBox
{
    public readonly Simulation simulation;
    private HButtonBox _hButtonBox;
    private bool _running = true;

    public SimulationItem(Simulation simulation)
    {
        this.simulation = simulation;

        ToggleButton startStopButton = new ToggleButton();
        startStopButton.Active = true;
        startStopButton.Clicked += StartStopButtonClicked;

        _hButtonBox = new HButtonBox();
        _hButtonBox.Add(startStopButton);
        
    }

    public void UpdateSimulation()
    {
        // only run when the internal thread is not updating
    }

    public void Run()
    {
        // start thread
    }

    private void Update()
    {
        if (_running)
        {
            if (simulation.Update())
            {
                FinishSimulation();
            }
        }
    }

    private void StartStopButtonClicked(object? obj, EventArgs e)
    {
        if (obj is null)
        {
            return;
        }

        ToggleButton button = (ToggleButton)obj;
        button.Active = !button.Active;
        _running = button.Active;
    }

    private void FinishSimulation()
    {
        // disable all buttons and let user know the sim is finished
    }

    private void DeleteItem(object? obj, EventArgs e)
    {

    }
}

class DeleteItemWindow : Window
{
    public DeleteItemWindow() : base("Delete Simulation")
    {
        Label warning = new Label("Are you sure that you want to delete this Simulation?");
        Button cancel = new Button("Cancel");
        Button delete = new Button("Delete");
    }
}
