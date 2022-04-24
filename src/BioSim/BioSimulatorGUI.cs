using Gtk;

namespace BioSim;

class BioSimulatorGUI
{
    private const int DEFAULT_WIDTH = 720;
    private const int DEFAULT_HEIGHT = 1270;

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
        
        NewSimButton newSimButton = new NewSimButton();
        _window.Add(newSimButton);

        HBox mainBox = new HBox();
        
        
        _window.ShowAll();
    }
}