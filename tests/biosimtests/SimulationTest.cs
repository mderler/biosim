using Xunit;
using BioSim;
using System;
using System.IO;
using ImageMagick;

namespace biosimtests;

public class SimulationTest
{
    private const string _mapImagefn = "mapImgage.png";

    [Fact]
    public void TestConstuct()
    {
        Simulation simulation = new Simulation
        (
            new SLLModel(),
            new InputFunction[0],
            new OutputFunction[0],
            new Map(_mapImagefn)
        );
    }

    [Fact]
    public void TestUpdate()
    {
        byte[] data = { 0, 0, 0 };
        TestDirHelper.CreateTestImage(data, 1, _mapImagefn);

        const int generations = 50;
        const int steps = 20;

        Simulation simulation = new Simulation
        (
            new SLLModel(),
            new InputFunction[0],
            new OutputFunction[0],
            new Map(_mapImagefn)
        )
        {
            Generations = generations,
            Steps = steps
        };

        for (int i = 0; i < generations * steps; i++)
        {
            Assert.True(simulation.Update());
        }

        Assert.False(simulation.Update());
    }

    [Fact]
    public void TestWholeSim()
    {
        const string path = "../../../../../res/testres/corner.png";
        if (!File.Exists(path))
        {
            return;
        }

        MagickImage image = new MagickImage(path);

        const int connectionCount = 2;

        SLLModel model = new SLLModel();
        model.InputCount = 2;
        model.InnerCount = 5;
        model.OutputCount = 4;
        model.ConnectionCount = connectionCount;
        model.MutateChance = 0f;
        model.MutateStrength = 0.2f;
        Map simMap = new Map(path);

        InputFunction[] inputFunctions = { InputFunctions.NearToEast, InputFunctions.NearToSouth };
        OutputFunction[] outputFunctions = {OutputFunctions.MoveNorth, OutputFunctions.MoveSouth,
                                            OutputFunctions.MoveWest, OutputFunctions.MoveEast};

        Simulation simulation = new Simulation(model, inputFunctions, outputFunctions, simMap);
        simulation.InitialPopulation = 1000;
        simulation.Generations = 50;
        simulation.Steps = 260;
        simulation.MinBirthAmount = 2;
        simulation.MaxBirthAmount = 10;
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
            collection[counter].AnimationDelay = 10;
            counter++;
        }

        collection.Write("../../../../../tmp/firstsim.gif", MagickFormat.Gif);
    }
}
