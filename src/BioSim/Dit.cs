// A Dit is a little creature evolving over time

public class Dit
{
    public (int x, int y) Position { get; set; }
    public int Age { get; set; }

    public Dit((int x, int y) position)
    {
        Position = position;
        Age = 0;
    }
}