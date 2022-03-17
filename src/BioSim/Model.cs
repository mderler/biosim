namespace BioSim;

public class Model
{
    public int InputCount { get; set; }
    public int InnerCount { get; set; }
    public int OutputCount { get; set; }
    private int _connectionCount;
    public int ConnectionCount
    {
        get => _connectionCount;
        set 
        {
            if (value < _connectionCount)
            {
                int count = _connectionCount - value;
                for (int i = 0; i < count; i++)
                {
                    _connections.RemoveAt(RandomNumberGenerator.Next());
                }
            }

            _connectionCount = value;
        }
    }
    public float MutateChance { get; set; }
    public float MutateStrength { get; set; }
    private List<(int src, int dst, float wht, bool act)> _connections;
    public (int src, int dst, float wht, bool act)[] Connections => _connections.ToArray();
    public Random RandomNumberGenerator { get; set; }

    public Model()
    {
        RandomNumberGenerator = new Random();
        _connectionCount = 0;
        _connections = new List<(int, int, float, bool)>();
    }

    public bool[] GetOutput(float[] input)
    {
        bool[] output = new bool[0];
        float[] tmp = new float[InnerCount+InputCount];

        for (int i = 0; i < _connections.Count; i++)
        {
            if (_connections[i].src > InputCount)
                tmp[_connections[i].dst] += MathF.Tanh(tmp[_connections[i].src-InputCount] * _connections[i].wht);
            else
                tmp[_connections[i].dst] += tmp[_connections[i].src] * _connections[i].wht;
        }

        for (int i = 0; i < OutputCount; i++)
        {
            output[i] = 0f <= tmp[i+InnerCount];
        }

        return output;
    }

    public void Randomize()
    {
        for (int i = 0; i < _connectionCount; i++)
        {
            (int src, int dst, float wht, bool) t = new (   
                RandomNumberGenerator.Next(InputCount + InnerCount),
                RandomNumberGenerator.Next(InnerCount + OutputCount),
                RandomNumberGenerator.NextSingle()*8-4,
                true);
            if (_connections.Count == 0 || _connections[_connections.Count-1].src < t.src)
                _connections.Add(t);
            else
            {
                for (int j = 0; j < _connections.Count; j++)
                {
                    if (_connections[j].src >= t.src)
                    {
                        _connections.Insert(j, t);
                        break;
                    }
                }
            }
        }

        CleanModel();
    }

    public void Mutate()
    {
        if (RandomNumberGenerator.NextSingle() < MutateChance)
        {
            int changeAmount = (int)(_connections.Count * MutateStrength);

            for (int i = 0; i < changeAmount; i++)
            {
                int index = RandomNumberGenerator.Next(_connections.Count);
                var change = _connections[index];
                _connections.RemoveAt(index);
                
                if (RandomNumberGenerator.NextSingle() < MutateStrength)
                {
                    change.src = RandomNumberGenerator.Next(InputCount+InnerCount);
                    if (change.src > _connections[_connections.Count-1].src)
                    {
                        _connections.Add(change);
                    }
                    else
                    {
                        for (int j = 0; j < _connections.Count; j++)
                        {
                            if (change.src <= _connections[j].src)
                            {
                                index = j;
                                break;
                            }
                        }
                    }
                }
                if (RandomNumberGenerator.NextSingle() < MutateStrength)
                {
                    change.dst = RandomNumberGenerator.Next(InnerCount+OutputCount);
                }
                if (RandomNumberGenerator.NextSingle() < MutateStrength)
                {
                    change.wht = RandomNumberGenerator.NextSingle()*8-4;
                }

                change.act = true;

                _connections.Insert(index, change);
            }
        }

        CleanModel();
    }

    private void CleanModel()
    {
        bool Rec(int src)
        {
            
            return false;
        }

        // Disable all connections that lead to no output
        for (int i = 0; i < ConnectionCount; i++)
        {
            if (_connections[i].dst <= InputCount+InnerCount)
            {
                var elem = _connections[i];
                elem.act = Rec(_connections[i].dst);
                _connections[i] = elem;
            }
        }
    }
}
