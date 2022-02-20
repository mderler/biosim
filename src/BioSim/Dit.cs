// A Dit is a little creature evolving over time

struct Dit
{
    public Tuple<uint, uint> Position { get; set; }
    public char[] Gene { get; }

    public Dit(char[] gene, uint x, uint y)
    {
        Position = new Tuple<uint, uint>(x, y);
        Gene = gene;
    }
}