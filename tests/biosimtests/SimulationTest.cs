using Xunit;
using BioSim;
using System.IO;
using ImageMagick;

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

    [Fact]
    public void TestFirstSim()
    {
        const string path = "C:/Users/Matthias/Desktop/BioSimMaps/simImg1Test.png";
        if (!Directory.Exists(path))
        {
            //return;
        }

        Model model = new Model();
        model.InnerCount = 2;
        Map simMap = new Map(path);

        Simulation simulation = new Simulation(model, new InputFunction[0], new OutputFunction[0], simMap);
        simulation.InitialPopulation = 50;
        simulation.Generations = 10;
        simulation.Steps = 20;

        simulation.Setup();

        MagickImageCollection collection = new MagickImageCollection();
        int counter = 0;
        while (simulation.Update())
        {
            collection.Add(new MagickImage(simulation.SimEnv.ReadData()));
            collection[counter].AnimationDelay = 100;
            counter++;
        }
        collection.Write("../tmp/firstSim.gif");
    }
}
