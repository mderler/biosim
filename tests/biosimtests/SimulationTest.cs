using Xunit;
using BioSim;

namespace biosimtests;

public class SimulationTest
{

    [Fact]
    public void TestConstuct()
    {
        SimulationSettings settings = new SimulationSettings();
        settings.map = new Map("");
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

        Simulation simulation = new Simulation(settings);

        for (int i = 0; i < 10*5; i++)
        {
            Assert.True(simulation.Update());
        }

        Assert.False(simulation.Update());
    }
}
