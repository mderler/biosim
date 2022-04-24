namespace BioSim;

public class SLLModel : Model
{
    public int InputCount { get; set; }
    public int InnerCount { get; set; }
    public int OutputCount { get; set; }
    private int _connectionCount;
    private List<(int src, int dst, float wht, bool act)> _connections;

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
                    _connections.RemoveAt(RNG.Next(_connections.Count));
                }
            }

            _connectionCount = value;
        }
    }

    public SLLModel(float mutateChance, float mutateStrength, Random rng) :
        base(mutateChance, mutateStrength, rng)
    {
        _connectionCount = 0;
        _connections = new List<(int, int, float, bool)>();
    }

    public SLLModel(float mutateChance, float mutateStrength) :
        base(mutateChance, mutateStrength)
    {
        _connectionCount = 0;
        _connections = new List<(int, int, float, bool)>();
    }

    public SLLModel()
    {
        _connectionCount = 0;
        _connections = new List<(int, int, float, bool)>();
    }

    override public bool[] GetOutput(float[] input)
    {
        bool[] output = new bool[OutputCount];
        float[] tmp = new float[InnerCount+OutputCount];

        for (int i = 0; i < _connections.Count; i++)
        {
            if (_connections[i].act)
            {
                if (_connections[i].src >= InputCount)
                    tmp[_connections[i].dst-InputCount] += MathF.Tanh(tmp[_connections[i].src-InputCount] * _connections[i].wht);
                else
                    tmp[_connections[i].dst-InputCount] += input[_connections[i].src] * _connections[i].wht;
            }
        }

        for (int i = 0; i < OutputCount; i++)
        {
            output[i] = 0f <= tmp[i+InnerCount];
        }

        return output;
    }

    override public void Randomize()
    {
        for (int i = 0; i < _connectionCount; i++)
        {
            (int src, int dst, float wht, bool) t = new (   
                RNG.Next(InputCount + InnerCount),
                RNG.Next(InputCount, InputCount+InnerCount+OutputCount),
                RNG.NextSingle()*8-4,
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

    override public void Mutate()
    {
        if (RNG.NextSingle() < MutateChance)
        {
            int changeAmount = (int)(_connections.Count * MutateStrength);

            for (int i = 0; i < changeAmount; i++)
            {
                int index = RNG.Next(_connections.Count);
                var change = _connections[index];
                _connections.RemoveAt(index);
                
                if (RNG.NextSingle() < MutateStrength)
                {
                    change.src = RNG.Next(InputCount+InnerCount);
                    if (change.src > _connections[_connections.Count-1].src)
                    {
                        index = _connections.Count;
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
                if (RNG.NextSingle() < MutateStrength)
                {
                    change.dst = RNG.Next(InputCount, InputCount+InnerCount+OutputCount);
                }
                if (RNG.NextSingle() < MutateStrength)
                {
                    change.wht = RNG.NextSingle()*8-4;
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

    public SLLModel Copy()
    {
        SLLModel model = new SLLModel(this.MutateChance, this.MutateStrength, this.RNG)
        {
            Connections = this.Connections,
            InputCount = this.InputCount,
            InnerCount = this.InnerCount,
            OutputCount = this.OutputCount
        };

        model.ConnectionCount = this._connectionCount;

        return model;
    }
}
