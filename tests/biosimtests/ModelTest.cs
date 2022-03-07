using Xunit;
using BioSim;

namespace biosimtests;

public class ModelTest
{
    [Fact]
    public void TestConstuct()
    {
        Model model = new Model(3, 2, 3, 18, 1);
    }

    [Fact]
    public void TestRandom()
    {
        int inputCount = 3;
        int innerCount = 2;
        int outputCount = 3;
        int con = 18;
        Model model = new Model(inputCount, innerCount, outputCount, con, 1);
        model.Randomize();

        var connections = model.Connections;

        bool right = true;
        for (int i = 0; i < connections.Length-1; i++)
        {
            right &= connections[i].src <= connections[i+1].src;
        }

        Assert.True(right);

        foreach (var item in connections)
        {
            right &= item.wht >= -4f && item.wht <= 4f;
            right &= item.src < inputCount+innerCount;
            right &= item.dst < innerCount+outputCount;
        }

        Assert.True(right);
    }
}
