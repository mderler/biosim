using Gtk;

namespace BioSim;

class BioSimWindow : Window
{
    public BioSimWindow() : base("Bio Simulator")
    {
        SetDefaultSize(1270, 720);
        DeleteEvent += delegate { Application.Quit(); };
    }
}