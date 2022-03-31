using Xunit;
using BioSim;
using System;
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
    public void TestWholeSim()
    {
        const string path = "../tmp/secondsim.png";
        if (!File.Exists(path))
        {
            return;
        }

        MagickImage image = new MagickImage(path);

        Model model = new Model();
        model.InnerCount = 2;
        Map simMap = new Map(path);

        Simulation simulation = new Simulation(model, new InputFunction[0], new OutputFunction[0], simMap);
        simulation.InitialPopulation = 200;
        simulation.Generations = 20;
        simulation.Steps = 10;
        simulation.MinBirthAmount = 0;
        simulation.MaxBirthAmount = 2;
        simulation.RandomNumberGenerator = new Random(0);

        simulation.Setup();

        MagickImageCollection collection = new MagickImageCollection();
        MagickReadSettings mrs = new MagickReadSettings();
        mrs.Format = MagickFormat.Rgb;
        mrs.Width = image.Width;
        mrs.Height = image.Height;

        int counter = 0;
        while (simulation.Update())
        {
            collection.Add(new MagickImage(simulation.SimEnv.ReadData(), mrs));
            collection[counter].AnimationDelay = 20;
            counter++;
        }

        collection.Write("../tmp/firstsim.gif");
    }
}
