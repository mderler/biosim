class NeuralNetworkModel
{
    private float[] _inputNeurons;
    private float[] _innerNeurons;
    private float[] _outerNeurons;

    private List<(int src, int rcv, float wgt)> _connections;
    public NeuralNetworkModel(int inputAmount, int innerAmount, int outerAmount)
    {
        _inputNeurons = new float[inputAmount];
        _innerNeurons = new float[innerAmount];
        _outerNeurons = new float[outerAmount];
        _connections = new List<(int, int, float)>();
    }

    public int Count => _inputNeurons.Length+
                        _innerNeurons.Length+
                        _outerNeurons.Length;

    private float this[int index]
    {
        get 
        { 
            if (index < _inputNeurons.Length)
                return _inputNeurons[index];
            else if (index < _innerNeurons.Length+_inputNeurons.Length)
                return _innerNeurons[index-_inputNeurons.Length];
            return _outerNeurons[index-_innerNeurons.Length-_innerNeurons.Length];
        }
        set
        {
            if (index < _inputNeurons.Length)
                _inputNeurons[index] = value;
            else if (index < _innerNeurons.Length+_inputNeurons.Length)
                _innerNeurons[index-_inputNeurons.Length] = value;
            else
                _outerNeurons[index-_innerNeurons.Length-_innerNeurons.Length] = value;
        }
    }

    public void RandomMutate(float chance)
    {
        
    }

    public int[] Compute(float[] inputSignals)
    {
        if (inputSignals.Length != _inputNeurons.Length)
            throw new Exception($"{nameof(inputSignals)} must have the same amount of Neurons");

        for (int i = 0; i < _connections.Count; i++)
        {
            this[_connections[i].rcv] += _connections[i].wgt*this[_connections[i].src];
            if (i != _connections.Count-1)
            {
                if (_connections[i].rcv != _connections[i+1].rcv)
                    this[_connections[i].rcv] = MathF.Tanh(this[_connections[i].rcv]); 
            }
        }

        int[] indexes = new int[_outerNeurons.Length];
        int count = 0;
        Random rnd = new Random();
        for (int i = 0; i < _outerNeurons.Length; i++)
        {
            if (_outerNeurons[i] > rnd.NextSingle())
            {
                indexes[count] = i;
                count++;
            }
        }
        Array.Resize(ref indexes, count+1);
        return indexes;
    }
}