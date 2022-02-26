struct NeuralNetworkModel
{
    public float[] InputNeurons { get; }
    public float[] InnerNeurons { get; }
    public float[] OuterNeurons { get; }

    private List<(int, int, float)> _connections;
    public NeuralNetworkModel(int inputAmount, int innerAmount, int outerAmount)
    {
        InputNeurons = new float[inputAmount];
        InnerNeurons = new float[innerAmount];
        OuterNeurons = new float[outerAmount];
        _connections = new List<(int, int, float)>();
    }

    public int Count => InputNeurons.Length+
                        InnerNeurons.Length+
                        OuterNeurons.Length;

    public float this[int index]
    {
        get 
        { 
            if (index < InputNeurons.Length)
                return InputNeurons[index];
            else if (index < InnerNeurons.Length+InputNeurons.Length)
                return InnerNeurons[index-InputNeurons.Length];
            else
                return OuterNeurons[index-InnerNeurons.Length-InnerNeurons.Length];
        }
        set {  }
    }
}