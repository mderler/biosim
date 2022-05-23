namespace BioSim;

public class Dit
{
    public (int x, int y) position;
    public Model model;

    public Dit((int, int) position, Model model)
    {
        this.position = position;
        this.model = model;
    }
}
