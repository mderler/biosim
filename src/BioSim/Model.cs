namespace BioSim;

public class Model
{
    public int InputCount { get; set; }
    public int InnerCount { get; set; }
    public int OutputCount { get; set; }
    private int _connectionCount;
    public float MutateChance { get; set; }
    public float MutateStrength { get; set; }
    private List<(int src, int dst, float wht, bool act)> _connections;
    public Random RandomNumberGenerator { get; set; }
    public (int src, int dst, float wht, bool act)[] Connections
    {
        get { return _connections.ToArray(); }
        set 
        {
            _connections = new List<(int, int, float, bool)>(value);
            _connectionCount = value.Length;
            CleanModel();
        }
    }
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
            if (_connections[i].act)
            {
                if (_connections[i].src > InputCount)
                    tmp[_connections[i].dst] += MathF.Tanh(tmp[_connections[i].src-InputCount] * _connections[i].wht);
                else
                    tmp[_connections[i].dst] += tmp[_connections[i].src] * _connections[i].wht;
            }
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
                RandomNumberGenerator.Next(InputCount, InputCount+InnerCount+OutputCount),
                RandomNumberGenerator.NextSingle()*8-4,
                false);
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
                    change.dst = RandomNumberGenerator.Next(InputCount, InputCount+InnerCount+OutputCount);
                }
                if (RandomNumberGenerator.NextSingle() < MutateStrength)
                {
                    change.wht = RandomNumberGenerator.NextSingle()*8-4;
                }

                change.act = false;

                _connections.Insert(index, change);
            }
        }

        CleanModel();
    }

    // disable all connections that lead to nowhere
    private void CleanModel()
    {
        List<int> inCon = new List<int>();
        List<int> outCon = new List<int>();

        List<int> intmp = new List<int>();
        List<int> outtmp = new List<int>();
        
        bool changed;
        do
        {
            changed = false;
            for (int i = 0; i < _connectionCount; i++)
            {
                // check and add the connections that are directly or indirectly
                // connected to the input
                if (_connections[i].src < InputCount ||
                    intmp.Contains(_connections[i].src))
                {
                    if (!inCon.Contains(i))
                    {
                        inCon.Add(i);
                        changed = true;
                    }
                    if (!intmp.Contains(_connections[i].dst))
                    {
                        intmp.Add(_connections[i].dst);
                        changed = true;
                    }
                }

                // check and add the connections that are directly or indirectly
                // connected to the output
                if (_connections[i].dst >= InputCount+InnerCount ||
                    outtmp.Contains(_connections[i].dst))
                {
                    if (!outCon.Contains(i))
                    {
                        outCon.Add(i);
                        changed = true;
                    }
                    if (!outtmp.Contains(_connections[i].src))
                    {
                        outtmp.Add(_connections[i].src);
                        changed = true;
                    }
                }
            }
        } while (changed);

        // activate all connections that are connected to input and output
        foreach (var index in inCon)
        {
            if (outCon.Contains(index))
            {
                var t = _connections[index];
                t.act = true;
                _connections[index] = t;
            }
        }
    }
}
