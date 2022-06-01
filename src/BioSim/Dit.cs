namespace BioSim;

// The Living thing that involves
public class Dit
{
    public (int x, int y) position;
    public Model model;

    // constructor
    public Dit((int, int) position, Model model)
    {
        this.position = position;
        this.model = model;
    }
}
