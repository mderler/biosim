namespace BioSim;

public class Dit
{
    public (int x, int y) position;
    public SLLModel model;

    public Dit((int, int) position, SLLModel model)
    {
        this.position = position;
        this.model = model;
    }
}
