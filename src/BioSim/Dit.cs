namespace BioSim;

// The Living thing that involves
public class Dit
{
    // get set position of dit
    public (int x, int y) Position { get; set; }
    // the model the dit uses
    public SLLModel Model { get; set; }

    // constructor
    public Dit((int, int) position, SLLModel model)
    {
        this.Position = position;
        this.Model = model;
    }
}
