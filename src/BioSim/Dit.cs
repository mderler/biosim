// A Dit is a little creature evolving over time

public struct Dit
{
    private List<(int, float)> _connections;
    public Tuple<uint, uint> Position { get; set; }
    public Dit(List<(int, float)> connections, uint x, uint y)
    {
        _connections = connections;
        Position = new Tuple<uint, uint>(x, y);
    }
}