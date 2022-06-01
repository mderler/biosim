using Gtk;

namespace BioSim;

// main Class for GUI
class BioSimulator
{
    private const int DEFAULT_WIDTH = 1260;
    private const int DEFAULT_HEIGHT = 720;

    private Window _window;
    private readonly FunctionFactory _simulationFactory;

    // constructor
    public BioSimulator()
    {
        _simulationFactory = new FunctionFactory();
        #region RegisterIOFuncions
        _simulationFactory.RegisterIOFunction("Sense left & right", InputFunctions.NearToEast);
        _simulationFactory.RegisterIOFunction("Sense up & down", InputFunctions.NearToSouth);
        _simulationFactory.RegisterIOFunction("Move left", OutputFunctions.MoveWest);
        _simulationFactory.RegisterIOFunction("Move right", OutputFunctions.MoveEast);
        _simulationFactory.RegisterIOFunction("Move up", OutputFunctions.MoveNorth);
        _simulationFactory.RegisterIOFunction("Move down", OutputFunctions.MoveSouth);
        #endregion

        _window = new Window("Bio Simulator");
        _window.DefaultWidth = DEFAULT_WIDTH;
        _window.DefaultHeight = DEFAULT_HEIGHT;

        _window.Destroyed += Terminate;

        SimulationDisplay simulationDisplay = new SimulationDisplay(DEFAULT_HEIGHT / 11);

        simulationDisplay.WidthRequest = DEFAULT_WIDTH / 3;

        HBox mainBox = new HBox();

        mainBox.PackStart(simulationDisplay, false, false, 0);

        _window.Add(mainBox);
        _window.ShowAll();
    }

    // Quit Application
    private void Terminate(object o, EventArgs e)
    {
        Application.Quit();
    }
}