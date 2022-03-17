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
        SimulationSettings settings = new SimulationSettings();
        byte[] data = {0, 0, 0};
        TestDirHelper.CreateTestImage(data, 1, _mapImagePath);
        settings.map = new Map(_mapImagePath);
        Simulation simulation = new Simulation(settings);
    }

    [Fact]
    public void TestUpdate()
    {
        SimulationSettings settings = new SimulationSettings()
        {
            steps = 10,
            generations = 5,
            inputFunctions = new InputFunction[0],
            outputFunctions = new OutputFunction[0]
        };

        byte[] data = {0, 0, 0};
        TestDirHelper.CreateTestImage(data, 1, _mapImagePath);

        settings.map = new Map(_mapImagePath);

        Simulation simulation = new Simulation(settings);

        for (int i = 0; i < 10*5; i++)
        {
            Assert.True(simulation.Update());
        }

        Assert.False(simulation.Update());
    }

    private void CreateTestMapImage()
    {
        string? currentDir = Path.GetDirectoryName(_mapImagePath);
        if (currentDir == null)
        {
            throw new Exception("Directory must not be null");
        }
        if (!Directory.Exists(currentDir))
        {
            Directory.CreateDirectory(currentDir);
        }
    }
}
