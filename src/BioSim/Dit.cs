// A Dit is a little creature evolving over time

struct Dit
{
    private List<(int, float)> _innerConnections;
    private List<(int, float)> _outerConnections;
    public Tuple<uint, uint> Position { get; set; }
    public Dit(List<(int, float)> innerConnections, uint x, uint y)
    {
        _innerConnections = new List<(int, float)>();
        _outerConnections = new List<(int, float)>();
        Position = new Tuple<uint, uint>(x, y);
    }
}