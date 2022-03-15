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
    }

    public void Mutate()
    {
        Random rnd = new Random();

        if (rnd.NextSingle() < _settings.mutateChance)
        {

        }
    }
}