using Gtk;
using ImageMagick;

namespace BioSim;

class BioSimulatorGUI
{
    private BioSimWindow _window;
    private readonly FunctionFactory _simulationFactory;

    public BioSimulatorGUI()
    {
        _simulationFactory = new FunctionFactory();
        _window = new BioSimWindow();
        
        NewSimButton newSimButton = new NewSimButton();
        _window.Add(newSimButton);
        
        _window.ShowAll();
    }
}