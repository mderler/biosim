using Xunit;
using BioSim;

namespace biosimtests;

public class SimulationTest
{
    private const string _mapImagePath = "../tmp/mapImgage.png";

    [Fact]
    public void TestConstuct()
    {
        Simulation simulation = new Simulation
        (
            new Model(),
            new InputFunction[0],
            new OutputFunction[0],
            new Map(_mapImagePath)
        );
    }

    [Fact]
    public void TestUpdate()
    {
        byte[] data = {0, 0, 0};
        TestDirHelper.CreateTestImage(data, 1, _mapImagePath);

        const int generations = 50;
        const int steps = 20;

        Simulation simulation = new Simulation
        (
            new Model(),
            new InputFunction[0],
            new OutputFunction[0],
            new Map(_mapImagePath)
        )
        {
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
