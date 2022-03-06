using Gtk;

namespace BioSim;

class SimulationViewer : Frame
{
    private Image _stepImage;

    public SimulationViewer()
    {
        _stepImage = new Image();

        HBox outerHBox = new HBox();
        Add(outerHBox);

        Frame imageFrame = new Frame();
        imageFrame.Add(_stepImage);

        VBox controlPanel = new VBox();
        outerHBox.Add(controlPanel);
    }
}