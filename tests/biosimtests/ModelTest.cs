using Xunit;
using BioSim;
using System;

namespace biosimtests;

public class ModelTest
{
    [Fact]
    public void TestConstuct()
    {
        Model model = new Model();
    }

    [Fact]
    public void TestRandom()
    {
        int innerCount = 40;
        int inputCount = 30;
        int outputCount = 20;
        int connectionsCount = 500;

        Model model = new Model()
        {
            InnerCount = innerCount,
            InputCount = inputCount,
            OutputCount = outputCount,
            ConnectionCount = connectionsCount
        };
        model.Randomize();

        var connections = model.Connections;

        Assert.Equal(connections.Length, connectionsCount);
        Assert.Equal(connectionsCount, model.ConnectionCount);

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

    [Fact]
    public void TestMutate()
    {
        int innerCount = 3;
        int inputCount = 2;
        int outputCount = 4;
        int connectionsCount = 6;
        float mutateChance = 1f;
        float mutateStrength = 1f;

        Model model = new Model()
        {
            InnerCount = innerCount,
            InputCount = inputCount,
            OutputCount = outputCount,
            ConnectionCount = connectionsCount,
            MutateChance = mutateChance,
            MutateStrength = mutateStrength,
            RandomNumberGenerator = new Random(0)
        };

        model.Randomize();
        var oldConnections = model.Connections;

        model.Mutate();

        Assert.Equal(model.ConnectionCount, connectionsCount);
        Assert.Equal(model.Connections.Length, connectionsCount);
    }

    [Fact]
    public void TestOutput()
    {
        // TODO: Finish this test
        Model model = new Model()
        {
            InputCount = 3,
            InnerCount = 2,
            OutputCount = 5,
            ConnectionCount = 10,
            RandomNumberGenerator = new Random(0)
        };

        model.Randomize();

        var connections = model.Connections;
        float[] input = {0.215f, 0.999f, 0.001f};

        bool[] expected = {};
    }

    public void TestCleanModel()
    {
        // TODO: Finish this test
    }
}
