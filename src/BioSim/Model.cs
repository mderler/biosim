public class Model
{
    private float[] _input;
    private float[] _inner;
    private float[] _output;

    public Model(int inputCount, int innerCount, int outputCount)
    {
        _input = new float[inputCount];
        _inner = new float[innerCount];
        _output = new float[outputCount];
    }
}