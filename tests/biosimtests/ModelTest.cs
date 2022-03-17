using Xunit;
using BioSim;

namespace biosimtests;

public class ModelTest
{
    [Fact]
    public void TestConstuct()
    {
        Model model = new Model(new SimulationSettings());
    }

    [Fact]
    public void TestRandom()
    {
        int innerCount = 10;
        int inputCount = 5;
        int outputCount = 7;
        int connectionsCount = 20;
        SimulationSettings settings = new SimulationSettings()
        {
            inputFunctions = new InputFunction[inputCount],
            innerNeurons = innerCount,
            outputFunctions = new OutputFunction[outputCount],
            connections = connectionsCount,
            mutateChance = 1,
            mutateStrength = 0.5f
        };
        Model model = new Model(settings);
        model.Randomize();

        var connections = model.Connections;

        Assert.Equal(connections.Length, connectionsCount);

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
