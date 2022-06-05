namespace BioSim;

// The Living thing that involves
public class Dit
{
    public (int x, int y) Position { get; set; }
    public Model Model { get; set; }

    // constructor
    public Dit((int, int) position, Model model)
    {
        this.Position = position;
        this.Model = model;
    }
}
