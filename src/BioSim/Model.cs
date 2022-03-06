public class Model
{
    private int _inputCount;
    private int _innerCount;
    private int _outputCount;

    private (int src, int dst, float wht)[] _connections;
    private float _mutateChance;

    public Model(int inputCount, int innerCount, int outputCount, int connections, float mutateChance)
    {
        _inputCount = inputCount;
        _innerCount = innerCount;
        _outputCount = outputCount;
        _mutateChance = mutateChance;
        _connections = new (int, int, float)[connections];
    }

    public bool[] GetOutput(float[] input)
    {
        bool[] output = new bool[0];
        float[] tmp = new float[_innerCount+_outputCount];

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
            output[i] = rnd.NextSingle() <= tmp[i+_innerCount];
        }

        return output;
    }

    public void Randomize()
    {
        Random rnd = new Random();

        for (int i = 0; i < _connections.Length; i++)
        {
            _connections[i].src = rnd.Next(_inputCount+_innerCount);
            _connections[i].dst = rnd.Next(_innerCount+_outputCount);
            _connections[i].wht = rnd.NextSingle()*8-4;
        }
    }
}