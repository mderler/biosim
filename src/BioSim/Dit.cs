namespace BioSim;

// The Living thing that involves
public class Dit
{
    public (int x, int y) Position { get; set; }
    public SLLModel Model { get; set; }

    // constructor
    public Dit((int, int) position, SLLModel model)
    {
        this.Position = position;
        this.Model = model;
    }
}
