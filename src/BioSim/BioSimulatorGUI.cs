using Gtk;

namespace BioSim;

class BioSimulatorGUI
{
    private const int DEFAULT_WIDTH = 1260;
    private const int DEFAULT_HEIGHT = 720;

    private Window _window;
    private readonly FunctionFactory _simulationFactory;

    public BioSimulatorGUI()
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

        SimulationDisplay simulationDisplay = new SimulationDisplay(DEFAULT_HEIGHT / 11);
        simulationDisplay.WidthRequest = DEFAULT_WIDTH / 3;

        HBox mainBox = new HBox();

        mainBox.PackStart(simulationDisplay, false, false, 0);

        _window.Add(mainBox);
        _window.ShowAll();
    }
}