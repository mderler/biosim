// A Dit is a little creature evolving over time

public class Dit
{
    private List<(int, int, float)> _connections;
    public (int x, int y) Position { get; set; }
    public Dit(List<(int, int, float)> connections, (int x, int y) position)
    {
        _connections = connections;
        Position = position;
    }
}