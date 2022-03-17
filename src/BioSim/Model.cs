namespace BioSim;

public class Model
{
    private SimulationSettings _settings;
    private int _inputCount;
    private int _outputCount;
    private (int src, int dst, float wht)[] _connections;
    public (int src, int dst, float wht)[] Connections => _connections;

    public Model(SimulationSettings settings)
    {
        _settings = settings;
        _inputCount = settings.inputFunctions.Length;
        _connections = new (int, int, float)[_settings.connections];
    }

    public bool[] GetOutput(float[] input)
    {
        bool[] output = new bool[0];
        float[] tmp = new float[_settings.innerNeurons+_inputCount];

        for (int i = 0; i < _connections.Length; i++)
        {
            if (_connections[i].src > _inputCount)
                tmp[_connections[i].dst] += MathF.Tanh(tmp[_connections[i].src-_inputCount] * _connections[i].wht);
            else
                tmp[_connections[i].dst] += tmp[_connections[i].src] * _connections[i].wht;
        }

        Random rnd = new Random();
        for (int i = 0; i < _outputCount; i++)
        {
            output[i] = rnd.NextSingle() <= tmp[i+_settings.innerNeurons];
        }

        return output;
    }

    public void Randomize()
    {
        List<(int src, int dst, float wht)> tmp = new List<(int, int, float)>();
        Random rnd = new Random();

        for (int i = 0; i < _connections.Length; i++)
        {
            (int src, int dst, float wht) t = new (   
                rnd.Next(_inputCount+_settings.innerNeurons),
                rnd.Next(_settings.innerNeurons+_outputCount),
                rnd.NextSingle()*8-4);
            if (tmp.Count == 0 || tmp[tmp.Count-1].src < t.src)
                tmp.Add(t);
            else
            {
                for (int j = 0; j < tmp.Count; j++)
                {
                    if (tmp[j].src >= t.src)
                    {
                        tmp.Insert(j, t);
                        break;
                    }
                }
            }
        }

        _connections = tmp.ToArray();
    }

    public void Mutate()
    {
        Random rnd = new Random();

        if (rnd.NextSingle() < _settings.mutateChance)
        {
            List<(int src, int dst, float wht)> tmp = new List<(int, int, float)>(_connections);

            int changeAmount = (int)(_connections.Length * _settings.mutateStrength);
            byte[] pickedFields = new byte[3];

            for (int i = 0; i < changeAmount; i++)
            {
                rnd.NextBytes(pickedFields);
                int index = rnd.Next(_connections.Length);
                var change = tmp[index];
                tmp.RemoveAt(index);
                
                if (pickedFields[0] % 2 == 0)
                {
                    change.src = rnd.Next(_inputCount+_settings.innerNeurons);
                    if (change.src > tmp[tmp.Count-1].src)
                    {
                        tmp.Add(change);
                    }
                    else
                    {
                        for (int j = 0; j < tmp.Count; j++)
                        {
                            if (change.src <= tmp[j].src)
                            {
                                index = j;
                                break;
                            }
                        }
                    }
                }
                if (pickedFields[1] % 2 == 0)
                {
                    change.dst = rnd.Next(_settings.innerNeurons+_outputCount);
                }
                if (pickedFields[2] % 2 == 0)
                {
                    change.wht = rnd.NextSingle()*8-4;
                }

                tmp.Insert(index, change);
            }

            _connections = tmp.ToArray();
        }
    }
}