using Xunit;
using BioSim;
using System;

namespace biosimtests;

public class ModelTest
{
    [Fact]
    public void TestConstuct()
    {
        SLLModel model = new SLLModel();
    }

    [Fact]
    public void TestRandom()
    {
        int innerCount = 40;
        int inputCount = 30;
        int outputCount = 20;
        int connectionsCount = 500;

        SLLModel model = new SLLModel()
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

        for (int i = 0; i < connections.Length-1; i++)
        {
            Assert.True(connections[i].src <= connections[i+1].src);
        }

        foreach (var item in connections)
        {
            Assert.True(item.wht >= -4f && item.wht <= 4f);
            Assert.True(item.src < inputCount+innerCount);
            Assert.True(item.dst < inputCount+innerCount+outputCount);
            Assert.True(item.dst >= inputCount);
        }
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

        SLLModel model = new SLLModel()
        {
            InnerCount = innerCount,
            InputCount = inputCount,
            OutputCount = outputCount,
            ConnectionCount = connectionsCount,
            MutateChance = mutateChance,
            MutateStrength = mutateStrength,
            RNG = new Random(0)
        };

        model.Randomize();
        var oldConnections = model.Connections;

        model.Mutate();

        Assert.Equal(model.ConnectionCount, connectionsCount);
        Assert.Equal(model.Connections.Length, connectionsCount);

        var connections = model.Connections;

        Assert.Equal(connections.Length, connectionsCount);
        Assert.Equal(connectionsCount, model.ConnectionCount);

        for (int i = 0; i < connections.Length-1; i++)
        {
            Assert.True(connections[i].src <= connections[i+1].src);
        }

        foreach (var item in connections)
        {
            Assert.True(item.wht >= -4f && item.wht <= 4f);
            Assert.True(item.src < inputCount+innerCount);
            Assert.True(item.dst < inputCount+innerCount+outputCount);
            Assert.True(item.dst > inputCount);
        }
    }

    [Fact]
    public void TestCleanModel()
    {
        (int, int, float, bool)[] connections = {
            (0, 12, 1f, false),
            (1, 16, 1f, false),
            (1, 18, 1f, false),
            (4, 18, 1f, false),
            (8, 22, 1f, false),
            (9, 26, 1f, false),
            (10, 27, 1f, false),
            (11, 38, 1f, false),
            (13, 28, 1f, false),
            (14, 29, 1f, false),
            (14, 30, 1f, false),
            (15, 29, 1f, false),
            (16, 17, 1f, false),
            (17, 18, 1f, false),
            (19, 19, 1f, false),
            (20, 22, 1f, false),
            (20, 21, 1f, false),
            (21, 22, 1f, false),
            (22, 23, 1f, false),
            (23, 23, 1f, false),
            (23, 35, 1f, false),
            (23, 24, 1f, false),
            (24, 25, 1f, false),
            (25, 36, 1f, false),
            (25, 26, 1f, false),
            (26, 36, 1f, false),
            (27, 37, 1f, false)
        };

        bool[] expected = {
            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true
        }; 

        SLLModel model = new SLLModel();
        model.InputCount = 12;
        model.InnerCount = 16;
        model.OutputCount = 11;
        model.Connections = connections;

        for (int i = 0; i < model.ConnectionCount; i++)
        {
            Assert.Equal(model.Connections[i].act, expected[i]);
        }
    }
}
