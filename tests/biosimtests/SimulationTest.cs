using Xunit;
using BioSim;
using System.IO;
using System;

namespace biosimtests;

public class SimulationTest
{
    private const string _mapImagePath = "../tmp/mapImgage.png";

    [Fact]
    public void TestConstuct()
    {
        Simulation simulation = new Simulation();
    }

    [Fact]
    public void TestUpdate()
    {
        byte[] data = {0, 0, 0};
        TestDirHelper.CreateTestImage(data, 1, _mapImagePath);

        const int generations = 50;
        const int steps = 20;

        Simulation simulation = new Simulation()
        {
            SimMap = new Map(_mapImagePath),
            InputFunctions = new InputFunction[0],
            OutputFunctions = new OutputFunction[0],
            Generations = generations,
            Steps = steps
        };

        for (int i = 0; i < generations*steps; i++)
        {
            Assert.True(simulation.Update());
        }

        Assert.False(simulation.Update());
    }
}
